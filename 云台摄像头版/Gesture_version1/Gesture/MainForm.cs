// Two Cameras Vision
//
// Copyright © Andrew Kirillov, 2009
// andrew.kirillov@aforgenet.com
//

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Windows.Forms;
using System.Threading;

using AForge;
using AForge.Imaging;
using AForge.Imaging.Filters;
using AForge.Video;
using AForge.Video.DirectShow;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.IO;
using System.Net;

namespace Gesture
{
    public partial class MainForm : Form
    {
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);
        string FileName = System.AppDomain.CurrentDomain.BaseDirectory + "Config.ini";
        StringBuilder temp = new StringBuilder(255);
        // list of video devices
        FilterInfoCollection videoDevices;
        // form to tune object detection filter
        TuneObjectFilterForm tuneObjectFilterForm;

        ColorFiltering colorFilter = new ColorFiltering( );
        GrayscaleBT709 grayFilter  = new GrayscaleBT709( );
        // use two blob counters, so the could run in parallel in two threads
        BlobCounter blobCounter1 = new BlobCounter( );

        private AutoResetEvent camera1Acquired = null;
        private Thread trackingThread = null;

        private Bitmap nowImg;

        // object coordinates in both cameras
        private float x1, y1, x2, y2;
        float targetX = 0;
        float targetY = 0;
        float targetX_temp = 0;
        float targetY_temp = 0;
        int x_temp,y_temp,x_position=90,y_position=45;
        

        IPAddress ip_input;
        int port, change_enable = 0, x_enable = 1, y_enable = 0;
        string str_camera;
        private static byte[] result = new byte[1024];
        byte[] sendMessage_x = new byte[] { 0xff, 0x01, 0x02, 0x5a, 0xff };
        byte[] sendMessage_y = new byte[] { 0xff, 0x01, 0x01, 0x2d, 0xff };

        Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
      
        public MainForm( )
        {
            InitializeComponent( );

            //
            colorFilter.Red = new IntRange( 0, 100 );
            colorFilter.Green = new IntRange( 0, 200 );
            colorFilter.Blue = new IntRange( 150, 255 );

            // configure blob counters
            blobCounter1.MinWidth = 10;
            blobCounter1.MinHeight = 10;
            blobCounter1.FilterBlobs = true;
            blobCounter1.ObjectsOrder = ObjectsOrder.Size;

        }

        // Main form closing - stop cameras
        private void MainForm_FormClosing( object sender, FormClosingEventArgs e )
        {
            StopCameras( );
        }


        // Stop cameras
        private void StopCameras( )
        {
            videoSourcePlayer1.SignalToStop( );

            videoSourcePlayer1.WaitForStop( );

            UpdateObjectPicture(null);

            if ( trackingThread != null )
            {
                // signal tracking thread to stop
                x1 = y1 = x2 = y2 = -1;
                camera1Acquired.Set( );
                trackingThread.Join( );
            }
        }

        // On "Tune Object Filter" button click - show filter tuning dialog
        private void tuneObjectFilterButton_Click( object sender, EventArgs e )
        {
            if ( tuneObjectFilterForm == null )
            {
                tuneObjectFilterForm = new TuneObjectFilterForm( );
                tuneObjectFilterForm.OnFilterUpdate += new EventHandler( tuneObjectFilterForm_OnFilterUpdate );

                tuneObjectFilterForm.RedRange   = colorFilter.Red;
                tuneObjectFilterForm.GreenRange = colorFilter.Green;
                tuneObjectFilterForm.BlueRange  = colorFilter.Blue;
            }
            tuneObjectFilterForm.Show( );
        }

        // Object filter properties are updated
        private void tuneObjectFilterForm_OnFilterUpdate( object sender, EventArgs eventArgs )
        {
            colorFilter.Red   = tuneObjectFilterForm.RedRange;
            colorFilter.Green = tuneObjectFilterForm.GreenRange;
            colorFilter.Blue  = tuneObjectFilterForm.BlueRange;
        }

        // Turn on/off object detection
        private void objectDetectionCheck_CheckedChanged( object sender, EventArgs e )
        {
            if ( ( !objectDetectionCheck.Checked ))
            {
                UpdateObjectPicture(null );
            }
        }

        // received frame from the 1st camera
        private void videoSourcePlayer1_NewFrame( object sender, ref Bitmap image )
        {
            nowImg = (Bitmap)image.Clone();
            if ( objectDetectionCheck.Checked )
            {
                Bitmap objectImage = colorFilter.Apply( image );

                // lock image for further processing
                BitmapData objectData = objectImage.LockBits( new Rectangle( 0, 0, image.Width, image.Height ),
                    ImageLockMode.ReadOnly, image.PixelFormat );

                // grayscaling
                UnmanagedImage grayImage = grayFilter.Apply( new UnmanagedImage( objectData ) );

                // unlock image
                objectImage.UnlockBits( objectData );

                // locate blobs 
                blobCounter1.ProcessImage( grayImage );
                Rectangle[] rects = blobCounter1.GetObjectsRectangles( );

                if ( rects.Length > 0 )
                {
                    Rectangle objectRect = rects[0];

                    // draw rectangle around derected object
                    Graphics g = Graphics.FromImage( image );

                    using ( Pen pen = new Pen( Color.FromArgb( 160, 255, 160 ), 3 ) )
                    {
                        g.DrawRectangle( pen, objectRect );
                    }

                    g.Dispose( );

                    // get object's center coordinates relative to image center
                    lock ( this )
                    {
                        x1 = ( objectRect.Left + objectRect.Right - objectImage.Width ) / 2;
                        y1 = ( objectImage.Height - ( objectRect.Top + objectRect.Bottom ) ) / 2;
                        // map to [-1, 1] range
                        //x1 /= ( objectImage.Width / 2 );
                        //y1 /= (objectImage.Height / 2);
                        x1 /= ( objectImage.Width / 2 );//修改后
                        y1 /= ( objectImage.Height / 2 );//修改后

                        try
                        {
                            camera1Acquired.Set();
                        }
                        catch
                        { }
                        
                    }
                }

                UpdateObjectPicture(objectImage );
            }
        }

        // Thread to track object
        private void TrackingThread( )
        {
          //  float targetX = 0;
          //  float targetY = 0;

            while ( true )
            {
                camera1Acquired.WaitOne( );
                lock ( this )
                {
                    // stop the thread if it was signaled
                    if ( ( x1 == -1 ) && ( y1 == -1 ) && ( x2 == -1 ) && ( y2 == -1 ) )
                    {
                        break;
                    }

                    // get middle point
                    targetX = ( x1 + x2 ) / 2;
                    targetY = ( y1 + y2 ) / 2;
                    
                }

            }
        }

        /// <summary>
        /// 选择绿色的物体为识别的对象
        /// </summary>
        private void videoSourcePlayer1_MouseUp(object sender, MouseEventArgs e)
        {
            //Color[] pixelColor = new Color[20];
            //int redMax = 0, blueMax = 0, greenMin = 0;
            //for (int i = -10, ii = 0; i < 10; i++, ii++)
            //{
            //    int x = e.X + i;
            //    int y = e.Y + i;

            //    pixelColor[ii] = nowImg.GetPixel(x, y);
            //    redMax = Math.Max(redMax, pixelColor[ii].R);
            //    blueMax = Math.Max(blueMax, pixelColor[ii].B);
            //    greenMin = Math.Max(redMax, blueMax);
            //}
            //colorFilter.Red.Min = 0;
            //colorFilter.Red.Max = redMax;
            //colorFilter.Blue.Min = 0;
            //colorFilter.Blue.Max = blueMax;
            //colorFilter.Green.Min = greenMin;
            //colorFilter.Green.Max = 255;
        }

        // Update object's picture
        public void UpdateObjectPicture(Bitmap picture)
        {
            System.Drawing.Image oldPicture = null;

            oldPicture = pictureBox1.Image;
            pictureBox1.Image = picture;

            if (oldPicture != null)
            {
                oldPicture.Dispose();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label_xvalue.Text = targetX.ToString();
            label_yvalue.Text = targetY.ToString();
            
        }

        private void btn_camera_Click(object sender, EventArgs e)
        {
            if (btn_camera.Text == "打开摄像头")
            {
                btn_camera.Text = "关闭摄像头";
                connectVidio();
                
            }
            else
            {
                btn_camera.Text = "打开摄像头";
                videoSourcePlayer1.Stop();
            }
            
        }
        private void connectVidio()
        {
            MJPEGStream mjpegSource = new MJPEGStream(str_camera);  //这里是从路由发出的视频流的地址，根据实际情况来写
            OpenVideoSource(mjpegSource);

            camera1Acquired = new AutoResetEvent(false);
            trackingThread = new Thread(new ThreadStart(TrackingThread));
            trackingThread.Start();
        }
        private void OpenVideoSource(IVideoSource source)
        {
            videoSourcePlayer1.SignalToStop();
            videoSourcePlayer1.WaitForStop();
            videoSourcePlayer1.VideoSource = source;
            videoSourcePlayer1.Start();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            if (!File.Exists(Environment.CurrentDirectory + "\\Config.ini"))
            {
                FileStream fs = new FileStream(Environment.CurrentDirectory + "\\Config.ini", FileMode.Create, FileAccess.Write);
                fs.Close();
            }
            // string str_ip;
            GetPrivateProfileString("配置", "ip_address", "", temp, 256, FileName);
            // string str_ip="192.168.191.001";
            string str_ip = temp.ToString();
            GetPrivateProfileString("配置", "ip_port", "", temp, 256, FileName);
            string str_port = temp.ToString();
            GetPrivateProfileString("配置", "camera", "", temp, 256, FileName);
            str_camera = temp.ToString();

            

            maskedTextBox1.Text = str_ip;
            maskedTextBox2.Text = str_port;
            textBox_camera.Text = str_camera;
            label5.Text = textBox_camera.Text;
            button_IP_Input_Click(button_IP_Input, new EventArgs());//触发按钮点击事件
            button_connect_Click(button_connect, new EventArgs());
            change_enable = 1;

            clientSocket.Send(sendMessage_x);

            clientSocket.Send(sendMessage_y);
        }

        

        private void button_connect_Click(object sender, EventArgs e)
        {
            try
            {
                clientSocket.Connect(new IPEndPoint(ip_input, port)); //配置服务器IP与端口  
                MessageBox.Show("连接服务器成功", "提示");
            }
            catch (Exception ex)
            {
                //MessageBox.Show("连接服务器失败", "提示");
                MessageBox.Show(ex.ToString(), "提示");
            }
        }

        private void button_IP_Input_Click(object sender, EventArgs e)
        {
            
                try
                {
                    ip_input = IPAddress.Parse(maskedTextBox1.Text);
                    port = Convert.ToInt16(maskedTextBox2.Text);
                    label1.Text = ip_input.ToString() + "  " + maskedTextBox2.Text;
                    WritePrivateProfileString("配置", "ip_address", maskedTextBox1.Text, FileName);
                    WritePrivateProfileString("配置", "ip_port", maskedTextBox2.Text, FileName);
                }
                catch (Exception error)
                {
                    MessageBox.Show("<IP地址输入格式错误> " + error.Message);
                }
            
        }
        private void label_xvalue_TextChanged(object sender, EventArgs e)
        {
            if ((change_enable == 1)&(x_enable==1))
            {
                try
                {
                    if (Math.Abs(targetX) > 0.4)
                    {
                        x_position -= (int)((targetX - 0) * 25);
                    }
                    else if (Math.Abs(targetX) > 0.3)
                    {
                        x_position -= (int)((targetX - 0) * 20);
                    }
                    else if (Math.Abs(targetX) > 0.2)
                    {
                        x_position -= (int)((targetX - 0) * 15);
                    }
                    else
                    {
                        x_position -= (int)((targetX - 0) * 10);
                    }
                    //x_position -= (int)((targetX - targetX_temp) * 34);
                    //targetX_temp = targetX;
                    if (x_position >= 24 && x_position <= 154)
                    {
                        sendMessage_x[1] = 0x01;
                        sendMessage_x[2] = 0x02;
                        sendMessage_x[3] = Convert.ToByte(x_position);
                        clientSocket.Send(sendMessage_x);
                    }
                    else if (x_position < 24)
                    {
                        x_position = 24;
                    }
                    else 
                    {
                        x_position = 154;
                    }

                    //  textBox_message.AppendText("send:");
                    // for (int i = 0; i < 5; i++)
                    //{
                    //    textBox_message.AppendText(Convert.ToString(sendMessage[i], 16));
                    //}
                    //textBox_message.AppendText("\r\n");
                }
                catch
                {
                    //clientSocket.Shutdown(SocketShutdown.Both);
                    //clientSocket.Close();
                }
            }
        }
        private void label_yvalue_TextChanged(object sender, EventArgs e)
        {
            if ((change_enable == 1)&&(y_enable==1))
            {
                try
                {
                    if (Math.Abs(targetY) > 0.4)
                    {
                        y_position += (int)((targetY - 0) * 24);
                    }
                    else if (Math.Abs(targetY) > 0.3)
                    {
                        y_position += (int)((targetY - 0) * 18);
                    }
                    else if (Math.Abs(targetY) > 0.2)
                    {
                        y_position += (int)((targetY - 0) * 15);
                    }
                    else
                    {
                        y_position += (int)((targetY - 0) * 10);
                    }

                    //  y_position += (int)((targetY - 0) * 28);
                    if (y_position >= 10 && y_position <= 68)
                    {
                        sendMessage_y[1] = 0x01;
                        sendMessage_y[2] = 0x01;
                        sendMessage_y[3] = Convert.ToByte(y_position);
                        clientSocket.Send(sendMessage_y);
                    }
                    //targetY_temp = targetY;

                    //  textBox_message.AppendText("send:");
                    // for (int i = 0; i < 5; i++)
                    //{
                    //    textBox_message.AppendText(Convert.ToString(sendMessage[i], 16));
                    //}
                    //textBox_message.AppendText("\r\n");
                }
                catch
                {
                    // clientSocket.Shutdown(SocketShutdown.Both);
                    // clientSocket.Close();
                }
            }
        }

        private void timer_X_Y_Change_Tick(object sender, EventArgs e)
        {
            x_enable = (x_enable + 1) % 2;
            y_enable = (y_enable + 1) % 2;
        }


    }
}

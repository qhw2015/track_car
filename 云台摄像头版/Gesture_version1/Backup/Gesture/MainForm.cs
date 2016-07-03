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

namespace Gesture
{
    public partial class MainForm : Form
    {
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

        public MainForm( )
        {
            InitializeComponent( );

            // show device list
            try
            {
                // enumerate video devices
                videoDevices = new FilterInfoCollection( FilterCategory.VideoInputDevice );

                if ( videoDevices.Count == 0 )
                {
                    throw new Exception( );
                }

                for ( int i = 1, n = videoDevices.Count; i <= n; i++ )
                {
                    string cameraName = i + " : " + videoDevices[i - 1].Name;

                    camera1Combo.Items.Add( cameraName );
                }

                camera1Combo.SelectedIndex = 0;
            }
            catch
            {
                startButton.Enabled = false;

                camera1Combo.Items.Add( "No cameras found" );
                camera1Combo.SelectedIndex = 0;
                camera1Combo.Enabled = false;
            }

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

        // On "Start" button click - start cameras
        private void startButton_Click( object sender, EventArgs e )
        {
            StartCameras( );

            startButton.Enabled = false;
            stopButton.Enabled = true;
        }

        // On "Stop" button click - stop cameras
        private void stopButton_Click( object sender, EventArgs e )
        {
            StopCameras( );

            startButton.Enabled = true;
            stopButton.Enabled = false;
        }

        // Start cameras
        private void StartCameras( )
        {
            // create first video source
            VideoCaptureDevice videoSource1 = new VideoCaptureDevice( videoDevices[camera1Combo.SelectedIndex].MonikerString );
            videoSource1.DesiredFrameRate = 15;

            videoSourcePlayer1.VideoSource = videoSource1;
            videoSourcePlayer1.Start( );

            camera1Acquired = new AutoResetEvent( false );
            // start tracking thread
            trackingThread = new Thread( new ThreadStart( TrackingThread ) );
            trackingThread.Start( );
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
                        x1 /= ( objectImage.Width / 2 );
                        y1 /= ( objectImage.Height / 2 );

                        camera1Acquired.Set( );
                    }
                }

                UpdateObjectPicture(objectImage );
            }
        }

        // Thread to track object
        private void TrackingThread( )
        {
            float targetX = 0;
            float targetY = 0;

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
            Color[] pixelColor = new Color[10];
            int redMax = 0, blueMax = 0, greenMin = 0;
            for (int i = -5, ii = 0; i < 5; i++, ii++)
            {
                int x = e.X + i;
                int y = e.Y + i;

                pixelColor[ii] = nowImg.GetPixel(x, y);
                redMax = Math.Max(redMax, pixelColor[ii].R);
                blueMax = Math.Max(blueMax, pixelColor[ii].B);
                greenMin = Math.Max(redMax, blueMax);
            }
            colorFilter.Red.Min = 0;
            colorFilter.Red.Max = redMax;
            colorFilter.Blue.Min = 0;
            colorFilter.Blue.Max = blueMax;
            colorFilter.Green.Min = greenMin;
            colorFilter.Green.Max = 255;
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
    }
}

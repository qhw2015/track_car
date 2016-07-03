namespace Gesture
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose( bool disposing )
        {
            if ( disposing && ( components != null ) )
            {
                components.Dispose( );
            }
            base.Dispose( disposing );
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent( )
        {
            this.components = new System.ComponentModel.Container();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.videoSourcePlayer1 = new AForge.Controls.VideoSourcePlayer();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btn_camera = new System.Windows.Forms.Button();
            this.label_yvalue = new System.Windows.Forms.Label();
            this.label_xvalue = new System.Windows.Forms.Label();
            this.label_location_y = new System.Windows.Forms.Label();
            this.label_location_x = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.tuneObjectFilterButton = new System.Windows.Forms.Button();
            this.objectDetectionCheck = new System.Windows.Forms.CheckBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.button_IP_Input = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.maskedTextBox1 = new System.Windows.Forms.MaskedTextBox();
            this.button_connect = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.maskedTextBox2 = new System.Windows.Forms.MaskedTextBox();
            this.button_camera_input = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox_camera = new System.Windows.Forms.TextBox();
            this.timer_X_Y_Change = new System.Windows.Forms.Timer(this.components);
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.videoSourcePlayer1);
            this.groupBox1.Controls.Add(this.pictureBox1);
            this.groupBox1.Location = new System.Drawing.Point(12, 57);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(576, 278);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Camera 1";
            // 
            // videoSourcePlayer1
            // 
            this.videoSourcePlayer1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.videoSourcePlayer1.ForeColor = System.Drawing.Color.White;
            this.videoSourcePlayer1.Location = new System.Drawing.Point(6, 20);
            this.videoSourcePlayer1.Name = "videoSourcePlayer1";
            this.videoSourcePlayer1.Size = new System.Drawing.Size(192, 144);
            this.videoSourcePlayer1.TabIndex = 0;
            this.videoSourcePlayer1.VideoSource = null;
            this.videoSourcePlayer1.NewFrame += new AForge.Controls.VideoSourcePlayer.NewFrameHandler(this.videoSourcePlayer1_NewFrame);
            this.videoSourcePlayer1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.videoSourcePlayer1_MouseUp);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Location = new System.Drawing.Point(250, 20);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(320, 240);
            this.pictureBox1.TabIndex = 4;
            this.pictureBox1.TabStop = false;
            // 
            // btn_camera
            // 
            this.btn_camera.Location = new System.Drawing.Point(12, 14);
            this.btn_camera.Name = "btn_camera";
            this.btn_camera.Size = new System.Drawing.Size(123, 31);
            this.btn_camera.TabIndex = 9;
            this.btn_camera.Text = "打开摄像头";
            this.btn_camera.UseVisualStyleBackColor = true;
            this.btn_camera.Click += new System.EventHandler(this.btn_camera_Click);
            // 
            // label_yvalue
            // 
            this.label_yvalue.AutoSize = true;
            this.label_yvalue.Location = new System.Drawing.Point(169, 33);
            this.label_yvalue.Name = "label_yvalue";
            this.label_yvalue.Size = new System.Drawing.Size(17, 12);
            this.label_yvalue.TabIndex = 8;
            this.label_yvalue.Text = "00";
            this.label_yvalue.TextChanged += new System.EventHandler(this.label_yvalue_TextChanged);
            // 
            // label_xvalue
            // 
            this.label_xvalue.AutoSize = true;
            this.label_xvalue.Location = new System.Drawing.Point(169, 12);
            this.label_xvalue.Name = "label_xvalue";
            this.label_xvalue.Size = new System.Drawing.Size(17, 12);
            this.label_xvalue.TabIndex = 7;
            this.label_xvalue.Text = "00";
            this.label_xvalue.TextChanged += new System.EventHandler(this.label_xvalue_TextChanged);
            // 
            // label_location_y
            // 
            this.label_location_y.AutoSize = true;
            this.label_location_y.Location = new System.Drawing.Point(146, 33);
            this.label_location_y.Name = "label_location_y";
            this.label_location_y.Size = new System.Drawing.Size(17, 12);
            this.label_location_y.TabIndex = 6;
            this.label_location_y.Text = "Y:";
            // 
            // label_location_x
            // 
            this.label_location_x.AutoSize = true;
            this.label_location_x.Location = new System.Drawing.Point(146, 12);
            this.label_location_x.Name = "label_location_x";
            this.label_location_x.Size = new System.Drawing.Size(17, 12);
            this.label_location_x.TabIndex = 5;
            this.label_location_x.Text = "X:";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.tuneObjectFilterButton);
            this.groupBox4.Controls.Add(this.objectDetectionCheck);
            this.groupBox4.Location = new System.Drawing.Point(242, 1);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(333, 50);
            this.groupBox4.TabIndex = 11;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Object Detection";
            // 
            // tuneObjectFilterButton
            // 
            this.tuneObjectFilterButton.Location = new System.Drawing.Point(201, 18);
            this.tuneObjectFilterButton.Name = "tuneObjectFilterButton";
            this.tuneObjectFilterButton.Size = new System.Drawing.Size(109, 21);
            this.tuneObjectFilterButton.TabIndex = 12;
            this.tuneObjectFilterButton.Text = "&Tune Object Filter";
            this.tuneObjectFilterButton.UseVisualStyleBackColor = true;
            this.tuneObjectFilterButton.Click += new System.EventHandler(this.tuneObjectFilterButton_Click);
            // 
            // objectDetectionCheck
            // 
            this.objectDetectionCheck.AutoSize = true;
            this.objectDetectionCheck.Location = new System.Drawing.Point(10, 23);
            this.objectDetectionCheck.Name = "objectDetectionCheck";
            this.objectDetectionCheck.Size = new System.Drawing.Size(168, 16);
            this.objectDetectionCheck.TabIndex = 11;
            this.objectDetectionCheck.Text = "Turn On Object &Detection";
            this.objectDetectionCheck.UseVisualStyleBackColor = true;
            this.objectDetectionCheck.CheckedChanged += new System.EventHandler(this.objectDetectionCheck_CheckedChanged);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 10;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label_location_y);
            this.groupBox2.Controls.Add(this.btn_camera);
            this.groupBox2.Controls.Add(this.label_yvalue);
            this.groupBox2.Controls.Add(this.label_location_x);
            this.groupBox2.Controls.Add(this.label_xvalue);
            this.groupBox2.Location = new System.Drawing.Point(12, 1);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(224, 50);
            this.groupBox2.TabIndex = 12;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "camera";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.button_IP_Input);
            this.groupBox5.Controls.Add(this.label1);
            this.groupBox5.Controls.Add(this.maskedTextBox1);
            this.groupBox5.Controls.Add(this.button_connect);
            this.groupBox5.Controls.Add(this.label5);
            this.groupBox5.Controls.Add(this.maskedTextBox2);
            this.groupBox5.Controls.Add(this.button_camera_input);
            this.groupBox5.Controls.Add(this.label2);
            this.groupBox5.Controls.Add(this.label4);
            this.groupBox5.Controls.Add(this.label3);
            this.groupBox5.Controls.Add(this.textBox_camera);
            this.groupBox5.Location = new System.Drawing.Point(12, 341);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(576, 79);
            this.groupBox5.TabIndex = 30;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "设置";
            // 
            // button_IP_Input
            // 
            this.button_IP_Input.Location = new System.Drawing.Point(184, 11);
            this.button_IP_Input.Name = "button_IP_Input";
            this.button_IP_Input.Size = new System.Drawing.Size(81, 23);
            this.button_IP_Input.TabIndex = 1;
            this.button_IP_Input.Text = "IP地址输入";
            this.button_IP_Input.UseVisualStyleBackColor = true;
            this.button_IP_Input.Click += new System.EventHandler(this.button_IP_Input_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 64);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "label1";
            // 
            // maskedTextBox1
            // 
            this.maskedTextBox1.Location = new System.Drawing.Point(57, 13);
            this.maskedTextBox1.Mask = "000.000.000.000";
            this.maskedTextBox1.Name = "maskedTextBox1";
            this.maskedTextBox1.Size = new System.Drawing.Size(100, 21);
            this.maskedTextBox1.TabIndex = 3;
            // 
            // button_connect
            // 
            this.button_connect.Location = new System.Drawing.Point(184, 35);
            this.button_connect.Name = "button_connect";
            this.button_connect.Size = new System.Drawing.Size(81, 23);
            this.button_connect.TabIndex = 4;
            this.button_connect.Text = "连接服务器";
            this.button_connect.UseVisualStyleBackColor = true;
            this.button_connect.Click += new System.EventHandler(this.button_connect_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(273, 63);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 12);
            this.label5.TabIndex = 25;
            this.label5.Text = "label5";
            // 
            // maskedTextBox2
            // 
            this.maskedTextBox2.Location = new System.Drawing.Point(57, 35);
            this.maskedTextBox2.Mask = "9999";
            this.maskedTextBox2.Name = "maskedTextBox2";
            this.maskedTextBox2.Size = new System.Drawing.Size(30, 21);
            this.maskedTextBox2.TabIndex = 5;
            // 
            // button_camera_input
            // 
            this.button_camera_input.Location = new System.Drawing.Point(497, 35);
            this.button_camera_input.Name = "button_camera_input";
            this.button_camera_input.Size = new System.Drawing.Size(75, 23);
            this.button_camera_input.TabIndex = 24;
            this.button_camera_input.Text = "确定";
            this.button_camera_input.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 12);
            this.label2.TabIndex = 6;
            this.label2.Text = "IP地址:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(271, 17);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(71, 12);
            this.label4.TabIndex = 23;
            this.label4.Text = "视频流地址:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(4, 38);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 12);
            this.label3.TabIndex = 7;
            this.label3.Text = "端口:";
            // 
            // textBox_camera
            // 
            this.textBox_camera.Location = new System.Drawing.Point(349, 13);
            this.textBox_camera.Name = "textBox_camera";
            this.textBox_camera.Size = new System.Drawing.Size(221, 21);
            this.textBox_camera.TabIndex = 22;
            // 
            // timer_X_Y_Change
            // 
            this.timer_X_Y_Change.Enabled = true;
            this.timer_X_Y_Change.Interval = 10;
            this.timer_X_Y_Change.Tick += new System.EventHandler(this.timer_X_Y_Change_Tick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(606, 425);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Two Cameras Vision";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private AForge.Controls.VideoSourcePlayer videoSourcePlayer1;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button tuneObjectFilterButton;
        private System.Windows.Forms.CheckBox objectDetectionCheck;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label_yvalue;
        private System.Windows.Forms.Label label_xvalue;
        private System.Windows.Forms.Label label_location_y;
        private System.Windows.Forms.Label label_location_x;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button btn_camera;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Button button_IP_Input;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.MaskedTextBox maskedTextBox1;
        private System.Windows.Forms.Button button_connect;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.MaskedTextBox maskedTextBox2;
        private System.Windows.Forms.Button button_camera_input;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox_camera;
        private System.Windows.Forms.Timer timer_X_Y_Change;
    }
}


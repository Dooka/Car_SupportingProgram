﻿using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Windows.Interop;
using webCamTest.MonitorBrightness;
using webCamTest.MovingWindows;
using webCamTest.ReverseCamera;
using webCamTest.ScreenCompare;
using System.Collections.Concurrent;
using System.Threading;

namespace webCamTest
{
    public partial class Form1 : Form
    {
        private Rectangle workingRectangle;
        private int ReversCamera = 0;
        private int LeftGaugeIndex = 1;
        private int MiddleGaugeIndex = 2;
        private int RightGaugeIndex = 3;
        private int morningTime;
        private int eveningTime;
        private string templatePicPath;
        private bool showGauges = true;
        private OpenCvVersion openCvVersionLeft;
        private OpenCvVersion openCvVersionRight;
        private OpenCvVersion openCvVersionMiddle;
        private OpenCvVersion openCvVersionReverse;

        public Form1()
        {
            InitializeComponent();

            InitializeStuff();
           
        }
    
        private void InitializeStuff()
        {
            CaptureLeftGaugeBtn.Visible = false;
            CaptureMiddletGaugeBtn.Visible = false;
            CaptureRightGaugeBtn.Visible = false;

            reverseCamPicBx.Visible = true;

            leftGuagePicBx.Visible = false;
            middleGuagePicBx.Visible = false;
            rightGuagePicBx.Visible = false;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            if(showGauges)
            {
                this.WindowState = FormWindowState.Maximized;

                reverseCamPicBx.Visible = false;

                CaptureLeftGaugeBtn.Visible = true;
                CaptureMiddletGaugeBtn.Visible = true;
                CaptureRightGaugeBtn.Visible = true;
                leftGuagePicBx.Visible = true;
                middleGuagePicBx.Visible = true;
                rightGuagePicBx.Visible = true;
                showGauges = false;
            }
            else
            {
                this.WindowState = FormWindowState.Normal;

                reverseCamPicBx.Visible = true;
                
                CaptureLeftGaugeBtn.Visible = false;
                CaptureMiddletGaugeBtn.Visible = false;
                CaptureRightGaugeBtn.Visible = false;
                leftGuagePicBx.Visible = false;
                middleGuagePicBx.Visible = false;
                rightGuagePicBx.Visible = false;
                showGauges = true;
            }
        }

        
        private void button3_Click(object sender, EventArgs e)
        {
            var Ocr = new IronOcr.AutoOcr();
            var Result = Ocr.Read(@"C:\Users\chris\Desktop\origImage.png");
            Console.WriteLine(Result.Text);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            workingRectangle = Screen.PrimaryScreen.WorkingArea;
            this.Location = new Point(295, 0);
            this.Size = new Size((workingRectangle.Width - 275), workingRectangle.Height + 10);


            //Moving Windows
            //Start_MovingWindows movingWindows = new Start_MovingWindows();
            //movingWindows.StartMovingWindows();

            ReadSettingsFile();

            //Start Streming Gauges
            SetUpReverseCameraPicBx();
            openCvVersionLeft = new OpenCvVersion("Left", LeftGaugeIndex, leftGuagePicBx, this);
            //openCvVersionRight = new OpenCvVersion("Right", RightGaugeIndex, rightGuagePicBx);
            //openCvVersionMiddle = new OpenCvVersion("Middle", MiddleGaugeIndex, middleGuagePicBx);
            //openCvVersionReverse = new OpenCvVersion("Reverse", ReversCamera, reverseCamPicBx, this);



            ////Start Reverse Camera
            //Start_ReverseCamera start_ReverseCamera = new Start_ReverseCamera();
            //start_ReverseCamera.SetCamIndex(ReversCamera);
            //start_ReverseCamera.StartReverseCamera(reverseCamPicBx);


            //Start Brightness Monitoring
            //IntPtr windowHandle = this.Handle;
            //Start_MonitorBrightness start_MonitorBrightness = new Start_MonitorBrightness();
            //start_MonitorBrightness.StartMonitorBrightness(windowHandle, morningTime, eveningTime);
        }



        private void SetUpReverseCameraPicBx()
        {
            reverseCamPicBx.Location = new Point(0, 31);
            reverseCamPicBx.SizeMode = PictureBoxSizeMode.StretchImage;
            reverseCamPicBx.Width = this.Width;
            reverseCamPicBx.Height = this.Height;
            reverseCamPicBx.BackColor = Color.Green;
        }


        private void ReadSettingsFile()
        {
            string[] lines = File.ReadAllLines("Settings.txt");
            foreach(string line in lines)
            {
                string[] result = line.Split('=');

                if(line.Contains("Reverse"))
                {
                    ReversCamera = Convert.ToInt32(result[1]);
                }
                if (line.Contains("Left"))
                {
                    LeftGaugeIndex = Convert.ToInt32(result[1]);
                }
                if (line.Contains("Middle"))
                {
                    MiddleGaugeIndex = Convert.ToInt32(result[1]);
                }
                if (line.Contains("Right"))
                {
                    RightGaugeIndex = Convert.ToInt32(result[1]);
                }
                if (line.Contains("Template"))
                {
                    templatePicPath = result[1];
                }
                if (line.Contains("Morning"))
                {
                    morningTime = Convert.ToInt32(result[1]);
                }
                if (line.Contains("Evening"))
                {
                    eveningTime = Convert.ToInt32(result[1]);
                }

            }

        }



        private void Form1_Resize(object sender, EventArgs e)
        {
            Control control = (Control)sender;
            leftGuagePicBx.SizeMode = PictureBoxSizeMode.StretchImage;
            leftGuagePicBx.Height = control.Size.Height;
            leftGuagePicBx.Width = control.Size.Width / 3;

            middleGuagePicBx.Location = new Point(control.Size.Width / 3, 31);
            middleGuagePicBx.SizeMode = PictureBoxSizeMode.StretchImage;
            middleGuagePicBx.Height = control.Size.Height;
            middleGuagePicBx.Width = control.Size.Width / 3;
            middleGuagePicBx.BackColor = Color.Blue;


            rightGuagePicBx.Location = new Point((control.Size.Width / 3) * 2, 31);
            rightGuagePicBx.SizeMode = PictureBoxSizeMode.StretchImage;
            rightGuagePicBx.Height = control.Size.Height;
            rightGuagePicBx.Width = control.Size.Width / 3;
            rightGuagePicBx.BackColor = Color.Red;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            openCvVersionLeft.takePic = true;
        }

        private void CaptureMiddletGaugeBtn_Click(object sender, EventArgs e)
        {
            openCvVersionMiddle.takePic = true;
        }

        private void CaptureRightGaugeBtn_Click(object sender, EventArgs e)
        {
            openCvVersionRight.takePic = true;
        }
    }
}
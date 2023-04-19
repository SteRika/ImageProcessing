using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.Util;
using Emgu.CV.Structure;
using Microsoft.VisualStudio.TestTools.UITesting;

namespace DIP
{
    public partial class Form1 : Form
    {
       
        public Form1()
        {
            InitializeComponent();
        }
        string gg;
        int max, min;

        Capture capture = new Capture();
        Image<Bgr, byte> frame= new Image<Bgr,byte>(320,240);
        Image<Bgr, byte> frameR = new Image<Bgr, byte>(320, 240);
        Image<Bgr, byte> frameG = new Image<Bgr, byte>(320, 240);
        Image<Bgr, byte> frameB = new Image<Bgr, byte>(320, 240);
        Image<Gray, byte> lightness = new Image<Gray, byte>(320, 240);        
        Image<Gray, byte> average = new Image<Gray, byte>(320, 240);
        Image<Gray, byte> luminosity = new Image<Gray, byte>(320, 240);
        Image<Gray, byte> binary = new Image<Gray, byte>(320, 240);

        double whiteColor = 0;
        double blackColor = 0;
        double whiteColor2 = 0;
        double blackColor2 = 0;
        double a = 0;

        private void Form1_Load(object sender, EventArgs e)
        {
            radioButton4.Checked = true;
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            frame = capture.QueryFrame().ToImage<Bgr, byte>();
            frame = frame.Resize(320, 240, Emgu.CV.CvEnum.Inter.Cubic).Flip(Emgu.CV.CvEnum.FlipType.Horizontal);


          

            if (radioButton4.Checked) 
            {
                
                for (int x = 0; x < frame.Height; x++)
                {
                    for (int y = 0; y < frame.Width; y++)
                    {
                        byte R = frame.Data[x, y, 2];
                        byte G = frame.Data[x, y, 1];
                        byte B = frame.Data[x, y, 0];
                        average.Data[x, y, 0] = Convert.ToByte((R + G + B) / 3);
                    }
                }
                for (int x = 0; x < frame.Height; x++)
                {
                    for (int y = 0; y < frame.Width; y++)
                    {
                        if (average.Data[x, y, 0] < 240)
                        {
                            binary.Data[x, y, 0] = 0;
                        }
                        else
                        {
                            binary.Data[x, y, 0] = 255;
                        }
                    }                    
                    imageBox2.Image=binary;
                    binary.Save(gg);
                    //histogramBox1.Visible = false;
                }                            
            }            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Enabled = true;
        }

        private void stop_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = binary.ToBitmap();
            binary.Save(@"D:\MyPic3.jpg");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Filter = "Image Files(*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.GIF|All files (*.*)|*.*";
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    pictureBox2.ImageLocation = dialog.FileName;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Bitmap bmp = new Bitmap(pictureBox1.Image);
            Bitmap bmp2 = new Bitmap(pictureBox2.Image);
            try
            {
                for (int x = 0; x < bmp.Width; x++)
                {
                    for (int y = 0; y < bmp.Height; y++)
                    {
                        Color color = bmp.GetPixel(x, y);

                        if (color.ToArgb() == Color.White.ToArgb())
                        {
                            whiteColor++;
                        }

                        else
                        if (color.ToArgb() == Color.Black.ToArgb())
                        {
                            blackColor++;
                        }
                    }
                    label1.Visible = true;
                    label2.Visible = true;
                    label1.Text = Convert.ToString(whiteColor ) + " white pixels";
                    label2.Text = Convert.ToString(blackColor ) + " black pixels";
                }

                {
                    for (int x = 0; x < bmp2.Width; x++)
                    {
                        for (int y = 0; y < bmp2.Height; y++)
                        {
                            Color color = bmp2.GetPixel(x, y);

                            if (color.ToArgb() == Color.White.ToArgb())
                            {
                                whiteColor2++;
                            }

                            else
                            if (color.ToArgb() == Color.Black.ToArgb())
                            {
                                blackColor2++;
                            }
                        }
                        label3.Visible = true;
                        label4.Visible = true;
                        label3.Text = Convert.ToString(blackColor2) + " black pixels";
                        label4.Text = Convert.ToString(whiteColor2) + " white pixels";
                    }
                }
                
                this.Cursor = Cursors.WaitCursor;
                a = Math.Round(((whiteColor / whiteColor2) * 100), 2 );

                if (a >= 0 && a <= 100)
                {
                    label5.Text = Convert.ToString(a);
                    label5.Visible = true;
                    label10.Visible = false;
                }

                else if ( a >= 100 && a <= 103 )
                {
                    a = 100;
                    label5.Text = Convert.ToString(a);
                    label5.Visible = true;
                    label10.Visible = false;
                }

                else if (a >= 104)
                {
                    label5.Visible = false;
                    label10.Text = "OUT OF RANGE";
                    label10.Visible = true;
                }

                if (a >= trackBar3.Value && a <= 103)
                {
                    textBox2.Text = "PASS";
                    textBox2.BackColor = Color.Green;
                    textBox2.ForeColor = Color.White;
                    blackColor = 0;
                    blackColor2 = 0;
                    whiteColor = 0;
                    whiteColor2 = 0;

                }
                else
                {
                    textBox2.Text = "FAIL";
                    textBox2.BackColor = Color.Red;
                    textBox2.ForeColor = Color.Black;
                    blackColor = 0;
                    blackColor2 = 0;
                    whiteColor = 0;
                    whiteColor2 = 0;
                }

            }
            catch (Exception ex)
            { MessageBox.Show(ex.InnerException.Message); }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            textBox1.Text = trackBar3.Value.ToString();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            binary.Save(@"D:\MyPic.bmp");
        }

        

         
    }
}

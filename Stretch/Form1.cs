using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Stretch
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            string[] supported = { ".jpg", ".bmp", ".png" };
            foreach (var path in files) 
            {
                //process the image...
                if (supported.Contains(Path.GetExtension(path).ToLower()))
                {
                    pictureBox1.ImageLocation = path;
                    break;  //only do first for now...
                }
                else
                {
                    Console.WriteLine("unsupported:  {0}", Path.GetExtension(path).ToLower());
                }
            }
            
        }

        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            
             if (e.Data.GetDataPresent(DataFormats.FileDrop)) 
             {
            e.Effect = DragDropEffects.Copy;
        }
             
        }

        private void buttonStretch_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image == null)
            {
                return;
            }

            double percentHorizontal = 100.0;
            double percentVertical = 100.0;

            double.TryParse(textBoxHorizontal.Text, out percentHorizontal);
            double.TryParse(textBoxVertical.Text, out percentVertical);

            int newWidth = (int)(Math.Round(percentHorizontal)/100 * pictureBox1.Image.Width);
            int newHeight = (int)(Math.Round(percentVertical)/100 * pictureBox1.Image.Height);

            pictureBox1.Image = StretchedImage(pictureBox1.Image, newWidth, newHeight);
        }
        static Image StretchedImage(Image imgPhoto, int Width, int Height)
        {
            int sourceWidth = imgPhoto.Width;
            int sourceHeight = imgPhoto.Height;
            int sourceX = 0;
            int sourceY = 0;
            int destX = 0;
            int destY = 0;


            //Bitmap bmPhoto = new Bitmap(Width, Height,PixelFormat.Format24bppRgb);
            Bitmap bmPhoto = new Bitmap(Width, Height, PixelFormat.Format32bppArgb);
            bmPhoto.SetResolution(imgPhoto.HorizontalResolution,
                             imgPhoto.VerticalResolution);

            Graphics grPhoto = Graphics.FromImage(bmPhoto);
            //grPhoto.Clear(Color.Red);
            grPhoto.Clear(Color.Empty);
            
            grPhoto.InterpolationMode =
                    InterpolationMode.HighQualityBicubic;

            grPhoto.DrawImage(imgPhoto,
                new Rectangle(destX, destY, Width, Height),
                new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
                GraphicsUnit.Pixel);

            grPhoto.Dispose();
            return bmPhoto;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = Application.ProductName;
        }

        private void buttonSaveAs_Click(object sender, EventArgs e)
        {
            ImageFormat[] arrFormats = {
                    ImageFormat.Jpeg,
                    ImageFormat.Png,
                    ImageFormat.Bmp};

            saveFileDialog1.Filter = "PNG (*.png)|*.png|JPEG (*.jpg)|*.jpg|Bitmap (*.bmp)|*.bmp";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                ImageFormat imgFormat = arrFormats[saveFileDialog1.FilterIndex - 1];
                pictureBox1.Image.Save(saveFileDialog1.FileName,imgFormat);
            }
        }

        private void buttonLoadImage_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Image Files (*.png, *.jpg, *.bmp)|*.png;*.jpg;*.bmp";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.ImageLocation = openFileDialog1.FileName;
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox1 about = new AboutBox1();
            about.ShowDialog();


        }
    }
}

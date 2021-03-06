﻿
using System.Windows.Forms;


namespace VideoEdit
{


    public partial class Form1 : Form
    {


        public Form1()
        {
            InitializeComponent();
        } // End Constructor 


        private static string MapProjectFile(string file)
        {
            string dir = System.IO.Path.GetDirectoryName(typeof(Form1).Assembly.Location);
            dir = System.IO.Path.Combine(dir, "../../");
            dir = System.IO.Path.GetFullPath(dir);
            dir = System.IO.Path.Combine(dir, file);

            dir = dir.Replace('\\', System.IO.Path.DirectorySeparatorChar);
            dir = dir.Replace('/', System.IO.Path.DirectorySeparatorChar);

            return dir;
        } // End Function MapProjectFile 


        private static string MapBindirFile(string file)
        {
            string dir = System.IO.Path.GetDirectoryName(typeof(Form1).Assembly.Location);
            dir = System.IO.Path.GetFullPath(dir);
            dir = System.IO.Path.Combine(dir, file);

            dir = dir.Replace('\\', System.IO.Path.DirectorySeparatorChar);
            dir = dir.Replace('/', System.IO.Path.DirectorySeparatorChar);

            return dir;
        } // End Function MapBindirFile 


        private void btnEditVideo_Click(object sender, System.EventArgs e)
        {
            // ffmpeg -i video.avi -r 0.5 -f image2 output_%05d.jpg
            // produces a frame every 2 seconds because -r means frame rate. 
            // In this case, 0.5 frames a second, or 1 frame every 2 seconds.

            // ffmpeg -i video.avi -r 0.01979 -f image2 output_%05d.jpg


            string ffmpegPath = MapBindirFile("ffmpeg/Win64/ffmpeg.exe");
            string dataPath = MapProjectFile("data/MOV_0911.mp4");
            System.Console.WriteLine(ffmpegPath);
            System.Console.WriteLine(dataPath);


            System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo(ffmpegPath);

            psi.WorkingDirectory = System.IO.Path.GetDirectoryName(typeof(Form1).Assembly.Location);
            psi.WorkingDirectory = System.IO.Path.Combine(psi.WorkingDirectory, "data");
            if (!System.IO.Directory.Exists(psi.WorkingDirectory))
                System.IO.Directory.CreateDirectory(psi.WorkingDirectory);

            psi.Arguments = "-i \"video.avi\" -r 0.5 -f image2 output_%05d.jpg";
            psi.Arguments = "-i \"" + "video.avi" + "\" -r 0.5 -f image2 output_%05d.png";
            psi.Arguments = "-i \"" + dataPath.Replace("\"", "\\\"") + "\" -r 0.5 -f image2 output_%05d.jpg";
            psi.Arguments = "-i \"" + dataPath.Replace("\"", "\\\"") + "\" -f image2 output_%05d.png";

            psi.UseShellExecute = false;
            psi.RedirectStandardOutput = true;

            
            using (System.Diagnostics.Process process = System.Diagnostics.Process.Start(psi))
            {
                System.Text.StringBuilder standardOutput = new System.Text.StringBuilder();

                // read chunk-wise while process is running.
                while (!process.HasExited)
                {
                    standardOutput.Append(process.StandardOutput.ReadToEnd());
                } // Whend

                // make sure not to miss out on any remaindings.
                standardOutput.Append(process.StandardOutput.ReadToEnd());
                string str = standardOutput.ToString();
                System.Console.WriteLine(str);
            } // End Using process 


            string[] filez = System.IO.Directory.GetFiles(psi.WorkingDirectory, "*.png");

            System.Drawing.StringFormat strFormat = new System.Drawing.StringFormat();
            strFormat.Alignment = System.Drawing.StringAlignment.Near; // Left, Center, Right
            strFormat.LineAlignment = System.Drawing.StringAlignment.Center; // Top, Middle, Bottom



            System.Drawing.Pen pen = new System.Drawing.Pen(System.Drawing.Color.Red, 2.0f);

            foreach (string file in filez)
            {
                using (System.Drawing.Image img = System.Drawing.Image.FromFile(file))
                {
                    //rotate the picture by 90 degrees and re-save the picture as a Jpeg
                    img.RotateFlip(System.Drawing.RotateFlipType.Rotate270FlipNone);

                    System.Drawing.Rectangle rect = new System.Drawing.Rectangle(0, 0, img.Width, img.Height);

                    using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(img))
                    {
                        //g.DrawString("My\nText", new Font("Tahoma", 20), Brushes.White, rect, strFormat);
                        // ®™ ©
                        g.DrawString("© 2016 COR Air™ GmbH & Co. KG aA", new System.Drawing.Font("Tahoma", 20), System.Drawing.Brushes.White, rect, strFormat);
                        // g.DrawRectangle(pen, rect);
                    } // End Using g 

                    img.Save(file, System.Drawing.Imaging.ImageFormat.Png);
                } // End Usinbg img 

            } // Next file 

            // https://stackoverflow.com/questions/24961127/ffmpeg-create-video-from-images
            // Create video 
            // ffmpeg -r 1/5 -i output_%05d.png -c:v libx264 -vf fps=25 -pix_fmt yuv420p out.mp4
            // ffmpeg -r 1/0.08 -i output_%05d.png -c:v libx264 -vf fps=25 -pix_fmt yuv420p out.mp4

            // ffmpeg -r 1/0.08 -i "D:\Stefan.Steiger\Documents\visual studio 2013\Projects\VideoEdit\VideoEdit\bin\Debug\data\output_%05d.png" -c:v libx264 -vf fps=25 -pix_fmt yuv420p out.mp4
            

            // https://stackoverflow.com/questions/10957412/fastest-way-to-extract-frames-using-ffmpeg
            // https://stackoverflow.com/questions/8679390/ffmpeg-extracting-20-images-from-a-video-of-variable-length
            // ffmpeg -i MOV_0911.mp4 $filename%03d.png
            // ffmpeg -i video.avi -r 0.5 -f image2 output_%05d.jpg

        } // End Sub btnEditVideo_Click 


    } // End Class Form1 : Form


} // End Namespace VideoEdit

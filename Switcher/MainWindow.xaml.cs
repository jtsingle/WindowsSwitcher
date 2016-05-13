using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using NHotkey;
using NHotkey.Wpf;

namespace Switcher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        [DllImport("user32.dll")]
        public static extern void SwitchToThisWindow(IntPtr hWnd);

        //private static Mutex mutex = new Mutex(false, "8fe9c1b5-9c10-433f-945e-2e99a86d8c76");


        public MainWindow()
        {
            //if (!mutex.WaitOne(TimeSpan.FromSeconds(1), false))
            //{
            //    this.Close();
            //    return;
            //}

            InitializeComponent();
            Hide();

            HotkeyManager.Current.AddOrReplace("SwitchWindow", Key.H, ModifierKeys.Shift | ModifierKeys.Alt, Show);
        }

        private void SwitchWindow(string title)
        {
            Process[] processlist = Process.GetProcesses();
            var ltitle = title.ToLower();
            foreach (Process process in processlist)
            {
                if (!string.IsNullOrWhiteSpace(process.MainWindowTitle) && (process.MainWindowTitle.ToLower().Contains(ltitle) || process.ProcessName.ToLower().Contains(ltitle)))
                {
                    Hide();
                    SwitchToThisWindow(process.MainWindowHandle);
                    break;
                }
            }

            this.textBox.SelectAll();
        }

        private void Show(object sender, HotkeyEventArgs e)
        {
            this.textBox.Focus();
            this.textBox.Text = string.Empty;
            this.WindowState = WindowState.Normal;
            this.Show();
        }

        private void TextBox_OnKeyUp(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl) && e.Key == Key.W)
            {
                this.Close();
            }

            if (e.Key == Key.Escape)
            {
                Hide();
            }

            if (e.Key == Key.Enter && !string.IsNullOrWhiteSpace(this.textBox.Text))
            {
                SwitchWindow(this.textBox.Text);
            }
        }
    }
}

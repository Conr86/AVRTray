using System;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace AVRTray
{
    public partial class MainForm : Form
    {
        private NotifyIcon notifyIcon;
        private AVR AVR;
        private Size defaultSize;
        private FormBorderStyle defaultStyle;

        public MainForm()
        {
            AVR = new AVR();
            InitializeComponent();

            defaultSize = Size;
            defaultStyle = FormBorderStyle;

            notifyIcon = new()
            {
                ContextMenuStrip = new ContextMenuStrip(),
                Icon = new Icon("./icon.ico"),
                Text = "AVRTray",
                Visible = true
            };
            notifyIcon.ContextMenuStrip.Items.Add("Exit", null, MenuExit_Click);
            notifyIcon.DoubleClick += Window_Show;
        }

        void Window_Hide()
        {
            Size = new Size(0, 0);
            FormBorderStyle = FormBorderStyle.None;
            ShowInTaskbar = false;
        }

        void Window_Show(object sender, EventArgs e)
        {
            Size = defaultSize;
            FormBorderStyle = defaultStyle;
            WindowState = FormWindowState.Normal;
            ShowInTaskbar = true;
        }

        void Form1_Load(object sender, EventArgs e)
        {
            volumeBar.Value = AVR.GetVolume();
            volumeField.Value = volumeBar.Value;
            sourceSelect.Items.AddRange(AVR.CustomSources.Keys.ToArray());
            sourceSelect.SelectedItem = AVR.GetSource();
            ipLabel.Text += $" {AVR.host}:{AVR.port}";
        }

        void MenuExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void volumeBar_ValueChanged(object sender, EventArgs e)
        {
            AVR.Write("MV" + volumeBar.Value);
            SetPrivateControlValue(volumeField, volumeBar.Value);
        }
        private void volumeField_ValueChanged(object sender, EventArgs e)
        {
            AVR.Write("MV" + (int)volumeField.Value);
            SetPrivateControlValue(volumeBar, (int)volumeField.Value);
        }

        private void SetPrivateControlValue(Control control, decimal value)
        {
            if (control == null) throw new ArgumentNullException(nameof(control));
            var currentValueField = control.GetType().GetField("currentValue", BindingFlags.Instance | BindingFlags.NonPublic);
            if (currentValueField != null)
            {
                currentValueField.SetValue(control, value);
                control.Text = value.ToString();
            }
        }

        private void togglePower_Click(object sender, EventArgs e)
        {
            AVR.TogglePower();
        }
        private void buttonMute_Click(object sender, EventArgs e)
        {
            AVR.ToggleMute();
        }

        private void sourceSelect_Changed(object sender, EventArgs e)
        {
            AVR.SetSource(sourceSelect.SelectedItem.ToString());
        }

        private void refreshButton_Click(object sender, EventArgs e)
        {
            SetPrivateControlValue(volumeBar, AVR.GetVolume());
            SetPrivateControlValue(volumeField, volumeBar.Value);
            sourceSelect.SelectedItem = AVR.GetSource();
        }

        private void Form_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                Window_Hide();
            } else
            {
                Window_Show(sender, e);
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            notifyIcon.Icon.Dispose();
            notifyIcon.Dispose();
        }
    }
}

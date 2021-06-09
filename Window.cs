using System;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace AVRTray
{
    public partial class MainForm : Form
    {
        private readonly NotifyIcon notifyIcon;
        private readonly AVR AVR;
        private Size defaultSize;
        private FormBorderStyle defaultStyle;

        private int timeLeft = 10;

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
            notifyIcon.ContextMenuStrip.Items.Add("Exit", null, (s, e) => Application.Exit());
            notifyIcon.DoubleClick += Window_Show;

            connectionTimer.Interval = 1000;
            connectionTimer.Start();
        }

        /// <summary>
        /// Minimise to notification tray
        /// </summary>
        void Window_Hide()
        {
            Size = new Size(0, 0);
            FormBorderStyle = FormBorderStyle.None;
            ShowInTaskbar = false;
        }

        /// <summary>
        /// Unminimize from notification tray
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Window_Show(object sender, EventArgs e)
        {
            Size = defaultSize;
            FormBorderStyle = defaultStyle;
            WindowState = FormWindowState.Normal;
            ShowInTaskbar = true;
        }

        /// <summary>
        /// Set the 'currentValue' property of a Control element without triggering its ValueChanged event
        /// </summary>
        /// <param name="control">The UI element to update</param>
        /// <param name="value">The new value to set</param>
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

        #region UI Events
        void Form_Load(object sender, EventArgs e)
        {
            // Set volume bar and field to current AVR volume
            // Note, this currently triggers a single send event setting the AVR volume to the 
            // just received volume. Not a real problem, but should be fixed at some point
            volumeBar.Value = AVR.GetVolume();
            SetPrivateControlValue(volumeField, volumeBar.Value);
            // Load custom sources into select box and set active
            sourceSelect.Items.AddRange(AVR.CustomSources.Keys.ToArray());
            sourceSelect.SelectedItem = AVR.GetSource();
            // Set IP address label
            ipLabel.Text += $" {AVR.host}:{AVR.port}";
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

        private void Form_Closing(object sender, FormClosingEventArgs e)
        {
            notifyIcon.Icon.Dispose();
            notifyIcon.Dispose();
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

        // Toggle buttons
        private void togglePower_Click(object sender, EventArgs e) => AVR.TogglePower();
        private void buttonMute_Click(object sender, EventArgs e) => AVR.ToggleMute();

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
        #endregion

        private void ConnectionStatus_Click(object sender, EventArgs e)
        {
            ConnectionStatus.Text = "Connected: " + AVR.IsConnected().ToString();
        }

        private void connectionTimer_Tick(object sender, EventArgs e)
        {
            timeLeft--;

            if (timeLeft <= 0)
            {
                AVR.Disconnect();
                ConnectionStatus_Click(null, null);

                connectionTimer.Stop();
                timeLeft = 10;
            }
        }

        private void MainForm_Click(object sender, EventArgs e)
        {
            if (!AVR.IsConnected())
            {
                AVR.Connect();
                ConnectionStatus_Click(null, null);

                connectionTimer.Start();
            }
        }
    }
}

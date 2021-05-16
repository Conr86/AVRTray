
namespace AVRTray
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.volumeBar = new System.Windows.Forms.TrackBar();
            this.powerButton = new System.Windows.Forms.Button();
            this.volumeField = new System.Windows.Forms.NumericUpDown();
            this.volumeLabel = new System.Windows.Forms.Label();
            this.buttonMute = new System.Windows.Forms.Button();
            this.sourceSelect = new System.Windows.Forms.ComboBox();
            this.refreshButton = new System.Windows.Forms.Button();
            this.ipLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.volumeBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.volumeField)).BeginInit();
            this.SuspendLayout();
            // 
            // volumeBar
            // 
            this.volumeBar.Location = new System.Drawing.Point(12, 44);
            this.volumeBar.Maximum = 98;
            this.volumeBar.Name = "volumeBar";
            this.volumeBar.Size = new System.Drawing.Size(244, 45);
            this.volumeBar.TabIndex = 0;
            this.volumeBar.ValueChanged += new System.EventHandler(this.volumeBar_ValueChanged);
            // 
            // powerButton
            // 
            this.powerButton.Location = new System.Drawing.Point(12, 188);
            this.powerButton.Name = "powerButton";
            this.powerButton.Size = new System.Drawing.Size(244, 23);
            this.powerButton.TabIndex = 1;
            this.powerButton.Text = "Toggle Power";
            this.powerButton.UseVisualStyleBackColor = true;
            this.powerButton.Click += new System.EventHandler(this.togglePower_Click);
            // 
            // volumeField
            // 
            this.volumeField.Location = new System.Drawing.Point(209, 12);
            this.volumeField.Maximum = new decimal(new int[] {
            98,
            0,
            0,
            0});
            this.volumeField.Name = "volumeField";
            this.volumeField.Size = new System.Drawing.Size(47, 23);
            this.volumeField.TabIndex = 2;
            this.volumeField.ValueChanged += new System.EventHandler(this.volumeField_ValueChanged);
            // 
            // volumeLabel
            // 
            this.volumeLabel.AutoSize = true;
            this.volumeLabel.Location = new System.Drawing.Point(12, 16);
            this.volumeLabel.Name = "volumeLabel";
            this.volumeLabel.Size = new System.Drawing.Size(47, 15);
            this.volumeLabel.TabIndex = 3;
            this.volumeLabel.Text = "Volume";
            // 
            // buttonMute
            // 
            this.buttonMute.Location = new System.Drawing.Point(12, 217);
            this.buttonMute.Name = "buttonMute";
            this.buttonMute.Size = new System.Drawing.Size(244, 23);
            this.buttonMute.TabIndex = 4;
            this.buttonMute.Text = "Toggle Mute";
            this.buttonMute.UseVisualStyleBackColor = true;
            this.buttonMute.Click += new System.EventHandler(this.buttonMute_Click);
            // 
            // sourceSelect
            // 
            this.sourceSelect.FormattingEnabled = true;
            this.sourceSelect.Location = new System.Drawing.Point(12, 95);
            this.sourceSelect.Name = "sourceSelect";
            this.sourceSelect.Size = new System.Drawing.Size(244, 23);
            this.sourceSelect.TabIndex = 5;
            this.sourceSelect.SelectionChangeCommitted += new System.EventHandler(this.sourceSelect_Changed);
            // 
            // refreshButton
            // 
            this.refreshButton.Location = new System.Drawing.Point(181, 159);
            this.refreshButton.Name = "refreshButton";
            this.refreshButton.Size = new System.Drawing.Size(75, 23);
            this.refreshButton.TabIndex = 6;
            this.refreshButton.Text = "Refresh";
            this.refreshButton.UseVisualStyleBackColor = true;
            this.refreshButton.Click += new System.EventHandler(this.refreshButton_Click);
            // 
            // ipLabel
            // 
            this.ipLabel.AutoSize = true;
            this.ipLabel.Location = new System.Drawing.Point(12, 262);
            this.ipLabel.Name = "ipLabel";
            this.ipLabel.Size = new System.Drawing.Size(65, 15);
            this.ipLabel.TabIndex = 7;
            this.ipLabel.Text = "IP Address:";
            this.ipLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(267, 296);
            this.Controls.Add(this.ipLabel);
            this.Controls.Add(this.refreshButton);
            this.Controls.Add(this.sourceSelect);
            this.Controls.Add(this.buttonMute);
            this.Controls.Add(this.volumeLabel);
            this.Controls.Add(this.volumeField);
            this.Controls.Add(this.powerButton);
            this.Controls.Add(this.volumeBar);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "AVRTray";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_Closing);
            this.Load += new System.EventHandler(this.Form_Load);
            this.Resize += new System.EventHandler(this.Form_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.volumeBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.volumeField)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TrackBar volumeBar;
        private System.Windows.Forms.Button powerButton;
        private System.Windows.Forms.NumericUpDown volumeField;
        private System.Windows.Forms.Label volumeLabel;
        private System.Windows.Forms.Button buttonMute;
        private System.Windows.Forms.ComboBox sourceSelect;
        private System.Windows.Forms.Button refreshButton;
        private System.Windows.Forms.Label ipLabel;
    }
}


namespace closer
{
	partial class MainForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
			this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.MenuSettings = new System.Windows.Forms.ToolStripMenuItem();
			this.MenuExit = new System.Windows.Forms.ToolStripMenuItem();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.contextMenuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// notifyIcon1
			// 
			this.notifyIcon1.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
			this.notifyIcon1.BalloonTipText = "Text";
			this.notifyIcon1.BalloonTipTitle = "Title";
			this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
			this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
			this.notifyIcon1.Text = "Закрывалка";
			this.notifyIcon1.Visible = true;
			// 
			// contextMenuStrip1
			// 
			this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
									this.MenuSettings,
									this.MenuExit});
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			this.contextMenuStrip1.Size = new System.Drawing.Size(135, 48);
			// 
			// MenuSettings
			// 
			this.MenuSettings.Image = ((System.Drawing.Image)(resources.GetObject("MenuSettings.Image")));
			this.MenuSettings.Name = "MenuSettings";
			this.MenuSettings.Size = new System.Drawing.Size(134, 22);
			this.MenuSettings.Text = "Настройки";
			this.MenuSettings.Click += new System.EventHandler(this.MenuSettingsClick);
			// 
			// MenuExit
			// 
			this.MenuExit.Image = ((System.Drawing.Image)(resources.GetObject("MenuExit.Image")));
			this.MenuExit.Name = "MenuExit";
			this.MenuExit.Size = new System.Drawing.Size(134, 22);
			this.MenuExit.Text = "Выход";
			this.MenuExit.Click += new System.EventHandler(this.MenuExitClick);
			// 
			// timer1
			// 
			this.timer1.Enabled = true;
			this.timer1.Interval = 1000;
			this.timer1.Tick += new System.EventHandler(this.Timer1Tick);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(185, 74);
			this.DoubleBuffered = true;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.Name = "MainForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Closer";
			this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainFormFormClosing);
			this.Load += new System.EventHandler(this.MainFormLoad);
			this.contextMenuStrip1.ResumeLayout(false);
			this.ResumeLayout(false);
		}
		private System.Windows.Forms.Timer timer1;
		private System.Windows.Forms.ToolStripMenuItem MenuExit;
		private System.Windows.Forms.ToolStripMenuItem MenuSettings;
		private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
		private System.Windows.Forms.NotifyIcon notifyIcon1;
	}
}

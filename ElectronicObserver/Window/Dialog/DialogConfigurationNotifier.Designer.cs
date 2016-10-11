namespace ElectronicObserver.Window.Dialog {
	partial class DialogConfigurationNotifier {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose( bool disposing ) {
			if ( disposing && ( components != null ) ) {
				components.Dispose();
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.components = new System.ComponentModel.Container();
			this.ButtonCancel = new System.Windows.Forms.Button();
			this.ButtonOK = new System.Windows.Forms.Button();
			this.GroupSound = new System.Windows.Forms.GroupBox();
			this.SoundPathDirectorize = new System.Windows.Forms.Button();
			this.LoopsSound = new System.Windows.Forms.CheckBox();
			this.label9 = new System.Windows.Forms.Label();
			this.SoundVolume = new System.Windows.Forms.NumericUpDown();
			this.PlaysSound = new System.Windows.Forms.CheckBox();
			this.SoundPathSearch = new System.Windows.Forms.Button();
			this.SoundPath = new System.Windows.Forms.TextBox();
			this.ButtonTest = new System.Windows.Forms.Button();
			this.IsEnabled = new System.Windows.Forms.CheckBox();
			this.GroupImage = new System.Windows.Forms.GroupBox();
			this.DrawsImage = new System.Windows.Forms.CheckBox();
			this.ImagePathSearch = new System.Windows.Forms.Button();
			this.ImagePath = new System.Windows.Forms.TextBox();
			this.GroupDialog = new System.Windows.Forms.GroupBox();
			this.label5 = new System.Windows.Forms.Label();
			this.CloseList = new System.Windows.Forms.CheckedListBox();
			this.ShowWithActivation = new System.Windows.Forms.CheckBox();
			this.label4 = new System.Windows.Forms.Label();
			this.DrawsMessage = new System.Windows.Forms.CheckBox();
			this.HasFormBorder = new System.Windows.Forms.CheckBox();
			this.label6 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.ClosingInterval = new System.Windows.Forms.NumericUpDown();
			this.BackColorPreview = new System.Windows.Forms.Label();
			this.BackColorSelect = new System.Windows.Forms.Button();
			this.ForeColorPreview = new System.Windows.Forms.Label();
			this.ForeColorSelect = new System.Windows.Forms.Button();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.AccelInterval = new System.Windows.Forms.NumericUpDown();
			this.TopMostFlag = new System.Windows.Forms.CheckBox();
			this.LocationY = new System.Windows.Forms.NumericUpDown();
			this.LocationX = new System.Windows.Forms.NumericUpDown();
			this.label1 = new System.Windows.Forms.Label();
			this.Alignment = new System.Windows.Forms.ComboBox();
			this.ShowsDialog = new System.Windows.Forms.CheckBox();
			this.GroupDamage = new System.Windows.Forms.GroupBox();
			this.NotifiesAtEndpoint = new System.Windows.Forms.CheckBox();
			this.ContainsFlagship = new System.Windows.Forms.CheckBox();
			this.ContainsSafeShip = new System.Windows.Forms.CheckBox();
			this.ContainsNotLockedShip = new System.Windows.Forms.CheckBox();
			this.label8 = new System.Windows.Forms.Label();
			this.LevelBorder = new System.Windows.Forms.NumericUpDown();
			this.NotifiesAfter = new System.Windows.Forms.CheckBox();
			this.NotifiesNow = new System.Windows.Forms.CheckBox();
			this.NotifiesBefore = new System.Windows.Forms.CheckBox();
			this.DialogColor = new System.Windows.Forms.ColorDialog();
			this.DialogOpenSound = new System.Windows.Forms.OpenFileDialog();
			this.DialogOpenImage = new System.Windows.Forms.OpenFileDialog();
			this.ToolTipText = new System.Windows.Forms.ToolTip(this.components);
			this.label10 = new System.Windows.Forms.Label();
			this.GroupAnchorageRepair = new System.Windows.Forms.GroupBox();
			this.label11 = new System.Windows.Forms.Label();
			this.AnchorageRepairNotificationLevel = new System.Windows.Forms.ComboBox();
			this.GroupSound.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.SoundVolume)).BeginInit();
			this.GroupImage.SuspendLayout();
			this.GroupDialog.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.ClosingInterval)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.AccelInterval)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.LocationY)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.LocationX)).BeginInit();
			this.GroupDamage.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.LevelBorder)).BeginInit();
			this.GroupAnchorageRepair.SuspendLayout();
			this.SuspendLayout();
			// 
			// ButtonCancel
			// 
			this.ButtonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.ButtonCancel.Location = new System.Drawing.Point(537, 407);
			this.ButtonCancel.Name = "ButtonCancel";
			this.ButtonCancel.Size = new System.Drawing.Size(75, 23);
			this.ButtonCancel.TabIndex = 7;
			this.ButtonCancel.Text = "取消";
			this.ButtonCancel.UseVisualStyleBackColor = true;
			this.ButtonCancel.Click += new System.EventHandler(this.ButtonCancel_Click);
			// 
			// ButtonOK
			// 
			this.ButtonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.ButtonOK.Location = new System.Drawing.Point(456, 407);
			this.ButtonOK.Name = "ButtonOK";
			this.ButtonOK.Size = new System.Drawing.Size(75, 23);
			this.ButtonOK.TabIndex = 6;
			this.ButtonOK.Text = "确定";
			this.ButtonOK.UseVisualStyleBackColor = true;
			this.ButtonOK.Click += new System.EventHandler(this.ButtonOK_Click);
			// 
			// GroupSound
			// 
			this.GroupSound.Controls.Add(this.SoundPathDirectorize);
			this.GroupSound.Controls.Add(this.LoopsSound);
			this.GroupSound.Controls.Add(this.label9);
			this.GroupSound.Controls.Add(this.SoundVolume);
			this.GroupSound.Controls.Add(this.PlaysSound);
			this.GroupSound.Controls.Add(this.SoundPathSearch);
			this.GroupSound.Controls.Add(this.SoundPath);
			this.GroupSound.Location = new System.Drawing.Point(12, 37);
			this.GroupSound.Name = "GroupSound";
			this.GroupSound.Size = new System.Drawing.Size(298, 78);
			this.GroupSound.TabIndex = 1;
			this.GroupSound.TabStop = false;
			this.GroupSound.Text = "通知音";
			this.GroupSound.DragDrop += new System.Windows.Forms.DragEventHandler(this.GroupSound_DragDrop);
			this.GroupSound.DragEnter += new System.Windows.Forms.DragEventHandler(this.GroupSound_DragEnter);
			// 
			// SoundPathDirectorize
			// 
			this.SoundPathDirectorize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.SoundPathDirectorize.Location = new System.Drawing.Point(260, 47);
			this.SoundPathDirectorize.Name = "SoundPathDirectorize";
			this.SoundPathDirectorize.Size = new System.Drawing.Size(32, 23);
			this.SoundPathDirectorize.TabIndex = 6;
			this.SoundPathDirectorize.Text = "Dir";
			this.ToolTipText.SetToolTip(this.SoundPathDirectorize, "删除文件名的部分，选定上级目录。\r\n设置为目录的情况，将随机播放该目录下的音频文件。");
			this.SoundPathDirectorize.UseVisualStyleBackColor = true;
			this.SoundPathDirectorize.Click += new System.EventHandler(this.SoundPathDirectorize_Click);
			// 
			// LoopsSound
			// 
			this.LoopsSound.AutoSize = true;
			this.LoopsSound.Location = new System.Drawing.Point(90, 22);
			this.LoopsSound.Name = "LoopsSound";
			this.LoopsSound.Size = new System.Drawing.Size(55, 19);
			this.LoopsSound.TabIndex = 5;
			this.LoopsSound.Text = "循环";
			this.LoopsSound.UseVisualStyleBackColor = true;
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Location = new System.Drawing.Point(163, 23);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(43, 15);
			this.label9.TabIndex = 1;
			this.label9.Text = "音量 :";
			// 
			// SoundVolume
			// 
			this.SoundVolume.Location = new System.Drawing.Point(212, 18);
			this.SoundVolume.Name = "SoundVolume";
			this.SoundVolume.Size = new System.Drawing.Size(80, 23);
			this.SoundVolume.TabIndex = 2;
			this.SoundVolume.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.ToolTipText.SetToolTip(this.SoundVolume, "设置通知音的音量。\r\n0 为无声 100 为最大音量。");
			// 
			// PlaysSound
			// 
			this.PlaysSound.AutoSize = true;
			this.PlaysSound.Location = new System.Drawing.Point(6, 22);
			this.PlaysSound.Name = "PlaysSound";
			this.PlaysSound.Size = new System.Drawing.Size(78, 19);
			this.PlaysSound.TabIndex = 0;
			this.PlaysSound.Text = "启用";
			this.PlaysSound.UseVisualStyleBackColor = true;
			// 
			// SoundPathSearch
			// 
			this.SoundPathSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.SoundPathSearch.Location = new System.Drawing.Point(222, 47);
			this.SoundPathSearch.Name = "SoundPathSearch";
			this.SoundPathSearch.Size = new System.Drawing.Size(32, 23);
			this.SoundPathSearch.TabIndex = 4;
			this.SoundPathSearch.Text = "...";
			this.SoundPathSearch.UseVisualStyleBackColor = true;
			this.SoundPathSearch.Click += new System.EventHandler(this.SoundPathSearch_Click);
			// 
			// SoundPath
			// 
			this.SoundPath.AllowDrop = true;
			this.SoundPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.SoundPath.Location = new System.Drawing.Point(6, 47);
			this.SoundPath.Name = "SoundPath";
			this.SoundPath.Size = new System.Drawing.Size(210, 23);
			this.SoundPath.TabIndex = 3;
			this.SoundPath.TextChanged += new System.EventHandler(this.SoundPath_TextChanged);
			// 
			// ButtonTest
			// 
			this.ButtonTest.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.ButtonTest.Location = new System.Drawing.Point(12, 407);
			this.ButtonTest.Name = "ButtonTest";
			this.ButtonTest.Size = new System.Drawing.Size(75, 23);
			this.ButtonTest.TabIndex = 5;
			this.ButtonTest.Text = "测试";
			this.ToolTipText.SetToolTip(this.ButtonTest, "测试通知。\r\n请注意，上述设置将被保存。");
			this.ButtonTest.UseVisualStyleBackColor = true;
			this.ButtonTest.Click += new System.EventHandler(this.ButtonTest_Click);
			// 
			// IsEnabled
			// 
			this.IsEnabled.AutoSize = true;
			this.IsEnabled.Location = new System.Drawing.Point(12, 12);
			this.IsEnabled.Name = "IsEnabled";
			this.IsEnabled.Size = new System.Drawing.Size(111, 19);
			this.IsEnabled.TabIndex = 0;
			this.IsEnabled.Text = "启用通知";
			this.IsEnabled.UseVisualStyleBackColor = true;
			// 
			// GroupImage
			// 
			this.GroupImage.Controls.Add(this.DrawsImage);
			this.GroupImage.Controls.Add(this.ImagePathSearch);
			this.GroupImage.Controls.Add(this.ImagePath);
			this.GroupImage.Location = new System.Drawing.Point(316, 37);
			this.GroupImage.Name = "GroupImage";
			this.GroupImage.Size = new System.Drawing.Size(298, 78);
			this.GroupImage.TabIndex = 2;
			this.GroupImage.TabStop = false;
			this.GroupImage.Text = "通知图片";
			this.GroupImage.DragDrop += new System.Windows.Forms.DragEventHandler(this.GroupImage_DragDrop);
			this.GroupImage.DragEnter += new System.Windows.Forms.DragEventHandler(this.GroupImage_DragEnter);
			// 
			// DrawsImage
			// 
			this.DrawsImage.AutoSize = true;
			this.DrawsImage.Location = new System.Drawing.Point(6, 22);
			this.DrawsImage.Name = "DrawsImage";
			this.DrawsImage.Size = new System.Drawing.Size(78, 19);
			this.DrawsImage.TabIndex = 0;
			this.DrawsImage.Text = "启用";
			this.DrawsImage.UseVisualStyleBackColor = true;
			// 
			// ImagePathSearch
			// 
			this.ImagePathSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.ImagePathSearch.Location = new System.Drawing.Point(260, 47);
			this.ImagePathSearch.Name = "ImagePathSearch";
			this.ImagePathSearch.Size = new System.Drawing.Size(32, 23);
			this.ImagePathSearch.TabIndex = 2;
			this.ImagePathSearch.Text = "...";
			this.ImagePathSearch.UseVisualStyleBackColor = true;
			this.ImagePathSearch.Click += new System.EventHandler(this.ImagePathSearch_Click);
			// 
			// ImagePath
			// 
			this.ImagePath.AllowDrop = true;
			this.ImagePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.ImagePath.Location = new System.Drawing.Point(6, 47);
			this.ImagePath.Name = "ImagePath";
			this.ImagePath.Size = new System.Drawing.Size(248, 23);
			this.ImagePath.TabIndex = 1;
			this.ImagePath.TextChanged += new System.EventHandler(this.ImagePath_TextChanged);
			// 
			// GroupDialog
			// 
			this.GroupDialog.Controls.Add(this.label5);
			this.GroupDialog.Controls.Add(this.CloseList);
			this.GroupDialog.Controls.Add(this.ShowWithActivation);
			this.GroupDialog.Controls.Add(this.label4);
			this.GroupDialog.Controls.Add(this.DrawsMessage);
			this.GroupDialog.Controls.Add(this.HasFormBorder);
			this.GroupDialog.Controls.Add(this.label6);
			this.GroupDialog.Controls.Add(this.label7);
			this.GroupDialog.Controls.Add(this.ClosingInterval);
			this.GroupDialog.Controls.Add(this.BackColorPreview);
			this.GroupDialog.Controls.Add(this.BackColorSelect);
			this.GroupDialog.Controls.Add(this.ForeColorPreview);
			this.GroupDialog.Controls.Add(this.ForeColorSelect);
			this.GroupDialog.Controls.Add(this.label3);
			this.GroupDialog.Controls.Add(this.label2);
			this.GroupDialog.Controls.Add(this.AccelInterval);
			this.GroupDialog.Controls.Add(this.TopMostFlag);
			this.GroupDialog.Controls.Add(this.LocationY);
			this.GroupDialog.Controls.Add(this.LocationX);
			this.GroupDialog.Controls.Add(this.label1);
			this.GroupDialog.Controls.Add(this.Alignment);
			this.GroupDialog.Controls.Add(this.ShowsDialog);
			this.GroupDialog.Location = new System.Drawing.Point(12, 121);
			this.GroupDialog.Name = "GroupDialog";
			this.GroupDialog.Size = new System.Drawing.Size(602, 171);
			this.GroupDialog.TabIndex = 3;
			this.GroupDialog.TabStop = false;
			this.GroupDialog.Text = "通知对话框";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(473, 18);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(49, 15);
			this.label5.TabIndex = 20;
			this.label5.Text = "关闭对话框 :";
			// 
			// CloseList
			// 
			this.CloseList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.CloseList.CheckOnClick = true;
			this.CloseList.FormattingEnabled = true;
			this.CloseList.Items.AddRange(new object[] {
            "鼠标左键单击",
            "鼠标左键双击",
            "鼠标右键单击",
            "鼠标右键双击",
            "鼠标中键单击",
            "鼠标中键双击",
            "鼠标悬停"});
			this.CloseList.Location = new System.Drawing.Point(476, 35);
			this.CloseList.Name = "CloseList";
			this.CloseList.Size = new System.Drawing.Size(120, 130);
			this.CloseList.TabIndex = 21;
			// 
			// ShowWithActivation
			// 
			this.ShowWithActivation.AutoSize = true;
			this.ShowWithActivation.Location = new System.Drawing.Point(6, 137);
			this.ShowWithActivation.Name = "ShowWithActivation";
			this.ShowWithActivation.Size = new System.Drawing.Size(151, 19);
			this.ShowWithActivation.TabIndex = 8;
			this.ShowWithActivation.Text = "弹出时激活窗口";
			this.ToolTipText.SetToolTip(this.ShowWithActivation, "将通知对话框设为活动窗口。\r\n取消勾选的话不会妨碍其他操作，但通知对话框可能被覆盖在其它窗口下。");
			this.ShowWithActivation.UseVisualStyleBackColor = true;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(6, 78);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(43, 15);
			this.label4.TabIndex = 3;
			this.label4.Text = "坐标 :";
			// 
			// DrawsMessage
			// 
			this.DrawsMessage.AutoSize = true;
			this.DrawsMessage.Location = new System.Drawing.Point(6, 108);
			this.DrawsMessage.Name = "DrawsMessage";
			this.DrawsMessage.Size = new System.Drawing.Size(123, 19);
			this.DrawsMessage.TabIndex = 6;
			this.DrawsMessage.Text = "显示文字信息";
			this.ToolTipText.SetToolTip(this.DrawsMessage, "是否在对话框内显示文字信息。\r\n只有图片就足够的情况可以取消勾选。");
			this.DrawsMessage.UseVisualStyleBackColor = true;
			// 
			// HasFormBorder
			// 
			this.HasFormBorder.AutoSize = true;
			this.HasFormBorder.Location = new System.Drawing.Point(135, 108);
			this.HasFormBorder.Name = "HasFormBorder";
			this.HasFormBorder.Size = new System.Drawing.Size(102, 19);
			this.HasFormBorder.TabIndex = 7;
			this.HasFormBorder.Text = "显示窗口边框";
			this.ToolTipText.SetToolTip(this.HasFormBorder, "是否显示窗口边框。\r\n只有图片就足够的情况可以取消勾选。");
			this.HasFormBorder.UseVisualStyleBackColor = true;
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(444, 52);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(19, 15);
			this.label6.TabIndex = 14;
			this.label6.Text = "秒";
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(269, 52);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(83, 15);
			this.label7.TabIndex = 12;
			this.label7.Text = "自动关闭 :";
			// 
			// ClosingInterval
			// 
			this.ClosingInterval.Location = new System.Drawing.Point(358, 47);
			this.ClosingInterval.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
			this.ClosingInterval.Name = "ClosingInterval";
			this.ClosingInterval.Size = new System.Drawing.Size(80, 23);
			this.ClosingInterval.TabIndex = 13;
			this.ClosingInterval.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.ToolTipText.SetToolTip(this.ClosingInterval, "设置通知对话框在几秒后自动关闭。\r\n设置为 0 秒时不会自动关闭。");
			// 
			// BackColorPreview
			// 
			this.BackColorPreview.AutoSize = true;
			this.BackColorPreview.Location = new System.Drawing.Point(269, 138);
			this.BackColorPreview.Name = "BackColorPreview";
			this.BackColorPreview.Size = new System.Drawing.Size(67, 15);
			this.BackColorPreview.TabIndex = 18;
			this.BackColorPreview.Text = "■ 背景色";
			this.ToolTipText.SetToolTip(this.BackColorPreview, "通知对话框的背景颜色。\r\n本栏中的 ■ 为预览。\r\n");
			this.BackColorPreview.ForeColorChanged += new System.EventHandler(this.BackColorPreview_ForeColorChanged);
			// 
			// BackColorSelect
			// 
			this.BackColorSelect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.BackColorSelect.Location = new System.Drawing.Point(342, 134);
			this.BackColorSelect.Name = "BackColorSelect";
			this.BackColorSelect.Size = new System.Drawing.Size(32, 23);
			this.BackColorSelect.TabIndex = 19;
			this.BackColorSelect.Text = "...";
			this.ToolTipText.SetToolTip(this.BackColorSelect, "设置通知对话框的背景颜色。\r\n左栏中的 ■ 为预览。\r\n");
			this.BackColorSelect.UseVisualStyleBackColor = true;
			this.BackColorSelect.Click += new System.EventHandler(this.BackColorSelect_Click);
			// 
			// ForeColorPreview
			// 
			this.ForeColorPreview.AutoSize = true;
			this.ForeColorPreview.Location = new System.Drawing.Point(269, 109);
			this.ForeColorPreview.Name = "ForeColorPreview";
			this.ForeColorPreview.Size = new System.Drawing.Size(67, 15);
			this.ForeColorPreview.TabIndex = 16;
			this.ForeColorPreview.Text = "■ 前景色";
			this.ToolTipText.SetToolTip(this.ForeColorPreview, "通知对话框的文字颜色。\r\n本栏中的 ■ 为预览。\r\n");
			this.ForeColorPreview.ForeColorChanged += new System.EventHandler(this.ForeColorPreview_ForeColorChanged);
			// 
			// ForeColorSelect
			// 
			this.ForeColorSelect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.ForeColorSelect.Location = new System.Drawing.Point(342, 105);
			this.ForeColorSelect.Name = "ForeColorSelect";
			this.ForeColorSelect.Size = new System.Drawing.Size(32, 23);
			this.ForeColorSelect.TabIndex = 17;
			this.ForeColorSelect.Text = "...";
			this.ToolTipText.SetToolTip(this.ForeColorSelect, "设置通知对话框的文字颜色。\r\n左栏中的 ■ 为预览。\r\n");
			this.ForeColorSelect.UseVisualStyleBackColor = true;
			this.ForeColorSelect.Click += new System.EventHandler(this.ForeColorSelect_Click);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(444, 23);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(19, 15);
			this.label3.TabIndex = 11;
			this.label3.Text = "秒";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(269, 23);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(83, 15);
			this.label2.TabIndex = 9;
			this.label2.Text = "提前通知 :";
			// 
			// AccelInterval
			// 
			this.AccelInterval.Location = new System.Drawing.Point(358, 18);
			this.AccelInterval.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
			this.AccelInterval.Name = "AccelInterval";
			this.AccelInterval.Size = new System.Drawing.Size(80, 23);
			this.AccelInterval.TabIndex = 10;
			this.AccelInterval.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.ToolTipText.SetToolTip(this.AccelInterval, "设置提前通知的秒数。\r\n远征、入渠的通知推荐设置为提前 60 秒。");
			// 
			// TopMostFlag
			// 
			this.TopMostFlag.AutoSize = true;
			this.TopMostFlag.Location = new System.Drawing.Point(268, 77);
			this.TopMostFlag.Name = "TopMostFlag";
			this.TopMostFlag.Size = new System.Drawing.Size(114, 19);
			this.TopMostFlag.TabIndex = 15;
			this.TopMostFlag.Text = "对话框置顶";
			this.TopMostFlag.UseVisualStyleBackColor = true;
			// 
			// LocationY
			// 
			this.LocationY.Location = new System.Drawing.Point(141, 76);
			this.LocationY.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
			this.LocationY.Minimum = new decimal(new int[] {
            99999,
            0,
            0,
            -2147483648});
			this.LocationY.Name = "LocationY";
			this.LocationY.Size = new System.Drawing.Size(80, 23);
			this.LocationY.TabIndex = 5;
			this.LocationY.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.ToolTipText.SetToolTip(this.LocationY, "设置「位置」为「手动」时对话框弹出的坐标。");
			this.LocationY.Value = new decimal(new int[] {
            99999,
            0,
            0,
            -2147483648});
			// 
			// LocationX
			// 
			this.LocationX.Location = new System.Drawing.Point(55, 76);
			this.LocationX.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
			this.LocationX.Minimum = new decimal(new int[] {
            99999,
            0,
            0,
            -2147483648});
			this.LocationX.Name = "LocationX";
			this.LocationX.Size = new System.Drawing.Size(80, 23);
			this.LocationX.TabIndex = 4;
			this.LocationX.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.ToolTipText.SetToolTip(this.LocationX, "设置「位置」为「手动」时对话框弹出的坐标。");
			this.LocationX.Value = new decimal(new int[] {
            99999,
            0,
            0,
            -2147483648});
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(6, 50);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(43, 15);
			this.label1.TabIndex = 1;
			this.label1.Text = "位置 :";
			// 
			// Alignment
			// 
			this.Alignment.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.Alignment.FormattingEnabled = true;
			this.Alignment.Items.AddRange(new object[] {
            "未设定",
            "左上",
            "上",
            "右上",
            "左",
            "中央",
            "右",
            "左下",
            "下",
            "右下",
            "手动 ( 绝对 )",
            "手动 ( 相对 )"});
			this.Alignment.Location = new System.Drawing.Point(55, 47);
			this.Alignment.Name = "Alignment";
			this.Alignment.Size = new System.Drawing.Size(121, 23);
			this.Alignment.TabIndex = 2;
			this.ToolTipText.SetToolTip(this.Alignment, "设置通知对话框的弹出位置。");
			// 
			// ShowsDialog
			// 
			this.ShowsDialog.AutoSize = true;
			this.ShowsDialog.Location = new System.Drawing.Point(6, 22);
			this.ShowsDialog.Name = "ShowsDialog";
			this.ShowsDialog.Size = new System.Drawing.Size(78, 19);
			this.ShowsDialog.TabIndex = 0;
			this.ShowsDialog.Text = "启用";
			this.ShowsDialog.UseVisualStyleBackColor = true;
			// 
			// GroupDamage
			// 
			this.GroupDamage.Controls.Add(this.NotifiesAtEndpoint);
			this.GroupDamage.Controls.Add(this.ContainsFlagship);
			this.GroupDamage.Controls.Add(this.ContainsSafeShip);
			this.GroupDamage.Controls.Add(this.ContainsNotLockedShip);
			this.GroupDamage.Controls.Add(this.label8);
			this.GroupDamage.Controls.Add(this.LevelBorder);
			this.GroupDamage.Controls.Add(this.NotifiesAfter);
			this.GroupDamage.Controls.Add(this.NotifiesNow);
			this.GroupDamage.Controls.Add(this.NotifiesBefore);
			this.GroupDamage.Location = new System.Drawing.Point(12, 298);
			this.GroupDamage.Name = "GroupDamage";
			this.GroupDamage.Size = new System.Drawing.Size(602, 103);
			this.GroupDamage.TabIndex = 4;
			this.GroupDamage.TabStop = false;
			this.GroupDamage.Text = "大破警告";
			// 
			// NotifiesAtEndpoint
			// 
			this.NotifiesAtEndpoint.AutoSize = true;
			this.NotifiesAtEndpoint.Location = new System.Drawing.Point(292, 22);
			this.NotifiesAtEndpoint.Name = "NotifiesAtEndpoint";
			this.NotifiesAtEndpoint.Size = new System.Drawing.Size(112, 19);
			this.NotifiesAtEndpoint.TabIndex = 6;
			this.NotifiesAtEndpoint.Text = "在终点也通知";
			this.ToolTipText.SetToolTip(this.NotifiesAtEndpoint, "是否在地图终点也通知。");
			this.NotifiesAtEndpoint.UseVisualStyleBackColor = true;
			// 
			// ContainsFlagship
			// 
			this.ContainsFlagship.AutoSize = true;
			this.ContainsFlagship.Location = new System.Drawing.Point(146, 72);
			this.ContainsFlagship.Name = "ContainsFlagship";
			this.ContainsFlagship.Size = new System.Drawing.Size(90, 19);
			this.ContainsFlagship.TabIndex = 5;
			this.ContainsFlagship.Text = "包含旗舰";
			this.ContainsFlagship.UseVisualStyleBackColor = true;
			// 
			// ContainsSafeShip
			// 
			this.ContainsSafeShip.AutoSize = true;
			this.ContainsSafeShip.Location = new System.Drawing.Point(146, 47);
			this.ContainsSafeShip.Name = "ContainsSafeShip";
			this.ContainsSafeShip.Size = new System.Drawing.Size(136, 19);
			this.ContainsSafeShip.TabIndex = 4;
			this.ContainsSafeShip.Text = "包含带损管舰";
			this.ContainsSafeShip.UseVisualStyleBackColor = true;
			// 
			// ContainsNotLockedShip
			// 
			this.ContainsNotLockedShip.AutoSize = true;
			this.ContainsNotLockedShip.Location = new System.Drawing.Point(146, 22);
			this.ContainsNotLockedShip.Name = "ContainsNotLockedShip";
			this.ContainsNotLockedShip.Size = new System.Drawing.Size(114, 19);
			this.ContainsNotLockedShip.TabIndex = 3;
			this.ContainsNotLockedShip.Text = "包含未锁定舰";
			this.ContainsNotLockedShip.UseVisualStyleBackColor = true;
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(289, 48);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(56, 15);
			this.label8.TabIndex = 7;
			this.label8.Text = "最低等级 :";
			// 
			// LevelBorder
			// 
			this.LevelBorder.Location = new System.Drawing.Point(351, 46);
			this.LevelBorder.Maximum = new decimal(new int[] {
            150,
            0,
            0,
            0});
			this.LevelBorder.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.LevelBorder.Name = "LevelBorder";
			this.LevelBorder.Size = new System.Drawing.Size(80, 23);
			this.LevelBorder.TabIndex = 8;
			this.LevelBorder.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.ToolTipText.SetToolTip(this.LevelBorder, "设定大破通知舰的最低等级。\r\n请注意，调整该值将导致低等级舰大破不被通知。");
			this.LevelBorder.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			// 
			// NotifiesAfter
			// 
			this.NotifiesAfter.AutoSize = true;
			this.NotifiesAfter.Location = new System.Drawing.Point(6, 72);
			this.NotifiesAfter.Name = "NotifiesAfter";
			this.NotifiesAfter.Size = new System.Drawing.Size(135, 19);
			this.NotifiesAfter.TabIndex = 2;
			this.NotifiesAfter.Text = "启用事后通知";
			this.ToolTipText.SetToolTip(this.NotifiesAfter, "大破进击中通知。");
			this.NotifiesAfter.UseVisualStyleBackColor = true;
			// 
			// NotifiesNow
			// 
			this.NotifiesNow.AutoSize = true;
			this.NotifiesNow.Location = new System.Drawing.Point(6, 47);
			this.NotifiesNow.Name = "NotifiesNow";
			this.NotifiesNow.Size = new System.Drawing.Size(135, 19);
			this.NotifiesNow.TabIndex = 1;
			this.NotifiesNow.Text = "启用事中通知";
			this.ToolTipText.SetToolTip(this.NotifiesNow, "出击前以及战斗结束时通知。");
			this.NotifiesNow.UseVisualStyleBackColor = true;
			// 
			// NotifiesBefore
			// 
			this.NotifiesBefore.AutoSize = true;
			this.NotifiesBefore.Location = new System.Drawing.Point(6, 22);
			this.NotifiesBefore.Name = "NotifiesBefore";
			this.NotifiesBefore.Size = new System.Drawing.Size(135, 19);
			this.NotifiesBefore.TabIndex = 0;
			this.NotifiesBefore.Text = "启用事先通知";
			this.ToolTipText.SetToolTip(this.NotifiesBefore, "出击前以及战斗结束之后立即通知。");
			this.NotifiesBefore.UseVisualStyleBackColor = true;
			// 
			// DialogColor
			// 
			this.DialogColor.AnyColor = true;
			this.DialogColor.FullOpen = true;
			// 
			// DialogOpenSound
			// 
			this.DialogOpenSound.Filter = "Wave|*.wav|File|*";
			this.DialogOpenSound.Title = "音声ファイルを開く";
			// 
			// DialogOpenImage
			// 
			this.DialogOpenImage.Filter = "Image|*.bmp;*.div;*.jpg;*.jpeg;*.jpe;*.jfif;*.gif;*.png;*.tif;*.tiff|BMP|*.bmp;*." +
    "div|JPEG|*.jpg;*.jpeg;*.jpe;*.jfif|GIF|*.gif|PNG|*.png|TIFF|*.tif;*.tiff|File|*";
			this.DialogOpenImage.Title = "画像ファイルを開く";
			// 
			// ToolTipText
			// 
			this.ToolTipText.AutoPopDelay = 30000;
			this.ToolTipText.InitialDelay = 500;
			this.ToolTipText.ReshowDelay = 100;
			// 
			// label10
			// 
			this.label10.AutoSize = true;
			this.label10.Location = new System.Drawing.Point(93, 411);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(213, 15);
			this.label10.TabIndex = 8;
			this.label10.Text = "＊测试时会应用设置";
			// 
			// GroupAnchorageRepair
			// 
			this.GroupAnchorageRepair.Controls.Add(this.AnchorageRepairNotificationLevel);
			this.GroupAnchorageRepair.Controls.Add(this.label11);
			this.GroupAnchorageRepair.Location = new System.Drawing.Point(12, 298);
			this.GroupAnchorageRepair.Name = "GroupAnchorageRepair";
			this.GroupAnchorageRepair.Size = new System.Drawing.Size(602, 103);
			this.GroupAnchorageRepair.TabIndex = 9;
			this.GroupAnchorageRepair.TabStop = false;
			this.GroupAnchorageRepair.Text = "泊地修理设置";
			// 
			// label11
			// 
			this.label11.AutoSize = true;
			this.label11.Location = new System.Drawing.Point(7, 22);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(67, 15);
			this.label11.TabIndex = 0;
			this.label11.Text = "通知条件 :";
			// 
			// AnchorageRepairNotificationLevel
			// 
			this.AnchorageRepairNotificationLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.AnchorageRepairNotificationLevel.FormattingEnabled = true;
			this.AnchorageRepairNotificationLevel.Items.AddRange(new object[] {
            "始终通知",
            "明石旗舰时通知",
            "有舰在泊地修理中通知"});
			this.AnchorageRepairNotificationLevel.Location = new System.Drawing.Point(80, 20);
			this.AnchorageRepairNotificationLevel.Name = "AnchorageRepairNotificationLevel";
			this.AnchorageRepairNotificationLevel.Size = new System.Drawing.Size(160, 23);
			this.AnchorageRepairNotificationLevel.TabIndex = 1;
			this.ToolTipText.SetToolTip(this.AnchorageRepairNotificationLevel, "始终通知：每 20 分通知。\r\n明石旗舰时通知：满足上述条件，同时明石为旗舰时才通知。\r\n有舰在泊地修理中通知：满足上述条件，同时实际有舰在修理时才通知。");
			// 
			// DialogConfigurationNotifier
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(624, 442);
			this.Controls.Add(this.GroupAnchorageRepair);
			this.Controls.Add(this.label10);
			this.Controls.Add(this.GroupDamage);
			this.Controls.Add(this.GroupDialog);
			this.Controls.Add(this.GroupImage);
			this.Controls.Add(this.IsEnabled);
			this.Controls.Add(this.ButtonTest);
			this.Controls.Add(this.GroupSound);
			this.Controls.Add(this.ButtonCancel);
			this.Controls.Add(this.ButtonOK);
			this.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "DialogConfigurationNotifier";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "通知设置";
			this.Load += new System.EventHandler(this.DialogConfigurationNotifier_Load);
			this.GroupSound.ResumeLayout(false);
			this.GroupSound.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.SoundVolume)).EndInit();
			this.GroupImage.ResumeLayout(false);
			this.GroupImage.PerformLayout();
			this.GroupDialog.ResumeLayout(false);
			this.GroupDialog.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.ClosingInterval)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.AccelInterval)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.LocationY)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.LocationX)).EndInit();
			this.GroupDamage.ResumeLayout(false);
			this.GroupDamage.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.LevelBorder)).EndInit();
			this.GroupAnchorageRepair.ResumeLayout(false);
			this.GroupAnchorageRepair.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button ButtonCancel;
		private System.Windows.Forms.Button ButtonOK;
		private System.Windows.Forms.GroupBox GroupSound;
		private System.Windows.Forms.Button ButtonTest;
		private System.Windows.Forms.CheckBox PlaysSound;
		private System.Windows.Forms.Button SoundPathSearch;
		internal System.Windows.Forms.TextBox SoundPath;
		private System.Windows.Forms.CheckBox IsEnabled;
		private System.Windows.Forms.GroupBox GroupImage;
		private System.Windows.Forms.CheckBox DrawsImage;
		private System.Windows.Forms.Button ImagePathSearch;
		internal System.Windows.Forms.TextBox ImagePath;
		private System.Windows.Forms.GroupBox GroupDialog;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.NumericUpDown AccelInterval;
		private System.Windows.Forms.CheckBox TopMostFlag;
		private System.Windows.Forms.NumericUpDown LocationY;
		private System.Windows.Forms.NumericUpDown LocationX;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox Alignment;
		private System.Windows.Forms.CheckBox ShowsDialog;
		private System.Windows.Forms.Label BackColorPreview;
		private System.Windows.Forms.Button BackColorSelect;
		private System.Windows.Forms.Label ForeColorPreview;
		private System.Windows.Forms.Button ForeColorSelect;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.NumericUpDown ClosingInterval;
		private System.Windows.Forms.CheckBox HasFormBorder;
		private System.Windows.Forms.GroupBox GroupDamage;
		private System.Windows.Forms.CheckBox NotifiesAfter;
		private System.Windows.Forms.CheckBox NotifiesNow;
		private System.Windows.Forms.CheckBox NotifiesBefore;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.NumericUpDown LevelBorder;
		private System.Windows.Forms.CheckBox ContainsSafeShip;
		private System.Windows.Forms.CheckBox ContainsNotLockedShip;
		private System.Windows.Forms.CheckBox ContainsFlagship;
		private System.Windows.Forms.CheckBox DrawsMessage;
		private System.Windows.Forms.ColorDialog DialogColor;
		private System.Windows.Forms.OpenFileDialog DialogOpenSound;
		private System.Windows.Forms.OpenFileDialog DialogOpenImage;
		private System.Windows.Forms.CheckBox NotifiesAtEndpoint;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.ToolTip ToolTipText;
		private System.Windows.Forms.CheckBox ShowWithActivation;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.CheckedListBox CloseList;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.NumericUpDown SoundVolume;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.CheckBox LoopsSound;
		private System.Windows.Forms.Button SoundPathDirectorize;
		private System.Windows.Forms.GroupBox GroupAnchorageRepair;
		private System.Windows.Forms.ComboBox AnchorageRepairNotificationLevel;
		private System.Windows.Forms.Label label11;
	}
}
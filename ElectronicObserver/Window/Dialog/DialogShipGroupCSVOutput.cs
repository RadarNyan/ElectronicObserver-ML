using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ElectronicObserver.Window.Dialog
{
	public partial class DialogShipGroupCSVOutput : Form
	{

		/// <summary>
		/// 出力フィルタを指定します。
		/// </summary>
		public enum FilterModeConstants
		{

			/// <summary>全て出力</summary>
			All,

			/// <summary>表示されている行のみ出力</summary>
			VisibleColumnOnly,
		}

		/// <summary>
		/// 出力フォーマットを指定します。
		/// </summary>
		public enum OutputFormatConstants
		{

			/// <summary>閲覧用</summary>
			User,

			/// <summary>データ用</summary>
			Data,
		}


		/// <summary>
		/// 出力ファイルのパス
		/// </summary>
		public string OutputPath
		{
			get { return TextOutputPath.Text; }
			set { TextOutputPath.Text = value; }
		}

		/// <summary>
		/// 出力フィルタ
		/// </summary>
		public FilterModeConstants FilterMode
		{
			get
			{
				if (RadioOutput_All.Checked)
					return FilterModeConstants.All;
				else
					return FilterModeConstants.VisibleColumnOnly;
			}
			set
			{
				switch (value)
				{
					case FilterModeConstants.All:
						RadioOutput_All.Checked = true; break;

					case FilterModeConstants.VisibleColumnOnly:
						RadioOutput_VisibleColumnOnly.Checked = true; break;
				}
			}
		}

		/// <summary>
		/// 出力フォーマット
		/// </summary>
		public OutputFormatConstants OutputFormat
		{
			get
			{
				if (RadioFormat_User.Checked)
					return OutputFormatConstants.User;
				else
					return OutputFormatConstants.Data;
			}
			set
			{
				switch (value)
				{
					case OutputFormatConstants.User:
						RadioFormat_User.Checked = true; break;

					case OutputFormatConstants.Data:
						RadioFormat_Data.Checked = true; break;
				}
			}
		}



		public DialogShipGroupCSVOutput()
		{
			InitializeComponent();

			#region UI translation
			switch (Utility.Configuration.Config.UI.Language) {
				case "zh":
					groupBox1.Text = "选项";
					RadioFormat_Data.Text = "原数据";
					RadioFormat_User.Text = "可读数据";
					RadioOutput_VisibleColumnOnly.Text = "仅导出可见列"; // unused?
					RadioOutput_All.Text = "导出所有列"; // unused?
					ButtonOK.Text = "确定";
					ButtonCancel.Text = "取消";
					groupBox2.Text = "输出目录";
					Text = "导出 .CSV 文件";
					DialogSaveCSV.Title = "保存 .CSV 文件";
					DialogSaveCSV.Filter = "CSV|*.csv|File|*";
					break;
				case "en":
					groupBox1.Text = "Options";
					RadioFormat_Data.Text = "Raw Data";
					RadioFormat_User.Text = "Human-readable";
					RadioOutput_VisibleColumnOnly.Text = "Export Visible Columns Only"; // unused?
					RadioOutput_All.Text = "Export All Columns"; // unused?
					ButtonOK.Text = "OK";
					ButtonCancel.Text = "Cancel";
					groupBox2.Text = "Output Path";
					Text = "Export as .CSV File";
					DialogSaveCSV.Title = "Save .CSV File";
					DialogSaveCSV.Filter = "CSV|*.csv|File|*";
					break;
				default:
					break;
			}
			#endregion

			Font = Utility.Configuration.Config.UI.MainFont;

			DialogSaveCSV.InitialDirectory = Utility.Configuration.Config.Connection.SaveDataPath;

		}

		private void DialogShipGroupCSVOutput_Load(object sender, EventArgs e)
		{


		}

		private void ButtonOutputPathSearch_Click(object sender, EventArgs e)
		{

			if (DialogSaveCSV.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{

				TextOutputPath.Text = DialogSaveCSV.FileName;

			}

			DialogSaveCSV.InitialDirectory = null;

		}

		private void ButtonOK_Click(object sender, EventArgs e)
		{
			DialogResult = System.Windows.Forms.DialogResult.OK;
		}

		private void ButtonCancel_Click(object sender, EventArgs e)
		{
			DialogResult = System.Windows.Forms.DialogResult.Cancel;
		}

	}
}

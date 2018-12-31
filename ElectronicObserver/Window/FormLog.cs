using ElectronicObserver.Resource;
using ElectronicObserver.Window.Control;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace ElectronicObserver.Window
{

	public partial class FormLog : DockContent
	{

		private MultiLangRichTextBox LogText;

		private string UILanguage;

		public FormLog(FormMain parent)
		{
			InitializeComponent();
			Controls.Remove(LogList);
			LogList.Dispose();
			LogText = new MultiLangRichTextBox();
			LogText.ContextMenuStrip = ContextMenuLog;
			Controls.Add(LogText);
			LogText.ForeColor = ForeColor = parent.ForeColor;
			LogText.BackColor = BackColor = parent.BackColor;

			UILanguage = parent.UILanguage;

			switch (UILanguage) {
				case "zh":
					ContextMenuLog_Clear.Text = "清空(&C)";
					Text = "日志";
					break;
				case "en":
					ContextMenuLog_Clear.Text = "&Clear";
					Text = "Log";
					break;
				default:
					break;
			}
		}

		private void FormLog_Load(object sender, EventArgs e)
		{

			foreach (var log in Utility.Logger.Log)
			{
				if (log.Priority >= Utility.Configuration.Config.Log.LogLevel)
					LogText.CreateLogLine(log);
			}

			Utility.Logger.Instance.LogAdded += new Utility.LogAddedEventHandler((Utility.Logger.LogData data) =>
			{
				if (InvokeRequired)
				{
					// Invokeはメッセージキューにジョブを投げて待つので、別のBeginInvokeされたジョブが既にキューにあると、
					// それを実行してしまい、BeginInvokeされたジョブの順番が保てなくなる
					// GUIスレッドによる処理は、順番が重要なことがあるので、GUIスレッドからInvokeを呼び出してはいけない
					Invoke(new Utility.LogAddedEventHandler(Logger_LogAdded), data);
				}
				else
				{
					Logger_LogAdded(data);
				}
			});

			Icon = ResourceManager.ImageToIcon(ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormLog]);
		}


		void Logger_LogAdded(Utility.Logger.LogData data)
		{
			LogText.CreateLogLine(data);
		}



		private void ContextMenuLog_Clear_Click(object sender, EventArgs e)
		{
			LogText.Clear();
		}



		protected override string GetPersistString()
		{
			return "Log";
		}


	}
}

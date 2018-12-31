using ElectronicObserver.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ElectronicObserver.Window.Control
{
	class MultiLangRichTextBox : RichTextBox
	{
		public MultiLangRichTextBox()
		{
			BorderStyle = BorderStyle.None;
			Dock = DockStyle.Fill;
			ReadOnly = true;
			DetectUrls = false;
			HideSelection = false; // auto scroll
			LanguageOption = RichTextBoxLanguageOptions.UIFonts;
		}

		public void CreateLogLine(Logger.LogData data)
		{
			SelectionFont = Configuration.Config.UI.MainFontJA;
			if (Configuration.Config.UI.LogReversed) {
				SelectionStart = 0;
				InputText($"{data}\r\n");
			} else {
				SelectionStart = TextLength;
				if (TextLength != 0)
					AppendText("\r\n");
				InputText(data.ToString());
			}
		}

		private void InputText(string text)
		{
			SuspendDrawing();
			ClearOldLines();
			string[] parts = Regex.Split(text, @"\[(zh|en|ja)\]");
			SelectionFont = Configuration.Config.UI.MainFont;
			for (int i = 0; i < parts.Length; i++){
				switch (parts[i]) {
					case "zh":
					case "en":
						SelectionFont = Configuration.Config.UI.MainFont;
						continue;
					case "ja":
						SelectionFont = Configuration.Config.UI.MainFontJA;
						continue;
					default:
						SelectedText = parts[i];
						continue;
				}
			}
			ResumeDrawing();
		}

		/// <summary>
		/// Remove 100 old lines once reaches 500 lines, while preserving formatting (font)
		/// </summary>
		private void ClearOldLines()
		{
			if (Lines.Length >= 500) {
				ReadOnly = false; // prevent beeping when set SelectedText = ""
				if (Configuration.Config.UI.LogReversed) {
					Select(GetFirstCharIndexFromLine(400), TextLength);
					SelectedText = "";
					Select(0, 0);
				} else {
					Select(0, GetFirstCharIndexFromLine(100));
					SelectedText = "";
					Select(TextLength, 0);
				}
				ReadOnly = true;
			}
		}

		public void SetText(string text)
		{
			Text = "";
			InputText(text.Replace("\r", "").Replace("\n", " "));
		}

		#region HideCaret
		[System.Runtime.InteropServices.DllImport("user32.dll")]
		static extern bool HideCaret(IntPtr hWnd);

		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);
			HideCaret(Handle);
		}

		protected override void OnEnter(EventArgs e)
		{
			base.OnEnter(e);
			HideCaret(Handle);
		}

		protected override void OnGotFocus(EventArgs e)
		{
			base.OnGotFocus(e);
			HideCaret(Handle);
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			if (e.Button == MouseButtons.Left)
				HideCaret(this.Handle);
		}

		public new void Clear()
		{
			base.Clear();
			HideCaret(Handle);
		}
		#endregion

		#region SuspendDrawing
		[System.Runtime.InteropServices.DllImport("user32.dll")]
		public static extern int SendMessage(IntPtr hWnd, Int32 wMsg, bool wParam, Int32 lParam);
		private const int WM_SETREDRAW = 11;

		private void SuspendDrawing()
		{
			SendMessage(Handle, WM_SETREDRAW, false, 0);
		}

		private void ResumeDrawing()
		{
			SendMessage(Handle, WM_SETREDRAW, true, 0);
			Invalidate(true);
			Update();
		}
		#endregion
	}
}

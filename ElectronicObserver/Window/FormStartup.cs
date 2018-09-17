using System;
using System.Windows.Forms;

namespace ElectronicObserver.Window
{
	public partial class FormStartup : Form
	{
		public string Language;

		public FormStartup()
		{
			InitializeComponent();
		}

		private void FormStartup_Load(object sender, EventArgs e)
		{
			// Set default language by OS settings
			string sysLanguage = System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
			switch (sysLanguage) {
				case "zh":
					radioButton1.Checked = true;
					break;
				case "ja":
					radioButton2.Checked = true;
					break;
				case "en":
					radioButton3.Checked = true;
					break;
				default:
					break;
			}
		}

		private void RadioButtons_CheckedChanged(object sender, EventArgs e)
		{
			if (radioButton1.Checked) {
				Language = "zh";
				RunButton.Text = "启动";
				RunButton.Font = radioButton1.Font;
			} else if (radioButton2.Checked) {
				Language = "ja";
				RunButton.Text = "起動";
				RunButton.Font = radioButton2.Font;
			} else if (radioButton3.Checked) {
				Language = "en";
				RunButton.Text = "Run";
				RunButton.Font = radioButton3.Font;
			}
		}

		private void RunButton_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.OK;
		}
	}
}

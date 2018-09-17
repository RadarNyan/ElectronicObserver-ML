using ElectronicObserver.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.IO;

namespace ElectronicObserver
{
	static class Program
	{
		/// <summary>
		/// アプリケーションのメイン エントリ ポイントです。
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			ReadLanguageConfigFile();
			if (String.IsNullOrEmpty(Language)) {
				using (var formStartup = new FormStartup()) {
					var result = formStartup.ShowDialog();
					if (result != DialogResult.OK)
						return;
					Language = formStartup.Language;
				}
			}
			WriteLanguageConfigFile();

			bool allowMultiInstance = args.Contains("-m") || args.Contains("--multi-instance");

			using (var mutex = new Mutex(false, Application.ExecutablePath.Replace('\\', '/'), out var created))
			{

				/*
				bool hasHandle = false;

				try
				{
					hasHandle = mutex.WaitOne(0, false);
				}
				catch (AbandonedMutexException)
				{
					hasHandle = true;
				}
				*/

				if (!created && !allowMultiInstance)
				{
					// 多重起動禁止
					switch (Language) {
						case "zh":
							MessageBox.Show(
								"已在运行中，不支持多重启动。\r\n" +
								"如需多重启动，请添加命令行开关：-m",
								"七四式电子观测仪", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
							break;
						case "en":
							MessageBox.Show(
								"ElectronicObserver is already running, running multiple instances is unsupported.\r\n" +
								"If you insist, please restart this program with command line switch: -m",
								"ElectronicObserver", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
							break;
						default:
							MessageBox.Show(
								"既に起動しています。多重起動はできません。\r\n" +
								"誤検出の場合は、コマンドラインから -m オプションを付けて起動してください。",
								"七四式電子観測儀", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
							break;
					}
					return;
				}

				Application.Run(new FormMain(Language));
			}
		}

		static string Language;
		static string languageConfigFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Settings\UIlanguage");

		static void ReadLanguageConfigFile()
		{
			try {
				string language = "";
				using (StreamReader sr = File.OpenText(languageConfigFile)) {
					language = sr.ReadToEnd();
				}
				switch (language) {
					case "zh":
					case "en":
					case "ja":
						Language = language;
						break;
					default:
						break;
				}
			}
			catch {
			}
		}

		static void WriteLanguageConfigFile()
		{
			try {
				Directory.CreateDirectory(Path.GetDirectoryName(languageConfigFile));
				using (StreamWriter sw = new StreamWriter(languageConfigFile)) {
					sw.Write(Language);
				}
			}
			catch {
			}
		}
	}
}

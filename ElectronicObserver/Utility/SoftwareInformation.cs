using ElectronicObserver.Utility.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ElectronicObserver.Utility
{
	/// <summary>
	/// ソフトウェアの情報を保持します。
	/// </summary>
	public static class SoftwareInformation
	{
		/// <summary>
		/// ソフトウェア名(日本語)
		/// </summary>
		public static string SoftwareNameJapanese => "七四式電子観測儀";

		/// <summary>
		/// ソフトウェア名(中国語)
		/// </summary>
		public static string SoftwareNameChinese => "七四式电子观测仪";

		/// <summary>
		/// ソフトウェア名(英語)
		/// </summary>
		public static string SoftwareNameEnglish => "ElectronicObserver";

		/// <summary>
		/// バージョン(日本語, ソフトウェア名を含みます)
		/// </summary>
		public static string VersionJapanese => SoftwareNameJapanese + "四〇型改二";

		/// <summary>
		/// バージョン(中国語, ソフトウェア名を含みます)
		/// </summary>
		public static string VersionChinese => $"{SoftwareNameChinese}四〇型改二";

		/// <summary>
		/// バージョン(英語)
		/// </summary>
		public static string VersionEnglish => "4.0.2";

		/// <summary>
		/// 更新日時
		/// </summary>
		public static DateTime UpdateTime => DateTimeHelper.CSVStringToTime("2018/09/17 10:00:00");

		private static System.Net.WebClient client;
		private static readonly Uri uri = new Uri("https://raw.githubusercontent.com/RadarNyan/ElectronicObserver-ML/master/VERSION");

		public static void CheckUpdate()
		{
			if (!Configuration.Config.Life.CheckUpdateInformation)
				return;

			if (client == null) {
				client = new System.Net.WebClient {
					Encoding = new UTF8Encoding(false) // UTF-8 w/o BOM
				};
				client.DownloadStringCompleted += DownloadStringCompleted;
			}

			if (!client.IsBusy)
				client.DownloadStringAsync(uri);
		}

		private static void DownloadStringCompleted(object sender, System.Net.DownloadStringCompletedEventArgs e)
		{
			string UILanguage = Configuration.Config.UI.Language;

			if (e.Error != null) {
				switch (UILanguage) {
					case "zh":
						ErrorReporter.SendErrorReport(e.Error, "获取更新信息失败。");
						return;
					case "en":
						ErrorReporter.SendErrorReport(e.Error, "Failed to retrieve update information.");
						return;
					default:
						ErrorReporter.SendErrorReport(e.Error, "アップデート情報の取得に失敗しました。");
						return;
				}
			}

			try {
				using (var sr = new System.IO.StringReader(e.Result)) {
					DateTime date = DateTimeHelper.CSVStringToTime(sr.ReadLine());
					if (UpdateTime < date) {
						string version = sr.ReadLine();

						var changelog = new StringBuilder();
						string changelog_line;
						while ((changelog_line = sr.ReadLine()) != null) {
							if (changelog_line.StartsWith($"[{UILanguage}]"))
								break;
						}
						while ((changelog_line = sr.ReadLine()) != null) {
							if (System.Text.RegularExpressions.Regex.IsMatch(changelog_line, @"^\[[a-z]{2}\]")) {
								break;
							} else {
								changelog.Append(changelog_line);
							}
						}

						DialogResult result;
						switch (UILanguage) {
							case "zh":
								Logger.Add(3, "发现新版本：" + version);
								result = MessageBox.Show(
									$"发现新版本：{version}\r\n" +
									$"更新内容：\r\n" +
									$"{changelog}\r\n" +
									$"\r\n" +
									$"要打开发布页吗？\r\n" +
									$"（选择取消将不再自动检查更新）",
									"更新信息",
									MessageBoxButtons.YesNoCancel,
									MessageBoxIcon.Information,
									MessageBoxDefaultButton.Button1);
								break;
							case "en":
								Logger.Add(3, "A new version has been released: " + version);
								result = MessageBox.Show(
									$"A new version has been released: {version}\r\n" +
									$"Change Log: \r\n" +
									$"{changelog}\r\n" +
									$"\r\n" +
									$"Do you want to open the download page?\r\n" +
									$"(Choose cancel will disable update check)",
									"Update Information",
									MessageBoxButtons.YesNoCancel,
									MessageBoxIcon.Information,
									MessageBoxDefaultButton.Button1);
								break;
							default:
								Logger.Add(3, "新しいバージョンがリリースされています！ : " + version);
								result = MessageBox.Show(
									$"新しいバージョンがリリースされています！ : {version}\r\n" +
									$"更新内容 : \r\n" +
									$"{changelog}\r\n" +
									$"\r\n" +
									$"ダウンロードページを開きますか？\r\n" +
									$"(キャンセルすると以降表示しません)",
									"アップデート情報",
									MessageBoxButtons.YesNoCancel,
									MessageBoxIcon.Information,
									MessageBoxDefaultButton.Button1);
								break;
						}
						if (result == DialogResult.Yes) {
							System.Diagnostics.Process.Start("https://github.com/RadarNyan/ElectronicObserver-ML/releases");
						} else if (result == DialogResult.Cancel) {
							Configuration.Config.Life.CheckUpdateInformation = false;
						}
					} else {
						switch (UILanguage) {
							case "zh":
								Logger.Add(1, "已经是最新版。");
								break;
							case "en":
								Logger.Add(1, "You're using the latest version.");
								break;
							default:
								Logger.Add(1, "お使いのバージョンは最新です。");
								break;
						}
					}
				}
			}
			catch (Exception ex) {
				switch (UILanguage) {
					case "zh":
						ErrorReporter.SendErrorReport(ex, "解析更新信息失败。");
						break;
					case "en":
						ErrorReporter.SendErrorReport(ex, "Failed to parse update information.");
						break;
					default:
						ErrorReporter.SendErrorReport(ex, "アップデート情報の処理に失敗しました。");
						break;
				}
			}
		}
	}
}

using ElectronicObserver.Data;
using ElectronicObserver.Observer;
using ElectronicObserver.Resource;
using ElectronicObserver.Utility;
using ElectronicObserver.Window.Support;
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

	public partial class FormQuest : DockContent
	{

		private DataGridViewCellStyle CSDefaultLeft, CSDefaultCenter;
		private DataGridViewCellStyle[] CSCategories;
		private bool IsLoaded = false;

		private string UILanguage;

		public FormQuest(FormMain parent)
		{
			InitializeComponent();

			UILanguage = parent.UILanguage;

			#region UI translation
			switch (UILanguage) {
				case "zh":
					QuestView_Type.HeaderText = "种";
					QuestView_Category.HeaderText = "类别";
					QuestView_Name.HeaderText = "任务名";
					QuestView_Progress.HeaderText = "进度";
					MenuProgress_Increment.Text = "进度 +1(&I)";
					MenuProgress_Decrement.Text = "进度 -1(&D)";
					MenuProgress_Reset.Text = "重置进度(&R)";
					MenuMain_ColumnFilter.Text = "显示列(&C)";
					MenuMain_ColumnFilter_State.Text = "进行中(&S)";
					MenuMain_ColumnFilter_Type.Text = "种(&T)";
					MenuMain_ColumnFilter_Category.Text = "类别(&C)";
					MenuMain_ColumnFilter_Name.Text = "任务名(&N)";
					MenuMain_ColumnFilter_Progress.Text = "进度(&P)";
					MenuMain_Initialize.Text = "初始化(&I)";
					MenuMain_QuestFilter.Text = "显示过滤器(&Q)";
					MenuMain_ShowRunningOnly.Text = "只显示进行中任务(&R)";
					MenuMain_ShowOnce.Text = "显示单次任务(&O)";
					MenuMain_ShowDaily.Text = "显示日常(&D)";
					MenuMain_ShowWeekly.Text = "显示周常(&W)";
					MenuMain_ShowMonthly.Text = "显示月常(&M)";
					MenuMain_ShowOther.Text = "显示其它任务(&R)";
					Text = "任务";
					break;
				case "en":
					QuestView_Type.HeaderText = "Type";
					QuestView_Category.HeaderText = "Category";
					QuestView_Name.HeaderText = "Name";
					QuestView_Progress.HeaderText = "Progress";
					MenuProgress_Increment.Text = "&Increase Progress (+1)";
					MenuProgress_Decrement.Text = "&Decrease Progress (-1)";
					MenuProgress_Reset.Text = "&Reset Progress";
					MenuMain_ColumnFilter.Text = "&Column Visibility";
					MenuMain_ColumnFilter_State.Text = "&State";
					MenuMain_ColumnFilter_Type.Text = "&Type";
					MenuMain_ColumnFilter_Category.Text = "&Category";
					MenuMain_ColumnFilter_Name.Text = "&Name";
					MenuMain_ColumnFilter_Progress.Text = "&Progress";
					MenuMain_Initialize.Text = "&Initialize";
					MenuMain_QuestFilter.Text = "&Quest Filter";
					MenuMain_ShowRunningOnly.Text = "Show &Active Quests Only";
					MenuMain_ShowOnce.Text = "Show &One-time Quests";
					MenuMain_ShowDaily.Text = "Show &Daily Quests";
					MenuMain_ShowWeekly.Text = "Show &Weekly Quests";
					MenuMain_ShowMonthly.Text = "Show &Monthly Quests";
					MenuMain_ShowOther.Text = "Show Othe&r Quests";
					Text = "Quests";
					break;
				default:
					break;
			}
			#endregion

			ControlHelper.SetDoubleBuffered(QuestView);

			ConfigurationChanged();


			#region set cellstyle

			CSDefaultLeft = new DataGridViewCellStyle
			{
				Alignment = DataGridViewContentAlignment.MiddleLeft
			};
			CSDefaultLeft.BackColor =
			CSDefaultLeft.SelectionBackColor = SystemColors.Control;
			CSDefaultLeft.ForeColor = SystemColors.ControlText;
			CSDefaultLeft.SelectionForeColor = SystemColors.ControlText;
			CSDefaultLeft.WrapMode = DataGridViewTriState.False;

			CSDefaultCenter = new DataGridViewCellStyle(CSDefaultLeft)
			{
				Alignment = DataGridViewContentAlignment.MiddleCenter
			};

			CSCategories = new DataGridViewCellStyle[9];
			for (int i = 0; i < CSCategories.Length; i++)
			{
				CSCategories[i] = new DataGridViewCellStyle(CSDefaultCenter);

				Color c;
				switch (i + 1)
				{
					case 1:     //編成
						c = Color.FromArgb(0xAA, 0xFF, 0xAA);
						break;
					case 2:     //出撃
						c = Color.FromArgb(0xFF, 0xCC, 0xCC);
						break;
					case 3:     //演習
						c = Color.FromArgb(0xDD, 0xFF, 0xAA);
						break;
					case 4:     //遠征
						c = Color.FromArgb(0xCC, 0xFF, 0xFF);
						break;
					case 5:     //補給/入渠
						c = Color.FromArgb(0xFF, 0xFF, 0xCC);
						break;
					case 6:     //工廠
						c = Color.FromArgb(0xDD, 0xCC, 0xBB);
						break;
					case 7:     //改装
						c = Color.FromArgb(0xDD, 0xCC, 0xFF);
						break;
					case 8:     //出撃(2)
						c = Color.FromArgb(0xFF, 0xCC, 0xCC);
						break;
					case 9:     //その他
					default:
						c = CSDefaultCenter.BackColor;
						break;
				}

				CSCategories[i].BackColor =
				CSCategories[i].SelectionBackColor = c;
			}

			QuestView.DefaultCellStyle = CSDefaultCenter;
			QuestView_Category.DefaultCellStyle = CSCategories[CSCategories.Length - 1];
			QuestView_Name.DefaultCellStyle = CSDefaultLeft;
			QuestView_Progress.DefaultCellStyle = CSDefaultLeft;

			#endregion


			SystemEvents.SystemShuttingDown += SystemEvents_SystemShuttingDown;
		}



		private void FormQuest_Load(object sender, EventArgs e)
		{

			/*/
			APIObserver o = APIObserver.Instance;

			APIReceivedEventHandler rec = ( string apiname, dynamic data ) => Invoke( new APIReceivedEventHandler( APIUpdated ), apiname, data );

			o.APIList["api_req_quest/clearitemget"].RequestReceived += rec;

			o.APIList["api_get_member/questlist"].ResponseReceived += rec;
			//*/

			KCDatabase.Instance.Quest.QuestUpdated += Updated;


			ClearQuestView();

			try
			{
				int sort = Utility.Configuration.Config.FormQuest.SortParameter;

				QuestView.Sort(QuestView.Columns[sort >> 1], (sort & 1) == 0 ? ListSortDirection.Ascending : ListSortDirection.Descending);

			}
			catch (Exception)
			{

				QuestView.Sort(QuestView_Name, ListSortDirection.Ascending);
			}


			Utility.Configuration.Instance.ConfigurationChanged += ConfigurationChanged;

			Icon = ResourceManager.ImageToIcon(ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormQuest]);

			IsLoaded = true;
		}


		void ConfigurationChanged()
		{

			var c = Utility.Configuration.Config;

			QuestView.Font = Font = c.UI.MainFont;

			MenuMain_ShowRunningOnly.Checked = c.FormQuest.ShowRunningOnly;
			MenuMain_ShowOnce.Checked = c.FormQuest.ShowOnce;
			MenuMain_ShowDaily.Checked = c.FormQuest.ShowDaily;
			MenuMain_ShowWeekly.Checked = c.FormQuest.ShowWeekly;
			MenuMain_ShowMonthly.Checked = c.FormQuest.ShowMonthly;
			MenuMain_ShowOther.Checked = c.FormQuest.ShowOther;

			if (c.FormQuest.ColumnFilter == null || ((List<bool>)c.FormQuest.ColumnFilter).Count != QuestView.Columns.Count)
			{
				c.FormQuest.ColumnFilter = Enumerable.Repeat(true, QuestView.Columns.Count).ToList();
			}
			if (c.FormQuest.ColumnWidth == null || ((List<int>)c.FormQuest.ColumnWidth).Count != QuestView.Columns.Count)
			{
				c.FormQuest.ColumnWidth = QuestView.Columns.Cast<DataGridViewColumn>().Select(column => column.Width).ToList();
			}
			{
				List<bool> list = c.FormQuest.ColumnFilter;
				List<int> width = c.FormQuest.ColumnWidth;

				for (int i = 0; i < QuestView.Columns.Count; i++)
				{
					QuestView.Columns[i].Visible =
					((ToolStripMenuItem)MenuMain_ColumnFilter.DropDownItems[i]).Checked = list[i];
					QuestView.Columns[i].Width = width[i];
				}
			}

			foreach (DataGridViewColumn column in QuestView.Columns)
			{
				column.SortMode = c.FormQuest.AllowUserToSortRows ? DataGridViewColumnSortMode.Automatic : DataGridViewColumnSortMode.NotSortable;
			}

			if (c.UI.IsLayoutFixed)
			{
				QuestView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
				QuestView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			}
			else
			{
				QuestView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
				QuestView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			}

			foreach (DataGridViewRow row in QuestView.Rows)
			{
				row.Height = 21;
			}

			Updated();

		}


		void SystemEvents_SystemShuttingDown()
		{

			try
			{

				if (QuestView.SortedColumn != null)
					Utility.Configuration.Config.FormQuest.SortParameter = QuestView.SortedColumn.Index << 1 | (QuestView.SortOrder == SortOrder.Ascending ? 0 : 1);

				Utility.Configuration.Config.FormQuest.ColumnWidth = QuestView.Columns.Cast<DataGridViewColumn>().Select(c => c.Width).ToList();

			}
			catch (Exception)
			{
				// *ぷちっ*				
			}
		}



		void Updated()
		{

			if (!KCDatabase.Instance.Quest.IsLoaded) return;

			QuestView.SuspendLayout();

			QuestView.Rows.Clear();

			foreach (var q in KCDatabase.Instance.Quest.Quests.Values)
			{

				if (MenuMain_ShowRunningOnly.Checked && !(q.State == 2 || q.State == 3))
					continue;

				switch (q.Type)
				{
					case 1:
						if (!MenuMain_ShowDaily.Checked) continue;
						break;
					case 2:
						if (!MenuMain_ShowWeekly.Checked) continue;
						break;
					case 3:
						if (!MenuMain_ShowMonthly.Checked) continue;
						break;
					case 4:
					default:
						if (!MenuMain_ShowOnce.Checked) continue;
						break;
					case 5:
						if (q.QuestID == 211 || q.QuestID == 212)
						{   // 空母3か輸送5
							if (!MenuMain_ShowDaily.Checked) continue;
						}
						else
						{
							if (!MenuMain_ShowOther.Checked) continue;
						}
						break;
				}


				DataGridViewRow row = new DataGridViewRow();
				row.CreateCells(QuestView);
				row.Height = 21;

				row.Cells[QuestView_State.Index].Value = (q.State == 3) ? ((bool?)null) : (q.State == 2);
				row.Cells[QuestView_Type.Index].Value = q.Type;
				row.Cells[QuestView_Category.Index].Value = q.Category;
				row.Cells[QuestView_Category.Index].Style = CSCategories[Math.Min(q.Category - 1, 8 - 1)];
				row.Cells[QuestView_Name.Index].Value = q.QuestID;
				{
					var progress = KCDatabase.Instance.QuestProgress[q.QuestID];
					row.Cells[QuestView_Name.Index].ToolTipText = $"{q.QuestID} : {q.Name}\r\n{q.Description}\r\n{progress?.GetClearCondition() ?? ""}";
				}
				{
					string value;
					double tag;

					if (q.State == 3)
					{
						switch (UILanguage) {
							case "zh":
								value = "完成！";
								break;
							case "en":
								value = "Completed!";
								break;
							default:
								value = "達成！";
								break;
						}
						tag = 1.0;

					}
					else
					{

						if (KCDatabase.Instance.QuestProgress.Progresses.ContainsKey(q.QuestID))
						{
							var p = KCDatabase.Instance.QuestProgress[q.QuestID];

							value = p.ToString();
							tag = p.ProgressPercentage;

						}
						else
						{

							switch (q.Progress)
							{
								case 0:
									value = "-";
									tag = 0.0;
									break;
								case 1:
									switch (UILanguage) {
										case "zh":
											value = "50% 以上";
											break;
										case "en":
											value = "More than 50%";
											break;
										default:
											value = "50%以上";
											break;
									}
									tag = 0.5;
									break;
								case 2:
									switch (UILanguage) {
										case "zh":
											value = "80% 以上";
											break;
										case "en":
											value = "More than 80%";
											break;
										default:
											value = "80%以上";
											break;
									}
									tag = 0.8;
									break;
								default:
									value = "???";
									tag = 0.0;
									break;
							}
						}
					}

					row.Cells[QuestView_Progress.Index].Value = value;
					row.Cells[QuestView_Progress.Index].Tag = tag;
				}

				QuestView.Rows.Add(row);
			}


			if (KCDatabase.Instance.Quest.Quests.Count < KCDatabase.Instance.Quest.Count)
			{
				int index = QuestView.Rows.Add();
				QuestView.Rows[index].Cells[QuestView_State.Index].Value = null;
				switch (UILanguage) {
					case "zh":
						QuestView.Rows[index].Cells[QuestView_Name.Index].Value = $"（未获取任务 x {KCDatabase.Instance.Quest.Count - KCDatabase.Instance.Quest.Quests.Count}）";
						break;
					case "en":
						QuestView.Rows[index].Cells[QuestView_Name.Index].Value = $"(Unretrieved quests x {KCDatabase.Instance.Quest.Count - KCDatabase.Instance.Quest.Quests.Count})";
						break;
					default:
						QuestView.Rows[index].Cells[QuestView_Name.Index].Value = $"(未取得の任務 x {KCDatabase.Instance.Quest.Count - KCDatabase.Instance.Quest.Quests.Count})";
						break;
				}
			}

			if (KCDatabase.Instance.Quest.Quests.Count == 0)
			{
				int index = QuestView.Rows.Add();
				QuestView.Rows[index].Cells[QuestView_State.Index].Value = null;
				switch (UILanguage) {
					case "zh":
						QuestView.Rows[index].Cells[QuestView_Name.Index].Value = "（任务全部完成！）";
						break;
					case "en":
						QuestView.Rows[index].Cells[QuestView_Name.Index].Value = "(All Quests Completed!)";
						break;
					default:
						QuestView.Rows[index].Cells[QuestView_Name.Index].Value = "(任務完遂！)";
						break;
				}
			}

			//更新時にソートする
			if (QuestView.SortedColumn != null)
				QuestView.Sort(QuestView.SortedColumn, QuestView.SortOrder == SortOrder.Ascending ? ListSortDirection.Ascending : ListSortDirection.Descending);


			QuestView.ResumeLayout();
		}


		private void QuestView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
		{

			if (e.Value is int)
			{
				if (e.ColumnIndex == QuestView_Type.Index)
				{
					e.Value = Constants.GetQuestType((int)e.Value);
					e.FormattingApplied = true;

				}
				else if (e.ColumnIndex == QuestView_Category.Index)
				{
					e.Value = Constants.GetQuestCategory((int)e.Value);
					e.FormattingApplied = true;

				}
				else if (e.ColumnIndex == QuestView_Name.Index)
				{
					var quest = KCDatabase.Instance.Quest[(int)e.Value];
					e.Value = quest != null ? quest.Name : "???";
					e.FormattingApplied = true;

				}

			}

		}



		private void QuestView_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
		{

			if (e.Column.Index == QuestView_State.Index)
			{
				e.SortResult = (e.CellValue1 == null ? 2 : ((bool)e.CellValue1 ? 1 : 0)) -
					(e.CellValue2 == null ? 2 : ((bool)e.CellValue2 ? 1 : 0));
			}
			else
			{
				e.SortResult = (e.CellValue1 as int? ?? 99999999) - (e.CellValue2 as int? ?? 99999999);
			}

			if (e.SortResult == 0)
			{
				e.SortResult = (QuestView.Rows[e.RowIndex1].Tag as int? ?? 0) - (QuestView.Rows[e.RowIndex2].Tag as int? ?? 0);
			}

			e.Handled = true;
		}

		private void QuestView_Sorted(object sender, EventArgs e)
		{

			for (int i = 0; i < QuestView.Rows.Count; i++)
			{
				QuestView.Rows[i].Tag = i;
			}

		}


		private void QuestView_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
		{

			if (e.ColumnIndex != QuestView_Progress.Index ||
				e.RowIndex < 0 ||
				(e.PaintParts & DataGridViewPaintParts.Background) == 0)
				return;


			using (var bback = new SolidBrush(e.CellStyle.BackColor))
			{

				Color col;
				double rate = QuestView.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag as double? ?? 0.0;

				if (rate < 0.5)
				{
					col = Color.FromArgb(0xFF, 0x88, 0x00);

				}
				else if (rate < 0.8)
				{
					col = Color.FromArgb(0x00, 0xCC, 0x00);

				}
				else if (rate < 1.0)
				{
					col = Color.FromArgb(0x00, 0x88, 0x00);

				}
				else
				{
					col = Color.FromArgb(0x00, 0x88, 0xFF);

				}

				using (var bgauge = new SolidBrush(col))
				{

					const int thickness = 4;

					e.Graphics.FillRectangle(bback, e.CellBounds);
					e.Graphics.FillRectangle(bgauge, new Rectangle(e.CellBounds.X, e.CellBounds.Bottom - thickness, (int)(e.CellBounds.Width * rate), thickness));
				}
			}

			e.Paint(e.ClipBounds, e.PaintParts & ~DataGridViewPaintParts.Background);
			e.Handled = true;

		}



		private void MenuMain_ShowRunningOnly_Click(object sender, EventArgs e)
		{
			Utility.Configuration.Config.FormQuest.ShowRunningOnly = MenuMain_ShowRunningOnly.Checked;
			Updated();
		}


		private void MenuMain_ShowOnce_Click(object sender, EventArgs e)
		{
			Utility.Configuration.Config.FormQuest.ShowOnce = MenuMain_ShowOnce.Checked;
			Updated();
		}

		private void MenuMain_ShowDaily_Click(object sender, EventArgs e)
		{
			Utility.Configuration.Config.FormQuest.ShowDaily = MenuMain_ShowDaily.Checked;
			Updated();
		}

		private void MenuMain_ShowWeekly_Click(object sender, EventArgs e)
		{
			Utility.Configuration.Config.FormQuest.ShowWeekly = MenuMain_ShowWeekly.Checked;
			Updated();
		}

		private void MenuMain_ShowMonthly_Click(object sender, EventArgs e)
		{
			Utility.Configuration.Config.FormQuest.ShowMonthly = MenuMain_ShowMonthly.Checked;
			Updated();
		}

		private void MenuMain_ShowOther_Click(object sender, EventArgs e)
		{
			Utility.Configuration.Config.FormQuest.ShowOther = MenuMain_ShowOther.Checked;
			Updated();
		}



		private void MenuMain_Initialize_Click(object sender, EventArgs e)
		{
			DialogResult result;
			switch (UILanguage) {
				case "zh":
					result = MessageBox.Show(
						"初始化任务数据。\r\n" +
						"除非数据出现异常，否则不建议使用。\r\n" +
						"确认初始化吗？", "要求确认",
						MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);
					break;
				case "en":
					result = MessageBox.Show(
						"Initialize Quests Data.\r\n" +
						"This is not recommended unless you have data inconsistency.\r\n" +
						"Confirm to initialize?", "Confirmation",
						MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);
					break;
				default:
					result = MessageBox.Show(
						"任務データを初期化します。\r\n" +
						"データに齟齬が生じている場合以外での使用は推奨しません。\r\n" +
						"よろしいですか？", "任務初期化の確認",
						MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);
					break;
			}
			if (result == DialogResult.Yes)
			{

				KCDatabase.Instance.Quest.Clear();
				KCDatabase.Instance.QuestProgress.Clear();
				ClearQuestView();
			}

		}


		private void ClearQuestView()
		{

			QuestView.Rows.Clear();

			{
				DataGridViewRow row = new DataGridViewRow();
				row.CreateCells(QuestView);
				switch (UILanguage) {
					case "zh":
						row.SetValues(null, null, null, "（未获取）", null);
						break;
					case "en":
						row.SetValues(null, null, null, "(Unretrieved)", null);
						break;
					default:
						row.SetValues(null, null, null, "(未取得)", null);
						break;
				}
				QuestView.Rows.Add(row);
			}

		}


		private void MenuMain_ColumnFilter_Click(object sender, EventArgs e)
		{

			var menu = sender as ToolStripMenuItem;
			if (menu == null) return;

			int index = -1;
			for (int i = 0; i < MenuMain_ColumnFilter.DropDownItems.Count; i++)
			{
				if (sender == MenuMain_ColumnFilter.DropDownItems[i])
				{
					index = i;
					break;
				}
			}

			if (index == -1) return;

			QuestView.Columns[index].Visible =
			Utility.Configuration.Config.FormQuest.ColumnFilter.List[index] = menu.Checked;
		}


		private void QuestView_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
		{

			if (IsLoaded)
				Utility.Configuration.Config.FormQuest.ColumnWidth = QuestView.Columns.Cast<DataGridViewColumn>().Select(c => c.Width).ToList();

		}




		private void QuestView_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
		{

			if (e.Button == System.Windows.Forms.MouseButtons.Right && e.RowIndex >= 0)
			{
				QuestView.ClearSelection();
				QuestView.Rows[e.RowIndex].Selected = true;
			}

		}

		private void MenuProgress_Increment_Click(object sender, EventArgs e)
		{

			int id = GetSelectedRowQuestID();

			var quest = KCDatabase.Instance.Quest[id];
			var progress = KCDatabase.Instance.QuestProgress[id];

			if (id != -1 && quest != null && progress != null)
			{

				try
				{
					progress.Increment();
					Updated();

				}
				catch (Exception)
				{
					switch (UILanguage) {
						case "zh":
							Logger.Add(3, $"无法修改任务『{quest.Name}』的进度。");
							break;
						case "en":
							Logger.Add(3, $"Can't modify progress of Quest: {quest.Name}");
							break;
						default:
							Logger.Add(3, $"任務『{quest.Name}』の進捗を変更することはできません。");
							break;
					}
					System.Media.SystemSounds.Hand.Play();
				}
			}
		}

		private void MenuProgress_Decrement_Click(object sender, EventArgs e)
		{

			int id = GetSelectedRowQuestID();
			var quest = KCDatabase.Instance.Quest[id];
			var progress = KCDatabase.Instance.QuestProgress[id];

			if (id != -1 && quest != null && progress != null)
			{

				try
				{
					progress.Decrement();
					Updated();

				}
				catch (Exception)
				{
					switch (UILanguage) {
						case "zh":
							Logger.Add(3, $"无法修改任务『{quest.Name}』的进度。");
							break;
						case "en":
							Logger.Add(3, $"Can't modify progress of Quest: {quest.Name}");
							break;
						default:
							Logger.Add(3, $"任務『{quest.Name}』の進捗を変更することはできません。");
							break;
					}
					System.Media.SystemSounds.Hand.Play();
				}
			}
		}

		private void MenuProgress_Reset_Click(object sender, EventArgs e)
		{

			int id = GetSelectedRowQuestID();

			var quest = KCDatabase.Instance.Quest[id];
			var progress = KCDatabase.Instance.QuestProgress[id];

			if (id != -1 && (quest != null || progress != null))
			{
				DialogResult result;
				switch (UILanguage) {
					case "zh":
						result = MessageBox.Show(
							$"从列表中删除任务{(quest != null ? $"『{quest.Name}』" : $"ID: {id} ")}并重制其进度。\r\n" +
							"确认删除吗？\r\n" +
							"（打开「艦これ」游戏内任务画面会重新读取任务进度）", "要求确认",
							MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
						break;
					case "en":
						result = MessageBox.Show(
							$"Delete Quest {(quest != null ? $"\"{quest.Name}\"" : $"ID: {id} ")} and reset its progress.\r\n" +
							"Confirm to Delete?\r\n" +
							"(Quest progress would be reloaded when opening in-game Quests panel)", "Confirmation",
							MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
						break;
					default:
						result = MessageBox.Show(
							$"任務{(quest != null ? $"『{quest.Name}』" : $"ID: {id} ")}を一覧から削除し、進捗をリセットします。\r\n" +
							"よろしいですか？\r\n" +
							"(艦これ本体の任務画面を開くと正しく更新されます。)", "任務削除の確認",
							MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
						break;
				}
				if (result == DialogResult.Yes)
				{

					if (quest != null)
						KCDatabase.Instance.Quest.Quests.Remove(quest);

					if (progress != null)
						KCDatabase.Instance.QuestProgress.Progresses.Remove(progress);

					Updated();
				}
			}

		}


		// デフォルトのツールチップは消える時間が速すぎるので、自分で制御する
		private void QuestView_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
		{

			if (e.RowIndex < 0 || e.ColumnIndex < 0 || e.RowIndex >= QuestView.RowCount || e.ColumnIndex >= QuestView.ColumnCount)
			{
				ToolTipInfo.SetToolTip(QuestView, null);
				return;
			}

			if (!string.IsNullOrWhiteSpace(QuestView[e.ColumnIndex, e.RowIndex].ToolTipText))
			{
				ToolTipInfo.SetToolTip(QuestView, QuestView[e.ColumnIndex, e.RowIndex].ToolTipText);

			}
			else if (e.ColumnIndex == QuestView_Progress.Index && QuestView[e.ColumnIndex, e.RowIndex].Value != null)
			{
				ToolTipInfo.SetToolTip(QuestView, QuestView[e.ColumnIndex, e.RowIndex].Value.ToString());

			}
			else
			{
				ToolTipInfo.SetToolTip(QuestView, null);
			}

		}

		private void QuestView_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
		{
			ToolTipInfo.SetToolTip(QuestView, null);
		}


		private void MenuMain_Opening(object sender, CancelEventArgs e)
		{

			var quest = KCDatabase.Instance.Quest[GetSelectedRowQuestID()];

			if (quest != null)
			{
				MenuMain_GoogleQuest.Enabled = true;
				switch (UILanguage) {
					case "zh":
						MenuMain_GoogleQuest.Text = $"使用 Google 搜索『{quest.Name}』(&G)";
						break;
					case "en":
						MenuMain_GoogleQuest.Text = $"&Google Search \"{quest.Name}\"";
						break;
					default:
						MenuMain_GoogleQuest.Text = $"『{quest.Name}』でGoogle検索(&G)";
						break;
				}
			}
			else
			{
				MenuMain_GoogleQuest.Enabled = false;
				switch (UILanguage) {
					case "zh":
						MenuMain_GoogleQuest.Text = "使用 Google 搜索任务名(&G)";
						break;
					case "en":
						MenuMain_GoogleQuest.Text = "&Google Search Quest Name";
						break;
					default:
						MenuMain_GoogleQuest.Text = "任務名でGoogle検索(&G)";
						break;
				}
			}
		}

		private void MenuMain_GoogleQuest_Click(object sender, EventArgs e)
		{
			var quest = KCDatabase.Instance.Quest[GetSelectedRowQuestID()];

			if (quest != null)
			{
				try
				{

					// google <任務名> 艦これ
					System.Diagnostics.Process.Start(@"https://www.google.co.jp/search?q=" + Uri.EscapeDataString(quest.Name) + "+%E8%89%A6%E3%81%93%E3%82%8C");

				}
				catch (Exception ex)
				{
					switch (UILanguage) {
						case "zh":
							ErrorReporter.SendErrorReport(ex, "尝试调用 Google 搜索失败。");
							break;
						case "en":
							ErrorReporter.SendErrorReport(ex, "Failed to open Google Search.");
							break;
						default:
							ErrorReporter.SendErrorReport(ex, "任務名の Google 検索に失敗しました。");
							break;
					}
				}
			}

		}

		private int GetSelectedRowQuestID()
		{
			var rows = QuestView.SelectedRows;

			if (rows != null && rows.Count > 0 && rows[0].Index != -1)
			{

				return rows[0].Cells[QuestView_Name.Index].Value as int? ?? -1;
			}

			return -1;
		}


		protected override string GetPersistString()
		{
			return "Quest";
		}


	}
}

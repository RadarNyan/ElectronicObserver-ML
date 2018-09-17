using ElectronicObserver.Data;
using ElectronicObserver.Resource;
using ElectronicObserver.Utility.Data;
using ElectronicObserver.Utility.Mathematics;
using ElectronicObserver.Window.Control;
using ElectronicObserver.Window.Support;
using System;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace ElectronicObserver.Window
{
	public partial class FormBaseAirCorps : DockContent
	{


		private class TableBaseAirCorpsControl : IDisposable
		{

			public ImageLabel Name;
			public ImageLabel ActionKind;
			public ImageLabel AirSuperiority;
			public ImageLabel Distance;
			public ShipStatusEquipment Squadrons;

			public ToolTip ToolTipInfo;

			private string UILanguage;

			public TableBaseAirCorpsControl(FormBaseAirCorps parent)
			{

				#region Initialize

				UILanguage = parent.UILanguage;

				Name = new ImageLabel
				{
					Name = "Name",
					Text = "*",
					Anchor = AnchorStyles.Left,
					TextAlign = ContentAlignment.MiddleLeft,
					ImageAlign = ContentAlignment.MiddleRight,
					ImageList = ResourceManager.Instance.Icons,
					Padding = new Padding(2, 2, 2, 2),
					Margin = new Padding(2, 1, 2, 1),      // ここを 2,0,2,0 にすると境界線の描画に問題が出るので
					AutoSize = true,
					ContextMenuStrip = parent.ContextMenuBaseAirCorps,
					Visible = false,
					Cursor = Cursors.Help
				};

				ActionKind = new ImageLabel
				{
					Text = "*",
					Anchor = AnchorStyles.Left,
					TextAlign = ContentAlignment.MiddleLeft,
					ImageAlign = ContentAlignment.MiddleCenter,
					//ActionKind.ImageList =
					Padding = new Padding(2, 2, 2, 2),
					Margin = new Padding(2, 0, 2, 0),
					AutoSize = true,
					Visible = false
				};

				AirSuperiority = new ImageLabel
				{
					Text = "*",
					Anchor = AnchorStyles.Left,
					TextAlign = ContentAlignment.MiddleLeft,
					ImageAlign = ContentAlignment.MiddleLeft,
					ImageList = ResourceManager.Instance.Equipments,
					ImageIndex = (int)ResourceManager.EquipmentContent.CarrierBasedFighter,
					Padding = new Padding(2, 2, 2, 2),
					Margin = new Padding(2, 0, 2, 0),
					AutoSize = true,
					Visible = false
				};

				Distance = new ImageLabel
				{
					Text = "*",
					Anchor = AnchorStyles.Left,
					TextAlign = ContentAlignment.MiddleLeft,
					ImageAlign = ContentAlignment.MiddleLeft,
					ImageList = ResourceManager.Instance.Icons,
					ImageIndex = (int)ResourceManager.IconContent.ParameterAircraftDistance,
					Padding = new Padding(2, 2, 2, 2),
					Margin = new Padding(2, 0, 2, 0),
					AutoSize = true,
					Visible = false
				};

				Squadrons = new ShipStatusEquipment
				{
					Anchor = AnchorStyles.Left,
					Padding = new Padding(0, 1, 0, 2),
					Margin = new Padding(2, 0, 2, 0),
					Size = new Size(40, 20),
					AutoSize = true,
					Visible = false
				};
				Squadrons.ResumeLayout();

				ConfigurationChanged(parent);

				ToolTipInfo = parent.ToolTipInfo;

				#endregion

			}


			public TableBaseAirCorpsControl(FormBaseAirCorps parent, TableLayoutPanel table, int row)
				: this(parent)
			{
				AddToTable(table, row);
			}

			public void AddToTable(TableLayoutPanel table, int row)
			{

				table.SuspendLayout();

				table.Controls.Add(Name, 0, row);
				table.Controls.Add(ActionKind, 1, row);
				table.Controls.Add(AirSuperiority, 2, row);
				table.Controls.Add(Distance, 3, row);
				table.Controls.Add(Squadrons, 4, row);
				table.ResumeLayout();

				ControlHelper.SetTableRowStyle(table, row, ControlHelper.GetDefaultRowStyle());
			}


			public void Update(int baseAirCorpsID)
			{

				KCDatabase db = KCDatabase.Instance;
				var corps = db.BaseAirCorps[baseAirCorpsID];

				if (corps == null)
				{
					baseAirCorpsID = -1;

				}
				else
				{

					Name.Text = string.Format("#{0} - {1}", corps.MapAreaID, corps.Name);
					Name.Tag = corps.MapAreaID;
					var sb = new StringBuilder();


					string areaName;
					switch (UILanguage) {
						case "zh":
							areaName = KCDatabase.Instance.MapArea.ContainsKey(corps.MapAreaID) ? KCDatabase.Instance.MapArea[corps.MapAreaID].Name : "百慕大海域";
							sb.AppendLine("所属海域：" + areaName);
							break;
						case "en":
							areaName = KCDatabase.Instance.MapArea.ContainsKey(corps.MapAreaID) ? KCDatabase.Instance.MapArea[corps.MapAreaID].Name : "Bermuda Waters";
							sb.AppendLine("Located in: " + areaName);
							break;
						default:
							areaName = KCDatabase.Instance.MapArea.ContainsKey(corps.MapAreaID) ? KCDatabase.Instance.MapArea[corps.MapAreaID].Name : "バミューダ海域";
							sb.AppendLine("所属海域: " + areaName);
							break;
					}

					// state 

					if (corps.Squadrons.Values.Any(sq => sq != null && sq.Condition > 1))
					{
						// 疲労
						int tired = corps.Squadrons.Values.Max(sq => sq?.Condition ?? 0);

						if (tired == 2)
						{
							Name.ImageAlign = ContentAlignment.MiddleRight;
							Name.ImageIndex = (int)ResourceManager.IconContent.ConditionTired;
							switch (UILanguage) {
								case "zh":
									sb.AppendLine("疲劳");
									break;
								case "en":
									sb.AppendLine("Tired");
									break;
								default:
									sb.AppendLine("疲労");
									break;
							}
						}
						else
						{
							Name.ImageAlign = ContentAlignment.MiddleRight;
							Name.ImageIndex = (int)ResourceManager.IconContent.ConditionVeryTired;
							switch (UILanguage) {
								case "zh":
									sb.AppendLine("过劳");
									break;
								case "en":
									sb.AppendLine("Very Tired");
									break;
								default:
									sb.AppendLine("過労");
									break;
							}
						}

					}
					else if (corps.Squadrons.Values.Any(sq => sq != null && sq.AircraftCurrent < sq.AircraftMax))
					{
						// 未補給
						Name.ImageAlign = ContentAlignment.MiddleRight;
						Name.ImageIndex = (int)ResourceManager.IconContent.FleetNotReplenished;
						switch (UILanguage) {
							case "zh":
								sb.AppendLine("未补给");
								break;
							case "en":
								sb.AppendLine("Need Replenish");
								break;
							default:
								sb.AppendLine("未補給");
								break;
						}
					}
					else
					{
						Name.ImageAlign = ContentAlignment.MiddleCenter;
						Name.ImageIndex = -1;

					}
					ToolTipInfo.SetToolTip(Name, sb.ToString());


					ActionKind.Text = "[" + Constants.GetBaseAirCorpsActionKind(corps.ActionKind) + "]";

					{
						int airSuperiority = Calculator.GetAirSuperiority(corps);
						if (Utility.Configuration.Config.FormFleet.ShowAirSuperiorityRange)
						{
							int airSuperiority_max = Calculator.GetAirSuperiority(corps, true);
							if (airSuperiority < airSuperiority_max)
								AirSuperiority.Text = string.Format("{0} ～ {1}", airSuperiority, airSuperiority_max);
							else
								AirSuperiority.Text = airSuperiority.ToString();
						}
						else
						{
							AirSuperiority.Text = airSuperiority.ToString();
						}

						switch (UILanguage) {
							case "zh":
								ToolTipInfo.SetToolTip(AirSuperiority,
									$"确保：{(int)(airSuperiority / 3.0)}\r\n" +
									$"优势：{(int)(airSuperiority / 1.5)}\r\n" +
									$"均衡：{Math.Max((int)(airSuperiority * 1.5 - 1), 0)}\r\n" +
									$"劣势：{Math.Max((int)(airSuperiority * 3.0 - 1), 0)}");
								break;
							case "en":
								ToolTipInfo.SetToolTip(AirSuperiority,
									$"Air Supremacy: {(int)(airSuperiority / 3.0)}\r\n" +
									$"Air Superiority: {(int)(airSuperiority / 1.5)}\r\n" +
									$"Air Parity: {Math.Max((int)(airSuperiority * 1.5 - 1), 0)}\r\n" +
									$"Air Denial: {Math.Max((int)(airSuperiority * 3.0 - 1), 0)}");
								break;
							default:
								ToolTipInfo.SetToolTip(AirSuperiority,
									$"確保: {(int)(airSuperiority / 3.0)}\r\n" +
									$"優勢: {(int)(airSuperiority / 1.5)}\r\n" +
									$"均衡: {Math.Max((int)(airSuperiority * 1.5 - 1), 0)}\r\n" +
									$"劣勢: {Math.Max((int)(airSuperiority * 3.0 - 1), 0)}");
								break;
						}
					}

					Distance.Text = corps.Distance.ToString();

					Squadrons.SetSlotList(corps);
					ToolTipInfo.SetToolTip(Squadrons, GetEquipmentString(corps));

				}


				Name.Visible =
				ActionKind.Visible =
				AirSuperiority.Visible =
				Distance.Visible =
				Squadrons.Visible =
					baseAirCorpsID != -1;
			}


			public void ConfigurationChanged(FormBaseAirCorps parent)
			{

				var config = Utility.Configuration.Config;

				var mainfont = config.UI.MainFont;
				var subfont = config.UI.SubFont;

				Name.Font = mainfont;
				ActionKind.Font = mainfont;
				AirSuperiority.Font = mainfont;
				Distance.Font = mainfont;
				Squadrons.Font = subfont;

				Squadrons.ShowAircraft = config.FormFleet.ShowAircraft;
				Squadrons.ShowAircraftLevelByNumber = config.FormFleet.ShowAircraftLevelByNumber;
				Squadrons.LevelVisibility = config.FormFleet.EquipmentLevelVisibility;

			}


			private string GetEquipmentString(BaseAirCorpsData corps)
			{
				var sb = new StringBuilder();

				if (corps == null)
				{
					switch (UILanguage) {
						case "zh":
							return "（未开放）\r\n";
						case "en":
							return "(Unopened)\r\n";
						default:
							return "(未開放)\r\n";
					}
				}

				foreach (var squadron in corps.Squadrons.Values)
				{
					if (squadron == null)
						continue;

					var eq = squadron.EquipmentInstance;

					switch (squadron.State)
					{
						case 0:     // 未配属
						default:
							switch (UILanguage) {
								case "zh":
									sb.AppendLine("（空）");
									break;
								case "en":
									sb.AppendLine("(Empty)");
									break;
								default:
									sb.AppendLine("(なし)");
									break;
							}
							break;

						case 1:     // 配属済み
							if (eq == null)
								goto case 0;
							sb.AppendFormat("[{0}/{1}] ",
								squadron.AircraftCurrent,
								squadron.AircraftMax);

							switch (squadron.Condition)
							{
								case 1:
								default:
									break;
								case 2:
									switch (UILanguage) {
										case "zh":
											sb.Append("[疲劳] ");
											break;
										case "en":
											sb.Append("[Tired] ");
											break;
										default:
											sb.Append("[疲労] ");
											break;
									}
									break;
								case 3:
									switch (UILanguage) {
										case "zh":
											sb.Append("[过劳] ");
											break;
										case "en":
											sb.Append("[Very Tired] ");
											break;
										default:
											sb.Append("[過労] ");
											break;
									}
									break;
							}

							switch (UILanguage) {
								case "zh":
									sb.AppendLine($"{eq.NameWithLevel}（半径：{eq.MasterEquipment.AircraftDistance}）");
									break;
								case "en":
									sb.AppendLine($"{eq.NameWithLevel} (Range: {eq.MasterEquipment.AircraftDistance})");
									break;
								default:
									sb.AppendLine($"{eq.NameWithLevel} (半径: {eq.MasterEquipment.AircraftDistance})");
									break;
							}
							break;

						case 2:     // 配置転換中
							switch (UILanguage) {
								case "zh":
									sb.AppendLine($"配置转换中（开始于：{DateTimeHelper.TimeToCSVString(squadron.RelocatedTime)}）");
									break;
								case "en":
									sb.AppendLine($"Relocating (Began at: {DateTimeHelper.TimeToCSVString(squadron.RelocatedTime)})");
									break;
								default:
									sb.AppendLine($"配置転換中 (開始時刻: {DateTimeHelper.TimeToCSVString(squadron.RelocatedTime)})");
									break;
							}
							break;
					}
				}

				return sb.ToString();
			}

			public void Dispose()
			{
				Name.Dispose();
				ActionKind.Dispose();
				AirSuperiority.Dispose();
				Distance.Dispose();
				Squadrons.Dispose();
			}
		}


		private TableBaseAirCorpsControl[] ControlMember;

		private string UILanguage;

		public FormBaseAirCorps(FormMain parent)
		{
			InitializeComponent();

			UILanguage = parent.UILanguage;

			switch (UILanguage) {
				case "zh":
					ContextMenuBaseAirCorps_CopyOrganization.Text = "复制到剪贴板(&C)";
					ContextMenuBaseAirCorps_DisplayRelocatedEquipments.Text = "查看配置转换中的装备(&R)";
					Text = "基地航空队";
					break;
				case "en":
					ContextMenuBaseAirCorps_CopyOrganization.Text = "&Copy to Clipboard";
					ContextMenuBaseAirCorps_DisplayRelocatedEquipments.Text = "Check &Relocating Aircrafts";
					Text = "Land Base";
					break;
				default:
					break;
			}


			ControlMember = new TableBaseAirCorpsControl[9];
			TableMember.SuspendLayout();
			for (int i = 0; i < ControlMember.Length; i++)
			{
				ControlMember[i] = new TableBaseAirCorpsControl(this, TableMember, i);
			}
			TableMember.ResumeLayout();

			ConfigurationChanged();

			Icon = ResourceManager.ImageToIcon(ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormBaseAirCorps]);
		}

		private void FormBaseAirCorps_Load(object sender, EventArgs e)
		{

			var api = Observer.APIObserver.Instance;

			api["api_port/port"].ResponseReceived += Updated;
			api["api_get_member/mapinfo"].ResponseReceived += Updated;
			api["api_get_member/base_air_corps"].ResponseReceived += Updated;
			api["api_req_air_corps/change_name"].ResponseReceived += Updated;
			api["api_req_air_corps/set_action"].ResponseReceived += Updated;
			api["api_req_air_corps/set_plane"].ResponseReceived += Updated;
			api["api_req_air_corps/supply"].ResponseReceived += Updated;
			api["api_req_air_corps/expand_base"].ResponseReceived += Updated;

			Utility.Configuration.Instance.ConfigurationChanged += ConfigurationChanged;

		}


		private void ConfigurationChanged()
		{

			var c = Utility.Configuration.Config;

			TableMember.SuspendLayout();

			Font = c.UI.MainFont;

			foreach (var control in ControlMember)
				control.ConfigurationChanged(this);

			ControlHelper.SetTableRowStyles(TableMember, ControlHelper.GetDefaultRowStyle());

			TableMember.ResumeLayout();

			if (KCDatabase.Instance.BaseAirCorps.Any())
				Updated(null, null);
		}


		void Updated(string apiname, dynamic data)
		{

			var keys = KCDatabase.Instance.BaseAirCorps.Keys;

			if (Utility.Configuration.Config.FormBaseAirCorps.ShowEventMapOnly)
			{
				var eventAreaCorps = KCDatabase.Instance.BaseAirCorps.Values.Where(b =>
				{
					var maparea = KCDatabase.Instance.MapArea[b.MapAreaID];
					return maparea != null && maparea.MapType == 1;
				}).Select(b => b.ID);

				if (eventAreaCorps.Any())
					keys = eventAreaCorps;
			}


			TableMember.SuspendLayout();
			TableMember.RowCount = keys.Count();
			for (int i = 0; i < ControlMember.Length; i++)
			{
				ControlMember[i].Update(i < keys.Count() ? keys.ElementAt(i) : -1);
			}
			TableMember.ResumeLayout();

			// set icon
			{
				var squadrons = KCDatabase.Instance.BaseAirCorps.Values.Where(b => b != null)
					.SelectMany(b => b.Squadrons.Values)
					.Where(s => s != null);
				bool isNotReplenished = squadrons.Any(s => s.State == 1 && s.AircraftCurrent < s.AircraftMax);
				bool isTired = squadrons.Any(s => s.State == 1 && s.Condition == 2);
				bool isVeryTired = squadrons.Any(s => s.State == 1 && s.Condition == 3);

				int imageIndex;

				if (isNotReplenished)
					imageIndex = (int)ResourceManager.IconContent.FleetNotReplenished;
				else if (isVeryTired)
					imageIndex = (int)ResourceManager.IconContent.ConditionVeryTired;
				else if (isTired)
					imageIndex = (int)ResourceManager.IconContent.ConditionTired;
				else
					imageIndex = (int)ResourceManager.IconContent.FormBaseAirCorps;

				if (Icon != null) ResourceManager.DestroyIcon(Icon);
				Icon = ResourceManager.ImageToIcon(ResourceManager.Instance.Icons.Images[imageIndex]);
				if (Parent != null) Parent.Refresh();       //アイコンを更新するため
			}

		}


		private void ContextMenuBaseAirCorps_Opening(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (KCDatabase.Instance.BaseAirCorps.Count == 0)
			{
				e.Cancel = true;
				return;
			}

			if (ContextMenuBaseAirCorps.SourceControl.Name == "Name")
				ContextMenuBaseAirCorps_CopyOrganization.Tag = ContextMenuBaseAirCorps.SourceControl.Tag as int? ?? -1;
			else
				ContextMenuBaseAirCorps_CopyOrganization.Tag = -1;
		}

		private void ContextMenuBaseAirCorps_CopyOrganization_Click(object sender, EventArgs e)
		{

			var sb = new StringBuilder();
			int areaid = ContextMenuBaseAirCorps_CopyOrganization.Tag as int? ?? -1;

			var baseaircorps = KCDatabase.Instance.BaseAirCorps.Values;
			if (areaid != -1)
				baseaircorps = baseaircorps.Where(c => c.MapAreaID == areaid);

			foreach (var corps in baseaircorps)
			{
				string areaName;
				switch (UILanguage) {
					case "zh":
						areaName = KCDatabase.Instance.MapArea.ContainsKey(corps.MapAreaID) ? KCDatabase.Instance.MapArea[corps.MapAreaID].Name : "百慕大海域";
						break;
					case "en":
						areaName = KCDatabase.Instance.MapArea.ContainsKey(corps.MapAreaID) ? KCDatabase.Instance.MapArea[corps.MapAreaID].Name : "Bermuda Waters";
						break;
					default:
						areaName = KCDatabase.Instance.MapArea.ContainsKey(corps.MapAreaID) ? KCDatabase.Instance.MapArea[corps.MapAreaID].Name : "バミューダ海域";
						break;
				}

				switch (UILanguage) {
					case "zh":
						sb.AppendLine(
							$"{(areaid == -1 ? (areaName + "：") : "") + corps.Name}\t" +
							$"[{Constants.GetBaseAirCorpsActionKind(corps.ActionKind)}]" +
							$" 制空战力 {Calculator.GetAirSuperiority(corps)} / 战斗行动半径 {corps.Distance}");
						break;
					case "en":
						sb.AppendLine(
							$"{(areaid == -1 ? (areaName + "：") : "") + corps.Name}\t" +
							$"[{Constants.GetBaseAirCorpsActionKind(corps.ActionKind)}]" +
							$" Fighter Power {Calculator.GetAirSuperiority(corps)} / Combat Radius {corps.Distance}");
						break;
					default:
						sb.AppendLine(
							$"{(areaid == -1 ? (areaName + "：") : "") + corps.Name}\t" +
							$"[{Constants.GetBaseAirCorpsActionKind(corps.ActionKind)}]" +
							$" 制空戦力{Calculator.GetAirSuperiority(corps)}/戦闘行動半径{corps.Distance}");
						break;
				}

				var sq = corps.Squadrons.Values.ToArray();

				for (int i = 0; i < sq.Length; i++)
				{
					if (i > 0)
						sb.Append(", ");

					if (sq[i] == null)
					{
						switch (UILanguage) {
							case "zh":
								sb.Append("（去向不明）");
								break;
							case "en":
								sb.Append("(Whereabouts unknown)");
								break;
							default:
								sb.Append("(消息不明)");
								break;
						}
						continue;
					}

					switch (sq[i].State)
					{
						case 0:
							switch (UILanguage) {
								case "zh":
									sb.Append("（未分配）");
									break;
								case "en":
									sb.Append("(Unassigned)");
									break;
								default:
									sb.Append("(未配属)");
									break;
							}
							break;
						case 1:
							{
								var eq = sq[i].EquipmentInstance;

								switch (UILanguage) {
									case "zh":
										sb.Append(eq?.NameWithLevel ?? "（空）");
										break;
									case "en":
										sb.Append(eq?.NameWithLevel ?? "(Empty)");
										break;
									default:
										sb.Append(eq?.NameWithLevel ?? "(なし)");
										break;
								}

								if (sq[i].AircraftCurrent < sq[i].AircraftMax)
									sb.AppendFormat("[{0}/{1}]", sq[i].AircraftCurrent, sq[i].AircraftMax);
							}
							break;
						case 2:
							switch (UILanguage) {
								case "zh":
									sb.Append("（配置转换中）");
									break;
								case "en":
									sb.Append("(Relocating)");
									break;
								default:
									sb.Append("(配置転換中)");
									break;
							}
							break;
					}
				}

				sb.AppendLine();
			}

			Clipboard.SetData(DataFormats.StringFormat, sb.ToString());
		}

		private void ContextMenuBaseAirCorps_DisplayRelocatedEquipments_Click(object sender, EventArgs e)
		{

			string message = string.Join("\r\n", KCDatabase.Instance.RelocatedEquipments.Values
				.Where(eq => eq.EquipmentInstance != null)
				.Select(eq => string.Format("{0} ({1}～)", eq.EquipmentInstance.NameWithLevel, DateTimeHelper.TimeToCSVString(eq.RelocatedTime))));

			if (message.Length == 0)
			{
				switch (UILanguage) {
					case "zh":
						message = "目前没有配置转换中的装备。";
						break;
					case "en":
						message = "No aircrafts being relocated at the moment.";
						break;
					default:
						message = "現在配置転換中の装備はありません。";
						break;
				}
			}

			switch (UILanguage) {
				case "zh":
					MessageBox.Show(message, "配置转换中装备", MessageBoxButtons.OK, MessageBoxIcon.Information);
					break;
				case "en":
					MessageBox.Show(message, "Relocating Aircrafts", MessageBoxButtons.OK, MessageBoxIcon.Information);
					break;
				default:
					MessageBox.Show(message, "配置転換中装備", MessageBoxButtons.OK, MessageBoxIcon.Information);
					break;
			}
		}


		private void TableMember_CellPaint(object sender, TableLayoutCellPaintEventArgs e)
		{
			e.Graphics.DrawLine(Pens.Silver, e.CellBounds.X, e.CellBounds.Bottom - 1, e.CellBounds.Right - 1, e.CellBounds.Bottom - 1);
		}

		protected override string GetPersistString()
		{
			return "BaseAirCorps";
		}




	}
}

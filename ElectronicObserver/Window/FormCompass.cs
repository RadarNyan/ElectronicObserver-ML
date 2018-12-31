using ElectronicObserver.Data;
using ElectronicObserver.Data.Battle;
using ElectronicObserver.Observer;
using ElectronicObserver.Resource;
using ElectronicObserver.Resource.Record;
using ElectronicObserver.Utility;
using ElectronicObserver.Utility.Data;
using ElectronicObserver.Window.Control;
using ElectronicObserver.Window.Dialog;
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

	public partial class FormCompass : DockContent
	{


		private class TableEnemyMemberControl : IDisposable
		{

			public ImageLabel ShipName;
			public ShipStatusEquipment Equipments;

			public FormCompass Parent;
			public ToolTip ToolTipInfo;


			public TableEnemyMemberControl(FormCompass parent)
			{

				#region Initialize

				Parent = parent;
				ToolTipInfo = parent.ToolTipInfo;


				ShipName = new ImageLabel
				{
					Anchor = AnchorStyles.Left,
					ForeColor = parent.MainFontColor,
					ImageAlign = ContentAlignment.MiddleCenter,
					Padding = new Padding(2, 2, 2, 2),
					Margin = new Padding(2, 0, 2, 1),
					AutoEllipsis = true,
					AutoSize = true,
					Cursor = Cursors.Help
				};
				ShipName.MouseClick += ShipName_MouseClick;

				Equipments = new ShipStatusEquipment();
				Equipments.SuspendLayout();
				Equipments.Anchor = AnchorStyles.Left;
				Equipments.Padding = new Padding(0, 1, 0, 2);
				Equipments.Margin = new Padding(2, 0, 2, 0);
				Equipments.AutoSize = true;
				Equipments.ResumeLayout();

				ConfigurationChanged();

				#endregion

			}


			public TableEnemyMemberControl(FormCompass parent, TableLayoutPanel table, int row)
				: this(parent)
			{

				AddToTable(table, row);
			}

			public void AddToTable(TableLayoutPanel table, int row)
			{

				table.Controls.Add(ShipName, 0, row);
				table.Controls.Add(Equipments, 1, row);

			}


			public void Update(int shipID)
			{
				var slot = shipID != -1 ? KCDatabase.Instance.MasterShips[shipID].DefaultSlot : null;
				Update(shipID, slot?.ToArray());
			}


			public void Update(int shipID, int[] slot)
			{

				ShipName.Tag = shipID;

				if (shipID == -1)
				{
					//なし
					ShipName.Text = "-";
					ShipName.ForeColor = UIColorScheme.Colors.MainFG;
					Equipments.Visible = false;
					ToolTipInfo.SetToolTip(ShipName, null);
					ToolTipInfo.SetToolTip(Equipments, null);

				}
				else
				{

					ShipDataMaster ship = KCDatabase.Instance.MasterShips[shipID];


					ShipName.Text = ship.Name;
					ShipName.ForeColor = ship.GetShipNameColor();
					ToolTipInfo.SetToolTip(ShipName, GetShipString(shipID, slot));

					Equipments.SetSlotList(shipID, slot);
					Equipments.Visible = true;
					ToolTipInfo.SetToolTip(Equipments, GetEquipmentString(shipID, slot));
				}

			}

			public void UpdateEquipmentToolTip(int shipID, int[] slot, int level, int hp, int firepower, int torpedo, int aa, int armor)
			{

				ToolTipInfo.SetToolTip(ShipName, GetShipString(shipID, slot, level, hp, firepower, torpedo, aa, armor));
			}


			void ShipName_MouseClick(object sender, MouseEventArgs e)
			{

				if ((e.Button & System.Windows.Forms.MouseButtons.Right) != 0)
				{
					int shipID = ShipName.Tag as int? ?? -1;

					if (shipID != -1)
						new DialogAlbumMasterShip(shipID).Show(Parent);
				}

			}


			public void ConfigurationChanged()
			{
				ShipName.Font = Parent.MainFont;
				Equipments.Font = Parent.SubFont;

				ShipName.MaximumSize = new Size(Utility.Configuration.Config.FormCompass.MaxShipNameWidth, int.MaxValue);
			}

			public void Dispose()
			{
				ShipName.Dispose();
				Equipments.Dispose();
			}
		}


		private class TableEnemyCandidateControl
		{

			public ImageLabel[] ShipNames;
			public ImageLabel Formation;
			public ImageLabel AirSuperiority;

			public FormCompass Parent;
			public ToolTip ToolTipInfo;


			public TableEnemyCandidateControl(FormCompass parent)
			{

				#region Initialize

				Parent = parent;
				ToolTipInfo = parent.ToolTipInfo;


				ShipNames = new ImageLabel[6];
				for (int i = 0; i < ShipNames.Length; i++)
				{
					ShipNames[i] = InitializeImageLabel();
					ShipNames[i].Cursor = Cursors.Help;
					ShipNames[i].MouseClick += TableEnemyCandidateControl_MouseClick;
				}

				Formation = InitializeImageLabel();
				Formation.Anchor = AnchorStyles.None;
				/*
				Formation.ImageAlign = ContentAlignment.MiddleLeft;
				Formation.ImageList = ResourceManager.Instance.Icons;
				Formation.ImageIndex = -1;
				*/

				AirSuperiority = InitializeImageLabel();
				AirSuperiority.Anchor = AnchorStyles.Right;
				AirSuperiority.ImageAlign = ContentAlignment.MiddleLeft;
				AirSuperiority.ImageList = ResourceManager.Instance.Equipments;
				AirSuperiority.ImageIndex = (int)ResourceManager.EquipmentContent.CarrierBasedFighter;


				ConfigurationChanged();

				#endregion

			}

			private ImageLabel InitializeImageLabel()
			{
				var label = new ImageLabel
				{
					Anchor = AnchorStyles.Left,
					ForeColor = Parent.MainFontColor,
					ImageAlign = ContentAlignment.MiddleCenter,
					Padding = new Padding(0, 1, 0, 1),
					Margin = new Padding(4, 0, 4, 1),
					AutoEllipsis = true,
					AutoSize = true
				};

				return label;
			}



			public TableEnemyCandidateControl(FormCompass parent, TableLayoutPanel table, int column)
				: this(parent)
			{

				AddToTable(table, column);
			}

			public void AddToTable(TableLayoutPanel table, int column)
			{

				table.ColumnCount = Math.Max(table.ColumnCount, column + 1);
				table.RowCount = Math.Max(table.RowCount, 8);

				for (int i = 0; i < 6; i++)
					table.Controls.Add(ShipNames[i], column, i);
				table.Controls.Add(Formation, column, 6);
				table.Controls.Add(AirSuperiority, column, 7);

			}


			public void ConfigurationChanged()
			{
				for (int i = 0; i < ShipNames.Length; i++)
					ShipNames[i].Font = Parent.MainFont;
				Formation.Font = AirSuperiority.Font = Parent.MainFont;

				var maxSize = new Size(Utility.Configuration.Config.FormCompass.MaxShipNameWidth, int.MaxValue);
				foreach (var label in ShipNames)
					label.MaximumSize = maxSize;
				Formation.MaximumSize = maxSize;
				AirSuperiority.MaximumSize = maxSize;
			}

			public void Update(EnemyFleetRecord.EnemyFleetElement fleet)
			{

				if (fleet == null)
				{
					for (int i = 0; i < 6; i++)
						ShipNames[i].Visible = false;
					Formation.Visible = false;
					AirSuperiority.Visible = false;
					ToolTipInfo.SetToolTip(AirSuperiority, null);

					return;
				}

				for (int i = 0; i < 6; i++)
				{

					var ship = KCDatabase.Instance.MasterShips[fleet.FleetMember[i]];

					// カッコカリ 上のとマージするといいかもしれない

					if (ship == null)
					{
						// nothing
						ShipNames[i].Text = "-";
						ShipNames[i].ForeColor = UIColorScheme.Colors.MainFG;
						ShipNames[i].Tag = -1;
						ShipNames[i].Cursor = Cursors.Default;
						ToolTipInfo.SetToolTip(ShipNames[i], null);

					}
					else
					{

						ShipNames[i].Text = ship.Name;
						ShipNames[i].ForeColor = ship.GetShipNameColor();
						ShipNames[i].Tag = ship.ShipID;
						ShipNames[i].Cursor = Cursors.Help;
						ToolTipInfo.SetToolTip(ShipNames[i], GetShipString(ship.ShipID, ship.DefaultSlot?.ToArray()));
					}

					ShipNames[i].Visible = true;

				}

				Formation.Text = Constants.GetFormationShort(fleet.Formation);
				//Formation.ImageIndex = (int)ResourceManager.IconContent.BattleFormationEnemyLineAhead + fleet.Formation - 1;
				Formation.Visible = true;

				{
					int air = Calculator.GetAirSuperiority(fleet.FleetMember);
					AirSuperiority.Text = air.ToString();
					ToolTipInfo.SetToolTip(AirSuperiority, GetAirSuperiorityString(air));
					AirSuperiority.Visible = true;
				}

			}


			void TableEnemyCandidateControl_MouseClick(object sender, MouseEventArgs e)
			{

				if ((e.Button & System.Windows.Forms.MouseButtons.Right) != 0)
				{
					int shipID = ((ImageLabel)sender).Tag as int? ?? -1;

					if (shipID != -1)
						new DialogAlbumMasterShip(shipID).Show(Parent);
				}
			}

		}



		#region ***Control method

		private static string GetShipString(int shipID, int[] slot)
		{

			ShipDataMaster ship = KCDatabase.Instance.MasterShips[shipID];
			if (ship == null) return null;

			return GetShipString(shipID, slot, -1, ship.HPMin, ship.FirepowerMax, ship.TorpedoMax, ship.AAMax, ship.ArmorMax,
				 ship.ASW != null && !ship.ASW.IsMaximumDefault ? ship.ASW.Maximum : -1,
				 ship.Evasion != null && !ship.Evasion.IsMaximumDefault ? ship.Evasion.Maximum : -1,
				 ship.LOS != null && !ship.LOS.IsMaximumDefault ? ship.LOS.Maximum : -1,
				 ship.LuckMin);
		}

		private static string GetShipString(int shipID, int[] slot, int level, int hp, int firepower, int torpedo, int aa, int armor)
		{
			ShipDataMaster ship = KCDatabase.Instance.MasterShips[shipID];
			if (ship == null) return null;

			return GetShipString(shipID, slot, level, hp, firepower, torpedo, aa, armor,
				ship.ASW != null && ship.ASW.IsAvailable ? ship.ASW.GetParameter(level) : -1,
				ship.Evasion != null && ship.Evasion.IsAvailable ? ship.Evasion.GetParameter(level) : -1,
				ship.LOS != null && ship.LOS.IsAvailable ? ship.LOS.GetParameter(level) : -1,
				level > 99 ? Math.Min(ship.LuckMin + 3, ship.LuckMax) : ship.LuckMin);
		}

		private static string GetShipString(int shipID, int[] slot, int level, int hp, int firepower, int torpedo, int aa, int armor, int asw, int evasion, int los, int luck)
		{

			ShipDataMaster ship = KCDatabase.Instance.MasterShips[shipID];
			if (ship == null) return null;

			int firepower_c = firepower;
			int torpedo_c = torpedo;
			int aa_c = aa;
			int armor_c = armor;
			int asw_c = asw;
			int evasion_c = evasion;
			int los_c = los;
			int luck_c = luck;
			int range = ship.Range;

			asw = Math.Max(asw, 0);
			evasion = Math.Max(evasion, 0);
			los = Math.Max(los, 0);

			if (slot != null)
			{
				int count = slot.Length;
				for (int i = 0; i < count; i++)
				{
					EquipmentDataMaster eq = KCDatabase.Instance.MasterEquipments[slot[i]];
					if (eq == null) continue;

					firepower += eq.Firepower;
					torpedo += eq.Torpedo;
					aa += eq.AA;
					armor += eq.Armor;
					asw += eq.ASW;
					evasion += eq.Evasion;
					los += eq.LOS;
					luck += eq.Luck;
					range = Math.Max(range, eq.Range);
				}
			}


			var sb = new StringBuilder();

			sb.Append(ship.ShipTypeName).Append(" ").AppendLine(ship.NameWithClass);
			if (level > 0)
				sb.Append("Lv. ").Append(level.ToString());

			switch (UILanguage) {
				case "zh":
					sb.AppendLine($"（ID：{shipID}）");
					sb.AppendLine($"耐久：{hp}");
					sb.AppendLine($"火力：{firepower_c}{(firepower_c != firepower ? $"/{firepower}" : "")}");
					sb.AppendLine($"雷装：{torpedo_c}{(torpedo_c != torpedo ? $"/{torpedo}" : "")}");
					sb.AppendLine($"对空：{aa_c}{(aa_c != aa ? $"/{aa}" : "")}");
					sb.AppendLine($"装甲：{armor_c}{(armor_c != armor ? $"/{armor}" : "")}");
					sb.AppendLine($"对潜：{(asw_c < 0 ? "???" : $"{asw_c}")}{(asw_c != asw ? $"/{asw}" : "")}");
					sb.AppendLine($"回避：{(evasion_c < 0 ? "???" : $"{evasion_c}")}{(evasion_c != evasion ? $"/{evasion}" : "")}");
					sb.AppendLine($"索敌：{(los_c < 0 ? "???" : $"{los_c}")}{(los_c != los ? $"/{los}" : "")}");
					sb.AppendLine($"运：{luck_c}{(luck_c != luck ? $"/{luck}" : "")}");
					sb.AppendLine($"射程：{Constants.GetRange(range)} / 速度：{Constants.GetSpeed(ship.Speed)}\r\n（点击右键转到图鉴）");
					break;
				case "en":
					sb.AppendLine($"(ID: {shipID})");
					sb.AppendLine($"HP: {hp}");
					sb.AppendLine($"Firepower: {firepower_c}{(firepower_c != firepower ? $"/{firepower}" : "")}");
					sb.AppendLine($"Torpedo: {torpedo_c}{(torpedo_c != torpedo ? $"/{torpedo}" : "")}");
					sb.AppendLine($"AA: {aa_c}{(aa_c != aa ? $"/{aa}" : "")}");
					sb.AppendLine($"Armor: {armor_c}{(armor_c != armor ? $"/{armor}" : "")}");
					sb.AppendLine($"ASW: {(asw_c < 0 ? "???" : $"{asw_c}")}{(asw_c != asw ? $"/{asw}" : "")}");
					sb.AppendLine($"Evasion: {(evasion_c < 0 ? "???" : $"{evasion_c}")}{(evasion_c != evasion ? $"/{evasion}" : "")}");
					sb.AppendLine($"LOS: {(los_c < 0 ? "???" : $"{los_c}")}{(los_c != los ? $"/{los}" : "")}");
					sb.AppendLine($"Luck: {luck_c}{(luck_c != luck ? $"/{luck}" : "")}");
					sb.AppendLine($"Range: {Constants.GetRange(range)} / Speed: {Constants.GetSpeed(ship.Speed)}\r\n(Right-click for Album)");
					break;
				default:
					sb.AppendLine($" (ID: {shipID})");
					sb.AppendLine($"耐久: {hp}");
					sb.AppendLine($"火力: {firepower_c}{(firepower_c != firepower ? $"/{firepower}" : "")}");
					sb.AppendLine($"雷装: {torpedo_c}{(torpedo_c != torpedo ? $"/{torpedo}" : "")}");
					sb.AppendLine($"対空: {aa_c}{(aa_c != aa ? $"/{aa}" : "")}");
					sb.AppendLine($"装甲: {armor_c}{(armor_c != armor ? $"/{armor}" : "")}");
					sb.AppendLine($"対潜: {(asw_c < 0 ? "???" : $"{asw_c}")}{(asw_c != asw ? $"/{asw}" : "")}");
					sb.AppendLine($"回避: {(evasion_c < 0 ? "???" : $"{evasion_c}")}{(evasion_c != evasion ? $"/{evasion}" : "")}");
					sb.AppendLine($"索敵: {(los_c < 0 ? "???" : $"{los_c}")}{(los_c != los ? $"/{los}" :"")}");
					sb.AppendLine($"運: {luck_c}{(luck_c != luck ? $"/{luck}" : "")}");
					sb.AppendLine($"射程: {Constants.GetRange(range)} / 速力: {Constants.GetSpeed(ship.Speed)}\r\n(右クリックで図鑑)");
					break;
			}

			return sb.ToString();

		}

		private static string GetEquipmentString(int shipID, int[] slot)
		{
			StringBuilder sb = new StringBuilder();
			ShipDataMaster ship = KCDatabase.Instance.MasterShips[shipID];

			if (ship == null || slot == null) return null;

			for (int i = 0; i < slot.Length; i++)
			{
				var eq = KCDatabase.Instance.MasterEquipments[slot[i]];
				if (eq != null)
					sb.AppendFormat("[{0}] {1}\r\n", ship.Aircraft[i], eq.Name);
			}

			switch (UILanguage) {
				case "zh":
					sb.AppendLine("\r\n" +
						$"昼战：{Constants.GetDayAttackKind(Calculator.GetDayAttackKind(slot, ship.ShipID, -1))}\r\n" +
						$"夜战：{Constants.GetNightAttackKind(Calculator.GetNightAttackKind(slot, ship.ShipID, -1))}");
					break;
				case "en":
					sb.AppendLine("\r\n" +
						$"Day: {Constants.GetDayAttackKind(Calculator.GetDayAttackKind(slot, ship.ShipID, -1))}\r\n" +
						$"Night: {Constants.GetNightAttackKind(Calculator.GetNightAttackKind(slot, ship.ShipID, -1))}");
					break;
				default:
					sb.AppendLine("\r\n" +
						$"昼戦: {Constants.GetDayAttackKind(Calculator.GetDayAttackKind(slot, ship.ShipID, -1))}\r\n" +
						$"夜戦: {Constants.GetNightAttackKind(Calculator.GetNightAttackKind(slot, ship.ShipID, -1))}");
					break;
			}

			{
				int aacutin = Calculator.GetAACutinKind(shipID, slot);
				if (aacutin != 0)
				{
					switch (UILanguage) {
						case "zh":
							sb.AppendLine($"对空：{Constants.GetAACutinKind(aacutin)}");
							break;
						case "en":
							sb.AppendLine($"AACI: {Constants.GetAACutinKind(aacutin)}");
							break;
						default:
							sb.AppendLine($"対空: {Constants.GetAACutinKind(aacutin)}");
							break;
					}
				}
			}
			{
				int airsup = Calculator.GetAirSuperiority(slot, ship.Aircraft.ToArray());
				if (airsup > 0)
				{
					switch (UILanguage) {
						case "zh":
							sb.AppendLine($"制空战力：{airsup}");
							break;
						case "en":
							sb.AppendLine($"Fighter Power: {airsup}");
							break;
						default:
							sb.AppendLine($"制空戦力: {airsup}");
							break;
					}
				}
			}

			return sb.ToString();
		}

		private static string GetAirSuperiorityString(int air)
		{
			if (air > 0)
			{
				switch (UILanguage) {
					case "zh":
						return
							$"确保：{(int)(air * 3.0)}\r\n" +
							$"优势：{(int)Math.Ceiling(air * 1.5)}\r\n" +
							$"均衡：{(int)(air / 1.5 + 1)}\r\n" +
							$"劣势：{(int)(air / 3.0 + 1)}\r\n";
					case "en":
						return
							$"Air Supremacy: {(int)(air * 3.0)}\r\n" +
							$"Air Superiority: {(int)Math.Ceiling(air * 1.5)}\r\n" +
							$"Air Parity: {(int)(air / 1.5 + 1)}\r\n" +
							$"Air Denial: {(int)(air / 3.0 + 1)}\r\n";
					default:
						return
							$"確保: {(int)(air * 3.0)}\r\n" +
							$"優勢: {(int)Math.Ceiling(air * 1.5)}\r\n" +
							$"均衡: {(int)(air / 1.5 + 1)}\r\n" +
							$"劣勢: {(int)(air / 3.0 + 1)}\r\n";
				}
			}
			return null;
		}

		#endregion




		public Font MainFont { get; set; }
		public Font SubFont { get; set; }
		public Color MainFontColor { get; set; }
		public Color SubFontColor { get; set; }


		private TableEnemyMemberControl[] ControlMembers;
		private TableEnemyCandidateControl[] ControlCandidates;

		private int _candidatesDisplayCount;


		/// <summary>
		/// 次に遭遇する敵艦隊候補
		/// </summary>
		private List<EnemyFleetRecord.EnemyFleetElement> _enemyFleetCandidate = null;

		/// <summary>
		/// 表示中の敵艦隊候補のインデックス
		/// </summary>
		private int _enemyFleetCandidateIndex = 0;




		private static string UILanguage;

		public FormCompass(FormMain parent)
		{
			InitializeComponent();

			UILanguage = parent.UILanguage;
			ForeColor = parent.ForeColor;
			BackColor = parent.BackColor;

			switch (UILanguage) {
				case "zh":
					Text = "罗盘";
					break;
				case "en":
					Text = "Compass";
					break;
				default:
					break;
			}



			MainFontColor = Color.FromArgb(0x00, 0x00, 0x00);
			SubFontColor = Color.FromArgb(0x88, 0x88, 0x88);


			ControlHelper.SetDoubleBuffered(BasePanel);
			ControlHelper.SetDoubleBuffered(TableEnemyFleet);
			ControlHelper.SetDoubleBuffered(TableEnemyMember);


			TableEnemyMember.SuspendLayout();
			ControlMembers = new TableEnemyMemberControl[6];
			for (int i = 0; i < ControlMembers.Length; i++)
			{
				ControlMembers[i] = new TableEnemyMemberControl(this, TableEnemyMember, i);
			}
			TableEnemyMember.ResumeLayout();

			TableEnemyCandidate.SuspendLayout();
			ControlCandidates = new TableEnemyCandidateControl[6];
			for (int i = 0; i < ControlCandidates.Length; i++)
			{
				ControlCandidates[i] = new TableEnemyCandidateControl(this, TableEnemyCandidate, i);
			}
			TableEnemyCandidate.ResumeLayout();


			//BasePanel.SetFlowBreak( TextMapArea, true );
			BasePanel.SetFlowBreak(TextDestination, true);
			//BasePanel.SetFlowBreak( TextEventKind, true );
			BasePanel.SetFlowBreak(TextEventDetail, true);


			TextDestination.ImageList = ResourceManager.Instance.Equipments;
			TextEventKind.ImageList = ResourceManager.Instance.Equipments;
			TextEventDetail.ImageList = ResourceManager.Instance.Equipments;
			TextFormation.ImageList = ResourceManager.Instance.Icons;
			TextAirSuperiority.ImageList = ResourceManager.Instance.Equipments;
			TextAirSuperiority.ImageIndex = (int)ResourceManager.EquipmentContent.CarrierBasedFighter;



			ConfigurationChanged();

			Icon = ResourceManager.ImageToIcon(ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormCompass]);

		}


		private void FormCompass_Load(object sender, EventArgs e)
		{

			BasePanel.Visible = false;


			APIObserver o = APIObserver.Instance;

			o["api_port/port"].ResponseReceived += Updated;
			o["api_req_map/start"].ResponseReceived += Updated;
			o["api_req_map/next"].ResponseReceived += Updated;
			o["api_req_member/get_practice_enemyinfo"].ResponseReceived += Updated;

			o["api_req_sortie/battle"].ResponseReceived += BattleStarted;
			o["api_req_battle_midnight/sp_midnight"].ResponseReceived += BattleStarted;
			o["api_req_sortie/night_to_day"].ResponseReceived += BattleStarted;
			o["api_req_sortie/airbattle"].ResponseReceived += BattleStarted;
			o["api_req_sortie/ld_airbattle"].ResponseReceived += BattleStarted;
			o["api_req_combined_battle/battle"].ResponseReceived += BattleStarted;
			o["api_req_combined_battle/sp_midnight"].ResponseReceived += BattleStarted;
			o["api_req_combined_battle/airbattle"].ResponseReceived += BattleStarted;
			o["api_req_combined_battle/battle_water"].ResponseReceived += BattleStarted;
			o["api_req_combined_battle/ld_airbattle"].ResponseReceived += BattleStarted;
			o["api_req_combined_battle/ec_battle"].ResponseReceived += BattleStarted;
			o["api_req_combined_battle/each_battle"].ResponseReceived += BattleStarted;
			o["api_req_combined_battle/each_battle_water"].ResponseReceived += BattleStarted;
			o["api_req_combined_battle/ec_night_to_day"].ResponseReceived += BattleStarted;
			o["api_req_practice/battle"].ResponseReceived += BattleStarted;


			Utility.Configuration.Instance.ConfigurationChanged += ConfigurationChanged;
		}


		private void Updated(string apiname, dynamic data)
		{

			Func<int, Color> getColorFromEventKind = (int kind) =>
			{
				switch (kind)
				{
					case 0:
					case 1:
					default:    //昼夜戦・その他
						return UIColorScheme.Colors.MainFG;
					case 2:
					case 3:     //夜戦・夜昼戦
						return UIColorScheme.Colors.Compass_TextEventKind3;
					case 4:     //航空戦
					case 6:     //長距離空襲戦
						return UIColorScheme.Colors.Compass_TextEventKind6;
					case 5:     // 敵連合
						return UIColorScheme.Colors.Compass_TextEventKind5;
					case 7:     // 夜昼戦(対連合艦隊)
						return UIColorScheme.Colors.Compass_TextEventKind3;
				}
			};

			if (apiname == "api_port/port")
			{

				BasePanel.Visible = false;

			}
			else if (apiname == "api_req_member/get_practice_enemyinfo")
			{
				switch (UILanguage) {
					case "zh":
						TextMapArea.Text = "演习";
						break;
					case "en":
						TextMapArea.Text = "Exercise";
						break;
					default:
						TextMapArea.Text = "演習";
						break;
				}
				TextDestination.Text = string.Format("{0} {1}", data.api_nickname, Constants.GetAdmiralRank((int)data.api_rank));
				TextDestination.ImageAlign = ContentAlignment.MiddleCenter;
				TextDestination.ImageIndex = -1;
				ToolTipInfo.SetToolTip(TextDestination, null);
				TextEventKind.Text = data.api_cmt;
				TextEventKind.ForeColor = getColorFromEventKind(0);
				TextEventKind.ImageAlign = ContentAlignment.MiddleCenter;
				TextEventKind.ImageIndex = -1;
				ToolTipInfo.SetToolTip(TextEventKind, null);
				TextEventDetail.Text = string.Format("Lv. {0} / {1} exp.", data.api_level, data.api_experience[0]);
				TextEventDetail.ImageAlign = ContentAlignment.MiddleCenter;
				TextEventDetail.ImageIndex = -1;
				ToolTipInfo.SetToolTip(TextEventDetail, null);
				TextEnemyFleetName.Text = data.api_deckname;

			}
			else
			{

				CompassData compass = KCDatabase.Instance.Battle.Compass;


				BasePanel.SuspendLayout();
				PanelEnemyFleet.Visible = false;
				PanelEnemyCandidate.Visible = false;

				_enemyFleetCandidate = null;
				_enemyFleetCandidateIndex = -1;


				switch (UILanguage) {
					case "zh":
						TextMapArea.Text = $"出击海域：{compass.MapAreaID}-{compass.MapInfoID}{(compass.MapInfo.EventDifficulty > 0 ? " [" + Constants.GetDifficulty(compass.MapInfo.EventDifficulty) + "]" : "")}";
						break;
					case "en":
						TextMapArea.Text = $"Sortie Area: {compass.MapAreaID}-{compass.MapInfoID}{(compass.MapInfo.EventDifficulty > 0 ? " [" + Constants.GetDifficulty(compass.MapInfo.EventDifficulty) + "]" : "")}";
						break;
					default:
						TextMapArea.Text = $"出撃海域 : {compass.MapAreaID}-{compass.MapInfoID}{(compass.MapInfo.EventDifficulty > 0 ? " [" + Constants.GetDifficulty(compass.MapInfo.EventDifficulty) + "]" : "")}";
						break;
				}
				{
					var mapinfo = compass.MapInfo;

					if (mapinfo.IsCleared)
					{
						ToolTipInfo.SetToolTip(TextMapArea, null);

					}
					else if (mapinfo.RequiredDefeatedCount != -1)
					{
						switch (UILanguage) {
							case "zh":
								ToolTipInfo.SetToolTip(TextMapArea, $"击破：{mapinfo.CurrentDefeatedCount} / {mapinfo.RequiredDefeatedCount} 次");
								break;
							case "en":
								ToolTipInfo.SetToolTip(TextMapArea, $"Defeat Count: {mapinfo.CurrentDefeatedCount} / {mapinfo.RequiredDefeatedCount}");
								break;
							default:
								ToolTipInfo.SetToolTip(TextMapArea, $"撃破: {mapinfo.CurrentDefeatedCount} / {mapinfo.RequiredDefeatedCount} 回");
								break;
						}
					}
					else if (mapinfo.MapHPMax > 0)
					{
						int current = compass.MapHPCurrent > 0 ? compass.MapHPCurrent : mapinfo.MapHPCurrent;
						int max = compass.MapHPMax > 0 ? compass.MapHPMax : mapinfo.MapHPMax;

						ToolTipInfo.SetToolTip(TextMapArea, string.Format("{0}{1}: {2} / {3}",
							mapinfo.CurrentGaugeIndex > 0 ? "#" + mapinfo.CurrentGaugeIndex + " " : "",
							mapinfo.GaugeType == 3 ? "TP" : "HP", current, max));

					}
					else
					{
						ToolTipInfo.SetToolTip(TextMapArea, null);
					}
				}


				switch (UILanguage) {
					case "zh":
						TextDestination.Text = $"下一航路：{compass.Destination}{(compass.IsEndPoint ? "（终点）" : "")}";
						break;
					case "en":
						TextDestination.Text = $"Next Route: {compass.Destination}{(compass.IsEndPoint ? " (End Point)" : "")}";
						break;
					default:
						TextDestination.Text = $"次のセル : {compass.Destination}{(compass.IsEndPoint ? " (終点)" : "")}";
						break;
				}
				if (compass.LaunchedRecon != 0)
				{
					TextDestination.ImageAlign = ContentAlignment.MiddleRight;
					TextDestination.ImageIndex = (int)ResourceManager.EquipmentContent.Seaplane;

					string tiptext;
					switch (UILanguage) {
						case "zh":
							switch (compass.CommentID) {
								case 1:
									tiptext = "发现敌舰队！";
									break;
								case 2:
									tiptext = "发现攻击目标！";
									break;
								case 3:
									tiptext = "航路巡逻！";
									break;
								default:
									tiptext = "索敌机离舰！";
									break;
							}
							break;
						case "en":
							switch (compass.CommentID) {
								case 1:
									tiptext = "Enemy Fleet Spotted!";
									break;
								case 2:
									tiptext = "Target Spotted!";
									break;
								case 3:
									tiptext = "Route Patrol!";
									break;
								default:
									tiptext = "Patrol Aircraft Take Off!";
									break;
							}
							break;
						default:
							switch (compass.CommentID) {
								case 1:
									tiptext = "敵艦隊発見！";
									break;
								case 2:
									tiptext = "攻撃目標発見！";
									break;
								case 3:
									tiptext = "針路哨戒！";
									break;
								default:
									tiptext = "索敵機発艦！";
									break;
							}
							break;
					}
					ToolTipInfo.SetToolTip(TextDestination, tiptext);

				}
				else
				{
					TextDestination.ImageAlign = ContentAlignment.MiddleCenter;
					TextDestination.ImageIndex = -1;
					ToolTipInfo.SetToolTip(TextDestination, null);
				}

				//とりあえずリセット
				TextEventDetail.ImageAlign = ContentAlignment.MiddleCenter;
				TextEventDetail.ImageIndex = -1;
				ToolTipInfo.SetToolTip(TextEventDetail, null);


				TextEventKind.ForeColor = getColorFromEventKind(0);

				{
					string eventkind = Constants.GetMapEventID(compass.EventID);

					switch (compass.EventID)
					{

						case 0:     //初期位置
							switch (UILanguage) {
								case "zh":
									TextEventDetail.Text = "为什么会变成这样呢？";
									break;
								case "en":
									TextEventDetail.Text = "Why did things end up like this?";
									break;
								default:
									TextEventDetail.Text = "どうしてこうなった";
									break;
							}
							break;

						case 2:     //資源
						case 8:     //船団護衛成功
							TextEventDetail.Text = GetMaterialInfo(compass);
							break;

						case 3:     //渦潮
							{
								int materialmax = KCDatabase.Instance.Fleet.Fleets.Values
									.Where(f => f != null && f.IsInSortie)
									.SelectMany(f => f.MembersWithoutEscaped)
									.Max(s =>
									{
										if (s == null) return 0;
										switch (compass.WhirlpoolItemID)
										{
											case 1:
												return s.Fuel;
											case 2:
												return s.Ammo;
											default:
												return 0;
										}
									});

								TextEventDetail.Text = string.Format("{0} x {1} ({2:p0})",
									Constants.GetMaterialName(compass.WhirlpoolItemID),
									compass.WhirlpoolItemAmount,
									(double)compass.WhirlpoolItemAmount / Math.Max(materialmax, 1));

							}
							break;

						case 4:     //通常戦闘
							if (compass.EventKind >= 2)
							{
								eventkind += " / " + Constants.GetMapEventKind(compass.EventKind);

								TextEventKind.ForeColor = getColorFromEventKind(compass.EventKind);
							}
							UpdateEnemyFleet();
							break;

						case 5:     //ボス戦闘
							TextEventKind.ForeColor = UIColorScheme.Colors.Red;

							if (compass.EventKind >= 2)
							{
								eventkind += " / " + Constants.GetMapEventKind(compass.EventKind);
							}
							UpdateEnemyFleet();
							break;

						case 1:     //イベントなし
						case 6:     //気のせいだった
							switch (UILanguage) {
								case "zh":
									switch (compass.EventKind) {
										case 0:
										default:
											break;
										case 1:
											eventkind = "不见敌影";
											break;
										case 2:
											eventkind = "能動分岐";
											break;
										case 3:
											eventkind = "平静的大海";
											break;
										case 4:
											eventkind = "平静的海峡";
											break;
										case 5:
											eventkind = "注意警戒";
											break;
										case 6:
											eventkind = "静谧的海洋";
											break;
										case 7:
											eventkind = "正在对潜警戒";
											break;
										case 8:
											eventkind = "发现敌巡逻机";
											break;
										case 9:
											eventkind = "栗田舰队进击中";
											break;
										case 10:
											eventkind = "西村舰队进击中";
											break;
										case 11:
											eventkind = "突入苏里高海峡";
											break;
										case 12:
											eventkind = "突入锡布延海";
											break;
										case 13:
											eventkind = "运输作战失败";
											break;
										case 14:
											eventkind = "突入锡布延海";
											break;
										case 15:
											eventkind = "向萨马岛海岸进击中";
											break;
										case 16:
											eventkind = "西村舰队突入";
											break;
										case 17:
											eventkind = "小泽舰队出击";
											break;
										case 18:
											eventkind = "班乃岛";
											break;
										case 19:
											eventkind = "入港棉兰老岛";
											break;
										case 20:
											eventkind = "志摩舰队出击";
											break;
										case 21:
											eventkind = "发现敌侦察机";
											break;
										case 22:
											eventkind = "对空对潜警戒";
											break;
										case 23:
											eventkind = "高速舰艇出击";
											break;
										case 24:
											eventkind = "机动部队出击";
											break;
										case 25:
											eventkind = "舰队决战";
											break;
									}
									break;
								case "en":
									switch (compass.EventKind) {
										case 0:
										default:
											break;
										case 1:
											eventkind = "Enemy Lost";
											break;
										case 2:
											eventkind = "Route Select";
											break;
										case 3:
											eventkind = "A Calm Sea";
											break;
										case 4:
											eventkind = "A Calm Strait";
											break;
										case 5:
											eventkind = "Caution Needed";
											break;
										case 6:
											eventkind = "A Quiet Sea";
											break;
										case 7:
											eventkind = "Proceed with ASW Caution";
											break;
										case 8:
											eventkind = "Enemy Patrol Aircraft Spotted";
											break;
										case 9:
											eventkind = "Kurita Fleet Advancing";
											break;
										case 10:
											eventkind = "Nishimura Fleet Advancing";
											break;
										case 11:
											eventkind = "Entering Straits of Surigao";
											break;
										case 12:
											eventkind = "Entering Sibuyan Sea";
											break;
										case 13:
											eventkind = "Transportation Operation Failed";
											break;
										case 14:
											eventkind = "Entering Sibuyan Sea";
											break;
										case 15:
											eventkind = "Entering Coast of Samar";
											break;
										case 16:
											eventkind = "Nishimura Fleet Advancing";
											break;
										case 17:
											eventkind = "Ozawa Fleet Advancing";
											break;
										case 18:
											eventkind = "Panay Island";
											break;
										case 19:
											eventkind = "Porting into Mindanao";
											break;
										case 20:
											eventkind = "Shima Fleet Sortie";
											break;
										case 21:
											eventkind = "Enemy Patrol Aircraft Spotted";
											break;
										case 22:
											eventkind = "Proceed With AA&ASW Caution";
											break;
										case 23:
											eventkind = "High-speed Ships Sortie";
											break;
										case 24:
											eventkind = "Carrier Task Force Sortie";
											break;
										case 25:
											eventkind = "Decisive Battle Doctrine";
											break;
									}
									break;
								default:
									switch (compass.EventKind) {
										case 0:     //気のせいだった
										default:
											break;
										case 1:
											eventkind = "敵影を見ず";
											break;
										case 2:
											eventkind = "能動分岐";
											break;
										case 3:
											eventkind = "穏やかな海";
											break;
										case 4:
											eventkind = "穏やかな海峡";
											break;
										case 5:
											eventkind = "警戒が必要";
											break;
										case 6:
											eventkind = "静かな海";
											break;
										case 7:
											eventkind = "対潜警戒進撃中";
											break;
										case 8:
											eventkind = "敵哨戒機発見";
											break;
										case 9:
											eventkind = "栗田艦隊進撃中";
											break;
										case 10:
											eventkind = "西村艦隊進撃中";
											break;
										case 11:
											eventkind = "スリガオ海峡突入"; // 西村
											break;
										case 12:
											eventkind = "シブヤン海突入";
											break;
										case 13:
											eventkind = "輸送作戦失敗";
											break;
										case 14:
											eventkind = "シブヤン海進撃中"; // 栗田
											break;
										case 15:
											eventkind = "サマール沖進撃中";
											break;
										case 16:
											eventkind = "西村艦隊突入"; // 西村
											break;
										case 17:
											eventkind = "小沢艦隊出撃";
											break;
										case 18:
											eventkind = "パナイ島";
											break;
										case 19:
											eventkind = "ミンダナオ島入港";
											break;
										case 20:
											eventkind = "志摩艦隊出撃";
											break;
										case 21:
											eventkind = "敵哨戒機発見";
											break;
										case 22:
											eventkind = "対空対潜警戒";
											break;
										case 23:
											eventkind = "高速艦艇出撃";
											break;
										case 24:
											eventkind = "機動部隊出撃";
											break;
										case 25:
											eventkind = "艦隊決戦";
											break;
									}
									break;
							}
							if (compass.RouteChoices != null)
								TextEventDetail.Text = string.Join("/", compass.RouteChoices);
							else
								TextEventDetail.Text = "";

							break;

						case 7:     //航空戦or航空偵察
							TextEventKind.ForeColor = getColorFromEventKind(compass.EventKind);

							switch (compass.EventKind)
							{
								case 0:     //航空偵察
									switch (UILanguage) {
										case "zh":
											eventkind = "航空侦察";
											break;
										case "en":
											eventkind = "Aerial Reconnaissance";
											break;
										default:
											eventkind = "航空偵察";
											break;
									}

									switch (UILanguage) {
										case "zh":
											switch (compass.AirReconnaissanceResult) {
												case 0:
												default:
													TextEventDetail.Text = "失败";
													break;
												case 1:
													TextEventDetail.Text = "成功";
													break;
												case 2:
													TextEventDetail.Text = "大成功";
													break;
											}
											break;
										case "en":
											switch (compass.AirReconnaissanceResult) {
												case 0:
												default:
													TextEventDetail.Text = "Fail";
													break;
												case 1:
													TextEventDetail.Text = "Success";
													break;
												case 2:
													TextEventDetail.Text = "Great Success";
													break;
											}
											break;
										default:
											switch (compass.AirReconnaissanceResult) {
												case 0:
												default:
													TextEventDetail.Text = "失敗";
													break;
												case 1:
													TextEventDetail.Text = "成功";
													break;
												case 2:
													TextEventDetail.Text = "大成功";
													break;
											}
											break;
									}

									switch (compass.AirReconnaissancePlane)
									{
										case 0:
										default:
											TextEventDetail.ImageAlign = ContentAlignment.MiddleCenter;
											TextEventDetail.ImageIndex = -1;
											break;
										case 1:
											TextEventDetail.ImageAlign = ContentAlignment.MiddleLeft;
											TextEventDetail.ImageIndex = (int)ResourceManager.EquipmentContent.FlyingBoat;
											break;
										case 2:
											TextEventDetail.ImageAlign = ContentAlignment.MiddleLeft;
											TextEventDetail.ImageIndex = (int)ResourceManager.EquipmentContent.Seaplane;
											break;
									}

									if (compass.GetItems.Any())
									{
										TextEventDetail.Text += "　" + GetMaterialInfo(compass);
									}

									break;

								case 4:     //航空戦
								default:
									UpdateEnemyFleet();
									break;
							}
							break;

						case 9:     //揚陸地点
							TextEventDetail.Text = "";
							break;

						default:
							TextEventDetail.Text = "";
							break;

					}
					TextEventKind.Text = eventkind;
				}


				if (compass.HasAirRaid)
				{
					TextEventKind.ImageAlign = ContentAlignment.MiddleRight;
					TextEventKind.ImageIndex = (int)ResourceManager.EquipmentContent.CarrierBasedBomber;
					switch (UILanguage) {
						case "zh":
							ToolTipInfo.SetToolTip(TextEventKind, "空袭 - " + Constants.GetAirRaidDamage(compass.AirRaidDamageKind));
							break;
						case "en":
							ToolTipInfo.SetToolTip(TextEventKind, "Air Raid - " + Constants.GetAirRaidDamage(compass.AirRaidDamageKind));
							break;
						default:
							ToolTipInfo.SetToolTip(TextEventKind, "空襲 - " + Constants.GetAirRaidDamage(compass.AirRaidDamageKind));
							break;
					}
				}
				else
				{
					TextEventKind.ImageAlign = ContentAlignment.MiddleCenter;
					TextEventKind.ImageIndex = -1;
					ToolTipInfo.SetToolTip(TextEventKind, null);
				}


				BasePanel.ResumeLayout();

				BasePanel.Visible = true;
			}


		}


		private string GetMaterialInfo(CompassData compass)
		{

			var strs = new LinkedList<string>();

			foreach (var item in compass.GetItems)
			{

				string itemName;

				if (item.ItemID == 4)
				{
					itemName = Constants.GetMaterialName(item.Metadata);

				}
				else
				{
					var itemMaster = KCDatabase.Instance.MasterUseItems[item.Metadata];
					if (itemMaster != null)
						itemName = itemMaster.Name;
					else
					{
						switch (UILanguage) {
							case "zh":
								itemName = "未知物品";
								break;
							case "en":
								itemName = "Unknown Item";
								break;
							default:
								itemName = "謎のアイテム";
								break;
						}
					}
				}

				strs.AddLast(itemName + " x " + item.Amount);
			}

			if (!strs.Any())
			{
				switch (UILanguage) {
					case "zh":
						return "（无）";
					case "en":
						return "(None)";
					default:
						return "(なし)";
				}
			}
			else
			{
				return string.Join(", ", strs);
			}
		}



		private void BattleStarted(string apiname, dynamic data)
		{
			UpdateEnemyFleetInstant(apiname.Contains("practice"));
		}





		private void UpdateEnemyFleet()
		{

			CompassData compass = KCDatabase.Instance.Battle.Compass;

			_enemyFleetCandidate = RecordManager.Instance.EnemyFleet.Record.Values.Where(
				r =>
					r.MapAreaID == compass.MapAreaID &&
					r.MapInfoID == compass.MapInfoID &&
					r.CellID == compass.Destination &&
					r.Difficulty == compass.MapInfo.EventDifficulty
				).ToList();
			_enemyFleetCandidateIndex = 0;


			if (_enemyFleetCandidate.Count == 0)
			{
				switch (UILanguage) {
					case "zh":
						TextEventDetail.Text = "（尚无敌舰队候选）";
						TextEnemyFleetName.Text = "（敌舰队名不明）";
						break;
					case "en":
						TextEventDetail.Text = "(No Enemy Fleet Candidates)";
						TextEnemyFleetName.Text = "(Enemy Fleet Name Unknown)";
						break;
					default:
						TextEventDetail.Text = "(敵艦隊候補なし)";
						TextEnemyFleetName.Text = "(敵艦隊名不明)";
						break;
				}


				TableEnemyCandidate.Visible = false;

			}
			else
			{
				_enemyFleetCandidate.Sort((a, b) =>
				{
					for (int i = 0; i < a.FleetMember.Length; i++)
					{
						int diff = a.FleetMember[i] - b.FleetMember[i];
						if (diff != 0)
							return diff;
					}
					return a.Formation - b.Formation;
				});

				NextEnemyFleetCandidate(0);
			}


			PanelEnemyFleet.Visible = false;

		}


		private void UpdateEnemyFleetInstant(bool isPractice = false)
		{

			BattleManager bm = KCDatabase.Instance.Battle;
			BattleData bd = bm.FirstBattle;

			int[] enemies = bd.Initial.EnemyMembers;
			int[][] slots = bd.Initial.EnemySlots;
			int[] levels = bd.Initial.EnemyLevels;
			int[][] parameters = bd.Initial.EnemyParameters;
			int[] hps = bd.Initial.EnemyMaxHPs;


			_enemyFleetCandidate = null;
			_enemyFleetCandidateIndex = -1;



			if (!bm.IsPractice)
			{
				var efcurrent = EnemyFleetRecord.EnemyFleetElement.CreateFromCurrentState();
				var efrecord = RecordManager.Instance.EnemyFleet[efcurrent.FleetID];
				if (efrecord != null)
				{
					TextEnemyFleetName.Text = efrecord.FleetName;
					TextEventDetail.Text = "Exp: " + efrecord.ExpShip;
				}
				switch (UILanguage) {
					case "zh":
						ToolTipInfo.SetToolTip(TextEventDetail, "敌舰队ID：" + efcurrent.FleetID.ToString("x16"));
						break;
					case "en":
						ToolTipInfo.SetToolTip(TextEventDetail, "Enemy Fleet ID: " + efcurrent.FleetID.ToString("x16"));
						break;
					default:
						ToolTipInfo.SetToolTip(TextEventDetail, "敵艦隊ID: " + efcurrent.FleetID.ToString("x16"));
						break;
				}
			}

			TextFormation.Text = Constants.GetFormationShort((int)bd.Searching.FormationEnemy);
			//TextFormation.ImageIndex = (int)ResourceManager.IconContent.BattleFormationEnemyLineAhead + bd.Searching.FormationEnemy - 1;
			TextFormation.Visible = true;
			{
				int air = Calculator.GetAirSuperiority(enemies, slots);
				TextAirSuperiority.Text = isPractice ?
					air.ToString() + " ~ " + Calculator.GetAirSuperiorityAtMaxLevel(enemies, slots).ToString() :
					air.ToString();
				ToolTipInfo.SetToolTip(TextAirSuperiority, GetAirSuperiorityString(isPractice ? 0 : air));
				TextAirSuperiority.Visible = true;
			}

			TableEnemyMember.SuspendLayout();
			for (int i = 0; i < ControlMembers.Length; i++)
			{
				int shipID = enemies[i];
				ControlMembers[i].Update(shipID, shipID != -1 ? slots[i] : null);

				if (shipID != -1)
					ControlMembers[i].UpdateEquipmentToolTip(shipID, slots[i], levels[i], hps[i], parameters[i][0], parameters[i][1], parameters[i][2], parameters[i][3]);
			}
			TableEnemyMember.ResumeLayout();
			TableEnemyMember.Visible = true;

			PanelEnemyFleet.Visible = true;

			PanelEnemyCandidate.Visible = false;

			BasePanel.Visible = true;           //checkme

		}



		private void TextEnemyFleetName_MouseDown(object sender, MouseEventArgs e)
		{

			if (e.Button == System.Windows.Forms.MouseButtons.Left)
				NextEnemyFleetCandidate();
			else if (e.Button == System.Windows.Forms.MouseButtons.Right)
				NextEnemyFleetCandidate(-_candidatesDisplayCount);
		}


		private void NextEnemyFleetCandidate()
		{
			NextEnemyFleetCandidate(_candidatesDisplayCount);
		}

		private void NextEnemyFleetCandidate(int offset)
		{

			if (_enemyFleetCandidate != null && _enemyFleetCandidate.Count != 0)
			{

				_enemyFleetCandidateIndex += offset;
				if (_enemyFleetCandidateIndex < 0)
					_enemyFleetCandidateIndex = (_enemyFleetCandidate.Count - 1) - (_enemyFleetCandidate.Count - 1) % _candidatesDisplayCount;
				else if (_enemyFleetCandidateIndex >= _enemyFleetCandidate.Count)
					_enemyFleetCandidateIndex = 0;


				var candidate = _enemyFleetCandidate[_enemyFleetCandidateIndex];


				TextEventDetail.Text = TextEnemyFleetName.Text = candidate.FleetName;

				if (_enemyFleetCandidate.Count > _candidatesDisplayCount)
				{
					TextEventDetail.Text += " ▼";
					switch (UILanguage) {
						case "zh":
							ToolTipInfo.SetToolTip(TextEventDetail, $"候选：{_enemyFleetCandidateIndex + 1} / {_enemyFleetCandidate.Count}\r\n（使用鼠标左、右键翻页）");
							break;
						case "en":
							ToolTipInfo.SetToolTip(TextEventDetail, $"Candidates: {_enemyFleetCandidateIndex + 1} / {_enemyFleetCandidate.Count}\r\n(Turn page with mouse Left/Right click)");
							break;
						default:
							ToolTipInfo.SetToolTip(TextEventDetail, $"候補: {_enemyFleetCandidateIndex + 1} / {_enemyFleetCandidate.Count}\r\n(左右クリックでページめくり)");
							break;
					}
				}
				else
				{
					switch (UILanguage) {
						case "zh":
							ToolTipInfo.SetToolTip(TextEventDetail, $"候选：{_enemyFleetCandidate.Count}");
							break;
						case "en":
							ToolTipInfo.SetToolTip(TextEventDetail, $"Candidates: {_enemyFleetCandidate.Count}");
							break;
						default:
							ToolTipInfo.SetToolTip(TextEventDetail, $"候補: {_enemyFleetCandidate.Count}");
							break;
					}
				}

				TableEnemyCandidate.SuspendLayout();
				for (int i = 0; i < ControlCandidates.Length; i++)
				{
					if (i + _enemyFleetCandidateIndex >= _enemyFleetCandidate.Count || i >= _candidatesDisplayCount)
					{
						ControlCandidates[i].Update(null);
						continue;
					}

					ControlCandidates[i].Update(_enemyFleetCandidate[i + _enemyFleetCandidateIndex]);
				}
				TableEnemyCandidate.ResumeLayout();
				TableEnemyCandidate.Visible = true;

				PanelEnemyCandidate.Visible = true;

			}
		}


		void ConfigurationChanged()
		{

			Font = PanelEnemyFleet.Font = MainFont = Utility.Configuration.Config.UI.MainFont;
			SubFont = Utility.Configuration.Config.UI.SubFont;

			TextMapArea.Font =
			TextDestination.Font =
			TextEventKind.Font =
			TextEventDetail.Font = Font;

			BasePanel.AutoScroll = Utility.Configuration.Config.FormCompass.IsScrollable;

			_candidatesDisplayCount = Utility.Configuration.Config.FormCompass.CandidateDisplayCount;
			_enemyFleetCandidateIndex = 0;
			if (PanelEnemyCandidate.Visible)
				NextEnemyFleetCandidate(0);

			if (ControlMembers != null)
			{
				TableEnemyMember.SuspendLayout();

				TableEnemyMember.Location = new Point(TableEnemyMember.Location.X, TableEnemyFleet.Bottom + 6);

				bool flag = Utility.Configuration.Config.FormFleet.ShowAircraft;
				for (int i = 0; i < ControlMembers.Length; i++)
				{
					ControlMembers[i].Equipments.ShowAircraft = flag;
					ControlMembers[i].ConfigurationChanged();
				}

				ControlHelper.SetTableRowStyles(TableEnemyMember, ControlHelper.GetDefaultRowStyle());
				TableEnemyMember.ResumeLayout();
			}


			if (ControlCandidates != null)
			{
				TableEnemyCandidate.SuspendLayout();

				for (int i = 0; i < ControlCandidates.Length; i++)
					ControlCandidates[i].ConfigurationChanged();

				ControlHelper.SetTableRowStyles(TableEnemyCandidate, new RowStyle(SizeType.AutoSize));
				ControlHelper.SetTableColumnStyles(TableEnemyCandidate, ControlHelper.GetDefaultColumnStyle());
				TableEnemyCandidate.ResumeLayout();
			}
		}



		protected override string GetPersistString()
		{
			return "Compass";
		}

		private void TableEnemyMember_CellPaint(object sender, TableLayoutCellPaintEventArgs e)
		{
			e.Graphics.DrawLine(UIColorScheme.Colors.SubBGPen, e.CellBounds.X, e.CellBounds.Bottom - 1, e.CellBounds.Right - 1, e.CellBounds.Bottom - 1);
		}

		private void TableEnemyCandidateMember_CellPaint(object sender, TableLayoutCellPaintEventArgs e)
		{

			if (_enemyFleetCandidate == null || _enemyFleetCandidateIndex + e.Column >= _enemyFleetCandidate.Count)
				return;


			e.Graphics.DrawLine(UIColorScheme.Colors.SubBGPen, e.CellBounds.Right - 1, e.CellBounds.Top, e.CellBounds.Right - 1, e.CellBounds.Bottom - 1);

			if (e.Row == 5 || e.Row == 7)
			{
				e.Graphics.DrawLine(UIColorScheme.Colors.SubBGPen, e.CellBounds.X, e.CellBounds.Bottom - 1, e.CellBounds.Right - 1, e.CellBounds.Bottom - 1);
			}
		}

	}

}

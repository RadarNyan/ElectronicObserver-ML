using Codeplex.Data;
using ElectronicObserver.Data;
using ElectronicObserver.Observer;
using ElectronicObserver.Resource;
using ElectronicObserver.Utility;
using ElectronicObserver.Utility.Data;
using ElectronicObserver.Utility.Mathematics;
using ElectronicObserver.Window.Control;
using ElectronicObserver.Window.Dialog;
using ElectronicObserver.Window.Support;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

	public partial class FormFleet : DockContent
	{

		private bool IsRemodeling = false;


		private class TableFleetControl : IDisposable
		{
			public Label Name;
			public FleetState State;
			public ImageLabel AirSuperiority;
			public ImageLabel SearchingAbility;
			public ImageLabel AntiAirPower;
			public ToolTip ToolTipInfo;

			private string UILanguage;

			public int BranchWeight { get; private set; } = 1;

			public TableFleetControl(FormFleet parent)
			{

				#region Initialize

				UILanguage = parent.UILanguage;

				Name = new Label
				{
					Text = "[" + parent.FleetID.ToString() + "]",
					Anchor = AnchorStyles.Left,
					ForeColor = parent.MainFontColor,
					UseMnemonic = false,
					Padding = new Padding(0, 1, 0, 1),
					Margin = new Padding(2, 0, 2, 0),
					AutoSize = true,
					//Name.Visible = false;
					Cursor = Cursors.Help
				};

				State = new FleetState
				{
					Anchor = AnchorStyles.Left,
					ForeColor = parent.MainFontColor,
					Padding = new Padding(),
					Margin = new Padding(),
					AutoSize = true
				};

				AirSuperiority = new ImageLabel
				{
					Anchor = AnchorStyles.Left,
					ForeColor = parent.MainFontColor,
					ImageList = ResourceManager.Instance.Equipments,
					ImageIndex = (int)ResourceManager.EquipmentContent.CarrierBasedFighter,
					Padding = new Padding(2, 2, 2, 2),
					Margin = new Padding(2, 0, 2, 0),
					AutoSize = true
				};

				SearchingAbility = new ImageLabel
				{
					Anchor = AnchorStyles.Left,
					ForeColor = parent.MainFontColor,
					ImageList = ResourceManager.Instance.Equipments,
					ImageIndex = (int)ResourceManager.EquipmentContent.CarrierBasedRecon,
					Padding = new Padding(2, 2, 2, 2),
					Margin = new Padding(2, 0, 2, 0),
					AutoSize = true
				};
				SearchingAbility.Click += (sender, e) => SearchingAbility_Click(sender, e, parent.FleetID);

				AntiAirPower = new ImageLabel
				{
					Anchor = AnchorStyles.Left,
					ForeColor = parent.MainFontColor,
					ImageList = ResourceManager.Instance.Equipments,
					ImageIndex = (int)ResourceManager.EquipmentContent.HighAngleGun,
					Padding = new Padding(2, 2, 2, 2),
					Margin = new Padding(2, 0, 2, 0),
					AutoSize = true
				};


				ConfigurationChanged(parent);

				ToolTipInfo = parent.ToolTipInfo;

				#endregion

			}

			public TableFleetControl(FormFleet parent, TableLayoutPanel table)
				: this(parent)
			{
				AddToTable(table);
			}

			public void AddToTable(TableLayoutPanel table)
			{

				table.SuspendLayout();
				table.Controls.Add(Name, 0, 0);
				table.Controls.Add(State, 1, 0);
				table.Controls.Add(AirSuperiority, 2, 0);
				table.Controls.Add(SearchingAbility, 3, 0);
				table.Controls.Add(AntiAirPower, 4, 0);
				table.ResumeLayout();

			}

			private void SearchingAbility_Click(object sender, EventArgs e, int fleetID)
			{
				switch (BranchWeight)
				{
					case 1:
						BranchWeight = 4;
						break;
					case 4:
						BranchWeight = 3;
						break;
					case 3:
						BranchWeight = 1;
						break;
				}
				Update(KCDatabase.Instance.Fleet[fleetID]);
			}

			public void Update(FleetData fleet)
			{

				KCDatabase db = KCDatabase.Instance;

				if (fleet == null) return;



				Name.Text = fleet.Name;
				{
					var members = fleet.MembersInstance.Where(s => s != null);

					int levelSum = members.Sum(s => s.Level);

					int fueltotal = members.Sum(s => Math.Max((int)Math.Floor(s.FuelMax * (s.IsMarried ? 0.85 : 1.00)), 1));
					int ammototal = members.Sum(s => Math.Max((int)Math.Floor(s.AmmoMax * (s.IsMarried ? 0.85 : 1.00)), 1));

					int fuelunit = members.Sum(s => Math.Max((int)Math.Floor(s.FuelMax * 0.2 * (s.IsMarried ? 0.85 : 1.00)), 1));
					int ammounit = members.Sum(s => Math.Max((int)Math.Floor(s.AmmoMax * 0.2 * (s.IsMarried ? 0.85 : 1.00)), 1));

					int speed = members.Select(s => s.Speed).DefaultIfEmpty(20).Min();

					string[] supportTypes;
					switch (UILanguage) {
						case "zh":
							supportTypes = new string[] {
								"不可支援",
								"航空支援",
								"支援射击",
								"支援长距离雷击"
							};
							break;
						case "en":
							supportTypes = new string[] {
								"No Support",
								"Aerial Support",
								"Support Shelling",
								"Long Range Torpedo Attack"
							};
							break;
						default:
							supportTypes = new string[] {
								"発動不能",
								"航空支援",
								"支援射撃",
								"支援長距離雷撃"
							};
							break;
					}

					double expeditionBonus = Calculator.GetExpeditionBonus(fleet);
					int tp = Calculator.GetTPDamage(fleet);

					// 各艦ごとの ドラム缶 or 大発系 を搭載している個数
					var transport = members.Select(s => s.AllSlotInstanceMaster.Count(eq => eq?.CategoryType == EquipmentTypes.TransportContainer));
					var landing = members.Select(s => s.AllSlotInstanceMaster.Count(eq => eq?.CategoryType == EquipmentTypes.LandingCraft || eq?.CategoryType == EquipmentTypes.SpecialAmphibiousTank));

					switch (UILanguage) {
						case "zh":
							ToolTipInfo.SetToolTip(Name,
								$"等级合计：{levelSum} / 平均：{(double)levelSum / Math.Max(fleet.Members.Count(id => id != -1), 1):0.00}\r\n" +
								$"{Constants.GetSpeed(speed)}舰队\r\n" +
								$"支援类型：{supportTypes[fleet.SupportType]}\r\n" +
								$"合计对空：{members.Sum(s => s.AATotal)} / 对潜：{members.Sum(s => s.ASWTotal)} / 索敌：{members.Sum(s => s.LOSTotal)}\r\n" +
								$"载有运输桶：{transport.Sum()}个（{transport.Count(i => i > 0)} 舰）\r\n" +
								$"载有大发动艇：{landing.Sum()}个（{landing.Count(i => i > 0)} 舰，收益 +{expeditionBonus:p1}）\r\n" +
								$"运输量(TP)：S {tp} / A {(int)(tp * 0.7)}\r\n" +
								$"总搭载：燃料 {fueltotal} / 弹药 {ammototal}\r\n" +
								$"单战消耗：燃料 {fuelunit} / 弹药 {ammounit}");
							break;
						case "en":
							ToolTipInfo.SetToolTip(Name,
								$"Lv Sum: {levelSum} / Avg: {(double)levelSum / Math.Max(fleet.Members.Count(id => id != -1), 1):0.00}\r\n" +
								$"{Constants.GetSpeed(speed)} Fleet\r\n" +
								$"Support Type: {supportTypes[fleet.SupportType]}\r\n" +
								$"TotalAA {members.Sum(s => s.AATotal)} / TotalASW {members.Sum(s => s.ASWTotal)} / TotalLOS {members.Sum(s => s.LOSTotal)}\r\n" +
								$"Drum Canisters Equipped: {transport.Sum()} (on {transport.Count(i => i > 0)} ships)\r\n" +
								$"Daihatsu Landing Crafts Equipped: {landing.Sum()} (on {landing.Count(i => i > 0)} ships, +{expeditionBonus:p1} bonus)\r\n" +
								$"Transport(TP): S {tp} / A {(int)(tp * 0.7)}\r\n" +
								$"Total on Board: Fuel {fueltotal} / Ammo {ammototal}\r\n" +
								$"Consumption per Battle: Fuel {fuelunit} / Ammo {ammounit}");
							break;
						default:
							ToolTipInfo.SetToolTip(Name,
								$"Lv合計: {levelSum} / 平均: {(double)levelSum / Math.Max(fleet.Members.Count(id => id != -1), 1):0.00}\r\n" +
								$"{Constants.GetSpeed(speed)}艦隊\r\n" +
								$"支援攻撃: {supportTypes[fleet.SupportType]}\r\n" +
								$"合計対空 {members.Sum(s => s.AATotal)} / 対潜 {members.Sum(s => s.ASWTotal)} / 索敵 {members.Sum(s => s.LOSTotal)}\r\n" +
								$"ドラム缶搭載: {transport.Sum()}個 ({transport.Count(i => i > 0)}艦)\r\n" +
								$"大発動艇搭載: {landing.Sum()}個 ({landing.Count(i => i > 0)}艦, +{expeditionBonus:p1})\r\n" +
								$"輸送量(TP): S {tp} / A {(int)(tp * 0.7)}\r\n" +
								$"総積載: 燃 {fueltotal} / 弾 {ammototal}\r\n" +
								$"(1戦当たり 燃 {fuelunit} / 弾 {ammounit})");
							break;
					}
				}


				State.UpdateFleetState(fleet, ToolTipInfo);


				//制空戦力計算	
				{
					int airSuperiority = fleet.GetAirSuperiority();
					bool includeLevel = Utility.Configuration.Config.FormFleet.AirSuperiorityMethod == 1;
					AirSuperiority.Text = fleet.GetAirSuperiorityString();
					switch (UILanguage) {
						case "zh":
							ToolTipInfo.SetToolTip(AirSuperiority,
								$"确保：{(int)(airSuperiority / 3.0)}\r\n" +
								$"优势：{(int)(airSuperiority / 1.5)}\r\n" +
								$"均衡：{Math.Max((int)(airSuperiority * 1.5 - 1), 0)}\r\n" +
								$"劣势：{Math.Max((int)(airSuperiority * 3.0 - 1), 0)}\r\n" +
								(includeLevel ?
								$"不计熟练度: {Calculator.GetAirSuperiorityIgnoreLevel(fleet)}" :
								$"计算熟练度: {Calculator.GetAirSuperiority(fleet)}"));
							break;
						case "en":
							ToolTipInfo.SetToolTip(AirSuperiority,
								$"Air Supremacy: {(int)(airSuperiority / 3.0)}\r\n" +
								$"Air Superiority: {(int)(airSuperiority / 1.5)}\r\n" +
								$"Air Parity: {Math.Max((int)(airSuperiority * 1.5 - 1), 0)}\r\n" +
								$"Air Denial: {Math.Max((int)(airSuperiority * 3.0 - 1), 0)}\r\n" +
								(includeLevel ?
								$"w/o Proficiency: {Calculator.GetAirSuperiorityIgnoreLevel(fleet)}" :
								$"w/ Proficiency: {Calculator.GetAirSuperiority(fleet)}"));
							break;
						default:
							ToolTipInfo.SetToolTip(AirSuperiority,
								$"確保: {(int)(airSuperiority / 3.0)}\r\n" +
								$"優勢: {(int)(airSuperiority / 1.5)}\r\n" +
								$"均衡: {Math.Max((int)(airSuperiority * 1.5 - 1), 0)}\r\n" +
								$"劣勢: {Math.Max((int)(airSuperiority * 3.0 - 1), 0)}\r\n" +
								(includeLevel ?
								$"熟練度なし: {Calculator.GetAirSuperiorityIgnoreLevel(fleet)}" :
								$"熟練度あり: {Calculator.GetAirSuperiority(fleet)}"));
							break;
					}
				}


				//索敵能力計算
				SearchingAbility.Text = fleet.GetSearchingAbilityString(BranchWeight);
				{
					StringBuilder sb = new StringBuilder();
					double probStart = fleet.GetContactProbability();
					var probSelect = fleet.GetContactSelectionProbability();

					switch (UILanguage) {
						case "zh":
							sb.Append(
								$"33式分歧点系数：{BranchWeight}\r\n" +
								$"　（点击切换）\r\n\r\n" +
								$"触接开始率：\r\n" +
								$"　确保 {probStart:p1} / 优势 {probStart * 0.6:p1}\r\n");
							break;
						case "en":
							sb.Append(
								$"Formula 33 Node Factor: {BranchWeight}\r\n" +
								$"  (Click to switch Node Factor) \r\n\r\n" +
								$"Contact Trigger Rate: \r\n" +
								$"  Air Supremacy {probStart:p1} / Air Superiority {probStart * 0.6:p1}\r\n");
							break;
						default:
							sb.Append(
								$"新判定式(33) 分岐点係数: {BranchWeight}\r\n" +
								$"　(クリックで切り替え)\r\n\r\n" +
								$"触接開始率: \r\n　" +
								$"確保 {probStart:p1} / 優勢 {probStart * 0.6:p1}\r\n");
							break;
					}

					if (probSelect.Count > 0)
					{
						switch (UILanguage) {
							case "zh":
								sb.AppendLine("触接选择率：");
								break;
							case "en":
								sb.AppendLine("Contact Selection Rate:");
								break;
							default:
								sb.AppendLine("触接選択率:");
								break;
						}
						foreach (var p in probSelect.OrderBy(p => p.Key))
						{
							switch (UILanguage) {
								case "zh":
									sb.Append(
										$"　命中 {p.Key} : {p.Value:p1}\r\n");
									break;
								case "en":
									sb.Append(
										$"  Accuracy {p.Key} : {p.Value:p1}\r\n");
									break;
								default:
									sb.Append(
										$"　命中{p.Key} : {p.Value:p1}\r\n");
									break;
							}
						}
					}

					ToolTipInfo.SetToolTip(SearchingAbility, sb.ToString());
				}

				// 対空能力計算
				{
					var sb = new StringBuilder();
					double lineahead = Calculator.GetAdjustedFleetAAValue(fleet, 1);

					AntiAirPower.Text = lineahead.ToString("0.0");

					switch (UILanguage) {
						case "zh":
							sb.Append(
								$"舰队防空\r\n" +
								$"单纵阵：{lineahead:0.0} / " +
								$"复纵阵：{Calculator.GetAdjustedFleetAAValue(fleet, 2):0.0} / " +
								$"轮形阵：{Calculator.GetAdjustedFleetAAValue(fleet, 3):0.0}\r\n");
							break;
						case "en":
							sb.Append(
								$"Fleet Anti-air Defense\r\n" +
								$"Line Ahead: {lineahead:0.0} / " +
								$"Double Line: {Calculator.GetAdjustedFleetAAValue(fleet, 2):0.0} / " +
								$"Diamond: {Calculator.GetAdjustedFleetAAValue(fleet, 3):0.0}\r\n");
							break;
						default:
							sb.Append(
								$"艦隊防空\r\n" +
								$"単縦陣: {lineahead:0.0} / " +
								$"複縦陣: {Calculator.GetAdjustedFleetAAValue(fleet, 2):0.0} / " +
								$"輪形陣: {Calculator.GetAdjustedFleetAAValue(fleet, 3):0.0}\r\n");
							break;
					}

					ToolTipInfo.SetToolTip(AntiAirPower, sb.ToString());
				}
			}


			public void Refresh()
			{

				State.RefreshFleetState();

			}

			public void ConfigurationChanged(FormFleet parent)
			{
				Name.Font = parent.MainFont;
				State.Font = parent.MainFont;
				State.RefreshFleetState();
				AirSuperiority.Font = parent.MainFont;
				SearchingAbility.Font = parent.MainFont;
				AntiAirPower.Font = parent.MainFont;

				ControlHelper.SetTableRowStyles(parent.TableFleet, ControlHelper.GetDefaultRowStyle());
			}

			public void Dispose()
			{
				Name.Dispose();
				State.Dispose();
				AirSuperiority.Dispose();
				SearchingAbility.Dispose();
				AntiAirPower.Dispose();
			}
		}


		private class TableMemberControl : IDisposable
		{
			public ImageLabel Name;
			public ShipStatusLevel Level;
			public ShipStatusHP HP;
			public ImageLabel Condition;
			public ShipStatusResource ShipResource;
			public ShipStatusEquipment Equipments;

			private ToolTip ToolTipInfo;
			private FormFleet Parent;

			private string UILanguage;

			public TableMemberControl(FormFleet parent)
			{

				#region Initialize

				UILanguage = parent.UILanguage;

				Name = new ImageLabel();
				Name.SuspendLayout();
				Name.Text = "*nothing*";
				Name.Anchor = AnchorStyles.Left;
				Name.TextAlign = ContentAlignment.MiddleLeft;
				Name.ImageAlign = ContentAlignment.MiddleCenter;
				Name.ForeColor = parent.MainFontColor;
				Name.Padding = new Padding(2, 1, 2, 1);
				Name.Margin = new Padding(2, 1, 2, 1);
				Name.AutoSize = true;
				//Name.AutoEllipsis = true;
				Name.Visible = false;
				Name.Cursor = Cursors.Help;
				Name.MouseDown += Name_MouseDown;
				Name.ResumeLayout();

				Level = new ShipStatusLevel();
				Level.SuspendLayout();
				Level.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
				Level.Value = 0;
				Level.MaximumValue = ExpTable.ShipMaximumLevel;
				Level.ValueNext = 0;
				Level.MainFontColor = parent.MainFontColor;
				Level.SubFontColor = parent.SubFontColor;
				//Level.TextNext = "n.";
				Level.Padding = new Padding(0, 0, 0, 0);
				Level.Margin = new Padding(2, 0, 2, 1);
				Level.AutoSize = true;
				Level.Visible = false;
				Level.Cursor = Cursors.Help;
				Level.MouseDown += Level_MouseDown;
				Level.ResumeLayout();

				HP = new ShipStatusHP();
				HP.SuspendUpdate();
				HP.Anchor = AnchorStyles.Left;
				HP.Value = 0;
				HP.MaximumValue = 0;
				HP.MaximumDigit = 999;
				HP.UsePrevValue = false;
				HP.MainFontColor = parent.MainFontColor;
				HP.SubFontColor = parent.SubFontColor;
				HP.Padding = new Padding(0, 0, 0, 0);
				HP.Margin = new Padding(2, 1, 2, 2);
				HP.AutoSize = true;
				HP.AutoSizeMode = AutoSizeMode.GrowAndShrink;
				HP.Visible = false;
				HP.ResumeUpdate();

				Condition = new ImageLabel();
				Condition.SuspendLayout();
				Condition.Text = "*";
				Condition.Anchor = AnchorStyles.Left | AnchorStyles.Right;
				Condition.ForeColor = parent.MainFontColor;
				Condition.TextAlign = ContentAlignment.BottomRight;
				Condition.ImageAlign = ContentAlignment.MiddleLeft;
				Condition.ImageList = ResourceManager.Instance.Icons;
				Condition.Padding = new Padding(2, 1, 2, 1);
				Condition.Margin = new Padding(2, 1, 2, 1);
				Condition.Size = new Size(40, 20);
				Condition.AutoSize = true;
				Condition.Visible = false;
				Condition.ResumeLayout();

				ShipResource = new ShipStatusResource(parent.ToolTipInfo);
				ShipResource.SuspendLayout();
				ShipResource.FuelCurrent = 0;
				ShipResource.FuelMax = 0;
				ShipResource.AmmoCurrent = 0;
				ShipResource.AmmoMax = 0;
				ShipResource.Anchor = AnchorStyles.Left;
				ShipResource.Padding = new Padding(0, 2, 0, 0);
				ShipResource.Margin = new Padding(2, 0, 2, 1);
				ShipResource.Size = new Size(30, 20);
				ShipResource.AutoSize = false;
				ShipResource.Visible = false;
				ShipResource.ResumeLayout();

				Equipments = new ShipStatusEquipment();
				Equipments.SuspendUpdate();
				Equipments.Anchor = AnchorStyles.Left;
				Equipments.Padding = new Padding(0, 1, 0, 1);
				Equipments.Margin = new Padding(2, 0, 2, 1);
				Equipments.Size = new Size(40, 20);
				Equipments.AutoSize = true;
				Equipments.AutoSizeMode = AutoSizeMode.GrowAndShrink;
				Equipments.Visible = false;
				Equipments.ResumeUpdate();

				ConfigurationChanged(parent);

				ToolTipInfo = parent.ToolTipInfo;
				Parent = parent;
				#endregion

			}

			public TableMemberControl(FormFleet parent, TableLayoutPanel table, int row)
				: this(parent)
			{
				AddToTable(table, row);

				Equipments.Name = string.Format("{0}_{1}", parent.FleetID, row + 1);
			}


			public void AddToTable(TableLayoutPanel table, int row)
			{

				table.SuspendLayout();

				table.Controls.Add(Name, 0, row);
				table.Controls.Add(Level, 1, row);
				table.Controls.Add(HP, 2, row);
				table.Controls.Add(Condition, 3, row);
				table.Controls.Add(ShipResource, 4, row);
				table.Controls.Add(Equipments, 5, row);

				table.ResumeLayout();

			}

			public void Update(int shipMasterID)
			{

				KCDatabase db = KCDatabase.Instance;
				ShipData ship = db.Ships[shipMasterID];

				if (ship != null)
				{

					bool isEscaped = KCDatabase.Instance.Fleet[Parent.FleetID].EscapedShipList.Contains(shipMasterID);


					Name.Text = ship.MasterShip.NameWithClass;
					Name.Tag = ship.ShipID;

					switch (UILanguage) {
						case "zh":
							ToolTipInfo.SetToolTip(Name,
								$"{ship.MasterShip.ShipTypeName} {ship.NameWithLevel}\r\n" +
								$"火力：{ship.FirepowerBase}/{ship.FirepowerTotal}\r\n" +
								$"雷装：{ship.TorpedoBase}/{ship.TorpedoTotal}\r\n" +
								$"对空：{ship.AABase}/{ship.AATotal}\r\n" +
								$"装甲：{ship.ArmorBase}/{ship.ArmorTotal}\r\n" +
								$"对潜：{ship.ASWBase}/{ship.ASWTotal}\r\n" +
								$"回避：{ship.EvasionBase}/{ship.EvasionTotal}\r\n" +
								$"索敌：{ship.LOSBase}/{ship.LOSTotal}\r\n" +
								$"运：{ship.LuckTotal}\r\n" +
								$"射程：{Constants.GetRange(ship.Range)} / 速度：{Constants.GetSpeed(ship.Speed)}\r\n" +
								$"（点击右键转到图鉴）\r\n");
							break;
						case "en":
							ToolTipInfo.SetToolTip(Name,
								$"{ship.MasterShip.ShipTypeName} {ship.NameWithLevel}\r\n" +
								$"Firepower: {ship.FirepowerBase}/{ship.FirepowerTotal}\r\n" +
								$"Torpedo: {ship.TorpedoBase}/{ship.TorpedoTotal}\r\n" +
								$"AA: {ship.AABase}/{ship.AATotal}\r\n" +
								$"Armor: {ship.ArmorBase}/{ship.ArmorTotal}\r\n" +
								$"ASW: {ship.ASWBase}/{ship.ASWTotal}\r\n" +
								$"Evasion: {ship.EvasionBase}/{ship.EvasionTotal}\r\n" +
								$"LOS: {ship.LOSBase}/{ship.LOSTotal}\r\n" +
								$"Luck: {ship.LuckTotal}\r\n" +
								$"Range: {Constants.GetRange(ship.Range)} / Speed: {Constants.GetSpeed(ship.Speed)}\r\n" +
								$"(Right-click for Album)\n");
							break;
						default:
							ToolTipInfo.SetToolTip(Name,
								$"{ship.MasterShip.ShipTypeName} {ship.NameWithLevel}\r\n" +
								$"火力: {ship.FirepowerBase}/{ship.FirepowerTotal}\r\n" +
								$"雷装: {ship.TorpedoBase}/{ship.TorpedoTotal}\r\n" +
								$"対空: {ship.AABase}/{ship.AATotal}\r\n" +
								$"装甲: {ship.ArmorBase}/{ship.ArmorTotal}\r\n" +
								$"対潜: {ship.ASWBase}/{ship.ASWTotal}\r\n" +
								$"回避: {ship.EvasionBase}/{ship.EvasionTotal}\r\n" +
								$"索敵: {ship.LOSBase}/{ship.LOSTotal}\r\n" +
								$"運: {ship.LuckTotal}\r\n" +
								$"射程: {Constants.GetRange(ship.Range)} / 速力: {Constants.GetSpeed(ship.Speed)}\r\n" +
								$"(右クリックで図鑑)\r\n");
							break;
					}

					Level.Value = ship.Level;
					Level.ValueNext = ship.ExpNext;
					Level.Tag = ship.MasterID;

					{
						StringBuilder tip = new StringBuilder();
						switch (UILanguage) {
							case "zh":
								tip.AppendLine($"总经验：{ship.ExpTotal} exp.");
								if (!Utility.Configuration.Config.FormFleet.ShowNextExp)
									tip.AppendLine($"距离升级：{ship.ExpNext} exp");
								if (ship.MasterShip.RemodelAfterShipID != 0 && ship.Level < ship.MasterShip.RemodelAfterLevel) {
									tip.AppendLine($"距离改装：Lv.{ship.MasterShip.RemodelAfterLevel - ship.Level} / {ship.ExpNextRemodel} exp.");
								} else if (ship.Level <= 99) {
									tip.AppendLine($"距离 Lv.99：{Math.Max(ExpTable.GetExpToLevelShip(ship.ExpTotal, 99), 0)} exp.");
								} else {
									tip.AppendLine($"距离 Lv.{ExpTable.ShipMaximumLevel}：{Math.Max(ExpTable.GetExpToLevelShip(ship.ExpTotal, ExpTable.ShipMaximumLevel), 0)} exp.");
								}
								tip.AppendLine("（点击右键计算所需经验值）");
								break;
							case "en":
								tip.AppendLine($"Total: {ship.ExpTotal} exp.");
								if (!Utility.Configuration.Config.FormFleet.ShowNextExp)
									tip.AppendLine($"To next level: {ship.ExpNext} exp");
								if (ship.MasterShip.RemodelAfterShipID != 0 && ship.Level < ship.MasterShip.RemodelAfterLevel) {
									tip.AppendLine($"To remodel: Lv. {ship.MasterShip.RemodelAfterLevel - ship.Level} / {ship.ExpNextRemodel} exp.");
								} else if (ship.Level <= 99) {
									tip.AppendLine($"To Lv.99: {Math.Max(ExpTable.GetExpToLevelShip(ship.ExpTotal, 99), 0)} exp.");
								} else {
									tip.AppendLine($"To Lv.{ExpTable.ShipMaximumLevel}: {Math.Max(ExpTable.GetExpToLevelShip(ship.ExpTotal, ExpTable.ShipMaximumLevel), 0)} exp.");
								}
								tip.AppendLine("(Right-click to calculate exp needed)");
								break;
							default:
								tip.AppendLine($"Total: {ship.ExpTotal} exp.");
								if (!Utility.Configuration.Config.FormFleet.ShowNextExp)
									tip.AppendLine($"次のレベルまで: {ship.ExpNext} exp");
								if (ship.MasterShip.RemodelAfterShipID != 0 && ship.Level < ship.MasterShip.RemodelAfterLevel) {
									tip.AppendLine($"改装まで: Lv. {ship.MasterShip.RemodelAfterLevel - ship.Level} / {ship.ExpNextRemodel} exp.");
								} else if (ship.Level <= 99) {
									tip.AppendLine($"Lv99まで: {Math.Max(ExpTable.GetExpToLevelShip(ship.ExpTotal, 99), 0)} exp.");
								} else {
									tip.AppendLine($"Lv{ExpTable.ShipMaximumLevel}まで: {Math.Max(ExpTable.GetExpToLevelShip(ship.ExpTotal, ExpTable.ShipMaximumLevel), 0)} exp.");
								}
								tip.AppendLine("(右クリックで必要Exp計算)");
								break;
						}
						ToolTipInfo.SetToolTip(Level, tip.ToString());
					}


					HP.SuspendUpdate();
					HP.Value = HP.PrevValue = ship.HPCurrent;
					HP.MaximumValue = ship.HPMax;
					HP.UsePrevValue = false;
					HP.ShowDifference = false;
					{
						int dockID = ship.RepairingDockID;

						if (dockID != -1)
						{
							HP.RepairTime = db.Docks[dockID].CompletionTime;
							HP.RepairTimeShowMode = ShipStatusHPRepairTimeShowMode.Visible;
						}
						else
						{
							HP.RepairTimeShowMode = ShipStatusHPRepairTimeShowMode.Invisible;
						}
					}
					HP.Tag = (ship.RepairingDockID == -1 && 0.5 < ship.HPRate && ship.HPRate < 1.0) ? DateTimeHelper.FromAPITimeSpan(ship.RepairTime).TotalSeconds : 0.0;
					if (isEscaped)
					{
						HP.BackColor = UIColorScheme.Colors.SubBG;
					}
					else
					{
						HP.BackColor = UIColorScheme.Colors.MainBG;
					}
					{
						StringBuilder sb = new StringBuilder();
						double hprate = (double)ship.HPCurrent / ship.HPMax;

						sb.AppendFormat("HP: {0:0.0}% [{1}]\n", hprate * 100, Constants.GetDamageState(hprate));
						switch (UILanguage) {
							case "zh":
								if (isEscaped) {
									sb.AppendLine("退避中");
								} else if (hprate > 0.50) {
									sb.AppendLine($"距离中破：{ship.HPCurrent - ship.HPMax / 2} / 距离大破：{ship.HPCurrent - ship.HPMax / 4}");
								} else if (hprate > 0.25) {
									sb.AppendLine($"距离大破：{ship.HPCurrent - ship.HPMax / 4}");
								} else {
									sb.AppendLine("已经大破！");
								}
								break;
							case "en":
								if (isEscaped) {
									sb.AppendLine("Retreated");
								} else if (hprate > 0.50) {
									sb.AppendLine($"To moderately damage: {ship.HPCurrent - ship.HPMax / 2} / To heavily damage: {ship.HPCurrent - ship.HPMax / 4}");
								} else if (hprate > 0.25) {
									sb.AppendLine($"To heavily damage: {ship.HPCurrent - ship.HPMax / 4}");
								} else {
									sb.AppendLine("HEAVILY DAMAGED!");
								}
								break;
							default:
								if (isEscaped) {
									sb.AppendLine("退避中");
								} else if (hprate > 0.50) {
									sb.AppendLine($"中破まで: {ship.HPCurrent - ship.HPMax / 2} / 大破まで: {ship.HPCurrent - ship.HPMax / 4}");
								} else if (hprate > 0.25) {
									sb.AppendLine($"大破まで: {ship.HPCurrent - ship.HPMax / 4}");
								} else {
									sb.AppendLine("大破しています！");
								}
								break;
						}

						if (ship.RepairTime > 0)
						{
							var span = DateTimeHelper.FromAPITimeSpan(ship.RepairTime);
							switch (UILanguage) {
								case "zh":
									sb.Append($"入渠时间：{DateTimeHelper.ToTimeRemainString(span)} @ {DateTimeHelper.ToTimeRemainString(Calculator.CalculateDockingUnitTime(ship))}");
									break;
								case "en":
									sb.Append($"Repair time: {DateTimeHelper.ToTimeRemainString(span)} @ {DateTimeHelper.ToTimeRemainString(Calculator.CalculateDockingUnitTime(ship))}");
									break;
								default:
									sb.Append($"入渠時間: {DateTimeHelper.ToTimeRemainString(span)} @ {DateTimeHelper.ToTimeRemainString(Calculator.CalculateDockingUnitTime(ship))}");
									break;
							}
						}

						ToolTipInfo.SetToolTip(HP, sb.ToString());
					}
					HP.ResumeUpdate();


					Condition.Text = ship.Condition.ToString();
					Condition.Tag = ship.Condition;
					SetConditionDesign(ship.Condition);

					switch (UILanguage) {
						case "zh":
							if (ship.Condition < 49) {
								TimeSpan ts = new TimeSpan(0, (int)Math.Ceiling((49 - ship.Condition) / 3.0) * 3, 0);
								ToolTipInfo.SetToolTip(Condition, $"距离完全恢复约 {(int)ts.TotalMinutes:D2}:{(int)ts.Seconds:D2}");
							} else {
								ToolTipInfo.SetToolTip(Condition, $"还可以远征 {(int)Math.Ceiling((ship.Condition - 49) / 3.0)} 次");
							}
							break;
						case "en":
							if (ship.Condition < 49) {
								TimeSpan ts = new TimeSpan(0, (int)Math.Ceiling((49 - ship.Condition) / 3.0) * 3, 0);
								ToolTipInfo.SetToolTip(Condition, $"About {(int)ts.TotalMinutes:D2}:{(int)ts.Seconds:D2} to fully recover");
							} else {
								ToolTipInfo.SetToolTip(Condition, $"Can go expedition {(int)Math.Ceiling((ship.Condition - 49) / 3.0)} times");
							}
							break;
						default:
							if (ship.Condition < 49) {
								TimeSpan ts = new TimeSpan(0, (int)Math.Ceiling((49 - ship.Condition) / 3.0) * 3, 0);
								ToolTipInfo.SetToolTip(Condition, $"完全回復まで 約 {(int)ts.TotalMinutes:D2}:{(int)ts.Seconds:D2}");
							} else {
								ToolTipInfo.SetToolTip(Condition, $"あと {(int)Math.Ceiling((ship.Condition - 49) / 3.0)} 回遠征可能");
							}
							break;
					}

					ShipResource.SetResources(ship.Fuel, ship.FuelMax, ship.Ammo, ship.AmmoMax);


					Equipments.SetSlotList(ship);
					ToolTipInfo.SetToolTip(Equipments, GetEquipmentString(ship));

				}
				else
				{
					Name.Tag = -1;
				}


				Name.Visible =
				Level.Visible =
				HP.Visible =
				Condition.Visible =
				ShipResource.Visible =
				Equipments.Visible = shipMasterID != -1;

			}

			void Name_MouseDown(object sender, MouseEventArgs e)
			{
				if (Name.Tag is int id && id != -1)
				{
					if ((e.Button & MouseButtons.Right) != 0)
					{
						new DialogAlbumMasterShip(id).Show(Parent);
					}
				}
			}

			private void Level_MouseDown(object sender, MouseEventArgs e)
			{
				if (Level.Tag is int id && id != -1)
				{
					if ((e.Button & MouseButtons.Right) != 0)
					{
						new DialogExpChecker(id).Show(Parent);
					}
				}
			}


			private string GetEquipmentString(ShipData ship)
			{
				StringBuilder sb = new StringBuilder();

				for (int i = 0; i < ship.Slot.Count; i++)
				{
					var eq = ship.SlotInstance[i];
					if (eq != null)
						sb.AppendFormat("[{0}/{1}] {2}\r\n", ship.Aircraft[i], ship.MasterShip.Aircraft[i], eq.NameWithLevel);
				}

				{
					var exslot = ship.ExpansionSlotInstance;
					if (exslot != null) {
						switch (UILanguage) {
							case "zh":
								sb.AppendLine($"补强：{exslot.NameWithLevel}");
								break;
							case "en":
								sb.AppendLine($"Reinforcement Expansion: {exslot.NameWithLevel}");
								break;
							default:
								sb.AppendLine($"補強: {exslot.NameWithLevel}");
								break;
						}
					}
				}

				int[] slotmaster = ship.AllSlotMaster.ToArray();

				switch (UILanguage) {
					case "zh":
						sb.Append($"\r\n昼战：{Constants.GetDayAttackKind(Calculator.GetDayAttackKind(slotmaster, ship.ShipID, -1))}");
						break;
					case "en":
						sb.Append($"\r\nDay Battle: {Constants.GetDayAttackKind(Calculator.GetDayAttackKind(slotmaster, ship.ShipID, -1))}");
						break;
					default:
						sb.Append($"\r\n昼戦: {Constants.GetDayAttackKind(Calculator.GetDayAttackKind(slotmaster, ship.ShipID, -1))}");
						break;
				}
				{
					int shelling = ship.ShellingPower;
					int aircraft = ship.AircraftPower;
					switch (UILanguage) {
						case "zh":
							if (shelling > 0) {
								if (aircraft > 0)
									sb.Append($" - 炮击：{shelling} / 空袭：{aircraft}");
								else
									sb.Append($" - 威力：{shelling}");
							} else if (aircraft > 0)
								sb.Append($" - 威力：{aircraft}");
							break;
						case "en":
							if (shelling > 0) {
								if (aircraft > 0)
									sb.Append($" - Shelling: {shelling} / Aerial Attack: {aircraft}");
								else
									sb.Append($" - Power: {shelling}");
							} else if (aircraft > 0)
								sb.Append($" - Power: {aircraft}");
							break;
						default:
							if (shelling > 0) {
								if (aircraft > 0)
									sb.Append($" - 砲撃: {shelling} / 空撃: {aircraft}");
								else
									sb.Append($" - 威力: {shelling}");
							} else if (aircraft > 0)
								sb.Append($" - 威力: {aircraft}");
							break;
					}
				}
				sb.AppendLine();

				if (ship.CanAttackAtNight)
				{
					switch (UILanguage) {
						case "zh":
							sb.Append($"夜战：{Constants.GetNightAttackKind(Calculator.GetNightAttackKind(slotmaster, ship.ShipID, -1))}");
							break;
						case "en":
							sb.Append($"Night Battle: {Constants.GetNightAttackKind(Calculator.GetNightAttackKind(slotmaster, ship.ShipID, -1))}");
							break;
						default:
							sb.Append($"夜戦: {Constants.GetNightAttackKind(Calculator.GetNightAttackKind(slotmaster, ship.ShipID, -1))}");
							break;
					}
					{
						int night = ship.NightBattlePower;
						if (night > 0)
						{
							switch (UILanguage) {
								case "zh":
									sb.Append($" - 威力：{night}");
									break;
								case "en":
									sb.Append($" - Power: {night}");
									break;
								default:
									sb.Append($" - 威力: {night}");
									break;
							}
						}
					}
					sb.AppendLine();
				}

				{
					int torpedo = ship.TorpedoPower;
					int asw = ship.AntiSubmarinePower;
					switch (UILanguage) {
						case "zh":
							if (torpedo > 0)
								sb.Append($"雷击：{torpedo}");
							if (asw > 0) {
								if (torpedo > 0)
									sb.Append(" / ");
								sb.Append($"对潜：{asw}");
								if (ship.CanOpeningASW)
									sb.Append("（可以先制）");
							}
							break;
						case "en":
							if (torpedo > 0)
								sb.Append($"Torpedo: {torpedo}");
							if (asw > 0) {
								if (torpedo > 0)
									sb.Append(" / ");
								sb.Append($"ASW: {asw}");
								if (ship.CanOpeningASW)
									sb.Append(" (OASW possible)");
							}
							break;
						default:
							if (torpedo > 0)
								sb.Append($"雷撃: {torpedo}");
							if (asw > 0) {
								if (torpedo > 0)
									sb.Append(" / ");
								sb.Append($"対潜: {asw}");
								if (ship.CanOpeningASW)
									sb.Append(" (先制可能)");
							}
							break;
					}
					if (torpedo > 0 || asw > 0)
						sb.AppendLine();
				}

				{
					int aacutin = Calculator.GetAACutinKind(ship.ShipID, slotmaster);
					if (aacutin != 0)
					{
						switch (UILanguage) {
							case "zh":
								sb.AppendLine($"对空：{Constants.GetAACutinKind(aacutin)}");
								break;
							case "en":
								sb.AppendLine($"Anti-Aircraft: {Constants.GetAACutinKind(aacutin)}");
								break;
							default:
								sb.AppendLine($"対空: {Constants.GetAACutinKind(aacutin)}");
								break;
						}
					}
					double adjustedaa = Calculator.GetAdjustedAAValue(ship);
					switch (UILanguage) {
						case "zh":
							sb.AppendLine($"加权对空：{adjustedaa} (比例击坠：{Calculator.GetProportionalAirDefense(adjustedaa):p2})");
							break;
						case "en":
							sb.AppendLine($"Adjusted AA: {adjustedaa} (Percentage shot down: {Calculator.GetProportionalAirDefense(adjustedaa):p2})");
							break;
						default:
							sb.AppendLine($"加重対空: {adjustedaa} (割合撃墜: {Calculator.GetProportionalAirDefense(adjustedaa):p2})");
							break;
					}
				}

				{
					int airsup_min;
					int airsup_max;
					if (Utility.Configuration.Config.FormFleet.AirSuperiorityMethod == 1)
					{
						airsup_min = Calculator.GetAirSuperiority(ship, false);
						airsup_max = Calculator.GetAirSuperiority(ship, true);
					}
					else
					{
						airsup_min = airsup_max = Calculator.GetAirSuperiorityIgnoreLevel(ship);
					}

					int airbattle = ship.AirBattlePower;
					if (airsup_min > 0)
					{

						string airsup_str;
						if (Utility.Configuration.Config.FormFleet.ShowAirSuperiorityRange && airsup_min < airsup_max)
						{
							airsup_str = string.Format("{0} ～ {1}", airsup_min, airsup_max);
						}
						else
						{
							airsup_str = airsup_min.ToString();
						}

						switch (UILanguage) {
							case "zh":
								if (airbattle > 0)
									sb.AppendLine($"制空战力：{airsup_str} / 航空战威力：{airbattle}");
								else
									sb.AppendLine($"制空战力：{airsup_str}");
								break;
							case "en":
								if (airbattle > 0)
									sb.AppendLine($"Fighter Power: {airsup_str} / Aerial Power: {airbattle}");
								else
									sb.AppendLine($"Fighter Power: {airsup_str}");
								break;
							default:
								if (airbattle > 0)
									sb.AppendLine($"制空戦力: {airsup_str} / 航空威力: {airbattle}");
								else
									sb.AppendLine($"制空戦力: {airsup_str}");
								break;
						}
					}
					else if (airbattle > 0) {
						switch (UILanguage) {
							case "zh":
								sb.AppendLine("航空战威力：{airbattle}");
								break;
							case "en":
								sb.AppendLine("Aerial Power: {airbattle}");
								break;
							default:
								sb.AppendLine("航空威力: {airbattle}");
								break;
						}
					}
				}

				return sb.ToString();
			}

			private void SetConditionDesign(int cond)
			{

				if (Condition.ImageAlign == ContentAlignment.MiddleCenter)
				{
					// icon invisible
					Condition.ImageIndex = -1;

					Condition.ForeColor = UIColorScheme.Colors.Fleet_ConditionFG;
					if (cond < 20)
						Condition.BackColor = UIColorScheme.Colors.Fleet_ConditionBGVeryTired;
					else if (cond < 30)
						Condition.BackColor = UIColorScheme.Colors.Fleet_ConditionBGTired;
					else if (cond < 40)
						Condition.BackColor = UIColorScheme.Colors.Fleet_ConditionBGLittleTired;
					else if (cond < 50)
					{
						Condition.ForeColor = UIColorScheme.Colors.MainFG;
						Condition.BackColor = Color.Transparent;
					}
					else
						Condition.BackColor = UIColorScheme.Colors.Fleet_ConditionBGSparkle;

				}
				else
				{
					Condition.BackColor = Color.Transparent;

					if (cond < 20)
						Condition.ImageIndex = (int)ResourceManager.IconContent.ConditionVeryTired;
					else if (cond < 30)
						Condition.ImageIndex = (int)ResourceManager.IconContent.ConditionTired;
					else if (cond < 40)
						Condition.ImageIndex = (int)ResourceManager.IconContent.ConditionLittleTired;
					else if (cond < 50)
						Condition.ImageIndex = (int)ResourceManager.IconContent.ConditionNormal;
					else
						Condition.ImageIndex = (int)ResourceManager.IconContent.ConditionSparkle;

				}
			}

			public void ConfigurationChanged(FormFleet parent)
			{
				Name.Font = parent.MainFont;
				Level.MainFont = parent.MainFont;
				Level.SubFont = parent.SubFont;
				HP.MainFont = parent.MainFont;
				HP.SubFont = parent.SubFont;
				Condition.Font = parent.MainFont;
				SetConditionDesign((Condition.Tag as int?) ?? 49);
				Equipments.Font = parent.SubFont;
			}

			public void Dispose()
			{
				Name.Dispose();
				Level.Dispose();
				HP.Dispose();
				Condition.Dispose();
				ShipResource.Dispose();
				Equipments.Dispose();

			}
		}




		public int FleetID { get; private set; }


		public Font MainFont { get; set; }
		public Font SubFont { get; set; }
		public Color MainFontColor { get; set; }
		public Color SubFontColor { get; set; }


		private TableFleetControl ControlFleet;
		private TableMemberControl[] ControlMember;

		private int AnchorageRepairBound;

		private string UILanguage;

		public FormFleet(FormMain parent, int fleetID)
		{
			InitializeComponent();
			SizeChanged += FormFleet_SizeChanged;

			UILanguage = parent.UILanguage;
			ForeColor = parent.ForeColor;
			BackColor = parent.BackColor;

			FleetID = fleetID;
			Utility.SystemEvents.UpdateTimerTick += UpdateTimerTick;

			ConfigurationChanged();

			MainFontColor = UIColorScheme.Colors.MainFG;
			SubFontColor = UIColorScheme.Colors.SubFG;

			AnchorageRepairBound = 0;

			//ui init

			ControlHelper.SetDoubleBuffered(TableFleet);
			ControlHelper.SetDoubleBuffered(TableMember);


			TableFleet.Visible = false;
			TableFleet.SuspendLayout();
			TableFleet.BackColor = UIColorScheme.Colors.SubBG;
			ControlFleet = new TableFleetControl(this, TableFleet);
			TableFleet.ResumeLayout();


			TableMember.SuspendLayout();
			ControlMember = new TableMemberControl[7];
			for (int i = 0; i < ControlMember.Length; i++)
			{
				ControlMember[i] = new TableMemberControl(this, TableMember, i);
			}
			TableMember.ResumeLayout();


			ConfigurationChanged();     //fixme: 苦渋の決断

			Icon = ResourceManager.ImageToIcon(ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormFleet]);

		}

		private void FormFleet_SizeChanged(object sender, EventArgs e)
		{
			TableFleet.MinimumSize = new Size(Math.Max(Width, TableMember.Width), 0);
			TableMember.MinimumSize = new Size(Width, 0);
		}

		private void FormFleet_Load(object sender, EventArgs e)
		{

			Text = string.Format("#{0}", FleetID);

			APIObserver o = APIObserver.Instance;

			o["api_req_nyukyo/start"].RequestReceived += Updated;
			o["api_req_nyukyo/speedchange"].RequestReceived += Updated;
			o["api_req_hensei/change"].RequestReceived += Updated;
			o["api_req_kousyou/destroyship"].RequestReceived += Updated;
			o["api_req_member/updatedeckname"].RequestReceived += Updated;
			o["api_req_kaisou/remodeling"].RequestReceived += Updated;
			o["api_req_map/start"].RequestReceived += Updated;
			o["api_req_hensei/combined"].RequestReceived += Updated;

			o["api_port/port"].ResponseReceived += Updated;
			o["api_get_member/ship2"].ResponseReceived += Updated;
			o["api_get_member/ndock"].ResponseReceived += Updated;
			o["api_req_kousyou/getship"].ResponseReceived += Updated;
			o["api_req_hokyu/charge"].ResponseReceived += Updated;
			o["api_req_kousyou/destroyship"].ResponseReceived += Updated;
			o["api_get_member/ship3"].ResponseReceived += Updated;
			o["api_req_kaisou/powerup"].ResponseReceived += Updated;        //requestのほうは面倒なのでこちらでまとめてやる
			o["api_get_member/deck"].ResponseReceived += Updated;
			o["api_get_member/slot_item"].ResponseReceived += Updated;
			o["api_req_map/start"].ResponseReceived += Updated;
			o["api_req_map/next"].ResponseReceived += Updated;
			o["api_get_member/ship_deck"].ResponseReceived += Updated;
			o["api_req_hensei/preset_select"].ResponseReceived += Updated;
			o["api_req_kaisou/slot_exchange_index"].ResponseReceived += Updated;
			o["api_get_member/require_info"].ResponseReceived += Updated;
			o["api_req_kaisou/slot_deprive"].ResponseReceived += Updated;


			//追加するときは FormFleetOverview にも同様に追加してください

			Utility.Configuration.Instance.ConfigurationChanged += ConfigurationChanged;
		}


		void Updated(string apiname, dynamic data)
		{

			if (IsRemodeling)
			{
				if (apiname == "api_get_member/slot_item")
					IsRemodeling = false;
				else
					return;
			}
			if (apiname == "api_req_kaisou/remodeling")
			{
				IsRemodeling = true;
				return;
			}

			KCDatabase db = KCDatabase.Instance;

			if (db.Ships.Count == 0) return;

			FleetData fleet = db.Fleet.Fleets[FleetID];
			if (fleet == null) return;

			TableFleet.SuspendLayout();
			ControlFleet.Update(fleet);
			TableFleet.Visible = true;
			TableFleet.ResumeLayout();

			AnchorageRepairBound = fleet.CanAnchorageRepair ? 2 + fleet.MembersInstance[0].SlotInstance.Count(eq => eq != null && eq.MasterEquipment.CategoryType == EquipmentTypes.RepairFacility) : 0;

			TableMember.SuspendLayout();
			TableMember.RowCount = fleet.Members.Count(id => id > 0);
			for (int i = 0; i < ControlMember.Length; i++)
			{
				ControlMember[i].Update(i < fleet.Members.Count ? fleet.Members[i] : -1);
			}
			TableMember.ResumeLayout();


			if (Icon != null) ResourceManager.DestroyIcon(Icon);
			Icon = ResourceManager.ImageToIcon(ResourceManager.Instance.Icons.Images[ControlFleet.State.GetIconIndex()]);
			if (Parent != null) Parent.Refresh();       //アイコンを更新するため

		}


		void UpdateTimerTick()
		{

			FleetData fleet = KCDatabase.Instance.Fleet.Fleets[FleetID];

			TableFleet.SuspendLayout();
			{
				if (fleet != null)
					ControlFleet.Refresh();

			}
			TableFleet.ResumeLayout();

			TableMember.SuspendLayout();
			for (int i = 0; i < ControlMember.Length; i++)
			{
				ControlMember[i].HP.Refresh();
			}
			TableMember.ResumeLayout();


			// anchorage repairing
			if (fleet != null && Utility.Configuration.Config.FormFleet.ReflectAnchorageRepairHealing)
			{
				TimeSpan elapsed = DateTime.Now - KCDatabase.Instance.Fleet.AnchorageRepairingTimer;

				if (elapsed.TotalMinutes >= 20 && AnchorageRepairBound > 0)
				{

					for (int i = 0; i < AnchorageRepairBound; i++)
					{
						var hpbar = ControlMember[i].HP;

						double dockingSeconds = hpbar.Tag as double? ?? 0.0;

						if (dockingSeconds <= 0.0)
							continue;

						hpbar.SuspendUpdate();

						if (!hpbar.UsePrevValue)
						{
							hpbar.UsePrevValue = true;
							hpbar.ShowDifference = true;
						}

						int damage = hpbar.MaximumValue - hpbar.PrevValue;
						int healAmount = Math.Min(Calculator.CalculateAnchorageRepairHealAmount(damage, dockingSeconds, elapsed), damage);

						hpbar.RepairTimeShowMode = ShipStatusHPRepairTimeShowMode.MouseOver;
						hpbar.RepairTime = KCDatabase.Instance.Fleet.AnchorageRepairingTimer + Calculator.CalculateAnchorageRepairTime(damage, dockingSeconds, Math.Min(healAmount + 1, damage));
						hpbar.Value = hpbar.PrevValue + healAmount;

						hpbar.ResumeUpdate();
					}
				}
			}
		}


		//艦隊編成のコピー
		private void ContextMenuFleet_CopyFleet_Click(object sender, EventArgs e)
		{

			StringBuilder sb = new StringBuilder();
			KCDatabase db = KCDatabase.Instance;
			FleetData fleet = db.Fleet[FleetID];
			if (fleet == null) return;

			switch (UILanguage) {
				case "zh":
					sb.AppendLine($"{fleet.Name}\t制空战力 {fleet.GetAirSuperiority()} / 索敌能力 {fleet.GetSearchingAbilityString(ControlFleet.BranchWeight)} / 运输能力 {Calculator.GetTPDamage(fleet)}");
					break;
				case "en":
					sb.AppendLine($"{fleet.Name}\tFighter Power {fleet.GetAirSuperiority()} / TOS {fleet.GetSearchingAbilityString(ControlFleet.BranchWeight)} / TP {Calculator.GetTPDamage(fleet)}");
					break;
				default:
					sb.AppendLine($"{fleet.Name}\t制空戦力{fleet.GetAirSuperiority()} / 索敵能力 {fleet.GetSearchingAbilityString(ControlFleet.BranchWeight)} / 輸送能力 {Calculator.GetTPDamage(fleet)}");
					break;
			}
			for (int i = 0; i < fleet.Members.Count; i++)
			{
				if (fleet[i] == -1)
					continue;

				ShipData ship = db.Ships[fleet[i]];

				sb.AppendFormat("{0}/{1}\t", ship.MasterShip.Name, ship.Level);

				var eq = ship.AllSlotInstance;


				if (eq != null)
				{
					for (int j = 0; j < eq.Count; j++)
					{

						if (eq[j] == null) continue;

						int count = 1;
						for (int k = j + 1; k < eq.Count; k++)
						{
							if (eq[k] != null && eq[k].EquipmentID == eq[j].EquipmentID && eq[k].Level == eq[j].Level && eq[k].AircraftLevel == eq[j].AircraftLevel)
							{
								count++;
							}
							else
							{
								break;
							}
						}

						if (count == 1)
						{
							sb.AppendFormat("{0}{1}", j == 0 ? "" : ", ", eq[j].NameWithLevel);
						}
						else
						{
							sb.AppendFormat("{0}{1}x{2}", j == 0 ? "" : ", ", eq[j].NameWithLevel, count);
						}

						j += count - 1;
					}
				}

				sb.AppendLine();
			}


			Clipboard.SetData(DataFormats.StringFormat, sb.ToString());
		}


		private void ContextMenuFleet_Opening(object sender, CancelEventArgs e)
		{

			ContextMenuFleet_Capture.Visible = Utility.Configuration.Config.Debug.EnableDebugMenu;

		}



		/// <summary>
		/// 「艦隊デッキビルダー」用編成コピー
		/// <see cref="http://www.kancolle-calc.net/deckbuilder.html"/>
		/// </summary>
		private void ContextMenuFleet_CopyFleetDeckBuilder_Click(object sender, EventArgs e)
		{

			StringBuilder sb = new StringBuilder();
			KCDatabase db = KCDatabase.Instance;

			// 手書き json の悲しみ

			sb.Append(@"{""version"":4,");

			foreach (var fleet in db.Fleet.Fleets.Values)
			{
				if (fleet == null || fleet.MembersInstance.All(m => m == null)) continue;

				sb.AppendFormat(@"""f{0}"":{{", fleet.FleetID);

				int shipcount = 1;
				foreach (var ship in fleet.MembersInstance)
				{
					if (ship == null) break;

					sb.AppendFormat(@"""s{0}"":{{""id"":{1},""lv"":{2},""luck"":{3},""items"":{{",
						shipcount,
						ship.ShipID,
						ship.Level,
						ship.LuckBase);

					int eqcount = 1;
					foreach (var eq in ship.AllSlotInstance.Where(eq => eq != null))
					{
						if (eq == null) break;

						sb.AppendFormat(@"""i{0}"":{{""id"":{1},""rf"":{2},""mas"":{3}}},", eqcount >= 6 ? "x" : eqcount.ToString(), eq.EquipmentID, eq.Level, eq.AircraftLevel);

						eqcount++;
					}

					if (eqcount > 1)
						sb.Remove(sb.Length - 1, 1);        // remove ","
					sb.Append(@"}},");

					shipcount++;
				}

				if (shipcount > 0)
					sb.Remove(sb.Length - 1, 1);        // remove ","
				sb.Append(@"},");

			}

			sb.Remove(sb.Length - 1, 1);        // remove ","
			sb.Append(@"}");

			Clipboard.SetData(DataFormats.StringFormat, sb.ToString());
		}


		/// <summary>
		/// 「艦隊晒しページ」用編成コピー
		/// <see cref="http://kancolle-calc.net/kanmusu_list.html"/>
		/// </summary>
		private void ContextMenuFleet_CopyKanmusuList_Click(object sender, EventArgs e)
		{

			StringBuilder sb = new StringBuilder();
			KCDatabase db = KCDatabase.Instance;

			// version
			sb.Append(".2");

			// <たね艦娘(完全未改造時)のID, 艦娘リスト>　に分類
			Dictionary<int, List<ShipData>> shiplist = new Dictionary<int, List<ShipData>>();

			foreach (var ship in db.Ships.Values.Where(s => s.IsLocked))
			{
				var master = ship.MasterShip;
				while (master.RemodelBeforeShip != null)
					master = master.RemodelBeforeShip;

				if (!shiplist.ContainsKey(master.ShipID))
				{
					shiplist.Add(master.ShipID, new List<ShipData>() { ship });
				}
				else
				{
					shiplist[master.ShipID].Add(ship);
				}
			}

			// 上で作った分類の各項を文字列化
			foreach (var sl in shiplist)
			{
				sb.Append("|").Append(sl.Key).Append(":");

				foreach (var ship in sl.Value.OrderByDescending(s => s.Level))
				{
					sb.Append(ship.Level);

					// 改造レベルに達しているのに未改造の艦は ".<たね=1, 改=2, 改二=3, ...>" を付加
					if (ship.MasterShip.RemodelAfterShipID != 0 && ship.ExpNextRemodel == 0)
					{
						sb.Append(".");
						int count = 1;
						var master = ship.MasterShip;
						while (master.RemodelBeforeShip != null)
						{
							master = master.RemodelBeforeShip;
							count++;
						}
						sb.Append(count);
					}
					sb.Append(",");
				}

				// 余った "," を削除
				sb.Remove(sb.Length - 1, 1);
			}

			Clipboard.SetData(DataFormats.StringFormat, sb.ToString());
		}


		private void ContextMenuFleet_AntiAirDetails_Click(object sender, EventArgs e)
		{

			var dialog = new DialogAntiAirDefense();

			dialog.SetFleetID(FleetID);
			dialog.Show(this);

		}


		private void ContextMenuFleet_Capture_Click(object sender, EventArgs e)
		{

			using (Bitmap bitmap = new Bitmap(this.ClientSize.Width, this.ClientSize.Height))
			{
				this.DrawToBitmap(bitmap, this.ClientRectangle);

				Clipboard.SetData(DataFormats.Bitmap, bitmap);
			}
		}


		private void ContextMenuFleet_OutputFleetImage_Click(object sender, EventArgs e)
		{

			using (var dialog = new DialogFleetImageGenerator(FleetID))
			{
				dialog.ShowDialog(this);
			}
		}



		void ConfigurationChanged()
		{

			var c = Utility.Configuration.Config;

			#region UI translation
			switch (UILanguage) {
				case "zh":
					ContextMenuFleet_CopyFleet.Text = "复制编成为文本(&C)";
					ContextMenuFleet_CopyFleetDeckBuilder.Text = "复制编成代码 - 「艦隊デッキビルダー」格式(&D)";
					ContextMenuFleet_CopyKanmusuList.Text = "复制编成代码 - 「艦隊晒しページ」格式(&R)";
					ContextMenuFleet_AntiAirDetails.Text = "对空炮火详情(&A)";
					ContextMenuFleet_Capture.Text = "截图该区域(&S)";
					ContextMenuFleet_OutputFleetImage.Text = "生成编成图片(&I)";
					break;
				case "en":
					ContextMenuFleet_CopyFleet.Text = "&Copy Fleet Configuration Text";
					ContextMenuFleet_CopyFleetDeckBuilder.Text = "Copy Fleet Configuration Code (&Deck Builder Format)";
					ContextMenuFleet_CopyKanmusuList.Text = "Copy Fleet Configuration Code (&Kanmusu List Format)";
					ContextMenuFleet_AntiAirDetails.Text = "&Anit-Air Defense Details";
					ContextMenuFleet_Capture.Text = "&Screenshot this Aera";
					ContextMenuFleet_OutputFleetImage.Text = "&Fleet Image Generator";
					break;
				default:
					break;
			}
			#endregion

			MainFont = Font = c.UI.MainFont;
			SubFont = c.UI.SubFont;

			AutoScroll = c.FormFleet.IsScrollable;

			var fleet = KCDatabase.Instance.Fleet[FleetID];

			TableFleet.SuspendLayout();
			if (ControlFleet != null && fleet != null)
			{
				ControlFleet.ConfigurationChanged(this);
				ControlFleet.Update(fleet);
			}
			TableFleet.ResumeLayout();

			TableMember.SuspendLayout();
			if (ControlMember != null)
			{
				bool showAircraft = c.FormFleet.ShowAircraft;
				bool fixShipNameWidth = c.FormFleet.FixShipNameWidth;
				bool shortHPBar = c.FormFleet.ShortenHPBar;
				bool colorMorphing = c.UI.BarColorMorphing;
				Color[] colorScheme = c.UI.BarColorScheme;
				bool showNext = c.FormFleet.ShowNextExp;
				bool showConditionIcon = c.FormFleet.ShowConditionIcon;
				var levelVisibility = c.FormFleet.EquipmentLevelVisibility;
				bool showAircraftLevelByNumber = c.FormFleet.ShowAircraftLevelByNumber;
				int fixedShipNameWidth = c.FormFleet.FixedShipNameWidth;
				bool isLayoutFixed = c.UI.IsLayoutFixed;

				for (int i = 0; i < ControlMember.Length; i++)
				{
					var member = ControlMember[i];

					member.Equipments.ShowAircraft = showAircraft;
					if (fixShipNameWidth)
					{
						member.Name.AutoSize = false;
						member.Name.Size = new Size(fixedShipNameWidth, 20);
					}
					else
					{
						member.Name.AutoSize = true;
					}

					member.HP.SuspendUpdate();
					member.HP.Text = shortHPBar ? "" : "HP:";
					member.HP.HPBar.ColorMorphing = colorMorphing;
					member.HP.HPBar.SetBarColorScheme(colorScheme);
					member.HP.MaximumSize = isLayoutFixed ? new Size(int.MaxValue, (int)ControlHelper.GetDefaultRowStyle().Height - member.HP.Margin.Vertical) : Size.Empty;
					member.HP.ResumeUpdate();
					member.Level.TextNext = showNext ? "next:" : null;
					member.Condition.ImageAlign = showConditionIcon ? ContentAlignment.MiddleLeft : ContentAlignment.MiddleCenter;
					member.Equipments.LevelVisibility = levelVisibility;
					member.Equipments.ShowAircraftLevelByNumber = showAircraftLevelByNumber;
					member.Equipments.MaximumSize = isLayoutFixed ? new Size(int.MaxValue, (int)ControlHelper.GetDefaultRowStyle().Height - member.Equipments.Margin.Vertical) : Size.Empty;
					member.ShipResource.BarFuel.ColorMorphing =
					member.ShipResource.BarAmmo.ColorMorphing = colorMorphing;
					member.ShipResource.BarFuel.SetBarColorScheme(colorScheme);
					member.ShipResource.BarAmmo.SetBarColorScheme(colorScheme);

					member.ConfigurationChanged(this);
					if (fleet != null)
						member.Update(i < fleet.Members.Count ? fleet.Members[i] : -1);
				}
			}

			ControlHelper.SetTableRowStyles(TableMember, ControlHelper.GetDefaultRowStyle());
			TableMember.ResumeLayout();

			TableMember.Location = new Point(TableMember.Location.X, TableFleet.Bottom /*+ Math.Max( TableFleet.Margin.Bottom, TableMember.Margin.Top )*/ );

			TableMember.PerformLayout();        //fixme:サイズ変更に親パネルが追随しない

		}



		private void TableMember_CellPaint(object sender, TableLayoutCellPaintEventArgs e)
		{
			e.Graphics.DrawLine(UIColorScheme.Colors.SubBGPen, e.CellBounds.X, e.CellBounds.Bottom - 1, e.CellBounds.Right - 1, e.CellBounds.Bottom - 1);
		}


		protected override string GetPersistString()
		{
			return "Fleet #" + FleetID.ToString();
		}


		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			ControlFleet.Dispose();
			for (int i = 0; i < ControlMember.Length; i++)
				ControlMember[i].Dispose();


			// --- auto generated ---
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}
	}

}

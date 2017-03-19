﻿using ElectronicObserver.Data;
using ElectronicObserver.Observer;
using ElectronicObserver.Resource;
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
using ElectronicObserver.Utility.Data;
using ElectronicObserver.Window.Support;
using ElectronicObserver.Resource.Record;

namespace ElectronicObserver.Window {

	public partial class FormHeadquarters : DockContent {

		private Form _parentForm;

		public FormHeadquarters( FormMain parent ) {
			InitializeComponent();

			_parentForm = parent;


			ImageList icons = ResourceManager.Instance.Icons;

			ShipCount.ImageList = icons;
			ShipCount.ImageIndex = (int)ResourceManager.IconContent.HeadQuartersShip;
			EquipmentCount.ImageList = icons;
			EquipmentCount.ImageIndex = (int)ResourceManager.IconContent.HeadQuartersEquipment;
			InstantRepair.ImageList = icons;
			InstantRepair.ImageIndex = (int)ResourceManager.IconContent.ItemInstantRepair;
			InstantConstruction.ImageList = icons;
			InstantConstruction.ImageIndex = (int)ResourceManager.IconContent.ItemInstantConstruction;
			DevelopmentMaterial.ImageList = icons;
			DevelopmentMaterial.ImageIndex = (int)ResourceManager.IconContent.ItemDevelopmentMaterial;
			ModdingMaterial.ImageList = icons;
			ModdingMaterial.ImageIndex = (int)ResourceManager.IconContent.ItemModdingMaterial;
			FurnitureCoin.ImageList = icons;
			FurnitureCoin.ImageIndex = (int)ResourceManager.IconContent.ItemFurnitureCoin;
			Fuel.ImageList = icons;
			Fuel.ImageIndex = (int)ResourceManager.IconContent.ResourceFuel;
			Ammo.ImageList = icons;
			Ammo.ImageIndex = (int)ResourceManager.IconContent.ResourceAmmo;
			Steel.ImageList = icons;
			Steel.ImageIndex = (int)ResourceManager.IconContent.ResourceSteel;
			Bauxite.ImageList = icons;
			Bauxite.ImageIndex = (int)ResourceManager.IconContent.ResourceBauxite;
			DisplayUseItem.ImageList = icons;
			DisplayUseItem.ImageIndex = (int)ResourceManager.IconContent.ItemPresentBox;


			ControlHelper.SetDoubleBuffered( FlowPanelMaster );
			ControlHelper.SetDoubleBuffered( FlowPanelAdmiral );
			ControlHelper.SetDoubleBuffered( FlowPanelFleet );
			ControlHelper.SetDoubleBuffered( FlowPanelUseItem );
			ControlHelper.SetDoubleBuffered( FlowPanelResource );


			ConfigurationChanged();

			Icon = ResourceManager.ImageToIcon( ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormHeadQuarters] );

		}


		private void FormHeadquarters_Load( object sender, EventArgs e ) {

			APIObserver o = APIObserver.Instance;

			o.APIList["api_req_nyukyo/start"].RequestReceived += Updated;
			o.APIList["api_req_nyukyo/speedchange"].RequestReceived += Updated;
			o.APIList["api_req_kousyou/createship"].RequestReceived += Updated;
			o.APIList["api_req_kousyou/createship_speedchange"].RequestReceived += Updated;
			o.APIList["api_req_kousyou/destroyship"].RequestReceived += Updated;
			o.APIList["api_req_kousyou/destroyitem2"].RequestReceived += Updated;
			o.APIList["api_req_member/updatecomment"].RequestReceived += Updated;

			o.APIList["api_get_member/basic"].ResponseReceived += Updated;
			o.APIList["api_get_member/slot_item"].ResponseReceived += Updated;
			o.APIList["api_port/port"].ResponseReceived += Updated;
			o.APIList["api_get_member/ship2"].ResponseReceived += Updated;
			o.APIList["api_req_kousyou/getship"].ResponseReceived += Updated;
			o.APIList["api_req_hokyu/charge"].ResponseReceived += Updated;
			o.APIList["api_req_kousyou/destroyship"].ResponseReceived += Updated;
			o.APIList["api_req_kousyou/destroyitem2"].ResponseReceived += Updated;
			o.APIList["api_req_kaisou/powerup"].ResponseReceived += Updated;
			o.APIList["api_req_kousyou/createitem"].ResponseReceived += Updated;
			o.APIList["api_req_kousyou/remodel_slot"].ResponseReceived += Updated;
			o.APIList["api_get_member/material"].ResponseReceived += Updated;
			o.APIList["api_get_member/ship_deck"].ResponseReceived += Updated;
			o.APIList["api_req_air_corps/set_plane"].ResponseReceived += Updated;
			o.APIList["api_req_air_corps/supply"].ResponseReceived += Updated;
			o.APIList["api_get_member/useitem"].ResponseReceived += Updated;
			

			Utility.Configuration.Instance.ConfigurationChanged += ConfigurationChanged;
			Utility.SystemEvents.UpdateTimerTick += SystemEvents_UpdateTimerTick;

			FlowPanelResource.SetFlowBreak( Ammo, true );

			FlowPanelMaster.Visible = false;

		}



		void ConfigurationChanged() {

			Font = FlowPanelMaster.Font = Utility.Configuration.Config.UI.JapFont;
			HQLevel.MainFont = Utility.Configuration.Config.UI.JapFont;
			HQLevel.SubFont = Utility.Configuration.Config.UI.JapFont2;
			HQLevel.MainFontColor = Utility.Configuration.Config.UI.ForeColor;
			HQLevel.SubFontColor  = Utility.Configuration.Config.UI.SubForeColor;

			// 点滅しない設定にしたときに消灯状態で固定されるのを防ぐ
			if ( !Utility.Configuration.Config.FormHeadquarters.BlinkAtMaximum ) {
				if ( ShipCount.Tag as bool? ?? false ) {
					ShipCount.BackColor = Utility.Configuration.Config.UI.Headquarters_ShipCountOverBG;
					ShipCount.ForeColor = Utility.Configuration.Config.UI.Headquarters_ShipCountOverFG;
				}

				if ( EquipmentCount.Tag as bool? ?? false ) {
					EquipmentCount.BackColor = Utility.Configuration.Config.UI.Headquarters_ShipCountOverBG;
					EquipmentCount.ForeColor = Utility.Configuration.Config.UI.Headquarters_ShipCountOverFG;
				}
			}

			//visibility
			CheckVisibilityConfiguration();
			{
				var visibility = Utility.Configuration.Config.FormHeadquarters.Visibility.List;
				AdmiralName.Visible = visibility[0];
				AdmiralComment.Visible = visibility[1];
				HQLevel.Visible = visibility[2];
				ShipCount.Visible = visibility[3];
				EquipmentCount.Visible = visibility[4];
				InstantRepair.Visible = visibility[5];
				InstantConstruction.Visible = visibility[6];
				DevelopmentMaterial.Visible = visibility[7];
				ModdingMaterial.Visible = visibility[8];
				FurnitureCoin.Visible = visibility[9];
				Fuel.Visible = visibility[10];
				Ammo.Visible = visibility[11];
				Steel.Visible = visibility[12];
				Bauxite.Visible = visibility[13];
				DisplayUseItem.Visible = visibility[14];
			}

			UpdateDisplayUseItem();
		}


		/// <summary>
		/// VisibleFlags 設定をチェックし、不正な値だった場合は初期値に戻します。
		/// </summary>
		public static void CheckVisibilityConfiguration() {
			const int count = 15;
			var config = Utility.Configuration.Config.FormHeadquarters;

			if ( config.Visibility == null )
				config.Visibility = new Utility.Storage.SerializableList<bool>( Enumerable.Repeat( true, count ).ToList() );

			for ( int i = config.Visibility.List.Count; i < count; i++ ) {
				config.Visibility.List.Add( true );
			}

		}

		/// <summary>
		/// 各表示項目の名称を返します。
		/// </summary>
		public static IEnumerable<string> GetItemNames() {
			yield return "提督名";
			yield return "提督签名";
			yield return "司令部等级";
			yield return "舰船数";
			yield return "装备数";
			yield return "高速修复材";
			yield return "高速建造材";
			yield return "开发资材";
			yield return "改修资材";
			yield return "家具币";
			yield return "燃料";
			yield return "弹药";
			yield return "钢材";
			yield return "铝土";
			yield return "自定义物品";
		}


		void Updated( string apiname, dynamic data ) {

			KCDatabase db = KCDatabase.Instance;


			if ( !db.Admiral.IsAvailable )
				return;


			// 資源上限超過時の色
			Color overcolor = Color.Moccasin;



			FlowPanelMaster.SuspendLayout();

			//Admiral
			FlowPanelAdmiral.SuspendLayout();
			AdmiralName.Text = string.Format( "{0} {1}", db.Admiral.AdmiralName, Constants.GetAdmiralRank( db.Admiral.Rank ) );
			AdmiralComment.Text = db.Admiral.Comment;
			FlowPanelAdmiral.ResumeLayout();

			//HQ Level
			HQLevel.Value = db.Admiral.Level;
			{
				StringBuilder tooltip = new StringBuilder();
				if ( Utility.Configuration.Config.UI.ShowGrowthInsteadOfNextInHQ ) {
					HQLevel.TextNext = "Growth:";
					var res1 = RecordManager.Instance.Resource.GetRecordPrevious();
					var res2 = RecordManager.Instance.Resource.GetRecordDay();
					if ( res1 != null && res2 != null ) {
						HQLevel.TextValueNext = String.Format(
							"{0:n2} / {1:n2}",
							(db.Admiral.Exp - res1.HQExp) * 7 / 10000.0,
							(db.Admiral.Exp - res2.HQExp) * 7 / 10000.0
						);
					} else {
						HQLevel.TextValueNext = "N/A";
					}
				} else {
					if ( db.Admiral.Level < ExpTable.AdmiralExp.Max( e => e.Key ) ) {
						HQLevel.TextNext = "next:";
						HQLevel.ValueNext = ExpTable.GetNextExpAdmiral( db.Admiral.Exp );
						tooltip.AppendFormat( "{0} / {1}\r\n", db.Admiral.Exp, ExpTable.AdmiralExp[db.Admiral.Level + 1].Total );
					} else {
						HQLevel.TextNext = "exp:";
						HQLevel.ValueNext = db.Admiral.Exp;
					}
				}



				//戦果ツールチップ
				//fixme: もっとましな書き方はなかっただろうか
				{
					var res = RecordManager.Instance.Resource.GetRecordPrevious();
					if ( res != null ) {
						int diff = db.Admiral.Exp - res.HQExp;
						tooltip.AppendFormat( "本次 : +{0} exp. / 战果 {1:n2}\r\n", diff, diff * 7 / 10000.0 );
					}
				}
				{
					var res = RecordManager.Instance.Resource.GetRecordDay();
					if ( res != null ) {
						int diff = db.Admiral.Exp - res.HQExp;
						tooltip.AppendFormat( "今日 : +{0} exp. / 战果 {1:n2}\r\n", diff, diff * 7 / 10000.0 );
					}
				}
				{
					var res = RecordManager.Instance.Resource.GetRecordMonth();
					if ( res != null ) {
						int diff = db.Admiral.Exp - res.HQExp;
						tooltip.AppendFormat( "本月 : +{0} exp. / 战果 {1:n2}\r\n", diff, diff * 7 / 10000.0 );
					}
				}

				ToolTipInfo.SetToolTip( HQLevel, tooltip.ToString() );
			}

			//Fleet
			FlowPanelFleet.SuspendLayout();
			{

				ShipCount.Text = string.Format( "{0}/{1}", RealShipCount, db.Admiral.MaxShipCount );
				if ( RealShipCount > db.Admiral.MaxShipCount - 5 ) {
					ShipCount.BackColor = Utility.Configuration.Config.UI.Headquarters_ShipCountOverBG;
					ShipCount.ForeColor = Utility.Configuration.Config.UI.Headquarters_ShipCountOverFG;
				} else {
					ShipCount.BackColor = Color.Transparent;
					ShipCount.ForeColor = Utility.Configuration.Config.UI.ForeColor;
				}
				ShipCount.Tag = RealShipCount >= db.Admiral.MaxShipCount;

				EquipmentCount.Text = string.Format( "{0}/{1}", RealEquipmentCount, db.Admiral.MaxEquipmentCount );
				if ( RealEquipmentCount > db.Admiral.MaxEquipmentCount + 3 - 20 ) {
					EquipmentCount.BackColor = Utility.Configuration.Config.UI.Headquarters_ShipCountOverBG;
					EquipmentCount.ForeColor = Utility.Configuration.Config.UI.Headquarters_ShipCountOverFG;
				} else {
					EquipmentCount.BackColor = Color.Transparent;
					EquipmentCount.ForeColor = Utility.Configuration.Config.UI.ForeColor;
				}
				EquipmentCount.Tag = RealEquipmentCount >= db.Admiral.MaxEquipmentCount;

			}
			FlowPanelFleet.ResumeLayout();



			var resday = RecordManager.Instance.Resource.GetRecord( DateTime.Now.AddHours( -5 ).Date.AddHours( 5 ) );
			var resweek = RecordManager.Instance.Resource.GetRecord( DateTime.Now.AddHours( -5 ).Date.AddDays( -( ( (int)DateTime.Now.AddHours( -5 ).DayOfWeek + 6 ) % 7 ) ).AddHours( 5 ) );	//月曜日起点
			var resmonth = RecordManager.Instance.Resource.GetRecord( new DateTime( DateTime.Now.Year, DateTime.Now.Month, 1 ).AddHours( 5 ) );


			//UseItems
			FlowPanelUseItem.SuspendLayout();

			InstantRepair.Text = db.Material.InstantRepair.ToString();
			InstantRepair.BackColor = db.Material.InstantRepair >= 3000 ? overcolor : Color.Transparent;
			ToolTipInfo.SetToolTip( InstantRepair, string.Format( "今日 : {0:+##;-##;±0}\n本周 : {1:+##;-##;±0}\n本月 : {2:+##;-##;±0}",
					resday == null ? 0 : ( db.Material.InstantRepair - resday.InstantRepair ),
					resweek == null ? 0 : ( db.Material.InstantRepair - resweek.InstantRepair ),
					resmonth == null ? 0 : ( db.Material.InstantRepair - resmonth.InstantRepair ) ) );

			InstantConstruction.Text = db.Material.InstantConstruction.ToString();
			InstantConstruction.BackColor = db.Material.InstantConstruction >= 3000 ? overcolor : Color.Transparent;
			ToolTipInfo.SetToolTip( InstantConstruction, string.Format( "今日 : {0:+##;-##;±0}\n本周 : {1:+##;-##;±0}\n本月 : {2:+##;-##;±0}",
					resday == null ? 0 : ( db.Material.InstantConstruction - resday.InstantConstruction ),
					resweek == null ? 0 : ( db.Material.InstantConstruction - resweek.InstantConstruction ),
					resmonth == null ? 0 : ( db.Material.InstantConstruction - resmonth.InstantConstruction ) ) );

			DevelopmentMaterial.Text = db.Material.DevelopmentMaterial.ToString();
			DevelopmentMaterial.BackColor = db.Material.DevelopmentMaterial >= 3000 ? overcolor : Color.Transparent;
			ToolTipInfo.SetToolTip( DevelopmentMaterial, string.Format( "今日 : {0:+##;-##;±0}\n本周 : {1:+##;-##;±0}\n本月 : {2:+##;-##;±0}",
					resday == null ? 0 : ( db.Material.DevelopmentMaterial - resday.DevelopmentMaterial ),
					resweek == null ? 0 : ( db.Material.DevelopmentMaterial - resweek.DevelopmentMaterial ),
					resmonth == null ? 0 : ( db.Material.DevelopmentMaterial - resmonth.DevelopmentMaterial ) ) );

			ModdingMaterial.Text = db.Material.ModdingMaterial.ToString();
			ModdingMaterial.BackColor = db.Material.ModdingMaterial >= 3000 ? overcolor : Color.Transparent;
			ToolTipInfo.SetToolTip( ModdingMaterial, string.Format( "今日 : {0:+##;-##;±0}\n本周 : {1:+##;-##;±0}\n本月 : {2:+##;-##;±0}",
					resday == null ? 0 : ( db.Material.ModdingMaterial - resday.ModdingMaterial ),
					resweek == null ? 0 : ( db.Material.ModdingMaterial - resweek.ModdingMaterial ),
					resmonth == null ? 0 : ( db.Material.ModdingMaterial - resmonth.ModdingMaterial ) ) );

			FurnitureCoin.Text = db.Admiral.FurnitureCoin.ToString();
			FurnitureCoin.BackColor = db.Admiral.FurnitureCoin >= 200000 ? overcolor : Color.Transparent;
			{
				int small = db.UseItems[10] != null ? db.UseItems[10].Count : 0;
				int medium = db.UseItems[11] != null ? db.UseItems[11].Count : 0;
				int large = db.UseItems[12] != null ? db.UseItems[12].Count : 0;

				ToolTipInfo.SetToolTip( FurnitureCoin,
						string.Format( "( 小 ) x {0} ( +{1} )\r\n( 中 ) x {2} ( +{3} )\r\n( 大 ) x {4} ( +{5} )\r\n",
							small, small * 200,
							medium, medium * 400,
							large, large * 700 ) );
			}
			UpdateDisplayUseItem();
			FlowPanelUseItem.ResumeLayout();


			//Resources
			FlowPanelResource.SuspendLayout();
			{
				
				Fuel.Text = db.Material.Fuel.ToString();
				if (db.Material.Fuel < db.Admiral.MaxResourceRegenerationAmount) {
					Fuel.ForeColor = Utility.Configuration.Config.UI.Headquarters_ResourceOverFG;
					Fuel.BackColor = Utility.Configuration.Config.UI.Headquarters_ResourceOverBG;
				} else {
					Fuel.ForeColor = Utility.Configuration.Config.UI.ForeColor;
					Fuel.BackColor = Color.Transparent;
				}
				ToolTipInfo.SetToolTip( Fuel, string.Format( "今日 : {0:+##;-##;±0}\n本周 : {1:+##;-##;±0}\n本月 : {2:+##;-##;±0}",
					resday == null ? 0 : ( db.Material.Fuel - resday.Fuel ),
					resweek == null ? 0 : ( db.Material.Fuel - resweek.Fuel ),
					resmonth == null ? 0 : ( db.Material.Fuel - resmonth.Fuel ) ) );

				Ammo.Text = db.Material.Ammo.ToString();
				if (db.Material.Ammo < db.Admiral.MaxResourceRegenerationAmount) {
					Ammo.ForeColor = Utility.Configuration.Config.UI.Headquarters_ResourceOverFG;
					Ammo.BackColor = Utility.Configuration.Config.UI.Headquarters_ResourceOverBG;
				} else {
					Ammo.ForeColor = Utility.Configuration.Config.UI.ForeColor;
					Ammo.BackColor = Color.Transparent;
				}
				ToolTipInfo.SetToolTip( Ammo, string.Format( "今日 : {0:+##;-##;±0}\n本周 : {1:+##;-##;±0}\n本月 : {2:+##;-##;±0}",
					resday == null ? 0 : ( db.Material.Ammo - resday.Ammo ),
					resweek == null ? 0 : ( db.Material.Ammo - resweek.Ammo ),
					resmonth == null ? 0 : ( db.Material.Ammo - resmonth.Ammo ) ) );

				Steel.Text = db.Material.Steel.ToString();
				if (db.Material.Steel < db.Admiral.MaxResourceRegenerationAmount) {
					Steel.ForeColor = Utility.Configuration.Config.UI.Headquarters_ResourceOverFG;
					Steel.BackColor = Utility.Configuration.Config.UI.Headquarters_ResourceOverBG;
				} else {
					Steel.ForeColor = Utility.Configuration.Config.UI.ForeColor;
					Steel.BackColor = Color.Transparent;
				}
				ToolTipInfo.SetToolTip( Steel, string.Format( "今日 : {0:+##;-##;±0}\n本周 : {1:+##;-##;±0}\n本月 : {2:+##;-##;±0}",
					resday == null ? 0 : ( db.Material.Steel - resday.Steel ),
					resweek == null ? 0 : ( db.Material.Steel - resweek.Steel ),
					resmonth == null ? 0 : ( db.Material.Steel - resmonth.Steel ) ) );

				Bauxite.Text = db.Material.Bauxite.ToString();
				if (db.Material.Bauxite < db.Admiral.MaxResourceRegenerationAmount) {
					Bauxite.ForeColor = Utility.Configuration.Config.UI.Headquarters_ResourceOverFG;
					Bauxite.BackColor = Utility.Configuration.Config.UI.Headquarters_ResourceOverBG;
				} else {
					Bauxite.ForeColor = Utility.Configuration.Config.UI.ForeColor;
					Bauxite.BackColor = Color.Transparent;
				}
				ToolTipInfo.SetToolTip( Bauxite, string.Format( "今日 : {0:+##;-##;±0}\n本周 : {1:+##;-##;±0}\n本月 : {2:+##;-##;±0}",
					resday == null ? 0 : ( db.Material.Bauxite - resday.Bauxite ),
					resweek == null ? 0 : ( db.Material.Bauxite - resweek.Bauxite ),
					resmonth == null ? 0 : ( db.Material.Bauxite - resmonth.Bauxite ) ) );

			}
			FlowPanelResource.ResumeLayout();

			FlowPanelMaster.ResumeLayout();
			if ( !FlowPanelMaster.Visible )
				FlowPanelMaster.Visible = true;
			AdmiralName.Refresh();
			AdmiralComment.Refresh();

		}


		void SystemEvents_UpdateTimerTick() {

			KCDatabase db = KCDatabase.Instance;

			if ( db.Ships.Count <= 0 ) return;

			if ( Utility.Configuration.Config.FormHeadquarters.BlinkAtMaximum ) {
				if ( ShipCount.Tag as bool? ?? false ) {
					ShipCount.BackColor = DateTime.Now.Second % 2 == 0 ? Utility.Configuration.Config.UI.Headquarters_ShipCountOverBG : Color.Transparent;
					ShipCount.ForeColor = DateTime.Now.Second % 2 == 0 ? Utility.Configuration.Config.UI.Headquarters_ShipCountOverFG : Utility.Configuration.Config.UI.ForeColor;
				}

				if ( EquipmentCount.Tag as bool? ?? false ) {
					EquipmentCount.BackColor = DateTime.Now.Second % 2 == 0 ? Utility.Configuration.Config.UI.Headquarters_ShipCountOverBG : Color.Transparent;
					EquipmentCount.ForeColor = DateTime.Now.Second % 2 == 0 ? Utility.Configuration.Config.UI.Headquarters_ShipCountOverFG : Utility.Configuration.Config.UI.ForeColor;
				}
			}
		}


		private void Resource_MouseClick( object sender, MouseEventArgs e ) {

			if ( e.Button == System.Windows.Forms.MouseButtons.Right )
				new Dialog.DialogResourceChart().Show( _parentForm );

		}


		private void UpdateDisplayUseItem() {
			var db = KCDatabase.Instance;
			var item = db.UseItems[Utility.Configuration.Config.FormHeadquarters.DisplayUseItemID];
			var itemMaster = db.MasterUseItems[Utility.Configuration.Config.FormHeadquarters.DisplayUseItemID];
			string tail = "\r\n( 可在设置中修改 )";

			if ( item != null ) {
				DisplayUseItem.Text = item.Count.ToString();
				ToolTipInfo.SetToolTip( DisplayUseItem, itemMaster.Name + tail );

			} else if ( itemMaster != null ) {
				DisplayUseItem.Text = "0";
				ToolTipInfo.SetToolTip( DisplayUseItem, itemMaster.Name + tail );

			} else {
				DisplayUseItem.Text = "???";
				ToolTipInfo.SetToolTip( DisplayUseItem, "不明なアイテム (ID: " + Utility.Configuration.Config.FormHeadquarters.DisplayUseItemID + ")" + tail );
			}
		}

		private int RealShipCount {
			get {
				if ( KCDatabase.Instance.Battle != null )
					return KCDatabase.Instance.Ships.Count + KCDatabase.Instance.Battle.DroppedShipCount;

				return KCDatabase.Instance.Ships.Count;
			}
		}

		private int RealEquipmentCount {
			get {
				if ( KCDatabase.Instance.Battle != null )
					return KCDatabase.Instance.Equipments.Count + KCDatabase.Instance.Battle.DroppedEquipmentCount;

				return KCDatabase.Instance.Equipments.Count;
			}
		}


		protected override string GetPersistString() {
			return "HeadQuarters";
		}

	}

}

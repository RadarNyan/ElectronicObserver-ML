using ElectronicObserver.Data;
using ElectronicObserver.Observer;
using ElectronicObserver.Resource;
using ElectronicObserver.Utility;
using ElectronicObserver.Utility.Mathematics;
using ElectronicObserver.Window.Control;
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

	public partial class FormArsenal : DockContent
	{

		private class TableArsenalControl : IDisposable
		{

			public Label ShipName;
			public Label CompletionTime;
			private ToolTip tooltip;

			private string UILanguage;

			public TableArsenalControl(FormArsenal parent)
			{

				#region Initialize

				UILanguage = parent.UILanguage;

				ShipName = new ImageLabel
				{
					Text = "???",
					Anchor = AnchorStyles.Left,
					ForeColor = parent.ForeColor,
					TextAlign = ContentAlignment.MiddleLeft,
					Padding = new Padding(0, 1, 0, 1),
					Margin = new Padding(2, 1, 2, 1),
					MaximumSize = new Size(60, int.MaxValue),
					//ShipName.AutoEllipsis = true;
					ImageAlign = ContentAlignment.MiddleCenter,
					AutoSize = true,
					Visible = true
				};

				CompletionTime = new Label
				{
					Text = "",
					Anchor = AnchorStyles.Left,
					ForeColor = parent.ForeColor,
					Tag = null,
					TextAlign = ContentAlignment.MiddleLeft,
					Padding = new Padding(0, 1, 0, 1),
					Margin = new Padding(2, 1, 2, 1),
					MinimumSize = new Size(60, 10),
					AutoSize = true,
					Visible = true
				};

				ConfigurationChanged(parent);

				tooltip = parent.ToolTipInfo;
				#endregion

			}


			public TableArsenalControl(FormArsenal parent, TableLayoutPanel table, int row)
				: this(parent)
			{

				AddToTable(table, row);
			}


			public void AddToTable(TableLayoutPanel table, int row)
			{

				table.Controls.Add(ShipName, 0, row);
				table.Controls.Add(CompletionTime, 1, row);

			}


			public void Update(int arsenalID)
			{

				KCDatabase db = KCDatabase.Instance;
				ArsenalData arsenal = db.Arsenals[arsenalID];
				bool showShipName = Utility.Configuration.Config.FormArsenal.ShowShipName;

				CompletionTime.ForeColor = UIColorScheme.Colors.MainFG;
				CompletionTime.BackColor = Color.Transparent;
				tooltip.SetToolTip(ShipName, null);
				tooltip.SetToolTip(CompletionTime, null);

				if (arsenal == null || arsenal.State == -1)
				{
					//locked
					ShipName.Text = "";
					CompletionTime.Text = "";
					CompletionTime.Tag = null;

				}
				else if (arsenal.State == 0)
				{
					//empty
					ShipName.Text = "----";
					CompletionTime.Text = "";
					CompletionTime.Tag = null;

				}
				else if (arsenal.State == 2)
				{
					//building
					string name = showShipName ? db.MasterShips[arsenal.ShipID].Name : "???";
					ShipName.Text = name;
					tooltip.SetToolTip(ShipName, name);
					CompletionTime.Text = DateTimeHelper.ToTimeRemainString(arsenal.CompletionTime);
					CompletionTime.Tag = arsenal.CompletionTime;
					switch (UILanguage) {
						case "zh":
							tooltip.SetToolTip(CompletionTime, "完成时间" + DateTimeHelper.TimeToCSVString(arsenal.CompletionTime));
							break;
						case "en":
							tooltip.SetToolTip(CompletionTime, "ETC: " + DateTimeHelper.TimeToCSVString(arsenal.CompletionTime));
							break;
						default:
							tooltip.SetToolTip(CompletionTime, "完了日時 : " + DateTimeHelper.TimeToCSVString(arsenal.CompletionTime));
							break;
					}

				}
				else if (arsenal.State == 3)
				{
					//complete!
					string name = showShipName ? db.MasterShips[arsenal.ShipID].Name : "???";
					ShipName.Text = name;
					tooltip.SetToolTip(ShipName, name);
					switch (UILanguage) {
						case "zh":
							CompletionTime.Text = "完成！";
							break;
						case "en":
							CompletionTime.Text = "Completed!";
							break;
						default:
							CompletionTime.Text = "完成！";
							break;
					}
					CompletionTime.Tag = null;

				}

			}


			public void Refresh(int arsenalID)
			{

				if (CompletionTime.Tag != null)
				{

					var time = (DateTime)CompletionTime.Tag;

					CompletionTime.Text = DateTimeHelper.ToTimeRemainString(time);

					if (Utility.Configuration.Config.FormArsenal.BlinkAtCompletion && (time - DateTime.Now).TotalMilliseconds <= Utility.Configuration.Config.NotifierConstruction.AccelInterval)
					{
						if (DateTime.Now.Second % 2 == 0) {
							CompletionTime.ForeColor = UIColorScheme.Colors.Arsenal_BuildCompleteFG;
							CompletionTime.BackColor = UIColorScheme.Colors.Arsenal_BuildCompleteBG;
						} else {
							CompletionTime.ForeColor = UIColorScheme.Colors.MainFG;
							CompletionTime.BackColor = Color.Transparent;
						}
					}

				}
				else if (Utility.Configuration.Config.FormArsenal.BlinkAtCompletion && !string.IsNullOrWhiteSpace(CompletionTime.Text))
				{
					//完成しているので
					if (DateTime.Now.Second % 2 == 0) {
						CompletionTime.ForeColor = UIColorScheme.Colors.Arsenal_BuildCompleteFG;
						CompletionTime.BackColor = UIColorScheme.Colors.Arsenal_BuildCompleteBG;
					} else {
						CompletionTime.ForeColor = UIColorScheme.Colors.MainFG;
						CompletionTime.BackColor = Color.Transparent;
					}
				}
			}


			public void ConfigurationChanged(FormArsenal parent)
			{

				var config = Utility.Configuration.Config.FormArsenal;

				ShipName.Font = parent.Font;
				CompletionTime.Font = parent.Font;
				CompletionTime.ForeColor = UIColorScheme.Colors.MainFG;
				CompletionTime.BackColor = Color.Transparent;
				ShipName.MaximumSize = new Size(config.MaxShipNameWidth, ShipName.MaximumSize.Height);
			}

			public void Dispose()
			{
				ShipName.Dispose();
				CompletionTime.Dispose();
			}
		}


		private TableArsenalControl[] ControlArsenal;
		private int _buildingID;

		private string UILanguage;

		public FormArsenal(FormMain parent)
		{
			InitializeComponent();

			UILanguage = parent.UILanguage;
			ForeColor = parent.ForeColor;
			BackColor = parent.BackColor;

			switch (UILanguage) {
				case "zh":
					MenuMain_ShowShipName.Text = "显示舰名(&V)";
					Text = "工厂";
					break;
				case "en":
					MenuMain_ShowShipName.Text = "&Show Ship Name";
					Text = "Arsenal";
					break;
				default:
					break;
			}

			Utility.SystemEvents.UpdateTimerTick += UpdateTimerTick;

			ControlHelper.SetDoubleBuffered(TableArsenal);

			TableArsenal.SuspendLayout();
			ControlArsenal = new TableArsenalControl[4];
			for (int i = 0; i < ControlArsenal.Length; i++)
			{
				ControlArsenal[i] = new TableArsenalControl(this, TableArsenal, i);
			}
			TableArsenal.ResumeLayout();

			_buildingID = -1;

			ConfigurationChanged();

			Icon = ResourceManager.ImageToIcon(ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormArsenal]);
		}



		private void FormArsenal_Load(object sender, EventArgs e)
		{

			APIObserver o = APIObserver.Instance;

			o["api_req_kousyou/createship"].RequestReceived += Updated;
			o["api_req_kousyou/createship_speedchange"].RequestReceived += Updated;

			o["api_get_member/kdock"].ResponseReceived += Updated;
			o["api_req_kousyou/getship"].ResponseReceived += Updated;
			o["api_get_member/require_info"].ResponseReceived += Updated;

			Utility.Configuration.Instance.ConfigurationChanged += ConfigurationChanged;

		}


		void Updated(string apiname, dynamic data)
		{

			if (_buildingID != -1 && apiname == "api_get_member/kdock")
			{

				ArsenalData arsenal = KCDatabase.Instance.Arsenals[_buildingID];
				ShipDataMaster ship = KCDatabase.Instance.MasterShips[arsenal.ShipID];
				string name;

				if (Utility.Configuration.Config.Log.ShowSpoiler && Utility.Configuration.Config.FormArsenal.ShowShipName)
				{

					name = string.Format("{0}「{1}」", ship.ShipTypeName, ship.NameWithClass);

				}
				else
				{
					switch (UILanguage) {
						case "zh":
							name = "新舰娘";
							break;
						case "en":
							name = "new ship";
							break;
						default:
							name = "艦娘";
							break;
					}
				}

				switch (UILanguage) {
					case "zh":
						Utility.Logger.Add(2,
							$"工厂船坞 #{_buildingID} 开始建造{name}。" +
							$"({arsenal.Fuel}/{arsenal.Ammo}/{arsenal.Steel}/{arsenal.Bauxite}-{arsenal.DevelopmentMaterial}" +
							$" 秘书舰：{KCDatabase.Instance.Fleet[1].MembersInstance[0].NameWithLevel})");
						break;
					case "en":
						Utility.Logger.Add(2,
							$"Arsenal dock #{_buildingID} begins building of {name}." +
							$"({arsenal.Fuel}/{arsenal.Ammo}/{arsenal.Steel}/{arsenal.Bauxite}-{arsenal.DevelopmentMaterial}" +
							$" Secretary: {KCDatabase.Instance.Fleet[1].MembersInstance[0].NameWithLevel})");
						break;
					default:
						Utility.Logger.Add(2,
							$"工廠ドック #{_buildingID}で {name}の建造を開始しました。" +
							$"({arsenal.Fuel}/{arsenal.Ammo}/{arsenal.Steel}/{arsenal.Bauxite}-{arsenal.DevelopmentMaterial}" +
							$" 秘書艦: {KCDatabase.Instance.Fleet[1].MembersInstance[0].NameWithLevel})");
						break;
				}

				_buildingID = -1;
			}

			if (apiname == "api_req_kousyou/createship")
			{
				_buildingID = int.Parse(data["api_kdock_id"]);
			}

			UpdateUI();
		}

		void UpdateUI()
		{

			if (ControlArsenal == null) return;

			TableArsenal.SuspendLayout();
			TableArsenal.RowCount = KCDatabase.Instance.Arsenals.Values.Count(a => a.State != -1);
			for (int i = 0; i < ControlArsenal.Length; i++)
				ControlArsenal[i].Update(i + 1);
			TableArsenal.ResumeLayout();

		}

		void UpdateTimerTick()
		{

			TableArsenal.SuspendLayout();
			for (int i = 0; i < ControlArsenal.Length; i++)
				ControlArsenal[i].Refresh(i + 1);
			TableArsenal.ResumeLayout();

		}


		void ConfigurationChanged()
		{

			Font = Utility.Configuration.Config.UI.MainFont;
			MenuMain_ShowShipName.Checked = Utility.Configuration.Config.FormArsenal.ShowShipName;

			if (ControlArsenal != null)
			{
				TableArsenal.SuspendLayout();

				foreach (var c in ControlArsenal)
					c.ConfigurationChanged(this);

				ControlHelper.SetTableRowStyles(TableArsenal, ControlHelper.GetDefaultRowStyle());

				TableArsenal.ResumeLayout();
			}
		}


		private void MenuMain_ShowShipName_CheckedChanged(object sender, EventArgs e)
		{
			Utility.Configuration.Config.FormArsenal.ShowShipName = MenuMain_ShowShipName.Checked;

			UpdateUI();
		}


		private void TableArsenal_CellPaint(object sender, TableLayoutCellPaintEventArgs e)
		{
			e.Graphics.DrawLine(UIColorScheme.Colors.SubBGPen, e.CellBounds.X, e.CellBounds.Bottom - 1, e.CellBounds.Right - 1, e.CellBounds.Bottom - 1);
		}



		protected override string GetPersistString()
		{
			return "Arsenal";
		}



	}

}

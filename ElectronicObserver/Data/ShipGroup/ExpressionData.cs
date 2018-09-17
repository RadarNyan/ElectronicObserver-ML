using ElectronicObserver.Utility.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ElectronicObserver.Data.ShipGroup
{

	/// <summary>
	/// 艦船フィルタの式データ
	/// </summary>
	[DataContract(Name = "ExpressionData")]
	public class ExpressionData : ICloneable
	{

		private static string UILanguage = Utility.Configuration.Config.UI.Language;

		public enum ExpressionOperator
		{
			Equal,
			NotEqual,
			LessThan,
			LessEqual,
			GreaterThan,
			GreaterEqual,
			Contains,
			NotContains,
			BeginWith,
			NotBeginWith,
			EndWith,
			NotEndWith,
			ArrayContains,
			ArrayNotContains,
		}


		[DataMember]
		public string LeftOperand { get; set; }

		[DataMember]
		public ExpressionOperator Operator { get; set; }

		[DataMember]
		public object RightOperand { get; set; }


		[DataMember]
		public bool Enabled { get; set; }


		[IgnoreDataMember]
		private static readonly Regex regex_index = new Regex(@"\.(?<name>\w+)(\[(?<index>\d+?)\])?", RegexOptions.Compiled);

		[IgnoreDataMember]
		private static readonly Dictionary<string, string> LeftOperandNameTable_zh = new Dictionary<string, string>() {
			{ ".MasterID", "舰船唯一ID" },
			{ ".ShipID", "舰船ID" },
			{ ".MasterShip.NameWithClass", "舰名" },
			{ ".MasterShip.ShipType", "舰种" },
			{ ".Level", "等级" },
			{ ".ExpTotal", "经验值" },
			{ ".ExpNext", "距离升级" },
			{ ".ExpNextRemodel", "距离改装" },
			{ ".HPCurrent", "当前HP" },
			{ ".HPMax", "最大HP" },
			{ ".HPRate", "HP比例" },
			{ ".Condition", "士气" },
			{ ".AllSlotMaster", "装备" },
			{ ".SlotMaster[0]", "装备 #1" },	//checkme: 要る?
			{ ".SlotMaster[1]", "装备 #2" },
			{ ".SlotMaster[2]", "装备 #3" },
			{ ".SlotMaster[3]", "装备 #4" },
			{ ".SlotMaster[4]", "装备 #5" },
			{ ".ExpansionSlotMaster", "补强装备" },
			{ ".Aircraft[0]", "搭载 #1" },
			{ ".Aircraft[1]", "搭载 #2" },
			{ ".Aircraft[2]", "搭载 #3" },
			{ ".Aircraft[3]", "搭载 #4" },
			{ ".Aircraft[4]", "搭载 #5" },
			{ ".AircraftTotal", "搭载数合计" },
			{ ".MasterShip.Aircraft[0]", "最大搭载 #1" },
			{ ".MasterShip.Aircraft[1]", "最大搭载 #2" },
			{ ".MasterShip.Aircraft[2]", "最大搭载 #3" },
			{ ".MasterShip.Aircraft[3]", "最大搭载 #4" },
			{ ".MasterShip.Aircraft[4]", "最大搭载 #5" },
			{ ".MasterShip.AircraftTotal", "最大搭载数合计" },		//要る？
			{ ".AircraftRate[0]", "搭载比例 #1" },
			{ ".AircraftRate[1]", "搭载比例 #2" },
			{ ".AircraftRate[2]", "搭载比例 #3" },
			{ ".AircraftRate[3]", "搭载比例 #4" },
			{ ".AircraftRate[4]", "搭载比例 #5" },
			{ ".AircraftTotalRate", "搭载比例合计" },
			{ ".Fuel", "燃料" },
			{ ".Ammo", "弹药" },
			{ ".FuelMax", "最大燃料载量" },
			{ ".AmmoMax", "最大弹药载量" },
			{ ".FuelRate", "燃料比例" },
			{ ".AmmoRate", "弹药比例" },
			{ ".SlotSize", "格数" },
			{ ".RepairingDockID", "入渠船坞" },
			{ ".RepairTime", "入渠时间" },
			{ ".RepairSteel", "入渠所需钢材" },
			{ ".RepairFuel", "入渠所需燃料" },
			//強化値シリーズは省略
			{ ".FirepowerBase", "基本火力" },
			{ ".TorpedoBase", "基本雷装" },
			{ ".AABase", "基本对空" },
			{ ".ArmorBase", "基本装甲" },
			{ ".EvasionBase", "基本回避" },
			{ ".ASWBase", "基本对潜" },
			{ ".LOSBase", "基本索敌" },
			{ ".LuckBase", "基本运" },
			{ ".FirepowerTotal", "火力合计" },
			{ ".TorpedoTotal", "雷装合计" },
			{ ".AATotal", "对空合计" },
			{ ".ArmorTotal", "装甲合计" },
			{ ".EvasionTotal", "回避合计" },
			{ ".ASWTotal", "对潜合计" },
			{ ".LOSTotal", "索敌合计" },
			{ ".LuckTotal", "运合计" },
			{ ".BomberTotal", "爆装合计" },
			{ ".FirepowerRemain", "剩余火力改修" },
			{ ".TorpedoRemain", "剩余雷装改修" },
			{ ".AARemain", "剩余对空改修" },
			{ ".ArmorRemain", "剩余装甲改修" },
			{ ".LuckRemain", "剩余运改修" },
			{ ".Range", "射程" },		//現在の射程
			{ ".Speed", "速度" },
			{ ".MasterShip.Speed", "基础速度" },
			{ ".MasterShip.Rarity", "稀有率" },
			{ ".IsLocked", "锁定" },
			{ ".IsLockedByEquipment", "装备锁" },
			{ ".SallyArea", "活动标签" },
			{ ".FleetWithIndex", "所属舰队" },
			{ ".IsMarried", "已结婚" },
			{ ".AirBattlePower", "航空战威力" },
			{ ".ShellingPower", "炮击威力" },
			{ ".AircraftPower", "空袭威力" },
			{ ".AntiSubmarinePower", "对潜威力" },
			{ ".TorpedoPower", "雷击威力" },
			{ ".NightBattlePower", "夜战威力" },
			{ ".MasterShip.AlbumNo", "图鉴编号" },
			{ ".MasterShip.NameReading", "舰名读法" },
			{ ".MasterShip.RemodelBeforeShipID", "改装前舰船ID" },
			{ ".MasterShip.RemodelAfterShipID", "改装后舰船ID" },
			//マスターのパラメータ系もおそらく意味がないので省略
		};

		[IgnoreDataMember]
		private static readonly Dictionary<string, string> LeftOperandNameTable_en = new Dictionary<string, string>() {
			{ ".MasterID", "Unique ID" },
			{ ".ShipID", "Ship ID" },
			{ ".MasterShip.NameWithClass", "Ship Name" },
			{ ".MasterShip.ShipType", "Ship Type" },
			{ ".Level", "Level" },
			{ ".ExpTotal", "Exp" },
			{ ".ExpNext", "Exp needed to level up" },
			{ ".ExpNextRemodel", "Exp needed to remodel" },
			{ ".HPCurrent", "Current HP" },
			{ ".HPMax", "Max HP" },
			{ ".HPRate", "HP Percentage" },
			{ ".Condition", "Condition" },
			{ ".AllSlotMaster", "Equipments" },
			{ ".SlotMaster[0]", "Equipment #1" },	//checkme: 要る?
			{ ".SlotMaster[1]", "Equipment #2" },
			{ ".SlotMaster[2]", "Equipment #3" },
			{ ".SlotMaster[3]", "Equipment #4" },
			{ ".SlotMaster[4]", "Equipment #5" },
			{ ".ExpansionSlotMaster", "Ex.Equipment" },
			{ ".Aircraft[0]", "Aircraft #1" },
			{ ".Aircraft[1]", "Aircraft #2" },
			{ ".Aircraft[2]", "Aircraft #3" },
			{ ".Aircraft[3]", "Aircraft #4" },
			{ ".Aircraft[4]", "Aircraft #5" },
			{ ".AircraftTotal", "Aircraft Total" },		//要る？
			{ ".MasterShip.Aircraft[0]", "Max Aircraft #1" },
			{ ".MasterShip.Aircraft[1]", "Max Aircraft #2" },
			{ ".MasterShip.Aircraft[2]", "Max Aircraft #3" },
			{ ".MasterShip.Aircraft[3]", "Max Aircraft #4" },
			{ ".MasterShip.Aircraft[4]", "Max Aircraft #5" },
			{ ".MasterShip.AircraftTotal", "Max Aircraft Total" },
			{ ".AircraftRate[0]", "Aircraft #1 Percentage" },
			{ ".AircraftRate[1]", "Aircraft #2 Percentage" },
			{ ".AircraftRate[2]", "Aircraft #3 Percentage" },
			{ ".AircraftRate[3]", "Aircraft #4 Percentage" },
			{ ".AircraftRate[4]", "Aircraft #5 Percentage" },
			{ ".AircraftTotalRate", "Aircraft Total Percentage" },
			{ ".Fuel", "Fuel" },
			{ ".Ammo", "Ammo" },
			{ ".FuelMax", "Max Fuel" },
			{ ".AmmoMax", "Max Ammo" },
			{ ".FuelRate", "Fuel Percentage" },
			{ ".AmmoRate", "Ammo Percentage" },
			{ ".SlotSize", "Slots Count" },
			{ ".RepairingDockID", "Dock ID" },
			{ ".RepairTime", "Repair Time" },
			{ ".RepairSteel", "Repair Steel" },
			{ ".RepairFuel", "Repair Fuel" },
			//強化値シリーズは省略
			{ ".FirepowerBase", "Firepower Base" },
			{ ".TorpedoBase", "Torpedo Base" },
			{ ".AABase", "AA Base" },
			{ ".ArmorBase", "Armor Base" },
			{ ".EvasionBase", "Evasion Base" },
			{ ".ASWBase", "ASW Base" },
			{ ".LOSBase", "LOS Base" },
			{ ".LuckBase", "Luck Base" },
			{ ".FirepowerTotal", "Firepower Total" },
			{ ".TorpedoTotal", "Torpedo Total" },
			{ ".AATotal", "AA Total" },
			{ ".ArmorTotal", "Armor Total" },
			{ ".EvasionTotal", "Evasion Total" },
			{ ".ASWTotal", "ASW Total" },
			{ ".LOSTotal", "LOS Total" },
			{ ".LuckTotal", "Luck Total" },
			{ ".BomberTotal", "Bomber Total" },
			{ ".FirepowerRemain", "Firepower Remain" },
			{ ".TorpedoRemain", "Torpedo Remain" },
			{ ".AARemain", "AA Remain" },
			{ ".ArmorRemain", "Armor Remain" },
			{ ".LuckRemain", "Luck Remain" },
			{ ".Range", "Range" },		//現在の射程
			{ ".Speed", "Speed" },
			{ ".MasterShip.Speed", "Speed Base" },
			{ ".MasterShip.Rarity", "Rarity" },
			{ ".IsLocked", "Locked" },
			{ ".IsLockedByEquipment", "Locked by Equipment" },
			{ ".SallyArea", "Event Tag" },
			{ ".FleetWithIndex", "Fleet Index" },
			{ ".IsMarried", "Married" },
			{ ".AirBattlePower", "Aerial Power" },
			{ ".ShellingPower", "Shelling Power" },
			{ ".AircraftPower", "Aircraft Power" },
			{ ".AntiSubmarinePower", "ASW Power" },
			{ ".TorpedoPower", "Torpedo Power" },
			{ ".NightBattlePower", "Night Battle Power" },
			{ ".MasterShip.AlbumNo", "Album No." },
			{ ".MasterShip.NameReading", "Name Reading" },
			{ ".MasterShip.RemodelBeforeShipID", "Ship ID Before Remodel" },
			{ ".MasterShip.RemodelAfterShipID", "Ship ID After Remodel" },
			//マスターのパラメータ系もおそらく意味がないので省略
		};

		[IgnoreDataMember]
		private static readonly Dictionary<string, string> LeftOperandNameTable_ja = new Dictionary<string, string>() {
			{ ".MasterID", "艦船固有ID" },
			{ ".ShipID", "艦船ID" },
			{ ".MasterShip.NameWithClass", "艦名" },
			{ ".MasterShip.ShipType", "艦種" },
			{ ".Level", "レベル" },
			{ ".ExpTotal", "経験値" },
			{ ".ExpNext", "次のレベルまで" },
			{ ".ExpNextRemodel", "次の改装まで" },
			{ ".HPCurrent", "現在HP" },
			{ ".HPMax", "最大HP" },
			{ ".HPRate", "HP割合" },
			{ ".Condition", "コンディション" },
			{ ".AllSlotMaster", "装備" },
			{ ".SlotMaster[0]", "装備 #1" },	//checkme: 要る?
			{ ".SlotMaster[1]", "装備 #2" },
			{ ".SlotMaster[2]", "装備 #3" },
			{ ".SlotMaster[3]", "装備 #4" },
			{ ".SlotMaster[4]", "装備 #5" },
			{ ".ExpansionSlotMaster", "補強装備" },
			{ ".Aircraft[0]", "搭載 #1" },
			{ ".Aircraft[1]", "搭載 #2" },
			{ ".Aircraft[2]", "搭載 #3" },
			{ ".Aircraft[3]", "搭載 #4" },
			{ ".Aircraft[4]", "搭載 #5" },
			{ ".AircraftTotal", "搭載機数合計" },
			{ ".MasterShip.Aircraft[0]", "最大搭載 #1" },
			{ ".MasterShip.Aircraft[1]", "最大搭載 #2" },
			{ ".MasterShip.Aircraft[2]", "最大搭載 #3" },
			{ ".MasterShip.Aircraft[3]", "最大搭載 #4" },
			{ ".MasterShip.Aircraft[4]", "最大搭載 #5" },
			{ ".MasterShip.AircraftTotal", "最大搭載機数" },		//要る？
			{ ".AircraftRate[0]", "搭載割合 #1" },
			{ ".AircraftRate[1]", "搭載割合 #2" },
			{ ".AircraftRate[2]", "搭載割合 #3" },
			{ ".AircraftRate[3]", "搭載割合 #4" },
			{ ".AircraftRate[4]", "搭載割合 #5" },
			{ ".AircraftTotalRate", "搭載割合合計" },
			{ ".Fuel", "搭載燃料" },
			{ ".Ammo", "搭載弾薬" },
			{ ".FuelMax", "最大搭載燃料" },
			{ ".AmmoMax", "最大搭載弾薬" },
			{ ".FuelRate", "搭載燃料割合" },
			{ ".AmmoRate", "搭載弾薬割合" },
			{ ".SlotSize", "スロット数" },
			{ ".RepairingDockID", "入渠ドック" },
			{ ".RepairTime", "入渠時間" },
			{ ".RepairSteel", "入渠消費鋼材" },
			{ ".RepairFuel", "入渠消費燃料" },
			//強化値シリーズは省略
			{ ".FirepowerBase", "基本火力" },
			{ ".TorpedoBase", "基本雷装" },
			{ ".AABase", "基本対空" },
			{ ".ArmorBase", "基本装甲" },
			{ ".EvasionBase", "基本回避" },
			{ ".ASWBase", "基本対潜" },
			{ ".LOSBase", "基本索敵" },
			{ ".LuckBase", "基本運" },
			{ ".FirepowerTotal", "合計火力" },
			{ ".TorpedoTotal", "合計雷装" },
			{ ".AATotal", "合計対空" },
			{ ".ArmorTotal", "合計装甲" },
			{ ".EvasionTotal", "合計回避" },
			{ ".ASWTotal", "合計対潜" },
			{ ".LOSTotal", "合計索敵" },
			{ ".LuckTotal", "合計運" },
			{ ".BomberTotal", "合計爆装" },
			{ ".FirepowerRemain", "火力改修残り" },
			{ ".TorpedoRemain", "雷装改修残り" },
			{ ".AARemain", "対空改修残り" },
			{ ".ArmorRemain", "装甲改修残り" },
			{ ".LuckRemain", "運改修残り" },
			{ ".Range", "射程" },		//現在の射程
			{ ".Speed", "速力" },
			{ ".MasterShip.Speed", "基礎速力" },
			{ ".MasterShip.Rarity", "レアリティ" },
			{ ".IsLocked", "ロック" },
			{ ".IsLockedByEquipment", "装備ロック" },
			{ ".SallyArea", "出撃海域" },
			{ ".FleetWithIndex", "所属艦隊" },
			{ ".IsMarried", "ケッコンカッコカリ" },
			{ ".AirBattlePower", "航空威力" },
			{ ".ShellingPower", "砲撃威力" },
			{ ".AircraftPower", "空撃威力" },
			{ ".AntiSubmarinePower", "対潜威力" },
			{ ".TorpedoPower", "雷撃威力" },
			{ ".NightBattlePower", "夜戦威力" },
			{ ".MasterShip.AlbumNo", "図鑑番号" },
			{ ".MasterShip.NameReading", "艦名読み" },
			{ ".MasterShip.RemodelBeforeShipID", "改装前艦船ID" },
			{ ".MasterShip.RemodelAfterShipID", "改装後艦船ID" },
			//マスターのパラメータ系もおそらく意味がないので省略		
		};

		public static readonly Dictionary<string, string> LeftOperandNameTable =
			UILanguage == "zh" ? LeftOperandNameTable_zh :
			UILanguage == "en" ? LeftOperandNameTable_en :
			LeftOperandNameTable_ja;

		private static Dictionary<string, Type> ExpressionTypeTable = new Dictionary<string, Type>();


		[IgnoreDataMember]
		private static readonly Dictionary<ExpressionOperator, string> OperatorNameTable_zh = new Dictionary<ExpressionOperator, string>() {
			{ ExpressionOperator.Equal, "等于" },
			{ ExpressionOperator.NotEqual, "不等于" },
			{ ExpressionOperator.LessThan, "小于" },
			{ ExpressionOperator.LessEqual, "未满" },
			{ ExpressionOperator.GreaterThan, "大于" },
			{ ExpressionOperator.GreaterEqual, "超过" },
			{ ExpressionOperator.Contains, "包含" },
			{ ExpressionOperator.NotContains, "不包含" },
			{ ExpressionOperator.BeginWith, "开头为" },
			{ ExpressionOperator.NotBeginWith, "开头不为" },
			{ ExpressionOperator.EndWith, "结尾为" },
			{ ExpressionOperator.NotEndWith, "结尾不为" },
			{ ExpressionOperator.ArrayContains, "包括" },
			{ ExpressionOperator.ArrayNotContains, "不包括" },
		};

		[IgnoreDataMember]
		private static readonly Dictionary<ExpressionOperator, string> OperatorNameTable_en = new Dictionary<ExpressionOperator, string>() {
			{ ExpressionOperator.Equal, "Equal" },
			{ ExpressionOperator.NotEqual, "Not Equal" },
			{ ExpressionOperator.LessThan, "Less than" },
			{ ExpressionOperator.LessEqual, "Equal or Less than" },
			{ ExpressionOperator.GreaterThan, "Greater Than" },
			{ ExpressionOperator.GreaterEqual, "Equal or Greater than" },
			{ ExpressionOperator.Contains, "Contain" },
			{ ExpressionOperator.NotContains, "Not Contain" },
			{ ExpressionOperator.BeginWith, "Begin with" },
			{ ExpressionOperator.NotBeginWith, "Not Begin with" },
			{ ExpressionOperator.EndWith, "End with" },
			{ ExpressionOperator.NotEndWith, "Not End with" },
			{ ExpressionOperator.ArrayContains, "Include" },
			{ ExpressionOperator.ArrayNotContains, "Not Include" },
		};

		[IgnoreDataMember]
		private static readonly Dictionary<ExpressionOperator, string> OperatorNameTable_ja = new Dictionary<ExpressionOperator, string>() {
			{ ExpressionOperator.Equal, "と等しい" },
			{ ExpressionOperator.NotEqual, "と等しくない" },
			{ ExpressionOperator.LessThan, "より小さい" },
			{ ExpressionOperator.LessEqual, "以下" },
			{ ExpressionOperator.GreaterThan, "より大きい" },
			{ ExpressionOperator.GreaterEqual, "以上" },
			{ ExpressionOperator.Contains, "を含む" },
			{ ExpressionOperator.NotContains, "を含まない" },
			{ ExpressionOperator.BeginWith, "から始まる" },
			{ ExpressionOperator.NotBeginWith, "から始まらない" },
			{ ExpressionOperator.EndWith, "で終わる" },
			{ ExpressionOperator.NotEndWith, "で終わらない" },
			{ ExpressionOperator.ArrayContains, "を含む" },
			{ ExpressionOperator.ArrayNotContains, "を含まない" },

		};

		public static readonly Dictionary<ExpressionOperator, string> OperatorNameTable =
			UILanguage == "zh" ? OperatorNameTable_zh :
			UILanguage == "en" ? OperatorNameTable_en :
			OperatorNameTable_ja;

		public ExpressionData()
		{
			Enabled = true;
		}

		public ExpressionData(string left, ExpressionOperator ope, object right)
			: this()
		{
			LeftOperand = left;
			Operator = ope;
			RightOperand = right;
		}


		public Expression Compile(ParameterExpression paramex)
		{

			Expression memberex = null;
			Expression constex = Expression.Constant(RightOperand, RightOperand.GetType());

			{
				Match match = regex_index.Match(LeftOperand);
				if (match.Success)
				{

					do
					{

						if (memberex == null)
						{
							memberex = Expression.PropertyOrField(paramex, match.Groups["name"].Value);
						}
						else
						{
							memberex = Expression.PropertyOrField(memberex, match.Groups["name"].Value);
						}

						if (int.TryParse(match.Groups["index"].Value, out int index))
						{
							memberex = Expression.Property(memberex, "Item", Expression.Constant(index, typeof(int)));
						}

					} while ((match = match.NextMatch()).Success);

				}
				else
				{
					memberex = Expression.PropertyOrField(paramex, LeftOperand);
				}
			}

			if (memberex.Type.IsEnum)
				memberex = Expression.Convert(memberex, typeof(int));

			Expression condex;
			switch (Operator)
			{
				case ExpressionOperator.Equal:
					condex = Expression.Equal(memberex, constex);
					break;
				case ExpressionOperator.NotEqual:
					condex = Expression.NotEqual(memberex, constex);
					break;
				case ExpressionOperator.LessThan:
					condex = Expression.LessThan(memberex, constex);
					break;
				case ExpressionOperator.LessEqual:
					condex = Expression.LessThanOrEqual(memberex, constex);
					break;
				case ExpressionOperator.GreaterThan:
					condex = Expression.GreaterThan(memberex, constex);
					break;
				case ExpressionOperator.GreaterEqual:
					condex = Expression.GreaterThanOrEqual(memberex, constex);
					break;
				case ExpressionOperator.Contains:
					condex = Expression.Call(memberex, typeof(string).GetMethod("Contains", new Type[] { typeof(string) }), constex);
					break;
				case ExpressionOperator.NotContains:
					condex = Expression.Not(Expression.Call(memberex, typeof(string).GetMethod("Contains", new Type[] { typeof(string) }), constex));
					break;
				case ExpressionOperator.BeginWith:
					condex = Expression.Call(memberex, typeof(string).GetMethod("StartsWith", new Type[] { typeof(string) }), constex);
					break;
				case ExpressionOperator.NotBeginWith:
					condex = Expression.Not(Expression.Call(memberex, typeof(string).GetMethod("StartsWith", new Type[] { typeof(string) }), constex));
					break;
				case ExpressionOperator.EndWith:
					condex = Expression.Call(memberex, typeof(string).GetMethod("EndsWith", new Type[] { typeof(string) }), constex);
					break;
				case ExpressionOperator.NotEndWith:
					condex = Expression.Not(Expression.Call(memberex, typeof(string).GetMethod("EndsWith", new Type[] { typeof(string) }), constex));
					break;
				case ExpressionOperator.ArrayContains:  // returns Enumerable.Contains<>( memberex )
					condex = Expression.Call(typeof(Enumerable), "Contains", new Type[] { memberex.Type.GetElementType() ?? memberex.Type.GetGenericArguments().First() }, memberex, constex);
					break;
				case ExpressionOperator.ArrayNotContains:   // returns !Enumerable.Contains<>( memberex )
					condex = Expression.Not(Expression.Call(typeof(Enumerable), "Contains", new Type[] { memberex.Type.GetElementType() ?? memberex.Type.GetGenericArguments().First() }, memberex, constex));
					break;

				default:
					throw new NotImplementedException();
			}

			return condex;
		}



		public static Type GetLeftOperandType(string left)
		{

			if (ExpressionTypeTable.ContainsKey(left))
			{
				return ExpressionTypeTable[left];

			}
			else if (KCDatabase.Instance.Ships.Count > 0)
			{

				object obj = KCDatabase.Instance.Ships.Values.First();

				Match match = regex_index.Match(left);
				if (match.Success)
				{

					do
					{

						if (int.TryParse(match.Groups["index"].Value, out int index))
						{
							obj = ((dynamic)obj.GetType().InvokeMember(match.Groups["name"].Value, System.Reflection.BindingFlags.GetProperty, null, obj, null))[index];
						}
						else
						{
							object obj2 = obj.GetType().InvokeMember(match.Groups["name"].Value, System.Reflection.BindingFlags.GetProperty, null, obj, null);
							if (obj2 == null)
							{   //プロパティはあるけどnull
								var type = obj.GetType().GetProperty(match.Groups["name"].Value).GetType();
								ExpressionTypeTable.Add(left, type);
								return type;
							}
							else
							{
								obj = obj2;
							}
						}

					} while (obj != null && (match = match.NextMatch()).Success);


					if (obj != null)
					{
						ExpressionTypeTable.Add(left, obj.GetType());
						return obj.GetType();
					}
				}

			}

			return null;
		}

		public Type GetLeftOperandType()
		{
			return GetLeftOperandType(LeftOperand);
		}



		public override string ToString()
		{
			switch (UILanguage) {
				case "zh":
					string ro = RightOperandToString();
					if (Regex.Match(ro[0].ToString(), @"[a-zA-Z0-9\u0400-\u052f]") != Match.Empty)
						ro = " " + ro;

					string result_zh = $"{LeftOperandToString()}{OperatorToString()}{ro}";
					switch (result_zh) {
						case "锁定等于是":
						case "锁定不等于否":
							return "已锁定";
						case "锁定等于否":
						case "锁定不等于是":
							return "未锁定";
						default:
							return result_zh;
					}
				case "en":
					string op;
					switch (Operator) {
						case ExpressionOperator.Equal:
							op = "is";
							break;
						case ExpressionOperator.NotEqual:
							op = "isn't";
							break;
						case ExpressionOperator.LessThan:
							op = "is less than";
							break;
						case ExpressionOperator.LessEqual:
							op = "is equal to or less than";
							break;
						case ExpressionOperator.GreaterThan:
							op = "is greater than";
							break;
						case ExpressionOperator.GreaterEqual:
							op = "is equal to or greater than";
							break;
						case ExpressionOperator.Contains:
							op = "contains";
							break;
						case ExpressionOperator.NotContains:
							op = "doesn't contain";
							break;
						case ExpressionOperator.BeginWith:
							op = "begins with";
							break;
						case ExpressionOperator.NotBeginWith:
							op = "doesn't begin with";
							break;
						case ExpressionOperator.EndWith:
							op = "ends with";
							break;
						case ExpressionOperator.NotEndWith:
							op = "doesn't ends with";
							break;
						case ExpressionOperator.ArrayContains:
							op = "includes";
							break;
						case ExpressionOperator.ArrayNotContains:
							op = "doesn't include";
							break;
						default:
							op = OperatorToString();
							break;
					}

					string result_en = $"{LeftOperandToString()} {op} {RightOperandToString()}";
					switch (result_en) {
						case "Locked is true":
						case "Locked isn't false":
							return "Locked";
						case "Locked is false":
						case "Locked isn't true":
							return "Unlocked";
						default:
							return result_en;
					}
				default:
					return $"{LeftOperandToString()} は {RightOperandToString()} {OperatorToString()}";
			}
		}



		/// <summary>
		/// 左辺値の文字列表現を求めます。
		/// </summary>
		public string LeftOperandToString()
		{
			if (LeftOperandNameTable.ContainsKey(LeftOperand))
				return LeftOperandNameTable[LeftOperand];
			else
				return LeftOperand;
		}

		/// <summary>
		/// 演算子の文字列表現を求めます。
		/// </summary>
		public string OperatorToString()
		{
			return OperatorNameTable[Operator];
		}

		/// <summary>
		/// 右辺値の文字列表現を求めます。
		/// </summary>
		public string RightOperandToString()
		{

			if (LeftOperand == ".MasterID")
			{
				var ship = KCDatabase.Instance.Ships[(int)RightOperand];
				if (ship != null)
					return $"{ship.MasterID} ({ship.NameWithLevel})";
				else
				{
					switch (UILanguage) {
						case "zh":
							return $"{(int)RightOperand}（未在籍）";
						case "en":
							return $"{(int)RightOperand} (Unavailable)";
						default:
							return $"{(int)RightOperand} (未在籍)";
					}
				}

			}
			else if (LeftOperand == ".ShipID")
			{
				var ship = KCDatabase.Instance.MasterShips[(int)RightOperand];
				if (ship != null)
					return $"{ship.ShipID} ({ship.NameWithClass})";
				else
				{
					switch (UILanguage) {
						case "zh":
							return $"{(int)RightOperand}（不存在）";
						case "en":
							return $"{(int)RightOperand} (Nonexistence)";
						default:
							return $"{(int)RightOperand} (存在せず)";
					}
				}

			}
			else if (LeftOperand == ".MasterShip.ShipType")
			{
				var shiptype = KCDatabase.Instance.ShipTypes[(int)RightOperand];
				if (shiptype != null)
					return shiptype.Name;
				else
				{
					switch (UILanguage) {
						case "zh":
							return $"{(int)RightOperand}（未定义）";
						case "en":
							return $"{(int)RightOperand} (Undefined)";
						default:
							return $"{(int)RightOperand} (未定義)";
					}
				}

			}
			else if (LeftOperand.Contains("SlotMaster"))
			{
				if ((int)RightOperand == -1)
				{
					switch (UILanguage) {
						case "zh":
							return "（空）";
						case "en":
							return "(Empty)";
						default:
							return "(なし)";
					}
				}
				else
				{
					var eq = KCDatabase.Instance.MasterEquipments[(int)RightOperand];
					if (eq != null)
						return eq.Name;
					else
					{
						switch (UILanguage) {
							case "zh":
								return $"{(int)RightOperand}（未定义）";
							case "en":
								return $"{(int)RightOperand} (Undefined)";
							default:
								return $"{(int)RightOperand} (未定義)";
						}
					}
				}
			}
			else if (LeftOperand.Contains("Rate") && RightOperand is double)
			{
				return ((double)RightOperand).ToString("P0");

			}
			else if (LeftOperand == ".RepairTime")
			{
				return DateTimeHelper.ToTimeRemainString(DateTimeHelper.FromAPITimeSpan((int)RightOperand));

			}
			else if (LeftOperand == ".Range")
			{
				return Constants.GetRange((int)RightOperand);

			}
			else if (LeftOperand == ".Speed" || LeftOperand == ".MasterShip.Speed")
			{
				return Constants.GetSpeed((int)RightOperand);

			}
			else if (LeftOperand == ".MasterShip.Rarity")
			{
				return Constants.GetShipRarity((int)RightOperand);

			}
			else if (LeftOperand == ".MasterShip.AlbumNo")
			{
				var ship = KCDatabase.Instance.MasterShips.Values.FirstOrDefault(s => s.AlbumNo == (int)RightOperand);
				if (ship != null)
					return $"{(int)RightOperand} ({ship.NameWithClass})";
				else
				{
					switch (UILanguage) {
						case "zh":
							return $"{(int)RightOperand}（不存在）";
						case "en":
							return $"{(int)RightOperand} (Nonexistence)";
						default:
							return $"{(int)RightOperand} (存在せず)";
					}
				}

			}
			else if (LeftOperand == ".MasterShip.RemodelAfterShipID")
			{

				if (((int)RightOperand) == 0)
				{
					switch (UILanguage) {
						case "zh":
							return "最终改装";
						case "en":
							return "Fully Remodeled";
						default:
							return "最終改装";
					}
				}

				var ship = KCDatabase.Instance.MasterShips[(int)RightOperand];
				if (ship != null)
					return $"{ship.ShipID} ({ship.NameWithClass})";
				else
				{
					switch (UILanguage) {
						case "zh":
							return $"{(int)RightOperand}（不存在）";
						case "en":
							return $"{(int)RightOperand} (Nonexistence)";
						default:
							return $"{(int)RightOperand} (存在せず)";
					}
				}

			}
			else if (LeftOperand == ".MasterShip.RemodelBeforeShipID")
			{

				if (((int)RightOperand) == 0)
				{
					switch (UILanguage) {
						case "zh":
							return "未改装";
						case "en":
							return "Unremodeled";
						default:
							return "未改装";
					}
				}

				var ship = KCDatabase.Instance.MasterShips[(int)RightOperand];
				if (ship != null)
					return $"{ship.ShipID} ({ship.NameWithClass})";
				else
				{
					switch (UILanguage) {
						case "zh":
							return $"{(int)RightOperand}（不存在）";
						case "en":
							return $"{(int)RightOperand} (Nonexistence)";
						default:
							return $"{(int)RightOperand} (存在せず)";
					}
				}

			}
			else if (RightOperand is bool)
			{
				switch (UILanguage) {
					case "zh":
						return ((bool)RightOperand) ? "是" : "否";
					case "en":
						return ((bool)RightOperand) ? "true" : "false";
					default:
						return ((bool)RightOperand) ? "○" : "×";
				}
			}
			else
			{
				return RightOperand.ToString();

			}

		}


		public ExpressionData Clone()
		{
			var clone = MemberwiseClone();      //checkme: 右辺値に参照型を含む場合死ぬ
			return (ExpressionData)clone;
		}

		object ICloneable.Clone()
		{
			return Clone();
		}
	}




}

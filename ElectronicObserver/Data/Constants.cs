using ElectronicObserver.Utility.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data
{

	public static class Constants
	{

		private static string UILanguage = Utility.Configuration.Config.UI.Language;

		#region 艦船・装備

		public static string GetTranslatedShipName(string shipName)
		{
			if (!Utility.Configuration.Config.UI.TranslateShipNames)
				return shipName;
			switch (UILanguage) {
				case "zh":
					switch (shipName) {
						case "秋月":
							return "秋月";
						case "摩耶":
							return "摩耶";
						case "五十鈴":
							return "五十铃";
						case "霞":
							return "霞";
						case "皐月":
							return "皋月";
						case "鬼怒":
							return "鬼怒";
						case "由良":
							return "由良";
						case "文月":
							return "文月";
						case "UIT-25":
							return "UIT-25";
						case "龍田":
							return "龙田";
						case "伊勢":
							return "伊势";
						case "武蔵":
							return "武藏";
						case "浜風":
							return "滨风";
						default:
							return shipName;
					}
				case "en":
					switch (shipName) {
						case "秋月":
							return "Akizuki";
						case "摩耶":
							return "Maya";
						case "五十鈴":
							return "Isuzu";
						case "霞":
							return "Kasumi";
						case "皐月":
							return "Satsuki";
						case "鬼怒":
							return "Kinu";
						case "由良":
							return "Yura";
						case "文月":
							return "Fumizuki";
						case "UIT-25":
							return "UIT-25";
						case "龍田":
							return "Tatsuta";
						case "伊勢":
							return "Ise";
						case "武蔵":
							return "Musashi";
						case "浜風":
							return "Hamakaze";
						default:
							return shipName;
					}
				default:
					return shipName;
			}
		}

		/// <summary>
		/// 艦船の速力を表す文字列を取得します。
		/// </summary>
		public static string GetSpeed(int value)
		{
			switch (UILanguage) {
			default:
			switch (value)
			{
				case 0:
					return "陸上";
				case 5:
					return "低速";
				case 10:
					return "高速";
				case 15:
					return "高速+";
				case 20:
					return "最速";
				default:
					return "不明";
			}
			case "zh":
				switch (value) {
					case 0:
						return "陆基";
					case 5:
						return "低速";
					case 10:
						return "高速";
					case 15:
						return "高速+";
					case 20:
						return "最速";
					default:
						return "不明";
				}
			case "en":
				switch (value) {
					case 0:
						return "Land Based";
					case 5:
						return "Slow";
					case 10:
						return "Fast";
					case 15:
						return "Fast+";
					case 20:
						return "Fastest";
					default:
						return "Unknown";
				}
			}
		}

		/// <summary>
		/// 射程を表す文字列を取得します。
		/// </summary>
		public static string GetRange(int value)
		{
			switch (UILanguage) {
			default:
			switch (value)
			{
				case 0:
					return "無";
				case 1:
					return "短";
				case 2:
					return "中";
				case 3:
					return "長";
				case 4:
					return "超長";
				case 5:
					return "超長+";
				default:
					return "不明";
			}
			case "zh":
				switch (value) {
					case 0:
						return "无";
					case 1:
						return "短";
					case 2:
						return "中";
					case 3:
						return "长";
					case 4:
						return "超长";
					case 5:
						return "超长+";
					default:
						return "不明";
				}
			case "en":
				switch (value) {
					case 0:
						return "None";
					case 1:
						return "Short";
					case 2:
						return "Medium";
					case 3:
						return "Long";
					case 4:
						return "Very Long";
					case 5:
						return "Very Long+";
					default:
						return "Unknown";
				}
			}
		}

		/// <summary>
		/// 艦船のレアリティを表す文字列を取得します。
		/// </summary>
		public static string GetShipRarity(int value)
		{
			switch (UILanguage) {
			default:
			switch (value)
			{
				case 0:
					return "赤";
				case 1:
					return "藍";
				case 2:
					return "青";
				case 3:
					return "水";
				case 4:
					return "銀";
				case 5:
					return "金";
				case 6:
					return "虹";
				case 7:
					return "輝虹";
				case 8:
					return "桜虹";
				default:
					return "不明";
			}
			case "zh":
				switch (value) {
					case 1:
						return "蓝";
					case 2:
						return "天蓝";
					case 3:
						return "浅蓝";
					case 4:
						return "银";
					case 5:
						return "金";
					case 6:
						return "虹";
					case 7:
						return "辉虹";
					case 8:
						return "樱虹";
					default:
						return "不明";
				}
			case "en":
				switch (value) {
					case 1:
						return "C";
					case 2:
						return "C";
					case 3:
						return "SC";
					case 4:
						return "R";
					case 5:
						return "SR";
					case 6:
						return "H";
					case 7:
						return "SH";
					case 8:
						return "SH+";
					default:
						return "Unknown";
				}
			}
		}

		/// <summary>
		/// 装備のレアリティを表す文字列を取得します。
		/// </summary>
		public static string GetEquipmentRarity(int value)
		{
			switch (value)
			{
				case 0:
					return "コモン";
				case 1:
					return "レア";
				case 2:
					return "ホロ";
				case 3:
					return "Sホロ";
				case 4:
					return "SSホロ";
				case 5:
					return "SSホロ'";
				case 6:
					return "SSホロ+";
				default:
					return "不明";
			}
		}

		/// <summary>
		/// 装備のレアリティの画像インデックスを取得します。
		/// </summary>
		public static int GetEquipmentRarityID(int value)
		{
			switch (value)
			{
				case 0:
					return 1;
				case 1:
					return 3;
				case 2:
					return 4;
				case 3:
					return 5;
				case 4:
					return 6;
				case 5:
					return 7;
				case 6:
					return 8;
				default:
					return 0;
			}
		}


		/// <summary>
		/// 艦船のボイス設定フラグを表す文字列を取得します。
		/// </summary>
		public static string GetVoiceFlag(int value)
		{

			switch (value)
			{
				case 0:
					return "-";
				case 1:
					return "放置";
				case 2:
					return "時報";
				case 3:
					return "放置+時報";
				case 4:
					return "特殊放置";
				case 5:
					return "放置+特殊放置";
				case 6:
					return "時報+特殊放置";
				case 7:
					return "放置+時報+特殊放置";
				default:
					return "不明";
			}
		}


		/// <summary>
		/// 艦船の損傷度合いを表す文字列を取得します。
		/// </summary>
		/// <param name="hprate">現在HP/最大HPで表される割合。</param>
		/// <param name="isPractice">演習かどうか。</param>
		/// <param name="isLandBase">陸上基地かどうか。</param>
		/// <param name="isEscaped">退避中かどうか。</param>
		/// <returns></returns>
		public static string GetDamageState(double hprate, bool isPractice = false, bool isLandBase = false, bool isEscaped = false)
		{
			switch (UILanguage) {
			default:
			if (isEscaped)
				return "退避";
			else if (hprate <= 0.0)
				return isPractice ? "離脱" : (!isLandBase ? "撃沈" : "破壊");
			else if (hprate <= 0.25)
				return !isLandBase ? "大破" : "損壊";
			else if (hprate <= 0.5)
				return !isLandBase ? "中破" : "損害";
			else if (hprate <= 0.75)
				return !isLandBase ? "小破" : "混乱";
			else if (hprate < 1.0)
				return "健在";
			else
				return "無傷";

			case "zh":
				if (isEscaped)
					return "退避";
				else if (hprate <= 0.0)
					return isPractice ? "脱离" : (!isLandBase ? "击沉" : "破坏");
				else if (hprate <= 0.25)
					return !isLandBase ? "大破" : "损坏";
				else if (hprate <= 0.5)
					return !isLandBase ? "中破" : "损害";
				else if (hprate <= 0.75)
					return !isLandBase ? "小破" : "混乱";
				else if (hprate < 1.0)
					return "健在";
				else
					return "无伤";
			case "en":
				if (isEscaped)
					return "Retreated";
				else if (hprate <= 0.0)
					return isPractice ? "Evacuated" : (!isLandBase ? "Sunken" : "Destroyed");
				else if (hprate <= 0.25)
					return !isLandBase ? "Heavily Damaged" : "Broken";
				else if (hprate <= 0.5)
					return !isLandBase ? "Moderately Damaged" : "Damaged";
				else if (hprate <= 0.75)
					return !isLandBase ? "Slightly Damaged" : "Confused";
				else if (hprate < 1.0)
					return "Little Damaged";
				else
					return "No Damage";
			}
		}


		/// <summary>
		/// 基地航空隊の行動指示を表す文字列を取得します。
		/// </summary>
		public static string GetBaseAirCorpsActionKind(int value)
		{
			switch (UILanguage) {
			default:
			switch (value)
			{
				case 0:
					return "待機";
				case 1:
					return "出撃";
				case 2:
					return "防空";
				case 3:
					return "退避";
				case 4:
					return "休息";
				default:
					return "不明";
			}
			case "zh":
				switch (value) {
					case 0:
						return "待机";
					case 1:
						return "出击";
					case 2:
						return "防空";
					case 3:
						return "退避";
					case 4:
						return "休息";
					default:
						return "不明";
				}
			case "en":
				switch (value) {
					case 0:
						return "Standby";
					case 1:
						return "Sortie";
					case 2:
						return "Air Defense";
					case 3:
						return "Retreat";
					case 4:
						return "Rest";
					default:
						return "Unknown";
				}
			}
		}


		/// <summary>
		/// 艦種略号を取得します。
		/// </summary>
		public static string GetShipClassClassification(ShipTypes shiptype)
		{
			switch (shiptype)
			{
				case ShipTypes.Escort:
					return "DE";
				case ShipTypes.Destroyer:
					return "DD";
				case ShipTypes.LightCruiser:
					return "CL";
				case ShipTypes.TorpedoCruiser:
					return "CLT";
				case ShipTypes.HeavyCruiser:
					return "CA";
				case ShipTypes.AviationCruiser:
					return "CAV";
				case ShipTypes.LightAircraftCarrier:
					return "CVL";
				case ShipTypes.Battlecruiser:
					return "BC";    // ? FBB, CC?
				case ShipTypes.Battleship:
					return "BB";
				case ShipTypes.AviationBattleship:
					return "BBV";
				case ShipTypes.AircraftCarrier:
					return "CV";
				case ShipTypes.SuperDreadnoughts:
					return "BB";
				case ShipTypes.Submarine:
					return "SS";
				case ShipTypes.SubmarineAircraftCarrier:
					return "SSV";
				case ShipTypes.Transport:
					return "AP";    // ? AO?
				case ShipTypes.SeaplaneTender:
					return "AV";
				case ShipTypes.AmphibiousAssaultShip:
					return "LHA";
				case ShipTypes.ArmoredAircraftCarrier:
					return "CVB";
				case ShipTypes.RepairShip:
					return "AR";
				case ShipTypes.SubmarineTender:
					return "AS";
				case ShipTypes.TrainingCruiser:
					return "CT";
				case ShipTypes.FleetOiler:
					return "AO";
				default:
					return "IX";
			}
		}


		/// <summary>
		/// 艦型を表す文字列を取得します。
		/// </summary>
		public static string GetShipClass(int id)
		{
			switch (id)
			{
				case 1: return "綾波型";
				case 2: return "伊勢型";
				case 3: return "加賀型";
				case 4: return "球磨型";
				case 5: return "暁型";
				case 6: return "金剛型";
				case 7: return "古鷹型";
				case 8: return "高雄型";
				case 9: return "最上型";
				case 10: return "初春型";
				case 11: return "祥鳳型";
				case 12: return "吹雪型";
				case 13: return "青葉型";
				case 14: return "赤城型";
				case 15: return "千歳型";
				case 16: return "川内型";
				case 17: return "蒼龍型";
				case 18: return "朝潮型";
				case 19: return "長門型";
				case 20: return "長良型";
				case 21: return "天龍型";
				case 22: return "島風型";
				case 23: return "白露型";
				case 24: return "飛鷹型";
				case 25: return "飛龍型";
				case 26: return "扶桑型";
				case 27: return "鳳翔型";
				case 28: return "睦月型";
				case 29: return "妙高型";
				case 30: return "陽炎型";
				case 31: return "利根型";
				case 32: return "龍驤型";
				case 33: return "翔鶴型";
				case 34: return "夕張型";
				case 35: return "海大VI型";
				case 36: return "巡潜乙型改二";
				case 37: return "大和型";
				case 38: return "夕雲型";
				case 39: return "巡潜乙型";
				case 40: return "巡潜3型";
				case 41: return "阿賀野型";
				case 42: return "「霧」";
				case 43: return "大鳳型";
				case 44: return "潜特型(伊400型潜水艦)";
				case 45: return "特種船丙型";
				case 46: return "三式潜航輸送艇";
				case 47: return "Bismarck級";
				case 48: return "Z1型";
				case 49: return "工作艦";
				case 50: return "大鯨型";
				case 51: return "龍鳳型";
				case 52: return "大淀型";
				case 53: return "雲龍型";
				case 54: return "秋月型";
				case 55: return "Admiral Hipper級";
				case 56: return "香取型";
				case 57: return "UボートIXC型";
				case 58: return "V.Veneto級";
				case 59: return "秋津洲型";
				case 60: return "改風早型";
				case 61: return "Maestrale級";
				case 62: return "瑞穂型";
				case 63: return "Graf Zeppelin級";
				case 64: return "Zara級";
				case 65: return "Iowa級";
				case 66: return "神風型";
				case 67: return "Queen Elizabeth級";
				case 68: return "Aquila級";
				case 69: return "Lexington級";
				case 70: return "C.Teste級";
				case 71: return "巡潜甲型改二";
				case 72: return "神威型";
				case 73: return "Гангут級";
				case 74: return "占守型";
				case 75: return "春日丸級";
				case 76: return "大鷹型";
				case 77: return "択捉型";
				case 78: return "Ark Royal級";
				case 79: return "Richelieu級";
				case 80: return "Guglielmo Marconi級";
				case 81: return "Ташкент級";
				case 82: return "J級";
				case 83: return "Casablanca級";
				case 84: return "Essex級";
				case 85: return "日振型";
				case 86: return "呂号潜水艦";
				case 87: return "John C.Butler級";
				default: return "不明";
			}
		}

		#endregion


		#region 出撃

		/// <summary>
		/// マップ上のセルでのイベントを表す文字列を取得します。
		/// </summary>
		public static string GetMapEventID(int value)
		{

			switch (value)
			{

				case 0:
					return "初期位置";
				case 1:
					return "イベントなし";
				case 2:
					return "資源";
				case 3:
					return "渦潮";
				case 4:
					return "通常戦闘";
				case 5:
					return "ボス戦闘";
				case 6:
					return "気のせいだった";
				case 7:
					return "航空戦";
				case 8:
					return "船団護衛成功";
				case 9:
					return "揚陸地点";
				default:
					return "不明";
			}
		}

		/// <summary>
		/// マップ上のセルでのイベント種別を表す文字列を取得します。
		/// </summary>
		public static string GetMapEventKind(int value)
		{

			switch (value)
			{
				case 0:
					return "非戦闘";
				case 1:
					return "昼夜戦";
				case 2:
					return "夜戦";
				case 3:
					return "夜昼戦";       // 対通常?
				case 4:
					return "航空戦";
				case 5:
					return "敵連合";
				case 6:
					return "空襲戦";
				case 7:
					return "夜昼戦";       // 対連合
				default:
					return "不明";
			}
		}


		/// <summary>
		/// 海域難易度を表す文字列を取得します。
		/// </summary>
		public static string GetDifficulty(int value)
		{

			switch (value)
			{
				case -1:
					return "なし";
				case 0:
					return "未選択";
				case 1:
					return "丁";
				case 2:
					return "丙";
				case 3:
					return "乙";
				case 4:
					return "甲";
				default:
					return "不明";
			}
		}

		/// <summary>
		/// 海域難易度を表す数値を取得します。
		/// </summary>
		public static int GetDifficulty(string value)
		{

			switch (value)
			{
				case "未選択":
					return 0;
				case "丁":
					return 1;
				case "丙":
					return 2;
				case "乙":
					return 3;
				case "甲":
					return 4;
				default:
					return -1;
			}

		}

		/// <summary>
		/// 空襲被害の状態を表す文字列を取得します。
		/// </summary>
		public static string GetAirRaidDamage(int value)
		{
			switch (value)
			{
				case 1:
					return "資源に損害";
				case 2:
					return "資源・航空隊に損害";
				case 3:
					return "航空隊に損害";
				case 4:
					return "損害なし";
				default:
					return "発生せず";
			}
		}

		/// <summary>
		/// 空襲被害の状態を表す文字列を取得します。(短縮版)
		/// </summary>
		public static string GetAirRaidDamageShort(int value)
		{
			switch (value)
			{
				case 1:
					return "資源損害";
				case 2:
					return "資源・航空";
				case 3:
					return "航空隊損害";
				case 4:
					return "損害なし";
				default:
					return "-";
			}
		}


		#endregion


		#region 戦闘

		/// <summary>
		/// 陣形を表す文字列を取得します。
		/// </summary>
		public static string GetFormation(int id)
		{
			switch (id)
			{
				case 1:
					return "単縦陣";
				case 2:
					return "複縦陣";
				case 3:
					return "輪形陣";
				case 4:
					return "梯形陣";
				case 5:
					return "単横陣";
				case 6:
					return "警戒陣";
				case 11:
					return "第一警戒航行序列";
				case 12:
					return "第二警戒航行序列";
				case 13:
					return "第三警戒航行序列";
				case 14:
					return "第四警戒航行序列";
				default:
					return "不明";
			}
		}

		/// <summary>
		/// 陣形を表す数値を取得します。
		/// </summary>
		public static int GetFormation(string value)
		{
			switch (value)
			{
				case "単縦陣":
					return 1;
				case "複縦陣":
					return 2;
				case "輪形陣":
					return 3;
				case "梯形陣":
					return 4;
				case "単横陣":
					return 5;
				case "警戒陣":
					return 6;
				case "第一警戒航行序列":
					return 11;
				case "第二警戒航行序列":
					return 12;
				case "第三警戒航行序列":
					return 13;
				case "第四警戒航行序列":
					return 14;
				default:
					return -1;
			}
		}

		/// <summary>
		/// 陣形を表す文字列(短縮版)を取得します。
		/// </summary>
		public static string GetFormationShort(int id)
		{
			switch (id)
			{
				case 1:
					return "単縦陣";
				case 2:
					return "複縦陣";
				case 3:
					return "輪形陣";
				case 4:
					return "梯形陣";
				case 5:
					return "単横陣";
				case 6:
					return "警戒陣";
				case 11:
					return "第一警戒";
				case 12:
					return "第二警戒";
				case 13:
					return "第三警戒";
				case 14:
					return "第四警戒";
				default:
					return "不明";
			}
		}

		/// <summary>
		/// 交戦形態を表す文字列を取得します。
		/// </summary>
		public static string GetEngagementForm(int id)
		{
			switch (id)
			{
				case 1:
					return "同航戦";
				case 2:
					return "反航戦";
				case 3:
					return "T字有利";
				case 4:
					return "T字不利";
				default:
					return "不明";
			}
		}

		/// <summary>
		/// 索敵結果を表す文字列を取得します。
		/// </summary>
		public static string GetSearchingResult(int id)
		{
			switch (id)
			{
				case 1:
					return "成功";
				case 2:
					return "成功(未帰還有)";
				case 3:
					return "未帰還";
				case 4:
					return "失敗";
				case 5:
					return "成功(非索敵機)";
				case 6:
					return "失敗(非索敵機)";
				default:
					return "不明";
			}
		}

		/// <summary>
		/// 索敵結果を表す文字列(短縮版)を取得します。
		/// </summary>
		public static string GetSearchingResultShort(int id)
		{
			switch (id)
			{
				case 1:
					return "成功";
				case 2:
					return "成功△";
				case 3:
					return "未帰還";
				case 4:
					return "失敗";
				case 5:
					return "成功";
				case 6:
					return "失敗";
				default:
					return "不明";
			}
		}

		/// <summary>
		/// 制空戦の結果を表す文字列を取得します。
		/// </summary>
		public static string GetAirSuperiority(int id)
		{
			switch (id)
			{
				case 0:
					return "航空均衡";
				case 1:
					return "制空権確保";
				case 2:
					return "航空優勢";
				case 3:
					return "航空劣勢";
				case 4:
					return "制空権喪失";
				default:
					return "不明";
			}
		}



		/// <summary>
		/// 昼戦攻撃種別を表す文字列を取得します。
		/// </summary>
		public static string GetDayAttackKind(DayAttackKind id)
		{
			switch (UILanguage) {
			default:
			switch (id)
			{
				case DayAttackKind.NormalAttack:
					return "通常攻撃";
				case DayAttackKind.Laser:
					return "レーザー攻撃";
				case DayAttackKind.DoubleShelling:
					return "連続射撃";
				case DayAttackKind.CutinMainSub:
					return "カットイン(主砲/副砲)";
				case DayAttackKind.CutinMainRadar:
					return "カットイン(主砲/電探)";
				case DayAttackKind.CutinMainAP:
					return "カットイン(主砲/徹甲)";
				case DayAttackKind.CutinMainMain:
					return "カットイン(主砲/主砲)";
				case DayAttackKind.CutinAirAttack:
					return "空母カットイン";
				case DayAttackKind.SpecialNelson:
					return "Nelson Touch";
				case DayAttackKind.Shelling:
					return "砲撃";
				case DayAttackKind.AirAttack:
					return "空撃";
				case DayAttackKind.DepthCharge:
					return "爆雷攻撃";
				case DayAttackKind.Torpedo:
					return "雷撃";
				case DayAttackKind.Rocket:
					return "ロケット砲撃";
				case DayAttackKind.LandingDaihatsu:
					return "揚陸攻撃(大発動艇)";
				case DayAttackKind.LandingTokuDaihatsu:
					return "揚陸攻撃(特大発動艇)";
				case DayAttackKind.LandingDaihatsuTank:
					return "揚陸攻撃(大発戦車)";
				case DayAttackKind.LandingAmphibious:
					return "揚陸攻撃(内火艇)";
				case DayAttackKind.LandingTokuDaihatsuTank:
					return "揚陸攻撃(特大発戦車)";
				default:
					return "不明";
			}
			case "zh":
				switch (id) {
					case DayAttackKind.NormalAttack:
						return "普通攻击";
					case DayAttackKind.Laser:
						return "激光攻击";
					case DayAttackKind.DoubleShelling:
						return "连续射击";
					case DayAttackKind.CutinMainSub:
						return "CI（主炮 / 副炮）";
					case DayAttackKind.CutinMainRadar:
						return "CI（主炮 / 电探）";
					case DayAttackKind.CutinMainAP:
						return "CI（主炮 / 穿甲弹）";
					case DayAttackKind.CutinMainMain:
						return "CI（主炮 / 主炮）";
					case DayAttackKind.CutinAirAttack:
						return "空母CI";
					case DayAttackKind.SpecialNelson:
						return "Nelson Touch";
					case DayAttackKind.Shelling:
						return "炮击";
					case DayAttackKind.AirAttack:
						return "空袭";
					case DayAttackKind.DepthCharge:
						return "爆雷攻击";
					case DayAttackKind.Torpedo:
						return "雷击";
					case DayAttackKind.Rocket:
						return "火箭炮炮击";
					case DayAttackKind.LandingDaihatsu:
						return "登陆攻击（大发动艇）";
					case DayAttackKind.LandingTokuDaihatsu:
						return "登陆攻击（特大发动艇）";
					case DayAttackKind.LandingDaihatsuTank:
						return "登陆攻击（大发战车）";
					case DayAttackKind.LandingAmphibious:
						return "登陆攻击（内火艇）";
					case DayAttackKind.LandingTokuDaihatsuTank:
						return "登陆攻击（特大发战车）";
					default:
						return "不明";
				}
			case "en":
				switch (id) {
					case DayAttackKind.NormalAttack:
						return "Normal Attack";
					case DayAttackKind.Laser:
						return "Laser Attack";
					case DayAttackKind.DoubleShelling:
						return "Double Attack";
					case DayAttackKind.CutinMainSub:
						return "Cut-in(Main/Sub)";
					case DayAttackKind.CutinMainRadar:
						return "Cut-in(Main/Radar)";
					case DayAttackKind.CutinMainAP:
						return "Cut-in(Main/AP Shell)";
					case DayAttackKind.CutinMainMain:
						return "Cut-in(Main/Main)";
					case DayAttackKind.CutinAirAttack:
						return "Aircraft Carrier Cut-in";
					case DayAttackKind.SpecialNelson:
						return "Nelson Touch";
					case DayAttackKind.Shelling:
						return "Shelling";
					case DayAttackKind.AirAttack:
						return "Aerial Attack";
					case DayAttackKind.DepthCharge:
						return "Depth Charge Attack";
					case DayAttackKind.Torpedo:
						return "Torpedo Attack";
					case DayAttackKind.Rocket:
						return "Rocket Attack";
					case DayAttackKind.LandingDaihatsu:
						return "Landing Attack(Daihatsu)";
					case DayAttackKind.LandingTokuDaihatsu:
						return "Landing Attack(Toku Daihatsu)";
					case DayAttackKind.LandingDaihatsuTank:
						return "Landing Attack(Daihatsu w/ Tank)";
					case DayAttackKind.LandingAmphibious:
						return "Landing Attack(Amphibious Tank)";
					case DayAttackKind.LandingTokuDaihatsuTank:
						return "Landing Attack(Toku Daihatsu w/ Tank)";
					default:
						return "Unknown";
				}
			}
		}


		/// <summary>
		/// 夜戦攻撃種別を表す文字列を取得します。
		/// </summary>
		public static string GetNightAttackKind(NightAttackKind id)
		{
			switch (UILanguage) {
			default:
			switch (id)
			{
				case NightAttackKind.NormalAttack:
					return "通常攻撃";
				case NightAttackKind.DoubleShelling:
					return "連続射撃";
				case NightAttackKind.CutinMainTorpedo:
					return "カットイン(主砲/魚雷)";
				case NightAttackKind.CutinTorpedoTorpedo:
					return "カットイン(魚雷x2)";
				case NightAttackKind.CutinMainSub:
					return "カットイン(主砲x2/副砲)";
				case NightAttackKind.CutinMainMain:
					return "カットイン(主砲x3)";
				case NightAttackKind.CutinAirAttack:
					return "空母カットイン";
				case NightAttackKind.CutinTorpedoRadar:
					return "駆逐カットイン(主砲/魚雷/電探)";
				case NightAttackKind.CutinTorpedoPicket:
					return "駆逐カットイン(魚雷/見張員/電探)";
				case NightAttackKind.SpecialNelson:
					return "Nelson Touch";
				case NightAttackKind.Shelling:
					return "砲撃";
				case NightAttackKind.AirAttack:
					return "空撃";
				case NightAttackKind.DepthCharge:
					return "爆雷攻撃";
				case NightAttackKind.Torpedo:
					return "雷撃";
				case NightAttackKind.Rocket:
					return "ロケット砲撃";
				case NightAttackKind.LandingDaihatsu:
					return "揚陸攻撃(大発動艇)";
				case NightAttackKind.LandingTokuDaihatsu:
					return "揚陸攻撃(特大発動艇)";
				case NightAttackKind.LandingDaihatsuTank:
					return "揚陸攻撃(大発戦車)";
				case NightAttackKind.LandingAmphibious:
					return "揚陸攻撃(内火艇)";
				case NightAttackKind.LandingTokuDaihatsuTank:
					return "揚陸攻撃(特大発戦車)";
				default:
					return "不明";
			}
			case "zh":
				switch (id) {
					case NightAttackKind.NormalAttack:
						return "普通攻击";
					case NightAttackKind.DoubleShelling:
						return "连续射击";
					case NightAttackKind.CutinMainTorpedo:
						return "CI（主炮 / 鱼雷）";
					case NightAttackKind.CutinTorpedoTorpedo:
						return "CI（鱼雷 x 2）";
					case NightAttackKind.CutinMainSub:
						return "CI（主炮 x 2 / 副炮）";
					case NightAttackKind.CutinMainMain:
						return "CI（主炮 x 3）";
					case NightAttackKind.CutinAirAttack:
						return "空母CI";
					case NightAttackKind.CutinTorpedoRadar:
						return "驱逐CI（主炮 / 鱼雷 / 电探）";
					case NightAttackKind.CutinTorpedoPicket:
						return "驱逐CI（鱼雷 / 见张员 / 电探测）";
					case NightAttackKind.SpecialNelson:
						return "Nelson Touch";
					case NightAttackKind.Shelling:
						return "炮击";
					case NightAttackKind.AirAttack:
						return "空袭";
					case NightAttackKind.DepthCharge:
						return "爆雷攻击";
					case NightAttackKind.Torpedo:
						return "雷击";
					case NightAttackKind.Rocket:
						return "火箭炮炮击";
					case NightAttackKind.LandingDaihatsu:
						return "登陆攻击（大发动艇）";
					case NightAttackKind.LandingTokuDaihatsu:
						return "登陆攻击（特大发动艇）";
					case NightAttackKind.LandingDaihatsuTank:
						return "登陆攻击（大发战车）";
					case NightAttackKind.LandingAmphibious:
						return "登陆攻击（内火艇）";
					case NightAttackKind.LandingTokuDaihatsuTank:
						return "登陆攻击（特大发战车）";
					default:
						return "不明";
				}
			case "en":
				switch (id) {
					case NightAttackKind.NormalAttack:
						return "Normal Attack";
					case NightAttackKind.DoubleShelling:
						return "Double Attack";
					case NightAttackKind.CutinMainTorpedo:
						return "Cut-in(Main/Torpedo)";
					case NightAttackKind.CutinTorpedoTorpedo:
						return "Cut-in(Torpedo x2)";
					case NightAttackKind.CutinMainSub:
						return "Cut-in(Main x2/Sub)";
					case NightAttackKind.CutinMainMain:
						return "Cut-in(Main x3)";
					case NightAttackKind.CutinAirAttack:
						return "Aircraft Carrier Cut-in";
					case NightAttackKind.CutinTorpedoRadar:
						return "Destroyer Cut-in(Main/Torpedo/Radar)";
					case NightAttackKind.CutinTorpedoPicket:
						return "Destroyer Cut-in(Torpedo/Lookouts/Radar)";
					case NightAttackKind.SpecialNelson:
						return "Nelson Touch";
					case NightAttackKind.Shelling:
						return "Shelling";
					case NightAttackKind.AirAttack:
						return "Aerial Attack";
					case NightAttackKind.DepthCharge:
						return "Depth Charge Attack";
					case NightAttackKind.Torpedo:
						return "Torpedo Attack";
					case NightAttackKind.Rocket:
						return "Rocket Attack";
					case NightAttackKind.LandingDaihatsu:
						return "Landing Attack(Daihatsu)";
					case NightAttackKind.LandingTokuDaihatsu:
						return "Landing Attack(Toku Daihatsu)";
					case NightAttackKind.LandingDaihatsuTank:
						return "Landing Attack(Daihatsu w/ Tank)";
					case NightAttackKind.LandingAmphibious:
						return "Landing Attack(Amphibious Tank)";
					case NightAttackKind.LandingTokuDaihatsuTank:
						return "Landing Attack(Toku Daihatsu w/ Tank)";
					default:
						return "Unknown";
				}
			}
		}


		/// <summary>
		/// 対空カットイン種別を表す文字列を取得します。
		/// </summary>
		public static string GetAACutinKind(int id)
		{
			switch (UILanguage) {
			default:
			switch (id)
			{
				case 0:
					return "なし";
				case 1:
					return "高角砲x2/電探<秋月>";
				case 2:
					return "高角砲/電探<秋月>";
				case 3:
					return "高角砲x2<秋月>";
				case 4:
					return "大口径主砲/三式弾/高射装置/電探";
				case 5:
					return "高角砲+高射装置x2/電探";
				case 6:
					return "大口径主砲/三式弾/高射装置";
				case 7:
					return "高角砲/高射装置/電探";
				case 8:
					return "高角砲+高射装置/電探";
				case 9:
					return "高角砲/高射装置";
				case 10:
					return "高角砲/集中機銃/電探<摩耶>";
				case 11:
					return "高角砲/集中機銃<摩耶>";
				case 12:
					return "集中機銃/機銃/電探";
				case 14:
					return "高角砲/機銃/電探<五十鈴>";
				case 15:
					return "高角砲/機銃<五十鈴>";
				case 16:
					return "高角砲/機銃/電探<霞>";
				case 17:
					return "高角砲/機銃<霞>";
				case 18:
					return "集中機銃<皐月>";
				case 19:
					return "高角砲(非高射装置)/集中機銃<鬼怒>";
				case 20:
					return "集中機銃<鬼怒>";
				case 21:
					return "高角砲/電探<由良>";
				case 22:
					return "集中機銃<文月>";
				case 23:
					return "機銃(非集中)<UIT-25>";
				case 24:
					return "高角砲/機銃(非集中)<龍田>";
				case 25:
					return "噴進砲改二/電探/三式弾<伊勢>";
				case 26:
					return "高角砲+増設機銃/電探<武蔵>";
				case 28:
					return "噴進砲改二/電探<伊勢>";
				case 29:
					return "高角砲/電探<浜風>";
				default:
					return "不明";
			}
			case "zh":
				switch (id) {
					case 0:
						return "无";
					case 1:
						return $"高角炮 x 2 / 电探 < {GetTranslatedShipName("秋月")} >";
					case 2:
						return $"高角炮 / 电探 < {GetTranslatedShipName("秋月")} >";
					case 3:
						return $"高角炮 x 2 < {GetTranslatedShipName("秋月")} >";
					case 4:
						return "大口径主炮 / 三式弹 / 高射装置 / 电探";
					case 5:
						return "高角炮+高射装置 x 2 / 电探";
					case 6:
						return "大口径主炮 / 三式弹 / 高射装置";
					case 7:
						return "高角炮 / 高射装置 / 电探";
					case 8:
						return "高角炮+高射装置 / 电探";
					case 9:
						return "高角炮 / 高射装置";
					case 10:
						return $"高角炮 / 集中机枪 / 电探 < {GetTranslatedShipName("摩耶")} >";
					case 11:
						return $"高角炮 / 集中机枪 < {GetTranslatedShipName("摩耶")} >";
					case 12:
						return "集中机枪 / 机枪 / 电探";
					case 14:
						return $"高角炮 / 机枪 / 电探 < {GetTranslatedShipName("五十鈴")} >";
					case 15:
						return $"高角炮 / 机枪 < {GetTranslatedShipName("五十鈴")} >";
					case 16:
						return $"高角炮 / 机枪 / 电探 < {GetTranslatedShipName("霞")} >";
					case 17:
						return $"高角炮 / 机枪 < {GetTranslatedShipName("霞")} >";
					case 18:
						return $"集中机枪 < {GetTranslatedShipName("皐月")} >";
					case 19:
						return $"高角炮（无高射装置）/ 集中机枪 < {GetTranslatedShipName("鬼怒")} >";
					case 20:
						return $"集中机枪 < {GetTranslatedShipName("鬼怒")} >";
					case 21:
						return $"高角炮 / 电探 < {GetTranslatedShipName("由良")} >";
					case 22:
						return $"集中机枪 < {GetTranslatedShipName("文月")} >";
					case 23:
						return $"机枪（非集中）< {GetTranslatedShipName("UIT-25")} >";
					case 24:
						return $"高角炮 / 机枪（非集中）< {GetTranslatedShipName("龍田")} >";
					case 25:
						return $"喷进炮改二 / 电探 / 三式弹 < {GetTranslatedShipName("伊勢")} >";
					case 26:
						return $"高角炮+增设机枪 / 电探 < {GetTranslatedShipName("武蔵")} >";
					case 28:
						return $"喷进炮改二 / 电探 < {GetTranslatedShipName("伊勢")} >";
					case 29:
						return $"高角炮 / 电探 < {GetTranslatedShipName("浜風")} >";
					default:
						return "不明";
				}
			case "en":
				switch (id) {
					case 0:
						return "N/A";
					case 1:
						return $"High-angle Gun x2 / Radar <{GetTranslatedShipName("秋月")}>";
					case 2:
						return $"High-angle Gun / Radar <{GetTranslatedShipName("秋月")}>";
					case 3:
						return $"High-angle Gun x2 <{GetTranslatedShipName("秋月")}>";
					case 4:
						return "Large Caliber Gun / Type 3 Shell / Anti-Aircraft Fire Director / Radar";
					case 5:
						return "High-angle Gun (w/ AAFD) x2 / Radar";
					case 6:
						return "Large Caliber Gun / Type 3 Shell / Anti-Aircraft Fire Director";
					case 7:
						return "High-angle Gun / Anti-Aircraft Fire Director / Radar";
					case 8:
						return "High-angle Gun (w/ AAFD) / Radar";
					case 9:
						return "High-angle Gun / Anti-Aircraft Fire Director";
					case 10:
						return $"High-angle Gun / Anti-Aircraft Gun (Concentrated) / Radar <{GetTranslatedShipName("摩耶")}>";
					case 11:
						return $"High-angle Gun / Anti-Aircraft Gun (Concentrated) <{GetTranslatedShipName("摩耶")}>";
					case 12:
						return "Anti-Aircraft Gun (Concentrated) / Anti-Aircraft Gun / Radar";
					case 14:
						return $"High-angle Gun / Anti-Aircraft Gun / Radar <{GetTranslatedShipName("五十鈴")}>";
					case 15:
						return $"High-angle Gun / Anti-Aircraft Gun <{GetTranslatedShipName("五十鈴")}>";
					case 16:
						return $"High-angle Gun / Anti-Aircraft Gun / Radar <{GetTranslatedShipName("霞")}>";
					case 17:
						return $"High-angle Gun / Anti-Aircraft Gun <{GetTranslatedShipName("霞")}>";
					case 18:
						return $"Anti-Aircraft Gun (Concentrated) <{GetTranslatedShipName("皐月")}>";
					case 19:
						return $"High-angle Gunb (w/o AAFD) / Anti-Aircraft Gun (Concentrated) <{GetTranslatedShipName("鬼怒")}>";
					case 20:
						return $"Anti-Aircraft Gun (Concentrated) <{GetTranslatedShipName("鬼怒")}>";
					case 21:
						return $"High-angle Gun / Radar <{GetTranslatedShipName("由良")}>";
					case 22:
						return $"Anti-Aircraft Gun (Concentrated) <{GetTranslatedShipName("文月")}>";
					case 23:
						return $"Anti-Aircraft Gun (Not Concentrated) <{GetTranslatedShipName("UIT-25")}>";
					case 24:
						return $"High-angle Gun / Anti-Aircraft Gun (Not Concentrated) <{GetTranslatedShipName("龍田")}>";
					case 25:
						return $"12cm 30-tube Rocket Launcher Kai Ni / Radar / Type 3 Shell <{GetTranslatedShipName("伊勢")}>";
					case 26:
						return $"High-angle Gun (w/ Additional Machine Guns) / Radar <{GetTranslatedShipName("武蔵")}>";
					case 28:
						return $"12cm 30-tube Rocket Launcher Kai Ni / Radar <{GetTranslatedShipName("伊勢")}>";
					case 29:
						return $"High-angle Gun / Radar <{GetTranslatedShipName("浜風")}>";
					default:
						return "Unknown";
				}
			}
		}


		/// <summary>
		/// 勝利ランクを表すIDを取得します。
		/// </summary>
		public static int GetWinRank(string rank)
		{
			switch (rank.ToUpper())
			{
				case "E":
					return 1;
				case "D":
					return 2;
				case "C":
					return 3;
				case "B":
					return 4;
				case "A":
					return 5;
				case "S":
					return 6;
				case "SS":
					return 7;
				default:
					return 0;
			}
		}

		/// <summary>
		/// 勝利ランクを表す文字列を取得します。
		/// </summary>
		public static string GetWinRank(int rank)
		{
			switch (rank)
			{
				case 1:
					return "E";
				case 2:
					return "D";
				case 3:
					return "C";
				case 4:
					return "B";
				case 5:
					return "A";
				case 6:
					return "S";
				case 7:
					return "SS";
				default:
					return "不明";
			}
		}

		#endregion


		#region その他

		/// <summary>
		/// 資源の名前を取得します。
		/// </summary>
		/// <param name="materialID">資源のID。</param>
		/// <returns>資源の名前。</returns>
		public static string GetMaterialName(int materialID)
		{

			switch (materialID)
			{
				case 1:
					return "燃料";
				case 2:
					return "弾薬";
				case 3:
					return "鋼材";
				case 4:
					return "ボーキサイト";
				case 5:
					return "高速建造材";
				case 6:
					return "高速修復材";
				case 7:
					return "開発資材";
				case 8:
					return "改修資材";
				default:
					return "不明";
			}
		}


		/// <summary>
		/// 階級を表す文字列を取得します。
		/// </summary>
		public static string GetAdmiralRank(int id)
		{
			switch (id)
			{
				case 1:
					return "元帥";
				case 2:
					return "大将";
				case 3:
					return "中将";
				case 4:
					return "少将";
				case 5:
					return "大佐";
				case 6:
					return "中佐";
				case 7:
					return "新米中佐";
				case 8:
					return "少佐";
				case 9:
					return "中堅少佐";
				case 10:
					return "新米少佐";
				default:
					return "提督";
			}
		}


		/// <summary>
		/// 任務の発生タイプを表す文字列を取得します。
		/// </summary>
		public static string GetQuestType(int id)
		{
			switch (id)
			{
				case 1:     //デイリー
					return "日";
				case 2:     //ウィークリー
					return "週";
				case 3:     //マンスリー
					return "月";
				case 4:     //単発
					return "単";
				case 5:     //その他(輸送5/空母3)
					return "他";
				default:
					return "?";
			}

		}


		/// <summary>
		/// 任務のカテゴリを表す文字列を取得します。
		/// </summary>
		public static string GetQuestCategory(int id)
		{
			switch (id)
			{
				case 1:
					return "編成";
				case 2:
					return "出撃";
				case 3:
					return "演習";
				case 4:
					return "遠征";
				case 5:
					return "補給";        //入渠も含むが、文字数の関係
				case 6:
					return "工廠";
				case 7:
					return "改装";
				case 8:
					return "出撃";
				case 9:
					return "他";
				default:
					return "不明";
			}
		}


		/// <summary>
		/// 遠征の結果を表す文字列を取得します。
		/// </summary>
		public static string GetExpeditionResult(int value)
		{
			switch (value)
			{
				case 0:
					return "失敗";
				case 1:
					return "成功";
				case 2:
					return "大成功";
				default:
					return "不明";
			}
		}


		/// <summary>
		/// 連合艦隊の編成名を表す文字列を取得します。
		/// </summary>
		public static string GetCombinedFleet(int value)
		{
			switch (value)
			{
				case 0:
					return "通常艦隊";
				case 1:
					return "機動部隊";
				case 2:
					return "水上部隊";
				case 3:
					return "輸送部隊";
				default:
					return "不明";
			}
		}

		#endregion

	}

}

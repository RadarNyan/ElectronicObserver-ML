using Codeplex.Data;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ElectronicObserver.Utility
{
	class UIColorScheme
	{
		public static UIColorScheme Instance { get; } = new UIColorScheme();

		public static UIColors Colors { get; private set; }

		private static dynamic ColorSchemeJson;

		public void Load(string UILanguage)
		{
			string ColorSchemeFilePath = Configuration.Config.UI.ColorSchemeFilePath;
			try {
				StringBuilder sb = new StringBuilder();
				using (StreamReader sr = File.OpenText(ColorSchemeFilePath)) {
					string s = "";
					while ((s = sr.ReadLine()) != null) {
						// Remove Comments as it's not part of json standard
						s = Regex.Replace(s, @"\/\/.*?$", string.Empty);
						if (!string.IsNullOrWhiteSpace(s))
							sb.Append(s);
					}
				}
				var json = DynamicJson.Parse(sb.ToString());
				int schemeID = Configuration.Config.UI.ColorSchemeID;
				if (json.IsDefined(schemeID) && json[schemeID].IsDefined("name")) {
					ColorSchemeJson = json[schemeID];
				} else {
					switch (UILanguage) {
						case "zh":
							Logger.Add(2, $"配色文件 {ColorSchemeFilePath} 中不存在 {schemeID} 号配色，将读取 0 号配色。");
							break;
						case "en":
							Logger.Add(2, $"Color scheme file: {ColorSchemeFilePath} doesn't contain scheme #{schemeID}, scheme #0 will be used.");
							break;
						default:
							Logger.Add(2, $"カラースキームファイル {ColorSchemeFilePath} に {schemeID} 番スキームを含まれません、0 番を読み込みます。");
							break;
					}
					ColorSchemeJson = json[0];
				}
			}
			catch (FileNotFoundException) {
				switch (UILanguage) {
					case "zh":
						Logger.Add(2, $"配色文件 {ColorSchemeFilePath} 不存在。");
						break;
					case "en":
						Logger.Add(2, $"Can't find color scheme file: {ColorSchemeFilePath}");
						break;
					default:
						Logger.Add(2, $"カラースキームファイル {ColorSchemeFilePath} 見つかりませんでした。");
						break;
				}
			}
			catch (Exception ex) {
				switch (UILanguage) {
					case "zh":
						ErrorReporter.SendErrorReport(ex, $"解析配色文件 {ColorSchemeFilePath} 失败。");
						break;
					case "en":
						ErrorReporter.SendErrorReport(ex, $"Faild to parse: {ColorSchemeFilePath}");
						break;
					default:
						ErrorReporter.SendErrorReport(ex, $"カラースキームファイル {ColorSchemeFilePath} の読み込みに失敗しました。");
						break;
				}
			}
			// Built-in Theme
			if (ColorSchemeJson == null || !ColorSchemeJson.IsDefined("name")) {
				#region Built-in Theme
				ColorSchemeJson = DynamicJson.Parse(@"{
""name"":""VS2012Light (Built-in)"",
""basicColors"":{
""red"":""#FF0000"",
""orange"":""#FFA500"",
""yellow"":""#FFFF00"",
""green"":""#00FF00"",
""cyan"":""#00FFFF"",
""blue"":""#0000FF"",
""magenta"":""#FF00FF"",
""violet"":""#EE82EE""
},
""barColors"":[[
""#FF0000"",
""#FF0000"",
""#FF8800"",
""#FF8800"",
""#FFCC00"",
""#FFCC00"",
""#00CC00"",
""#00CC00"",
""#0044CC"",
""#44FF00"",
""#882222"",
""#888888""
],[
""#FF0000"",
""#FF0000"",
""#FF4400"",
""#FF8800"",
""#FFAA00"",
""#EEEE00"",
""#CCEE00"",
""#00CC00"",
""#0044CC"",
""#00FF44"",
""#882222"",
""#888888""
]],
""panelColors"":{
""foreground"":""#000000"",
""background"":""#F0F0F0"",
""foreground2"":""#888888"",
""background2"":""#E3E3E3"",
""statusBarFG"":""#000000"",
""statusBarBG"":""#E3E3E3"",
""skin"":{
""panelSplitter"":""#E3E3E3"",
""docTabBarFG"":""#000000"",
""docTabBarBG"":""#F0F0F0"",
""docTabActiveFG"":""#FFFFFF"",
""docTabActiveBG"":""#007ACC"",
""docTabActiveLostFocusFG"":""#6D6D6D"",
""docTabActiveLostFocusBG"":""#CCCEDB"",
""docTabInactiveHoverFG"":""#FFFFFF"",
""docTabInactiveHoverBG"":""#1C97EA"",
""docBtnActiveHoverFG"":""#FFFFFF"",
""docBtnActiveHoverBG"":""#1C97EA"",
""docBtnActiveLostFocusHoverFG"":""#717171"",
""docBtnActiveLostFocusHoverBG"":""#E6E7ED"",
""docBtnInactiveHoverFG"":""#FFFFFF"",
""docBtnInactiveHoverBG"":""#52B0EF"",
""toolTabBarFG"":""#6D6D6D"",
""toolTabBarBG"":""#F0F0F0"",
""toolTabActive"":""#007ACC"",
""toolTitleActiveFG"":""#FFFFFF"",
""toolTitleActiveBG"":""#007ACC"",
""toolTitleLostFocusFG"":""#6D6D6D"",
""toolTitleLostFocusBG"":""#F0F0F0"",
""toolTitleDotActive"":""#50AADC"",
""toolTitleDotLostFocus"":""#A0A0A0"",
""autoHideTabBarFG"":""#E3E3E3"",
""autoHideTabBarBG"":""#F0F0F0"",
""autoHideTabActive"":""#007ACC"",
""autoHideTabInactive"":""#6D6D6D""
},
""fleet"":{
""repairTimerText"":""#888888"",
""conditionText"":""#000000"",
""conditionVeryTired"":""#F08080"",
""conditionTired"":""#FFA07A"",
""conditionLittleTired"":""#FFE4B5"",
""conditionSparkle"":""#90EE90"",
""equipmentLevel"":""#006666""
},
""fleetOverview"":{
""shipDamagedFG"":""#000000"",
""shipDamagedBG"":""#F08080"",
""expeditionOverFG"":""#000000"",
""expeditionOverBG"":""#90EE90"",
""tiredRecoveredFG"":""#000000"",
""tiredRecoveredBG"":""#90EE90"",
""alertNotInExpeditionFG"":""#000000"",
""alertNotInExpeditionBG"":""#90EE90""
},
""dock"":{
""repairFinishedFG"":""#000000"",
""repairFinishedBG"":""#90EE90""
},
""arsenal"":{
""buildCompleteFG"":""#000000"",
""buildCompleteBG"":""#90EE90""
},
""hq"":{
""resOverFG"":""#000000"",
""resOverBG"":""#FFE4B5"",
""shipOverFG"":""#000000"",
""shipOverBG"":""#F08080"",
""materialMaxFG"":""#000000"",
""materialMaxBG"":""#F08080"",
""coinMaxFG"":""#000000"",
""coinMaxBG"":""#F08080"",
""resLowFG"":""#000000"",
""resLowBG"":""#F08080"",
""resMaxFG"":""#000000"",
""resMaxBG"":""#F08080""
},
""quest"":{
""typeFG"":""#000000"",
""typeHensei"":""#AAFFAA"",
""typeShutsugeki"":""#FFCCCC"",
""typeEnshu"":""#DDFFAA"",
""typeEnsei"":""#DDFFAA"",
""typeHokyu"":""#CCFFFF"",
""typeKojo"":""#DDCCBB"",
""typeKaiso"":""#DDCCFF"",
""processLT50"":""#FF8800"",
""processLT80"":""#00CC00"",
""processLT100"":""#008800"",
""processDefault"":""#0088FF""
},
""compass"":{
""shipClass2"":""#FF0000"",
""shipClass3"":""#FF8800"",
""shipClass4"":""#006600"",
""shipClass5"":""#880000"",
""shipClass6"":""#0088FF"",
""shipClass7"":""#0000FF"",
""shipDestroyed"":""#FF00FF"",
""eventKind3"":""#000080"",
""eventKind6"":""#006400"",
""eventKind5"":""#8B0000""
},
""battle"":{
""barMVP"":""#FFE4B5"",
""textMVP"":""#000000"",
""textMVP2"":""#888888"",
""barEscaped"":""#C0C0C0"",
""textEscaped"":""#000000"",
""textEscaped2"":""#888888"",
""barBossDamaged"":""#FFE4E1"",
""textBossDamaged"":""#000000"",
""textBossDamaged2"":""#888888""
}}}");
				#endregion
			}
			// Load Colors from ColorSchemeJson
			Colors = new UIColors();
			if (Configuration.Config.UI.BarColorMorphing) {
				Configuration.Config.UI.BarColorScheme = Colors.BarColors;
			} else {
				Configuration.Config.UI.BarColorScheme = Colors.BarColorsMorphed;
			}
			switch (UILanguage) {
				case "zh":
					Logger.Add(2, $"已载入配色：{ColorSchemeJson["name"]}");
					break;
				case "en":
					Logger.Add(2, $"Color Scheme: {ColorSchemeJson["name"]} loaded.");
					break;
				default:
					Logger.Add(2, $"カラースキーム：{ColorSchemeJson["name"]} を読み込みました。");
					break;
			}
		}

		public class UIColors
		{
			#region Define Colors
			// Base Colors
			public Color Red { get; private set; }
			public Color Orange { get; private set; }
			public Color Yellow { get; private set; }
			public Color Green { get; private set; }
			public Color Cyan { get; private set; }
			public Color Blue { get; private set; }
			public Color Magenta { get; private set; }
			public Color Violet { get; private set; }
			// Panel Colors
			public Color MainFG { get; private set; }
			public Color MainBG { get; private set; }
			public Color SubFG { get; private set; }
			public Color SubBG { get; private set; }
			public Pen SubBGPen { get; private set; }
			// Bar Colors
			public Color[] BarColors;
			public Color[] BarColorsMorphed;
			// StatusBar Colors
			public Color StatusBarFG { get; private set; }
			public Color StatusBarBG { get; private set; }
			// DockPanelSuite.Styles
			public string[] DockPanelSuiteStyles { get; private set; }
			// FormFleet
			public Color Fleet_ConditionFG { get; private set; }
			public Color Fleet_ConditionBGVeryTired { get; private set; }
			public Color Fleet_ConditionBGTired { get; private set; }
			public Color Fleet_ConditionBGLittleTired { get; private set; }
			public Color Fleet_ConditionBGSparkle { get; private set; }
			public Color Fleet_RepairTimerText { get; private set; } // ShipStatusHP
			public Color Fleet_EquipmentLevel { get; private set; } // ShipStatusEquipment
			// FormFleetOverView
			public Color FleetOverview_ShipDamagedFG { get; private set; }
			public Color FleetOverview_ShipDamagedBG { get; private set; }
			public Color FleetOverview_ExpeditionOverFG { get; private set; }
			public Color FleetOverview_ExpeditionOverBG { get; private set; }
			public Color FleetOverview_TiredRecoveredFG { get; private set; }
			public Color FleetOverview_TiredRecoveredBG { get; private set; }
			public Color FleetOverview_AlertNotInExpeditionFG { get; private set; }
			public Color FleetOverview_AlertNotInExpeditionBG { get; private set; }
			// FormDock
			public Color Dock_RepairFinishedFG { get; private set; }
			public Color Dock_RepairFinishedBG { get; private set; }
			// FormArsenal
			public Color Arsenal_BuildCompleteFG { get; private set; }
			public Color Arsenal_BuildCompleteBG { get; private set; }
			// FormHeadquarters
			public Color Headquarters_ShipCountOverFG { get; private set; }
			public Color Headquarters_ShipCountOverBG { get; private set; }
			public Color Headquarters_ResourceOverFG { get; private set; }
			public Color Headquarters_ResourceOverBG { get; private set; }
			public Color Headquarters_MaterialMaxFG { get; private set; }
			public Color Headquarters_MaterialMaxBG { get; private set; }
			public Color Headquarters_CoinMaxFG { get; private set; }
			public Color Headquarters_CoinMaxBG { get; private set; }
			public Color Headquarters_ResourceMaxFG { get; private set; }
			public Color Headquarters_ResourceMaxBG { get; private set; }
			public Color Headquarters_ResourceLowFG { get; private set; }
			public Color Headquarters_ResourceLowBG { get; private set; }
			// FormQuest
			public Color Quest_TypeFG { get; private set; }
			public Color Quest_Type1BG { get; private set; }
			public Color Quest_Type2BG { get; private set; }
			public Color Quest_Type3BG { get; private set; }
			public Color Quest_Type4BG { get; private set; }
			public Color Quest_Type5BG { get; private set; }
			public Color Quest_Type6BG { get; private set; }
			public Color Quest_Type7BG { get; private set; }
			public Color Quest_ProgressLT50 { get; private set; }
			public Color Quest_ProgressLT80 { get; private set; }
			public Color Quest_ProgressLT100 { get; private set; }
			public Color Quest_ProgressDefault { get; private set; }
			// FormCompass
			public Color Compass_ShipName2 { get; private set; }
			public Color Compass_ShipName3 { get; private set; }
			public Color Compass_ShipName4 { get; private set; }
			public Color Compass_ShipName5 { get; private set; }
			public Color Compass_ShipName6 { get; private set; }
			public Color Compass_ShipName7 { get; private set; }
			public Color Compass_ShipNameDestroyed { get; private set; }
			public Color Compass_TextEventKind3 { get; private set; }
			public Color Compass_TextEventKind6 { get; private set; }
			public Color Compass_TextEventKind5 { get; private set; }
			public SolidBrush Compass_OverlayBrush { get; private set; } // ShipStatusEquipment
			// FormBattle
			public Color Battle_HPBarsMVP { get; private set; }
			public Color Battle_TextMVP { get; private set; }
			public Color Battle_TextMVP2 { get; private set; }
			public Color Battle_HPBarsEscaped { get; private set; }
			public Color Battle_TextEscaped { get; private set; }
			public Color Battle_TextEscaped2 { get; private set; }
			public Color Battle_HPBarsBossDamaged { get; private set; }
			public Color Battle_TextBossDamaged { get; private set; }
			public Color Battle_TextBossDamaged2 { get; private set; }
			#endregion

			public UIColors()
			{
				// Base Colors
				Red = ThemeColor("basicColors", "red");
				Orange = ThemeColor("basicColors", "orange");
				Yellow = ThemeColor("basicColors", "yellow");
				Green = ThemeColor("basicColors", "green");
				Cyan = ThemeColor("basicColors", "cyan");
				Blue = ThemeColor("basicColors", "blue");
				Magenta = ThemeColor("basicColors", "magenta");
				Violet = ThemeColor("basicColors", "violet");
				// Panel Colors
				MainFG = ThemeColor("panelColors", "foreground");
				MainBG = ThemeColor("panelColors", "background");
				SubFG = ThemeColor("panelColors", "foreground2");
				SubBG = ThemeColor("panelColors", "background2");
				SubBGPen = new Pen(SubBG);
				// Bar Colors
				BarColors = new Color[] {
					ThemeBarColor(0, 0),
					ThemeBarColor(0, 1),
					ThemeBarColor(0, 2),
					ThemeBarColor(0, 3),
					ThemeBarColor(0, 4),
					ThemeBarColor(0, 5),
					ThemeBarColor(0, 6),
					ThemeBarColor(0, 7),
					ThemeBarColor(0, 8),
					ThemeBarColor(0, 9),
					ThemeBarColor(0, 10),
					ThemeBarColor(0, 11)
				};
				BarColorsMorphed = new Color[] {
					ThemeBarColor(1, 0),
					ThemeBarColor(1, 1),
					ThemeBarColor(1, 2),
					ThemeBarColor(1, 3),
					ThemeBarColor(1, 4),
					ThemeBarColor(1, 5),
					ThemeBarColor(1, 6),
					ThemeBarColor(1, 7),
					ThemeBarColor(1, 8),
					ThemeBarColor(1, 9),
					ThemeBarColor(1, 10),
					ThemeBarColor(1, 11)
				};
				// StatusBar Colors
				StatusBarFG = ThemeColor("panelColors", "statusBarFG");
				StatusBarBG = ThemeColor("panelColors", "statusBarBG");
				// DockPanelSuite.Styles
				DockPanelSuiteStyles = new string[] {
					ThemePanelColorHex("skin", "panelSplitter"),
					ThemePanelColorHex("skin", "docTabBarFG"),
					ThemePanelColorHex("skin", "docTabBarBG"),
					ThemePanelColorHex("skin", "docTabActiveFG"),
					ThemePanelColorHex("skin", "docTabActiveBG"),
					ThemePanelColorHex("skin", "docTabActiveLostFocusFG"),
					ThemePanelColorHex("skin", "docTabActiveLostFocusBG"),
					ThemePanelColorHex("skin", "docTabInactiveHoverFG"),
					ThemePanelColorHex("skin", "docTabInactiveHoverBG"),
					ThemePanelColorHex("skin", "docBtnActiveHoverFG"),
					ThemePanelColorHex("skin", "docBtnActiveHoverBG"),
					ThemePanelColorHex("skin", "docBtnActiveLostFocusHoverFG"),
					ThemePanelColorHex("skin", "docBtnActiveLostFocusHoverBG"),
					ThemePanelColorHex("skin", "docBtnInactiveHoverFG"),
					ThemePanelColorHex("skin", "docBtnInactiveHoverBG"),
					ThemePanelColorHex("skin", "toolTabBarFG"),
					ThemePanelColorHex("skin", "toolTabBarBG"),
					ThemePanelColorHex("skin", "toolTabActive"),
					ThemePanelColorHex("skin", "toolTitleActiveFG"),
					ThemePanelColorHex("skin", "toolTitleActiveBG"),
					ThemePanelColorHex("skin", "toolTitleLostFocusFG"),
					ThemePanelColorHex("skin", "toolTitleLostFocusBG"),
					ThemePanelColorHex("skin", "toolTitleDotActive"),
					ThemePanelColorHex("skin", "toolTitleDotLostFocus"),
					ThemePanelColorHex("skin", "autoHideTabBarFG"),
					ThemePanelColorHex("skin", "autoHideTabBarBG"),
					ThemePanelColorHex("skin", "autoHideTabActive"),
					ThemePanelColorHex("skin", "autoHideTabInactive")
				};
				// FormFleet
				Fleet_ConditionFG = ThemePanelColor("fleet", "conditionText");
				Fleet_ConditionBGVeryTired = ThemePanelColor("fleet", "conditionVeryTired");
				Fleet_ConditionBGTired = ThemePanelColor("fleet", "conditionTired");
				Fleet_ConditionBGLittleTired = ThemePanelColor("fleet", "conditionLittleTired");
				Fleet_ConditionBGSparkle = ThemePanelColor("fleet", "conditionSparkle");
				Fleet_RepairTimerText = ThemePanelColor("fleet", "repairTimerText");
				Fleet_EquipmentLevel = ThemePanelColor("fleet", "equipmentLevel");
				// FormFleetOverView
				FleetOverview_ShipDamagedFG = ThemePanelColor("fleetOverview", "shipDamagedFG");
				FleetOverview_ShipDamagedBG = ThemePanelColor("fleetOverview", "shipDamagedBG");
				FleetOverview_ExpeditionOverFG = ThemePanelColor("fleetOverview", "expeditionOverFG");
				FleetOverview_ExpeditionOverBG = ThemePanelColor("fleetOverview", "expeditionOverBG");
				FleetOverview_TiredRecoveredFG = ThemePanelColor("fleetOverview", "tiredRecoveredFG");
				FleetOverview_TiredRecoveredBG = ThemePanelColor("fleetOverview", "tiredRecoveredBG");
				FleetOverview_AlertNotInExpeditionFG = ThemePanelColor("fleetOverview", "alertNotInExpeditionFG");
				FleetOverview_AlertNotInExpeditionBG = ThemePanelColor("fleetOverview", "alertNotInExpeditionBG");
				// FormDock
				Dock_RepairFinishedFG = ThemePanelColor("dock", "repairFinishedFG");
				Dock_RepairFinishedBG = ThemePanelColor("dock", "repairFinishedBG");
				// FormArsenal
				Arsenal_BuildCompleteFG = ThemePanelColor("arsenal", "buildCompleteFG");
				Arsenal_BuildCompleteBG = ThemePanelColor("arsenal", "buildCompleteBG");
				// FormHeadquarters
				Headquarters_ShipCountOverFG = ThemePanelColor("hq", "shipOverFG");
				Headquarters_ShipCountOverBG = ThemePanelColor("hq", "shipOverBG");
				Headquarters_ResourceOverFG = ThemePanelColor("hq", "resOverFG");
				Headquarters_ResourceOverBG = ThemePanelColor("hq", "resOverBG");
				Headquarters_MaterialMaxFG = ThemePanelColor("hq", "materialMaxFG");
				Headquarters_MaterialMaxBG = ThemePanelColor("hq", "materialMaxBG");
				Headquarters_CoinMaxFG = ThemePanelColor("hq", "coinMaxFG");
				Headquarters_CoinMaxBG = ThemePanelColor("hq", "coinMaxBG");
				Headquarters_ResourceMaxFG = ThemePanelColor("hq", "resMaxFG");
				Headquarters_ResourceMaxBG = ThemePanelColor("hq", "resMaxBG");
				// -(not implented)-
				Headquarters_ResourceLowFG = ThemePanelColor("hq", "resLowFG");
				Headquarters_ResourceLowBG = ThemePanelColor("hq", "resLowBG");
				// FormQuest
				Quest_TypeFG = ThemePanelColor("quest", "typeFG");
				Quest_Type1BG = ThemePanelColor("quest", "typeHensei");
				Quest_Type2BG = ThemePanelColor("quest", "typeShutsugeki");
				Quest_Type3BG = ThemePanelColor("quest", "typeEnshu");
				Quest_Type4BG = ThemePanelColor("quest", "typeEnsei");
				Quest_Type5BG = ThemePanelColor("quest", "typeHokyu");
				Quest_Type6BG = ThemePanelColor("quest", "typeKojo");
				Quest_Type7BG = ThemePanelColor("quest", "typeKaiso");
				Quest_ProgressLT50 = ThemePanelColor("quest", "processLT50");
				Quest_ProgressLT80 = ThemePanelColor("quest", "processLT80");
				Quest_ProgressLT100 = ThemePanelColor("quest", "processLT100");
				Quest_ProgressDefault = ThemePanelColor("quest", "processDefault");
				// FormCompass
				Compass_ShipName2 = ThemePanelColor("compass", "shipClass2");
				Compass_ShipName3 = ThemePanelColor("compass", "shipClass3");
				Compass_ShipName4 = ThemePanelColor("compass", "shipClass4");
				Compass_ShipName5 = ThemePanelColor("compass", "shipClass5");
				Compass_ShipName6 = ThemePanelColor("compass", "shipClass6");
				Compass_ShipName7 = ThemePanelColor("compass", "shipClass7");
				Compass_ShipNameDestroyed = ThemePanelColor("compass", "shipDestroyed");
				Compass_TextEventKind3 = ThemePanelColor("compass", "eventKind3");
				Compass_TextEventKind6 = ThemePanelColor("compass", "eventKind6");
				Compass_TextEventKind5 = ThemePanelColor("compass", "eventKind5");
				Compass_OverlayBrush = new SolidBrush(ThemePanelColor("compass", "overlayBrush"));
				// FormBattle
				Battle_HPBarsMVP = ThemePanelColor("battle", "barMVP");
				Battle_TextMVP = ThemePanelColor("battle", "textMVP");
				Battle_TextMVP2 = ThemePanelColor("battle", "textMVP2");
				Battle_HPBarsEscaped = ThemePanelColor("battle", "barEscaped");
				Battle_TextEscaped = ThemePanelColor("battle", "textEscaped");
				Battle_TextEscaped2 = ThemePanelColor("battle", "textEscaped2");
				Battle_HPBarsBossDamaged = ThemePanelColor("battle", "barBossDamaged");
				Battle_TextBossDamaged = ThemePanelColor("battle", "textBossDamaged");
				Battle_TextBossDamaged2 = ThemePanelColor("battle", "textBossDamaged2");
			}

			private Color ThemeColor(string type, string name)
			{
				if (ColorSchemeJson.IsDefined(type) && ColorSchemeJson[type].IsDefined(name)) {
					return ColorTranslator.FromHtml(ColorSchemeJson[type][name]);
				} else {
					switch (type + "_" + name) {
						case "basicColors_red":
							return Color.Red;
						case "basicColors_orange":
							return Color.Orange;
						case "basicColors_yellow":
							return Color.Yellow;
						case "basicColors_green":
							return Color.Green;
						case "basicColors.cyan":
							return Color.Cyan;
						case "basicColors.blue":
							return Color.Blue;
						case "basicColors.magenta":
							return Color.Magenta;
						case "basicColors.violet":
							return Color.Violet;
						case "panelColors_foreground":
							return SystemColors.ControlText;
						case "panelColors_background":
							return SystemColors.Control;
						case "panelColors_foreground2":
							return SystemColors.GrayText;
						case "panelColors_background2":
							return SystemColors.ControlLight;
						case "panelColors_statusBarFG":
							return SubFG;
						case "panelColors_statusBarBG":
							return SubBG;
						default:
							return Color.Magenta;
					}
				}
			}

			private string ThemeColorHex(string type, string name)
			{
				if (ColorSchemeJson.IsDefined(type) && ColorSchemeJson[type].IsDefined(name)) {
					return ColorSchemeJson[type][name];
				} else {
					switch (type + "_" + name) {
						case "panelColors_tabActiveFG":
							return ThemeColorHex("panelColors", "foreground2");
						case "panelColors_tabActiveBG":
							return ThemeColorHex("panelColors", "background2");
						case "panelColors_tabLostFocusFG":
							return ThemeColorHex("panelColors", "foreground2");
						case "panelColors_tabLostFocusBG":
							return ThemeColorHex("panelColors", "background2");
						case "panelColors_tabHoverFG":
							return ThemeColorHex("panelColors", "foreground2");
						case "panelColors_tabHoverBG":
							return ThemeColorHex("panelColors", "background2");
						default:
							var c = ThemeColor(type, name);
							return "#" + c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2");
					}
				}
			}

			private Color ThemePanelColor(string form, string name)
			{
				if (ColorSchemeJson.IsDefined("panelColors") && ColorSchemeJson["panelColors"].IsDefined(form) && ColorSchemeJson["panelColors"][form].IsDefined(name)) {
					return ColorTranslator.FromHtml(ColorSchemeJson["panelColors"][form][name]);
				} else {
					switch (form + "_" + name) {
						// 视图 - 舰队
						case "fleet_repairTimerText":
							return SubFG;
						case "fleet_conditionText":
							return MainBG;
						case "fleet_conditionVeryTired":
							return Red;
						case "fleet_conditionTired":
							return Orange;
						case "fleet_conditionLittleTired":
							return Yellow;
						case "fleet_conditionSparkle":
							return Blue;
						case "fleet_equipmentLevel":
							return Cyan;
						// 视图 - 舰队一览
						case "fleetOverview_shipDamagedFG":
							return MainBG;
						case "fleetOverview_shipDamagedBG":
							return Red;
						case "fleetOverview_expeditionOverFG":
							return MainBG;
						case "fleetOverview_expeditionOverBG":
							return Blue;
						case "fleetOverview_tiredRecoveredFG":
							return MainBG;
						case "fleetOverview_tiredRecoveredBG":
							return Blue;
						case "fleetOverview_alertNotInExpeditionFG":
							return MainBG;
						case "fleetOverview_alertNotInExpeditionBG":
							return Blue;
						// 视图 - 司令部
						case "hq_resOverFG":
							return MainFG;
						case "hq_resOverBG":
							return SubBG;
						case "hq_shipOverFG":
							return MainBG;
						case "hq_shipOverBG":
							return Red;
						case "hq_materialMaxFG":
							return MainBG;
						case "hq_materialMaxBG":
							return Blue;
						case "hq_coinMaxFG":
							return MainBG;
						case "hq_coinMaxBG":
							return Blue;
						case "hq_resLowFG":
							return MainBG;
						case "hq_resLowBG":
							return Red;
						case "hq_resMaxFG":
							return MainBG;
						case "hq_resMaxBG":
							return Blue;
						// 视图 - 入渠
						case "dock_repairFinishedFG":
							return MainBG;
						case "dock_repairFinishedBG":
							return Blue;
						// 视图 - 工厂
						case "arsenal_buildCompleteFG":
							return MainBG;
						case "arsenal_buildCompleteBG":
							return Blue;
						// 视图 - 任务
						case "quest_typeFG":
							return MainBG;
						case "quest_typeHensei":
							return Green;
						case "quest_typeShutsugeki":
							return Red;
						case "quest_typeEnshu":
							return Green;
						case "quest_typeEnsei":
							return Cyan;
						case "quest_typeHokyu":
							return Yellow;
						case "quest_typeKojo":
							return Orange;
						case "quest_typeKaiso":
							return Violet;
						case "quest_processLT50":
							return Orange;
						case "quest_processLT80":
							return Green;
						case "quest_processLT100":
							return Cyan;
						case "quest_processDefault":
							return Blue;
						// 视图 - 罗盘
						case "compass_shipClass2":
							return Red;
						case "compass_shipClass3":
							return Orange;
						case "compass_shipClass4":
							return Green;
						case "compass_shipClass5":
							return Violet;
						case "compass_shipClass6":
							return Cyan;
						case "compass_shipClass7":
							return Blue;
						case "compass_shipDestroyed":
							return Magenta;
						case "compass_eventKind3":
							return Violet;
						case "compass_eventKind6":
							return Green;
						case "compass_eventKind5":
							return Red;
						case "compass_overlayBrush": // %75 透明度背景色
							return Color.FromArgb(0xC0, MainBG);
						// 视图 - 战斗
						case "battle_barMVP":
							return Blue;
						case "battle_textMVP":
							return MainBG;
						case "battle_textMVP2":
							return SubBG;
						case "battle_barEscaped":
							return SubBG;
						case "battle_textEscaped":
							return MainFG;
						case "battle_textEscaped2":
							return SubFG;
						case "battle_barBossDamaged":
							return Orange;
						case "battle_textBossDamaged":
							return MainBG;
						case "battle_textBossDamaged2":
							return SubBG;
						// 未定义颜色
						default:
							return Color.Magenta;
					}
				}
			}

			private string ThemePanelColorHex(string form, string name)
			{
				if (ColorSchemeJson.IsDefined("panelColors") && ColorSchemeJson["panelColors"].IsDefined(form) && ColorSchemeJson["panelColors"][form].IsDefined(name)) {
					return ColorSchemeJson["panelColors"][form][name];
				} else {
					switch (form + "_" + name) {
						// 面板分割线
						case "skin_panelSplitter":
							return ThemeColorHex("panelColors", "background2");
						case "skin_docTabBarFG":
							return ThemeColorHex("panelColors", "foreground2");
						case "skin_docTabBarBG":
							return ThemeColorHex("panelColors", "background");
						case "skin_docTabActiveFG":
							return ThemeColorHex("panelColors", "foreground");
						case "skin_docTabActiveBG":
							return ThemeColorHex("panelColors", "background2");
						case "skin_docTabActiveLostFocusFG":
							return ThemeColorHex("panelColors", "foreground");
						case "skin_docTabActiveLostFocusBG":
							return ThemeColorHex("panelColors", "background2");
						case "skin_docTabInactiveHoverFG":
							return ThemeColorHex("panelColors", "foreground");
						case "skin_docTabInactiveHoverBG":
							return ThemeColorHex("panelColors", "background2");
						case "skin_docBtnActiveHoverFG":
							return ThemeColorHex("panelColors", "foreground");
						case "skin_docBtnActiveHoverBG":
							return ThemeColorHex("panelColors", "background2");
						case "skin_docBtnActiveLostFocusHoverFG":
							return ThemeColorHex("panelColors", "foreground");
						case "skin_docBtnActiveLostFocusHoverBG":
							return ThemeColorHex("panelColors", "background2");
						case "skin_docBtnInactiveHoverFG":
							return ThemeColorHex("panelColors", "foreground");
						case "skin_docBtnInactiveHoverBG":
							return ThemeColorHex("panelColors", "background2");
						case "skin_toolTabBarFG":
							return ThemeColorHex("panelColors", "foreground2");
						case "skin_toolTabBarBG":
							return ThemeColorHex("panelColors", "background");
						case "skin_toolTabActive":
							return ThemeColorHex("panelColors", "foreground");
						case "skin_toolTitleActiveFG":
							return ThemeColorHex("panelColors", "foreground");
						case "skin_toolTitleActiveBG":
							return ThemeColorHex("panelColors", "background2");
						case "skin_toolTitleLostFocusFG":
							return ThemeColorHex("panelColors", "foreground2");
						case "skin_toolTitleLostFocusBG":
							return ThemeColorHex("panelColors", "background");
						case "skin_toolTitleDotActive":
							return ThemeColorHex("panelColors", "background");
						case "skin_toolTitleDotLostFocus":
							return ThemeColorHex("panelColors", "background2");
						case "skin_autoHideTabBarFG":
							return ThemeColorHex("panelColors", "background2");
						case "skin_autoHideTabBarBG":
							return ThemeColorHex("panelColors", "background");
						case "skin_autoHideTabActive":
							return ThemeColorHex("panelColors", "foreground");
						case "skin_autoHideTabInactive":
							return ThemeColorHex("panelColors", "foreground2");
						default:
							var c = ThemePanelColor(form, name);
							return "#" + c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2");
					}
				}
			}

			private Color ThemeBarColor(int type, int index)
			{
				if (ColorSchemeJson.IsDefined("barColors") && ColorSchemeJson["barColors"].IsDefined(type) && ColorSchemeJson["barColors"][type].IsDefined(11)) {
					return ColorTranslator.FromHtml(ColorSchemeJson["barColors"][type][index]);
				} else {
					switch (type + "_" + index) {
						case "0_0":
							return Red;
						case "0_1":
							return Red;
						case "0_2":
							return Orange;
						case "0_3":
							return Orange;
						case "0_4":
							return Yellow;
						case "0_5":
							return Yellow;
						case "0_6":
							return Green;
						case "0_7":
							return Green;
						case "0_8":
							return Blue;
						case "0_9":
							return Magenta;
						case "0_10":
							return Magenta;
						case "0_11":
							return SubBG;
						case "1_0":
							return ThemeBarColor(0, 0);
						case "1_1":
							return ThemeBarColor(0, 1);
						case "1_2":
							return ThemeBarColor(0, 2);
						case "1_3":
							return ThemeBarColor(0, 3);
						case "1_4":
							return ThemeBarColor(0, 4);
						case "1_5":
							return ThemeBarColor(0, 5);
						case "1_6":
							return ThemeBarColor(0, 6);
						case "1_7":
							return ThemeBarColor(0, 7);
						case "1_8":
							return ThemeBarColor(0, 8);
						case "1_9":
							return ThemeBarColor(0, 9);
						case "1_10":
							return ThemeBarColor(0, 10);
						case "1_11":
							return ThemeBarColor(0, 11);
						default:
							return Color.Magenta;
					}
				}
			}
		}
	}
}

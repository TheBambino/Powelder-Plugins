using Microsoft.Xna.Framework;
using SurvivalCore.Economy.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Localization;
using TShockAPI;

namespace SurvivalCore
{
	public class SrvCommands
	{
		public static void broadcastXedlefix(TShockAPI.CommandArgs args)
		{
			string str = string.Join(" ", args.Parameters);
			TSPlayer.All.SendMessage("[i:3570] [c/595959:⮘] [c/18B690:Serwer] [c/595959:⮚] [c/18B690:Xedlefix][c/595959::] " + str, byte.MaxValue, byte.MaxValue, byte.MaxValue);
		}

		public static void broadcastIwobos(TShockAPI.CommandArgs args)
		{
			string str = string.Join(" ", args.Parameters);
		}

		public static void Vanish(TShockAPI.CommandArgs args)
		{
			args.TPlayer.active = !args.TPlayer.active;
			SurvivalCore.srvPlayers[args.Player.Index].isVanished = !SurvivalCore.srvPlayers[args.Player.Index].isVanished;
			NetMessage.SendData(14, -1, args.Player.Index, null, args.Player.Index, args.TPlayer.active.GetHashCode());
			if (args.TPlayer.active)
			{
				NetMessage.SendData(4, -1, args.Player.Index, null, args.Player.Index);
				NetMessage.SendData(13, -1, args.Player.Index, null, args.Player.Index);
				for (int i = 0; i < NetItem.MaxInventory; i++)
				{
					if (i < NetItem.InventorySlots)
					{
						int num = i;
						NetMessage.SendData(5, -1, -1, NetworkText.FromLiteral(args.Player.TPlayer.inventory[num].Name), args.Player.Index, i, (int)args.Player.TPlayer.inventory[num].prefix);
						NetMessage.SendData(5, args.Player.Index, -1, NetworkText.FromLiteral(args.Player.TPlayer.inventory[num].Name), args.Player.Index, i, (int)args.Player.TPlayer.inventory[num].prefix);
					}
					else if (i < NetItem.InventorySlots + NetItem.ArmorSlots)
					{
						int num = i - NetItem.InventorySlots;
						NetMessage.SendData(5, -1, -1, NetworkText.FromLiteral(args.Player.TPlayer.armor[num].Name), args.Player.Index, i, (int)args.Player.TPlayer.armor[num].prefix);
						NetMessage.SendData(5, args.Player.Index, -1, NetworkText.FromLiteral(args.Player.TPlayer.armor[num].Name), args.Player.Index, i, (int)args.Player.TPlayer.armor[num].prefix);
					}
					else if (i < NetItem.InventorySlots + NetItem.ArmorSlots + NetItem.DyeSlots)
					{
						int num = i - (NetItem.InventorySlots + NetItem.ArmorSlots);
						NetMessage.SendData(5, -1, -1, NetworkText.FromLiteral(args.Player.TPlayer.dye[num].Name), args.Player.Index, i, (int)args.Player.TPlayer.dye[num].prefix);
						NetMessage.SendData(5, args.Player.Index, -1, NetworkText.FromLiteral(args.Player.TPlayer.dye[num].Name), args.Player.Index, i, (int)args.Player.TPlayer.dye[num].prefix);
					}
					else if (i < NetItem.InventorySlots + NetItem.ArmorSlots + NetItem.DyeSlots + NetItem.MiscEquipSlots)
					{
						int num = i - (NetItem.InventorySlots + NetItem.ArmorSlots + NetItem.DyeSlots);
						NetMessage.SendData(5, -1, -1, NetworkText.FromLiteral(args.Player.TPlayer.miscEquips[num].Name), args.Player.Index, i, (int)args.Player.TPlayer.miscEquips[num].prefix);
						NetMessage.SendData(5, args.Player.Index, -1, NetworkText.FromLiteral(args.Player.TPlayer.miscEquips[num].Name), args.Player.Index, i, (int)args.Player.TPlayer.miscEquips[num].prefix);
					}
					else if (i < NetItem.InventorySlots + NetItem.ArmorSlots + NetItem.DyeSlots + NetItem.MiscEquipSlots + NetItem.MiscDyeSlots)
					{
						int num = i - (NetItem.InventorySlots + NetItem.ArmorSlots + NetItem.DyeSlots + NetItem.MiscEquipSlots);
						NetMessage.SendData(5, -1, -1, NetworkText.FromLiteral(args.Player.TPlayer.miscDyes[num].Name), args.Player.Index, i, (int)args.Player.TPlayer.miscDyes[num].prefix);
						NetMessage.SendData(5, args.Player.Index, -1, NetworkText.FromLiteral(args.Player.TPlayer.miscDyes[num].Name), args.Player.Index, i, (int)args.Player.TPlayer.miscDyes[num].prefix);
					}
				}
			}
			args.Player.SendMessage("[c/595959:»]  Vanish zostal " + (args.TPlayer.active ? "[c/ff6666:wylaczony]" : "[c/66ff66:wlaczony]") + ".", Color.Gray);
		}

		public static void top(TShockAPI.CommandArgs args)
		{
			string text = null;
			if (args.Parameters.Count > 0)
			{
				text = args.Parameters[0].ToLower();
			}
			switch (text)
			{
			default:
				args.Player.SendMessage("[c/595959:»]  [c/66FF66:Uzycie][c/595959::] /top <kasa/pvp/czasgry>", Color.Gray);
				break;
			case "kasa":
			{
				Dictionary<string, int> topKasa = QueryPlr.GetTopKasa(args.Player.Name);
				int num3 = 1;
				Color color3 = Color.Gold;
				args.Player.SendMessage("               [c/595959:«]Top 5 - Kasa[c/595959:»]", Color.LightGray);
				foreach (string key in topKasa.Keys)
				{
					args.Player.SendMessage(string.Format("[c/595959:»]  {0}# [c/595959:-] {1} [c/595959:(]{2} €[c/595959:)]", num3, key, topKasa[key].ToString("N0")), color3);
					num3++;
					switch (num3)
					{
					case 2:
						color3 = Color.Silver;
						break;
					case 3:
						color3 = new Color(205, 127, 50);
						break;
					default:
						color3 = Color.Gray;
						break;
					}
				}
				break;
			}
			case "pvp":
			{
				Dictionary<string, double[]> topPvp = DataBase.GetTopPvp(args.Player.Name);
				int num2 = 1;
				Color color2 = Color.Gold;
				args.Player.SendMessage("              [c/595959:«]Top 5 - PvP[c/595959:»]", Color.LightGray);
				foreach (string key2 in topPvp.Keys)
				{
					args.Player.SendMessage($"[c/595959:»]  {num2}# [c/595959:-] {key2} [c/595959:(]{topPvp[key2][0]}/{topPvp[key2][1]} | {topPvp[key2][2]}[c/595959:)]", color2);
					num2++;
					switch (num2)
					{
					case 2:
						color2 = Color.Silver;
						break;
					case 3:
						color2 = new Color(205, 127, 50);
						break;
					default:
						color2 = Color.Gray;
						break;
					}
				}
				break;
			}
			case "czasgry":
			{
				Dictionary<string, TimeSpan> topCzasGry = DataBase.GetTopCzasGry(args.Player.Name);
				int num = 1;
				Color color = Color.Gold;
				args.Player.SendMessage("             [c/595959:«]Top 5 - Czas Gry[c/595959:»]", Color.LightGray);
				foreach (string key3 in topCzasGry.Keys)
				{
					args.Player.SendMessage($"[c/595959:»]  {num}# [c/595959:-] {key3} [c/595959:(]{Math.Round(topCzasGry[key3].TotalHours, 1)} h[c/595959:)]", color);
					num++;
					switch (num)
					{
					case 2:
						color = Color.Silver;
						break;
					case 3:
						color = new Color(205, 127, 50);
						break;
					default:
						color = Color.Gray;
						break;
					}
				}
				break;
			}
			}
		}

		public static void GetPlayTime(TShockAPI.CommandArgs args)
		{
			string text = null;
			if (args.Parameters.Count > 0)
			{
				text = args.Parameters[0].ToLower();
			}
			if (text != null)
			{
				List<TSPlayer> list = TSPlayer.FindByNameOrID(text);
				if (list.Count < 1)
				{
					args.Player.SendErrorMessage("[c/595959:»]  Nie mozna bylo znalezc gracza {0}.", args.Parameters[0]);
					return;
				}
				TimeSpan timeSpan = TimeSpan.FromSeconds(SurvivalCore.srvPlayers[list[0].Index].playTime + (long)SurvivalCore.playTime[(byte)list[0].Index].Elapsed.TotalSeconds);
				string text2 = (timeSpan.Seconds == 1) ? "sekunde" : ((timeSpan.Seconds <= 1 || timeSpan.Seconds >= 5) ? "sekund" : "sekundy");
				string text3 = (timeSpan.Minutes == 1) ? "minute" : ((timeSpan.Minutes <= 1 || timeSpan.Minutes >= 5) ? "minut" : "minuty");
				string text4 = ((int)timeSpan.TotalHours == 1) ? "godzine" : (((int)timeSpan.TotalHours <= 1 || (int)timeSpan.TotalHours >= 5) ? "godzin" : "godziny");
				args.Player.SendMessage($"[c/595959:»] [c/66ff66:{list[0].Name}] lacznie na serwerze spedzil [c/66ff66:{(int)timeSpan.TotalHours}] {text4}, [c/66ff66:{timeSpan.Minutes}] {text3}, [c/66ff66:{timeSpan.Seconds}] {text2}.", Color.Gray);
			}
			else
			{
				TimeSpan timeSpan2 = TimeSpan.FromSeconds(SurvivalCore.srvPlayers[args.Player.Index].playTime + (long)SurvivalCore.playTime[(byte)args.Player.Index].Elapsed.TotalSeconds);
				string text5 = (timeSpan2.Seconds == 1) ? "sekunde" : ((timeSpan2.Seconds <= 1 || timeSpan2.Seconds >= 5) ? "sekund" : "sekundy");
				string text6 = (timeSpan2.Minutes == 1) ? "minute" : ((timeSpan2.Minutes <= 1 || timeSpan2.Minutes >= 5) ? "minut" : "minuty");
				string text7 = ((int)timeSpan2.TotalHours == 1) ? "godzine" : (((int)timeSpan2.TotalHours <= 1 || (int)timeSpan2.TotalHours >= 5) ? "godzin" : "godziny");
				args.Player.SendMessage($"[c/595959:»] Lacznie na serwerze spedziles [c/66ff66:{(int)timeSpan2.TotalHours}] {text7}, [c/66ff66:{timeSpan2.Minutes}] {text6}, [c/66ff66:{timeSpan2.Seconds}] {text5}.", Color.Gray);
			}
		}

		public static void StaffChat(TShockAPI.CommandArgs args)
		{
			string text = string.Join(" ", args.Parameters);
			if (text == null || text == "")
			{
				args.Player.SendMessage("[c/595959:»]  [c/66FF66:Uzycie][c/595959::] /s <wiadomosc>", Color.Gray);
				return;
			}
			foreach (TSPlayer item in TShock.Players.Where((TSPlayer p) => p?.HasPermission("server.jmod") ?? false))
			{
				item.SendMessage("[i:549] [c/595959:⮘] [c/2ECC71:Kadra] [c/595959:⮚] [c/" + PowelderAPI.Utils.getGroupColor(args.Player.Group.Name) + ":" + args.Player.Name + "][c/595959::] " + text, new Color(136, 255, 77));
			}
		}

		public static void HelpOp(TShockAPI.CommandArgs args)
		{
			string text = string.Join(" ", args.Parameters);
			if (text == null || text == "")
			{
				args.Player.SendMessage("[c/595959:»]  [c/66FF66:Uzycie][c/595959::] /adminhelp <wiadomosc>", Color.Gray);
				return;
			}
			foreach (TSPlayer item in TShock.Players.Where((TSPlayer p) => p?.HasPermission("server.jmod") ?? false))
			{
				item.SendMessage("[i:548] [c/595959:⮘] [c/1072F6:AdminHelp] [c/595959:⮚] [c/" + PowelderAPI.Utils.getGroupColor(args.Player.Group.Name) + ":" + args.Player.Name + "][c/595959::] " + text, new Color(75, 171, 255));
			}
			args.Player.SendMessage("[c/595959:»]  Pomyslne wyslano wiadomosc do wszystkich czlonkow kadry!", Color.Gray);
		}

		public static void statusOptions(TShockAPI.CommandArgs args)
		{
			string text = null;
			if (args.Parameters.Count > 0)
			{
				text = args.Parameters[0].ToLower();
			}
			switch (text)
			{
			default:
				args.Player.SendMessage("[c/595959:»]  [c/66FF66:Uzycie][c/595959::] /status <opcja>", Color.Gray);
				args.Player.SendMessage("[c/595959:»]  [c/66FF66:Opcje][c/595959::] cale, online, ping, konto, zgony, pvp, waznosc, clean", Color.Gray);
				break;
			case "cale":
				if (SavingFormat.IsTrue(SurvivalCore.srvPlayers[args.Player.Index].statusOptions, 0))
				{
					SurvivalCore.srvPlayers[args.Player.Index].statusOptions = SavingFormat.Change(SurvivalCore.srvPlayers[args.Player.Index].statusOptions, 0, false);
					args.Player.SendMessage("[c/595959:»]  Status zostal calkowice wylaczony.", Color.Gray);
					args.Player.SendData(PacketTypes.Status);
				}
				else
				{
					SurvivalCore.srvPlayers[args.Player.Index].statusOptions = SavingFormat.Change(SurvivalCore.srvPlayers[args.Player.Index].statusOptions, 0, true);
					args.Player.SendMessage("[c/595959:»]  Status zostal wlaczony.", Color.Gray);
				}
				break;
			case "online":
				if (SavingFormat.IsTrue(SurvivalCore.srvPlayers[args.Player.Index].statusOptions, 1))
				{
					SurvivalCore.srvPlayers[args.Player.Index].statusOptions = SavingFormat.Change(SurvivalCore.srvPlayers[args.Player.Index].statusOptions, 1, false);
					args.Player.SendMessage("[c/595959:»]  [c/66ff66:Online] w statusie zostal wylaczony.", Color.Gray);
				}
				else
				{
					SurvivalCore.srvPlayers[args.Player.Index].statusOptions = SavingFormat.Change(SurvivalCore.srvPlayers[args.Player.Index].statusOptions, 1, true);
					args.Player.SendMessage("[c/595959:»]  [c/66ff66:Online] w statusie zostal wlaczony.", Color.Gray);
				}
				break;
			case "konto":
				if (SavingFormat.IsTrue(SurvivalCore.srvPlayers[args.Player.Index].statusOptions, 2))
				{
					SurvivalCore.srvPlayers[args.Player.Index].statusOptions = SavingFormat.Change(SurvivalCore.srvPlayers[args.Player.Index].statusOptions, 2, false);
					args.Player.SendMessage("[c/595959:»]  [c/66ff66:Konto] w statusie zostalo wylaczone.", Color.Gray);
				}
				else
				{
					SurvivalCore.srvPlayers[args.Player.Index].statusOptions = SavingFormat.Change(SurvivalCore.srvPlayers[args.Player.Index].statusOptions, 2, true);
					args.Player.SendMessage("[c/595959:»]  [c/66ff66:Konto] w statusie zostalo wlaczone.", Color.Gray);
				}
				break;
			case "zgony":
				if (SavingFormat.IsTrue(SurvivalCore.srvPlayers[args.Player.Index].statusOptions, 3))
				{
					SurvivalCore.srvPlayers[args.Player.Index].statusOptions = SavingFormat.Change(SurvivalCore.srvPlayers[args.Player.Index].statusOptions, 3, false);
					args.Player.SendMessage("[c/595959:»]  [c/66ff66:Zgony] w statusie zostaly wylaczone.", Color.Gray);
				}
				else
				{
					SurvivalCore.srvPlayers[args.Player.Index].statusOptions = SavingFormat.Change(SurvivalCore.srvPlayers[args.Player.Index].statusOptions, 3, true);
					args.Player.SendMessage("[c/595959:»]  [c/66ff66:Zgony] w statusie zostaly wlaczone.", Color.Gray);
				}
				break;
			case "waznosc":
				if (SavingFormat.IsTrue(SurvivalCore.srvPlayers[args.Player.Index].statusOptions, 4))
				{
					SurvivalCore.srvPlayers[args.Player.Index].statusOptions = SavingFormat.Change(SurvivalCore.srvPlayers[args.Player.Index].statusOptions, 4, false);
					args.Player.SendMessage("[c/595959:»]  [c/66ff66:Waznosc] w statusie zostala wylaczona.", Color.Gray);
				}
				else
				{
					SurvivalCore.srvPlayers[args.Player.Index].statusOptions = SavingFormat.Change(SurvivalCore.srvPlayers[args.Player.Index].statusOptions, 4, true);
					args.Player.SendMessage("[c/595959:»]  [c/66ff66:Waznosc] w statusie zostala wlaczona.", Color.Gray);
				}
				break;
			case "clean":
				if (SavingFormat.IsTrue(SurvivalCore.srvPlayers[args.Player.Index].statusOptions, 5))
				{
					SurvivalCore.srvPlayers[args.Player.Index].statusOptions = SavingFormat.Change(SurvivalCore.srvPlayers[args.Player.Index].statusOptions, 5, false);
					args.Player.SendMessage("[c/595959:»]  [c/66ff66:Clean] w statusie zostal wylaczony.", Color.Gray);
				}
				else
				{
					SurvivalCore.srvPlayers[args.Player.Index].statusOptions = SavingFormat.Change(SurvivalCore.srvPlayers[args.Player.Index].statusOptions, 5, true);
					args.Player.SendMessage("[c/595959:»]  [c/66ff66:Clean] w statusie zostal wlaczony.", Color.Gray);
				}
				break;
			case "pvp":
				if (SavingFormat.IsTrue(SurvivalCore.srvPlayers[args.Player.Index].statusOptions, 6))
				{
					SurvivalCore.srvPlayers[args.Player.Index].statusOptions = SavingFormat.Change(SurvivalCore.srvPlayers[args.Player.Index].statusOptions, 6, false);
					args.Player.SendMessage("[c/595959:»]  [c/66ff66:PVP] w statusie zostalo wylaczone.", Color.Gray);
				}
				else
				{
					SurvivalCore.srvPlayers[args.Player.Index].statusOptions = SavingFormat.Change(SurvivalCore.srvPlayers[args.Player.Index].statusOptions, 6, true);
					args.Player.SendMessage("[c/595959:»]  [c/66ff66:PVP] w statusie zostal wlaczone.", Color.Gray);
				}
				break;
			case "ping":
				if (SavingFormat.IsTrue(SurvivalCore.srvPlayers[args.Player.Index].statusOptions, 7))
				{
					SurvivalCore.srvPlayers[args.Player.Index].statusOptions = SavingFormat.Change(SurvivalCore.srvPlayers[args.Player.Index].statusOptions, 7, false);
					args.Player.SendMessage("[c/595959:»]  [c/66ff66:Ping] w statusie zostalo wylaczony.", Color.Gray);
				}
				else
				{
					SurvivalCore.srvPlayers[args.Player.Index].statusOptions = SavingFormat.Change(SurvivalCore.srvPlayers[args.Player.Index].statusOptions, 7, true);
					args.Player.SendMessage("[c/595959:»]  [c/66ff66:Ping] w statusie zostal wlaczony.", Color.Gray);
				}
				break;
			}
		}

		public static void clearChat(TShockAPI.CommandArgs args)
		{
			string str = string.Join(" ", args.Parameters);
			int num = 0;
			do
			{
				TSPlayer.All.SendInfoMessage(" ");
				num++;
			}
			while (num != 450);
			if (args.Parameters.Count > 0)
			{
				TSPlayer.All.SendInfoMessage("[c/595959:»]  Czat zostal wyczyszczony. [c/595959:(]" + str + "[c/595959:)]");
			}
			else
			{
				TSPlayer.All.SendInfoMessage("[c/595959:»]  Czat zostal wyczyszczony.");
			}
		}

		public static void DeathMessages(TShockAPI.CommandArgs args)
		{
			if (!SurvivalCore.isDeathMessage[args.Player.Index])
			{
				SurvivalCore.isDeathMessage[args.Player.Index] = true;
				args.Player.SendMessage("[c/595959:»]  Powiadomienia o zgonach zostaly [c/66ff66:wlaczone].", Color.Gray);
			}
			else
			{
				SurvivalCore.isDeathMessage[args.Player.Index] = false;
				args.Player.SendMessage("[c/595959:»]  Powiadomienia o zgonach zostaly [c/66ff66:wylaczone].", Color.Gray);
			}
		}

		public static void Przywolywanie(TShockAPI.CommandArgs args)
		{
			string text = null;
			string text2 = null;
			if (args.Parameters.Count > 0)
			{
				text = args.Parameters[0].ToLower();
			}
			if (args.Parameters.Count > 1)
			{
				text2 = args.Parameters[1].ToLower();
			}
			switch (text)
			{
			default:
				args.Player.SendMessage("[c/595959:»]  [c/66FF66:Uzycie][c/595959::] /przywolaj <co>", Color.Gray);
				break;
			case "wof":
			case "wall of flesh":
			{
				int cost3 = 1600;
				cost3 = Utils.costCalc(args.Player, cost3);
				if (SurvivalCore.srvPlayers[args.Player.Index].Money < cost3)
				{
					args.Player.SendErrorMessage("[c/595959:»]  Nie stac cie na przywolanie Wall of Flesha. [c/595959:(]Koszt {0} €[c/595959:)]", cost3);
					break;
				}
				Item itemById = TShock.Utils.GetItemById(267);
				if (PowelderAPI.Utils.PlayerItemAmount(args.Player, itemById) < 1)
				{
					args.Player.SendErrorMessage("[c/595959:»]  Nie masz wymaganych materialow w ekwipunku.");
					args.Player.SendErrorMessage("[c/595959:»]  Wymagane[c/595959::]  [i:267]");
					break;
				}
				if (Main.wofNPCIndex >= 0)
				{
					args.Player.SendErrorMessage("[c/595959:»]  Wall of Flesh juz jest na swiecie.");
					break;
				}
				if (args.Player.Y / 16f < (float)(Main.maxTilesY - 205))
				{
					args.Player.SendErrorMessage("[c/595959:»]  Musisz byc w piekle, aby przywolac Wall of Flesha.");
					break;
				}
				PowelderAPI.Utils.PlayerRemovingItems(args.Player, itemById, 1);
				SurvivalCore.srvPlayers[args.Player.Index].Money -= cost3;
				NPC.SpawnWOF(new Vector2(args.Player.X, args.Player.Y));
				TSPlayer.All.SendInfoMessage("[c/595959:»]  {0} przywolal Wall of Flesha.", args.Player.Name);
				break;
			}
			case "lunatic":
			case "Lun":
			case "lc":
			case "lunatic cultist":
			{
				NPC nPC2 = new NPC();
				nPC2.SetDefaults(439);
				if (!NPC.downedGolemBoss)
				{
					args.Player.SendErrorMessage("[c/595959:»]  Przywolanie bedzie mozliwe po pokonaniu Golema.");
					break;
				}
				if (PowelderAPI.Utils.IsNpcOnWorld(nPC2.type))
				{
					args.Player.SendErrorMessage("[c/595959:»]  Lunatic Cultist juz jest na swiecie.");
					break;
				}
				int cost2 = 5000;
				cost2 = Utils.costCalc(args.Player, cost2);
				if (SurvivalCore.srvPlayers[args.Player.Index].Money < cost2)
				{
					args.Player.SendErrorMessage("[c/595959:»]  Nie stac cie na przywolanie Lunatic Cultist. [c/595959:(]Koszt {0} €[c/595959:)]", cost2);
					break;
				}
				if (PowelderAPI.Utils.PlayerItemAmount(args.Player, TShock.Utils.GetItemById(1274)) < 1 || PowelderAPI.Utils.PlayerItemAmount(args.Player, TShock.Utils.GetItemById(148)) < 5)
				{
					args.Player.SendErrorMessage("[c/595959:»]  Nie masz wymaganych materialow w ekwipunku.");
					args.Player.SendErrorMessage("[c/595959:»]  Wymagane[c/595959::]  [i:1274] [i/s5:148]");
					break;
				}
				PowelderAPI.Utils.PlayerRemovingItems(args.Player, TShock.Utils.GetItemById(1274), 1);
				PowelderAPI.Utils.PlayerRemovingItems(args.Player, TShock.Utils.GetItemById(148), 5);
				SurvivalCore.srvPlayers[args.Player.Index].Money -= cost2;
				TSPlayer.Server.SpawnNPC(nPC2.type, nPC2.FullName, 1, args.Player.TileX, args.Player.TileY);
				TSPlayer.All.SendInfoMessage("[c/595959:»]  {0} przywolal Lunatic Cultista.", args.Player.Name);
				break;
			}
			case "martians":
			case "martian madness":
			case "mm":
			case "martian":
			{
				if (!NPC.downedGolemBoss)
				{
					args.Player.SendErrorMessage("[c/595959:»]  Przywolanie bedzie mozliwe po pokonaniu Golema.");
					break;
				}
				if (Main.invasionType != 0)
				{
					args.Player.SendErrorMessage("[c/595959:»]  Na swiecie juz jest jakas inwazja.");
					break;
				}
				int cost4 = 5500;
				cost4 = Utils.costCalc(args.Player, cost4);
				if (SurvivalCore.srvPlayers[args.Player.Index].Money < cost4)
				{
					args.Player.SendErrorMessage("[c/595959:»]  Nie stac cie na rozpoczecie Martian Madness. [c/595959:(]Koszt {0} €[c/595959:)]", cost4);
				}
				else if (PowelderAPI.Utils.PlayerItemAmount(args.Player, TShock.Utils.GetItemById(3118)) < 1 || PowelderAPI.Utils.PlayerItemAmount(args.Player, TShock.Utils.GetItemById(530)) < 95)
				{
					args.Player.SendErrorMessage("[c/595959:»]  Nie masz wymaganych materialow w ekwipunku.");
					args.Player.SendErrorMessage("[c/595959:»]  Wymagane[c/595959::]  [i:3118] [i/s5:148]");
				}
				else
				{
				    PowelderAPI.Utils.PlayerRemovingItems(args.Player, TShock.Utils.GetItemById(3118), 1);
					PowelderAPI.Utils.PlayerRemovingItems(args.Player, TShock.Utils.GetItemById(530), 95);
					SurvivalCore.srvPlayers[args.Player.Index].Money -= cost4;
				}
				break;
			}
			case "skeletron":
			case "skele":
			{
				NPC nPC = new NPC();
				nPC.SetDefaults(35);
				if (!NPC.downedBoss3)
				{
					args.Player.SendErrorMessage("[c/595959:»]  Przywolanie bedzie mozliwe po pierwszym pokonaniu Skeletrona.");
					break;
				}
				if (PowelderAPI.Utils.IsNpcOnWorld(nPC.type))
				{
					args.Player.SendErrorMessage("[c/595959:»]  Skeletron juz jest na swiecie.");
					break;
				}
				if (Main.dayTime)
				{
					args.Player.SendErrorMessage("[c/595959:»]  Przywolanie jest mozliwe tylko w nocy.");
					break;
				}
				int cost = 1000;
				cost = Utils.costCalc(args.Player, cost);
				if (SurvivalCore.srvPlayers[args.Player.Index].Money < cost)
				{
					args.Player.SendErrorMessage("[c/595959:»]  Nie stac cie na przywolanie Skeletrona. [c/595959:(]Koszt {0} €[c/595959:)]", cost);
					break;
				}
				if (PowelderAPI.Utils.PlayerItemAmount(args.Player, TShock.Utils.GetItemById(1307)) < 1)
				{
					args.Player.SendErrorMessage("[c/595959:»]  Nie masz wymaganych materialow w ekwipunku.");
					args.Player.SendErrorMessage("[c/595959:»]  Wymagane[c/595959::]  [i:1307]");
					break;
				}
				PowelderAPI.Utils.PlayerRemovingItems(args.Player, TShock.Utils.GetItemById(1307), 1);
				SurvivalCore.srvPlayers[args.Player.Index].Money -= cost;
				TSPlayer.Server.SpawnNPC(nPC.type, nPC.FullName, 1, args.Player.TileX, args.Player.TileY);
				TSPlayer.All.SendInfoMessage("[c/595959:»]  {0} przywolal Skeletrona.", args.Player.Name);
				break;
			}
			}
		}
	}
}
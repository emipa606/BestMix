using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;

namespace BestMix
{
	// Token: 0x02000005 RID: 5
	public class BMBillUtility
	{
		// Token: 0x0600001D RID: 29 RVA: 0x00004670 File Offset: 0x00002870
		public static string UseBMixMode(CompBestMix compBM, Thing billGiver, Bill bill)
		{
			string mode = "DIS";
			if (compBM != null)
			{
				mode = compBM.CurMode;
				if ((compBM?.BillBMModes) != null)
				{
					List<string> BillModeListing = compBM?.BillBMModes;
					if (BillModeListing.Count > 0)
					{
						foreach (string BillMode in BillModeListing)
						{
							if (BMBillUtility.BillValuePart(BillMode) == bill.GetUniqueLoadID())
							{
								mode = BMBillUtility.ModeValuePart(BillMode);
								if (mode == "NON")
								{
									mode = compBM.CurMode;
									break;
								}
								break;
							}
						}
					}
				}
			}
			return mode;
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00004720 File Offset: 0x00002920
		public static Texture2D GetBillBMTex(Thing billGiver, Bill bill)
		{
			string mode = BMBillUtility.GetBillBMMode(billGiver, bill);
			string TexPath = "UI/BestMix/NONIcon";
			if (mode != "NON")
			{
				TexPath = BestMixUtility.GetBMixIconPath(mode);
			}
			TexPath += "Bill";
			return ContentFinder<Texture2D>.Get(TexPath, false);
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00004764 File Offset: 0x00002964
		public static string GetBillBMMode(Thing billGiver, Bill bill)
		{
			string mode = "NON";
			if (billGiver != null && !(billGiver is Pawn))
			{
				CompBestMix CBM = billGiver.TryGetComp<CompBestMix>();
				if (CBM != null)
				{
					string billID = bill?.GetUniqueLoadID();
					if (billID != null && (CBM?.BillBMModes) != null)
					{
						List<string> BillModes = CBM.BillBMModes;
						if (BillModes.Count > 0)
						{
							foreach (string BillMode in BillModes)
							{
								if (BMBillUtility.BillValuePart(BillMode) == billID)
								{
									mode = BMBillUtility.ModeValuePart(BillMode);
									break;
								}
							}
						}
					}
				}
			}
			return mode;
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00004814 File Offset: 0x00002A14
		public static void SetBillBMVal(Thing billGiver, Bill bill)
		{
			if (billGiver != null)
			{
				CompBestMix CBM = billGiver.TryGetComp<CompBestMix>();
				if (CBM != null)
				{
					BMBillUtility.DoBillModeSelMenu(CBM, bill);
				}
			}
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00004838 File Offset: 0x00002A38
		public static void DoBillModeSelMenu(CompBestMix CBM, Bill bill)
		{
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			string text = "BestMix.DoNothing".Translate();
			list.Add(new FloatMenuOption(text, delegate()
			{
				BMBillUtility.SetBMixBillMode(CBM, bill, "NON", true);
			}, MenuOptionPriority.Default, null, null, 29f, null, null));
			using (List<string>.Enumerator enumerator = BestMixUtility.BMixModes().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					string mode = enumerator.Current;
					text = BestMixUtility.GetBMixModeDisplay(mode);
					list.Add(new FloatMenuOption(text, delegate()
					{
						BMBillUtility.SetBMixBillMode(CBM, bill, mode, true);
					}, MenuOptionPriority.Default, null, null, 29f, null, null));
				}
			}
			List<FloatMenuOption> sortedlist = (from bm in list
			orderby bm.Label
			select bm).ToList<FloatMenuOption>();
			Find.WindowStack.Add(new FloatMenu(sortedlist));
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00004950 File Offset: 0x00002B50
		public static void SetBMixBillMode(CompBestMix CBM, Bill bill, string mode, bool set)
		{
			if (CBM != null && bill != null)
			{
				string billID = bill.GetUniqueLoadID();
				List<string> newlist = new List<string>();
				if ((CBM?.BillBMModes) != null)
				{
					List<string> current = CBM?.BillBMModes;
					if (current.Count > 0)
					{
						foreach (string BillBMMode in current)
						{
							if (BMBillUtility.BillValuePart(BillBMMode) != billID)
							{
								newlist.AddDistinct(BillBMMode);
							}
						}
					}
					current.Clear();
				}
				newlist.AddDistinct(BMBillUtility.NewBillBMMode(billID, mode));
				CBM.BillBMModes = newlist;
			}
		}

		// Token: 0x06000023 RID: 35 RVA: 0x00004A08 File Offset: 0x00002C08
		public static void CheckBillBMValues(CompBestMix CBM, Thing billGiver, List<string> BillModes)
		{
			if (BillModes != null)
			{
				if (BillModes.Count > 0)
				{
					List<string> newBillModes = new List<string>();
					List<string> billIDs = new List<string>();
					IBillGiver billGiver2 = billGiver as IBillGiver;
					BillStack billStack = billGiver2?.BillStack;
					if (billStack != null)
					{
						List<Bill> bills = billStack?.Bills;
						if (bills.Count > 0)
						{
							foreach (Bill bill in bills)
							{
								string id = bill?.GetUniqueLoadID();
								if (id != null)
								{
									billIDs.AddDistinct(id);
								}
							}
						}
					}
					foreach (string BillMode in BillModes)
					{
						if (billIDs.Contains(BMBillUtility.BillValuePart(BillMode)))
						{
							newBillModes.AddDistinct(BillMode);
						}
					}
					CBM.BillBMModes = newBillModes;
					return;
				}
			}
			else
			{
				CBM.BillBMModes = new List<string>();
			}
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00004B18 File Offset: 0x00002D18
		public static string NewBillBMMode(string id, string mode)
		{
			return id + ";" + mode;
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00004B28 File Offset: 0x00002D28
		public static string BillValuePart(string value)
		{
			char[] divider = new char[]
			{
				';'
			};
			return value.Split(divider)[0];
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00004B4C File Offset: 0x00002D4C
		public static string ModeValuePart(string value)
		{
			char[] divider = new char[]
			{
				';'
			};
			return value.Split(divider)[1];
		}
	}
}

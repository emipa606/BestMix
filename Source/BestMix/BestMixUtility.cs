using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.AI;

namespace BestMix
{
	// Token: 0x02000002 RID: 2
	public class BestMixUtility
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public static bool BMixRegionIsInRange(Region r, Thing billGiver, Bill bill)
		{
			if (!Controller.Settings.IncludeRegionLimiter)
			{
				return true;
			}
			if (!BestMixUtility.IsValidForComp(billGiver))
			{
				return true;
			}
			CompBestMix compBMix = billGiver.TryGetComp<CompBestMix>();
			if (compBMix == null)
			{
				return true;
			}
			if (BMBillUtility.UseBMixMode(compBMix, billGiver, bill) == "DIS")
			{
				return true;
			}
			Map map = billGiver?.Map;
			if (map != null)
			{
				RegionGrid regions = map?.regionGrid;
				if (regions != null)
				{
					IntVec3 chkcell = IntVec3.Zero;
					for (int i = 0; i < 4; i++)
					{
						switch (i)
						{
						case 0:
							chkcell = new IntVec3(r.extentsClose.minX, 0, r.extentsClose.minZ);
							break;
						case 1:
							chkcell = new IntVec3(r.extentsClose.maxX, 0, r.extentsClose.maxZ);
							break;
						case 2:
							chkcell = new IntVec3(r.extentsClose.maxX, 0, r.extentsClose.minZ);
							break;
						case 3:
							chkcell = new IntVec3(r.extentsClose.minX, 0, r.extentsClose.maxZ);
							break;
						}
						if (regions.GetRegionAt_NoRebuild_InvalidAllowed(chkcell) == r)
						{
							float num = (float)(chkcell - billGiver.Position).LengthHorizontalSquared;
							float scaleSearchRadius = bill.ingredientSearchRadius * bill.ingredientSearchRadius;
							if (Controller.Settings.UseRadiusLimit)
							{
								float num2 = (float)Controller.Settings.RadiusLimit;
								float scaleLimit = num2 * num2;
								if (scaleLimit < scaleSearchRadius)
								{
									scaleSearchRadius = scaleLimit;
								}
							}
							if (num <= scaleSearchRadius)
							{
								return true;
							}
						}
					}
				}
			}
			return false;
		}

		// Token: 0x06000002 RID: 2 RVA: 0x000021CC File Offset: 0x000003CC
		public static bool BMixFinishedStatus(bool foundAll, Thing billGiver, Bill bill)
		{
			if (!foundAll)
			{
				return false;
			}
			if (billGiver is Pawn)
			{
				return true;
			}
			if (BestMixUtility.IsValidForComp(billGiver))
			{
				CompBestMix CBM = billGiver.TryGetComp<CompBestMix>();
				return CBM != null && BMBillUtility.UseBMixMode(CBM, billGiver, bill) == "DIS";
			}
			return true;
		}

		// Token: 0x06000003 RID: 3 RVA: 0x00002214 File Offset: 0x00000414
		public static bool IsValidForComp(Thing thing)
		{
			return Controller.Settings.AllowBestMix && thing is Building && (thing as Building).def.inspectorTabs.Contains(typeof(ITab_Bills)) && (!Controller.Settings.AllowMealMakersOnly || (Controller.Settings.AllowMealMakersOnly && (thing as Building).def.building.isMealSource));
		}

		// Token: 0x06000004 RID: 4 RVA: 0x00002288 File Offset: 0x00000488
		public static float RNDFloat()
		{
			return Rand.Range(1f, 9999f);
		}

		// Token: 0x06000005 RID: 5 RVA: 0x00002299 File Offset: 0x00000499
		public static bool IsCEActive()
		{
			return ModLister.HasActiveModWithName("Combat Extended");
		}

		// Token: 0x06000006 RID: 6 RVA: 0x000022AC File Offset: 0x000004AC
		public static List<string> BMixModes()
		{
			List<string> list = new List<string>();
			list.AddDistinct("DIS");
			list.AddDistinct("DTR");
			list.AddDistinct("HPT");
			list.AddDistinct("RHP");
			list.AddDistinct("VLC");
			list.AddDistinct("VLE");
			list.AddDistinct("TMP");
			list.AddDistinct("FRZ");
			list.AddDistinct("RND");
			list.AddDistinct("BIT");
			if (Controller.Settings.useStock)
			{
				list.AddDistinct("MST");
				list.AddDistinct("LST");
			}
			list.AddDistinct("BTY");
			list.AddDistinct("UGY");
			list.AddDistinct("HVY");
			list.AddDistinct("LGT");
			list.AddDistinct("FLM");
			list.AddDistinct("PTB");
			list.AddDistinct("PTS");
			list.AddDistinct("PTH");
			if (BestMixUtility.IsCEActive())
			{
				list.AddDistinct("PTE");
			}
			list.AddDistinct("INH");
			list.AddDistinct("INC");
			list.AddDistinct("WSP");
			list.AddDistinct("WBT");
			return list;
		}
		internal static uint ComputeStringHash(string s)
		{
			uint num = 0;
			if (s != null)
			{
				num = 2166136261U;
				for (int i = 0; i < s.Length; i++)
				{
					num = ((uint)s[i] ^ num) * 16777619U;
				}
			}
			return num;
		}

		// Token: 0x06000007 RID: 7 RVA: 0x000023E8 File Offset: 0x000005E8
		public static string GetBMixIconPath(string BMixMode)
		{
			string BMixIconPath = "UI/BestMix/";
			uint num = ComputeStringHash(BMixMode);
			if (num <= 2134933104U)
			{
				if (num <= 967981233U)
				{
					if (num <= 603573262U)
					{
						if (num != 339145631U)
						{
							if (num != 535424181U)
							{
								if (num == 603573262U)
								{
									if (BMixMode == "VLE")
									{
										return BMixIconPath + "Value";
									}
								}
							}
							else if (BMixMode == "DTR")
							{
								return BMixIconPath + "Expiry";
							}
						}
						else if (BMixMode == "MST")
						{
							return BMixIconPath + "StockMost";
						}
					}
					else if (num != 637128500U)
					{
						if (num != 785104710U)
						{
							if (num == 967981233U)
							{
								if (BMixMode == "FRZ")
								{
									return BMixIconPath + "Coldest";
								}
							}
						}
						else if (BMixMode == "FLM")
						{
							return BMixIconPath + "Ignition";
						}
					}
					else if (BMixMode == "VLC")
					{
						return BMixIconPath + "Cheapest";
					}
				}
				else if (num <= 1936927907U)
				{
					if (num != 1811023517U)
					{
						if (num != 1863960391U)
						{
							if (num == 1936927907U)
							{
								if (BMixMode == "HPT")
								{
									return BMixIconPath + "Damaged";
								}
							}
						}
						else if (BMixMode == "DIS")
						{
							return BMixIconPath + "Nearest";
						}
					}
					else if (BMixMode == "WSP")
					{
						return BMixIconPath + "Sharpest";
					}
				}
				else if (num != 2053385502U)
				{
					if (num != 2112807477U)
					{
						if (num == 2134933104U)
						{
							if (BMixMode == "LGT")
							{
								return BMixIconPath + "Lightest";
							}
						}
					}
					else if (BMixMode == "RHP")
					{
						return BMixIconPath + "Robust";
					}
				}
				else if (BMixMode == "HVY")
				{
					return BMixIconPath + "Heaviest";
				}
			}
			else if (num <= 2719427704U)
			{
				if (num <= 2334871756U)
				{
					if (num != 2258458980U)
					{
						if (num != 2318094137U)
						{
							if (num == 2334871756U)
							{
								if (BMixMode == "PTE")
								{
									return BMixIconPath + "ProtectElectric";
								}
							}
						}
						else if (BMixMode == "PTB")
						{
							return BMixIconPath + "ProtectBlunt";
						}
					}
					else if (BMixMode == "UGY")
					{
						return BMixIconPath + "Duckling";
					}
				}
				else if (num != 2418759851U)
				{
					if (num != 2569758422U)
					{
						if (num == 2719427704U)
						{
							if (BMixMode == "WBT")
							{
								return BMixIconPath + "Bluntest";
							}
						}
					}
					else if (BMixMode == "PTS")
					{
						return BMixIconPath + "ProtectSharp";
					}
				}
				else if (BMixMode == "PTH")
				{
					return BMixIconPath + "ProtectHeat";
				}
			}
			else if (num <= 3479010932U)
			{
				if (num != 2783618047U)
				{
					if (num != 2928569262U)
					{
						if (num == 3479010932U)
						{
							if (BMixMode == "LST")
							{
								return BMixIconPath + "StockLeast";
							}
						}
					}
					else if (BMixMode == "BTY")
					{
						return BMixIconPath + "Beauty";
					}
				}
				else if (BMixMode == "RND")
				{
					return BMixIconPath + "Random";
				}
			}
			else if (num <= 3850499664U)
			{
				if (num != 3838027874U)
				{
					if (num == 3850499664U)
					{
						if (BMixMode == "BIT")
						{
							return BMixIconPath + "Fraction";
						}
					}
				}
				else if (BMixMode == "INH")
				{
					return BMixIconPath + "InsulateHeat";
				}
			}
			else if (num != 3955471207U)
			{
				if (num == 4115772078U)
				{
					if (BMixMode == "TMP")
					{
						return BMixIconPath + "Warmest";
					}
				}
			}
			else if (BMixMode == "INC")
			{
				return BMixIconPath + "InsulateCold";
			}
			return BMixIconPath + "Nearest";
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002930 File Offset: 0x00000B30
		public static string GetBMixModeDisplay(string BMixMode)
		{
			uint num = ComputeStringHash(BMixMode);
			if (num <= 2134933104U)
			{
				if (num <= 967981233U)
				{
					if (num <= 603573262U)
					{
						if (num != 339145631U)
						{
							if (num != 535424181U)
							{
								if (num == 603573262U)
								{
									if (BMixMode == "VLE")
									{
										return "BestMix.ModeValueVLE".Translate();
									}
								}
							}
							else if (BMixMode == "DTR")
							{
								return "BestMix.ModeDaysToRotDTR".Translate();
							}
						}
						else if (BMixMode == "MST")
						{
							return "BestMix.ModeStockMST".Translate();
						}
					}
					else if (num != 637128500U)
					{
						if (num != 785104710U)
						{
							if (num == 967981233U)
							{
								if (BMixMode == "FRZ")
								{
									return "BestMix.ModeTemperatureFRZ".Translate();
								}
							}
						}
						else if (BMixMode == "FLM")
						{
							return "BestMix.ModeFlammableFLM".Translate();
						}
					}
					else if (BMixMode == "VLC")
					{
						return "BestMix.ModeValueVLC".Translate();
					}
				}
				else if (num <= 1936927907U)
				{
					if (num != 1811023517U)
					{
						if (num != 1863960391U)
						{
							if (num == 1936927907U)
							{
								if (BMixMode == "HPT")
								{
									return "BestMix.ModeHealthHPT".Translate();
								}
							}
						}
						else if (BMixMode == "DIS")
						{
							return "BestMix.ModeDistanceDIS".Translate();
						}
					}
					else if (BMixMode == "WSP")
					{
						return "BestMix.ModeWeaponWSP".Translate();
					}
				}
				else if (num != 2053385502U)
				{
					if (num != 2112807477U)
					{
						if (num == 2134933104U)
						{
							if (BMixMode == "LGT")
							{
								return "BestMix.ModeMassLGT".Translate();
							}
						}
					}
					else if (BMixMode == "RHP")
					{
						return "BestMix.ModeHealthRHP".Translate();
					}
				}
				else if (BMixMode == "HVY")
				{
					return "BestMix.ModeMassHVY".Translate();
				}
			}
			else if (num <= 2719427704U)
			{
				if (num <= 2334871756U)
				{
					if (num != 2258458980U)
					{
						if (num != 2318094137U)
						{
							if (num == 2334871756U)
							{
								if (BMixMode == "PTE")
								{
									return "BestMix.ModeProtectPTE".Translate();
								}
							}
						}
						else if (BMixMode == "PTB")
						{
							return "BestMix.ModeProtectPTB".Translate();
						}
					}
					else if (BMixMode == "UGY")
					{
						return "BestMix.ModeBeautyUGY".Translate();
					}
				}
				else if (num != 2418759851U)
				{
					if (num != 2569758422U)
					{
						if (num == 2719427704U)
						{
							if (BMixMode == "WBT")
							{
								return "BestMix.ModeWeaponWBT".Translate();
							}
						}
					}
					else if (BMixMode == "PTS")
					{
						return "BestMix.ModeProtectPTS".Translate();
					}
				}
				else if (BMixMode == "PTH")
				{
					return "BestMix.ModeProtectPTH".Translate();
				}
			}
			else if (num <= 3479010932U)
			{
				if (num != 2783618047U)
				{
					if (num != 2928569262U)
					{
						if (num == 3479010932U)
						{
							if (BMixMode == "LST")
							{
								return "BestMix.ModeStockLST".Translate();
							}
						}
					}
					else if (BMixMode == "BTY")
					{
						return "BestMix.ModeBeautyBTY".Translate();
					}
				}
				else if (BMixMode == "RND")
				{
					return "BestMix.ModeRandomRND".Translate();
				}
			}
			else if (num <= 3850499664U)
			{
				if (num != 3838027874U)
				{
					if (num == 3850499664U)
					{
						if (BMixMode == "BIT")
						{
							return "BestMix.ModeFractionBIT".Translate();
						}
					}
				}
				else if (BMixMode == "INH")
				{
					return "BestMix.ModeInsulateINH".Translate();
				}
			}
			else if (num != 3955471207U)
			{
				if (num == 4115772078U)
				{
					if (BMixMode == "TMP")
					{
						return "BestMix.ModeTemperatureTMP".Translate();
					}
				}
			}
			else if (BMixMode == "INC")
			{
				return "BestMix.ModeInsulateINC".Translate();
			}
			return "BestMix.ModeDistanceDIS".Translate();
		}

		// Token: 0x06000009 RID: 9 RVA: 0x00002EE0 File Offset: 0x000010E0
		public static Comparison<Thing> GetBMixComparer(Thing billGiver, IntVec3 rootCell, Bill bill, out bool rnd)
		{
			rnd = false;
			string BMixMode = "DIS";
			if (BestMixUtility.IsValidForComp(billGiver))
			{
				CompBestMix compBM = billGiver.TryGetComp<CompBestMix>();
				if (compBM != null)
				{
					BMixMode = BMBillUtility.UseBMixMode(compBM, billGiver, bill);
					bool bmixDebug = compBM.BMixDebug;
				}
			}
			uint num = ComputeStringHash(BMixMode);
			Comparison<Thing> comparison;
			if (num <= 2134933104U)
			{
				if (num <= 967981233U)
				{
					if (num <= 603573262U)
					{
						if (num != 339145631U)
						{
							if (num != 535424181U)
							{
								if (num == 603573262U)
								{
									if (BMixMode == "VLE")
									{
										return delegate(Thing t1, Thing t2)
										{
											float num2 = t2.MarketValue;
											float value = t1.MarketValue;
											return num2.CompareTo(value);
										};
									}
								}
							}
							else if (BMixMode == "DTR")
							{
								return delegate(Thing t1, Thing t2)
								{
									float t1dtr;
									float num3 = t1dtr = 7.2E+07f;
									CompRottable t1comp = t1.TryGetComp<CompRottable>();
									if (t1comp != null)
									{
										t1dtr = (float)t1comp.TicksUntilRotAtCurrentTemp;
									}
									float t2dtr = num3;
									CompRottable t2comp = t2.TryGetComp<CompRottable>();
									if (t2comp != null)
									{
										t2dtr = (float)t2comp.TicksUntilRotAtCurrentTemp;
									}
									float num2 = t1dtr;
									float value = t2dtr;
									return num2.CompareTo(value);
								};
							}
						}
						else if (BMixMode == "MST")
						{
							return delegate(Thing t1, Thing t2)
							{
								float num2 = BestMixUtility.GetStockAmount(t2, billGiver, rootCell, bill);
								float value = BestMixUtility.GetStockAmount(t1, billGiver, rootCell, bill);
								return num2.CompareTo(value);
							};
						}
					}
					else if (num != 637128500U)
					{
						if (num != 785104710U)
						{
							if (num == 967981233U)
							{
								if (BMixMode == "FRZ")
								{
									return delegate(Thing t1, Thing t2)
									{
										float num2 = 0f - t2.AmbientTemperature;
										float value = 0f - t1.AmbientTemperature;
										return num2.CompareTo(value);
									};
								}
							}
						}
						else if (BMixMode == "FLM")
						{
							return delegate(Thing t1, Thing t2)
							{
								float num2 = t2.GetStatValue(StatDefOf.Flammability, true);
								float value = t1.GetStatValue(StatDefOf.Flammability, true);
								return num2.CompareTo(value);
							};
						}
					}
					else if (BMixMode == "VLC")
					{
						return delegate(Thing t1, Thing t2)
						{
							float num2 = 0f - t2.MarketValue;
							float value = 0f - t1.MarketValue;
							return num2.CompareTo(value);
						};
					}
				}
				else if (num <= 1936927907U)
				{
					if (num != 1811023517U)
					{
						if (num != 1863960391U)
						{
							if (num == 1936927907U)
							{
								if (BMixMode == "HPT")
								{
									return delegate(Thing t1, Thing t2)
									{
										float num2 = 0f;
										if (t2.def.useHitPoints)
										{
											num2 = (float)(t2.MaxHitPoints - t2.HitPoints) / (float)Math.Max(1, t2.MaxHitPoints);
										}
										float value = 0f;
										if (t1.def.useHitPoints)
										{
											value = (float)(t1.MaxHitPoints - t1.HitPoints) / (float)Math.Max(1, t1.MaxHitPoints);
										}
										return num2.CompareTo(value);
									};
								}
							}
						}
						else if (BMixMode == "DIS")
						{
							return delegate(Thing t1, Thing t2)
							{
								float num2 = (float)(t1.Position - rootCell).LengthHorizontalSquared;
								float value = (float)(t2.Position - rootCell).LengthHorizontalSquared;
								return num2.CompareTo(value);
							};
						}
					}
					else if (BMixMode == "WSP")
					{
						return delegate(Thing t1, Thing t2)
						{
							float num2 = t2.GetStatValue(StatDefOf.SharpDamageMultiplier, true);
							float value = t1.GetStatValue(StatDefOf.SharpDamageMultiplier, true);
							return num2.CompareTo(value);
						};
					}
				}
				else if (num != 2053385502U)
				{
					if (num != 2112807477U)
					{
						if (num == 2134933104U)
						{
							if (BMixMode == "LGT")
							{
								return delegate(Thing t1, Thing t2)
								{
									float num2 = 0f - t2.GetStatValue(StatDefOf.Mass, true);
									float value = 0f - t1.GetStatValue(StatDefOf.Mass, true);
									return num2.CompareTo(value);
								};
							}
						}
					}
					else if (BMixMode == "RHP")
					{
						return delegate(Thing t1, Thing t2)
						{
							float num2 = 0f;
							if (t2.def.useHitPoints)
							{
								num2 = (float)t2.HitPoints / (float)Math.Max(1, t2.MaxHitPoints);
							}
							float value = 0f;
							if (t1.def.useHitPoints)
							{
								value = (float)t1.HitPoints / (float)Math.Max(1, t1.MaxHitPoints);
							}
							return num2.CompareTo(value);
						};
					}
				}
				else if (BMixMode == "HVY")
				{
					return delegate(Thing t1, Thing t2)
					{
						float num2 = t2.GetStatValue(StatDefOf.Mass, true);
						float value = t1.GetStatValue(StatDefOf.Mass, true);
						return num2.CompareTo(value);
					};
				}
			}
			else if (num <= 2719427704U)
			{
				if (num <= 2334871756U)
				{
					if (num != 2258458980U)
					{
						if (num != 2318094137U)
						{
							if (num == 2334871756U)
							{
								if (BMixMode == "PTE")
								{
									return delegate(Thing t1, Thing t2)
									{
										float num2 = 0f;
										float value = 0f;
										StatDef protElectric = DefDatabase<StatDef>.GetNamed(BestMixUtility.ProtElectricStat, false);
										if (protElectric != null)
										{
											num2 = t2.GetStatValue(protElectric, true);
											value = t1.GetStatValue(protElectric, true);
										}
										return num2.CompareTo(value);
									};
								}
							}
						}
						else if (BMixMode == "PTB")
						{
							return delegate(Thing t1, Thing t2)
							{
								float num2 = t2.GetStatValue(StatDefOf.StuffPower_Armor_Blunt, true);
								float value = t1.GetStatValue(StatDefOf.StuffPower_Armor_Blunt, true);
								return num2.CompareTo(value);
							};
						}
					}
					else if (BMixMode == "UGY")
					{
						return delegate(Thing t1, Thing t2)
						{
							float num2 = 0f - t2.GetStatValue(StatDefOf.Beauty, true);
							ThingDef def = t2.def;
							if ((def?.stuffProps) != null)
							{
								ThingDef def2 = t2.def;
								bool flag;
								if (def2 == null)
								{
									flag = (null != null);
								}
								else
								{
									StuffProperties stuffProps = def2.stuffProps;
									flag = ((stuffProps?.statOffsets) != null);
								}
								if (flag && t2.def.stuffProps.statOffsets.StatListContains(StatDefOf.Beauty))
								{
									num2 -= t2.def.stuffProps.statOffsets.GetStatOffsetFromList(StatDefOf.Beauty);
								}
							}
							float value = 0f - t1.GetStatValue(StatDefOf.Beauty, true);
							ThingDef def3 = t1.def;
							if ((def3?.stuffProps) != null)
							{
								ThingDef def4 = t1.def;
								bool flag2;
								if (def4 == null)
								{
									flag2 = (null != null);
								}
								else
								{
									StuffProperties stuffProps2 = def4.stuffProps;
									flag2 = ((stuffProps2?.statOffsets) != null);
								}
								if (flag2 && t1.def.stuffProps.statOffsets.StatListContains(StatDefOf.Beauty))
								{
									value -= t1.def.stuffProps.statOffsets.GetStatOffsetFromList(StatDefOf.Beauty);
								}
							}
							return num2.CompareTo(value);
						};
					}
				}
				else if (num != 2418759851U)
				{
					if (num != 2569758422U)
					{
						if (num == 2719427704U)
						{
							if (BMixMode == "WBT")
							{
								return delegate(Thing t1, Thing t2)
								{
									float num2 = t2.GetStatValue(StatDefOf.BluntDamageMultiplier, true);
									float value = t1.GetStatValue(StatDefOf.BluntDamageMultiplier, true);
									return num2.CompareTo(value);
								};
							}
						}
					}
					else if (BMixMode == "PTS")
					{
						return delegate(Thing t1, Thing t2)
						{
							float num2 = t2.GetStatValue(StatDefOf.StuffPower_Armor_Sharp, true);
							float value = t1.GetStatValue(StatDefOf.StuffPower_Armor_Sharp, true);
							return num2.CompareTo(value);
						};
					}
				}
				else if (BMixMode == "PTH")
				{
					return delegate(Thing t1, Thing t2)
					{
						float num2 = t2.GetStatValue(StatDefOf.StuffPower_Armor_Heat, true);
						float value = t1.GetStatValue(StatDefOf.StuffPower_Armor_Heat, true);
						return num2.CompareTo(value);
					};
				}
			}
			else if (num <= 3479010932U)
			{
				if (num != 2783618047U)
				{
					if (num != 2928569262U)
					{
						if (num == 3479010932U)
						{
							if (BMixMode == "LST")
							{
								return delegate(Thing t1, Thing t2)
								{
									float num2 = BestMixUtility.GetStockAmount(t1, billGiver, rootCell, bill);
									float value = BestMixUtility.GetStockAmount(t2, billGiver, rootCell, bill);
									return num2.CompareTo(value);
								};
							}
						}
					}
					else if (BMixMode == "BTY")
					{
						return delegate(Thing t1, Thing t2)
						{
							float num2 = t2.GetStatValue(StatDefOf.Beauty, true);
							ThingDef def = t2.def;
							if ((def?.stuffProps) != null)
							{
								ThingDef def2 = t2.def;
								bool flag;
								if (def2 == null)
								{
									flag = (null != null);
								}
								else
								{
									StuffProperties stuffProps = def2.stuffProps;
									flag = ((stuffProps?.statOffsets) != null);
								}
								if (flag && t2.def.stuffProps.statOffsets.StatListContains(StatDefOf.Beauty))
								{
									num2 += t2.def.stuffProps.statOffsets.GetStatOffsetFromList(StatDefOf.Beauty);
								}
							}
							float value = t1.GetStatValue(StatDefOf.Beauty, true);
							ThingDef def3 = t1.def;
							if ((def3?.stuffProps) != null)
							{
								ThingDef def4 = t1.def;
								bool flag2;
								if (def4 == null)
								{
									flag2 = (null != null);
								}
								else
								{
									StuffProperties stuffProps2 = def4.stuffProps;
									flag2 = ((stuffProps2?.statOffsets) != null);
								}
								if (flag2 && t1.def.stuffProps.statOffsets.StatListContains(StatDefOf.Beauty))
								{
									value += t1.def.stuffProps.statOffsets.GetStatOffsetFromList(StatDefOf.Beauty);
								}
							}
							return num2.CompareTo(value);
						};
					}
				}
				else if (BMixMode == "RND")
				{
					comparison = delegate(Thing t1, Thing t2)
					{
						float num2 = BestMixUtility.RNDFloat();
						float value = BestMixUtility.RNDFloat();
						return num2.CompareTo(value);
					};
					rnd = true;
					return comparison;
				}
			}
			else if (num <= 3850499664U)
			{
				if (num != 3838027874U)
				{
					if (num == 3850499664U)
					{
						if (BMixMode == "BIT")
						{
							return delegate(Thing t1, Thing t2)
							{
								float num2 = (float)t2.def.stackLimit / (float)Math.Max(1, t2.stackCount);
								float value = (float)t1.def.stackLimit / (float)Math.Max(1, t1.stackCount);
								return num2.CompareTo(value);
							};
						}
					}
				}
				else if (BMixMode == "INH")
				{
					return delegate(Thing t1, Thing t2)
					{
						float num2 = t2.GetStatValue(StatDefOf.StuffPower_Insulation_Heat, true);
						float value = t1.GetStatValue(StatDefOf.StuffPower_Insulation_Heat, true);
						return num2.CompareTo(value);
					};
				}
			}
			else if (num != 3955471207U)
			{
				if (num == 4115772078U)
				{
					if (BMixMode == "TMP")
					{
						return delegate(Thing t1, Thing t2)
						{
							float num2 = t2.AmbientTemperature;
							float value = t1.AmbientTemperature;
							return num2.CompareTo(value);
						};
					}
				}
			}
			else if (BMixMode == "INC")
			{
				return delegate(Thing t1, Thing t2)
				{
					float num2 = t2.GetStatValue(StatDefOf.StuffPower_Insulation_Cold, true);
					float value = t1.GetStatValue(StatDefOf.StuffPower_Insulation_Cold, true);
					return num2.CompareTo(value);
				};
			}
			comparison = delegate(Thing t1, Thing t2)
			{
				float num2 = (float)(t1.Position - rootCell).LengthHorizontalSquared;
				float value = (float)(t2.Position - rootCell).LengthHorizontalSquared;
				return num2.CompareTo(value);
			};
			return comparison;
		}

		// Token: 0x0600000A RID: 10 RVA: 0x0000366C File Offset: 0x0000186C
		public static float GetStockAmount(Thing t, Thing billGiver, IntVec3 root, Bill bill)
		{
			float num = 0f;
			List<Thing> things = t.Map.listerThings.ThingsOfDef(t.def);
			if (things.Count > 0)
			{
				foreach (Thing thing in things)
				{
					if (BestMixUtility.BMStockInRadius(thing, billGiver, bill) && (!Controller.Settings.inStorage || thing.IsInValidStorage()) && !BestMixUtility.BMIsForbidden(thing) && !thing.IsBurning() && !thing.Fogged())
					{
						num += (float)thing.stackCount;
					}
				}
			}
			return num;
		}

		// Token: 0x0600000B RID: 11 RVA: 0x0000371C File Offset: 0x0000191C
		internal static void BMixDebugList(List<Thing> list, Thing billGiver, IntVec3 rootCell, Bill bill)
		{
			if (Prefs.DevMode && Controller.Settings.DebugMaster && Controller.Settings.DebugSort)
			{
				bool ignore = Controller.Settings.DebugIgnore;
				if (BestMixUtility.IsValidForComp(billGiver))
				{
					CompBestMix compBMix = billGiver.TryGetComp<CompBestMix>();
					if (compBMix != null && compBMix.BMixDebug && list.Count > 0)
					{
						for (int i = 0; i < list.Count; i++)
						{
							Thing thing = list[i];
							Log.Message(BestMixUtility.MakeDebugString(i, thing, billGiver, rootCell, bill, BMBillUtility.UseBMixMode(compBMix, billGiver, bill)), ignore);
						}
					}
				}
			}
		}

		// Token: 0x0600000C RID: 12 RVA: 0x000037A8 File Offset: 0x000019A8
		internal static void DebugChosenList(Thing billGiver, List<ThingCount> p_chosen)
		{
			if (Prefs.DevMode && Controller.Settings.DebugMaster && Controller.Settings.DebugChosen)
			{
				bool ignore = Controller.Settings.DebugIgnore;
				if (BestMixUtility.IsValidForComp(billGiver))
				{
					CompBestMix compBestMix = billGiver.TryGetComp<CompBestMix>();
					if (compBestMix != null && compBestMix.BMixDebug)
					{
						if (p_chosen != null)
						{
							if (p_chosen.Count > 0)
							{
								Log.Message(billGiver.Label + ", " + "BestMix.Chosen".Translate() + ":", ignore);
								using (List<ThingCount>.Enumerator enumerator = p_chosen.GetEnumerator())
								{
									while (enumerator.MoveNext())
									{
										ThingCount TC = enumerator.Current;
										Entity thing = TC.Thing;
										int count = TC.Count;
										Log.Message(thing.Label + " , " + count.ToString(), ignore);
									}
									return;
								}
							}
							Log.Message(billGiver.Label + ", " + "BestMix.NoChosen".Translate() + ":", ignore);
							return;
						}
						Log.Message(billGiver.Label + ", " + "BestMix.NullChosen".Translate() + ":", ignore);
					}
				}
			}
		}

		// Token: 0x0600000D RID: 13 RVA: 0x00003920 File Offset: 0x00001B20
		internal static void DebugFoundAll(Thing billGiver, bool foundAll)
		{
			if (Prefs.DevMode && Controller.Settings.DebugMaster && Controller.Settings.DebugFound)
			{
				bool ignore = Controller.Settings.DebugIgnore;
				if (BestMixUtility.IsValidForComp(billGiver))
				{
					CompBestMix compBMix = billGiver.TryGetComp<CompBestMix>();
					if (compBMix != null && compBMix.BMixDebug)
					{
						string debugMsg = billGiver.Label;
						if (foundAll)
						{
							debugMsg += ", " + "BestMix.FoundAll".Translate();
						}
						else
						{
							debugMsg += ", " + "BestMix.NotFoundAll".Translate();
						}
						Log.Message(debugMsg, ignore);
					}
				}
			}
		}

		// Token: 0x0600000E RID: 14 RVA: 0x000039CC File Offset: 0x00001BCC
		public static string MakeDebugString(int indx, Thing thing, Thing billGiver, IntVec3 rootCell, Bill bill, string BMixMode)
		{
			float stat = 0f;
			uint num = ComputeStringHash(BMixMode);
			IntVec3 intVec;
			if (num <= 2134933104U)
			{
				if (num <= 967981233U)
				{
					if (num <= 603573262U)
					{
						if (num != 339145631U)
						{
							if (num != 535424181U)
							{
								if (num == 603573262U)
								{
									if (BMixMode == "VLE")
									{
										stat = thing.MarketValue;
										goto IL_6CB;
									}
								}
							}
							else if (BMixMode == "DTR")
							{
								float thingdtr;
								float num2 = thingdtr = 7.2E+07f;
								CompRottable thingcomp = thing.TryGetComp<CompRottable>();
								if (thingcomp != null)
								{
									thingdtr = (float)thingcomp.TicksUntilRotAtCurrentTemp;
								}
								stat = num2 - thingdtr;
								goto IL_6CB;
							}
						}
						else if (BMixMode == "MST")
						{
							stat = BestMixUtility.GetStockAmount(thing, billGiver, rootCell, bill);
							goto IL_6CB;
						}
					}
					else if (num != 637128500U)
					{
						if (num != 785104710U)
						{
							if (num == 967981233U)
							{
								if (BMixMode == "FRZ")
								{
									stat = 0f - thing.AmbientTemperature;
									goto IL_6CB;
								}
							}
						}
						else if (BMixMode == "FLM")
						{
							stat = thing.GetStatValue(StatDefOf.Flammability, true);
							goto IL_6CB;
						}
					}
					else if (BMixMode == "VLC")
					{
						stat = 0f - thing.MarketValue;
						goto IL_6CB;
					}
				}
				else if (num <= 1936927907U)
				{
					if (num != 1811023517U)
					{
						if (num != 1863960391U)
						{
							if (num == 1936927907U)
							{
								if (BMixMode == "HPT")
								{
									stat = 0f;
									if (thing.def.useHitPoints)
									{
										stat = (float)(thing.MaxHitPoints - thing.HitPoints) / (float)Math.Max(1, thing.MaxHitPoints);
										goto IL_6CB;
									}
									goto IL_6CB;
								}
							}
						}
						else if (BMixMode == "DIS")
						{
							intVec = thing.Position - rootCell;
							stat = (float)intVec.LengthHorizontalSquared;
							goto IL_6CB;
						}
					}
					else if (BMixMode == "WSP")
					{
						stat = thing.GetStatValue(StatDefOf.SharpDamageMultiplier, true);
						goto IL_6CB;
					}
				}
				else if (num != 2053385502U)
				{
					if (num != 2112807477U)
					{
						if (num == 2134933104U)
						{
							if (BMixMode == "LGT")
							{
								stat = 0f - thing.GetStatValue(StatDefOf.Mass, true);
								goto IL_6CB;
							}
						}
					}
					else if (BMixMode == "RHP")
					{
						stat = 0f;
						if (thing.def.useHitPoints)
						{
							stat = (float)thing.HitPoints / (float)Math.Max(1, thing.MaxHitPoints);
							goto IL_6CB;
						}
						goto IL_6CB;
					}
				}
				else if (BMixMode == "HVY")
				{
					stat = thing.GetStatValue(StatDefOf.Mass, true);
					goto IL_6CB;
				}
			}
			else if (num <= 2719427704U)
			{
				if (num <= 2334871756U)
				{
					if (num != 2258458980U)
					{
						if (num != 2318094137U)
						{
							if (num == 2334871756U)
							{
								if (BMixMode == "PTE")
								{
									StatDef protElectric = DefDatabase<StatDef>.GetNamed(BestMixUtility.ProtElectricStat, false);
									if (protElectric != null)
									{
										stat = thing.GetStatValue(protElectric, true);
										goto IL_6CB;
									}
									goto IL_6CB;
								}
							}
						}
						else if (BMixMode == "PTB")
						{
							stat = thing.GetStatValue(StatDefOf.StuffPower_Armor_Blunt, true);
							goto IL_6CB;
						}
					}
					else if (BMixMode == "UGY")
					{
						stat = 0f - thing.GetStatValue(StatDefOf.Beauty, true);
						ThingDef def = thing.def;
						if ((def?.stuffProps) == null)
						{
							goto IL_6CB;
						}
						ThingDef def2 = thing.def;
						bool flag;
						if (def2 == null)
						{
							flag = (null != null);
						}
						else
						{
							StuffProperties stuffProps = def2.stuffProps;
							flag = ((stuffProps?.statOffsets) != null);
						}
						if (flag && thing.def.stuffProps.statOffsets.StatListContains(StatDefOf.Beauty))
						{
							stat -= thing.def.stuffProps.statOffsets.GetStatOffsetFromList(StatDefOf.Beauty);
							goto IL_6CB;
						}
						goto IL_6CB;
					}
				}
				else if (num != 2418759851U)
				{
					if (num != 2569758422U)
					{
						if (num == 2719427704U)
						{
							if (BMixMode == "WBT")
							{
								stat = thing.GetStatValue(StatDefOf.BluntDamageMultiplier, true);
								goto IL_6CB;
							}
						}
					}
					else if (BMixMode == "PTS")
					{
						stat = thing.GetStatValue(StatDefOf.StuffPower_Armor_Sharp, true);
						goto IL_6CB;
					}
				}
				else if (BMixMode == "PTH")
				{
					stat = thing.GetStatValue(StatDefOf.StuffPower_Armor_Heat, true);
					goto IL_6CB;
				}
			}
			else if (num <= 3479010932U)
			{
				if (num != 2783618047U)
				{
					if (num != 2928569262U)
					{
						if (num == 3479010932U)
						{
							if (BMixMode == "LST")
							{
								stat = 0f - BestMixUtility.GetStockAmount(thing, billGiver, rootCell, bill);
								goto IL_6CB;
							}
						}
					}
					else if (BMixMode == "BTY")
					{
						stat = thing.GetStatValue(StatDefOf.Beauty, true);
						ThingDef def3 = thing.def;
						if ((def3?.stuffProps) == null)
						{
							goto IL_6CB;
						}
						ThingDef def4 = thing.def;
						bool flag2;
						if (def4 == null)
						{
							flag2 = (null != null);
						}
						else
						{
							StuffProperties stuffProps2 = def4.stuffProps;
							flag2 = ((stuffProps2?.statOffsets) != null);
						}
						if (flag2 && thing.def.stuffProps.statOffsets.StatListContains(StatDefOf.Beauty))
						{
							stat += thing.def.stuffProps.statOffsets.GetStatOffsetFromList(StatDefOf.Beauty);
							goto IL_6CB;
						}
						goto IL_6CB;
					}
				}
				else if (BMixMode == "RND")
				{
					stat = BestMixUtility.RNDFloat();
					goto IL_6CB;
				}
			}
			else if (num <= 3850499664U)
			{
				if (num != 3838027874U)
				{
					if (num == 3850499664U)
					{
						if (BMixMode == "BIT")
						{
							stat = (float)thing.def.stackLimit / (float)Math.Max(1, thing.stackCount);
							goto IL_6CB;
						}
					}
				}
				else if (BMixMode == "INH")
				{
					stat = thing.GetStatValue(StatDefOf.StuffPower_Insulation_Heat, true);
					goto IL_6CB;
				}
			}
			else if (num != 3955471207U)
			{
				if (num == 4115772078U)
				{
					if (BMixMode == "TMP")
					{
						stat = thing.AmbientTemperature;
						goto IL_6CB;
					}
				}
			}
			else if (BMixMode == "INC")
			{
				stat = thing.GetStatValue(StatDefOf.StuffPower_Insulation_Cold, true);
				goto IL_6CB;
			}
			stat = 0f;
			IL_6CB:
			string[] array = new string[5];
			array[0] = "(";
			int num3 = 1;
			intVec = thing.Position;
			array[num3] = intVec.x.ToString();
			array[2] = ", ";
			int num4 = 3;
			intVec = thing.Position;
			array[num4] = intVec.z.ToString();
			array[4] = ")";
			string debugPos = string.Concat(array);
			return string.Concat(new string[]
			{
				"Debug ",
				BMixMode,
				" ",
				indx.ToString(),
				" ",
				billGiver.ThingID,
				" ",
				thing.LabelShort,
				" ",
				debugPos,
				" ",
				stat.ToString("F2")
			});
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00004169 File Offset: 0x00002369
		public static bool BMIsForbidden(Thing thing)
		{
			return thing != null && thing.TryGetComp<CompForbiddable>() != null && thing.TryGetComp<CompForbiddable>().Forbidden;
		}

		// Token: 0x06000010 RID: 16 RVA: 0x00004188 File Offset: 0x00002388
		public static bool BMStockInRadius(Thing t, Thing billGiver, Bill bill)
		{
			return Controller.Settings.mapStock || (float)(t.Position - billGiver.Position).LengthHorizontalSquared < bill.ingredientSearchRadius * bill.ingredientSearchRadius;
		}

		// Token: 0x06000011 RID: 17 RVA: 0x000041CC File Offset: 0x000023CC
		public static Predicate<Thing> BestMixValidator(Pawn pawn, Thing billGiver, Bill bill)
		{
			return (Thing t) => t.Spawned && !t.IsForbidden(pawn) && (float)(t.Position - billGiver.Position).LengthHorizontalSquared < bill.ingredientSearchRadius * bill.ingredientSearchRadius && bill.IsFixedOrAllowedIngredient(t) && bill.recipe.ingredients.Any((IngredientCount ingNeed) => ingNeed.filter.Allows(t)) && pawn.CanReserve(t, 1, -1, null, false);
		}

		// Token: 0x06000012 RID: 18 RVA: 0x000041F3 File Offset: 0x000023F3
		internal static bool TryFindBestMixInSet(List<Thing> availableThings, Bill bill, List<ThingCount> chosen, List<IngredientCount> ingredientsOrdered)
		{
			if (bill.recipe.allowMixingIngredients)
			{
				return BestMixUtility.TryFindBestMixInSet_AllowMix(availableThings, bill, chosen);
			}
			return BestMixUtility.TryFindBestMixInSet_NoMix(availableThings, bill, chosen, ingredientsOrdered);
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00004214 File Offset: 0x00002414
		internal static bool TryFindBestMixInSet_NoMix(List<Thing> availableThings, Bill bill, List<ThingCount> chosen, List<IngredientCount> ingredientsOrdered)
		{
			BestMixUtility.BMixDefCountList availableCounts = new BestMixUtility.BMixDefCountList();
			RecipeDef recipe = bill.recipe;
			chosen.Clear();
			availableCounts.Clear();
			availableCounts.GenerateFrom(availableThings);
			for (int i = 0; i < ingredientsOrdered.Count; i++)
			{
				IngredientCount ingredientCount = recipe.ingredients[i];
				bool flag = false;
				for (int j = 0; j < availableCounts.Count; j++)
				{
					float num = (float)ingredientCount.CountRequiredOfFor(availableCounts.GetDef(j), bill.recipe);
					if (num <= availableCounts.GetCount(j) && ingredientCount.filter.Allows(availableCounts.GetDef(j)) && (ingredientCount.IsFixedIngredient || bill.ingredientFilter.Allows(availableCounts.GetDef(j))))
					{
						for (int k = 0; k < availableThings.Count; k++)
						{
							if (availableThings[k].def == availableCounts.GetDef(j))
							{
								int num2 = availableThings[k].stackCount - ThingCountUtility.CountOf(chosen, availableThings[k]);
								if (num2 > 0)
								{
									int num3 = Mathf.Min(Mathf.FloorToInt(num), num2);
									ThingCountUtility.AddToList(chosen, availableThings[k], num3);
									num -= (float)num3;
									if (num < 0.001f)
									{
										flag = true;
										float count = availableCounts.GetCount(j);
										count -= (float)ingredientCount.CountRequiredOfFor(availableCounts.GetDef(j), bill.recipe);
										availableCounts.SetCount(j, count);
										break;
									}
								}
							}
						}
						if (flag)
						{
							break;
						}
					}
				}
				if (!flag)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000014 RID: 20 RVA: 0x000043A8 File Offset: 0x000025A8
		internal static bool TryFindBestMixInSet_AllowMix(List<Thing> availableThings, Bill bill, List<ThingCount> chosen)
		{
			chosen.Clear();
			for (int i = 0; i < bill.recipe.ingredients.Count; i++)
			{
				IngredientCount ingredientCount = bill.recipe.ingredients[i];
				float num = ingredientCount.GetBaseCount();
				for (int j = 0; j < availableThings.Count; j++)
				{
					Thing thing = availableThings[j];
					if (ingredientCount.filter.Allows(thing) && (ingredientCount.IsFixedIngredient || bill.ingredientFilter.Allows(thing)))
					{
						float num2 = bill.recipe.IngredientValueGetter.ValuePerUnitOf(thing.def);
						int num3 = Mathf.Min(Mathf.CeilToInt(num / num2), thing.stackCount);
						ThingCountUtility.AddToList(chosen, thing, num3);
						num -= (float)num3 * num2;
						if (num <= 0.0001f)
						{
							break;
						}
					}
				}
				if (num > 0.0001f)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x04000001 RID: 1
		public static string ProtElectricStat = "StuffPower_Armor_Electric";

		// Token: 0x02000013 RID: 19
		private class BMixDefCountList
		{
			// Token: 0x17000014 RID: 20
			// (get) Token: 0x06000076 RID: 118 RVA: 0x00005904 File Offset: 0x00003B04
			public int Count
			{
				get
				{
					return this.defs.Count;
				}
			}

			// Token: 0x17000015 RID: 21
			public float this[ThingDef def]
			{
				get
				{
					int num = this.defs.IndexOf(def);
					if (num < 0)
					{
						return 0f;
					}
					return this.counts[num];
				}
				set
				{
					int num = this.defs.IndexOf(def);
					if (num < 0)
					{
						this.defs.Add(def);
						this.counts.Add(value);
						num = this.defs.Count - 1;
					}
					else
					{
						this.counts[num] = value;
					}
					this.CheckRemove(num);
				}
			}

			// Token: 0x06000079 RID: 121 RVA: 0x0000599E File Offset: 0x00003B9E
			public float GetCount(int index)
			{
				return this.counts[index];
			}

			// Token: 0x0600007A RID: 122 RVA: 0x000059AC File Offset: 0x00003BAC
			public void SetCount(int index, float val)
			{
				this.counts[index] = val;
				this.CheckRemove(index);
			}

			// Token: 0x0600007B RID: 123 RVA: 0x000059C2 File Offset: 0x00003BC2
			public ThingDef GetDef(int index)
			{
				return this.defs[index];
			}

			// Token: 0x0600007C RID: 124 RVA: 0x000059D0 File Offset: 0x00003BD0
			private void CheckRemove(int index)
			{
				if (this.counts[index] == 0f)
				{
					this.counts.RemoveAt(index);
					this.defs.RemoveAt(index);
				}
			}

			// Token: 0x0600007D RID: 125 RVA: 0x000059FD File Offset: 0x00003BFD
			public void Clear()
			{
				this.defs.Clear();
				this.counts.Clear();
			}

			// Token: 0x0600007E RID: 126 RVA: 0x00005A18 File Offset: 0x00003C18
			public void GenerateFrom(List<Thing> things)
			{
				this.Clear();
				for (int i = 0; i < things.Count; i++)
				{
					ThingDef def = things[i].def;
					this[def] += (float)things[i].stackCount;
				}
			}

			// Token: 0x04000030 RID: 48
			private readonly List<ThingDef> defs = new List<ThingDef>();

			// Token: 0x04000031 RID: 49
			private readonly List<float> counts = new List<float>();
		}
	}
}

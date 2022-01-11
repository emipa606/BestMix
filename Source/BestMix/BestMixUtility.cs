using System;
using System.Collections.Generic;
using BestMix.Patches;
using RimWorld;
using Verse;
using Verse.AI;

namespace BestMix;

public class BestMixUtility
{
    public const string SoftnessStat = "Textile_Softness";
    public const string ProtElectricStat = "StuffPower_Armor_Electric";

    public static bool IsCEActive;
    public static bool IsSoftBedsActive;

    public static StatDef softness;
    public static StatDef protElectric;

    public static bool BMixRegionIsInRange(Region r, Thing billGiver, Bill bill)
    {
        if (!Controller.Settings.IncludeRegionLimiter)
        {
            return true;
        }

        if (!IsValidForComp(billGiver))
        {
            return true;
        }

        var compBMix = billGiver.TryGetComp<CompBestMix>();
        if (compBMix != null)
        {
            //if (compBMix.CurMode == "DIS")
            if (BMBillUtility.UseBMixMode(compBMix, billGiver, bill) == "DIS")
            {
                return true;
            }
        }
        else
        {
            return true;
        }

        /*
        List<IntVec3> cells = r?.Cells.ToList<IntVec3>();
        if ((cells != null) && (cells.Count > 0))
        {
            foreach (IntVec3 cell in cells)
            {
                if (((float)((cell - billGiver.Position).LengthHorizontalSquared)) < ((float)(bill.ingredientSearchRadius * bill.ingredientSearchRadius)))
                {
                    return true;
                }
            }
        }
        */

        // optimised to region corners
        var map = billGiver?.Map;

        var regions = map?.regionGrid; // *
        if (regions == null)
        {
            return false;
        }

        var chkcell = IntVec3.Zero;
        for (var i = 0; i < 4; i++)
        {
            switch (i)
            {
                case 0:
                    chkcell = new IntVec3(r.extentsClose.minX, 0, r.extentsClose.minZ);
                    break;
                case 3:
                    chkcell = new IntVec3(r.extentsClose.minX, 0, r.extentsClose.maxZ);
                    break;
                case 2:
                    chkcell = new IntVec3(r.extentsClose.maxX, 0, r.extentsClose.minZ);
                    break;
                case 1:
                    chkcell = new IntVec3(r.extentsClose.maxX, 0, r.extentsClose.maxZ);
                    break;
            }

            //if (chkcell.GetRegion(map) == r)
            if (!Equals(regions.GetRegionAt_NoRebuild_InvalidAllowed(chkcell), r))
            {
                continue;
            }

            var scaleToCell = (float)(chkcell - billGiver.Position).LengthHorizontalSquared;
            var scaleSearchRadius = bill.ingredientSearchRadius * bill.ingredientSearchRadius;
            if (Controller.Settings.UseRadiusLimit)
            {
                var RadiusLimit = (float)Controller.Settings.RadiusLimit;
                var scaleLimit = RadiusLimit * RadiusLimit;
                if (scaleLimit < scaleSearchRadius)
                {
                    scaleSearchRadius = scaleLimit;
                }
            }

            if (scaleToCell <= scaleSearchRadius)
            {
                return true;
            }
        }

        return false;
    }

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

        if (!IsValidForComp(billGiver))
        {
            return true;
        }

        //if (billGiver.TryGetComp<CompBestMix>().CurMode == "DIS")
        var CBM = billGiver.TryGetComp<CompBestMix>();
        if (CBM == null)
        {
            return false;
        }

        if (BMBillUtility.UseBMixMode(CBM, billGiver, bill) == "DIS")
        {
            return true;
        }

        return false;
    }

    public static bool IsValidForComp(Thing thing)
    {
        if (!Controller.Settings.AllowBestMix || thing is not Building building ||
            !building.def.inspectorTabs.Contains(typeof(ITab_Bills)))
        {
            return false;
        }

        if (!Controller.Settings.AllowMealMakersOnly || Controller.Settings.AllowMealMakersOnly &&
            building.def.building.isMealSource)
        {
            return true;
        }

        return false;
    }

    public static float RNDFloat()
    {
        return Rand.Range(1f, 9999f);
    }

    public static List<string> BMixModes()
    {
        var list = new List<string>();
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
        if (IsCEActive)
        {
            list.AddDistinct("PTE");
        }

        list.AddDistinct("INH");
        list.AddDistinct("INC");
        if (IsSoftBedsActive)
        {
            list.AddDistinct("SOF");
        }

        list.AddDistinct("WSP");
        list.AddDistinct("WBT");
        list.AddDistinct("NUL");
        list.AddDistinct("NUH");
        return list;
    }

    public static string GetBMixIconPath(string BMixMode)
    {
        var BMixIconPath = "UI/BestMix/";

        switch (BMixMode)
        {
            case "DIS":
                BMixIconPath += "Nearest";
                break;
            case "DTR":
                BMixIconPath += "Expiry";
                break;
            case "HPT":
                BMixIconPath += "Damaged";
                break;
            case "RHP":
                BMixIconPath += "Robust";
                break;
            case "VLC":
                BMixIconPath += "Cheapest";
                break;
            case "VLE":
                BMixIconPath += "Value";
                break;
            case "TMP":
                BMixIconPath += "Warmest";
                break;
            case "FRZ":
                BMixIconPath += "Coldest";
                break;
            case "RND":
                BMixIconPath += "Random";
                break;
            case "BIT":
                BMixIconPath += "Fraction";
                break;
            case "MST":
                BMixIconPath += "StockMost";
                break;
            case "LST":
                BMixIconPath += "StockLeast";
                break;
            case "BTY":
                BMixIconPath += "Beauty";
                break;
            case "UGY":
                BMixIconPath += "Duckling";
                break;
            case "HVY":
                BMixIconPath += "Heaviest";
                break;
            case "LGT":
                BMixIconPath += "Lightest";
                break;
            case "FLM":
                BMixIconPath += "Ignition";
                break;
            case "PTB":
                BMixIconPath += "ProtectBlunt";
                break;
            case "PTS":
                BMixIconPath += "ProtectSharp";
                break;
            case "PTH":
                BMixIconPath += "ProtectHeat";
                break;
            case "PTE":
                BMixIconPath += "ProtectElectric";
                break;
            case "INH":
                BMixIconPath += "InsulateHeat";
                break;
            case "INC":
                BMixIconPath += "InsulateCold";
                break;
            case "SOF":
                BMixIconPath += "Softness";
                break;
            case "WSP":
                BMixIconPath += "Sharpest";
                break;
            case "WBT":
                BMixIconPath += "Bluntest";
                break;
            case "NUL":
                BMixIconPath += "NutritionLow";
                break;
            case "NUH":
                BMixIconPath += "NutritionHigh";
                break;
            default:
                BMixIconPath += "Nearest";
                break;
        }

        return BMixIconPath;
    }

    public static string GetBMixModeDisplay(string BMixMode)
    {
        string ModeDisplay;
        switch (BMixMode)
        {
            case "DIS":
                ModeDisplay = "BestMix.ModeDistanceDIS".Translate();
                break;
            case "DTR":
                ModeDisplay = "BestMix.ModeDaysToRotDTR".Translate();
                break;
            case "HPT":
                ModeDisplay = "BestMix.ModeHealthHPT".Translate();
                break;
            case "RHP":
                ModeDisplay = "BestMix.ModeHealthRHP".Translate();
                break;
            case "VLC":
                ModeDisplay = "BestMix.ModeValueVLC".Translate();
                break;
            case "VLE":
                ModeDisplay = "BestMix.ModeValueVLE".Translate();
                break;
            case "RND":
                ModeDisplay = "BestMix.ModeRandomRND".Translate();
                break;
            case "TMP":
                ModeDisplay = "BestMix.ModeTemperatureTMP".Translate();
                break;
            case "FRZ":
                ModeDisplay = "BestMix.ModeTemperatureFRZ".Translate();
                break;
            case "BIT":
                ModeDisplay = "BestMix.ModeFractionBIT".Translate();
                break;
            case "MST":
                ModeDisplay = "BestMix.ModeStockMST".Translate();
                break;
            case "LST":
                ModeDisplay = "BestMix.ModeStockLST".Translate();
                break;
            case "BTY":
                ModeDisplay = "BestMix.ModeBeautyBTY".Translate();
                break;
            case "UGY":
                ModeDisplay = "BestMix.ModeBeautyUGY".Translate();
                break;
            case "HVY":
                ModeDisplay = "BestMix.ModeMassHVY".Translate();
                break;
            case "LGT":
                ModeDisplay = "BestMix.ModeMassLGT".Translate();
                break;
            case "FLM":
                ModeDisplay = "BestMix.ModeFlammableFLM".Translate();
                break;
            case "PTB":
                ModeDisplay = "BestMix.ModeProtectPTB".Translate();
                break;
            case "PTS":
                ModeDisplay = "BestMix.ModeProtectPTS".Translate();
                break;
            case "PTH":
                ModeDisplay = "BestMix.ModeProtectPTH".Translate();
                break;
            case "PTE":
                ModeDisplay = "BestMix.ModeProtectPTE".Translate();
                break;
            case "INH":
                ModeDisplay = "BestMix.ModeInsulateINH".Translate();
                break;
            case "INC":
                ModeDisplay = "BestMix.ModeInsulateINC".Translate();
                break;
            case "SOF":
                ModeDisplay = "BestMix.ModeTextileSOF".Translate();
                break;
            case "WSP":
                ModeDisplay = "BestMix.ModeWeaponWSP".Translate();
                break;
            case "WBT":
                ModeDisplay = "BestMix.ModeWeaponWBT".Translate();
                break;
            case "NUL":
                ModeDisplay = "BestMix.ModeFoodNUL".Translate();
                break;
            case "NUH":
                ModeDisplay = "BestMix.ModeFoodNUH".Translate();
                break;
            default:
                ModeDisplay = "BestMix.ModeDistanceDIS".Translate();
                break;
        }

        return ModeDisplay;
    }

    public static Comparison<Thing> GetBMixComparer(Thing billGiver, IntVec3 rootCell, Bill bill, out bool rnd)
    {
        rnd = false;
        var BMixMode = "DIS";

        if (IsValidForComp(billGiver))
        {
            var compBM = billGiver.TryGetComp<CompBestMix>();
            if (compBM != null)
            {
                //BMixMode = compBM.CurMode;
                BMixMode = BMBillUtility.UseBMixMode(compBM, billGiver, bill);
            }
        }

        Comparison<Thing> comparison;
        switch (BMixMode)
        {
            case "DIS":
                comparison = delegate(Thing t1, Thing t2)
                {
                    float num = (t1.Position - rootCell).LengthHorizontalSquared;
                    float value = (t2.Position - rootCell).LengthHorizontalSquared;
                    return num.CompareTo(value);
                };
                break;
            case "DTR":
                comparison = delegate(Thing t1, Thing t2)
                {
                    float maxdtr = 72000000;
                    var t1dtr = maxdtr;
                    var t1comp = t1.TryGetComp<CompRottable>();
                    if (t1comp != null)
                    {
                        t1dtr = t1comp.TicksUntilRotAtCurrentTemp;
                    }

                    var t2dtr = maxdtr;
                    var t2comp = t2.TryGetComp<CompRottable>();
                    if (t2comp != null)
                    {
                        t2dtr = t2comp.TicksUntilRotAtCurrentTemp;
                    }

                    var num = t1dtr;
                    var value = t2dtr;
                    return num.CompareTo(value);
                };
                break;
            case "HPT":
                comparison = delegate(Thing t1, Thing t2)
                {
                    var num = 0f;
                    if (t2.def.useHitPoints)
                    {
                        num = (t2.MaxHitPoints - t2.HitPoints) / (float)Math.Max(1, t2.MaxHitPoints);
                    }

                    var value = 0f;
                    if (t1.def.useHitPoints)
                    {
                        value = (t1.MaxHitPoints - t1.HitPoints) / (float)Math.Max(1, t1.MaxHitPoints);
                    }

                    return num.CompareTo(value);
                };
                break;
            case "RHP":
                comparison = delegate(Thing t1, Thing t2)
                {
                    var num = 0f;
                    if (t2.def.useHitPoints)
                    {
                        num = t2.HitPoints / (float)Math.Max(1, t2.MaxHitPoints);
                    }

                    var value = 0f;
                    if (t1.def.useHitPoints)
                    {
                        value = t1.HitPoints / (float)Math.Max(1, t1.MaxHitPoints);
                    }

                    return num.CompareTo(value);
                };
                break;
            case "VLC":
                comparison = delegate(Thing t1, Thing t2)
                {
                    var num = 0f - t2.MarketValue;
                    var value = 0f - t1.MarketValue;
                    return num.CompareTo(value);
                };
                break;
            case "VLE":
                comparison = delegate(Thing t1, Thing t2)
                {
                    var num = t2.MarketValue;
                    var value = t1.MarketValue;
                    return num.CompareTo(value);
                };
                break;
            case "TMP":
                comparison = delegate(Thing t1, Thing t2)
                {
                    var num = t2.AmbientTemperature;
                    var value = t1.AmbientTemperature;
                    return num.CompareTo(value);
                };
                break;
            case "FRZ":
                comparison = delegate(Thing t1, Thing t2)
                {
                    var num = 0f - t2.AmbientTemperature;
                    var value = 0f - t1.AmbientTemperature;
                    return num.CompareTo(value);
                };
                break;
            case "BIT":
                comparison = delegate(Thing t1, Thing t2)
                {
                    var num = t2.def.stackLimit / (float)Math.Max(1, t2.stackCount);
                    var value = t1.def.stackLimit / (float)Math.Max(1, t1.stackCount);
                    return num.CompareTo(value);
                };
                break;
            case "MST":
                comparison = delegate(Thing t1, Thing t2)
                {
                    var thing2Amount = GetStockAmount(t2, billGiver, bill);
                    var thing1Amount = GetStockAmount(t1, billGiver, bill);
                    return thing2Amount.CompareTo(thing1Amount);
                };
                break;
            case "LST":
                comparison = delegate(Thing t1, Thing t2)
                {
                    var thing1Amount = GetStockAmount(t1, billGiver, bill);
                    var thing2Amount = GetStockAmount(t2, billGiver, bill);
                    return thing1Amount.CompareTo(thing2Amount);
                };
                break;
            case "RND":
                comparison = delegate
                {
                    var num = RNDFloat();
                    var value = RNDFloat();
                    return num.CompareTo(value);
                };
                rnd = true;
                break;
            case "BTY":
                comparison = delegate(Thing t1, Thing t2)
                {
                    var thing1Beauty = t1.GetStatValue(StatDefOf.Beauty);
                    var thing2Beauty = t2.GetStatValue(StatDefOf.Beauty);

                    if (t1.def?.stuffProps?.statFactors != null)
                    {
                        if (t1.def.stuffProps.statFactors.StatListContains(StatDefOf.Beauty))
                        {
                            thing1Beauty += t1.def.stuffProps.statFactors.GetStatOffsetFromList(StatDefOf.Beauty);
                        }
                    }

                    if (t2.def?.stuffProps?.statFactors == null)
                    {
                        return thing2Beauty.CompareTo(thing1Beauty);
                    }

                    if (t2.def.stuffProps.statFactors.StatListContains(StatDefOf.Beauty))
                    {
                        thing2Beauty += t2.def.stuffProps.statFactors.GetStatOffsetFromList(StatDefOf.Beauty);
                    }

                    return thing2Beauty.CompareTo(thing1Beauty);
                };
                break;
            case "UGY":
                comparison = delegate(Thing t1, Thing t2)
                {
                    var thing2Beauty = t2.GetStatValue(StatDefOf.Beauty);
                    if (t2.def?.stuffProps?.statFactors != null)
                    {
                        if (t2.def.stuffProps.statFactors.StatListContains(StatDefOf.Beauty))
                        {
                            thing2Beauty += t2.def.stuffProps.statFactors.GetStatOffsetFromList(StatDefOf.Beauty);
                        }
                    }

                    var thing1Beauty = t1.GetStatValue(StatDefOf.Beauty);
                    if (t1.def?.stuffProps?.statFactors == null)
                    {
                        return thing1Beauty.CompareTo(thing2Beauty);
                    }

                    if (t1.def.stuffProps.statFactors.StatListContains(StatDefOf.Beauty))
                    {
                        thing1Beauty += t1.def.stuffProps.statFactors.GetStatOffsetFromList(StatDefOf.Beauty);
                    }

                    return thing1Beauty.CompareTo(thing2Beauty);
                };
                break;
            case "HVY":
                comparison = delegate(Thing t1, Thing t2)
                {
                    var num = t2.GetStatValue(StatDefOf.Mass);
                    var value = t1.GetStatValue(StatDefOf.Mass);
                    return num.CompareTo(value);
                };
                break;
            case "LGT":
                comparison = delegate(Thing t1, Thing t2)
                {
                    var num = 0f - t2.GetStatValue(StatDefOf.Mass);
                    var value = 0f - t1.GetStatValue(StatDefOf.Mass);
                    return num.CompareTo(value);
                };
                break;
            case "FLM":
                comparison = delegate(Thing t1, Thing t2)
                {
                    var num = t2.GetStatValue(StatDefOf.Flammability);
                    var value = t1.GetStatValue(StatDefOf.Flammability);
                    return num.CompareTo(value);
                };
                break;
            case "PTB":
                comparison = delegate(Thing t1, Thing t2)
                {
                    var num = t2.GetStatValue(StatDefOf.StuffPower_Armor_Blunt);
                    var value = t1.GetStatValue(StatDefOf.StuffPower_Armor_Blunt);
                    return num.CompareTo(value);
                };
                break;
            case "PTS":
                comparison = delegate(Thing t1, Thing t2)
                {
                    var num = t2.GetStatValue(StatDefOf.StuffPower_Armor_Sharp);
                    var value = t1.GetStatValue(StatDefOf.StuffPower_Armor_Sharp);
                    return num.CompareTo(value);
                };
                break;
            case "PTH":
                comparison = delegate(Thing t1, Thing t2)
                {
                    var num = t2.GetStatValue(StatDefOf.StuffPower_Armor_Heat);
                    var value = t1.GetStatValue(StatDefOf.StuffPower_Armor_Heat);
                    return num.CompareTo(value);
                };
                break;
            case "PTE":
                comparison = delegate(Thing t1, Thing t2)
                {
                    var num = 0f;
                    var value = 0f;
                    if (protElectric == null)
                    {
                        return num.CompareTo(value);
                    }

                    num = t2.GetStatValue(protElectric);
                    value = t1.GetStatValue(protElectric);

                    return num.CompareTo(value);
                };
                break;
            case "INH":
                comparison = delegate(Thing t1, Thing t2)
                {
                    var num = t2.GetStatValue(StatDefOf.StuffPower_Insulation_Heat);
                    var value = t1.GetStatValue(StatDefOf.StuffPower_Insulation_Heat);
                    return num.CompareTo(value);
                };
                break;
            case "INC":
                comparison = delegate(Thing t1, Thing t2)
                {
                    var num = t2.GetStatValue(StatDefOf.StuffPower_Insulation_Cold);
                    var value = t1.GetStatValue(StatDefOf.StuffPower_Insulation_Cold);
                    return num.CompareTo(value);
                };
                break;
            case "SOF":
                comparison = delegate(Thing t1, Thing t2)
                {
                    var num = 0f;
                    var value = 0f;
                    if (softness == null)
                    {
                        return num.CompareTo(value);
                    }

                    num = t2.GetStatValue(softness);
                    value = t1.GetStatValue(softness);

                    return num.CompareTo(value);
                };
                break;
            case "WSP":
                comparison = delegate(Thing t1, Thing t2)
                {
                    var num = t2.GetStatValue(StatDefOf.SharpDamageMultiplier);
                    var value = t1.GetStatValue(StatDefOf.SharpDamageMultiplier);
                    return num.CompareTo(value);
                };
                break;
            case "WBT":
                comparison = delegate(Thing t1, Thing t2)
                {
                    var num = t2.GetStatValue(StatDefOf.BluntDamageMultiplier);
                    var value = t1.GetStatValue(StatDefOf.BluntDamageMultiplier);
                    return num.CompareTo(value);
                };
                break;
            case "NUL":
                comparison = delegate(Thing t1, Thing t2)
                {
                    var num = 0f - t2.GetStatValue(StatDefOf.Nutrition);
                    var value = 0f - t1.GetStatValue(StatDefOf.Nutrition);
                    return num.CompareTo(value);
                };
                break;
            case "NUH":
                comparison = delegate(Thing t1, Thing t2)
                {
                    var num = t2.GetStatValue(StatDefOf.Nutrition);
                    var value = t1.GetStatValue(StatDefOf.Nutrition);
                    return num.CompareTo(value);
                };
                break;
            default:
                comparison = delegate(Thing t1, Thing t2)
                {
                    float num = (t1.Position - rootCell).LengthHorizontalSquared;
                    float value = (t2.Position - rootCell).LengthHorizontalSquared;
                    return num.CompareTo(value);
                };
                break;
        }

        return comparison;
    }

    private static float GetStockAmount(Thing t, Thing billGiver, Bill bill)
    {
        var num = 0f;

        var things = t.Map.listerThings.ThingsOfDef(t.def);
        if (things.Count <= 0)
        {
            return num;
        }

        foreach (var thing in things)
        {
            if (!BMStockInRadius(thing, billGiver, bill))
            {
                continue;
            }

            if (Controller.Settings.inStorage)
            {
                if (!thing.IsInValidStorage())
                {
                    continue;
                }
            }

            if (BMIsForbidden(thing))
            {
                continue;
            }

            if (thing.IsBurning() || thing.Fogged())
            {
                continue;
            }

            num += thing.stackCount;
        }

        return num;
    }

    public static void Sort(List<Thing> availableThings, IntVec3 rootCell, Bill bill = null)
    {
        if (WorkGiver_DoBill_TryFindBestBillIngredients.curGiver == null)
        {
            return;
        }

        var comparison = GetBMixComparer(WorkGiver_DoBill_TryFindBestBillIngredients.curGiver, rootCell, bill,
            out var rnd);
        if (rnd)
        {
            availableThings.Shuffle();
        }
        else
        {
            availableThings.Sort(comparison);
        }
    }

    // Debug
    internal static void BMixDebugList(List<Thing> list, Thing billGiver, IntVec3 rootCell, Bill bill)
    {
#if !DEBUG
        if (!Prefs.DevMode || !Controller.Settings.DebugMaster || !Controller.Settings.DebugSort)
        {
            return;
        }
#endif
        if (!IsValidForComp(billGiver))
        {
            return;
        }

        var compBMix = billGiver.TryGetComp<CompBestMix>();
        if (compBMix == null)
        {
            return;
        }

        if (!compBMix.BMixDebug)
        {
            return;
        }

        if (list.Count <= 0)
        {
            return;
        }

        for (var i = 0; i < list.Count; i++)
        {
            var thing = list[i];
            var debugMsg = MakeDebugString(i, thing, billGiver, rootCell, bill,
                BMBillUtility.UseBMixMode(compBMix, billGiver, bill));
            Log.Message(debugMsg);
        }
    }

    internal static void DebugChosenList(Thing billGiver, List<ThingCount> p_chosen)
    {
#if !DEBUG
        if (!Prefs.DevMode || !Controller.Settings.DebugMaster || !Controller.Settings.DebugChosen)
        {
            return;
        }
#endif
        if (!IsValidForComp(billGiver))
        {
            return;
        }

        var compBestMix = billGiver.TryGetComp<CompBestMix>();
        if (compBestMix == null || !compBestMix.BMixDebug)
        {
            return;
        }

        if (p_chosen != null)
        {
            if (p_chosen.Count > 0)
            {
                Log.Message(billGiver.Label + ", " + "BestMix.Chosen".Translate() + ":");
                foreach (var TC in p_chosen)
                {
                    var thing = TC.Thing;
                    var count = TC.Count;
                    Log.Message(thing.Label + " , " + count);
                }
            }
            else
            {
                Log.Message(billGiver.Label + ", " + "BestMix.NoChosen".Translate() + ":");
            }
        }
        else
        {
            Log.Message(billGiver.Label + ", " + "BestMix.NullChosen".Translate() + ":");
        }
    }

    internal static void DebugFoundAll(Thing billGiver, bool foundAll)
    {
#if !DEBUG
        if (!Prefs.DevMode || !Controller.Settings.DebugMaster || !Controller.Settings.DebugFound)
        {
            return;
        }
#endif

        if (!IsValidForComp(billGiver))
        {
#if DEBUG
                Log.Message($"Not IsValidForComp {billGiver}");
#endif
            return;
        }

        var compBMix = billGiver.TryGetComp<CompBestMix>();
        if (compBMix == null)
        {
#if DEBUG
                Log.Message($"No compBMix for {billGiver}");
#endif
            return;
        }

        if (!compBMix.BMixDebug)
        {
#if DEBUG
                Log.Message($"Not compBMix.BMixDebug {billGiver}");
#endif
            return;
        }

        var debugMsg = billGiver.Label;
        if (foundAll)
        {
            debugMsg += ", " + "BestMix.FoundAll".Translate();
        }
        else
        {
            debugMsg += ", " + "BestMix.NotFoundAll".Translate();
        }

        Log.Message(debugMsg);
    }

    private static string MakeDebugString(int indx, Thing thing, Thing billGiver, IntVec3 rootCell, Bill bill,
        string BMixMode)
    {
        var stat = 0f;

        switch (BMixMode)
        {
            case "DIS":
                stat = (thing.Position - rootCell).LengthHorizontalSquared;
                break;
            case "DTR":
                float maxdtr = 72000000;
                var thingdtr = maxdtr;
                var thingcomp = thing.TryGetComp<CompRottable>();
                if (thingcomp != null)
                {
                    thingdtr = thingcomp.TicksUntilRotAtCurrentTemp;
                }

                stat = maxdtr - thingdtr;
                break;
            case "HPT":
                stat = 0f;
                if (thing.def.useHitPoints)
                {
                    stat = (thing.MaxHitPoints - thing.HitPoints) / (float)Math.Max(1, thing.MaxHitPoints);
                }

                break;
            case "RHP":
                stat = 0f;
                if (thing.def.useHitPoints)
                {
                    stat = thing.HitPoints / (float)Math.Max(1, thing.MaxHitPoints);
                }

                break;
            case "VLC":
                stat = 0f - thing.MarketValue;
                break;
            case "VLE":
                stat = thing.MarketValue;
                break;
            case "TMP":
                stat = thing.AmbientTemperature;
                break;
            case "FRZ":
                stat = 0f - thing.AmbientTemperature;
                break;
            case "BIT":
                stat = thing.def.stackLimit / (float)Math.Max(1, thing.stackCount);
                break;
            case "MST":
                stat = GetStockAmount(thing, billGiver, bill);
                break;
            case "LST":
                stat = 0f - GetStockAmount(thing, billGiver, bill);
                break;
            case "RND":
                stat = RNDFloat();
                break;
            case "BTY":
                stat = thing.GetStatValue(StatDefOf.Beauty);
                if (thing.def?.stuffProps?.statOffsets != null)
                {
                    if (thing.def.stuffProps.statOffsets.StatListContains(StatDefOf.Beauty))
                    {
                        stat += thing.def.stuffProps.statOffsets.GetStatOffsetFromList(StatDefOf.Beauty);
                    }
                }

                break;
            case "UGY":
                stat = 0f - thing.GetStatValue(StatDefOf.Beauty);
                if (thing.def?.stuffProps?.statOffsets != null)
                {
                    if (thing.def.stuffProps.statOffsets.StatListContains(StatDefOf.Beauty))
                    {
                        stat -= thing.def.stuffProps.statOffsets.GetStatOffsetFromList(StatDefOf.Beauty);
                    }
                }

                break;
            case "HVY":
                stat = thing.GetStatValue(StatDefOf.Mass);
                break;
            case "LGT":
                stat = 0f - thing.GetStatValue(StatDefOf.Mass);
                break;
            case "FLM":
                stat = thing.GetStatValue(StatDefOf.Flammability);
                break;
            case "PTB":
                stat = thing.GetStatValue(StatDefOf.StuffPower_Armor_Blunt);
                break;
            case "PTS":
                stat = thing.GetStatValue(StatDefOf.StuffPower_Armor_Sharp);
                break;
            case "PTH":
                stat = thing.GetStatValue(StatDefOf.StuffPower_Armor_Heat);
                break;
            case "PTE":
                if (protElectric != null)
                {
                    stat = thing.GetStatValue(protElectric);
                }

                break;
            case "INH":
                stat = thing.GetStatValue(StatDefOf.StuffPower_Insulation_Heat);
                break;
            case "INC":
                stat = thing.GetStatValue(StatDefOf.StuffPower_Insulation_Cold);
                break;
            case "SOF":
                if (softness != null)
                {
                    stat = thing.GetStatValue(softness);
                }

                break;
            case "WSP":
                stat = thing.GetStatValue(StatDefOf.SharpDamageMultiplier);
                break;
            case "WBT":
                stat = thing.GetStatValue(StatDefOf.BluntDamageMultiplier);
                break;
            case "NUL":
                stat = 0f - thing.GetStatValue(StatDefOf.Nutrition);
                break;
            case "NUH":
                stat = thing.GetStatValue(StatDefOf.Nutrition);
                break;
            default:
                stat = 0f;
                break;
        }

        var debugPos = "(" + thing.Position.x + ", " + thing.Position.z + ")";
        var debugMsg = "Debug " + BMixMode + " " + indx + " " + billGiver.ThingID + " " + thing.LabelShort + " " +
                       debugPos + " " + stat.ToString("F2");
        return debugMsg;
    }

    public static bool BMIsForbidden(Thing thing)
    {
        if (thing?.TryGetComp<CompForbiddable>() == null)
        {
            return false;
        }

        if (thing.TryGetComp<CompForbiddable>().Forbidden)
        {
            return true;
        }

        return false;
    }

    private static bool BMStockInRadius(Thing t, Thing billGiver, Bill bill)
    {
        if (Controller.Settings.mapStock)
        {
            return true;
        }

        return (t.Position - billGiver.Position).LengthHorizontalSquared <
               bill.ingredientSearchRadius * bill.ingredientSearchRadius;
    }

    public static Predicate<Thing> BestMixValidator(Pawn pawn, Thing billGiver, Bill bill)
    {
        bool Validator(Thing t)
        {
            return t.Spawned && !t.IsForbidden(pawn)
                             && (t.Position - billGiver.Position).LengthHorizontalSquared <
                             bill.ingredientSearchRadius * bill.ingredientSearchRadius
                             && bill.IsFixedOrAllowedIngredient(t) &&
                             bill.recipe.ingredients.Any(ingNeed => ingNeed.filter.Allows(t))
                             && pawn.CanReserve(t);
        }

        return Validator;
    }

    private class BMixDefCountList
    {
        private readonly List<float> counts = new List<float>();
        private readonly List<ThingDef> defs = new List<ThingDef>();

        public int Count => defs.Count;

        private float this[ThingDef def]
        {
            get
            {
                var num = defs.IndexOf(def);
                if (num < 0)
                {
                    return 0f;
                }

                return counts[num];
            }
            set
            {
                var num = defs.IndexOf(def);
                if (num < 0)
                {
                    defs.Add(def);
                    counts.Add(value);
                    num = defs.Count - 1;
                }
                else
                {
                    counts[num] = value;
                }

                CheckRemove(num);
            }
        }

        public float GetCount(int index)
        {
            return counts[index];
        }

        public void SetCount(int index, float val)
        {
            counts[index] = val;
            CheckRemove(index);
        }

        public ThingDef GetDef(int index)
        {
            return defs[index];
        }

        private void CheckRemove(int index)
        {
            if (counts[index] != 0f)
            {
                return;
            }

            counts.RemoveAt(index);
            defs.RemoveAt(index);
        }

        public void Clear()
        {
            defs.Clear();
            counts.Clear();
        }

        public void GenerateFrom(List<Thing> things)
        {
            Clear();
            foreach (var thing in things)
            {
                this[thing.def] += thing.stackCount;
            }
        }
    }
}
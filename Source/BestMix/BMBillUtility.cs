using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;

namespace BestMix;

public class BMBillUtility
{
    public static string UseBMixMode(CompBestMix compBM, Thing billGiver, Bill bill)
    {
        var mode = "DIS";
        if (compBM == null)
        {
            return mode;
        }

        mode = compBM.CurMode; // defaults to bench Gizmo
        if (compBM.BillBMModes == null)
        {
            return mode;
        }

        var billModeListing = compBM.BillBMModes;
        if (billModeListing.Count <= 0)
        {
            return mode;
        }

        foreach (var billMode in billModeListing)
        {
            if (billValuePart(billMode) != bill.GetUniqueLoadID())
            {
                continue;
            }

            mode = modeValuePart(billMode);
            if (mode == "NON")
            {
                mode = compBM.CurMode;
            }

            break;
        }

        return mode;
    }

    public static Texture2D GetBillBmTex(Thing billGiver, Bill bill)
    {
        var mode = GetBillBMMode(billGiver, bill);
        var texPath = "UI/BestMix/NONIcon";

        if (mode != "NON")
        {
            texPath = BestMixUtility.GetBMixIconPath(mode);
        }

        texPath += "Bill";

        var tex = ContentFinder<Texture2D>.Get(texPath, false);
        return tex;
    }

    private static string GetBillBMMode(Thing billGiver, Bill bill)
    {
        var mode = "NON";
        if (billGiver is null or Pawn)
        {
            return mode;
        }

        var cbm = billGiver.TryGetComp<CompBestMix>();
        if (cbm == null)
        {
            return mode;
        }

        var billID = bill?.GetUniqueLoadID();
        if (billID == null || cbm.BillBMModes == null)
        {
            return mode;
        }

        var billModes = cbm.BillBMModes;
        if (billModes.Count <= 0)
        {
            return mode;
        }

        foreach (var billMode in billModes)
        {
            if (billValuePart(billMode) != billID)
            {
                continue;
            }

            mode = modeValuePart(billMode);
            break;
        }

        return mode;
    }

    public static void SetBillBmVal(Thing billGiver, Bill bill)
    {
        var cbm = billGiver?.TryGetComp<CompBestMix>();
        if (cbm != null)
        {
            doBillModeSelMenu(cbm, bill);
        }
    }

    private static void doBillModeSelMenu(CompBestMix cbm, Bill bill)
    {
        var list = new List<FloatMenuOption>();

        string text = "BestMix.DoNothing".Translate();
        list.Add(new FloatMenuOption(text, delegate { SetBMixBillMode(cbm, bill, "NON", true); },
            MenuOptionPriority.Default, null, null, 29f));

        foreach (var mode in BestMixUtility.BMixModes())
        {
            text = BestMixUtility.GetBMixModeDisplay(mode);
            list.Add(new FloatMenuOption(text, delegate { SetBMixBillMode(cbm, bill, mode, true); },
                MenuOptionPriority.Default, null, null, 29f));
        }

        var sortedlist = list.OrderBy(bm => bm.Label).ToList();
        Find.WindowStack.Add(new FloatMenu(sortedlist));
    }

    public static void SetBMixBillMode(CompBestMix cbm, Bill bill, string mode, bool set)
    {
        if (cbm == null || bill == null)
        {
            return;
        }

        var billID = bill.GetUniqueLoadID();
        var newList = new List<string>();
        if (cbm.BillBMModes != null)
        {
            var current = cbm.BillBMModes;
            if (current.Count > 0)
            {
                foreach (var BillBMMode in current)
                {
                    if (billValuePart(BillBMMode) != billID)
                    {
                        newList.AddDistinct(BillBMMode);
                    }
                }
            }

            current.Clear();
        }

        newList.AddDistinct(newBillBmMode(billID, mode));

        cbm.BillBMModes = newList;
    }

    public static void CheckBillBmValues(CompBestMix cbm, Thing billGiver, List<string> billModes)
    {
        if (billModes != null)
        {
            if (billModes.Count <= 0)
            {
                return;
            }

            var newBillModes = new List<string>();
            var billIDs = new List<string>();
            var billStack = (billGiver as IBillGiver)?.BillStack;
            if (billStack != null)
            {
                var bills = billStack.Bills;
                if (bills.Count > 0)
                {
                    foreach (var bill in bills)
                    {
                        var id = bill?.GetUniqueLoadID();
                        if (id != null)
                        {
                            billIDs.AddDistinct(id);
                        }
                    }
                }
            }

            foreach (var billMode in billModes)
            {
                if (billIDs.Contains(billValuePart(billMode)))
                {
                    newBillModes.AddDistinct(billMode);
                }
            }

            cbm.BillBMModes = newBillModes;
        }
        else
        {
            cbm.BillBMModes = [];
        }
    }

    private static string newBillBmMode(string id, string mode)
    {
        return $"{id};{mode}";
    }

    private static string billValuePart(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return string.Empty;
        }

        char[] divider = [';'];
        var segments = value.Split(divider);
        return segments[0];
    }

    private static string modeValuePart(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return "NON";
        }

        char[] divider = [';'];
        var segments = value.Split(divider);
        return segments.Length > 1 ? segments[1] : "NON";
    }
}
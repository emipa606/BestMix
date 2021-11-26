using UnityEngine;
using Verse;

namespace BestMix;

public class Settings : ModSettings
{
    private readonly bool adjBillBMPos = false; // not saved

    public readonly bool DebugMaster = false; // not saved
    private readonly bool RadiusRestrict = false; // not saved

    public readonly bool useStock = true; // not saved

    public bool AllowBestMix = true;
    public bool AllowBMBillMaxSet = true;
    public bool AllowMealMakersOnly;
    private float BillBMPos = 150f; // not saved
    public bool DebugChosen;
    public bool DebugFound;
    public bool DebugIgnore;
    public bool DebugSort;

    public bool IncludeRegionLimiter = true;
    public bool inStorage = true;
    public bool mapStock;
    public int RadiusLimit = 100;
    public bool UseRadiusLimit;

    public void DoWindowContents(Rect canvas)
    {
        var gap = 10f;
        var listing_Standard = new Listing_Standard
        {
            ColumnWidth = canvas.width
        };
        listing_Standard.Begin(canvas);
        listing_Standard.Gap(gap);
        checked
        {
            listing_Standard.CheckboxLabeled("BestMix.AllowBestMix".Translate(), ref AllowBestMix);
            listing_Standard.Gap(gap);
            listing_Standard.CheckboxLabeled("BestMix.AllowMealMakersOnly".Translate(), ref AllowMealMakersOnly);
            listing_Standard.Gap(gap * 2f);

            // Max Bills
            /*
            if ((Prefs.DevMode) && (DebugMaster))
            {
                listing_Standard.CheckboxLabeled("BestMix.AllowBMBillMaxSet".Translate(), ref AllowBMBillMaxSet, null);
                listing_Standard.Gap(gap);
            }
            */

            // Stock comparison (performance considerations)

            if (useStock)
            {
                listing_Standard.CheckboxLabeled("BestMix.MapStock".Translate(), ref mapStock);
                listing_Standard.Gap(gap);
                listing_Standard.CheckboxLabeled("BestMix.InStorage".Translate(), ref inStorage);
                listing_Standard.Gap(gap);
            }

            listing_Standard.Gap(gap);

            if (adjBillBMPos)
            {
                listing_Standard.Label("BestMix.BillBMPos".Translate() + "  " + (int)BillBMPos);
                BillBMPos = (int)listing_Standard.Slider((int)BillBMPos, 150f, 200f);
                listing_Standard.Gap(gap);
            }

            listing_Standard.Gap(gap);
            // if restrict by radius
            if (RadiusRestrict)
            {
                listing_Standard.CheckboxLabeled("BestMix.UseRadiusLimit".Translate(), ref UseRadiusLimit);
                listing_Standard.Gap(gap);
                listing_Standard.Label("BestMix.RadiusLimit".Translate() + "  " + RadiusLimit);
                RadiusLimit = (int)listing_Standard.Slider(RadiusLimit, 10f, 100f);
                listing_Standard.Gap(gap);
            }

            // debug
            if (Prefs.DevMode && DebugMaster)
            {
                listing_Standard.Gap(gap * 2);
                listing_Standard.CheckboxLabeled("BestMix.IncludeRegionLimiter".Translate(),
                    ref IncludeRegionLimiter);
                listing_Standard.Gap(gap * 2);
                listing_Standard.CheckboxLabeled("BestMix.DebugSort".Translate(), ref DebugSort);
                listing_Standard.Gap(gap);
                listing_Standard.CheckboxLabeled("BestMix.DebugChosen".Translate(), ref DebugChosen);
                listing_Standard.Gap(gap);
                listing_Standard.CheckboxLabeled("BestMix.DebugFound".Translate(), ref DebugFound);
                listing_Standard.Gap(gap * 2);
                listing_Standard.CheckboxLabeled("BestMix.DebugIgnore".Translate(), ref DebugIgnore);
                listing_Standard.Gap(gap);
            }

            listing_Standard.End();
        }
    }

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Values.Look(ref AllowBestMix, "AllowBestMix", true);
        Scribe_Values.Look(ref AllowMealMakersOnly, "AllowMealMakersOnly");
        Scribe_Values.Look(ref AllowBMBillMaxSet, "AllowDMBillMaxSet", true);
        Scribe_Values.Look(ref mapStock, "mapStock");
        Scribe_Values.Look(ref inStorage, "inStorage", true);
        Scribe_Values.Look(ref UseRadiusLimit, "UseRadiusLimit");
        Scribe_Values.Look(ref RadiusLimit, "RadiusLimit", 100);
        Scribe_Values.Look(ref IncludeRegionLimiter, "IncludeRegionLimiter", true);
        Scribe_Values.Look(ref DebugSort, "DebugSort");
        Scribe_Values.Look(ref DebugChosen, "DebugChosen");
        Scribe_Values.Look(ref DebugFound, "DebugFound");
        Scribe_Values.Look(ref DebugIgnore, "DebugIgnore");
    }
}
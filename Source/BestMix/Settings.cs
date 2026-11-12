using UnityEngine;
using Verse;

namespace BestMix;

public class Settings : ModSettings
{
    public readonly bool DebugMaster = false; // not saved
    private readonly bool RadiusRestrict = false; // not saved
    private readonly bool adjBillBMPos = false; // not saved

    public readonly bool useStock = true; // not saved
    public bool AllowBMBillMaxSet = true;

    public bool AllowBestMix = true;
    public bool AllowMealMakersOnly;
    private float BillBMPos = 150f; // not saved
    public bool DebugChosen;
    public bool DebugFound;
    private bool DebugIgnore;
    public bool DebugSort;

    public bool IncludeRegionLimiter = true;
    public int RadiusLimit = 100;
    public bool UseRadiusLimit;
    public bool inStorage = true;
    public bool mapStock;

    public void DoWindowContents(Rect canvas)
    {
        const float gap = 10f;
        var listingStandard = new Listing_Standard
        {
            ColumnWidth = canvas.width
        };
        listingStandard.Begin(canvas);
        listingStandard.Gap(gap);
        checked
        {
            listingStandard.CheckboxLabeled("BestMix.AllowBestMix".Translate(), ref AllowBestMix);
            listingStandard.Gap(gap);
            listingStandard.CheckboxLabeled("BestMix.AllowMealMakersOnly".Translate(), ref AllowMealMakersOnly);
            listingStandard.Gap(gap * 2f);

            if (useStock)
            {
                listingStandard.CheckboxLabeled("BestMix.MapStock".Translate(), ref mapStock);
                listingStandard.Gap(gap);
                listingStandard.CheckboxLabeled("BestMix.InStorage".Translate(), ref inStorage);
                listingStandard.Gap(gap);
            }

            listingStandard.Gap(gap);

            if (adjBillBMPos)
            {
                listingStandard.Label("BestMix.BillBMPos".Translate() + "  " + (int)BillBMPos);
                BillBMPos = (int)listingStandard.Slider((int)BillBMPos, 150f, 200f);
                listingStandard.Gap(gap);
            }

            listingStandard.Gap(gap);
            // if restrict by radius
            if (RadiusRestrict)
            {
                listingStandard.CheckboxLabeled("BestMix.UseRadiusLimit".Translate(), ref UseRadiusLimit);
                listingStandard.Gap(gap);
                listingStandard.Label("BestMix.RadiusLimit".Translate() + "  " + RadiusLimit);
                RadiusLimit = (int)listingStandard.Slider(RadiusLimit, 10f, 100f);
                listingStandard.Gap(gap);
            }

            // debug
            if (Prefs.DevMode && DebugMaster)
            {
                listingStandard.Gap(gap * 2);
                listingStandard.CheckboxLabeled("BestMix.IncludeRegionLimiter".Translate(),
                    ref IncludeRegionLimiter);
                listingStandard.Gap(gap * 2);
                listingStandard.CheckboxLabeled("BestMix.DebugSort".Translate(), ref DebugSort);
                listingStandard.Gap(gap);
                listingStandard.CheckboxLabeled("BestMix.DebugChosen".Translate(), ref DebugChosen);
                listingStandard.Gap(gap);
                listingStandard.CheckboxLabeled("BestMix.DebugFound".Translate(), ref DebugFound);
                listingStandard.Gap(gap * 2);
                listingStandard.CheckboxLabeled("BestMix.DebugIgnore".Translate(), ref DebugIgnore);
                listingStandard.Gap(gap);
            }

            if (Controller.CurrentVersion != null)
            {
                listingStandard.Gap();
                GUI.contentColor = Color.gray;
                listingStandard.Label("BestMix.VersionInfo".Translate(Controller.CurrentVersion));
                GUI.contentColor = Color.white;
            }

            listingStandard.End();
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
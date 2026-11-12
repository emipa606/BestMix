using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace BestMix;

public class CompBestMix : ThingComp
{
    public bool BMixDebug;
    public List<string> BillBMModes = [];
    public string CurMode;

    private CompProperties_BestMix BmProps => (CompProperties_BestMix)props;

    public override void PostExposeData()
    {
        base.PostExposeData();
        Scribe_Values.Look(ref CurMode, "CurMode", "DIS");
        Scribe_Values.Look(ref BMixDebug, "BMixDebug");
        Scribe_Collections.Look(ref BillBMModes, "BillBMModes", LookMode.Value);
    }

    public override void PostSpawnSetup(bool respawningAfterLoad)
    {
        base.PostSpawnSetup(respawningAfterLoad);

        CurMode ??= BmProps.DefaultMode;

        if (respawningAfterLoad)
        {
            BMBillUtility.CheckBillBmValues(this, parent, BillBMModes);
        }
    }

    public override string CompInspectStringExtra()
    {
        if (!BestMixUtility.IsValidForComp(parent))
        {
            return null;
        }

        var modeDisplay = BestMixUtility.GetBMixModeDisplay(CurMode);
        return "BestMix.CurrentMode".Translate(modeDisplay);
    }

    public override IEnumerable<Gizmo> CompGetGizmosExtra()
    {
        foreach (var item in base.CompGetGizmosExtra())
        {
            yield return item;
        }

        if (!BestMixUtility.IsValidForComp(parent))
        {
            yield break;
        }

        if (!parent.Spawned || parent.Faction != Faction.OfPlayer)
        {
            yield break;
        }

        var bMixIconPath = BestMixUtility.GetBMixIconPath(CurMode);
        yield return new Command_Action
        {
            action = delegate
            {
                SoundDefOf.Tick_Tiny.PlayOneShotOnCamera();
                doModeSelMenu();
            },
            hotKey = KeyBindingDefOf.Misc1,
            defaultLabel = "BestMix.SelectModeLabel".Translate(),
            defaultDesc = "BestMix.SelectModeDesc".Translate(),
            icon = ContentFinder<Texture2D>.Get(bMixIconPath)
        };
        if (!Prefs.DevMode || !Controller.Settings.DebugMaster)
        {
            yield break;
        }

        const string debugIconPath = "UI/BestMix/DebugList";
        yield return new Command_Toggle
        {
            icon = ContentFinder<Texture2D>.Get(debugIconPath),
            defaultLabel = "BestMix.DebugLabel".Translate(),
            defaultDesc = "BestMix.DebugDesc".Translate(),
            isActive = () => BMixDebug,
            toggleAction = delegate { toggleDebug(BMixDebug); }
        };
    }

    private void toggleDebug(bool flag)
    {
        BMixDebug = !flag;
    }

    private void doModeSelMenu()
    {
        var list = new List<FloatMenuOption>();

        string text = "BestMix.DoNothing".Translate();
        var icon = ContentFinder<Texture2D>.Get(BestMixUtility.GetBMixIconPath("Nothing"));
        list.Add(new FloatMenuOption(text, delegate { SetBMixMode(this, "DIS", true); }, icon, Color.white,
            MenuOptionPriority.Default, null, null, 29f));

        foreach (var mode in BestMixUtility.BMixModes())
        {
            text = BestMixUtility.GetBMixModeDisplay(mode);
            icon = ContentFinder<Texture2D>.Get(BestMixUtility.GetBMixIconPath(mode));
            list.Add(new FloatMenuOption(text, delegate { SetBMixMode(this, mode, true); }, icon, Color.white,
                MenuOptionPriority.Default, null, null, 29f));
        }

        var sortedlist = list.OrderBy(bm => bm.Label).ToList();
        Find.WindowStack.Add(new FloatMenu(sortedlist));
    }

    public void SetBMixMode(CompBestMix cbm, string gizmoSel, bool edit)
    {
        if (edit)
        {
            cbm.CurMode = gizmoSel;
        }
    }
}
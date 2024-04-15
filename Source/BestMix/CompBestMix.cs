using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace BestMix;

public class CompBestMix : ThingComp
{
    public List<string> BillBMModes = [];
    public bool BMixDebug;
    public string CurMode;

    private CompProperties_BestMix BMProps => (CompProperties_BestMix)props;

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

        if (CurMode == null)
        {
            CurMode = BMProps.DefaultMode;
        }

        if (respawningAfterLoad)
        {
            BMBillUtility.CheckBillBMValues(this, parent, BillBMModes);
        }
    }

    public override string CompInspectStringExtra()
    {
        if (!BestMixUtility.IsValidForComp(parent))
        {
            return null;
        }

        var ModeDisplay = BestMixUtility.GetBMixModeDisplay(CurMode);
        return "BestMix.CurrentMode".Translate(ModeDisplay);
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

        var BMixIconPath = BestMixUtility.GetBMixIconPath(CurMode);
        yield return new Command_Action
        {
            action = delegate
            {
                SoundDefOf.Tick_Tiny.PlayOneShotOnCamera();
                DoModeSelMenu();
            },
            hotKey = KeyBindingDefOf.Misc1,
            defaultLabel = "BestMix.SelectModeLabel".Translate(),
            defaultDesc = "BestMix.SelectModeDesc".Translate(),
            icon = ContentFinder<Texture2D>.Get(BMixIconPath)
        };
        if (!Prefs.DevMode || !Controller.Settings.DebugMaster)
        {
            yield break;
        }

        var DebugIconPath = "UI/BestMix/DebugList";
        yield return new Command_Toggle
        {
            icon = ContentFinder<Texture2D>.Get(DebugIconPath),
            defaultLabel = "BestMix.DebugLabel".Translate(),
            defaultDesc = "BestMix.DebugDesc".Translate(),
            isActive = () => BMixDebug,
            toggleAction = delegate { ToggleDebug(BMixDebug); }
        };
    }

    private void ToggleDebug(bool flag)
    {
        BMixDebug = !flag;
    }

    private void DoModeSelMenu()
    {
        var list = new List<FloatMenuOption>();

        string text = "BestMix.DoNothing".Translate();
        list.Add(new FloatMenuOption(text, delegate { SetBMixMode(this, null, false); },
            MenuOptionPriority.Default, null, null, 29f));

        foreach (var mode in BestMixUtility.BMixModes())
        {
            text = BestMixUtility.GetBMixModeDisplay(mode);
            list.Add(new FloatMenuOption(text, delegate { SetBMixMode(this, mode, true); },
                MenuOptionPriority.Default, null, null, 29f));
        }

        var sortedlist = list.OrderBy(bm => bm.Label).ToList();
        Find.WindowStack.Add(new FloatMenu(sortedlist));
    }

    public void SetBMixMode(CompBestMix CBM, string GizmoSel, bool edit)
    {
        if (edit)
        {
            //MultiplayerSupport.MPLog("Gizmo mode:", GizmoSel);
            CBM.CurMode = GizmoSel;
        }
    }
}
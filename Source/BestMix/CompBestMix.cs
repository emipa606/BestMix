using RimWorld;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace BestMix
{
    public class CompBestMix : ThingComp
    {
        public string CurMode;
        public bool BMixDebug = false;
        public List<string> BillBMModes = new List<string>();

        public CompProperties_BestMix BMProps => (CompProperties_BestMix)props;

        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Values.Look<string>(ref CurMode, "CurMode", "DIS", false);
            Scribe_Values.Look<bool>(ref BMixDebug, "BMixDebug", false, false);
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
                BMBillUtility.CheckBillBMValues(this, parent as Thing, BillBMModes);
            }
        }

        public override string CompInspectStringExtra()
        {
            if (BestMixUtility.IsValidForComp(parent))
            {
                var ModeDisplay = BestMixUtility.GetBMixModeDisplay(CurMode);
                return "BestMix.CurrentMode".Translate(ModeDisplay);
            }
            return null;
        }

        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {
            foreach (Gizmo item in base.CompGetGizmosExtra())
            {
                yield return item;
            }
            if (BestMixUtility.IsValidForComp(parent))
            {
                if (parent.Spawned && parent.Faction == Faction.OfPlayer)
                {
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
                    if (Prefs.DevMode && Controller.Settings.DebugMaster)
                    {
                        var DebugIconPath = "UI/BestMix/DebugList";
                        yield return new Command_Toggle
                        {
                            icon = ContentFinder<Texture2D>.Get(DebugIconPath),
                            defaultLabel = "BestMix.DebugLabel".Translate(),
                            defaultDesc = "BestMix.DebugDesc".Translate(),
                            isActive = () => BMixDebug == true,
                            toggleAction = delegate
                            {
                                ToggleDebug(BMixDebug);
                            }
                        };
                    }

                }
            }
            yield break;
        }

        public void ToggleDebug(bool flag)
        {
            BMixDebug = !flag;
        }

        public void DoModeSelMenu()
        {
            var list = new List<FloatMenuOption>();

            string text = "BestMix.DoNothing".Translate();
            list.Add(new FloatMenuOption(text, delegate
            {
                SetBMixMode(this, null, false);
            },
            MenuOptionPriority.Default, null, null, 29f, null));

            foreach (var mode in BestMixUtility.BMixModes())
            {
                text = BestMixUtility.GetBMixModeDisplay(mode);
                list.Add(new FloatMenuOption(text, delegate
                {
                    SetBMixMode(this, mode, true);
                },
                MenuOptionPriority.Default, null, null, 29f, null));
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

    public class CompProperties_BestMix : CompProperties
    {
        public string DefaultMode = "DIS";

        public CompProperties_BestMix()
        {
            compClass = typeof(CompBestMix);
        }
    }
}



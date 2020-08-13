using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace BestMix
{
	// Token: 0x02000006 RID: 6
	public class CompBestMix : ThingComp
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000028 RID: 40 RVA: 0x00004B76 File Offset: 0x00002D76
		public CompProperties_BestMix BMProps
		{
			get
			{
				return (CompProperties_BestMix)this.props;
			}
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00004B84 File Offset: 0x00002D84
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<string>(ref this.CurMode, "CurMode", "DIS", false);
			Scribe_Values.Look<bool>(ref this.BMixDebug, "BMixDebug", false, false);
			Scribe_Collections.Look<string>(ref this.BillBMModes, "BillBMModes", LookMode.Value, Array.Empty<object>());
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00004BD5 File Offset: 0x00002DD5
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			if (this.CurMode == null)
			{
				this.CurMode = this.BMProps.DefaultMode;
			}
			if (respawningAfterLoad)
			{
				BMBillUtility.CheckBillBMValues(this, this.parent, this.BillBMModes);
			}
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00004C0C File Offset: 0x00002E0C
		public override string CompInspectStringExtra()
		{
			if (BestMixUtility.IsValidForComp(this.parent))
			{
				string ModeDisplay = BestMixUtility.GetBMixModeDisplay(this.CurMode);
				return "BestMix.CurrentMode".Translate(ModeDisplay);
			}
			return null;
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00004C49 File Offset: 0x00002E49
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			foreach (Gizmo item in base.CompGetGizmosExtra())
			{
				yield return item;
			}
			IEnumerator<Gizmo> enumerator = null;
			if (BestMixUtility.IsValidForComp(this.parent) && this.parent.Spawned && this.parent.Faction == Faction.OfPlayer)
			{
				string BMixIconPath = BestMixUtility.GetBMixIconPath(this.CurMode);
				yield return new Command_Action
				{
					action = delegate()
					{
						SoundDefOf.Tick_Tiny.PlayOneShotOnCamera(null);
						this.DoModeSelMenu();
					},
					hotKey = KeyBindingDefOf.Misc1,
					defaultLabel = "BestMix.SelectModeLabel".Translate(),
					defaultDesc = "BestMix.SelectModeDesc".Translate(),
					icon = ContentFinder<Texture2D>.Get(BMixIconPath, true)
				};
				if (Prefs.DevMode && Controller.Settings.DebugMaster)
				{
					string DebugIconPath = "UI/BestMix/DebugList";
					yield return new Command_Toggle
					{
						icon = ContentFinder<Texture2D>.Get(DebugIconPath, true),
						defaultLabel = "BestMix.DebugLabel".Translate(),
						defaultDesc = "BestMix.DebugDesc".Translate(),
						isActive = (() => this.BMixDebug),
						toggleAction = delegate()
						{
							this.ToggleDebug(this.BMixDebug);
						}
					};
				}
			}
			yield break;
			yield break;
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00004C59 File Offset: 0x00002E59
		public void ToggleDebug(bool flag)
		{
			this.BMixDebug = !flag;
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00004C68 File Offset: 0x00002E68
		public void DoModeSelMenu()
		{
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			string text = "BestMix.DoNothing".Translate();
			list.Add(new FloatMenuOption(text, delegate()
			{
				this.SetBMixMode(this, null, false);
			}, MenuOptionPriority.Default, null, null, 29f, null, null));
			using (List<string>.Enumerator enumerator = BestMixUtility.BMixModes().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					string mode = enumerator.Current;
					text = BestMixUtility.GetBMixModeDisplay(mode);
					list.Add(new FloatMenuOption(text, delegate()
					{
						this.SetBMixMode(this, mode, true);
					}, MenuOptionPriority.Default, null, null, 29f, null, null));
				}
			}
			List<FloatMenuOption> sortedlist = (from bm in list
			orderby bm.Label
			select bm).ToList<FloatMenuOption>();
			Find.WindowStack.Add(new FloatMenu(sortedlist));
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00004D6C File Offset: 0x00002F6C
		public void SetBMixMode(CompBestMix CBM, string GizmoSel, bool edit)
		{
			if (edit)
			{
				CBM.CurMode = GizmoSel;
			}
		}

		// Token: 0x04000002 RID: 2
		public string CurMode;

		// Token: 0x04000003 RID: 3
		public bool BMixDebug;

		// Token: 0x04000004 RID: 4
		public List<string> BillBMModes = new List<string>();
	}
}

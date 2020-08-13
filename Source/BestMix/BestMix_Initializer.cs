using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace BestMix
{
	// Token: 0x02000003 RID: 3
	[StaticConstructorOnStartup]
	internal static class BestMix_Initializer
	{
		// Token: 0x06000017 RID: 23 RVA: 0x000044A2 File Offset: 0x000026A2
		static BestMix_Initializer()
		{
			LongEventHandler.QueueLongEvent(new Action(BestMix_Initializer.Setup), "LibraryStartup", false, null, true);
		}

		// Token: 0x06000018 RID: 24 RVA: 0x000044C0 File Offset: 0x000026C0
		public static void Setup()
		{
			List<ThingDef> thingDefs = DefDatabase<ThingDef>.AllDefsListForReading;
			if (thingDefs.Count > 0)
			{
				int num = 0;
				foreach (ThingDef thingDef in thingDefs)
				{
					if (BestMix_Initializer.IsBuildingClass(thingDef) && BestMix_Initializer.IsBillItem(thingDef) && BestMix_Initializer.TryAddBestMixComp(thingDef))
					{
						num++;
					}
				}
				if (num > 0)
				{
					Log.Message("BestMix.SetupCount".Translate(num.ToString()), false);
				}
			}
			RegionProcessorSubtitution.Initialize(new RegionWork());
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00004564 File Offset: 0x00002764
		public static bool TryAddBestMixComp(ThingDef def)
		{
			if (def.comps.OfType<CompProperties_BestMix>().FirstOrDefault<CompProperties_BestMix>() == null)
			{
				CompProperties_BestMix compProp_BestMix = new CompProperties_BestMix();
				def.comps.Add(compProp_BestMix);
				return true;
			}
			return false;
		}

		// Token: 0x0600001A RID: 26 RVA: 0x0000459C File Offset: 0x0000279C
		public static bool IsBuildingClass(ThingDef def)
		{
			bool bClass = false;
			if ((def?.thingClass) != null)
			{
				Type chkClass = def.thingClass;
				if (chkClass.IsClass && chkClass.IsSubclassOf(typeof(Building)))
				{
					bClass = true;
				}
			}
			return bClass;
		}

		// Token: 0x0600001B RID: 27 RVA: 0x000045E4 File Offset: 0x000027E4
		public static bool IsBillItem(ThingDef def)
		{
			bool billItem = false;
			if ((def?.inspectorTabs) != null)
			{
				List<Type> inspects = def.inspectorTabs;
				if (inspects.Count > 0)
				{
					using (List<Type>.Enumerator enumerator = inspects.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							if (enumerator.Current == typeof(ITab_Bills))
							{
								billItem = true;
							}
						}
					}
				}
			}
			return billItem;
		}
	}
}

using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace BestMix
{
	// Token: 0x0200000D RID: 13
	public class RegionWork : RegionProcessorSubtitution
	{
		// Token: 0x06000068 RID: 104 RVA: 0x00005190 File Offset: 0x00003390
		protected override bool RegionProcessor(Region r)
		{
			if (BestMixUtility.BMixRegionIsInRange(r, base.P_billGiver, base.P_bill))
			{
				Predicate<Thing> BMixValidator = BestMixUtility.BestMixValidator(base.P_pawn, base.P_billGiver, base.P_bill);
				List<Thing> list = r.ListerThings.ThingsMatching(ThingRequest.ForGroup(ThingRequestGroup.HaulableEver));
				for (int i = 0; i < list.Count; i++)
				{
					Thing thing = list[i];
					if (!BestMixUtility.BMIsForbidden(thing) && !base.ProcessedThings.Contains(thing) && ReachabilityWithinRegion.ThingFromRegionListerReachable(thing, r, PathEndMode.ClosestTouch, base.P_pawn) && BMixValidator(thing) && (!thing.def.IsMedicine || !(base.P_billGiver is Pawn)))
					{
						base.NewRelevantThings.Add(thing);
						base.ProcessedThings.Add(thing);
					}
				}
				int lf_regionsProcessed = base.Lf_regionsProcessed;
				base.Lf_regionsProcessed = lf_regionsProcessed + 1;
				if (base.NewRelevantThings.Count > 0 && base.Lf_regionsProcessed > base.Lf_adjacentRegionsAvailable)
				{
					bool rnd;
					Comparison<Thing> comparison = BestMixUtility.GetBMixComparer(base.P_billGiver, base.Lf_rootCell, base.P_bill, out rnd);
					base.RelevantThings.AddRange(base.NewRelevantThings);
					if (rnd)
					{
						base.RelevantThings.Shuffle<Thing>();
					}
					else
					{
						base.RelevantThings.Sort(comparison);
					}
					BestMixUtility.BMixDebugList(base.RelevantThings, base.P_billGiver, base.Lf_rootCell, base.P_bill);
					base.NewRelevantThings.Clear();
					if (BestMixUtility.TryFindBestMixInSet(base.RelevantThings, base.P_bill, base.P_chosen, base.IngredientsOrdered))
					{
						base.Lf_foundAll = true;
						bool finishNow = BestMixUtility.BMixFinishedStatus(base.Lf_foundAll, base.P_billGiver, base.P_bill);
						if (base.Lf_foundAll && finishNow)
						{
							BestMixUtility.DebugChosenList(base.P_billGiver, base.P_chosen);
							BestMixUtility.DebugFoundAll(base.P_billGiver, base.Lf_foundAll);
							return true;
						}
					}
				}
			}
			BestMixUtility.DebugChosenList(base.P_billGiver, base.P_chosen);
			BestMixUtility.DebugFoundAll(base.P_billGiver, base.Lf_foundAll);
			return false;
		}
	}
}

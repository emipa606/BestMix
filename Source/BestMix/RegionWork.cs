using RimWorld;
using System;
using System.Collections.Generic;
using System.Reflection;
using Verse;
using Verse.AI;
using HarmonyLib;

namespace BestMix
{
    public class RegionWork : RegionProcessorSubtitution
    {
        protected override bool RegionProcessor(Region r)
        {
            if (BestMixUtility.BMixRegionIsInRange(r, P_billGiver, P_bill))
            {
                Predicate<Thing> BMixValidator = BestMixUtility.BestMixValidator(P_pawn, P_billGiver, P_bill);

                List<Thing> list = r.ListerThings.ThingsMatching(ThingRequest.ForGroup(ThingRequestGroup.HaulableEver));
                for (var i = 0; i < list.Count; i++)
                {
                    Thing thing = list[i];
                    if (!BestMixUtility.BMIsForbidden(thing))
                    {
                        if (!ProcessedThings.Contains(thing) && ReachabilityWithinRegion.ThingFromRegionListerReachable(thing, r, PathEndMode.ClosestTouch, P_pawn)
                            && BMixValidator(thing) && (!thing.def.IsMedicine || (!(P_billGiver is Pawn))))
                        {
                            NewRelevantThings.Add(thing);
                            ProcessedThings.Add(thing);
                        }
                    }
                }
                Lf_regionsProcessed++;
                if (NewRelevantThings.Count > 0 && Lf_regionsProcessed > Lf_adjacentRegionsAvailable)
                {
                    Comparison<Thing> comparison = BestMixUtility.GetBMixComparer(P_billGiver, Lf_rootCell, P_bill, out var rnd);
                    //newRelevantThings.Sort(comparison);
                    RelevantThings.AddRange(NewRelevantThings);
                    if (rnd)
                    {
                        RelevantThings.Shuffle();
                    }
                    else
                    {
                        RelevantThings.Sort(comparison);
                    }
                    BestMixUtility.BMixDebugList(RelevantThings, P_billGiver, Lf_rootCell, P_bill);
                    NewRelevantThings.Clear();

                    if (BestMixUtility.TryFindBestMixInSet(RelevantThings, P_bill, P_chosen, IngredientsOrdered))
                    {
                        Lf_foundAll = true;
                        var finishNow = BestMixUtility.BMixFinishedStatus(Lf_foundAll, P_billGiver, P_bill);
                        if (Lf_foundAll && finishNow)
                        {
                            BestMixUtility.DebugChosenList(P_billGiver, P_chosen);
                            BestMixUtility.DebugFoundAll(P_billGiver, Lf_foundAll);
                            return true;
                        }
                    }
                }
            }
            BestMixUtility.DebugChosenList(P_billGiver, P_chosen);
            BestMixUtility.DebugFoundAll(P_billGiver, Lf_foundAll);
            return false;
        }
    }
}


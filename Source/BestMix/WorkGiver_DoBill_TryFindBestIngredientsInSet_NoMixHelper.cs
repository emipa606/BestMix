using System.Collections.Generic;
using HarmonyLib;
using RimWorld;
using Verse;

namespace BestMix.Patches;

[HarmonyPatch(typeof(WorkGiver_DoBill), "TryFindBestIngredientsInSet_NoMixHelper")]
public static class WorkGiver_DoBill_TryFindBestIngredientsInSet_NoMixHelper
{
    [HarmonyPriority(Priority.First)]
    public static void Prefix(List<Thing> availableThings, IntVec3 rootCell, ref bool alreadySorted, Bill bill = null)
    {
        BestMixUtility.Sort(availableThings, rootCell, bill);
        alreadySorted = true;
    }
}
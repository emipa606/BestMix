using System.Collections.Generic;
using HarmonyLib;
using RimWorld;
using Verse;

namespace BestMix.Patches;

[HarmonyPatch(typeof(WorkGiver_DoBill), "TryFindBestBillIngredients")]
public static class WorkGiver_DoBill_TryFindBestBillIngredients
{
    public static Thing curGiver;

    public static void Prefix(Bill bill, Pawn pawn, Thing billGiver, List<ThingCount> chosen)
    {
        curGiver = billGiver;
    }

    public static void Postfix()
    {
        curGiver = null;
    }
}
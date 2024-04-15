using HarmonyLib;
using RimWorld;
using Verse;

namespace BestMix.Patches;

[HarmonyPatch(typeof(WorkGiver_DoBill), "TryFindBestBillIngredients")]
public static class WorkGiver_DoBill_TryFindBestBillIngredients
{
    public static Thing curGiver;

    public static void Prefix(Thing billGiver)
    {
        curGiver = billGiver;
    }

    public static void Postfix()
    {
        curGiver = null;
    }
}
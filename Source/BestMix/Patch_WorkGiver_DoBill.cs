using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using RimWorld;
using Verse;
using Verse.AI;

namespace BestMix.Patches
{
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

    [HarmonyPatch(typeof(WorkGiver_DoBill), "TryFindBestIngredientsInSet_NoMixHelper")]
    public static class WorkGiver_DoBill_TryFindBestIngredientsInSet_NoMixHelper
    {
        [HarmonyPriority(Priority.First)]
        public static void Prefix(List<Thing> availableThings, List<IngredientCount> ingredients, List<ThingCount> chosen, IntVec3 rootCell, ref bool alreadySorted, Bill bill = null)
        {
            BestMixUtility.Sort(availableThings, rootCell, bill);
            alreadySorted = true;
        }
    }

    [HarmonyPatch(typeof(WorkGiver_DoBill), "TryFindBestBillIngredientsInSet_AllowMix")]
    public static class WorkGiver_DoBill_TryFindBestBillIngredientsInSet_AllowMix
    {
        public const int sortILIndex = 7;
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var codes = instructions.ToList();
            bool found = false;
            for (var i = 0; i < codes.Count; i++)
            {
                if (i + sortILIndex < (codes.Count - 1) && !found && codes[i + sortILIndex].opcode == OpCodes.Call && codes[i + sortILIndex].operand.ToString().Contains("SortBy"))
                {
                    found = true;
                    yield return new CodeInstruction(OpCodes.Ldarg_0);
                    yield return new CodeInstruction(OpCodes.Ldarg_3);
                    yield return new CodeInstruction(OpCodes.Ldarg_1);
                    yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(BestMixUtility), nameof(BestMixUtility.Sort)));
                    i += sortILIndex + 1;
                }
                yield return codes[i];
            }
        }
    }
}
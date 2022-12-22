using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using HarmonyLib;
using RimWorld;

namespace BestMix.Patches;

[HarmonyPatch(typeof(WorkGiver_DoBill), "TryFindBestBillIngredientsInSet_AllowMix")]
public static class WorkGiver_DoBill_TryFindBestBillIngredientsInSet_AllowMix
{
    public const int sortILIndex = 6;

    private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var codes = instructions.ToList();
        var found = false;
        for (var i = 0; i < codes.Count; i++)
        {
            if (i + sortILIndex < codes.Count - 1 && !found && codes[i + sortILIndex].opcode == OpCodes.Call &&
                codes[i + sortILIndex].operand.ToString().Contains("SortBy"))
            {
                found = true;
                //yield return new CodeInstruction(OpCodes.Ldarg_0);
                yield return new CodeInstruction(OpCodes.Ldarg_3);
                yield return new CodeInstruction(OpCodes.Ldarg_1);
                yield return new CodeInstruction(OpCodes.Call,
                    AccessTools.Method(typeof(BestMixUtility), nameof(BestMixUtility.Sort)));
                i += sortILIndex + 1;
            }

            yield return codes[i];
        }
    }
}
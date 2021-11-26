using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace BestMix;

[HarmonyPatch(typeof(Bill), "DoInterface")]
public static class Patch_Bill_DoInterface
{
    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var billStack_FieldInfo = AccessTools.Field(typeof(Bill), "billStack");
        var EndGroupGUI = AccessTools.Method(typeof(GUI), "EndGroup");
        var AddBMGUIPart = AccessTools.Method(typeof(Patch_Bill_DoInterface), "AddBMGUI",
            new[] { typeof(float), typeof(BillStack), typeof(Bill) });

        var instructionList = instructions.ToList();
        var length = instructionList.Count;
        for (var i = 0; i < length; i++)
        {
            var codeInstruction = instructionList[i];
            if (instructionList[i].opcode == OpCodes.Call && instructionList[i].Calls(EndGroupGUI))
            {
                //Log.Message($"Opcode : {codeInstruction.opcode} | Operand : {codeInstruction.operand ?? "null"}"); // Debug
                //Log.Message($"Opcode : {instructionList[i - 1].opcode} | Operand : {instructionList[i - 1].operand ?? "null"}"); // Debug

                // Insert pseudo code here
                // static method doesn't need any reference to instance to call method.
                // yield return new CodeInstruction(OpCodes.Ldarg_0);

                // push width value to stack
                yield return new CodeInstruction(OpCodes.Ldarg, 3) { labels = codeInstruction.labels };

                // push BillStack instance to stack
                yield return new CodeInstruction(OpCodes.Ldarg_0);
                yield return new CodeInstruction(OpCodes.Ldfld, billStack_FieldInfo);

                // push Bill instance to stack
                yield return new CodeInstruction(OpCodes.Ldarg_0);

                // call static method
                yield return new CodeInstruction(OpCodes.Call, AddBMGUIPart);

                // returning original code.
                //yield return codeInstruction;
                yield return new CodeInstruction(OpCodes.Call, codeInstruction.operand);

                continue; // preventing duplicating same IL twice.
            }

            yield return codeInstruction;
        }

        // yield break; <- doesn't need
    }

    public static void AddBMGUI(float width, BillStack billstack, Bill bill)
    {
        if (bill.recipe.IsSurgery)
        {
            return;
        }

        //Color baseColor = new Color(1f, 0.7f, 0.7f, 0.7f);
        var baseColor = Color.white;
        //float offset = Controller.Settings.BillBMPos;
        var rectBM = new Rect(width - (24f + 150f), 0f, 24f, 24f);
        var BMTex = BMBillUtility.GetBillBMTex(billstack.billGiver as Thing, bill);
        if (!Widgets.ButtonImage(rectBM, BMTex, baseColor, baseColor * GenUI.SubtleMouseoverColor))
        {
            return;
        }

        BMBillUtility.SetBillBMVal(billstack.billGiver as Thing, bill);
        SoundDefOf.Click.PlayOneShotOnCamera();
    }
}
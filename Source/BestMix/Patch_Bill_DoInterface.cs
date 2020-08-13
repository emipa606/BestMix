using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace BestMix
{
	// Token: 0x0200000B RID: 11
	[HarmonyPatch(typeof(Bill), "DoInterface")]
	public static class Patch_Bill_DoInterface
	{
		// Token: 0x0600003F RID: 63 RVA: 0x00004F10 File Offset: 0x00003110
		public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
		{
			FieldInfo billStack_FieldInfo = AccessTools.Field(typeof(Bill), "billStack");
			MethodInfo EndGroupGUI = AccessTools.Method(typeof(GUI), "EndGroup", null, null);
			MethodInfo AddBMGUIPart = AccessTools.Method(typeof(Patch_Bill_DoInterface), "AddBMGUI", new Type[]
			{
				typeof(float),
				typeof(BillStack),
				typeof(Bill)
			}, null);
			List<CodeInstruction> instructionList = instructions.ToList<CodeInstruction>();
			int length = instructionList.Count;
			int num;
			for (int i = 0; i < length; i = num + 1)
			{
				CodeInstruction codeInstruction = instructionList[i];
				if (instructionList[i].opcode == OpCodes.Call && CodeInstructionExtensions.Calls(instructionList[i], EndGroupGUI))
				{
					yield return new CodeInstruction(OpCodes.Ldarg, 3)
					{
						labels = codeInstruction.labels
					};
					yield return new CodeInstruction(OpCodes.Ldarg_0, null);
					yield return new CodeInstruction(OpCodes.Ldfld, billStack_FieldInfo);
					yield return new CodeInstruction(OpCodes.Ldarg_0, null);
					yield return new CodeInstruction(OpCodes.Call, AddBMGUIPart);
					yield return new CodeInstruction(OpCodes.Call, codeInstruction.operand);
				}
				else
				{
					yield return codeInstruction;
					codeInstruction = null;
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00004F20 File Offset: 0x00003120
		public static void AddBMGUI(float width, BillStack billstack, Bill bill)
		{
			if (!bill.recipe.IsSurgery)
			{
				Color baseColor = Color.white;
				Rect butRect = new Rect(width - 174f, 0f, 24f, 24f);
				Texture2D BMTex = BMBillUtility.GetBillBMTex(billstack.billGiver as Thing, bill);
				if (Widgets.ButtonImage(butRect, BMTex, baseColor, baseColor * GenUI.SubtleMouseoverColor, true))
				{
					BMBillUtility.SetBillBMVal(billstack.billGiver as Thing, bill);
					SoundDefOf.Click.PlayOneShotOnCamera(null);
				}
			}
		}
	}
}

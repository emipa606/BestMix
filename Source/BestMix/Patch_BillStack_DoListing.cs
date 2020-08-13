using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using HarmonyLib;
using RimWorld;
using Verse;

namespace BestMix
{
	// Token: 0x02000004 RID: 4
	[HarmonyPatch(typeof(BillStack), "DoListing")]
	public static class Patch_BillStack_DoListing
	{
		// Token: 0x0600001C RID: 28 RVA: 0x00004660 File Offset: 0x00002860
		public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
		{
			if (!Controller.Settings.AllowBMBillMaxSet)
			{
				yield break;
			}
			int newMax = 125;
			AccessTools.Property(typeof(BillStack), "Count").GetGetMethod();
			List<CodeInstruction> instructionList = instructions.ToList<CodeInstruction>();
			bool found = false;
			int num;
			for (int i = 0; i < instructionList.Count; i = num)
			{
				CodeInstruction codeInstruction = instructionList[i];
				if (instructionList[i].opcode == OpCodes.Ldc_I4_S && Convert.ToInt32(instructionList[i].operand) == 15)
				{
					found = true;
					instructionList[i].operand = newMax;
				}
				yield return codeInstruction;
				num = i + 1;
			}
			if (!found)
			{
				Log.Error("BestMix.MaxBillFail".Translate(), false);
			}
			else
			{
				Log.Message("BestMix.MaxBillDone".Translate(newMax.ToString()), false);
			}
			yield break;
		}
	}
}

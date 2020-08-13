using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using RimWorld;
using Verse;

namespace BestMix.Patches
{
	// Token: 0x02000011 RID: 17
	internal class Patch_WorkGiver_DoBill : CustomHarmonyPatch
	{
		// Token: 0x06000072 RID: 114 RVA: 0x00005840 File Offset: 0x00003A40
		internal override void Patch(Harmony HMinstance)
		{
			try
			{
				MethodInfo original = AccessTools.Method(typeof(WorkGiver_DoBill), "TryFindBestBillIngredients", null, null);
				MethodInfo transpiler = AccessTools.Method(typeof(Patch_WorkGiver_DoBill), "Transpiler_TryFindBestBillIngredients", null, null);
				HMinstance.Patch(original, null, null, new HarmonyMethod(transpiler), null);
			}
			catch (Exception ex)
			{
				Log.Error(string.Format("Exception while patching WorkGiver_DoBill !\n{0}", ex), false);
			}
		}

		// Token: 0x06000073 RID: 115 RVA: 0x000058B4 File Offset: 0x00003AB4
		private static IEnumerable<CodeInstruction> Transpiler_TryFindBestBillIngredients(IEnumerable<CodeInstruction> instructions)
		{
			Type workGiverType = typeof(WorkGiver_DoBill);
			FieldInfo RegionProcessorSubtitutionSingleton = AccessTools.Field(typeof(RegionProcessorSubtitution), "singleton");
			MethodInfo LdvirtftnMethodBase = AccessTools.Method(typeof(RegionProcessorSubtitution), "RegionProcessor", null, null);
			Type RegionProcessorType = AccessTools.TypeByName("RegionProcessor");
			ConstructorInfo RegionProcessorPointerCtor = AccessTools.Constructor(RegionProcessorType, new Type[]
			{
				typeof(object),
				typeof(IntPtr)
			}, false);
			MethodInfo FetchLocalFields = AccessTools.Method(typeof(RegionProcessorSubtitution), RegionProcessorSubtitution.FetchLocalFieldsMethodName, null, null);
			MethodInfo FetchStaticFields = AccessTools.Method(typeof(RegionProcessorSubtitution), RegionProcessorSubtitution.FetchStaticFieldsMethodName, null, null);
			MethodInfo UpdateData = AccessTools.Method(typeof(RegionProcessorSubtitution), RegionProcessorSubtitution.UpdateDataName, null, null);
			Type c__DisplayClass20_0 = AccessTools.FirstInner(workGiverType, (Type type) => type.Name.Contains("<>c__DisplayClass20_0"));
			FieldInfo h_adjacentRegionsAvailable = AccessTools.Field(c__DisplayClass20_0, "adjacentRegionsAvailable");
			FieldInfo h_pawn = AccessTools.Field(c__DisplayClass20_0, "pawn");
			FieldInfo h_regionsProcessed = AccessTools.Field(c__DisplayClass20_0, "regionsProcessed");
			FieldInfo h_rootCell = AccessTools.Field(c__DisplayClass20_0, "rootCell");
			FieldInfo h_foundAll = AccessTools.Field(c__DisplayClass20_0, "foundAll");
			FieldInfo h_bill = AccessTools.Field(c__DisplayClass20_0, "bill");
			FieldInfo h_billGiver = AccessTools.Field(c__DisplayClass20_0, "billGiver");
			FieldInfo h_chosen = AccessTools.Field(c__DisplayClass20_0, "chosen");
			FieldInfo sf_chosenIngThings = AccessTools.Field(workGiverType, "chosenIngThings");
			FieldInfo sf_relevantThings = AccessTools.Field(workGiverType, "relevantThings");
			FieldInfo sf_processedThings = AccessTools.Field(workGiverType, "processedThings");
			FieldInfo sf_newRelevantThings = AccessTools.Field(workGiverType, "newRelevantThings");
			FieldInfo sf_ingredientsOrdered = AccessTools.Field(workGiverType, "ingredientsOrdered");
			List<CodeInstruction> insts = instructions.ToList<CodeInstruction>();
			int instsLength = insts.Count;
			int num;
			for (int i = 0; i < instsLength; i = num + 1)
			{
				CodeInstruction inst = insts[i];
				if (i < instsLength - 2 && i > 0 && inst.opcode == OpCodes.Ldloc_0 && insts[i + 1].opcode == OpCodes.Ldftn && insts[i - 1].opcode == OpCodes.Call)
				{
					yield return new CodeInstruction(OpCodes.Ldsfld, RegionProcessorSubtitutionSingleton);
					yield return new CodeInstruction(OpCodes.Ldloc_0, null);
					yield return new CodeInstruction(OpCodes.Ldfld, h_adjacentRegionsAvailable);
					yield return new CodeInstruction(OpCodes.Ldloc_0, null);
					yield return new CodeInstruction(OpCodes.Ldfld, h_regionsProcessed);
					yield return new CodeInstruction(OpCodes.Ldloc_0, null);
					yield return new CodeInstruction(OpCodes.Ldfld, h_rootCell);
					yield return new CodeInstruction(OpCodes.Ldloc_0, null);
					yield return new CodeInstruction(OpCodes.Ldfld, h_bill);
					yield return new CodeInstruction(OpCodes.Ldloc_0, null);
					yield return new CodeInstruction(OpCodes.Ldfld, h_pawn);
					yield return new CodeInstruction(OpCodes.Ldloc_0, null);
					yield return new CodeInstruction(OpCodes.Ldfld, h_billGiver);
					yield return new CodeInstruction(OpCodes.Ldloc_0, null);
					yield return new CodeInstruction(OpCodes.Ldfld, h_chosen);
					yield return new CodeInstruction(OpCodes.Ldc_I4_0, null);
					yield return new CodeInstruction(OpCodes.Callvirt, FetchLocalFields);
					yield return new CodeInstruction(OpCodes.Ldsfld, RegionProcessorSubtitutionSingleton);
					yield return new CodeInstruction(OpCodes.Ldsfld, sf_chosenIngThings);
					yield return new CodeInstruction(OpCodes.Ldsfld, sf_relevantThings);
					yield return new CodeInstruction(OpCodes.Ldsfld, sf_processedThings);
					yield return new CodeInstruction(OpCodes.Ldsfld, sf_newRelevantThings);
					yield return new CodeInstruction(OpCodes.Ldsfld, sf_ingredientsOrdered);
					yield return new CodeInstruction(OpCodes.Callvirt, FetchStaticFields);
					yield return new CodeInstruction(OpCodes.Ldsfld, RegionProcessorSubtitutionSingleton);
					yield return new CodeInstruction(OpCodes.Dup, null);
					yield return new CodeInstruction(OpCodes.Ldvirtftn, LdvirtftnMethodBase);
					yield return new CodeInstruction(OpCodes.Newobj, RegionProcessorPointerCtor);
					i += 2;
				}
				else if (inst.opcode == OpCodes.Call && inst.operand != null && inst.operand.ToString().Contains("BreadthFirstTraverse"))
				{
					yield return inst;
					yield return new CodeInstruction(OpCodes.Ldsfld, RegionProcessorSubtitutionSingleton);
					yield return new CodeInstruction(OpCodes.Ldloc_0, null);
					yield return new CodeInstruction(OpCodes.Ldflda, h_bill);
					yield return new CodeInstruction(OpCodes.Ldloc_0, null);
					yield return new CodeInstruction(OpCodes.Ldflda, h_pawn);
					yield return new CodeInstruction(OpCodes.Ldloc_0, null);
					yield return new CodeInstruction(OpCodes.Ldflda, h_billGiver);
					yield return new CodeInstruction(OpCodes.Ldloc_0, null);
					yield return new CodeInstruction(OpCodes.Ldflda, h_chosen);
					yield return new CodeInstruction(OpCodes.Ldloc_0, null);
					yield return new CodeInstruction(OpCodes.Ldflda, h_foundAll);
					yield return new CodeInstruction(OpCodes.Callvirt, UpdateData);
				}
				else
				{
					yield return inst;
				}
				num = i;
			}
			yield break;
		}
	}
}

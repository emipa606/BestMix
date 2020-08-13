using System;
using System.Reflection;
using BestMix.Patches;
using HarmonyLib;
using Verse;

namespace BestMix
{
	// Token: 0x02000009 RID: 9
	[StaticConstructorOnStartup]
	internal static class HarmonyPatching
	{
		// Token: 0x0600003A RID: 58 RVA: 0x00004E1C File Offset: 0x0000301C
		static HarmonyPatching()
		{
			Harmony harmony = new Harmony("com.Pelador.Rimworld.BestMix");
			harmony.PatchAll(Assembly.GetExecutingAssembly());
			HarmonyPatchTool.PatchAll(harmony);
		}
	}
}

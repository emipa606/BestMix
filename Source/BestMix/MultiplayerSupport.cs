using System;
using System.Reflection;
using HarmonyLib;
using Multiplayer.API;
using Verse;

namespace BestMix
{
	// Token: 0x0200000A RID: 10
	[StaticConstructorOnStartup]
	internal static class MultiplayerSupport
	{
		// Token: 0x0600003B RID: 59 RVA: 0x00004E38 File Offset: 0x00003038
		static MultiplayerSupport()
		{
			if (!MP.enabled)
			{
				return;
			}
			MP.RegisterSyncMethod(typeof(BMBillUtility), "SetBMixBillMode", null);
			MP.RegisterSyncMethod(typeof(CompBestMix), "SetBMixMode", null);
			MethodInfo[] array = new MethodInfo[]
			{
				AccessTools.Method(typeof(BestMixUtility), "RNDFloat", null, null)
			};
			for (int i = 0; i < array.Length; i++)
			{
				MultiplayerSupport.FixRNG(array[i]);
			}
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00004EBE File Offset: 0x000030BE
		private static void FixRNG(MethodInfo method)
		{
			MultiplayerSupport.harmony.Patch(method, new HarmonyMethod(typeof(MultiplayerSupport), "FixRNGPre", null), new HarmonyMethod(typeof(MultiplayerSupport), "FixRNGPos", null), null, null);
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00004EF8 File Offset: 0x000030F8
		private static void FixRNGPre()
		{
			Rand.PushState(Find.TickManager.TicksAbs);
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00004F09 File Offset: 0x00003109
		private static void FixRNGPos()
		{
			Rand.PopState();
		}

		// Token: 0x04000007 RID: 7
		private static readonly Harmony harmony = new Harmony("rimworld.pelador.bestmix.multiplayersupport");
	}
}

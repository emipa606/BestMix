using System;
using System.Linq;
using System.Reflection;
using HarmonyLib;

namespace BestMix.Patches
{
	// Token: 0x02000010 RID: 16
	public static class HarmonyPatchTool
	{
		// Token: 0x0600006F RID: 111 RVA: 0x00005778 File Offset: 0x00003978
		public static void PatchAll(Harmony HMinstance)
		{
			HarmonyPatchTool.EnsurePatchingOnlyOnce();
			try
			{
				foreach (Type type2 in from type in Assembly.GetAssembly(typeof(HarmonyPatchTool)).GetTypes()
				where type.IsSubclassOf(typeof(CustomHarmonyPatch))
				select type)
				{
					(Activator.CreateInstance(type2) as CustomHarmonyPatch).Patch(HMinstance);
				}
			}
			catch (Exception)
			{
			}
			finally
			{
				HarmonyPatchTool.initialized = true;
			}
		}

		// Token: 0x06000070 RID: 112 RVA: 0x00005828 File Offset: 0x00003A28
		private static void EnsurePatchingOnlyOnce()
		{
			if (HarmonyPatchTool.initialized)
			{
				throw new Exception("trying to invoke PatchAll method twice!");
			}
		}

		// Token: 0x0400002F RID: 47
		private static bool initialized;
	}
}

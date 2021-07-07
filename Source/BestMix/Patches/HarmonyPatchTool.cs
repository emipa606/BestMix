using System;
using System.Linq;
using System.Reflection;
using HarmonyLib;

namespace BestMix.Patches
{
    public static class HarmonyPatchTool
    {
        private static bool initialized;

        public static void PatchAll(Harmony HMinstance)
        {
            EnsurePatchingOnlyOnce();

            try
            {
                var CustomPatchTypes = from type in Assembly.GetAssembly(typeof(HarmonyPatchTool)).GetTypes()
                    where type.IsSubclassOf(typeof(CustomHarmonyPatch))
                    select type;


                foreach (var type in CustomPatchTypes)
                {
                    var instance = Activator.CreateInstance(type) as CustomHarmonyPatch;
                    instance?.Patch(HMinstance);
                }
            }
            catch
            {
                // ignored
            }
            finally
            {
                initialized = true;
            }
        }

        private static void EnsurePatchingOnlyOnce()
        {
            if (initialized)
            {
                throw new Exception("trying to invoke PatchAll method twice!");
            }
        }
    }
}
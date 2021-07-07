using HarmonyLib;

namespace BestMix.Patches
{
    internal abstract class CustomHarmonyPatch
    {
        internal abstract void Patch(Harmony HMinstance);
    }
}
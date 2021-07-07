using System.Reflection;
using HarmonyLib;
using Multiplayer.API;
using Verse;

namespace BestMix
{
    [StaticConstructorOnStartup]
    internal static class MultiplayerSupport
    {
        private static readonly Harmony harmony = new Harmony("rimworld.pelador.bestmix.multiplayersupport");

        static MultiplayerSupport()
        {
            if (!MP.enabled)
            {
                return;
            }

            //SyncMethods
            MP.RegisterSyncMethod(typeof(BMBillUtility), nameof(BMBillUtility.SetBMixBillMode));
            MP.RegisterSyncMethod(typeof(CompBestMix), nameof(CompBestMix.SetBMixMode));

            // Add all Methods where there is Rand calls here
            var methods = new[]
            {
                AccessTools.Method(typeof(BestMixUtility), nameof(BestMixUtility.RNDFloat))
            };
            foreach (var method in methods)
            {
                FixRNG(method);
            }
        }

        private static void FixRNG(MethodInfo method)
        {
            harmony.Patch(method,
                new HarmonyMethod(typeof(MultiplayerSupport), nameof(FixRNGPre)),
                new HarmonyMethod(typeof(MultiplayerSupport), nameof(FixRNGPos))
            );
        }

        private static void FixRNGPre()
        {
            Rand.PushState(Find.TickManager.TicksAbs);
        }

        private static void FixRNGPos()
        {
            Rand.PopState();
        }
    }
}
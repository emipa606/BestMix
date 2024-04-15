using System.Reflection;
using HarmonyLib;
using Verse;

namespace BestMix;

[StaticConstructorOnStartup]
internal static class HarmonyPatching
{
    static HarmonyPatching()
    {
        new Harmony("com.Pelador.Rimworld.BestMix").PatchAll(Assembly.GetExecutingAssembly());
    }
}
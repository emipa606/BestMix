using System.Linq;
using RimWorld;
using Verse;

namespace BestMix;

[StaticConstructorOnStartup]
internal static class BestMix_Initializer
{
    static BestMix_Initializer()
    {
        LongEventHandler.QueueLongEvent(Setup, "LibraryStartup", false, null);
    }

    private static void Setup()
    {
        var thingDefs = DefDatabase<ThingDef>.AllDefsListForReading;
        if (thingDefs.Count <= 0)
        {
            return;
        }

        var num = 0;
        foreach (var thingDef in thingDefs)
        {
            if (!IsBuildingClass(thingDef) || !IsBillItem(thingDef))
            {
                continue;
            }

            if (TryAddBestMixComp(thingDef))
            {
                num++;
            }
        }

        if (num <= 0)
        {
            return;
        }

        string msg = "BestMix.SetupCount".Translate(num.ToString());
        Log.Message(msg);
    }

    private static bool TryAddBestMixComp(ThingDef def)
    {
        var compProp_BestMix = def.comps.OfType<CompProperties_BestMix>().FirstOrDefault();
        if (compProp_BestMix != null)
        {
            return false;
        }

        compProp_BestMix = new CompProperties_BestMix();
        def.comps.Add(compProp_BestMix);
        return true;
    }

    private static bool IsBuildingClass(ThingDef def)
    {
        var bClass = false;
        if (def?.thingClass == null)
        {
            return false;
        }

        var chkClass = def.thingClass;
        if (!chkClass.IsClass)
        {
            return false;
        }

        if (chkClass.IsSubclassOf(typeof(Building)))
        {
            bClass = true;
        }

        return bClass;
    }

    private static bool IsBillItem(ThingDef def)
    {
        if (def?.inspectorTabs == null)
        {
            return false;
        }

        var inspects = def.inspectorTabs;
        if (inspects.Count <= 0)
        {
            return false;
        }

        foreach (var inspect in inspects)
        {
            if (inspect == typeof(ITab_Bills))
            {
                return true;
            }
        }

        return false;
    }
}
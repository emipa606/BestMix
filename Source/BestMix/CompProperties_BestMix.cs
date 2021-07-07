using Verse;

namespace BestMix
{
    public class CompProperties_BestMix : CompProperties
    {
        public readonly string DefaultMode = "DIS";

        public CompProperties_BestMix()
        {
            compClass = typeof(CompBestMix);
        }
    }
}
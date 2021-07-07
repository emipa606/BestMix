using System;
using System.Collections.Generic;
using RimWorld;
using Verse;

namespace BestMix
{
    public abstract class RegionProcessorSubtitution
    {
        public static RegionProcessorSubtitution singleton;
        public static readonly string FetchLocalFieldsMethodName = nameof(FetchLocalFields);
        public static readonly string FetchStaticFieldsMethodName = nameof(FetchStaticFields);

        public static readonly string UpdateDataName = nameof(UpdateData);
        //protected WorkGiver_DoBill.DefCountList availableCounts { get; set; }


        protected List<ThingCount> ChosenIngThings { get; set; } // 바뀜
        protected IntRange ReCheckFailedBillTicksRange { get; } // 안바뀜
        protected string MissingMaterialsTranslated { get; } // 안바뀜
        protected List<Thing> RelevantThings { get; set; } // 바뀔수도
        protected HashSet<Thing> ProcessedThings { get; set; } // 바뀔수도
        protected List<Thing> NewRelevantThings { get; set; } // 바뀔수도
        protected List<IngredientCount> IngredientsOrdered { get; set; } // 바뀜
        protected List<Thing> TmpMedicine { get; set; } // 필요없음
        protected int Lf_adjacentRegionsAvailable { get; set; }
        protected int Lf_regionsProcessed { get; set; }
        protected IntVec3 Lf_rootCell { get; set; }
        protected Bill P_bill { get; set; }
        protected Pawn P_pawn { get; set; }
        protected Thing P_billGiver { get; set; }
        protected List<ThingCount> P_chosen { get; set; }
        protected bool Lf_foundAll { get; set; }


        //class option
        protected virtual bool ApplyToParameter { get; } = true;
        protected virtual bool CopyOnOverride { get; } = false;

        // called by reflection, connected by Patch_WorkGiver_DoBill
        // lf = local field, p = parameter
        private void FetchStaticFields(List<ThingCount> _chosenIngThings,
            List<Thing> _relevantThings,
            HashSet<Thing> _processedThings,
            List<Thing> _newRelevantThings,
            List<IngredientCount> _ingredientsOrdered)
        {
            //get by harmony
            ChosenIngThings = _chosenIngThings;
            RelevantThings = _relevantThings;
            ProcessedThings = _processedThings;
            NewRelevantThings = _newRelevantThings;
            IngredientsOrdered = _ingredientsOrdered;
        }

        // called by reflection, connected by Patch_WorkGiver_DoBill
        private void FetchLocalFields(int lf_adjacentRegionsAvailable,
            int lf_regionsProcessed,
            IntVec3 lf_rootCell,
            Bill p_bill,
            Pawn p_pawn,
            Thing p_billGiver,
            List<ThingCount> p_chosen,
            bool lf_foundAll)
        {
            //local fields  
            Lf_adjacentRegionsAvailable = lf_adjacentRegionsAvailable;
            Lf_regionsProcessed = lf_regionsProcessed;
            Lf_rootCell = lf_rootCell;
            P_bill = p_bill;
            P_pawn = p_pawn;
            P_billGiver = p_billGiver;
            P_chosen = p_chosen;
            Lf_foundAll = lf_foundAll;
        }

        protected abstract bool RegionProcessor(Region reg);

        // called by reflection, connected by Patch_WorkGiver_DoBill
        private void UpdateData(ref Bill bill,
            ref Pawn pawn,
            ref Thing billGiver,
            ref List<ThingCount> chosen,
            ref bool foundAll)
        {
            if (ApplyToParameter)
            {
                bill = P_bill;
                pawn = P_pawn;
                billGiver = P_billGiver;
                chosen = P_chosen;
                foundAll = Lf_foundAll;
            }
        }

        public static void Initialize(RegionProcessorSubtitution instance)
        {
            if (singleton != null)
            {
                throw new Exception(
                    "RegionProcessorSubtitution should be initialized once! you're calling initializer more than once.");
            }

            singleton = instance;
        }
    }
}
using System;
using System.Collections.Generic;
using RimWorld;
using Verse;

namespace BestMix
{
	// Token: 0x0200000C RID: 12
	public abstract class RegionProcessorSubtitution
	{
		// Token: 0x06000041 RID: 65 RVA: 0x00004F9E File Offset: 0x0000319E
		public static void Initialize(RegionProcessorSubtitution instance)
		{
			if (RegionProcessorSubtitution.singleton != null)
			{
				throw new Exception("RegionProcessorSubtitution should be initialized once! you're calling initializer more than once.");
			}
			RegionProcessorSubtitution.singleton = instance;
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000042 RID: 66 RVA: 0x00004FB8 File Offset: 0x000031B8
		// (set) Token: 0x06000043 RID: 67 RVA: 0x00004FC0 File Offset: 0x000031C0
		protected List<ThingCount> ChosenIngThings { get; set; }

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000044 RID: 68 RVA: 0x00004FC9 File Offset: 0x000031C9
		protected IntRange ReCheckFailedBillTicksRange { get; }

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000045 RID: 69 RVA: 0x00004FD1 File Offset: 0x000031D1
		protected string MissingMaterialsTranslated { get; }

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000046 RID: 70 RVA: 0x00004FD9 File Offset: 0x000031D9
		// (set) Token: 0x06000047 RID: 71 RVA: 0x00004FE1 File Offset: 0x000031E1
		protected List<Thing> RelevantThings { get; set; }

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000048 RID: 72 RVA: 0x00004FEA File Offset: 0x000031EA
		// (set) Token: 0x06000049 RID: 73 RVA: 0x00004FF2 File Offset: 0x000031F2
		protected HashSet<Thing> ProcessedThings { get; set; }

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600004A RID: 74 RVA: 0x00004FFB File Offset: 0x000031FB
		// (set) Token: 0x0600004B RID: 75 RVA: 0x00005003 File Offset: 0x00003203
		protected List<Thing> NewRelevantThings { get; set; }

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600004C RID: 76 RVA: 0x0000500C File Offset: 0x0000320C
		// (set) Token: 0x0600004D RID: 77 RVA: 0x00005014 File Offset: 0x00003214
		protected List<IngredientCount> IngredientsOrdered { get; set; }

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600004E RID: 78 RVA: 0x0000501D File Offset: 0x0000321D
		// (set) Token: 0x0600004F RID: 79 RVA: 0x00005025 File Offset: 0x00003225
		protected List<Thing> TmpMedicine { get; set; }

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000050 RID: 80 RVA: 0x0000502E File Offset: 0x0000322E
		// (set) Token: 0x06000051 RID: 81 RVA: 0x00005036 File Offset: 0x00003236
		protected int Lf_adjacentRegionsAvailable { get; set; }

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000052 RID: 82 RVA: 0x0000503F File Offset: 0x0000323F
		// (set) Token: 0x06000053 RID: 83 RVA: 0x00005047 File Offset: 0x00003247
		protected int Lf_regionsProcessed { get; set; }

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000054 RID: 84 RVA: 0x00005050 File Offset: 0x00003250
		// (set) Token: 0x06000055 RID: 85 RVA: 0x00005058 File Offset: 0x00003258
		protected IntVec3 Lf_rootCell { get; set; }

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000056 RID: 86 RVA: 0x00005061 File Offset: 0x00003261
		// (set) Token: 0x06000057 RID: 87 RVA: 0x00005069 File Offset: 0x00003269
		protected Bill P_bill { get; set; }

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000058 RID: 88 RVA: 0x00005072 File Offset: 0x00003272
		// (set) Token: 0x06000059 RID: 89 RVA: 0x0000507A File Offset: 0x0000327A
		protected Pawn P_pawn { get; set; }

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600005A RID: 90 RVA: 0x00005083 File Offset: 0x00003283
		// (set) Token: 0x0600005B RID: 91 RVA: 0x0000508B File Offset: 0x0000328B
		protected Thing P_billGiver { get; set; }

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x0600005C RID: 92 RVA: 0x00005094 File Offset: 0x00003294
		// (set) Token: 0x0600005D RID: 93 RVA: 0x0000509C File Offset: 0x0000329C
		protected List<ThingCount> P_chosen { get; set; }

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x0600005E RID: 94 RVA: 0x000050A5 File Offset: 0x000032A5
		// (set) Token: 0x0600005F RID: 95 RVA: 0x000050AD File Offset: 0x000032AD
		protected bool Lf_foundAll { get; set; }

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000060 RID: 96 RVA: 0x000050B6 File Offset: 0x000032B6
		protected virtual bool ApplyToParameter { get; } = true;

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000061 RID: 97 RVA: 0x000050BE File Offset: 0x000032BE
		protected virtual bool CopyOnOverride { get; }

		// Token: 0x06000063 RID: 99 RVA: 0x000050D5 File Offset: 0x000032D5
		private void FetchStaticFields(List<ThingCount> _chosenIngThings, List<Thing> _relevantThings, HashSet<Thing> _processedThings, List<Thing> _newRelevantThings, List<IngredientCount> _ingredientsOrdered)
		{
			this.ChosenIngThings = _chosenIngThings;
			this.RelevantThings = _relevantThings;
			this.ProcessedThings = _processedThings;
			this.NewRelevantThings = _newRelevantThings;
			this.IngredientsOrdered = _ingredientsOrdered;
		}

		// Token: 0x06000064 RID: 100 RVA: 0x000050FC File Offset: 0x000032FC
		private void FetchLocalFields(int lf_adjacentRegionsAvailable, int lf_regionsProcessed, IntVec3 lf_rootCell, Bill p_bill, Pawn p_pawn, Thing p_billGiver, List<ThingCount> p_chosen, bool lf_foundAll)
		{
			this.Lf_adjacentRegionsAvailable = lf_adjacentRegionsAvailable;
			this.Lf_regionsProcessed = lf_regionsProcessed;
			this.Lf_rootCell = lf_rootCell;
			this.P_bill = p_bill;
			this.P_pawn = p_pawn;
			this.P_billGiver = p_billGiver;
			this.P_chosen = p_chosen;
			this.Lf_foundAll = lf_foundAll;
		}

		// Token: 0x06000065 RID: 101
		protected abstract bool RegionProcessor(Region reg);

		// Token: 0x06000066 RID: 102 RVA: 0x0000513B File Offset: 0x0000333B
		private void UpdateData(ref Bill bill, ref Pawn pawn, ref Thing billGiver, ref List<ThingCount> chosen, ref bool foundAll)
		{
			if (this.ApplyToParameter)
			{
				bill = this.P_bill;
				pawn = this.P_pawn;
				billGiver = this.P_billGiver;
				chosen = this.P_chosen;
				foundAll = this.Lf_foundAll;
			}
		}

		// Token: 0x04000008 RID: 8
		public static RegionProcessorSubtitution singleton;

		// Token: 0x04000009 RID: 9
		public static readonly string FetchLocalFieldsMethodName = "FetchLocalFields";

		// Token: 0x0400000A RID: 10
		public static readonly string FetchStaticFieldsMethodName = "FetchStaticFields";

		// Token: 0x0400000B RID: 11
		public static readonly string UpdateDataName = "UpdateData";
	}
}

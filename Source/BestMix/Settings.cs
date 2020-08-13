using System;
using UnityEngine;
using Verse;

namespace BestMix
{
	// Token: 0x0200000E RID: 14
	public class Settings : ModSettings
	{
		// Token: 0x0600006A RID: 106 RVA: 0x00005398 File Offset: 0x00003598
		public void DoWindowContents(Rect canvas)
		{
			float gap = 10f;
			Listing_Standard listing_Standard = new Listing_Standard();
			listing_Standard.ColumnWidth = canvas.width;
			listing_Standard.Begin(canvas);
			listing_Standard.Gap(gap);
			listing_Standard.CheckboxLabeled("BestMix.AllowBestMix".Translate(), ref this.AllowBestMix, null);
			listing_Standard.Gap(gap);
			listing_Standard.CheckboxLabeled("BestMix.AllowMealMakersOnly".Translate(), ref this.AllowMealMakersOnly, null);
			listing_Standard.Gap(gap * 2f);
			if (this.useStock)
			{
				listing_Standard.CheckboxLabeled("BestMix.MapStock".Translate(), ref this.mapStock, null);
				listing_Standard.Gap(gap);
				listing_Standard.CheckboxLabeled("BestMix.InStorage".Translate(), ref this.inStorage, null);
				listing_Standard.Gap(gap);
			}
			listing_Standard.Gap(gap);
			checked
			{
				if (this.adjBillBMPos)
				{
					listing_Standard.Label("BestMix.BillBMPos".Translate() + "  " + (int)this.BillBMPos, -1f, null);
					this.BillBMPos = (float)((int)listing_Standard.Slider((float)((int)this.BillBMPos), 150f, 200f));
					listing_Standard.Gap(gap);
				}
				listing_Standard.Gap(gap);
				if (this.RadiusRestrict)
				{
					listing_Standard.CheckboxLabeled("BestMix.UseRadiusLimit".Translate(), ref this.UseRadiusLimit, null);
					listing_Standard.Gap(gap);
					listing_Standard.Label("BestMix.RadiusLimit".Translate() + "  " + this.RadiusLimit, -1f, null);
					this.RadiusLimit = (int)listing_Standard.Slider((float)this.RadiusLimit, 10f, 100f);
					listing_Standard.Gap(gap);
				}
			}
			if (Prefs.DevMode && this.DebugMaster)
			{
				listing_Standard.Gap(gap * 2f);
				listing_Standard.CheckboxLabeled("BestMix.IncludeRegionLimiter".Translate(), ref this.IncludeRegionLimiter, null);
				listing_Standard.Gap(gap * 2f);
				listing_Standard.CheckboxLabeled("BestMix.DebugSort".Translate(), ref this.DebugSort, null);
				listing_Standard.Gap(gap);
				listing_Standard.CheckboxLabeled("BestMix.DebugChosen".Translate(), ref this.DebugChosen, null);
				listing_Standard.Gap(gap);
				listing_Standard.CheckboxLabeled("BestMix.DebugFound".Translate(), ref this.DebugFound, null);
				listing_Standard.Gap(gap * 2f);
				listing_Standard.CheckboxLabeled("BestMix.DebugIgnore".Translate(), ref this.DebugIgnore, null);
				listing_Standard.Gap(gap);
			}
			listing_Standard.End();
		}

		// Token: 0x0600006B RID: 107 RVA: 0x00005644 File Offset: 0x00003844
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.AllowBestMix, "AllowBestMix", true, false);
			Scribe_Values.Look<bool>(ref this.AllowMealMakersOnly, "AllowMealMakersOnly", false, false);
			Scribe_Values.Look<bool>(ref this.AllowBMBillMaxSet, "AllowDMBillMaxSet", true, false);
			Scribe_Values.Look<bool>(ref this.mapStock, "mapStock", false, false);
			Scribe_Values.Look<bool>(ref this.inStorage, "inStorage", true, false);
			Scribe_Values.Look<bool>(ref this.UseRadiusLimit, "UseRadiusLimit", false, false);
			Scribe_Values.Look<int>(ref this.RadiusLimit, "RadiusLimit", 100, false);
			Scribe_Values.Look<bool>(ref this.IncludeRegionLimiter, "IncludeRegionLimiter", true, false);
			Scribe_Values.Look<bool>(ref this.DebugSort, "DebugSort", false, false);
			Scribe_Values.Look<bool>(ref this.DebugChosen, "DebugChosen", false, false);
			Scribe_Values.Look<bool>(ref this.DebugFound, "DebugFound", false, false);
			Scribe_Values.Look<bool>(ref this.DebugIgnore, "DebugIgnore", false, false);
		}

		// Token: 0x0400001E RID: 30
		public bool AllowBestMix = true;

		// Token: 0x0400001F RID: 31
		public bool AllowMealMakersOnly;

		// Token: 0x04000020 RID: 32
		public bool AllowBMBillMaxSet = true;

		// Token: 0x04000021 RID: 33
		public bool UseRadiusLimit;

		// Token: 0x04000022 RID: 34
		public int RadiusLimit = 100;

		// Token: 0x04000023 RID: 35
		public bool useStock = true;

		// Token: 0x04000024 RID: 36
		public bool mapStock;

		// Token: 0x04000025 RID: 37
		public bool inStorage = true;

		// Token: 0x04000026 RID: 38
		public bool adjBillBMPos;

		// Token: 0x04000027 RID: 39
		public float BillBMPos = 150f;

		// Token: 0x04000028 RID: 40
		public bool IncludeRegionLimiter = true;

		// Token: 0x04000029 RID: 41
		public bool DebugSort;

		// Token: 0x0400002A RID: 42
		public bool DebugChosen;

		// Token: 0x0400002B RID: 43
		public bool DebugFound;

		// Token: 0x0400002C RID: 44
		public bool DebugIgnore;

		// Token: 0x0400002D RID: 45
		public bool DebugMaster;

		// Token: 0x0400002E RID: 46
		public bool RadiusRestrict;
	}
}

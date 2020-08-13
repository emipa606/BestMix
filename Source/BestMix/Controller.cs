using System;
using UnityEngine;
using Verse;

namespace BestMix
{
	// Token: 0x02000008 RID: 8
	public class Controller : Mod
	{
		// Token: 0x06000037 RID: 55 RVA: 0x00004DEA File Offset: 0x00002FEA
		public override string SettingsCategory()
		{
			return "BestMix.Name".Translate();
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00004DFB File Offset: 0x00002FFB
		public override void DoSettingsWindowContents(Rect canvas)
		{
			Controller.Settings.DoWindowContents(canvas);
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00004E08 File Offset: 0x00003008
		public Controller(ModContentPack content) : base(content)
		{
			Controller.Settings = base.GetSettings<Settings>();
		}

		// Token: 0x04000006 RID: 6
		public static Settings Settings;
	}
}

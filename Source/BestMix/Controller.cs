﻿using UnityEngine;
using Verse;

namespace BestMix;

public class Controller : Mod
{
    public static Settings Settings;

    public Controller(ModContentPack content) : base(content)
    {
        Settings = GetSettings<Settings>();
    }

    public override string SettingsCategory()
    {
        return "BestMix.Name".Translate();
    }

    public override void DoSettingsWindowContents(Rect canvas)
    {
        Settings.DoWindowContents(canvas);
    }
}
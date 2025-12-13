using System;

[Serializable]
public class SettingsData
{
    // Per-setting saved state:
    // - activePresetIndex[i] is:
    //    - -1 => custom input was active
    //    - 0..2 => which preset toggle was active for setting i
    public int[] activePresetIndex = new int[3] { -1, -1, -1 };

    // lastInputText[i] stores the raw input text for setting i (custom or preset text)
    public string[] lastInputText = new string[3] { "", "", "" };

    // The runtime numeric values we persist too (keeps GlobalHandler safe)
    public float startingBreakTimeSeconds = 5.0f;
    public float maxTimeSeconds = 30 * 60.0f;
    public float fillRatio = 0.02f;
}

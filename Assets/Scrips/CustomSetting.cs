using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

/// <summary>
/// Attach one instance of this component for each setting (starting time, ratio, max time).
/// Configuration in inspector:
///  - input: TMP_InputField for custom value
///  - myToggle: the "custom" toggle (last toggle) to indicate the input is active
///  - optionToggles: array of 4 toggles: first 3 are preset options, index 3 is the custom toggle (same as myToggle)
///  - presetStrings: array length 3 with display strings for the presets (e.g. "5", "1:30", "25")
///  - setting: 0 = starting time, 1 = fill ratio, 2 = max time
/// </summary>
public class CustomSettings : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_InputField input;
    [SerializeField] private Toggle myToggle;                       // custom toggle (should be optionToggles[3])
    [SerializeField] private Toggle[] optionToggles;                // expected length 4 (3 presets + 1 custom)
    [SerializeField] private string[] presetStrings = new string[3];// display strings for the 3 presets
    [Tooltip("0 = starting time, 1 = fill ratio, 2 = max time")]
    public int setting;

    // parsed runtime values
    private float minutes;
    private float hours;

    private void Awake()
    {
        // Basic safety checks
        if (optionToggles == null || optionToggles.Length != 4)
            Debug.LogWarning($"[CustomSettings] optionToggles should be 4 (3 presets + 1 custom). Current length: {(optionToggles == null ? 0 : optionToggles.Length)}");

        if (presetStrings == null || presetStrings.Length != 3)
            Debug.LogWarning($"[CustomSettings] presetStrings should be length 3. Current length: {(presetStrings == null ? 0 : presetStrings.Length)}");
    }

    private void Start()
    {
        // Remove old listeners to avoid duplicates
        if (input != null)
        {
            input.onValueChanged.RemoveAllListeners();
            input.onValueChanged.AddListener(OnInputValueChanged);
        }

        // Wire preset toggles
        if (optionToggles != null)
        {
            for (int i = 0; i < optionToggles.Length; i++)
            {
                int captured = i; // capture for closure
                if (optionToggles[i] != null)
                {
                    optionToggles[i].onValueChanged.RemoveAllListeners();
                    optionToggles[i].onValueChanged.AddListener(isOn => OnOptionToggleChanged(captured, isOn));
                }
            }
        }

        // Ensure myToggle references the custom toggle if available
        if (myToggle == null && optionToggles != null && optionToggles.Length == 4)
            myToggle = optionToggles[3];

        // Populate UI from saved settings
        ApplySavedToUI();
    }

    /// <summary>
    /// Called when any preset/custom toggle changes.
    /// presetIndex: 0..2 = preset toggles, 3 = custom toggle
    /// </summary>
    private void OnOptionToggleChanged(int presetIndex, bool isOn)
    {
        // Only react when toggled on (we treat toggles like radio buttons)
        if (!isOn) return;

        // Turn off other toggles in the group (simple radio behavior)
        if (optionToggles != null)
        {
            for (int i = 0; i < optionToggles.Length; i++)
            {
                if (i != presetIndex && optionToggles[i] != null)
                    optionToggles[i].isOn = false;
            }
        }

        // If it's a preset, set input to that preset's string and apply
        if (presetIndex >= 0 && presetIndex <= 2)
        {
            string presetText = "0";
            if (presetStrings != null && presetStrings.Length > presetIndex)
                presetText = presetStrings[presetIndex];

            // When setting text programmatically, we want to avoid echoing back a callback loop.
            if (input != null)
                input.SetTextWithoutNotify(presetText);

            // Parse and apply the preset
            ParseInputAndApply(presetText);

            // make custom toggle reflect it's not active
            if (myToggle != null)
                myToggle.isOn = false;
        }
        else // presetIndex == 3 -> custom toggle selected
        {
            // enable the input (if you want to enable/disable interactability)
            if (input != null)
                input.interactable = true;

            // if input has content, parse and apply it
            if (input != null && !string.IsNullOrWhiteSpace(input.text))
                ParseInputAndApply(input.text);
        }
    }

    private void OnInputValueChanged(string value)
    {
        // If input changed, mark the custom toggle on and others off
        if (myToggle != null)
            myToggle.isOn = true;

        if (optionToggles != null)
        {
            for (int i = 0; i < optionToggles.Length; i++)
            {
                if (i != 3 && optionToggles[i] != null) // keep presets off
                    optionToggles[i].isOn = false;
            }
        }

        ParseInputAndApply(value);
    }

    /// <summary>
    /// Parses the string (either "HH:MM" or a single number) and applies to GlobalHandler.
    /// </summary>
    private void ParseInputAndApply(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            if (myToggle != null) myToggle.isOn = false;
            return;
        }

        string hourText = "0";
        string minuteText = "0";

        // Handle HH:MM format
        if (value.Contains(":"))
        {
            var parts = value.Split(':');
            if (parts.Length >= 1 && parts[0].Length > 0)
                hourText = parts[0];

            if (parts.Length >= 2 && parts[1].Length > 0)
                minuteText = parts[1];
        }
        else
        {
            minuteText = value;
        }

        // Safe parse
        if (!float.TryParse(hourText, out hours))
            hours = 0f;
        if (!float.TryParse(minuteText, out minutes))
            minutes = 0f;

        // Apply to the selected setting
        changeSetting(setting);
    }

    private string SecondsToDisplayString(float totalSeconds)
    {
        int totalMinutes = Mathf.FloorToInt(totalSeconds / 60f);
        int h = totalMinutes / 60;
        int m = totalMinutes % 60;
        return $"{h}:{m:D2}";
    }

    public void changeSetting(int whichSetting)
    {
        float totalSeconds = hours * 3600f + minutes * 60f;

        switch (whichSetting)
        {
            case 0: // starting time
                GlobalHandler.Instance.SetStartingTime(totalSeconds);
                break;

            case 1: // fill ratio (we interpret the input as minutes/hours or as a single number -> numerator)
                float numerator = minutes;
                float denominator = hours;

                if (input != null && !input.text.Contains(":"))
                    denominator = 1f;

                if (denominator == 0f)
                {
                    Debug.LogWarning("Ratio denominator is 0 — defaulting to 1.");
                    denominator = 1f;
                }

                float ratio = numerator / denominator;
                GlobalHandler.Instance.SetFillRatio(ratio);
                break;

            case 2: // max time
                GlobalHandler.Instance.SetMaxTime(totalSeconds);
                break;
        }
    }

    /// <summary>
    /// Examine saved values and apply them to the UI.
    /// If the saved value matches one of the presets (numerically), select that preset toggle.
    /// Otherwise set custom toggle and populate input.
    /// </summary>
    private void ApplySavedToUI()
    {
        if (SettingsDataManager.Instance == null)
        {
            Debug.Log("[CustomSettings] SettingsDataManager missing at Start; using defaults.");
            return;
        }

        var s = SettingsDataManager.Instance.Settings;

        // valueToCompare is the numeric stored value we want to match against presets
        float valueToCompare = 0f;
        switch (setting)
        {
            case 0:
                valueToCompare = s.startingBreakTimeSeconds;
                break;
            case 1:
                valueToCompare = s.fillRatio;
                break;
            case 2:
                valueToCompare = s.maxTimeSeconds;
                break;
        }

        // Try to match presets (convert each preset string to numeric using same parsing)
        bool matchedPreset = false;
        if (presetStrings != null && presetStrings.Length >= 3)
        {
            for (int i = 0; i < 3; i++)
            {
                float presetNumeric = ConvertPresetStringToStoredNumeric(presetStrings[i], setting);
                // For float comparisons use an epsilon
                if (Mathf.Abs(presetNumeric - valueToCompare) < 0.001f)
                {
                    // turn that preset toggle on
                    if (optionToggles != null && optionToggles.Length > i && optionToggles[i] != null)
                        optionToggles[i].isOn = true;

                    // populate input with display form for clarity (but keep custom toggle off)
                    if (input != null)
                        input.SetTextWithoutNotify(presetStrings[i]);

                    matchedPreset = true;
                    break;
                }
            }
        }

        if (!matchedPreset)
        {
            // Mark custom toggle on and set the input text to the saved value
            if (optionToggles != null && optionToggles.Length > 3 && optionToggles[3] != null)
                optionToggles[3].isOn = true;

            if (input != null)
            {
                // Display numeric properly: if we store seconds for times, format that
                if (setting == 0 || setting == 2)
                    input.SetTextWithoutNotify(SecondsToDisplayString(valueToCompare));
                else // ratio: just show the numeric ratio
                    input.SetTextWithoutNotify(valueToCompare.ToString("0.###"));
            }
        }
    }

    /// <summary>
    /// Convert a preset display string to the numeric stored form used in settings.json.
    /// For settings 0 and 2 (times) the stored value is seconds.
    /// For setting 1 (ratio) the stored value is ratio (float).
    /// The preset strings should be in "HH:MM" or "MM" or a numeric format suitable for a ratio.
    /// </summary>
    private float ConvertPresetStringToStoredNumeric(string preset, int whichSetting)
    {
        if (string.IsNullOrWhiteSpace(preset)) return 0f;

        if (whichSetting == 1)
        {
            // For ratios, try parse directly
            if (float.TryParse(preset, out float r))
                return r;
            return 0f;
        }
        else
        {
            // For times, parse HH:MM or single minutes value -> convert to seconds
            string hourText = "0";
            string minuteText = "0";
            if (preset.Contains(":"))
            {
                var parts = preset.Split(':');
                if (parts.Length >= 1 && parts[0].Length > 0) hourText = parts[0];
                if (parts.Length >= 2 && parts[1].Length > 0) minuteText = parts[1];
            }
            else
            {
                minuteText = preset;
            }

            if (!float.TryParse(hourText, out float h)) h = 0f;
            if (!float.TryParse(minuteText, out float m)) m = 0f;

            return h * 3600f + m * 60f;
        }
    }
}

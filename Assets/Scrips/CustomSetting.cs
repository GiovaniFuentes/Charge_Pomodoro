using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NewMonoBehaviourScript : MonoBehaviour
{
    [SerializeField] private TMP_InputField input;
    [SerializeField] private Toggle myToggle;
    public int setting;

    private float minutes;
    private float hours;

    private void Start()
    {
        input.onValueChanged.AddListener(OnInputValueChanged);
    }

    private void OnInputValueChanged(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            myToggle.isOn = false;
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
        float.TryParse(hourText, out hours);
        float.TryParse(minuteText, out minutes);



        changeSetting(setting);
        myToggle.isOn = true;
    }

    public void changeSetting(int setting)
    {
        float totalSeconds = hours * 3600f + minutes * 60f;

        switch (setting)
        {
            case 0:
                GlobalHandler.Instance.SetStartingTime(totalSeconds);
                break;

            case 1:
                float numerator = minutes;
                float denominator = hours; 

                if (!input.text.Contains(":"))
                    denominator = 1f;

                // Prevent division by zero
                if (denominator == 0)
                {
                    Debug.LogWarning("Ratio denominator is 0 — defaulting to 1.");
                    denominator = 1f;
                }

                float ratio = numerator / denominator;

                GlobalHandler.Instance.SetFillRatio(ratio);
                break;

            case 2:
                GlobalHandler.Instance.SetMaxTime(totalSeconds);
                break;
        }
    }

}

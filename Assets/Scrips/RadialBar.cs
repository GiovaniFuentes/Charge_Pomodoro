using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class RadialBar : MonoBehaviour
{
    [SerializeField] Image radialIndicator = null;
    [SerializeField] TMP_Text digitalIndicator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        radialIndicator.fillAmount = GlobalHandler.Instance.currentTimeLeft / GlobalHandler.Instance.maxTime;
        int hours   = Mathf.FloorToInt(GlobalHandler.Instance.currentTimeLeft / 3600f);
        int minutes = Mathf.FloorToInt((GlobalHandler.Instance.currentTimeLeft % 3600f) / 60f);
        int seconds = Mathf.FloorToInt(GlobalHandler.Instance.currentTimeLeft % 60f);
        digitalIndicator.text = string.Format("{0:D2}:{1:D2}:{2:D2}", hours, minutes, seconds);
    }
}

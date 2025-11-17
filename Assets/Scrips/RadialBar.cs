using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class RadialBar : MonoBehaviour
{
    [SerializeField] Image radialIndicator = null;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        radialIndicator.fillAmount = GlobalHandler.Instance.currentTimeLeft / GlobalHandler.Instance.maxTime;
    }
}

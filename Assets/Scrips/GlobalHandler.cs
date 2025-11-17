using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GlobalHandler : MonoBehaviour
{
    [SerializeField] float currentTimeLeft = 0.0f;
    [SerializeField] float maxTime = 10.0f;
    [SerializeField] float startingBreakTime = 5.0f;
    [SerializeField] float fillRatio =0.02f;
    [SerializeField] Image radialIndicator = null;
    public bool TimeFill = false;
    //awake is called when the object is created directly (I think)
    void Awake()
    {
        //this is to make the object persistent across scenes
        DontDestroyOnLoad(gameObject);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentTimeLeft = startingBreakTime; 
    }

    // Update is called once per frame
    void Update()
    {
        if(TimeFill && currentTimeLeft < maxTime)
        {
            currentTimeLeft += fillRatio * Time.deltaTime;
        }
        else
        {
            if(currentTimeLeft > 0)
            {
                currentTimeLeft -= fillRatio * Time.deltaTime;
            }
        }
        radialIndicator.fillAmount = currentTimeLeft / maxTime;
    }

    public void ToggleTimeFill()
    {
        TimeFill = !TimeFill;
    }

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}

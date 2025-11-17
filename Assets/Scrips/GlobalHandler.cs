using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GlobalHandler : MonoBehaviour
{
    public static GlobalHandler Instance;
    [SerializeField] public float currentTimeLeft = 0.0f;
    [SerializeField] public float maxTime = 10.0f;
    [SerializeField] float startingBreakTime = 5.0f;
    [SerializeField] float fillRatio =0.02f;
    public bool TimeFill = false;
    //awake is called when the object is created directly (I think)
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentTimeLeft = startingBreakTime; 
        ChangeScene("Main Page");
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
        
    }

    static public void ToggleTimeFill()
    {
        Instance.TimeFill = !Instance.TimeFill;
    }

    static public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}

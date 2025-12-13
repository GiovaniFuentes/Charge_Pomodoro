using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GlobalHandler : MonoBehaviour
{
    public static GlobalHandler Instance;

    // These are the runtime values used by the app
    [SerializeField] public float currentTimeLeft = 0.0f;
    [SerializeField] public float maxTime = 30 * 60.0f;
    [SerializeField] float startingBreakTime = 5.0f;
    [SerializeField] float fillRatio = 0.02f;

    public bool TimeFill = false;

    // Awake: singleton pattern
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

    // Start: load persisted settings (if SettingsDataManager exists) and initialize currentTimeLeft
    void Start()
    {
        // Apply saved settings if available
        if (SettingsDataManager.Instance != null)
        {
            var s = SettingsDataManager.Instance.Settings;

            // Only overwrite if values look valid; this keeps your serialized defaults safe
            startingBreakTime = s.startingBreakTimeSeconds;
            maxTime = s.maxTimeSeconds;
            fillRatio = s.fillRatio;

            Debug.Log("[GlobalHandler] Applied saved settings.");
        }
        else
        {
            Debug.Log("[GlobalHandler] SettingsDataManager not found; using defaults.");
        }

        currentTimeLeft = startingBreakTime;
        ChangeScene("Main Page");
    }

    // Update runs every frame
    void Update()
    {
        if (TimeFill && currentTimeLeft < maxTime)
        {
            currentTimeLeft += Time.deltaTime * fillRatio;
        }
        else
        {
            if (currentTimeLeft > 0)
            {
                currentTimeLeft -= Time.deltaTime;
            }
            if (currentTimeLeft < 0)
            {
                currentTimeLeft = 0;
            }
        }

        // Check for Escape to quit build
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
            Debug.Log("Application quit.");
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

    // NOTE: these setters now persist to settings.json via SettingsDataManager (if present)

    public void SetStartingTime(float _startingBreakTime)
    {
        Instance.startingBreakTime = _startingBreakTime;

        // Update currentTimeLeft only if you want to reset immediately; currently not changing currentTimeLeft
        // Instance.currentTimeLeft = _startingBreakTime;

        // Persist
        if (SettingsDataManager.Instance != null)
        {
            var s = SettingsDataManager.Instance.Settings;
            s.startingBreakTimeSeconds = _startingBreakTime;
            SettingsDataManager.Instance.Save();
            Debug.Log("[GlobalHandler] Saved startingBreakTime to settings.json");
        }
    }

    public void SetMaxTime(float _maxTime)
    {
        Instance.maxTime = _maxTime;

        if (SettingsDataManager.Instance != null)
        {
            var s = SettingsDataManager.Instance.Settings;
            s.maxTimeSeconds = _maxTime;
            SettingsDataManager.Instance.Save();
            Debug.Log("[GlobalHandler] Saved maxTime to settings.json");
        }
    }

    public void SetFillRatio(float _fillRatio)
    {
        Instance.fillRatio = _fillRatio;

        if (SettingsDataManager.Instance != null)
        {
            var s = SettingsDataManager.Instance.Settings;
            s.fillRatio = _fillRatio;
            SettingsDataManager.Instance.Save();
            Debug.Log("[GlobalHandler] Saved fillRatio to settings.json");
        }
    }
}

using System.IO;
using UnityEngine;

public class SettingsDataManager : MonoBehaviour
{
    public static SettingsDataManager Instance { get; private set; }

    public SettingsData Settings = new SettingsData();

    private string SaveFilePath => Path.Combine(Application.persistentDataPath, "settings.json");

    // Ensure this manager exists before any scene loads
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void EnsureExistsBeforeSceneLoad()
    {
        if (Instance != null) return;

        // If already in the scene (rare), find it first
        var existing = FindObjectOfType<SettingsDataManager>();
        if (existing != null)
        {
            Instance = existing;
            return;
        }

        // Otherwise create one so Awake() will run and set Instance
        var go = new GameObject("SettingsDataManager");
        DontDestroyOnLoad(go);
        go.AddComponent<SettingsDataManager>();
        // Awake will run automatically for this new GameObject
    }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        Load();
    }

    public void Save()
    {
        try
        {
            string json = JsonUtility.ToJson(Settings, true);
            File.WriteAllText(SaveFilePath, json);
            Debug.Log("[SettingsDataManager] Saved settings to: " + SaveFilePath);
        }
        catch (System.Exception e)
        {
            Debug.LogWarning("[SettingsDataManager] Save failed: " + e);
        }
    }

    public void Load()
    {
        try
        {
            if (File.Exists(SaveFilePath))
            {
                string json = File.ReadAllText(SaveFilePath);
                Settings = JsonUtility.FromJson<SettingsData>(json) ?? new SettingsData();
                Debug.Log("[SettingsDataManager] Loaded settings from: " + SaveFilePath);
            }
            else
            {
                Settings = new SettingsData();
                Debug.Log("[SettingsDataManager] No settings file found. Using defaults.");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogWarning("[SettingsDataManager] Load failed: " + e);
            Settings = new SettingsData();
        }
    }

    public void ResetToDefaults()
    {
        Settings = new SettingsData();
        Save();
    }
}

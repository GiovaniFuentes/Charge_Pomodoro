using System.IO;
using UnityEngine;

public class TaskDataManager : MonoBehaviour
{
    public static TaskDataManager Instance { get; private set; }

    public AllTasksData AllTasks = new AllTasksData();

    private string SaveFilePath => Path.Combine(Application.persistentDataPath, "tasks.json");

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
            string json = JsonUtility.ToJson(AllTasks, true);
            File.WriteAllText(SaveFilePath, json);
            Debug.Log("[TaskDataManager] Saved tasks to: " + SaveFilePath);
        }
        catch (System.Exception e)
        {
            Debug.LogWarning("[TaskDataManager] Save failed: " + e);
        }
    }

    public void Load()
    {
        try
        {
            if (File.Exists(SaveFilePath))
            {
                string json = File.ReadAllText(SaveFilePath);
                AllTasks = JsonUtility.FromJson<AllTasksData>(json) ?? new AllTasksData();
                Debug.Log($"[TaskDataManager] Loaded {AllTasks.mainTasks.Count} main tasks from {SaveFilePath}");
            }
            else
            {
                AllTasks = new AllTasksData();
                Debug.Log("[TaskDataManager] No save file found, starting with empty list.");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogWarning("[TaskDataManager] Load failed: " + e);
            AllTasks = new AllTasksData();
        }
    }

    // ----------------------
    // Public helpers
    // ----------------------

    /// <summary>
    /// Adds a new main task and returns its index
    /// </summary>
    public int AddMainTask(string title)
    {
        var mt = new MainTaskData { title = title };
        AllTasks.mainTasks.Add(mt);
        Save();
        return AllTasks.mainTasks.Count - 1;
    }

    /// <summary>
    /// Adds a subtask under a main task
    /// </summary>
    public void AddSubtask(int mainIndex, string subtext)
    {
        if (!IsValidMainIndex(mainIndex)) return;

        AllTasks.mainTasks[mainIndex].subtasks.Add(new SubtaskData { text = subtext, done = false });
        Save();
    }

    /// <summary>
    /// Update the title of a main task
    /// </summary>
    public void SetMainTitle(int mainIndex, string newTitle)
    {
        if (!IsValidMainIndex(mainIndex)) return;

        AllTasks.mainTasks[mainIndex].title = newTitle;
        Save();
    }

    /// <summary>
    /// Toggle whether a subtask is done
    /// </summary>
    public void ToggleSubtaskDone(int mainIndex, int subIndex, bool done)
    {
        if (!IsValidSubtaskIndex(mainIndex, subIndex)) return;

        AllTasks.mainTasks[mainIndex].subtasks[subIndex].done = done;
        Save();
    }

    /// <summary>
    /// Remove a main task
    /// </summary>
    public void RemoveMainTask(int mainIndex)
    {
        if (!IsValidMainIndex(mainIndex)) return;

        AllTasks.mainTasks.RemoveAt(mainIndex);
        Save();
    }

    /// <summary>
    /// Remove a subtask
    /// </summary>
    public void RemoveSubtask(int mainIndex, int subIndex)
    {
        if (!IsValidSubtaskIndex(mainIndex, subIndex)) return;

        AllTasks.mainTasks[mainIndex].subtasks.RemoveAt(subIndex);
        Save();
    }

    // ----------------------
    // Validation helpers
    // ----------------------
    public bool IsValidMainIndex(int index) => index >= 0 && index < AllTasks.mainTasks.Count;
    private bool IsValidSubtaskIndex(int mainIndex, int subIndex) =>
        IsValidMainIndex(mainIndex) && subIndex >= 0 && subIndex < AllTasks.mainTasks[mainIndex].subtasks.Count;
}

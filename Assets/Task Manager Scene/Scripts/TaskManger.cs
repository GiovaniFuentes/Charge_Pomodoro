using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages the UI for all main tasks and subtasks
/// </summary>
public class TaskManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject mainTaskPrefab;   // Assign MainTask prefab
    public Transform mainTaskContent;   // Content inside ScrollView
    public Button addMainButton;        // + button for main tasks

    private void Start()
    {
        if (addMainButton != null)
            addMainButton.onClick.AddListener(OnAddMainTask);

        BuildUIFromData();
    }

    /// <summary>
    /// Builds the UI from saved TaskDataManager data
    /// </summary>
    private void BuildUIFromData()
    {
        if (mainTaskPrefab == null || mainTaskContent == null) return;

        // Clear existing children
        foreach (Transform child in mainTaskContent)
        {
            Destroy(child.gameObject);
        }

        var allTasks = TaskDataManager.Instance.AllTasks;

        for (int i = 0; i < allTasks.mainTasks.Count; i++)
        {
            var taskData = allTasks.mainTasks[i];

            // Create MainTask UI
            GameObject mainGO = Instantiate(mainTaskPrefab, mainTaskContent);
            MainTask mt = mainGO.GetComponent<MainTask>();
            if (mt != null)
            {
                mt.SetDataIndex(i);
                mt.Initialize(taskData.title);

                // Create subtasks with proper indices and state
                for (int j = 0; j < taskData.subtasks.Count; j++)
                {
                    var sub = taskData.subtasks[j];
                    mt.CreateSubtask(sub.text, sub.done, j);
                }
            }
        }
    }

    /// <summary>
    /// Adds a new main task when user clicks + button
    /// </summary>
    private void OnAddMainTask()
    {
        // Add to data first
        int mainIndex = TaskDataManager.Instance.AddMainTask("New Task");

        // Create MainTask UI
        GameObject mainGO = Instantiate(mainTaskPrefab, mainTaskContent);
        MainTask mt = mainGO.GetComponent<MainTask>();
        if (mt != null)
        {
            mt.SetDataIndex(mainIndex);
            mt.Initialize("New Task");
        }
    }
}







/*
using UnityEngine;
using UnityEngine.UI;

public class TaskManager : MonoBehaviour
{
    public GameObject mainTaskPrefab;    // assign MainTaskPrefab
    public Transform mainTaskContent;    // assign the Content object inside ScrollView
    public Button addMainButton;

    private int mainTaskCounter = 0;

    void Start()
    {
        if (addMainButton != null)
            addMainButton.onClick.AddListener(CreateMainTask);
    }

    public void CreateMainTask()
    {
        GameObject go = Instantiate(mainTaskPrefab, mainTaskContent);
        go.transform.SetParent(mainTaskContent, false);
        mainTaskCounter++;
        // set default title if the prefab has a MainTask script
        MainTask mt = go.GetComponent<MainTask>();
        if (mt != null)
        {
            mt.Initialize("Main Task " + mainTaskCounter);
        }
    }
}
*/

using UnityEngine;
using UnityEngine.UI;

public class TaskManager : MonoBehaviour
{
    public GameObject mainTaskPrefab;
    public Transform mainTaskContent;
    public Button addMainButton;

    private void Start()
    {
        if (addMainButton != null)
            addMainButton.onClick.AddListener(AddMainTask);

        BuildAllMainTasks();
    }

    private void BuildAllMainTasks()
    {
        // Clear existing children first
        foreach (Transform child in mainTaskContent)
            Destroy(child.gameObject);

        var allTasks = TaskDataManager.Instance.AllTasks.mainTasks;

        for (int i = 0; i < allTasks.Count; i++)
        {
            GameObject mainGO = Instantiate(mainTaskPrefab, mainTaskContent);
            MainTask mt = mainGO.GetComponent<MainTask>();
            if (mt != null)
            {
                mt.SetDataIndex(i);
                mt.Initialize(allTasks[i].title);

                // Rebuild subtasks for this main task
                for (int j = 0; j < allTasks[i].subtasks.Count; j++)
                {
                    var sub = allTasks[i].subtasks[j];
                    mt.CreateSubtask(sub.text, sub.done, j);
                }
            }
        }
    }

    private void AddMainTask()
    {
        int index = TaskDataManager.Instance.AddMainTask("New Task");

        // Automatically add first subtask
        TaskDataManager.Instance.AddSubtask(index, "Enter Subtask");

        GameObject mainGO = Instantiate(mainTaskPrefab, mainTaskContent);
        MainTask mt = mainGO.GetComponent<MainTask>();
        if (mt != null)
        {
            mt.SetDataIndex(index);
            mt.Initialize("New Task");

            // Create the first subtask in UI
            mt.CreateSubtask("Enter Subtask", false, 0);
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

using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Subtask : MonoBehaviour
{
    public TMP_InputField inputField; // assign InputField in prefab
    public Toggle doneToggle;         // assign Toggle in prefab

    private int mainIndex;
    private int subIndex;

    /// <summary>
    /// Initialize the subtask with data from TaskDataManager
    /// </summary>
    public void Initialize(int mainIndex, int subIndex, string text, bool done)
    {
        this.mainIndex = mainIndex;
        this.subIndex = subIndex;

        if (inputField != null)
        {
            inputField.text = text;
            inputField.onEndEdit.RemoveAllListeners();
            inputField.onEndEdit.AddListener((string newText) =>
            {
                TaskDataManager.Instance.AllTasks.mainTasks[mainIndex]
                    .subtasks[subIndex].text = newText;
                TaskDataManager.Instance.Save();
            });
        }

        if (doneToggle != null)
        {
            doneToggle.isOn = done;
            doneToggle.onValueChanged.RemoveAllListeners();
            doneToggle.onValueChanged.AddListener((bool val) =>
            {
                // Remove subtask from data
                TaskDataManager.Instance.RemoveSubtask(mainIndex, subIndex);

                // Destroy this UI object
                Destroy(gameObject);

                // If main task has no subtasks left, remove it
                if (TaskDataManager.Instance.AllTasks.mainTasks[mainIndex].subtasks.Count == 0)
                {
                    TaskDataManager.Instance.RemoveMainTask(mainIndex);
                }

                TaskDataManager.Instance.Save();
            });
        }
    }
}






/*
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Subtask : MonoBehaviour
{
    public TMP_InputField labelInput; // assign InputField in prefab
    public Toggle doneToggle;

    private int mainIndex;
    private int subIndex;

    /// <summary>
    /// Initialize subtask with indices, text, and done state
    /// </summary>
    public void Initialize(int mainIndex, int subIndex, string text, bool done)
    {
        this.mainIndex = mainIndex;
        this.subIndex = subIndex;

        if (labelInput != null)
        {
            labelInput.text = text;
            labelInput.onEndEdit.RemoveAllListeners();
            labelInput.onEndEdit.AddListener((string newText) =>
            {
                TaskDataManager.Instance.AllTasks.mainTasks[mainIndex].subtasks[subIndex].text = newText;
                TaskDataManager.Instance.Save();
            });
        }

        if (doneToggle != null)
        {
            doneToggle.isOn = done;
            doneToggle.onValueChanged.RemoveAllListeners();
            doneToggle.onValueChanged.AddListener((bool val) =>
            {
                TaskDataManager.Instance.AllTasks.mainTasks[mainIndex].subtasks[subIndex].done = val;
                TaskDataManager.Instance.Save();
            });
        }
    }
}
*/

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



/*
using UnityEngine;
using UnityEngine.UI;

public class Subtask : MonoBehaviour
{
    public Text labelText; // assign the Text child in prefab
    public Toggle doneToggle; 

    public void Initialize(string label)
    {
        if (labelText != null) labelText.text = label;
    }
}
*/

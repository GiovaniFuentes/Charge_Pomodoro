using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.UI; // LayoutRebuilder

public class MainTask : MonoBehaviour
{
    [Header("UI References")]
    public TMP_InputField titleInput;
    public Button addSubtaskButton;
    public GameObject subtaskPrefab;
    public Transform subtaskContainer;

    private int mainIndex;
    private int subtaskCounter = 0;

    public void SetDataIndex(int index) => mainIndex = index;

    public void Initialize(string title)
    {
        if (titleInput != null)
        {
            titleInput.text = title;
            titleInput.onEndEdit.RemoveAllListeners();
            titleInput.onEndEdit.AddListener((string newText) =>
            {
                TaskDataManager.Instance.SetMainTitle(mainIndex, newText);
            });
        }

        if (addSubtaskButton != null)
        {
            addSubtaskButton.onClick.RemoveAllListeners();
            addSubtaskButton.onClick.AddListener(OnAddSubtaskClicked);
        }
    }

    private void OnAddSubtaskClicked()
    {
        string text = "Enter Subtask";
        int subIndex = TaskDataManager.Instance.AllTasks.mainTasks[mainIndex].subtasks.Count;

        // Add to data first
        TaskDataManager.Instance.AddSubtask(mainIndex, text);

        // Create UI
        CreateSubtask(text, false, subIndex);
    }

    public void CreateSubtask(string text, bool done, int subIndex)
    {
        if (subtaskPrefab == null || subtaskContainer == null) return;

        GameObject s = Instantiate(subtaskPrefab, subtaskContainer);
        s.transform.SetParent(subtaskContainer, false);
        subtaskCounter++;

        Subtask st = s.GetComponent<Subtask>();
        if (st != null)
            st.Initialize(mainIndex, subIndex, text, done);

        // Force layout rebuild
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)subtaskContainer);
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)this.transform);
    }

    /// <summary>
    /// For loading existing subtasks from JSON
    /// </summary>
    public void LoadSubtaskFromData(string text, bool done, int subIndex)
    {
        CreateSubtask(text, done, subIndex);
    }
}










/*
using UnityEngine;
using UnityEngine.UI;
using TMPro; 
public class MainTask : MonoBehaviour
{
    public TMP_InputField titleInput;          // drag InputField here
    public Button addSubtaskButton;        // small + button
    public GameObject subtaskPrefab;       // assign SubtaskPrefab
    public Transform subtaskContainer;     // SubtaskContainer transform

    private int subtaskCounter = 0;

    // called by TaskManager when creating
    public void Initialize(string title)
{
    if (titleInput != null) titleInput.text = title;

    // Wire up the button listener here
    if (addSubtaskButton != null)
    {
        addSubtaskButton.onClick.RemoveAllListeners(); // remove any old listeners
        addSubtaskButton.onClick.AddListener(CreateSubtask);
    }
}


    void Start()
    {
       intentionaly empty 
    }
    /*
/*
public void CreateSubtask()
{
    Debug.Log("Subtask button clicked!");

    if (subtaskPrefab == null || subtaskContainer == null)
    {
        Debug.LogWarning("SubtaskPrefab or Container is null!");
        return;
    }

    GameObject s = Instantiate(subtaskPrefab, subtaskContainer);
    s.transform.SetParent(subtaskContainer, false);
    subtaskCounter++;

    Subtask st = s.GetComponent<Subtask>();
    if (st != null)
    {
        st.Initialize("Subtask " + subtaskCounter);
    }

public void CreateSubtask()
    {
        Debug.Log("CreateSubtask called on " + gameObject.name);

        if (subtaskPrefab == null || subtaskContainer == null)
        {
            Debug.LogWarning("SubtaskPrefab or subtaskContainer is null on " + gameObject.name);
            return;
        }

        GameObject s = Instantiate(subtaskPrefab, subtaskContainer);
        s.transform.SetParent(subtaskContainer, false);
        subtaskCounter++;

        Debug.Log("Instantiated subtask: " + s.name + " under " + subtaskContainer.name);

        Subtask st = s.GetComponent<Subtask>();
        if (st != null)
        {
            st.Initialize("Subtask " + subtaskCounter);
        }

        // extra debug: log rect info if it has RectTransform
        RectTransform rt = s.GetComponent<RectTransform>();
        if (rt != null)
            Debug.Log($"Subtask rect: size={rt.rect.size}, anchoredPos={rt.anchoredPosition}");
    }

}

*/
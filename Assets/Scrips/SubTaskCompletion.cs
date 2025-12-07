using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SubTaskCompletion : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    [SerializeField] Image image;
    [SerializeField] Button button;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(TaskDataManager.Instance.AllTasks.mainTasks.Count == 0)
        {
            image.enabled = false;
            button.enabled = false;
            text.enabled = false;
        }
        else
        {
            image.enabled = true;
            button.enabled = true;
            text.enabled = true;
            text.text = TaskDataManager.Instance.AllTasks.mainTasks[0].subtasks[0].text;
        }
    }

    public void OnClick()
    {
        if(TaskDataManager.Instance.IsValidSubtaskIndex(0,1))
        {
            GlobalHandler.Instance.currentTimeLeft += 60f;
            TaskDataManager.Instance.RemoveSubtask(0,0);
        }
        else
        {
            GlobalHandler.Instance.currentTimeLeft += 60f;
            TaskDataManager.Instance.RemoveMainTask(0);
        }

    }
}

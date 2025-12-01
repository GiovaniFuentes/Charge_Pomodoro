using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TaskCompletion : MonoBehaviour
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
            text.text = TaskDataManager.Instance.AllTasks.mainTasks[0].title;
        }
    }

    public void OnClick()
    {
        GlobalHandler.Instance.currentTimeLeft += 60f;
        TaskDataManager.Instance.RemoveMainTask(0);
    }
}

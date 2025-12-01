using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TaskCompletion : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        text.text = TaskDataManager.Instance.AllTasks.mainTasks[0].title;
    }

    public void OnClick()
    {
        GlobalHandler.Instance.currentTimeLeft += 0.5f;
    }
}

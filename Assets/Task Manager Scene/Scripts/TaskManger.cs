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

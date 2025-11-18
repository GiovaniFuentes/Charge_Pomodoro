using UnityEngine;

public class TaskCompletion : MonoBehaviour
{
    [SerializeField] GlobalHandler globalHander;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {
        GlobalHandler.Instance.currentTimeLeft += 0.5f;
    }
}

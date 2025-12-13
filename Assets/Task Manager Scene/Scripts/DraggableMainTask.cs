using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableMainTask : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;
    private Transform originalParent;
    private int originalSiblingIndex;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;
        originalSiblingIndex = transform.GetSiblingIndex();
        canvasGroup.alpha = 0.8f;          // make it semi-transparent while dragging
        canvasGroup.blocksRaycasts = false; // allow drop detection
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / transform.root.GetComponent<Canvas>().scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        // Snap back to correct parent if dropped somewhere invalid
        transform.SetParent(originalParent);

        // After drop, reorder JSON
        UpdateMainTaskOrder();
    }

    private void UpdateMainTaskOrder()
    {
        var mainTasks = TaskDataManager.Instance.AllTasks.mainTasks;
        for (int i = 0; i < transform.parent.childCount; i++)
        {
            Transform child = transform.parent.GetChild(i);
            MainTask mt = child.GetComponent<MainTask>();
            if (mt != null)
            {
                mt.SetDataIndex(i); // update index in MainTask
            }
        }

        TaskDataManager.Instance.Save(); // save new order to JSON
    }
}

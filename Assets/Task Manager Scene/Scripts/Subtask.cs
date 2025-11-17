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

using UnityEngine;
using UnityEngine.SceneManagement;

public class EndCutscene : MonoBehaviour
{
    DialogueBox start;

    private void Start()
    {
        start = GetComponent<DialogueBox>();
        start.Type();
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

public class StartingCutscene : MonoBehaviour
{
    DialogueBox start;

    private void Start()
    {
        start = GetComponent<DialogueBox>();
        start.Type();
    }

    private void Update()
    {
        bool canProceed = start.iterator >= start.dialogueLines.Count - 1 && start.finished;
        if (canProceed && Input.GetKeyDown(KeyCode.E))
        {
            SceneManager.LoadScene("World");
        }
    }
}

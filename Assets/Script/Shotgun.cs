using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
public class Shotgun : Interactable
{
    [SerializeField] List<string> thoughts;
    QuestHandling questHandling;
    DialogueBox dialogue;
    private void Awake()
    {
        questHandling = FindAnyObjectByType<QuestHandling>();
        dialogue = FindAnyObjectByType<DialogueBox>();
    }

    public override void Interact()
    {
        if (questHandling.questId == 8) SceneManager.LoadScene("End");
        else if (!dialogue.isActive) dialogue.SetMessages(thoughts);
    }
}

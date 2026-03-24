using UnityEngine;
using System.Collections.Generic;

public class Axe : Interactable
{
    QuestHandling questHandling;
    DialogueBox dialogueBox;
    [SerializeField][TextArea(10,10)] List<string> thoughts;

    private void Awake()
    {
        dialogueBox = FindAnyObjectByType<DialogueBox>();
        questHandling = FindAnyObjectByType<QuestHandling>();
    }
    public override void Interact()
    {
        if (questHandling.questId == 9) gameObject.SetActive(false);
        else if (!dialogueBox.isActive) dialogueBox.SetMessages(thoughts);
    }
}

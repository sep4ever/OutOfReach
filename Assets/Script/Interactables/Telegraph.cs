using UnityEngine;
using System.Collections.Generic;
public class Telegraph : Interactable
{
    Player player;
    DialogueBox dialogueBox;
    QuestHandling questHandling;

    [SerializeField]
    [TextArea] List<string> beforeNews;
    [SerializeField]
    [TextArea] List<string> afterNews;

    private void Awake()
    {
        player = FindAnyObjectByType<Player>();
        dialogueBox = FindAnyObjectByType<DialogueBox>();
        questHandling = FindAnyObjectByType<QuestHandling>();
    }

    public override void Interact()
    {
        if (!dialogueBox.isActive)
        {
            List<string> thoughts = questHandling.questId == 10 ? afterNews : beforeNews;
            dialogueBox.SetMessages(thoughts);
        }
    }
}

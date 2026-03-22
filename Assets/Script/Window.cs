using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Unity.VisualScripting;

public class Window : Interactable
{

    [SerializeField] GameObject windowImage;
    [SerializeField] DialogueBox dialogueBox;
    [SerializeField] List<string> windowThoughts = new List<string>();
    Player player;
    QuestHandling questHandling;
    private void Start()
    {
        player = FindAnyObjectByType<Player>();
        questHandling = FindAnyObjectByType<QuestHandling>();
        //windowThoughts = new List<string>{ "I hate that view.", "Always hated it, and always will hate." };
    }

    public override void Interact()
    {
        if (true)// (questHandling.quests[questHandling.questId].isCompleted)
        {
            switch (questHandling.questId)
            {
                case 0:
                    windowThoughts = new List<string> { "Безнадёжный вид безнадёжного города!", "Честно, мне кажется, что скоро наше правительство нас самих всех перестреляет!" };
                    break;
                case 3:
                    windowThoughts = new List<string> { "Окно. Кровавый туман. Безнадёга.", "Впрочем, никаких изменений со вчерашнего дня...", "Эх... Когда я в окно смотрю чаще чем в зеркало...", "Как я вообще выгляжу?" };
                    break;
                case 6:
                    windowThoughts = new List<string> { "Ммм... Это окно.", "Погоди... Что это там... В тумане...", "Там... Человек?!" };
                    break;

                default:
                    //windowThoughts = new List<string> { "" };
                    break;
            }
        }
        if (dialogueBox.dialogueLines != windowThoughts) dialogueBox.SetMessages(windowThoughts);
        windowImage.SetActive(true);
        player.canMove = false;
    }

    public void CloseWindow()
    {
        windowImage.SetActive(false);
        dialogueBox.ClearAll();
        player.canMove = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && dialogueBox.finishedDialogue) CloseWindow();
    }
}

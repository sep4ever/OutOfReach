using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Unity.VisualScripting;

public class Window : Interactable
{

    [SerializeField] GameObject windowImage;
    [SerializeField] DialogueBox dialogueBox;
    [SerializeField][TextArea] List<string> windowThoughts = new List<string>();
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
        switch (questHandling.questId)
        {
            case 0:
                windowThoughts = new List<string> { "Сегодня мне передали отчёт о погибших...", "Я... Я всё ещё не могу поверить в то, что я увидел...", "Она... Любовь моей жизни...", "Она мертва... И я даже не знаю по какой причине...", "Я... Я не могу даже на похороны придти...", "Всё из-за этих чёртовых отчётов... Меня убьют, если я пропущу хоть 1..." };
                break;
            case 4:
                windowThoughts = new List<string> { "Окно. Кровавый туман. Безнадёга.", "Впрочем, никаких изменений со вчерашнего дня...", "Эх... Когда я в окно смотрю чаще чем в зеркало...", "Как я вообще выгляжу?" };
                break;
            case 8:
                windowThoughts = new List<string> { "Ммм... Это окно.", "Погоди... Что это там... В тумане...", "Там... Человек?!" };
                break;

            default:
                if (dialogueBox.isActive) return;
                //windowThoughts = new List<string> { "" };
                break;
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

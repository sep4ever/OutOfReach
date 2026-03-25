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
    }

    public override void Interact()
    {
        switch (questHandling.questId)
        {
            case 0:
                windowThoughts = new List<string> { "Сегодня мне передали отчёт о погибших...", "Я... Я всё ещё не могу поверить в то, что я увидел...", "Это все... Это все мирные люди, которые никому не причиняли зла...", "Они мертвы... И я даже не знаю по какой причине...", "Я... Я не могу даже как-либо им помочь", "Всё из-за этих чёртовых отчётов... Меня убьют, если я пропущу хоть 1..." };
                break;
            case 4:
                windowThoughts = new List<string> { "Окно. Кровавый туман. Безнадёга. Какой же животный страх меня накрывает...", "...при мысли что когда-то они придут и к моей двери...", "Надо бы заварить дверь...", "Даже если выбраться я потом не смогу...", "Лучше уж сдохнуть тут, чем медленно подыхать от газов..." };
                break;
            case 8:
                windowThoughts = new List<string> { "Ммм... Это окно.", "Так... Что это там... В тумане...", "Это человек?!", "Как же он изувечен, все лицо как будто каша...", "Еще и розоватая лужица возле него... Брррр..."};
                break;
            case 12:
                windowThoughts = new List<string> { "Я ненавижу свою жизнь. Я ненавижу их. Я ненавижу нас. Я ненавижу всех." };
                break;
            default:
                if (dialogueBox.isActive) return;;
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

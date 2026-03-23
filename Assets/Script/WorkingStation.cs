using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Unity.VisualScripting;

public class WorkingStation : Interactable
{
    [SerializeField] RawImage workingStationImage;
    [SerializeField] DialogueBox dialogueBox;
    [SerializeField] List<string> workingDialogues;
    QuestHandling questHandling;
    Player player;
    float duration = 2f;

    private void Awake()
    {
        questHandling = FindAnyObjectByType<QuestHandling>();
        player = FindAnyObjectByType<Player>();
    }

    Coroutine fadingCoroutine = null;
    public override void Interact()
    {
        //workingDialogues.Clear();
        switch (questHandling.questId)
        {
            case 1:
                workingDialogues = new List<string> { "Как же после этих отчётов хочется спать.." };
                break;
            case 4:
                workingDialogues = new List<string> { "Работа, работа и опять работа... Господи. Неужели я всё ещё должен заполнять документы?", "Неужто ещё не знают, что мы все умрём...", "Это всё бессмысленно." };
                break;

            case 7:
                workingDialogues = new List<string> { "Сегодня я видел человека в окне... Я должен отправить это начальству!", "Всё же, не все в городе вымерли..." };
                break;

            default:
                if (!dialogueBox.finishedDialogue && fadingCoroutine != null || dialogueBox.isActive) { return; }
                workingDialogues = new List<string> { "Не время для работы." };
                dialogueBox.SetMessages(workingDialogues);
                fadingCoroutine = null;
                return;
        }
        if (fadingCoroutine == null)
        {
            fadingCoroutine = StartCoroutine(FadeToBlack());
        }
    }

    IEnumerator FadeToBlack()
    {

        float t = 0f;
        player.canMove = false;

        while (t < duration)
        {
            t += Time.deltaTime;
            float alpha = t / duration;
            workingStationImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        yield return new WaitForSeconds(duration * 2);

        t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float alpha = 1 - t / duration;
            workingStationImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        workingStationImage.color = new Color(0, 0, 0, 0);
        player.canMove = true;

        fadingCoroutine = null;
        dialogueBox.SetMessages(workingDialogues);
    }
}

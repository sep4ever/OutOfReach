using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using TMPro;
using UnityEngine;

[System.Serializable]
public class Quest
{
    public string name;
    public string description;
    public bool isCompleted;

    public Func<bool> completionCondition;
}

public class QuestHandling : MonoBehaviour
{

    //float duration = 1f;
    RectTransform questImage;
    [SerializeField] TMP_Text questTexts;
    [SerializeField] public List<Quest> quests = new List<Quest>(); // Короче, есть лист с квестами, он является константой(просто незаданным, чтоб был в инспекторе). Каждому квесту задаётся условие В РУЧНУЮ. В методе Awake().
     
    [SerializeField] public int questId;
    public Player player;
    [SerializeField] string markUpParameter;
    bool opened = false;
    private void Awake()
    {
        questImage = GetComponent<RectTransform>();
        player = FindAnyObjectByType<Player>();

        //questId = Random.Range(0, quests.Count);
        player.quest = quests[questId];
        Func<bool> interactWithWindow = () => player.interactions.Contains(InteractionType.Window);
        Func<bool> interactWithBed = () => player.interactions.Contains(InteractionType.Bed);
        Func<bool> interactWithWorkStation = () => player.interactions.Contains(InteractionType.WorkStation);
        Func<bool> interactWithDocuments = () => player.interactions.Contains(InteractionType.Documents);
        Func<bool> interactWithShotgun = () => player.interactions.Contains(InteractionType.Shotgun);
        Func<bool> interactWithAxe = () => player.interactions.Contains(InteractionType.Axe);
        Func<bool> hasAxe = () => player.hasAxe;
        Func<bool> interactWithDoor = () => player.interactions.Contains(InteractionType.Door);
        Func<bool> interactWithTelegraph = () => player.interactions.Contains(InteractionType.Telegraph);

        quests[0].completionCondition = interactWithWindow;//player.interactedWithWindow; //player.interactedWithWindow - костыль, проверяет название объекта. Измени, если будет возможность.
        quests[1].completionCondition = interactWithWorkStation;//player.interactedWithWorkingStation;
        quests[2].completionCondition = interactWithDocuments;
        quests[3].completionCondition = interactWithBed;//player.interactedWithBed;

        quests[4].completionCondition = interactWithWindow;//player.interactedWithWindow;
        quests[5].completionCondition = interactWithWorkStation;//player.interactedWithWorkingStation;
        quests[6].completionCondition = interactWithDoor;
        quests[7].completionCondition = interactWithBed;//player.interactedWithBed;

        quests[8].completionCondition = interactWithWindow;
        quests[9].completionCondition = interactWithWorkStation;
        quests[10].completionCondition = interactWithTelegraph;
        quests[11].completionCondition = interactWithBed;

        quests[12].completionCondition = interactWithWindow;//player.interactedWithWindow;
        quests[13].completionCondition = interactWithAxe;
        quests[14].completionCondition = () => player.hasAxe && player.interactions.Contains(InteractionType.WorkStation);
        quests[15].completionCondition = interactWithDocuments;
        quests[16].completionCondition = interactWithShotgun;//player.interactedWithShotgun;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && !opened) Open();
        if (Input.GetKeyDown(KeyCode.Q) && opened) Close();

        if (quests[questId].isCompleted && !AllQuestsCompleted())
        {
            do
            {
                questId++;// = Random.Range(0, quests.Count);
            }
            while (quests[questId].isCompleted);
            player.quest = quests[questId];
        }

        questTexts.text = $"{player.quest.name} \n {player.quest.description}";

        if (!player.quest.isCompleted && player.quest.completionCondition != null)
        {
            if (player.quest.completionCondition())
            {
                player.quest.isCompleted = true;
            }
        }
    }

    bool AllQuestsCompleted()
    {
        foreach (var q in quests)
        {
            if (!q.isCompleted) return false;
        }
        return true;
    }

    void Open()
    {
        StopAllCoroutines();
        StartCoroutine(OpenCoroutine());
    }

    void Close()
    {
        StopAllCoroutines();
        StartCoroutine(CloseCoroutine());
    }   

    IEnumerator OpenCoroutine()
    {
        float targetX = 490;
        float speed = 5f; // чем больше, тем быстрее

        while (Mathf.Abs(questImage.localPosition.x - targetX) > 0.1f)
        {
            Vector3 pos = questImage.localPosition;
            pos.x = Mathf.Lerp(pos.x, targetX, Time.deltaTime * speed);
            questImage.localPosition = pos;
            yield return null;
        }

        // точно ставим в цель
        Vector3 finalPos = questImage.localPosition;
        finalPos.x = targetX;
        questImage.localPosition = finalPos;
        opened = true;
    }
    IEnumerator CloseCoroutine()
    {
        float startX = questImage.localPosition.x;
        float targetX = 790; // куда прячем UI
        float speed = 5f;

        while (Mathf.Abs(questImage.localPosition.x - targetX) > 0.1f)
        {
            Vector3 pos = questImage.localPosition;
            pos.x = Mathf.Lerp(pos.x, targetX, Time.deltaTime * speed);
            questImage.localPosition = pos;
            yield return null;
        }

        Vector3 finalPos = questImage.localPosition;
        finalPos.x = targetX;
        questImage.localPosition = finalPos;
        opened=false;
    }
}

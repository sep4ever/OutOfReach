using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[System.Serializable]
public class Quest
{
    public string name;
    public string description;
    public bool isCompleted;

    public System.Func<bool> completionCondition;
}

public class QuestHandling : MonoBehaviour
{

    //float duration = 1f;
    RectTransform questImage;
    [SerializeField] TMP_Text questTexts;
    [SerializeField] public List<Quest> quests = new List<Quest>(); // Короче, есть лист с квестами, он является константой(просто незаданным, чтоб был в инспекторе). Каждому квесту задаётся условие В РУЧНУЮ. В методе Awake().
     
    [SerializeField] public int questId;
    Player player;
    [SerializeField] string markUpParameter;
    bool opened = false;
    private void Awake()
    {
        questImage = GetComponent<RectTransform>();
        player = FindAnyObjectByType<Player>();

        //questId = Random.Range(0, quests.Count);
        player.quest = quests[questId];

        quests[0].completionCondition = () => player.interactedWithWindow; //player.interactedWithWindow - костыль, проверяет название объекта. Измени, если будет возможность.
        quests[1].completionCondition = () => player.interactedWithWorkingStation;
        quests[2].completionCondition = () => player.interactedWithBed;
        quests[3].completionCondition = () => player.interactedWithWindow;
        quests[4].completionCondition = () => player.interactedWithWorkingStation;
        quests[5].completionCondition = () => player.interactedWithBed;
        quests[6].completionCondition = () => player.interactedWithWindow;
        quests[7].completionCondition = () => player.interactedWithWorkingStation;
        quests[8].completionCondition = () => player.interactedWithShotgun;
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

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
    [SerializeField] List<Quest> quests = new List<Quest>(); // Короче, есть лист с квестами, он является константой(просто незаданным, чтоб был в инспекторе). Каждому квесту задаётся условие В РУЧНУЮ. В методе Awake().
     
    [SerializeField] int questId;
    Player player;
    [SerializeField] string markUpParameter;
    private void Awake()
    {
        questImage = GetComponent<RectTransform>();
        player = FindAnyObjectByType<Player>();

        questId = Random.Range(0, quests.Count);
        player.quest = quests[questId];

        quests[0].completionCondition = () => player.interactedWithWindow; //player.interactedWithWindow - костыль, проверяет название объекта. Измени, если будет возможность.
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q)) Open();
        if (Input.GetKeyDown(KeyCode.E)) Close();

        if (quests[questId].isCompleted)
        {
            do
            {
                questId = Random.Range(0, quests.Count);
            }
            while (quests[questId].isCompleted && !AllQuestsCompleted());
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
    }
}

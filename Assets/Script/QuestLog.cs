using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class QuestLog : MonoBehaviour
{
    [SerializeField]
    private GameObject questPrefab;

    [SerializeField]
    private Transform questParent;

    private Quest selected;

   

    [SerializeField]
    private TextMeshProUGUI questCountTxt;

    [SerializeField]
    private int maxCount;

    private int currentCount;

    [SerializeField]
    private CanvasGroup canvasGroup;

    private List<QuestScript> questScripts = new List<QuestScript>();

    private List<Quest> quests = new List<Quest>();

    [SerializeField]
    private TextMeshProUGUI questDescription;

    private static QuestLog instance;

    public static QuestLog MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindAnyObjectByType<QuestLog>();
            }

            return instance;
        }
    }

    public List<Quest> MyQuests { get => quests; set => quests = value; }

    // Start is called before the first frame update
    void Start()
    {
        questCountTxt.text = currentCount + "/" + maxCount;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateSelected()
    {
        ShowDescription(selected);
    }

    public void AcceptQuest(Quest quest)
    {
        if(currentCount < maxCount)
        {
            currentCount++;
            questCountTxt.text = currentCount + "/" + maxCount;
            foreach (CollectObjective o in quest.MyCollectObjectives)
            {

                InventoryScript.MyInstance.itemCountChangedEvent += new ItemCountChanged(o.UpdateItemCount);
                o.UpdateItemCount();
            }

            foreach (KillObjective o in quest.MyKillObjectives)
            {

                GameManager.MyInstance.killConfirmedEvent += new KillConfirmed(o.UpdateKillCount);
            }



            MyQuests.Add(quest);

            GameObject go = Instantiate(questPrefab, questParent);

            QuestScript qs = go.GetComponent<QuestScript>();
            quest.MyQuestScript = qs;
            qs.MyQuest = quest;
            questScripts.Add(qs);

            go.GetComponent<TextMeshProUGUI>().text = quest.MyTitle;
        }

       
    }

    public void ShowDescription(Quest quest)
    {
        if(quest != null)
        {
            if (selected != null && selected != quest)
            {
                selected.MyQuestScript.DeSelect();
            }

            string objectives = string.Empty;

            selected = quest;

            string title = quest.MyTitle;

            foreach (Objective obj in quest.MyCollectObjectives)
            {
                objectives += obj.MyType + ": " + obj.MyCurrentAmount + "/" + obj.MyAmount + "\n";
            }
            foreach (Objective obj in quest.MyKillObjectives)
            {
                objectives += obj.MyType + ": " + obj.MyCurrentAmount + "/" + obj.MyAmount + "\n";
            }

            questDescription.text = string.Format("{0}\n<size=25>{1}</size>\n\nObjectives:\n<size=25>{2}</size>", title, quest.MyDescription, objectives);
        }

       
    }

    public void CheckCompletion()
    {
        foreach(QuestScript qs in questScripts)
        {
            qs.MyQuest.MyQuestGiver.UpdateQuestStatus();
            qs.IsComplete();
        }
    }

    public void OpenClose()
    {
        if(canvasGroup.alpha == 1)
        {
            canvasGroup.alpha = 0;
            canvasGroup.blocksRaycasts = false;
        }
        else
        {
            canvasGroup.alpha = 1;
            canvasGroup.blocksRaycasts = true;
        }
    }

    public void Close()
    {
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
    }

    public void RemoveQuest(QuestScript qs)
    {
        questScripts.Remove(qs);
        Destroy(qs.gameObject);
        MyQuests.Remove(qs.MyQuest);
        questDescription.text = string.Empty;
        selected = null;
        currentCount--;
        questCountTxt.text = currentCount + "/" + maxCount;
        qs.MyQuest.MyQuestGiver.UpdateQuestStatus();
        qs = null;
    }

    public void AbandonQuest()
    {
        foreach (CollectObjective o in selected.MyCollectObjectives)
        {
            InventoryScript.MyInstance.itemCountChangedEvent -= new ItemCountChanged(o.UpdateItemCount);
           
        }

        foreach (KillObjective o in selected.MyKillObjectives)
        {
            GameManager.MyInstance.killConfirmedEvent -= new KillConfirmed(o.UpdateKillCount);
        }

        RemoveQuest(selected.MyQuestScript);
    }

    public bool HasQuest(Quest quest)
    {
        return MyQuests.Exists(x => x.MyTitle == quest.MyTitle);
    }
}

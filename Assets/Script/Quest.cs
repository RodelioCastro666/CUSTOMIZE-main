using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest 
{
    [SerializeField]
    private string title;

    [SerializeField]
    private string description;

    [SerializeField]
    private CollectObjective[] collectObjectives;

    [SerializeField]
    private KillObjective[] killObjectives;

    public QuestScript MyQuestScript { get; set; }

    public QuestGiver MyQuestGiver { get; set; }

    public string MyTitle { get => title; set => title = value; }

    public string MyDescription { get => description; set => description = value; }

    public CollectObjective[] MyCollectObjectives { get => collectObjectives;}

    public KillObjective[] MyKillObjectives { get => killObjectives; set => killObjectives = value; }

    [SerializeField]
    private int level;
    [SerializeField]
    private int xP;

    public bool IsComplete
    {
        get
        {
            foreach(Objective o in collectObjectives)
            {
                if (!o.Iscomplete)
                {
                    return false;
                }
            }

            foreach (Objective o in MyKillObjectives)
            {
                if (!o.Iscomplete)
                {
                    return false;
                }
            }

            return true;
        }
    }

    public int MyLevel { get => level; set => level = value; }


    public int MyXP { get => xP;  }
}

[System.Serializable]
public abstract class Objective
{
    [SerializeField]
    private int amount;

    private int currentAmount;

    [SerializeField]
    private string type;

    public int MyAmount { get => amount;}

    public int MyCurrentAmount { get => currentAmount; set => currentAmount = value; }

    public string MyType { get => type;}

    public bool Iscomplete
    {
        get
        {
            return MyCurrentAmount >= MyAmount; 
        }
    }

}

[System.Serializable]
public class CollectObjective : Objective 
{
    public void UpdateItemCount(Item item)
    {
        if(MyType.ToLower() == item.MyTitle.ToLower())
        {
            MyCurrentAmount = InventoryScript.MyInstance.GetItemCount(item.MyTitle);

            if(MyCurrentAmount <= MyAmount)
            {
                MessageFeedManager.MyInstance.WriteMessage(string.Format("{0}: {1}/{2}", item.MyTitle, MyCurrentAmount, MyAmount));
            }

           
            QuestLog.MyInstance.UpdateSelected();
            QuestLog.MyInstance.CheckCompletion();
        }
    }


    public void UpdateItemCount()
    {

        MyCurrentAmount = InventoryScript.MyInstance.GetItemCount(MyType);
        QuestLog.MyInstance.UpdateSelected();
        QuestLog.MyInstance.CheckCompletion();

    }

    public void Complete()
    {
        Stack<Item> items = InventoryScript.MyInstance.GetItems(MyType, MyAmount);

        foreach(Item item in items)
        {
            item.Remove();
        }
    }
}

[System.Serializable]
public class KillObjective : Objective
{
    public void UpdateKillCount(Character character)
    {
        if(MyType == character.MyType)
        {
            if(MyCurrentAmount < MyAmount)
            {
                MyCurrentAmount++;

                MessageFeedManager.MyInstance.WriteMessage(string.Format("{0}: {1}/{2}", character.MyType, MyCurrentAmount, MyAmount));
                QuestLog.MyInstance.CheckCompletion();
                QuestLog.MyInstance.UpdateSelected();
               

            }


        }
    }


}





using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;



public abstract class Item : ScriptableObject, IMoveable,IDescribable
{
    private static Item instance;

    public static Item MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindAnyObjectByType<Item>();
            }

            return instance;
        }
    }

    [SerializeField]
    private Sprite icon;

    [SerializeField]
    private int stackSize;

    [SerializeField]
    private string title;

    [SerializeField]
    private Quality quality;

    [SerializeField]
    private int price;

    private SlotScript slot;

    public Sprite MyIcon { get => icon;}

    public int MyStackSize { get => stackSize;}

    public SlotScript MySlot { get => slot; set => slot = value; }

    public Quality MyQuality { get => quality;}

    public string MyTitle { get => title;}

    public CharButton MyCharButton { get; set; }


    public int MyPrice { get => price; }

    public void Remove()
    {
        if(MySlot != null)
        {
            MySlot.RemoveItem(this);
           // MySlot = null;
        }
    }

    public virtual string GetDescription()
    {

        return string.Format("<color={0}> {1}</color>", QualityColor.MyColors[MyQuality],MyTitle);
    }
}

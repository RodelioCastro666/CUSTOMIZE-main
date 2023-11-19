using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class SlotScript : MonoBehaviour, IPointerClickHandler, IClickable, IDragHandler, IDropHandler,IEndDragHandler
{
    private static SlotScript instance;

    public static SlotScript MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindAnyObjectByType<SlotScript>();
            }

            return instance;
        }
    }

    [SerializeField]
    private Image icon;

    [SerializeField]
    private TextMeshProUGUI stackSize;

    private ObservableStack<Item> items = new ObservableStack<Item>();

    public BagScript MyBag { get; set; }

    public int MyIndex { get; set; }

    public bool IsEmpty
    {
        get
        {
            return MyItems.Count == 0;
        }
    }

    private void Awake()
    {
        MyItems.OnPop += new UpdateStackEvent(UpdateSlot);
        MyItems.OnPush += new UpdateStackEvent(UpdateSlot);
        MyItems.OnClear += new UpdateStackEvent(UpdateSlot);
    }

    public bool IsFull 
    {
        get
        {
            if(IsEmpty || MyCount < MyItem.MyStackSize)
            {
                return false;
            }

            return true; 
        }
    }

    public Item MyItem
    {
        get
        {
            if (!IsEmpty)
            {
                return MyItems.Peek();
            }

            return null;
        }
    }

    public Image MyIcon
    {
        get => icon;

        set
        {
            icon = value;

        }
    }

    public int MyCount
    {
        get { return MyItems.Count;}
    }

    public TextMeshProUGUI MyStackText => stackSize;

    public ObservableStack<Item> MyItems { get => items; }

    public bool AddItem(Item item)
    {
        MyItems.Push(item);
        icon.sprite = item.MyIcon;
        icon.color = Color.white;
        item.MySlot = this;
        return true;
    }

    public bool AddItems(ObservableStack<Item> newItems)
    {
        if(IsEmpty || newItems.Peek().GetType() == MyItem.GetType())
        {
            int count = newItems.Count;

            for(int i = 0; i < count; i++)
            {
                if (IsFull)
                {
                    return false;
                }

                AddItem(newItems.Pop()); 
            }
            return true;
        }

        return false;

        
    }
    


    public void RemoveItem(Item item)
    {
        if (!IsEmpty)
        {
            
            InventoryScript.MyInstance.OnItemCountChanged(MyItems.Pop());
           
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {

        if (eventData.button == PointerEventData.InputButton.Left && HandScript.MyInstance.MyMoveable == null)
        {
            
            UseItem();
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!IsEmpty)
        {
            UiManager.MyInstance.ShowToolTip(MyItem);
        }

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (InventoryScript.MyInstance.FromSlot == null)
            {
                HandScript.MyInstance.TakeMoveable(MyItem as IMoveable);
                InventoryScript.MyInstance.FromSlot = this;
                Debug.Log("Drag");
            }
           



        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        UiManager.MyInstance.HideToolTip();

        if (InventoryScript.MyInstance.FromSlot != null)
        {
            if (PutItemBack() || MergeItems(InventoryScript.MyInstance.FromSlot) || SwapItems(InventoryScript.MyInstance.FromSlot) || AddItems(InventoryScript.MyInstance.FromSlot.MyItems))
            {
                HandScript.MyInstance.Drop();
                InventoryScript.MyInstance.FromSlot = null;
                Debug.Log("Drop");
                
            }

            
        }
        if (InventoryScript.MyInstance.FromSlot == null && IsEmpty)
        {
            if(HandScript.MyInstance.MyMoveable is Armor)
            {
                Armor armor = (Armor)HandScript.MyInstance.MyMoveable;
                
                CharacterPanel.MyInstance.MySelectedButton.DequipArmor();
                AddItem(armor);
                HandScript.MyInstance.Drop();

            }
        }

        if(InventoryScript.MyInstance.FromSlot == null && !IsEmpty)
        {
            if(HandScript.MyInstance.MyMoveable != null)
            {
                if(HandScript.MyInstance.MyMoveable is Armor)
                {
                    if(MyItem is Armor && (MyItem as Armor).MyArmorType == (HandScript.MyInstance.MyMoveable as Armor).MyArmorType)
                    {
                        (MyItem as Armor).Equip();
                       // UiManager.MyInstance.RefreshToolTip();
                        HandScript.MyInstance.Drop();
                    }
                }
            }
        }
    }
    public void OnEndDrag(PointerEventData eventData)
    {
      UiManager.MyInstance.HideToolTip();
        
        if(!EventSystem.current.IsPointerOverGameObject() && HandScript.MyInstance.MyMoveable != null)
        {
            if (HandScript.MyInstance.MyMoveable is Item && InventoryScript.MyInstance.FromSlot != null)
            {
                (HandScript.MyInstance.MyMoveable as Item).MySlot.Clear();

            }


            Debug.Log("Endrag");

            HandScript.MyInstance.Drop();
            InventoryScript.MyInstance.FromSlot = null;
        }


        PutItemBack();
        HandScript.MyInstance.Drop();

    }

    public void UseItem()
    {
        if(MyItem is IUseable)
        {
            (MyItem as IUseable).Use();
           
        }
        else if (MyItem is Armor)
        {
            (MyItem as Armor).Equip();
        }
        
    }

    private bool MergeItems(SlotScript from)
    {
        if (IsEmpty)
        {
            return false;
        }
        if(from.MyItem.GetType() == MyItem.GetType() && !IsFull)
        {
            int free = MyItem.MyStackSize - MyCount;

            for(int i  = 0; i < free; i++)
            {
                AddItem(from.MyItems.Pop());
            }

            return true;
        }

        return false;
    }

    public bool StackItem(Item item)
    {
        if(!IsEmpty && item.name == MyItem.name && MyItems.Count < MyItem.MyStackSize)
        {
            MyItems.Push(item);
            item.MySlot = this;
            return true;
        }

        return false;
    }

    public bool PutItemBack()
    {
        if(InventoryScript.MyInstance.FromSlot == this)
        {
            InventoryScript.MyInstance.FromSlot.MyIcon.color = Color.white;
            return true;
        }

        return false;
    }

    public void Clear()
    {
        int initCount = MyItems.Count;

        if (initCount > 0)
        {
            for (int i = 0; i < initCount; i++)
            {
                InventoryScript.MyInstance.OnItemCountChanged(MyItems.Pop());

            }
           
            
        }
    }
    private bool SwapItems(SlotScript from)
    {
        if (IsEmpty)
        {
            return false;
        }
        if(from.MyItem.GetType() != MyItem.GetType() || from.MyCount+MyCount > MyItem.MyStackSize)
        {
            ObservableStack<Item> tmpFrom = new ObservableStack<Item>(from.MyItems);

            from.MyItems.Clear();
            from.AddItems(MyItems);
            MyItems.Clear();
            AddItems(tmpFrom);
            return true;
        }

        return false;
    }

    private void UpdateSlot()
    {
        UiManager.MyInstance.UpdateStackSize(this);
    }

    

    

    
}

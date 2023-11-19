
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class ActionButton : MonoBehaviour, IPointerClickHandler, IClickable, IDropHandler
{
    
    public IUseable MyUseable { get; set; }

    public Button MyButton { get; private set; }

    private Stack<IUseable> useables = new Stack<IUseable>();

    private int count;

    [SerializeField]
    private TextMeshProUGUI stackSize;
    
    [SerializeField]
    private Image icon;

    public Image MyIcon { get => icon; set => icon = value; }

    public int MyCount => count;

    public TextMeshProUGUI MyStackText 
    {
        get { return stackSize; }
    }

    public Stack<IUseable> MyUseables 
    { get => useables;

        set
        {
            if(value.Count > 0)
            {
                MyUseable = value.Peek();
            }
            else
            {
                MyUseable = null;
            }

            useables = value;

        }
    }




    // Start is called before the first frame update
    void Start()
    {
        MyButton = GetComponent<Button>();
        MyButton.onClick.AddListener(OnClick);
        InventoryScript.MyInstance.itemCountChangedEvent += new ItemCountChanged(UpdateItemCount);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {
        if(HandScript.MyInstance.MyMoveable == null)
        {
            if (MyUseable != null)
            {
                MyUseable.Use();
            }
            else if(MyUseables != null && MyUseables.Count > 0)
            {
                MyUseables.Peek().Use();
            }
        }

       
    }

    public void OnPointerClick(PointerEventData eventData)
    {
       
    }

    public void SetUseable(IUseable useable)
    {
        if(useable is Item)
        {
            MyUseables = InventoryScript.MyInstance.GetUseables(useable);
           if(InventoryScript.MyInstance.FromSlot != null)
            {
                InventoryScript.MyInstance.FromSlot.MyIcon.color = Color.white;
                InventoryScript.MyInstance.FromSlot = null;
            }
           
        }
        else
        {
            //useables.Clear();
            this.MyUseable = useable;
        }


        count = MyUseables.Count;
        UpdateVisual(useable as IMoveable);
        UiManager.MyInstance.RefreshToolTip(MyUseable as IDescribable);
    }

    public void UpdateVisual(IMoveable moveable)
    {
        if (HandScript.MyInstance.MyMoveable != null)
        {
            HandScript.MyInstance.Drop();
        }

        MyIcon.sprite = moveable.MyIcon;
     
        MyIcon.color = Color.white;

        if(count > 1)
        {
            UiManager.MyInstance.UpdateStackSize(this);
        }
        else if(MyUseable is Spell)
        {
            UiManager.MyInstance.ClearStackCount(this);
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (HandScript.MyInstance.MyMoveable != null && HandScript.MyInstance.MyMoveable is IUseable)
            {
                SetUseable(HandScript.MyInstance.MyMoveable as IUseable);
            }
        }
    }

    public void UpdateItemCount(Item item)
    {
        if(item is IUseable && MyUseables.Count > 0)
        {
            if(MyUseables.Peek().GetType() == item.GetType())
            {
                MyUseables = InventoryScript.MyInstance.GetUseables(item as IUseable);

                count = MyUseables.Count;

                UiManager.MyInstance.UpdateStackSize(this);
            }
        }
    }

   

   
}

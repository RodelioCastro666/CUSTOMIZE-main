using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private Sprite openSprite, closedSprite;

    private bool isOpen;

    private List<Item> items;

    [SerializeField]
    private BagScript bag;

    [SerializeField]
    private CanvasGroup canvasGroup;

    public List<Item> MyItems { get => items; set => items = value; }


    public BagScript MyBag { get => bag; set => bag = value; }

    public void Awake()
    {
        items = new List<Item>();
        //InventoryScript.MyInstance.AddItem((Armor)Instantiate(items[1]));
    }

    public void Interact()
    {
        if (isOpen)
        {
            StopInteract();
        }
        else
        {
            AddItems();
            isOpen = true;
            spriteRenderer.sprite = openSprite;
            canvasGroup.alpha = 1;
            canvasGroup.blocksRaycasts = true;
        }
    }

    public void StopInteract()
    {
        if (isOpen)
        {
            StoreItems();
            MyBag.Clear();
            isOpen = false;
            spriteRenderer.sprite = closedSprite;
            canvasGroup.blocksRaycasts = false;
            canvasGroup.alpha = 0;
        }

    }

    public void AddItems()
    {
        if(MyItems != null)
        {
            foreach(Item item in MyItems)
            {
                item.MySlot.AddItem(item);
            }
        }
    }

    public void StoreItems()
    {
        MyItems = MyBag.GetItems();
    }
}

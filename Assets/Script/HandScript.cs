using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HandScript : MonoBehaviour
{
    //[SerializeField]
    private Vector3 offset;

    private static HandScript instance;

    public static HandScript MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindAnyObjectByType<HandScript>();
            }

            return instance;
        }
    }

    public IMoveable MyMoveable { get; set; }

    private Image icon;

    private void Start()
    {
        icon = GetComponent<Image>();
    }

    private void Update()
    {
        icon.transform.position = Input.mousePosition;

      // DeleteItem();
        
    }

    public void TakeMoveable(IMoveable moveable)
    {
        
        
            this.MyMoveable = moveable;
            icon.sprite = moveable.MyIcon;
            icon.color = Color.white;

    }

    public IMoveable Put()
    {
        IMoveable tmp = MyMoveable;

        MyMoveable = null;

        icon.color = new Color(0, 0, 0, 0);

        return tmp;
    }
    public void Drop()
    {
        MyMoveable = null;
        icon.color = new Color(0, 0, 0, 0);
        InventoryScript.MyInstance.FromSlot = null;

    }

    public void DeleteItem()
    {
        
            
            if (MyMoveable is Item && InventoryScript.MyInstance.FromSlot != null)
            {
                (MyMoveable as Item).MySlot.Clear();
               
            }

            Drop();

            InventoryScript.MyInstance.FromSlot = null;
        
    }

    
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharButton : MonoBehaviour,  IDropHandler,IPointerDownHandler,IPointerUpHandler,IDragHandler, IEndDragHandler
{
    [SerializeField]
    private ArmorType armorType;

    private Armor equippedArmor;

    [SerializeField]
    private Image icon;

    public Armor MyEquippedArmor { get => equippedArmor; }

    public void EquipArmor(Armor armor)
    {
        armor.Remove();

        if(MyEquippedArmor != null)
        {
            if(MyEquippedArmor != armor)
            {
                armor.MySlot.AddItem(MyEquippedArmor);
            }

            UiManager.MyInstance.RefreshToolTip(MyEquippedArmor);
        }

        icon.enabled = true;
        icon.sprite = armor.MyIcon;
        icon.color = Color.white;
        this.equippedArmor = armor;
        this.MyEquippedArmor.MyCharButton = this;


        if(HandScript.MyInstance.MyMoveable == (armor as IMoveable))
        {
            HandScript.MyInstance.Drop();
        }

      
    }

    public void OnDrag(PointerEventData eventData)
    {
       if(eventData.button == PointerEventData.InputButton.Left)
        {
            if(HandScript.MyInstance.MyMoveable == null && MyEquippedArmor != null)
            {
                HandScript.MyInstance.TakeMoveable(MyEquippedArmor);
                CharacterPanel.MyInstance.MySelectedButton = this;
                icon.color = Color.gray;
            }
          
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (HandScript.MyInstance.MyMoveable is Armor)
            {
                Armor tmp = (Armor)HandScript.MyInstance.MyMoveable;

                if (tmp.MyArmorType == armorType)
                {
                    EquipArmor(tmp);
                }
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(MyEquippedArmor != null)
        {
            UiManager.MyInstance.ShowToolTip(MyEquippedArmor);
        }
       
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        UiManager.MyInstance.HideToolTip();
    }

    public void DequipArmor()
    {
        icon.color = Color.white;
        icon.enabled = false;

        equippedArmor.MyCharButton = null;
        equippedArmor = null;
    }

    private bool PutItemBack()
    {
        if (CharacterPanel.MyInstance.MySelectedButton == this)
        {
            CharacterPanel.MyInstance.MySelectedButton.icon.color = Color.white;
            return true;
        }
        return false;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!EventSystem.current.IsPointerOverGameObject() && HandScript.MyInstance.MyMoveable != null)
        {
            if (HandScript.MyInstance.MyMoveable is Item)
            {
                Item item = (Item)HandScript.MyInstance.MyMoveable;
                if (item.MyCharButton != null)
                {
                    item.MySlot.Clear();
                    item.MyCharButton.DequipArmor();
                }


            }

            HandScript.MyInstance.Drop();

            InventoryScript.MyInstance.FromSlot = null;
        }

            

        
        PutItemBack();
        HandScript.MyInstance.Drop();
        //Debug.Log("tytyt");
    }
}

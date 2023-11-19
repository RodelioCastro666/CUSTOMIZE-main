using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPanel : MonoBehaviour
{
    private static CharacterPanel instance;

    public static CharacterPanel MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindAnyObjectByType<CharacterPanel>();
            }

            return instance;
        }
    }

    [SerializeField]
    private CanvasGroup canvasGroup;

    [SerializeField]
    private CharButton helmet, shoulders, chest, gloves, boots, orb, sword, staff;

    public CharButton MySelectedButton { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenClose()
    {
        if(canvasGroup.alpha <= 0)
        {
            canvasGroup.blocksRaycasts = true;
            canvasGroup.alpha = 1;
        }
        else
        {
            canvasGroup.blocksRaycasts = false;
            canvasGroup.alpha = 0;
        }
    }

    public void EquipArmor(Armor armor)
    {
        switch (armor.MyArmorType)
        {
            case ArmorType.Helmet:
                helmet.EquipArmor(armor);
                break;
            case ArmorType.Shoulders:
                shoulders.EquipArmor(armor);
                break;
            case ArmorType.Chest:
                chest.EquipArmor(armor);
                break;
            case ArmorType.Boots:
                boots.EquipArmor(armor);
                break;
            case ArmorType.Orb:
                orb.EquipArmor(armor);
                break;
            case ArmorType.Gloves:
                gloves.EquipArmor(armor);
                break;
            case ArmorType.Sword:
                sword.EquipArmor(armor);
                break;
            case ArmorType.Wand:
                staff.EquipArmor(armor);
                break;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class LootButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler,IPointerClickHandler
{
    [SerializeField]
    private Image icon;

    [SerializeField]
    private TextMeshProUGUI title;


    public Image MyIcon { get => icon;  }

    public TextMeshProUGUI MyTitle { get => title; }

    public Item MyLoot { get; set; }

    private LootWindow lootWindow;

    private void Awake()
    {
        lootWindow = GetComponentInParent<LootWindow>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (InventoryScript.MyInstance.AddItem(MyLoot))
        {
            gameObject.SetActive(false);
            lootWindow.TakeLoot(MyLoot);
            UiManager.MyInstance.HideToolTip();
        }
       
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        UiManager.MyInstance.ShowToolTip(MyLoot);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        UiManager.MyInstance.HideToolTip();
    }
}

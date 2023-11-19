using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class UiManager : MonoBehaviour
{
    private static UiManager instance;

    public static UiManager MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindAnyObjectByType<UiManager>();
            }

            return instance;
        }
    }

    [SerializeField]
    private CanvasGroup keyBindMenu;

    [SerializeField]
    private ActionButton[] actionButton;

    //[SerializeField]
    //private CanvasGroup spellBook;

    private TextMeshProUGUI toolTipText;

    private GameObject[] keybindButtons;

    [SerializeField]
    private GameObject toolTip;

    [SerializeField]
    private CharacterPanel charPanel;

    [SerializeField]
    private CanvasGroup[] menus;

    [SerializeField]
    private InventoryScript InventoryScript;

    private readonly int enemyLvl;

    private void Awake()
    {
        keybindButtons = GameObject.FindGameObjectsWithTag("Keybinds");
        toolTipText = toolTip.GetComponentInChildren<TextMeshProUGUI>();
    }

    // Start is called before the first frame update
    void Start()
    {


       
       
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OpenClose(menus[0]);
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            OpenClose(menus[1]);
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            InventoryScript.MyInstance.OpenClose();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            OpenClose(menus[2]);
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            OpenClose(menus[3]);
        }


        //if (Input.GetKeyDown(KeyCode.P))
        //{
        //    OpenClose(menus[6]);
        //}

       
       


        //if (Input.GetKeyDown(KeyCode.Escape))
        //{
        //    OpenClose(keyBindMenu);
        //}
        ////if (Input.GetKeyDown(KeyCode.P))
        ////{
        ////    OpenClose(spellBook);
        ////}
        //if (Input.GetKeyDown(KeyCode.B))
        //{
        //    InventoryScript.OpenClose();
        //}
        //if (Input.GetKeyDown(KeyCode.C))
        //{
        //    charPanel.OpenClose();
        //}
    }



    //public void OpenCloseMenu()
    //{
    //    keyBindMenu.alpha = keyBindMenu.alpha > 0 ? 0 : 1;
    //    keyBindMenu.blocksRaycasts = keyBindMenu.blocksRaycasts == true ? false : true;
    //    Time.timeScale = Time.timeScale > 0 ? 0 : 1;
    //}

    public void OpenMainMenu()
    {
        OpenClose(menus[0]);
    }

    public void CharOpen()
    {
        OpenClose(menus[2]);
    }

    public void OpenSingle(CanvasGroup canvasGroup)
    {
        foreach (CanvasGroup canvas in menus)
        {
            CloseSingle(canvas);
        }

        canvasGroup.alpha = canvasGroup.alpha > 0 ? 0 : 1;
        canvasGroup.blocksRaycasts = canvasGroup.blocksRaycasts == true ? false : true;
    }

    public void CloseSingle(CanvasGroup canvasGroup)
    {
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
    }

    public void UpdateKeyText(string key, KeyCode code)
    {
        Text tmp = Array.Find(keybindButtons, x => x.name == key).GetComponentInChildren<Text>();
        tmp.text = code.ToString();
    }

    public void CLickActionButton(string buttonName)
    {
        Array.Find(actionButton, x => x.gameObject.name == buttonName).MyButton.onClick.Invoke();
    }

    

    public void OpenClose(CanvasGroup canvasGroup)
    {
        canvasGroup.alpha = canvasGroup.alpha > 0 ? 0 : 1;
        canvasGroup.blocksRaycasts = canvasGroup.blocksRaycasts == true ? false : true;
    }

    public void UpdateStackSize(IClickable clickable)
    {
        if(clickable.MyCount > 1)
        {
            clickable.MyStackText.text = clickable.MyCount.ToString();
            clickable.MyStackText.color = Color.white;
            clickable.MyIcon.color = Color.white;
        }
        else
        {
            clickable.MyStackText.color = new Color(0, 0, 0, 0);
            clickable.MyIcon.color = Color.white;
        }
        if(clickable.MyCount == 0)
        {
            clickable.MyIcon.color = new Color(0, 0, 0, 0);
            clickable.MyStackText.color = new Color(0, 0, 0, 0);
        }
    }

    public void ShowToolTip(IDescribable description)
    {
        toolTip.SetActive(true);
        toolTipText.text = description.GetDescription();
    }

    public void HideToolTip()
    {
        toolTip.SetActive(false);
    }

    public void ClearStackCount(IClickable clickable)
    {
        clickable.MyStackText.color = new Color(0, 0, 0, 0);
        clickable.MyIcon.color = Color.white;
    }

    public void RefreshToolTip(IDescribable description)
    {
        toolTipText.text = description.GetDescription();
    }

   
}

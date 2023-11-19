using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LootWindow : MonoBehaviour
{
    private static LootWindow instance;

    public static LootWindow MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindAnyObjectByType<LootWindow>();
            }

            return instance;
        }
    }

    [SerializeField]
    private LootButton[] lootButtons;

    [SerializeField]
    private Item[] items;

    private int pageIndex = 0;

    private List<Drop> droppedLoot = new List<Drop>();

    [SerializeField]
    private TextMeshProUGUI pageNumber;

    public IInteractable MyInteractable { get; set; }

    private CanvasGroup canvasGroup;

    [SerializeField]
    private GameObject nextBtn, previousBtn;

    private List<List<Drop>> pages = new List<List<Drop>>();

    public bool IsOpen
    {
        get { return canvasGroup.alpha > 0; }
    }

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    // Start is called before the first frame update
   

    public void CreatePages(List<Drop> items) 
    {
        if (!IsOpen)
        {
            List<Drop> page = new List<Drop>();

            droppedLoot = items;

            for (int i = 0; i < items.Count; i++)
            {
                page.Add(items[i]);

                if (page.Count == 5 || i == items.Count - 1)
                {
                    pages.Add(page);
                    page = new List<Drop>();
                }
            }

            AddLoot();

            Open();
        }

       
    }

    // Update is called once per frame
     private void AddLoot()
    {
        if (pages.Count > 0)
        {
            pageNumber.text = pageIndex + 1 + "/" + pages.Count;

            previousBtn.SetActive(pageIndex > 0);
            nextBtn.SetActive(pages.Count > 1 && pageIndex < pages.Count - 1);


            for (int i = 0; i < pages[pageIndex].Count; i++)
            {
                if (pages[pageIndex][i] != null)
                {
                    lootButtons[i].MyIcon.sprite = pages[pageIndex][i].MyItem.MyIcon;

                    lootButtons[i].MyLoot = pages[pageIndex][i].MyItem;

                    lootButtons[i].gameObject.SetActive(true);

                    string title = string.Format("<color={0}>{1}</color>", QualityColor.MyColors[pages[pageIndex][i].MyItem.MyQuality], pages[pageIndex][i].MyItem.MyTitle);

                    lootButtons[i].MyTitle.text = title;
                }
               
            }
        }

       

       
    }

    public void ClearButtons()
    {
        foreach(LootButton btn in lootButtons)
        {
            btn.gameObject.SetActive(false);
        }
    }
    public void NextPage()
    {
        if(pageIndex < pages.Count)
        {
            pageIndex++;
            ClearButtons();
            AddLoot();
        }
    }
    public void PreviousPage()
    {
        if(pageIndex > 0)
        {
            pageIndex--;
            ClearButtons();
            AddLoot();
        }
    }

    public void TakeLoot(Item loot)
    {
        Drop drop = pages[pageIndex].Find(x => x.MyItem == loot);

        pages[pageIndex].Remove(drop);

        drop.Remove();

        if(pages[pageIndex].Count == 0) 
        {
            pages.Remove(pages[pageIndex]);

            if(pageIndex == pages.Count && pageIndex > 0)
            {
                pageIndex--;
            }

            AddLoot();
        }
    }

    public void Close()
    {
        pageIndex = 0;
        pages.Clear();
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
        ClearButtons();

        if(MyInteractable != null)
        {
            MyInteractable.StopInteract();
        }

        MyInteractable = null;  
    }

    public void Open()
    {
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;
    }
}

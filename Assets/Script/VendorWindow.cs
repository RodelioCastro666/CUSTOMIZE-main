using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VendorWindow : Window
{
    private static VendorWindow instance;

    public static VendorWindow MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindAnyObjectByType<VendorWindow>();
            }

            return instance;
        }
    }

    [SerializeField]
    private VendorButton[] vendorButtons;

    [SerializeField]
    private TextMeshProUGUI pageNumber;

    private Vendor vendor;

    private List<List<VendorItem>> pages = new List<List<VendorItem>>();

    private int pageIndex;

    public void CreatePages(VendorItem[] items)
    {
        pages.Clear();

        List<VendorItem> page = new List<VendorItem>();

        for(int i = 0; i < items.Length; i++)
        {
            page.Add(items[i]);

            if(page.Count == 10 || i == items.Length - 1)
            {
                pages.Add(page);
                page = new List<VendorItem>();
            }
        }

        AddItems();
    }

    public void AddItems()
    {
        pageNumber.text = pageIndex + 1 + "/" + pages.Count;

        if(pages.Count > 0)
        {
            for (int i = 0; i < pages[pageIndex].Count; i++)
            {
                if (pages[pageIndex][i] != null){
                    vendorButtons[i].AddItem(pages[pageIndex][i]);
                }
            }
        }
    }

    

    public void NextPage()
    {
        if(pageIndex < pages.Count - 1)
        {
            ClearButtons();
            pageIndex++;
            AddItems();
        }
    }

    public void PreviousPage()
    {
        if(pageIndex > 0)
        {
            ClearButtons();
            pageIndex--;
            AddItems();
        }
    }

    public void ClearButtons()
    {
        foreach(VendorButton btn in vendorButtons)
        {
            btn.gameObject.SetActive(false);
        }
    }

    public override void Open(NPC npc)
    {
        CreatePages((npc as Vendor).MyItems);
        base.Open(npc);
    }
}

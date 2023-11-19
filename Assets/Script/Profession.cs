using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Profession : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI title;

    [SerializeField]
    private TextMeshProUGUI description;

    [SerializeField]
    private GameObject materialPRefab;

    [SerializeField]
    private Transform parent;

    private List<GameObject> materials = new List<GameObject>();

    [SerializeField]
    private Recipe selectedRecipe;

    [SerializeField]
    private Text countTxt;

    [SerializeField]
    private ItemInfo craftItemInfo;

    private int maxAmount;

    private int amount;

    private void Start()
    {
        InventoryScript.MyInstance.itemCountChangedEvent += new ItemCountChanged(UpdateMaterialCount);
    }

    public void ShowDescription(Recipe recipe)
    {
        if(selectedRecipe != null)
        {
            selectedRecipe.DeSelect();
        }

        this.selectedRecipe = recipe;

        this.selectedRecipe.Select();

        foreach(GameObject gameObject in materials)
        {
            Destroy(gameObject);
        }

        materials.Clear();

        title.text = recipe.Output.MyTitle;

        description.text = recipe.MyDescription + " " + recipe.Output.MyTitle;

        craftItemInfo.Initialize(recipe.Output, 1);

        foreach (CraftingMaterial material in recipe.Materials)
        {
            GameObject tmp = Instantiate(materialPRefab, parent);
            tmp.GetComponent<ItemInfo>().Initialize(material.MyItem, material.MyCount);
            materials.Add(tmp);
        }
        UpdateMaterialCount(null);
    }


    private void UpdateMaterialCount(Item item)
    {
        foreach(GameObject material in materials)
        {
            ItemInfo tmp = material.GetComponent<ItemInfo>();
            tmp.UpdateStackCount();
        }
    }

    public void Craft()
    {

    }

    private IEnumerator  CraftRoutine(int count)
    {
        yield return Player.MyInstance.MyInitRoutine = StartCoroutine(Player.MyInstance.CraftRoutine(selectedRecipe));
    }

    public void AddItemsToInventory()
    {
        InventoryScript.MyInstance.AddItem(craftItemInfo.MyItem);
    }
}

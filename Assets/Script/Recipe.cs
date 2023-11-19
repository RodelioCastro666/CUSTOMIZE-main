using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Recipe : MonoBehaviour
{
    [SerializeField]
    private CraftingMaterial[] materials;

    [SerializeField]
    private Item output;

    [SerializeField]
    private int outputCount;

    [SerializeField]
    private string description;

    [SerializeField]
    private Image highlight;

    public Item Output { get => output; }

    public int OutputCount { get => outputCount; set => outputCount = value; }

    public string MyDescription { get => description; }

    public CraftingMaterial[] Materials { get => materials; }


    private void Start()
    {
        GetComponent<TextMeshProUGUI>().text = output.MyTitle;
    }


    public void Select()
    {
        Color c = highlight.color;
        c.a = .3f;
        highlight.color = c;
    }

    public void DeSelect()
    {
        Color c = highlight.color;
        c.a = .0f;
        highlight.color = c;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private Transform map;

    [SerializeField]
    private Texture2D[] mapData;

    [SerializeField]
    private MapElement[] mapElements;

    [SerializeField]
    private Sprite defaultSprite;

    

    private Vector3 WorldStartPos
    {
        get
        {
            return Camera.main.ScreenToWorldPoint(new Vector3(0, 0));
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        GenerateMap();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GenerateMap()
    {
        Debug.Log("jkjk");

        for(int i =0; i < mapData.Length; i++)
        {
            for(int x = 0; x <mapData[i].width; x++)
            {
                for(int y = 0; y <mapData[i].height; y++)
                {
                    Color c = mapData[i].GetPixel(x, y);

                    MapElement newElement = Array.Find(mapElements, e => e.MyColor == c);

                    if(newElement != null)
                    {
                        float xPos = WorldStartPos.x + (defaultSprite.bounds.size.x * x);
                        float yPos = WorldStartPos.y + (defaultSprite.bounds.size.y * y);

                        GameObject go = Instantiate(newElement.MyElementPrefab);
                        go.transform.position = new Vector2(xPos, yPos);
                        go.transform.parent = map;
                    }
                }
            }
        }
    }
}
[Serializable]
public class MapElement
{
    [SerializeField]
    private string tileTag;

    [SerializeField]
    private Color color;

    [SerializeField]
    private GameObject elementPrefab;

    public Color MyColor { get => color; }

    public string TileTag { get => tileTag;  }

    public GameObject MyElementPrefab { get => elementPrefab;  }
}


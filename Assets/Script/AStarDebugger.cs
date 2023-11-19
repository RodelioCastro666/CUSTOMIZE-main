using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AStarDebugger : MonoBehaviour
{
    private static AStarDebugger instance;

    public static AStarDebugger MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<AStarDebugger>();
            }

            return instance;
        }
    }

    [SerializeField]
    private Grid grid;

    [SerializeField]
    private Tilemap tileMap;

    [SerializeField]
    private Tile tile;

    [SerializeField]
    private Canvas canvas;

    [SerializeField]
    private Color openColor, closedColor, pathColor, currentColor, startColor, goalColor;

    [SerializeField]
    private GameObject debugTextPrefab;

    private List<GameObject> debugObjects = new List<GameObject>();

    public void CreateTiles(HashSet<Node> openList,HashSet<Node> closeList, Dictionary<Vector3Int,Node> allNodes,Vector3Int start, Vector3Int goal,Stack<Vector3Int> path = null)
    {
        foreach(Node node in openList)
        {
            ColorTile(node.Position, openColor);
        }

        foreach(Node node in closeList)
        {
            ColorTile(node.Position, closedColor);
        }

        

        if (path != null)
        {
            foreach (Vector3Int pos in path)
            {
                if (pos != start && pos != goal)
                {
                    ColorTile(pos, pathColor);
                }
            }
        }

        ColorTile(start, startColor);
        ColorTile(goal, goalColor);

        foreach (KeyValuePair<Vector3Int, Node> node in allNodes)
        {
            if (node.Value.Parent != null)
            {
                GameObject go = Instantiate(debugTextPrefab, canvas.transform);
                go.transform.position = grid.CellToWorld(node.Key);
                debugObjects.Add(go);
                GenerateDebugText(node.Value, go.GetComponent<DebugTxt>());

            }

        }
    }

    private void GenerateDebugText(Node node, DebugTxt debugText)
    {

        debugText.P.text = $"P:{node.Position.x}, {node.Position.y}";
        debugText.F.text = $"F:{node.F}";
        debugText.G.text = $"G:{node.G}";
        debugText.H.text = $"H:{node.H}";

        if (node.Parent.Position.x < node.Position.x && node.Parent.Position.y == node.Position.y)
        {
            debugText.MyArrow.localRotation = Quaternion.Euler(new Vector3(0, 0, 180));
        }
        else if (node.Parent.Position.x < node.Position.x && node.Parent.Position.y > node.Position.y)
        {
            debugText.MyArrow.localRotation = Quaternion.Euler(new Vector3(0, 0, 135));
        }
        else if (node.Parent.Position.x < node.Position.x && node.Parent.Position.y < node.Position.y)
        {
            debugText.MyArrow.localRotation = Quaternion.Euler(new Vector3(0, 0, 225));
        }
        else if (node.Parent.Position.x > node.Position.x && node.Parent.Position.y == node.Position.y)
        {
            debugText.MyArrow.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }
        else if (node.Parent.Position.x > node.Position.x && node.Parent.Position.y > node.Position.y)
        {
            debugText.MyArrow.localRotation = Quaternion.Euler(new Vector3(0, 0, 45));
        }
        else if (node.Parent.Position.x > node.Position.x && node.Parent.Position.y < node.Position.y)
        {
            debugText.MyArrow.localRotation = Quaternion.Euler(new Vector3(0, 0, -45));
        }
        else if (node.Parent.Position.x == node.Position.x && node.Parent.Position.y > node.Position.y)
        {
            debugText.MyArrow.localRotation = Quaternion.Euler(new Vector3(0, 0, 90));
        }
        else if (node.Parent.Position.x == node.Position.x && node.Parent.Position.y < node.Position.y)
        {
            debugText.MyArrow.localRotation = Quaternion.Euler(new Vector3(0, 0, 270));
        }
        if (node.Parent.Position.x < node.Position.x && node.Parent.Position.y == node.Position.y)
        {
            debugText.MyArrow.localRotation = Quaternion.Euler(new Vector3(0, 0, 180));
        }
    }

    public void ColorTile(Vector3Int position, Color color)
    {
        tileMap.SetTile(position, tile);
        tileMap.SetTileFlags(position, TileFlags.None);
        tileMap.SetColor(position, color);

    }

    public void ShowHide()
    {
        canvas.gameObject.SetActive(!canvas.isActiveAndEnabled);
        Color c = tileMap.color;
        c.a = c.a != 0 ? 0 : 1;
        tileMap.color = c;
    }

    public void Reset(Dictionary<Vector3Int, Node> allNodes)
    {
        foreach (GameObject go in debugObjects)
        {
            Destroy(go);
        }

        debugObjects.Clear();

        foreach (Vector3Int position in allNodes.Keys)
        {
            tileMap.SetTile(position, null);
        }
    }
}

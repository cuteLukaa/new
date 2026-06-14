using UnityEngine;
using System.Collections.Generic;

public class GridManager : MonoBehaviour
{
    [Header("Grid Settings")]
    public int width = 5;
    public int height = 5;
    public float spacing = 1.2f;

    [Header("Prefabs")]
    public GameObject nodePrefab;

    private Dictionary<Vector2Int, Node> gridNodes = new Dictionary<Vector2Int, Node>();

    void Start()
    {
        GenerateGrid();
        SpawnLevelDots();
    }

    void GenerateGrid()
    {
        Vector3 offset = new Vector3((width - 1) * spacing / 2f, (height - 1) * spacing / 2f, 0);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 spawnPosition = new Vector3(x * spacing, y * spacing, 0) - offset;
                GameObject spawnedNode = Instantiate(nodePrefab, spawnPosition, Quaternion.identity);
                spawnedNode.transform.parent = this.transform;
                spawnedNode.name = $"Node_{x}_{y}";

                Node nodeScript = spawnedNode.GetComponent<Node>();
                nodeScript.gridPosition = new Vector2Int(x, y);

                gridNodes.Add(new Vector2Int(x, y), nodeScript);
            }
        }
    }

    void SpawnLevelDots()
    {
        int currentLevel = 1;
        if (GameManager.Instance != null)
        {
            currentLevel = GameManager.Instance.currentLevel;
        }

        switch (currentLevel)
        {
            case 1:
                SetDotColor(0, 0, Color.red);     SetDotColor(4, 0, Color.red);
                SetDotColor(0, 1, Color.blue);    SetDotColor(4, 1, Color.blue);
                SetDotColor(0, 2, Color.green);   SetDotColor(4, 2, Color.green);
                SetDotColor(0, 3, Color.yellow);  SetDotColor(4, 3, Color.yellow);  
                SetDotColor(0, 4, Color.cyan);    SetDotColor(4, 4, Color.cyan);
                break;

            case 2:
                SetDotColor(1, 1, Color.red);     SetDotColor(3, 4, Color.red);
                SetDotColor(1, 0, Color.blue);    SetDotColor(0, 4, Color.blue);
                SetDotColor(2, 0, Color.green);   SetDotColor(4, 1, Color.green);
                SetDotColor(2, 2, Color.yellow);  SetDotColor(3, 3, Color.yellow);
                SetDotColor(2, 1, Color.cyan);    SetDotColor(4, 4, Color.cyan);
                break;

            case 3:
                SetDotColor(2, 2, Color.red);     SetDotColor(3, 3, Color.red);
                SetDotColor(0, 0, Color.blue);    SetDotColor(2, 4, Color.blue);
                SetDotColor(3, 0, Color.green);   SetDotColor(3, 4, Color.green);
                SetDotColor(0, 1, Color.yellow);  SetDotColor(1, 4, Color.yellow);
                SetDotColor(2, 0, Color.magenta); SetDotColor(3, 1, Color.magenta);
                break;

            case 4:
                SetDotColor(0, 0, Color.red);     SetDotColor(1, 1, Color.red);
                SetDotColor(1, 0, Color.blue);    SetDotColor(2, 1, Color.blue);
                SetDotColor(1, 2, Color.green);   SetDotColor(0, 4, Color.green);
                SetDotColor(1, 4, Color.yellow);  SetDotColor(4, 4, Color.yellow);
                SetDotColor(4, 0, Color.cyan);    SetDotColor(1, 3, Color.cyan);
                SetDotColor(3, 0, Color.orange);    SetDotColor(2, 2, Color.orange);
                break;

            case 5:
                SetDotColor(2, 3, Color.red);     SetDotColor(4, 4, Color.red);
                SetDotColor(3, 1, Color.blue);    SetDotColor(3, 3, Color.blue);
                SetDotColor(0, 2, Color.green);   SetDotColor(0, 4, Color.green);
                SetDotColor(1, 2, Color.yellow);  SetDotColor(1, 4, Color.yellow);
                SetDotColor(4, 0, Color.cyan);    SetDotColor(1, 0, Color.cyan);
                SetDotColor(0, 0, Color.orange);    SetDotColor(3, 2, Color.orange);
                break;

            default:
                SetDotColor(0, 0, Color.red);     SetDotColor(4, 4, Color.red);
                SetDotColor(4, 0, Color.blue);    SetDotColor(0, 4, Color.blue);
                break;
        }
    }

    void SetDotColor(int x, int y, Color color)
    {
        Vector2Int pos = new Vector2Int(x, y);
        if (gridNodes.ContainsKey(pos))
        {
            gridNodes[pos].SetAsTargetDot(color);
        }
        else
        {
            Debug.LogWarning($"Анхаар: ({x}, {y}) байрлал дээр Node алга байна. Координатыг шалгана уу!");
        }
    }
}
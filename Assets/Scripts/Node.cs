using UnityEngine;

public class Node : MonoBehaviour
{
    [Header("Node Info")]
    public Vector2Int gridPosition;
    public Color nodeColor = Color.clear;
    public bool isTargetDot = false;

    [HideInInspector]
    public string occupiedByLineId = "";

    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetAsTargetDot(Color color)
    {
        isTargetDot = true;
        nodeColor = color;

        GameObject dot = new GameObject("Dot");
        dot.transform.parent = this.transform;
        dot.transform.localPosition = Vector3.zero;
        dot.transform.localScale = new Vector3(0.5f, 0.5f, 1f);

        SpriteRenderer dotRenderer = dot.AddComponent<SpriteRenderer>();
        dotRenderer.sprite = GetComponent<SpriteRenderer>().sprite; 
        dotRenderer.color = color;
        dotRenderer.sortingOrder = 1;
    }
}
using UnityEngine;
using System.Collections.Generic;

public class LineDrawer : MonoBehaviour
{
    private LineRenderer currentLine;
    private List<Node> currentPath = new List<Node>();
    private Color activeColor;
    private string activeColorId;

    private Dictionary<string, GameObject> activeLines = new Dictionary<string, GameObject>();

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TryStartDrawing();
        }
        else if (Input.GetMouseButton(0) && currentLine != null)
        {
            TryContinueDrawing();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            EndDrawing();
        }
    }

    void TryStartDrawing()
    {
        Node hitNode = GetNodeUnderMouse();
        
        if (hitNode != null)
        {
            if (hitNode.isTargetDot || hitNode.occupiedByLineId != "")
            {
                string targetColorId = hitNode.isTargetDot ? hitNode.nodeColor.ToString() : hitNode.occupiedByLineId;
                ClearLine(targetColorId);

                if (hitNode.isTargetDot)
                {
                    StartNewLine(hitNode);
                }
            }
        }
    }

    void StartNewLine(Node startNode)
    {
        currentPath.Clear();
        activeColor = startNode.nodeColor;
        activeColorId = startNode.nodeColor.ToString();

        GameObject lineObj = new GameObject("Line_" + activeColorId);
        currentLine = lineObj.AddComponent<LineRenderer>();
        
        currentLine.startWidth = 0.3f;
        currentLine.endWidth = 0.3f;
        currentLine.material = new Material(Shader.Find("Sprites/Default"));
        currentLine.startColor = activeColor;
        currentLine.endColor = activeColor;
        currentLine.sortingOrder = 2;
        currentLine.positionCount = 0;

        AddNodeToPath(startNode);
    }

    void TryContinueDrawing()
    {
        Node hitNode = GetNodeUnderMouse();
        
        if (hitNode != null && !currentPath.Contains(hitNode))
        {
            Node lastNode = currentPath[currentPath.Count - 1];
            float distance = Vector2Int.Distance(lastNode.gridPosition, hitNode.gridPosition);
            
            if (distance == 1f)
            {
                if (hitNode.occupiedByLineId != "" && hitNode.occupiedByLineId != activeColorId)
                {
                    ClearLine(hitNode.occupiedByLineId);
                }

                if (hitNode.isTargetDot && hitNode.nodeColor == activeColor)
                {
                    AddNodeToPath(hitNode);
                    EndDrawing();
                    return;
                }

                if (hitNode.isTargetDot && hitNode.nodeColor != activeColor)
                    return;

                AddNodeToPath(hitNode);
            }
        }
    }

    void EndDrawing()
    {
        if (currentLine != null)
        {
            Node lastNode = currentPath[currentPath.Count - 1];
            
            if (lastNode.isTargetDot && lastNode.nodeColor == activeColor && currentPath.Count > 1)
            {
                if (activeLines.ContainsKey(activeColorId))
                {
                    Destroy(activeLines[activeColorId]);
                    activeLines[activeColorId] = currentLine.gameObject;
                }
                else
                {
                    activeLines.Add(activeColorId, currentLine.gameObject);
                }

                foreach (Node node in currentPath)
                {
                    node.occupiedByLineId = activeColorId;
                }
            }
            else
            {
                Destroy(currentLine.gameObject);
            }
            
            currentLine = null;
        }
        if (GameManager.Instance != null)
        {
            GameManager.Instance.CheckWinCondition();
        }
    }

    void AddNodeToPath(Node node)
    {
        currentPath.Add(node);
        currentLine.positionCount = currentPath.Count;
        currentLine.SetPosition(currentPath.Count - 1, new Vector3(node.transform.position.x, node.transform.position.y, -0.1f));
    }

    void ClearLine(string colorId)
    {
        if (activeLines.ContainsKey(colorId))
        {
            Destroy(activeLines[colorId]);
            activeLines.Remove(colorId);

            Node[] allNodes = FindObjectsByType<Node>(FindObjectsSortMode.None);
            foreach (Node node in allNodes)
            {
                if (node.occupiedByLineId == colorId)
                {
                    node.occupiedByLineId = "";
                }
            }
        }
    }

    Node GetNodeUnderMouse()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
        
        if (hit.collider != null)
        {
            return hit.collider.GetComponent<Node>();
        }
        return null;
    }
}
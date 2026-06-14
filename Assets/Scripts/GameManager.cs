using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI Panels")]
    public GameObject winPanel;

    [Header("Level Progress")]
    public int currentLevel = 1;

    private int totalNodesCount;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GridManager grid = FindFirstObjectByType<GridManager>();
        if (grid != null)
        {
            totalNodesCount = grid.width * grid.height;
        }
        
        FindWinPanel();
    }

    void FindWinPanel()
    {
        Canvas mainCanvas = FindFirstObjectByType<Canvas>();
        if (mainCanvas != null)
        {
            Transform panelTransform = mainCanvas.transform.Find("WinPanel");
            if (panelTransform != null)
            {
                winPanel = panelTransform.gameObject;
                winPanel.SetActive(false);

                Button nextBtn = panelTransform.Find("NextButton").GetComponent<Button>();
                Button replayBtn = panelTransform.Find("ReplayButton").GetComponent<Button>();

                if (nextBtn != null)
                {
                    nextBtn.onClick.RemoveAllListeners(); 
                    nextBtn.onClick.AddListener(NextLevel);
                }

                if (replayBtn != null)
                {
                    replayBtn.onClick.RemoveAllListeners();
                    replayBtn.onClick.AddListener(ReplayLevel);
                }
                // --------------------------------------------------------
            }
            else
            {
                Debug.LogWarning("Canvas дотроос 'WinPanel' нэртэй обьект олдсонгүй!");
            }
        }
    }

    public void CheckWinCondition()
    {
        if (winPanel == null)
        {
            FindWinPanel();
        }

        Node[] allNodes = FindObjectsByType<Node>(FindObjectsSortMode.None);
        int occupiedCount = 0;

        foreach (Node node in allNodes)
        {
            if (node.occupiedByLineId != "" || node.isTargetDot)
            {
                occupiedCount++;
            }
        }

        if (occupiedCount == totalNodesCount)
        {
            if (winPanel != null) 
            {
                winPanel.SetActive(true);
                Debug.Log("Яг одоо WinPanel нээгдлээ!");
            }
        }
    }

    public void NextLevel()
    {
        if (currentLevel < 5)
        {
            currentLevel++;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else
        {
            Debug.Log("Бүх үеийг дуусгалаа!");
        }
    }

    public void ReplayLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
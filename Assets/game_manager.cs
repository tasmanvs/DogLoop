using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class game_manager : MonoBehaviour
{
    public static game_manager instance;
    private int timer;
    private int deductions;
    private int nextObstacleIndex;
    private bool roundStarted;
    
    [Header("UI References")]
    public TextMeshProUGUI scoreText;
    
    [Header("Obstacle Door Order")]
    [SerializeField] private List<GameObject> gameObjects = new List<GameObject>();

    [Header("Start-Finish")]
    public GameObject startFinish;

    [Header("Player Dog")]
    public GameObject dog;

    private void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
        }
        instance = this;
        DontDestroyOnLoad(this);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        dog.transform.position = startFinish.transform.position;
        roundStarted = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartRound()
    {
        ResetGame();
        roundStarted = true;
        InvokeRepeating("IncrementTimer", 1.0f, 1.0f);
    }

    public void StopRound()
    {
        if(roundStarted)
        {
            CancelInvoke("IncrementTimer");
            roundStarted = false;
        }
    }

    public void ResetGame()
    {
        nextObstacleIndex = 0;
        deductions = 0;
        timer = 0;
        UpdateScoreText();
    }

    public void IncrementTimer()
    {
        timer++;
        UpdateScoreText();
    }
    

    public void TakeDeduction(int amount)
    {
        deductions += amount;
        UpdateScoreText();
    }

    public bool CheckCorrectNextObstacle(GameObject obstacle)
    {
        return obstacle == NextObstacle();
    }

    public GameObject NextObstacle()
    {
        if (nextObstacleIndex < gameObjects.Count)
        {
            return gameObjects[nextObstacleIndex];
        }
        return startFinish;
    }

    public void IncrementNextObstacleIndex()
    {
        nextObstacleIndex++;
    }

    private void UpdateScoreText()
    {
        scoreText.text = "Time: " + timer.ToString() + "\nDeductions: " + deductions.ToString();
    }
}

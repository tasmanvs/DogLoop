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

    private int bestScore = int.MaxValue;
    
    [Header("Current Score Text")]
    public TextMeshProUGUI scoreText;

    [Header("Previous Scores Text")]
    public TextMeshProUGUI pastScoresText;
    
    [Header("Obstacle Door Order")]
    [SerializeField] private List<GameObject> obstacleList = new List<GameObject>();

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
        if(!roundStarted)
        {
            Debug.LogWarning("StopRound called but round was never started");
            return;
        }
        CancelInvoke("IncrementTimer");
        roundStarted = false;

        Debug.Log($"next obstacle index: {nextObstacleIndex}. obstacleList.Count = {obstacleList.Count}");

        if(nextObstacleIndex < obstacleList.Count)
        {
            scoreText.text = "Not all obstacles completed!\nDISQUALIFIED";
            return;
        }

        int finalScore = timer + deductions;
        if (bestScore > finalScore) {
            bestScore = finalScore;
        }
        scoreText.text = $"{timer}s + {deductions} deductions\n Final Score: {finalScore}";
        pastScoresText.text = $"Last Round: {finalScore}\nBest Score: {bestScore}";
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
        if (nextObstacleIndex < obstacleList.Count)
        {
            return obstacleList[nextObstacleIndex];
        }
        return startFinish;
    }

    public void IncrementNextObstacleIndex()
    {
        nextObstacleIndex++;
    }

    private void UpdateScoreText()
    {
        scoreText.text = $"Time: {timer}\nDeductions: {deductions}";
    }
}

using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class game_manager : MonoBehaviour
{
    public static game_manager instance;
    private int timer;
    private int deductions;
    private int nextObstacleIndex;
    
    [Header("UI References")]
    public TextMeshProUGUI scoreText;
    
    [Header("Obstacle Door Order")]
    [SerializeField] private List<GameObject> gameObjects = new List<GameObject>();

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
        deductions = 0;
        timer = 0;
        UpdateScoreText();
        InvokeRepeating("IncrementTimer", 1.0f, 1.0f);
    }

    public void ResetGame()
    {
        deductions = 0;
        timer = 0;
        UpdateScoreText();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void IncrementTimer()
    {
        timer++;
        UpdateScoreText();
    }

    public void TakeDeduction(int amount)
    {
        deductions -= amount;
        UpdateScoreText();
    }

    public bool CheckCorrectNextObstacle(GameObject obstacle)
    {
        if (nextObstacleIndex < gameObjects.Count)
        {
            return gameObjects[nextObstacleIndex] == obstacle;
        }
        return false;
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

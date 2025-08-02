using UnityEngine;
using TMPro;

public class game_manager : MonoBehaviour
{
    public static game_manager instance;
    private int timer;
    private int deductions;
    
    [Header("UI References")]
    public TextMeshProUGUI scoreText;

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
        updateScoreText();
        InvokeRepeating("IncrementTimer", 1.0f, 1.0f);
    }

    public void ResetGame()
    {
        deductions = 0;
        timer = 0;
        updateScoreText();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void IncrementTimer()
    {
        timer++;
        updateScoreText();
    }

    public void TakeDeduction(int amount)
    {
        deductions -= amount;
        updateScoreText();
    }

    private void updateScoreText()
    {
        scoreText.text = "Time: " + timer.ToString() + "\nDeductions: " + deductions.ToString();
    }
}

using UnityEngine;
using TMPro;

public class game_manager : MonoBehaviour
{
    public static game_manager instance;
    private int timer;
    private int deductions;

    private void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
        }
        instance = this;
        DontDestroyOnLoad(this)
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        deductions = 0;
        timer = 0;
        timerText.text = timer.ToString();
        deductionsText.text = deductions.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

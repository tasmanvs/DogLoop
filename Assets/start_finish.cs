using UnityEngine;

public class start_finish : MonoBehaviour
{    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Ensure this GameObject has a Collider2D for trigger detection
        Collider2D collider = GetComponent<Collider2D>();
        if (collider == null)
        {
            Debug.LogWarning("start_finish requires a Collider2D component for trigger detection!");
        }
        else if (!collider.isTrigger)
        {
            Debug.LogWarning("start_finish Collider2D should be set to 'Is Trigger' for proper detection!");
        }

        if (game_manager.instance == null)
        {
            Debug.LogWarning("no game instance found");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collider belongs to the player
        if (other.CompareTag("Player"))
        {
            Debug.Log($"Player entered start_finish point at {transform.position}");
            // You can add additional logic here for start/finish behavior
            game_manager.instance.StopRound();
        }
    }
    
    void OnTriggerExit2D(Collider2D other)
    {
        // Check if the collider belongs to the player
        if (other.CompareTag("Player"))
        {
            game_manager.instance.StartRound();
            // add start game logic
            Debug.Log($"Player exited starting point at {transform.position}");
        }
    }
}

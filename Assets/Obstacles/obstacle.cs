using System.ComponentModel.Design;
using UnityEngine;
using UnityEngine.InputSystem; // Add this for the new Input System

public class obstacle : MonoBehaviour
{
    public BoxCollider2D door_a;
    public BoxCollider2D door_b;

    public string obstacleWord = "UDLR"; // The word the player must spell to pass through the obstacle

    private bool awaitingInput = false;
    private string playerInput = "";
    private Vector2 originalPlayerPosition;
    private Collider2D currentPlayerCollider;
    private BoxCollider2D enteredDoor;
    private BoxCollider2D targetDoor;
    private ObstacleDisplay obstacleDisplay;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        obstacleDisplay = GetComponentInChildren<ObstacleDisplay>(true);
        if (obstacleDisplay == null)
        {
            Debug.LogWarning("ObstacleDisplay child not found on obstacle: " + gameObject.name);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (awaitingInput && currentPlayerCollider != null)
        {
            if (game_manager.instance == null)
            {
                Debug.Log("Game manager instance is null!");
                return;
            }

            // Check that player is at correct next door
            if (!game_manager.instance.CheckCorrectNextObstacle(enteredDoor.gameObject)) {
                FailObstacle(10);
                Debug.Log("Incorrect obstacle! Returned to previous position.");
                return;
            }

            // Listen for arrow key input and build the input string using the new Input System
            if (Keyboard.current.upArrowKey.wasPressedThisFrame) playerInput += "U";
            if (Keyboard.current.downArrowKey.wasPressedThisFrame) playerInput += "D";
            if (Keyboard.current.leftArrowKey.wasPressedThisFrame) playerInput += "L";
            if (Keyboard.current.rightArrowKey.wasPressedThisFrame) playerInput += "R";

            // Check for immediate failure
            int len = playerInput.Length;
            if (len > 0 && len <= obstacleWord.Length)
            {
                if (playerInput[len - 1] != obstacleWord[len - 1])
                {
                    // Incorrect input at this step: fail immediately
                    FailObstacle(5);
                    Debug.Log("Incorrect string! Returned to previous position.");
                    return;
                }
            }

            // If input is complete and correct, teleport
            if (playerInput.Length == obstacleWord.Length)
            {
                currentPlayerCollider.transform.position = targetDoor.transform.position;
                Debug.Log("Correct! Teleported to the other door.");
                ResetObstacleState();
                game_manager.instance.IncrementNextObstacleIndex();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D player_collider)
    {
        // Print debug message about player_collider.
        Debug.Log("Obstacle triggered by: " + player_collider.name);
        // Print the tag
        Debug.Log("Obstacle triggered by tag: " + player_collider.tag);
        // Check if the collider belongs to the player
        if (player_collider.CompareTag("Player"))
        {
            Vector2 playerPos = player_collider.transform.position;
            // Check which door the player is overlapping
            // And move the xy position to the other door's position
            if (door_a.OverlapPoint(playerPos))
            {
                enteredDoor = door_a;
                targetDoor = door_b;
            }
            else if (door_b.OverlapPoint(playerPos))
            {
                enteredDoor = door_b;
                targetDoor = door_a;
            }
            else
            {
                return;
            }

            // No need to store previousPosition from player script anymore
            currentPlayerCollider = player_collider;
            awaitingInput = true;
            playerInput = "";
            player.isFrozen = true; // Freeze player while awaiting input
            Debug.Log("Enter the obstacle word using arrow keys: " + obstacleWord);
            if (obstacleDisplay != null)
            {
                obstacleDisplay.SetCode(obstacleWord);
                obstacleDisplay.ShowDisplay();
            }
            else
            {
                Debug.LogWarning("ObstacleDisplay not found!");
            }
        }
    }

    private void FailObstacle(int deduction)
    {
        player playerScript = currentPlayerCollider.GetComponent<player>();
            if (playerScript != null)
            {
                playerScript.ReverseLastMove();
            }
            
            // Apply deduction for incorrect input
            game_manager.instance.TakeDeduction(deduction);
            if (obstacleDisplay != null) obstacleDisplay.HideDisplay();
            ResetObstacleState();
    }
    
    private void ResetObstacleState()
    {
        awaitingInput = false;
        playerInput = "";
        currentPlayerCollider = null;
        enteredDoor = null;
        targetDoor = null;
        player.isFrozen = false;
        if (obstacleDisplay != null) obstacleDisplay.HideDisplay();
        Debug.Log("Obstacle state reset");
    }
}

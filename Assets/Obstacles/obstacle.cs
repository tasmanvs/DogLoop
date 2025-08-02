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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (awaitingInput && currentPlayerCollider != null)
        {
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
                    player playerScript = currentPlayerCollider.GetComponent<player>();
                    if (playerScript != null)
                    {
                        playerScript.ReverseLastMove();
                    }
                    
                    // Apply deduction for incorrect input
                    ApplyDeduction(5);
                    
                    Debug.Log("Incorrect! Returned to previous position.");
                    // Reset state
                    awaitingInput = false;
                    playerInput = "";
                    currentPlayerCollider = null;
                    enteredDoor = null;
                    targetDoor = null;
                    player.isFrozen = false;
                    return;
                }
            }

            // If input is complete and correct, teleport
            if (playerInput.Length == obstacleWord.Length)
            {
                currentPlayerCollider.transform.position = targetDoor.transform.position;
                Debug.Log("Correct! Teleported to the other door.");
                // Reset state
                awaitingInput = false;
                playerInput = "";
                currentPlayerCollider = null;
                enteredDoor = null;
                targetDoor = null;
                player.isFrozen = false;
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
        }
    }
    
    private void ApplyDeduction(int amount)
    {
        if (game_manager.instance != null)
        {
            game_manager.instance.TakeDeduction(amount);
            Debug.Log($"Applied deduction of {amount} points");
        }
        else
        {
            Debug.LogWarning("Game manager instance not found! Cannot apply deduction.");
        }
    }
}

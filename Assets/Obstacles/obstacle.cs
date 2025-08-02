using System.ComponentModel.Design;
using UnityEngine;

public class obstacle : MonoBehaviour
{
    public BoxCollider2D door_a;
    public BoxCollider2D door_b;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Print debug message about other.
        Debug.Log("Obstacle triggered by: " + other.name);
        // Print the tag
        Debug.Log("Obstacle triggered by tag: " + other.tag);
        // Check if the collider belongs to the player
        if (other.CompareTag("Player"))
        {
            Vector3 playerPos = other.transform.position;
            // Check which door the player is overlapping
            if (door_a.OverlapPoint(playerPos))
            {
                // other.transform.position = door_b.transform.position;
                // Just move the xy position to the other door's position
                other.transform.position = new Vector3(door_b.transform.position.x, door_b.transform.position.y, playerPos.z);
                // Debug message to confirm the teleportation
            }
            else if (door_b.OverlapPoint(playerPos))
            {
                // other.transform.position = door_a.transform.position;
                // Just move the xy position to the other door's position
                other.transform.position = new Vector3(door_a.transform.position.x, door_a.transform.position.y, playerPos.z);
            }
        }
    }
}

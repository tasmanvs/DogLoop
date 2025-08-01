using UnityEngine;
using UnityEngine.InputSystem;

public class player : MonoBehaviour
{
    public float moveAmount = 1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 move = Vector3.zero;
        var keyboard = Keyboard.current;
        if (keyboard != null)
        {
            if (keyboard.leftArrowKey.wasPressedThisFrame) move.x -= moveAmount;
            if (keyboard.rightArrowKey.wasPressedThisFrame) move.x += moveAmount;
            if (keyboard.upArrowKey.wasPressedThisFrame) move.y += moveAmount;
            if (keyboard.downArrowKey.wasPressedThisFrame) move.y -= moveAmount;
        }
        if (move != Vector3.zero)
        {
            transform.Translate(move, Space.World);
        }
    }
}

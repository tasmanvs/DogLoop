using UnityEngine;
using UnityEngine.InputSystem;

public class player : MonoBehaviour
{
    public static bool isFrozen = false; // Add this flag
    public float moveAmount = 1f;

    // Sprites for each direction
    public Sprite leftSprite;
    public Sprite rightSprite;
    public Sprite upSprite;
    public Sprite downSprite;

    private SpriteRenderer spriteRenderer;

    private Vector2 lastMove = Vector2.zero; // Track last move direction

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isFrozen) return; // Prevent movement if frozen

        Vector3 move = Vector3.zero;
        var keyboard = Keyboard.current;
        if (keyboard != null)
        {
            if (keyboard.leftArrowKey.wasPressedThisFrame) {
                move.x -= moveAmount;
                lastMove = Vector2.left;
                if (leftSprite != null && spriteRenderer != null)
                    spriteRenderer.sprite = leftSprite;
            }
            if (keyboard.rightArrowKey.wasPressedThisFrame) {
                move.x += moveAmount;
                lastMove = Vector2.right;
                if (rightSprite != null && spriteRenderer != null)
                    spriteRenderer.sprite = rightSprite;
            }
            if (keyboard.upArrowKey.wasPressedThisFrame) {
                move.y += moveAmount;
                lastMove = Vector2.up;
                if (upSprite != null && spriteRenderer != null)
                    spriteRenderer.sprite = upSprite;
            }
            if (keyboard.downArrowKey.wasPressedThisFrame) {
                move.y -= moveAmount;
                lastMove = Vector2.down;
                if (downSprite != null && spriteRenderer != null)
                    spriteRenderer.sprite = downSprite;
            }
        }
        if (move != Vector3.zero)
        {
            transform.Translate(move, Space.World);
        }
    }

    public void ReverseLastMove()
    {
        // Move in the opposite direction of lastMove and update sprite
        if (lastMove == Vector2.left) {
            transform.Translate(Vector2.right * moveAmount, Space.World);
            if (rightSprite != null && spriteRenderer != null)
                spriteRenderer.sprite = rightSprite;
        }
        else if (lastMove == Vector2.right) {
            transform.Translate(Vector2.left * moveAmount, Space.World);
            if (leftSprite != null && spriteRenderer != null)
                spriteRenderer.sprite = leftSprite;
        }
        else if (lastMove == Vector2.up) {
            transform.Translate(Vector2.down * moveAmount, Space.World);
            if (downSprite != null && spriteRenderer != null)
                spriteRenderer.sprite = downSprite;
        }
        else if (lastMove == Vector2.down) {
            transform.Translate(Vector2.up * moveAmount, Space.World);
            if (upSprite != null && spriteRenderer != null)
                spriteRenderer.sprite = upSprite;
        }
    }
}

using UnityEngine;
using UnityEngine.InputSystem;

public class player : MonoBehaviour
{
    public float moveAmount = 1f;

    // Sprites for each direction
    public Sprite leftSprite;
    public Sprite rightSprite;
    public Sprite upSprite;
    public Sprite downSprite;

    private SpriteRenderer spriteRenderer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 move = Vector3.zero;
        var keyboard = Keyboard.current;
        if (keyboard != null)
        {
            if (keyboard.leftArrowKey.wasPressedThisFrame) {
                move.x -= moveAmount;
                if (leftSprite != null && spriteRenderer != null)
                    spriteRenderer.sprite = leftSprite;
            }
            if (keyboard.rightArrowKey.wasPressedThisFrame) {
                move.x += moveAmount;
                if (rightSprite != null && spriteRenderer != null)
                    spriteRenderer.sprite = rightSprite;
            }
            if (keyboard.upArrowKey.wasPressedThisFrame) {
                move.y += moveAmount;
                if (upSprite != null && spriteRenderer != null)
                    spriteRenderer.sprite = upSprite;
            }
            if (keyboard.downArrowKey.wasPressedThisFrame) {
                move.y -= moveAmount;
                if (downSprite != null && spriteRenderer != null)
                    spriteRenderer.sprite = downSprite;
            }
        }
        if (move != Vector3.zero)
        {
            transform.Translate(move, Space.World);
        }
    }
}

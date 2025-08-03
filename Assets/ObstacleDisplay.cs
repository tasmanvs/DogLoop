using System.Collections.Generic;
using UnityEngine;

public class ObstacleDisplay : MonoBehaviour
{
    [Header("Arrow Sprites")]
    public Sprite upArrowSprite;
    public Sprite downArrowSprite;
    public Sprite leftArrowSprite;
    public Sprite rightArrowSprite;

    private readonly Dictionary<char, Sprite> _arrowSpriteMap = new Dictionary<char, Sprite>();

    private void Awake()
    {
        _arrowSpriteMap['U'] = upArrowSprite;
        _arrowSpriteMap['D'] = downArrowSprite;
        _arrowSpriteMap['L'] = leftArrowSprite;
        _arrowSpriteMap['R'] = rightArrowSprite;
        
        HideDisplay();
    }

    public void SetCode(string code)
    {
        UpdateDisplay(code);
    }

    public void ShowDisplay() => gameObject.SetActive(true);

    public void HideDisplay() => gameObject.SetActive(false);

    private void UpdateDisplay(string code)
    {
        // Clear existing arrows
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        // Create new arrows
        for (int i = 0; i < code.Length; i++)
        {
            if (!_arrowSpriteMap.TryGetValue(code[i], out Sprite sprite)) continue;

            var arrow = new GameObject("Arrow_" + i);
            arrow.transform.SetParent(transform);
            arrow.transform.localPosition = new Vector3(i * 1.1f, 0, 0); // Space arrows horizontally
            
            var sr = arrow.AddComponent<SpriteRenderer>();
            sr.sprite = sprite;
        }
    }
}

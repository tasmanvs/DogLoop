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
    private readonly List<GameObject> _arrowGameObjects = new List<GameObject>();

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

        _arrowGameObjects.Clear();

        // Create new arrows
        for (int i = 0; i < code.Length; i++)
        {
            if (!_arrowSpriteMap.TryGetValue(code[i], out Sprite sprite))
            {
                _arrowGameObjects.Add(null); // Add a placeholder for non-displayable characters
                continue;
            }

            var arrow = new GameObject("Arrow_" + i);
            arrow.transform.SetParent(transform);
            arrow.transform.localPosition = new Vector3(i * 1.1f, 0, 0); // Space arrows horizontally
            
            var sr = arrow.AddComponent<SpriteRenderer>();
            sr.sprite = sprite;
            _arrowGameObjects.Add(arrow);
        }
    }

    public void OnCorrectInput(int index)
    {
        if (index < 0 || index >= _arrowGameObjects.Count) return;

        var arrowToRemove = _arrowGameObjects[index];
        if (arrowToRemove != null)
        {
            StartCoroutine(AnimateFlyAway(arrowToRemove));
            _arrowGameObjects[index] = null; // Mark as removed
        }

        // Animate remaining arrows to slam left
        for (int i = index + 1; i < _arrowGameObjects.Count; i++)
        {
            var arrowToMove = _arrowGameObjects[i];
            if (arrowToMove != null)
            {
                var targetPosition = arrowToMove.transform.localPosition - new Vector3(1.1f, 0, 0);
                StartCoroutine(AnimateSlamLeft(arrowToMove, targetPosition));
            }
        }
    }

    private System.Collections.IEnumerator AnimateFlyAway(GameObject arrow)
    {
        const float duration = 0.5f;
        var elapsed = 0f;

        var startPosition = arrow.transform.position;
        var randomSpin = new Vector3(Random.Range(-360, 360), Random.Range(-360, 360), Random.Range(-360, 360));
        
        while (elapsed < duration)
        {
            arrow.transform.position += new Vector3(0, 5f, 0) * Time.deltaTime;
            arrow.transform.Rotate(randomSpin * Time.deltaTime);
            elapsed += Time.deltaTime;
            yield return null;
        }

        Destroy(arrow);
    }

    private System.Collections.IEnumerator AnimateSlamLeft(GameObject arrow, Vector3 targetPosition)
    {
        const float duration = 0.1f;
        var elapsed = 0f;
        var startPosition = arrow.transform.localPosition;

        while (elapsed < duration)
        {
            var t = elapsed / duration;
            arrow.transform.localPosition = Vector3.Lerp(startPosition, targetPosition, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        arrow.transform.localPosition = targetPosition;
    }
}

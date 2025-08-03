using UnityEngine;

public class ObstacleDisplay : MonoBehaviour
{
    public Sprite upArrowSprite;
    public Sprite downArrowSprite;
    public Sprite leftArrowSprite;
    public Sprite rightArrowSprite;

    private string currentCode = "";
    private GameObject[] arrowObjects = null;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        HideDisplay();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetCode(string code)
    {
        currentCode = code;
        UpdateDisplay();
    }

    public void ShowDisplay()
    {
        gameObject.SetActive(true);
        Debug.Log("ObstacleDisplay: ShowDisplay called with code: " + currentCode);
    }

    public void HideDisplay()
    {
        gameObject.SetActive(false);
    }

    private void UpdateDisplay()
    {
        // Destroy old arrows
        if (arrowObjects != null)
        {
            foreach (var obj in arrowObjects)
            {
                if (obj != null) Destroy(obj);
            }
        }
        arrowObjects = new GameObject[currentCode.Length];

        // Create new arrows as child GameObjects with SpriteRenderer
        for (int i = 0; i < currentCode.Length; i++)
        {
            GameObject arrow = new GameObject("Arrow_" + i);
            arrow.transform.SetParent(transform);
            arrow.transform.localPosition = new Vector3(i * 1.1f, 0, 0); // space arrows horizontally
            var sr = arrow.AddComponent<SpriteRenderer>();
            switch (currentCode[i])
            {
                case 'U': sr.sprite = upArrowSprite; break;
                case 'D': sr.sprite = downArrowSprite; break;
                case 'L': sr.sprite = leftArrowSprite; break;
                case 'R': sr.sprite = rightArrowSprite; break;
                default: sr.sprite = null; break;
            }
            arrowObjects[i] = arrow;
        }
    }
}

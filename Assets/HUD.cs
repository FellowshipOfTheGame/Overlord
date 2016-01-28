using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HUD : MonoBehaviour
{
    public Life playerLife;
    public Health playerHealth;

    public RectTransform HPTransform;
    public RectTransform lifeTransform;
    public GameObject gameOver;
    public Text text;
    public GameObject heartUiPrefab;
    public float size = 32;

    private int currentLives = -1;
    private int currentHearts = 0;
    private Stack hearts = new Stack();

    void Update()
    {
        UpdateHP();
        UpdateLife();
        if (playerLife.gameOver)
            gameOver.active = true;
    }

    void UpdateLife()
    {
        if (currentLives != playerLife.currentLives)
        {
            currentLives = playerLife.currentLives;
            text.text = "X " + currentLives.ToString();
        }
    }

    void UpdateHP()
    {
        if (playerHealth && playerHealth.currentHitPoints != currentHearts)
        {
            if (playerHealth.currentHitPoints > currentHearts)
            {
                for (int i = 0; i < playerHealth.currentHitPoints - currentHearts; i++)
                {
                    GameObject heart = Instantiate(heartUiPrefab);
                    RectTransform heartTransform = heart.GetComponent<RectTransform>();
                    if (heartTransform)
                    {
                        heartTransform.sizeDelta = new Vector2(size, size);
                        heartTransform.position = new Vector3((i + currentHearts) * size, 0f, 0f);
                        heartTransform.SetParent(HPTransform.transform, false);
                    }
                    hearts.Push(heart);
                }
            }
            else
            {
                for (int i = 0; i < currentHearts - playerHealth.currentHitPoints; i++)
                    Destroy((GameObject)hearts.Pop());
            }

            currentHearts = playerHealth.currentHitPoints;
        }
    }

    IEnumerator GameOver()
    {
        yield return new WaitForEndOfFrame();
    }
}

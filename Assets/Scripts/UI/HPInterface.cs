using UnityEngine;
using System.Collections;

public class HPInterface : MonoBehaviour
{
    [HideInInspector]
    public Health playerHealth;
    [HideInInspector]
    public Life playerLife;
    public GameObject heartUiPrefab;
    public float size = 32;

    private int currentHearts = 0;
    private Stack hearts = new Stack();

    void Update()
    {
        if (playerLife.gameOver)
            StartCoroutine("GameOver");
        else if(playerHealth && playerHealth.currentHitPoints != currentHearts)
        {
            if(playerHealth.currentHitPoints > currentHearts)
            {
                for (int i = 0; i < playerHealth.currentHitPoints - currentHearts; i++)
                {
                    GameObject heart = Instantiate(heartUiPrefab);
                    RectTransform hTransform = heart.GetComponent<RectTransform>();
                    if(hTransform)
                    {
                        hTransform.sizeDelta =  new Vector2(size, size);
                        hTransform.position = new Vector3((i + currentHearts) * size, 0f, 0f);
                        hTransform.SetParent(gameObject.transform, false);
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

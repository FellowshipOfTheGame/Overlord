using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Life : MonoBehaviour
{
    [HideInInspector]
    public CheckPoint lastCheckpoint;
    private Rigidbody2D rigid;

    public int maxLives = 99;
    public int currentLives = 5;
    public bool gameOver = false;


    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    bool GameOver()
    {
        gameOver = true;
        return true;
    }

    public bool Die()
    {
        if (currentLives == 0)
            return GameOver();

        currentLives--;
        if (rigid)
            rigid.velocity = Vector2.zero;
        transform.position = lastCheckpoint.transform.position;
        return false;

    }
}

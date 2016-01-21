using UnityEngine;
using System.Collections;

public class Life : MonoBehaviour
{

    private Rigidbody2D rigid;
    [HideInInspector]
    public CheckPoint lastCheckpoint;

    public int maxLives = 99;
    public int currentLives = 5;


    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    bool GameOver()
    {
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

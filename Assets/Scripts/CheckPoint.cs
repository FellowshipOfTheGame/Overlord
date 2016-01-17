using UnityEngine;
using System.Collections;

public class CheckPoint : MonoBehaviour {

    public bool active = false;

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (active)
            return;

        Life objectLife = collider.gameObject.GetComponent<Life>();
        if (objectLife)
        {
            objectLife.lastCheckpoint = this;
            active = true;
        }
    }
}

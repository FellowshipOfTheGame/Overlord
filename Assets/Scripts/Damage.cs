using UnityEngine;
using System.Collections;

public class Damage : MonoBehaviour {
    public bool instaKill = false;
    public int hitPointDamage = 1;
    public float pushForce = 10f;
    public string enemyTag = "Player";

    void OnCollisionStay2D(Collision2D col)
    {
        Health objectHealth = col.gameObject.GetComponent<Health>();
        if (objectHealth)
            objectHealth.Damage(gameObject, this);
    }
}

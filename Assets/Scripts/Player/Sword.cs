using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Damage))]
public class Sword : MonoBehaviour
{
    private Damage dmg;
    void Start()
    {
        dmg = GetComponent<Damage>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(dmg.enemyTag))
        {
            Health h = other.GetComponent<Health>();
            if(h) h.Damage(gameObject, dmg);
        }
    }
}

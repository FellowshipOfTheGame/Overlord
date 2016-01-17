using UnityEngine;
using System.Collections;

public class Damage : MonoBehaviour {
    public int hitPointDamage = 1;
    public float pushForce = 10f;
    
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

	}

    void OnCollisionEnter2D(Collision2D col)
    {
        Health objectHealth = col.gameObject.GetComponent<Health>();
        if (objectHealth)
            objectHealth.Damage(gameObject, this);
    }
}

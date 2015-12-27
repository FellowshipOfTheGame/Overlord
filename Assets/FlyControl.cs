using UnityEngine;
using System.Collections;

public class FlyControl : MonoBehaviour {

    public float force = 100f;
    public float maxVelocity = 10f;

    private Rigidbody2D rigid;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector2 velocity = rigid.velocity;
        Vector2 f = new Vector2();
        //if (Mathf.Abs(velocity.x) < maxVelocity)
            f.x = h * force;
        //if (Mathf.Abs(velocity.y) < maxVelocity)
            f.y = v * force;

        rigid.AddForce(f);
	}
}

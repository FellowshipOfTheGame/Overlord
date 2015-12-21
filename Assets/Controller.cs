using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterAnimator))]
public class Controller : MonoBehaviour {
    CharacterAnimator anim;
    bool grounded = true;
	// Use this for initialization
	void Awake () {
        anim = GetComponent<CharacterAnimator>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        anim.SetVelocity(Input.GetAxisRaw("Horizontal"));
        if(Input.GetAxis("Vertical") > 0f && grounded)
        {
            grounded = false;
            anim.SetGrounded(grounded);
            StartCoroutine("Jump");
        }
	}

    IEnumerator Jump()
    {
        yield return new WaitForSeconds(1f);
        grounded = true;
        anim.SetGrounded(grounded);
    }
}

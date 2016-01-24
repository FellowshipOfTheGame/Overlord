using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof(PlatformerMovement))]
[RequireComponent(typeof(CharacterAnimator))]
public class CharacterController : MonoBehaviour {

	private bool jumpPressed;
	private bool grabbing;
	private PlatformerMovement character;
    private CharacterAnimator anim;
    private Rigidbody2D rigid;

	// Na inicializacao, pega o script com as funcoes de movimento
	private void Start () {
		character = GetComponent<PlatformerMovement>();
        anim = GetComponent<CharacterAnimator>();
        rigid = GetComponent<Rigidbody2D>();
	}
	
	private void Update () {
		// Le o input do botao de pulo de maneira que pegue apenas uma vez a cada pressionamento do botao
		if(!jumpPressed) jumpPressed = CrossPlatformInputManager.GetButtonDown ("Jump");
		// Analisa se o botao left ctrl esta sendo pressionado
		grabbing = Input.GetKey (KeyCode.LeftControl);
        if (CrossPlatformInputManager.GetButtonDown("Fire1"))
            anim.Attack();
	}

	private void FixedUpdate(){
		// Le o input de movimento
		float xAxis = CrossPlatformInputManager.GetAxis ("Horizontal");
		// Chama a funcao de movimento
		character.Move (xAxis, jumpPressed, grabbing);
		// Muda jumpPressed para false para que o input seja analisado novamente
		jumpPressed = false;

        anim.SetVelocity(rigid.velocity.sqrMagnitude);
        anim.SetGrounded(character.grounded);
        anim.SetGrabbing(character.grabbing);
	}
}

using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof(PlatformerMovement))]
public class CharacterController : MonoBehaviour {

	private bool jumpPressed;
	public bool grabbing;
	private PlatformerMovement character;

	// Na inicializacao, pega o script com as funcoes de movimento
	private void Start () {
		character = GetComponent<PlatformerMovement>();
	}
	
	private void Update () {
		// Le o input do botao de pulo de maneira que pegue apenas uma vez a cada pressionamento do botao
		if(!jumpPressed) jumpPressed = CrossPlatformInputManager.GetButtonDown ("Jump");
		// Analisa se o botao left ctrl esta sendo pressionado
		grabbing = Input.GetKey (KeyCode.LeftControl);
	}

	private void FixedUpdate(){
		// Le o input de movimento
		float xAxis = CrossPlatformInputManager.GetAxis ("Horizontal");
		// Chama a funcao de movimento
		character.Move(xAxis, jumpPressed, grabbing);
		// Muda jumpPressed para false para que o input seja analisado novamente
		jumpPressed = false;
	}
}

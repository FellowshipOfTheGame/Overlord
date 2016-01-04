﻿using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof(my_platformer_movement))]
public class my_character_controller : MonoBehaviour {

	private bool jumpPressed;
	public bool grabbing;
	private my_platformer_movement character;

	// Na inicializacao, pega o script com as funcoes de movimento
	private void Start () {
		character = GetComponent<my_platformer_movement>();
	}
	
	private void Update () {
		// Le o input do botao de pulo de maneira que pegue apenas uma vez a cada pressionamento do botao
		if(!jumpPressed) jumpPressed = CrossPlatformInputManager.GetButtonDown ("Jump");
		// Analisa se o botao left ctrl esta sendo pressionado
		grabbing = Input.GetKey (KeyCode.LeftControl);
	}

	private void FixedUpdate(){
		// Le o input de movimento
		float x_axis = CrossPlatformInputManager.GetAxis ("Horizontal");
		// Chama a funcao de movimento
		character.move (x_axis, jumpPressed, grabbing);
		// Muda jumpPressed para false para que o input seja analisado novamente
		jumpPressed = false;
	}
}

using UnityEngine;
using System.Collections;

public class SimpleEnemyBehaviour : MonoBehaviour {

	private Rigidbody2D enemyRigidBody;
	private Transform lookAhead;
	private bool facingRight = true;
	public float walkSpeed = 10;
	private float direction = 1;

	// E necessario um rigidbody2d como componente e um filho indicando onde ele deve olhar para saber se e possivel ir em frente
	void Awake () {
		enemyRigidBody = GetComponent<Rigidbody2D> ();
		lookAhead = transform.Find ("lookAhead");
	}

	void Flip(){
		// Faz com que os filhos mudem de lado em relaçao ao eixo x
		Vector3 aux = transform.localScale;
		aux.x *= -1;
		transform.localScale = aux;
		// altera o indicador booleano de direçao e o modificador de velocidade
		facingRight = (facingRight)? false : true;
		direction = (facingRight) ? 1f : -1f;
	}

	void Update () {
		// Caso nao veja mais chao a sua frente, vira para o lado oposto
		if (Physics2D.OverlapCircle (lookAhead.position, 0.1f, (1 << LayerMask.NameToLayer ("Ground")) | (1 << LayerMask.NameToLayer ("Object"))) == false) {
			Flip ();
		}
	}

	// Mantem-se em movimento
	void FixedUpdate(){
		enemyRigidBody.velocity = new Vector2 (direction*walkSpeed, 0);
	}
}

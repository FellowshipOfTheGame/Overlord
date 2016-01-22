using UnityEngine;
using System.Collections;

public class SimpleEnemyBehaviour : MonoBehaviour {

	protected Rigidbody2D enemyRigidBody;
	protected Transform lookAhead;
	protected bool facingRight = true;
	public float walkSpeed = 10;
	protected float direction = 1;
	
	// E necessario um rigidbody2d como componente e um filho indicando onde ele deve olhar para saber se e possivel ir em frente
	protected void GetComponents(){
		enemyRigidBody = GetComponent<Rigidbody2D> ();
		lookAhead = transform.Find ("lookAhead");

	}

	void Awake () {
		// Inicializa os componentes necessarios para o movimento basico
		GetComponents ();
	}

	public void Flip(){
		// Faz com que os filhos mudem de lado em relaçao ao eixo x
		Vector3 aux = transform.localScale;
		aux.x *= -1;
		transform.localScale = aux;
		// altera o indicador booleano de direçao e o modificador de velocidade
		facingRight = (facingRight)? false : true;
		direction = (facingRight) ? 1f : -1f;
	}

	// Analisa se ha uma plataforma a frente
	public bool CheckAhead(){
		return Physics2D.OverlapCircle (lookAhead.position, 0.1f, (1 << LayerMask.NameToLayer ("Ground")) | (1 << LayerMask.NameToLayer ("Object")));
	}

	// Comportamento padrao, deve ser chamada dentro de update
	protected void StandardUpdate(){
		// Caso nao veja mais chao a sua frente, vira para o lado oposto
		if (!CheckAhead())
			Flip ();
	}

	void Update () {
		StandardUpdate ();
	}

	// Mantem-se em movimento
	void FixedUpdate(){
		enemyRigidBody.velocity = new Vector2 (direction*walkSpeed, 0);
	}
}

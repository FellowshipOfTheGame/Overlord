using UnityEngine;
using System.Collections;

public class followPlayer : SimpleEnemyBehaviour {
	
	//private Rigidbody2D enemyRigidBody;
	//private Transform lookAhead;
	public float detectionRange = 20, attackRange = 5;
	private GameObject player;
	private Rigidbody2D playerRigidBody;
	private float distance;

	void Awake () {
		// Inicializa os componentes necessarios para os movimentos basicos
		GetComponents ();
		// Procura o gameObject correspondente ao player
		player = GameObject.FindGameObjectWithTag("Player");
		// E obtem o seu rigidbody
		playerRigidBody = player.GetComponent<Rigidbody2D> ();
	}
	
	void Update () {
		// calcula a distancia entre o jogador e o inimigo
		distance = Vector2.Distance (playerRigidBody.position, enemyRigidBody.position);

		// Se o jogador estiver dentro do raio de detecao do inimigo
		if (distance < detectionRange) {
			// Define para que lado o jogador esta
			direction = (player.transform.position.x < transform.position.x) ? -1f : 1f;
			// Caso necessario, vira o inimigo
			if(facingRight ^ (direction > 0))
				Flip ();
			// Caso o inimigo nao possa ir em frente, zera a velocidade que ele deve receber
			if(!CheckAhead())
				direction = 0;
			// Caso o jogador esteja dentro do raio de ataque, para de se mover e ataca
			if(distance < attackRange){
				direction = 0;
				// attack
			}
		// Se o jogador estiver fora do raio de deteccao, mantem o comportamento padrao
		} else {
			direction = facingRight ? 1f : -1f;
			StandardUpdate();
		}
	}
	
	// Mantem-se em movimento
	void FixedUpdate(){
		enemyRigidBody.velocity = new Vector2 (direction*walkSpeed, 0);
	}
}
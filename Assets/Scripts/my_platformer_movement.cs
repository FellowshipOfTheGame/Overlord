using UnityEngine;
using System;

public class my_platformer_movement : MonoBehaviour {
	public float maxMoveSpeed = 15f;
	public float maxFallingSpeed = 80f;
	public float jump_speed = 15f;
	private bool facingRight;
	public bool hitHead, grounded;
	// Nao usei mas ta ae
	private Animator animator;
	private Transform topHead;
	private Transform feet;
	private Rigidbody2D playerRigidBody;

	void Awake () {
		// Pega os componentes necessarios do player
		animator = GetComponent<Animator>();
		playerRigidBody = GetComponent<Rigidbody2D> ();
		facingRight = true;
		// Deve ser indicada a posicao do topo da cabeca e da sola dos pes
		topHead = transform.Find ("head");
		feet = transform.Find ("feet");
	}
	
	void raycasting(){
		// Desenha duas linhas saindo da cabeça e dos pes
		Debug.DrawLine (topHead.position, new Vector2 (topHead.position.x, topHead.position.y + 0.1f), Color.green);
		Debug.DrawLine (feet.position, new Vector2 (feet.position.x, feet.position.y - 0.1f), Color.green);
		// Se essas linhas colidirem com uma layer denominada ground (ou ainda Platform no caso dos pes) armazena numa variavel que houve colisao
		hitHead = Physics2D.Raycast(topHead.position, Vector2.down, 0.1f, 1 << LayerMask.NameToLayer ("Ground"));
		grounded = Physics2D.Raycast(feet.position, Vector2.down, 0.1f, 1 << LayerMask.NameToLayer ("Ground"));
		if(!grounded)
			grounded = Physics2D.Raycast (feet.position, Vector2.down, 0.1f, 1 << LayerMask.NameToLayer ("Platform"));
	}

	// Chama a funcao de raycasting acima
	void Update () {
		raycasting ();
	}

	// Funcao para realizar o giro
	void Flip(){
		// Alguem me explica pq exatamente isso e necessario pls (tava no script da unity)
		Vector3 aux = transform.localScale;
		aux.x *= -1;
		transform.localScale = aux;
		// Inverte a variavel
		facingRight = (facingRight) ? false : true;
	}

	// A variavel xMove armazena a direcao pressionada no eixo x
	public void move(float xMove, bool jump){
		// Caso nenhuma direcao esteja sendo pressionada
		if (xMove == 0) {
			// Se o jogador estiver no chao
			if(grounded)
				// Para completamente
				playerRigidBody.velocity = new Vector2(0, 0);
			// Caso contrario
			else
				// Zera apenas a velocidade no eixo x
				playerRigidBody.velocity = new Vector2(0, playerRigidBody.velocity.y);
		} else {
			// Caso o jogador tente se mover para um lado oposto ao qual esta atualmente virado, realiza o giro
			if(!((xMove > 0 && facingRight) || (xMove < 0 && !facingRight)))
				Flip();

			// A variavel modifier armazena um modificador, para que no ar a velocidade do jogador seja a metade da padrao
			float modifier = (grounded)? 1f : 0.5f;
			// Caso seja pressionado para movimentar-se a direita
			if(xMove > 0){
				// Altera a velocidade com valor positivo
				playerRigidBody.velocity = new Vector2(maxMoveSpeed*modifier, playerRigidBody.velocity.y);
			// E caso seja pressionado para movimentar-se a esquerda
			}else{
				// Altera a velocidade com valor negativo
				playerRigidBody.velocity = new Vector2(maxMoveSpeed*(-1f)*modifier, playerRigidBody.velocity.y);
			}
		}

		// Se for pressionado para pular e o jogador estiver no chao
		if(jump && grounded){
			// Muda a velocidade para a velocidade de pulo
			playerRigidBody.velocity = new Vector2(playerRigidBody.velocity.x, jump_speed);
		}

		// Se a velocidade for maior que a velocidade maxima de caida
		if(playerRigidBody.velocity.y < (-1)*maxFallingSpeed){
			// Mantem a velocidade maxima
			playerRigidBody.velocity = new Vector2(playerRigidBody.velocity.x, (-1f)*maxFallingSpeed);
		}
	}
}

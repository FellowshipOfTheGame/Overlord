﻿using UnityEngine;
using System;

public class PlatformerMovement : MonoBehaviour {
	public float maxMoveSpeed = 15f;
	public float maxFallingSpeed = 80f;
	public float jumpSpeed = 15f;
	private bool facingRight;
	public bool hitHead, grounded, grabAvailable;
	private Animator animator; // Nao usei mas ta ae
	private Transform topHead, feet;
	private Rigidbody2D playerRigidBody;
	private RaycastHit2D detectedObject;

	void Awake () {
		// Pega os componentes necessarios do player
		animator = GetComponent<Animator>();
		playerRigidBody = GetComponent<Rigidbody2D> ();
		facingRight = false;
		// Deve ser indicada a posicao do topo da cabeca e da sola dos pes
		topHead = transform.Find ("head");
		feet = transform.Find ("feet");
	}
	
	void Raycasting(){
		// Desenha tres linhas saindo da cabeça e tres dos pes
		Debug.DrawLine (topHead.position, new Vector2 (topHead.position.x, topHead.position.y + 0.1f), Color.green);
		Debug.DrawLine (new Vector2 (topHead.position.x-0.5f, topHead.position.y), new Vector2 (topHead.position.x-0.5f, topHead.position.y + 0.1f), Color.green);
		Debug.DrawLine (new Vector2 (topHead.position.x+0.5f, topHead.position.y), new Vector2 (topHead.position.x+0.5f, topHead.position.y + 0.1f), Color.green);
		Debug.DrawLine (feet.position, new Vector2 (feet.position.x, feet.position.y - 0.1f), Color.green);
		Debug.DrawLine (new Vector2 (feet.position.x-0.5f, feet.position.y), new Vector2 (feet.position.x-0.5f, feet.position.y - 0.1f), Color.green);
		Debug.DrawLine (new Vector2 (feet.position.x+0.5f, feet.position.y), new Vector2 (feet.position.x+0.5f, feet.position.y - 0.1f), Color.green);
		// Se essas linhas colidirem com uma layer denominada ground (ou ainda Platform ou Object no caso dos pes) armazena numa variavel que houve colisao
		hitHead = Physics2D.Raycast(new Vector2(topHead.position.x-0.5f, topHead.position.y), Vector2.down, 0.1f, 1 << LayerMask.NameToLayer ("Ground"));
		if (!hitHead) hitHead = Physics2D.Raycast(topHead.position, Vector2.down, 0.1f, 1 << LayerMask.NameToLayer ("Ground"));
		if (!hitHead) hitHead = Physics2D.Raycast(new Vector2(topHead.position.x+0.5f, topHead.position.y), Vector2.down, 0.1f, 1 << LayerMask.NameToLayer ("Ground"));

		grounded = Physics2D.Raycast(new Vector2(feet.position.x-0.5f, feet.position.y), Vector2.down, 0.1f, (1 << LayerMask.NameToLayer ("Ground")) | (1 << LayerMask.NameToLayer("Object")) );
		if (!grounded) grounded = Physics2D.Raycast(feet.position, Vector2.down, 0.1f, (1 << LayerMask.NameToLayer ("Ground")) | (1 << LayerMask.NameToLayer("Object")) );
		if (!grounded) grounded = Physics2D.Raycast(new Vector2(feet.position.x+0.5f, feet.position.y), Vector2.down, 0.1f, (1 << LayerMask.NameToLayer ("Ground")) | (1 << LayerMask.NameToLayer("Object")) );
		// Analisa a posicao do player para saber de onde os raios deve ser criados
		Vector2 playerPosition = playerRigidBody.position;
		// Analisa se os raios devem ser criados pelo lado esquerdo ou direito
		playerPosition.x += (facingRight) ? 0.5f : (-1) * 0.5f;
		// Desenha as 3 linhas para facilitar debug
		Debug.DrawLine (new Vector2 (playerPosition.x, playerPosition.y + 0.5f), new Vector2 (playerPosition.x + (facingRight ? 0.5f : (-1)*0.5f), playerPosition.y + 0.5f), Color.green);
		Debug.DrawLine (new Vector2 (playerPosition.x, playerPosition.y), new Vector2 (playerPosition.x + (facingRight ? 0.5f : (-1)*0.5f), playerPosition.y), Color.green);
		Debug.DrawLine (new Vector2 (playerPosition.x, playerPosition.y - 0.5f), new Vector2 (playerPosition.x + (facingRight ? 0.5f : (-1)*0.5f), playerPosition.y - 0.5f), Color.green);
		// Checa as tres linhas, de cima para baixo, ate que uma delas encontre um objeto
		playerPosition.y += 0.5f;
		grabAvailable = Physics2D.Raycast (playerPosition, (facingRight) ? Vector2.right : Vector2.left, 0.5f, 1 << LayerMask.NameToLayer ("Object"));
		if (!grabAvailable) {
			playerPosition.y -= 0.5f;
			grabAvailable = Physics2D.Raycast (playerPosition, (facingRight) ? Vector2.right : Vector2.left, 0.5f, 1 << LayerMask.NameToLayer ("Object"));
		}
		if (!grabAvailable) {
			playerPosition.y -= 0.5f;
			grabAvailable = Physics2D.Raycast (playerPosition, (facingRight) ? Vector2.right : Vector2.left, 0.5f, 1 << LayerMask.NameToLayer ("Object"));
		}

		// Caso tenha detectado colisao com um objeto, armazena numa variavel qual o objeto com o qual houve a colisao
		if(grabAvailable)
			detectedObject = Physics2D.Raycast (playerPosition, (facingRight) ? Vector2.right : Vector2.left, 0.5f, 1 << LayerMask.NameToLayer ("Object"));
	}

	// Chama a funcao de raycasting acima
	void Update () {
        Raycasting();
	}

	// Funcao para realizar o giro
	void Flip(bool grab){
		// Faz com que os objetos ligados ao player mudem de lado em relaçao ao eixo x
		Vector3 aux = transform.localScale;
		aux.x *= -1;
		transform.localScale = aux;
		// Caso o jogador esteja carregando um objeto, muda o objeto de lado
		if (grab && grabAvailable) {
			// Armazena a posicao do objeto numa variavel (o valor x nao pode ser acessado diretamente)
			Vector2 objectPosition = detectedObject.transform.position;
			// Calcula a diferenca no eixo x
			float difference = objectPosition.x - this.transform.position.x;
			// Obtem qual deve ser a nova posicao
			objectPosition.x -= 2*difference;
			// altera a posicao do objeto
			detectedObject.transform.position = objectPosition;
		}
		// Inverte a variavel
		facingRight = (facingRight) ? false : true;
	}

	// A variavel xMove armazena a direcao pressionada no eixo x
	public void Move(float xMove, bool jump, bool grab){
		// A variavel grabspeed altera a valocidade de movimento do personagem caso ele esteja segurando um objeto
		float grabSpeed = (grab && grabAvailable) ? 0.5f : 1f;
		// Caso nenhuma direcao esteja sendo pressionada
		if (xMove == 0) {
			// Se o jogador estiver no chao
			if (grounded)
				// Para completamente
				playerRigidBody.velocity = new Vector2 (0, 0);
			// Caso contrario
			else
				// Zera apenas a velocidade no eixo x
				playerRigidBody.velocity = new Vector2 (0, playerRigidBody.velocity.y);
		} else {
			// Caso o jogador tente se mover para um lado oposto ao qual esta atualmente virado, realiza o giro
			if (!((xMove > 0 && facingRight) || (xMove < 0 && !facingRight)))
				Flip (grab);

			// A variavel airspeed armazena um modificador, para que no ar a velocidade do jogador seja tres quartos da padrao
			float airSpeed = (grounded) ? 1f : 0.75f;
			// Caso seja pressionado para movimentar-se a direita
			if (xMove > 0) {
				// Altera a velocidade com valor positivo
				playerRigidBody.velocity = new Vector2 (maxMoveSpeed * airSpeed * grabSpeed, playerRigidBody.velocity.y);
				// E caso seja pressionado para movimentar-se a esquerda
			} else {
				// Altera a velocidade com valor negativo
				playerRigidBody.velocity = new Vector2 ((-1f) * maxMoveSpeed * airSpeed * grabSpeed, playerRigidBody.velocity.y);
			}
		}

		// Se for pressionado para pular e o jogador estiver no chao
		if (jump && grounded) {
			// Muda a velocidade para a velocidade de pulo
			playerRigidBody.velocity = new Vector2 (playerRigidBody.velocity.x, jumpSpeed);
		}

		// Se a velocidade for maior que a velocidade maxima de caida
		if (playerRigidBody.velocity.y < (-1) * maxFallingSpeed) {
			// Mantem a velocidade maxima
			playerRigidBody.velocity = new Vector2 (playerRigidBody.velocity.x, (-1f) * maxFallingSpeed);
		}

		// Altera a posiçao do objeto sendo segurado para ficar a frente do player
		if (grab && grabAvailable) {
			Vector2 grabbedPosition = playerRigidBody.position;
			grabbedPosition.x += (facingRight) ? 1.2f : -1.2f;
			grabbedPosition.y += 0.1f;
			detectedObject.rigidbody.position = grabbedPosition;
			// altera a velocidade do objeto para evitar que "escorregue" do jogador
			detectedObject.rigidbody.velocity = new Vector2(0, playerRigidBody.velocity.y);
		}
	}
}

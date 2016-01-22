using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody2D))]
public class PlatformerMovement : MonoBehaviour {
	public float maxMoveSpeed = 15f;
	public float maxFallingSpeed = 80f;
	public float jumpSpeed = 15f;
	private bool facingRight;
	public bool hitHead, grounded, grabAvailable, grabbing;
	private Transform head, feet;
	private Rigidbody2D playerRigidBody;
	private RaycastHit2D detectedObject;

	void Awake () {
		// Pega os componentes necessarios do player
		playerRigidBody = GetComponent<Rigidbody2D> ();
		facingRight = true;
		// Deve ser indicada a posicao do topo da cabeca e da sola dos pes
		head = transform.Find ("head");
		feet = transform.Find ("feet");
	}

	void OnDrawGizmos()
    {
        head = transform.Find("head");
        feet = transform.Find("feet");
        playerRigidBody = GetComponent<Rigidbody2D>();

        Vector2 rayBegin = new Vector2(head.position.x, head.position.y);
        Vector2 rayEnd = new Vector2(head.position.x, head.position.y);
        Gizmos.color = Color.green;

        rayEnd.y += headYOffset;
        Gizmos.DrawLine(rayBegin, rayEnd);


        rayBegin.x -= headXOffset;
        rayEnd.x -= headXOffset;
        Gizmos.DrawLine(rayBegin, rayEnd);

        rayBegin.x += 2 * headXOffset;
        rayEnd.x += 2 * headXOffset;
        Gizmos.DrawLine(rayBegin, rayEnd);

        rayBegin = new Vector2(feet.position.x, feet.position.y);
        rayEnd = new Vector2(feet.position.x, feet.position.y);


        rayEnd.y -= feetYOffset;
        Gizmos.DrawLine(rayBegin, rayEnd);


        rayBegin.x -= feetXOffset;
        rayEnd.x -= feetXOffset;
        Gizmos.DrawLine(rayBegin, rayEnd);

        rayBegin.x += 2 * feetXOffset;
        rayEnd.x += 2 * feetXOffset;
        Gizmos.DrawLine(rayBegin, rayEnd);


        rayBegin = new Vector2(playerRigidBody.position.x, playerRigidBody.position.y);
        rayEnd = new Vector2(playerRigidBody.position.x, playerRigidBody.position.y);

        rayEnd.x += (facingRight ? grabXOffset : (-1) * grabXOffset);
        Gizmos.DrawLine(rayBegin, rayEnd);

        rayBegin.y -= grabYOffset;
        rayEnd.y -= grabYOffset;
        Gizmos.DrawLine(rayBegin, rayEnd);

        rayBegin.y -= 2 * grabYOffset;
        rayEnd.y -= 2 * grabYOffset;
        Gizmos.DrawLine(rayBegin, rayEnd);
    }

    public float headXOffset = 0.5f;
    public float headYOffset = 0.1f;
    public float feetXOffset = 0.5f;
    public float feetYOffset = 0.1f;
    public float grabXOffset = 0.5f;
    public float grabYOffset = 0.5f;

    void Raycast(){
        Vector2 rayCast = new Vector2(head.position.x, head.position.y);
        int layerMask = 1 << LayerMask.NameToLayer("Ground");


        // Se essas linhas colidirem com uma layer denominada ground (ou ainda Platform ou Object no caso dos pes) armazena numa variavel que houve colisao
        hitHead = Physics2D.Raycast(rayCast, Vector2.up, headYOffset, layerMask);

        rayCast.x -= headXOffset;
		if (!hitHead) hitHead = Physics2D.Raycast(rayCast, Vector2.up, headYOffset, layerMask);
        rayCast.x += 2 * headXOffset;
        if (!hitHead) hitHead = Physics2D.Raycast(rayCast, Vector2.up, headYOffset, layerMask);


        rayCast = new Vector2(feet.position.x, feet.position.y);

        grounded = Physics2D.Raycast(rayCast, Vector2.down, headYOffset, layerMask);

        rayCast.x -= feetXOffset;
        if (!grounded) grounded = Physics2D.Raycast(rayCast, Vector2.down, feetYOffset, layerMask);
        rayCast.x += 2 * feetXOffset;
        if (!grounded) grounded = Physics2D.Raycast(rayCast, Vector2.down, feetYOffset, layerMask);


        // Analisa a posicao do player para saber de onde os raios deve ser criados
        Vector2 playerPosition = playerRigidBody.position;
        layerMask = 1 << LayerMask.NameToLayer("Object");

        // Analisa se os raios devem ser criados pelo lado esquerdo ou direito
        playerPosition.x += (facingRight) ? grabXOffset : (-1) * grabXOffset;

		// Checa as tres linhas, de cima para baixo, ate que uma delas encontre um objeto
		grabAvailable = Physics2D.Raycast (playerPosition, (facingRight) ? Vector2.right : Vector2.left, grabXOffset, layerMask);

        playerPosition.y -= grabYOffset;
        if (!grabAvailable)
			grabAvailable = Physics2D.Raycast (playerPosition, (facingRight) ? Vector2.right : Vector2.left, grabXOffset, layerMask);

        playerPosition.y += 2 * grabYOffset;
        if (!grabAvailable)
			grabAvailable = Physics2D.Raycast (playerPosition, (facingRight) ? Vector2.right : Vector2.left, grabXOffset, layerMask);

		// Caso tenha detectado colisao com um objeto, armazena numa variavel qual o objeto com o qual houve a colisao
		if(grabAvailable)
			detectedObject = Physics2D.Raycast (playerPosition, (facingRight) ? Vector2.right : Vector2.left, 0.5f, layerMask);
	}

	// Chama a funcao de raycasting acima
	void Update () {
        Raycast();
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
			objectPosition.x -= 2 * difference;
			// altera a posicao do objeto
			detectedObject.transform.position = objectPosition;
		}
		// Inverte a variavel
		facingRight = !facingRight;
	}

    public float grabSpeedModifier = 0.75f;
    public float groundSpeedModifier = 0.75f;
    public Vector2 grabbedObjectDistance = new Vector2(1.2f, 0.1f);

	// A variavel xMove armazena a direcao pressionada no eixo x
	public void Move(float xMove, bool jump, bool grab){
		// A variavel grabspeed altera a valocidade de movimento do personagem caso ele esteja segurando um objeto
		float speedModifier = (grab && grabAvailable) ? grabSpeedModifier : 1f;
		// Caso nenhuma direcao esteja sendo pressionada
		if (xMove == 0) {
			// Se o jogador estiver no chao
			if (grounded)
				// Para completamente
				playerRigidBody.velocity = Vector2.zero;
			// Caso contrario
			else
				// Zera apenas a velocidade no eixo x
				playerRigidBody.velocity = new Vector2 (0, playerRigidBody.velocity.y);
		} else {
			// Caso o jogador tente se mover para um lado oposto ao qual esta atualmente virado, realiza o giro
			if (!((xMove > 0 && facingRight) || (xMove < 0 && !facingRight)))
				Flip (grab);

            // A variavel airspeed armazena um modificador, para que no ar a velocidade do jogador seja tres quartos da padrao
            speedModifier  *= (grounded) ? 1f : groundSpeedModifier;

			// Caso seja pressionado para movimentar-se a direita
			if (xMove > 0) {
				// Altera a velocidade com valor positivo
				playerRigidBody.velocity = new Vector2 (maxMoveSpeed * speedModifier, playerRigidBody.velocity.y);
				// E caso seja pressionado para movimentar-se a esquerda
			} else {
				// Altera a velocidade com valor negativo
				playerRigidBody.velocity = new Vector2 ((-1f) * maxMoveSpeed * speedModifier, playerRigidBody.velocity.y);
			}
		}

		// Se for pressionado para pular e o jogador estiver no chao
		if (jump && grounded) {
            // Muda a velocidade para a velocidade de pulo
            speedModifier = (grab && grabAvailable) ? grabSpeedModifier : 1f;
            playerRigidBody.velocity = new Vector2 (playerRigidBody.velocity.x, jumpSpeed * speedModifier);
		}

		// Se a velocidade for maior que a velocidade maxima de caida
		if (playerRigidBody.velocity.y < (-1) * maxFallingSpeed) {
			// Mantem a velocidade maxima
			playerRigidBody.velocity = new Vector2 (playerRigidBody.velocity.x, (-1f) * maxFallingSpeed);
		}

		// Altera a posiçao do objeto sendo segurado para ficar a frente do player
		if (grab && grabAvailable) {
			Vector2 grabbedPosition = playerRigidBody.position;
			grabbedPosition.x += (facingRight) ? grabbedObjectDistance.x : -grabbedObjectDistance.x;
			grabbedPosition.y += grabbedObjectDistance.y;
			detectedObject.rigidbody.position = grabbedPosition;
			// altera a velocidade do objeto para evitar que "escorregue" do jogador
			detectedObject.rigidbody.velocity = new Vector2(0, playerRigidBody.velocity.y);
		}

        grabbing = grab & grabAvailable;
	}
}

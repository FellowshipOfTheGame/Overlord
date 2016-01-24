using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class CameraFollow : MonoBehaviour
{
    public CharacterController player;
    public float velocity;
    public Vector2 velMultiplier = new Vector2(0.5f, 0.5f);

    private Camera cam;
    private float originalSize;
    private Transform transf;
    private Vector3 positionTarget;
    private float sizeTarget;

    private Transform playerTransform;
    private Rigidbody2D playerRigid;


    // Use this for initialization
    void Start()
    {
        cam = GetComponent<Camera>();
        transf = this.transform;
        originalSize = cam.orthographicSize;
    }
    
    void Update()
    {
        if (playerRigid == null)
        {
            playerRigid = player.GetComponent<Rigidbody2D>();
            playerTransform = player.transform;
            transf.position = playerRigid.position;
            return;
        }
        positionTarget = playerRigid.position;
        positionTarget.z = -1f;
        if (!Mathf.Approximately(playerRigid.velocity.x, 0f))
            positionTarget.x += playerRigid.velocity.x * velMultiplier.x;

        if (playerRigid.velocity.y > 0f)
            sizeTarget = originalSize + playerRigid.velocity.y * velMultiplier.y;
        else
            sizeTarget = originalSize;


        transf.position = Vector3.Lerp(transf.position, positionTarget, velocity);
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, sizeTarget, velocity);
    }
}

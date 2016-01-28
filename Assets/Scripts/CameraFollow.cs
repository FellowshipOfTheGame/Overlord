using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class CameraFollow : MonoBehaviour
{
    public CharacterController player;
    public float velocity;
    public float velMultiplier = 0.5f;
    public float airOrtographicSize = 10f;

    private Camera cam;
    private float originalSize;
    private Transform transf;
    private Vector3 positionTarget;
    private float sizeTarget;

    private Rigidbody2D playerRigid;
    private PlatformerMovement playerMov;


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
            transf.position = playerRigid.position;
            playerMov = player.GetComponent<PlatformerMovement>();
            return;
        }
        positionTarget = playerRigid.position;
        positionTarget.z = -1f;
        if (!Mathf.Approximately(playerRigid.velocity.x, 0f))
            positionTarget.x += playerRigid.velocity.x * velMultiplier;

        if (!playerMov.grounded)
            sizeTarget = airOrtographicSize;
        else
            sizeTarget = originalSize;


        transf.position = Vector3.Lerp(transf.position, positionTarget, velocity);
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, sizeTarget, velocity);
    }
}

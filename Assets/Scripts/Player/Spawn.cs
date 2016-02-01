using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CheckPoint))]
public class Spawn : MonoBehaviour
{
    public GameObject playerPrefab;
    public CameraFollow cameraScript;
    public HUD hud;

    void Start()
    {
        // Instatiate with player prefab
        GameObject player = (GameObject)Instantiate(playerPrefab, transform.position, Quaternion.identity);
        cameraScript.player = player.GetComponent<CharacterController>();
        // Set checkpoint
        Life life = player.GetComponent<Life>();
        Health health = player.GetComponent<Health>();

        if (life)
            life.lastCheckpoint = GetComponent<CheckPoint>();

        if(hud)
        {
            if (health)
                hud.playerHealth = health;
            if (life)
                hud.playerLife = life;
        }
    }
}

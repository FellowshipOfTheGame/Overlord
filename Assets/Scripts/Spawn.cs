using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CheckPoint))]
public class Spawn : MonoBehaviour
{
    public GameObject playerPrefab;
    public CameraFollow cameraScript;
    public Canvas hud;

    private HPInterface lifeUI;

    void Start()
    {
        // Instatiate with player prefab
        GameObject player = (GameObject)Instantiate(playerPrefab, transform.position, Quaternion.identity);
        cameraScript.player = player.GetComponent<CharacterController>();
        // Set checkpoint
        Life life = player.GetComponent<Life>();
        if (life)
            life.lastCheckpoint = GetComponent<CheckPoint>();
        if(hud)
        {
            lifeUI = hud.GetComponentInChildren<HPInterface>();
            Health health = player.GetComponent<Health>();
            if (lifeUI && health)
                lifeUI.playerHealth = health;
            if (lifeUI && life)
                lifeUI.playerLife = life;
        }
    }
}

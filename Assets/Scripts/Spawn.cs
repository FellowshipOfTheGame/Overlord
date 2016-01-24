using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CheckPoint))]
public class Spawn : MonoBehaviour
{
    public GameObject playerPrefab;
    public CameraFollow cameraScript;

    void Start()
    {
        // Instatiate with player prefab
        GameObject player = (GameObject)Instantiate(playerPrefab, transform.position, Quaternion.identity);
        cameraScript.player = player.GetComponent<CharacterController>();
        // Set checkpoint
        Life life = player.GetComponent<Life>();
        if (life)
            life.lastCheckpoint = GetComponent<CheckPoint>();
    }
}

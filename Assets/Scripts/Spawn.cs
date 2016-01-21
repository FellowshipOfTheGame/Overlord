using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CheckPoint))]
public class Spawn : MonoBehaviour
{
    public GameObject playerPrefab;

    void Start()
    {
        // Instatiate with player prefab
        Instantiate(playerPrefab, transform.position, Quaternion.identity);

        // Set checkpoint
        Life life = playerPrefab.GetComponent<Life>();
        if (life)
            life.lastCheckpoint = GetComponent<CheckPoint>();
    }
}

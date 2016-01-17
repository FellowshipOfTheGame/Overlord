using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CheckPoint))]
public class Spawn : MonoBehaviour
{
    public GameObject playerPrefab;

    void Start()
    {
        Instantiate(playerPrefab, transform.position, Quaternion.identity);
        Life life = playerPrefab.GetComponent<Life>();
        if (life)
            life.lastCheckpoint = GetComponent<CheckPoint>();
    }
}

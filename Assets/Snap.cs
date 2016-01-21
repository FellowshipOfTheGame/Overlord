using UnityEngine;
using System.Collections;

// Execute on Editor
[ExecuteInEditMode]
public class Snap : MonoBehaviour
{
    // Offset regardless of the snap
    public Vector3 offset = new Vector3(0f, 0f);

    void Update()
    {
        // Get renderer
        Renderer renderer = GetComponent<Renderer>();

        // Get position and renderer size
        Vector3 pos = transform.position;
        Vector3 size = renderer.bounds.size;

        // Snap position
        pos.x = Mathf.RoundToInt(pos.x / size.x) * size.x;
        pos.y = Mathf.RoundToInt(pos.y / size.y) * size.y;

        // Set position with offset
        transform.position = pos + offset;
    }
}

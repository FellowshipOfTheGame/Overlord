using UnityEngine;
using System.Collections;

// Execute on Editor
[ExecuteInEditMode]
public class Snap : MonoBehaviour
{
    // Offset regardless of the snap
    public Vector3 offset = new Vector3(0f, 0f);
    public bool useCustomSize = false;
    public Vector3 size = new Vector3(1f, 1f, 0f);

    void Update()
    {
        // Get renderer
        Renderer renderer = GetComponent<Renderer>();

        // Get position and renderer size
        Vector3 pos = transform.position;
        Vector3 snapSize = !useCustomSize ? renderer.bounds.size : size;

        // Snap position
        pos.x = Mathf.RoundToInt(pos.x / snapSize.x) * snapSize.x;
        pos.y = Mathf.RoundToInt(pos.y / snapSize.y) * snapSize.y;

        // Set position with offset
        transform.position = pos + offset;
    }
}

using UnityEngine;

public class ObjectSizeReporter : MonoBehaviour
{
    void Start()
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            Vector3 objectSize = renderer.bounds.size;
            Debug.Log($"Object size (X, Y, Z): {objectSize.x}, {objectSize.y}, {objectSize.z}");
        }
        else
        {
            Debug.LogWarning("No Renderer found on this GameObject.");
        }
    }
}
using UnityEngine;

public class Resource : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Called when a process accesses the resource
    public void OnResourceAccessed()
    {
        spriteRenderer.color = Color.red; // Indicate that the resource is being accessed
    }

    // Called when a process releases the resource
    public void OnResourceReleased()
    {
        spriteRenderer.color = Color.white; // Reset the resource color when released
    }
}

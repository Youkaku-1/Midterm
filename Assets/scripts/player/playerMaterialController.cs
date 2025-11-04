using UnityEngine;

public class PlayerMaterialController : MonoBehaviour
{
    [Header("Player Materials")]
    public Material redMaterial;
    public Material greenMaterial;
    public Material blueMaterial;

    private Material originalMaterial;
    private Renderer playerRenderer;

    void Start()
    {
        playerRenderer = GetComponent<Renderer>();
        if (playerRenderer != null)
        {
            originalMaterial = playerRenderer.material;
            ApplyMaterialBasedOnPlayerColor();
        }
    }

    private void ApplyMaterialBasedOnPlayerColor()
    {
        if (playerRenderer == null) return;

        Color playerColor = playerRenderer.material.color;
        string colorHex = ColorUtility.ToHtmlStringRGB(playerColor).ToLower();

        switch (colorHex)
        {
            case "ff0000": // Red
                if (redMaterial != null)
                    playerRenderer.material = redMaterial;
                break;
            case "00ff00": // Green
                if (greenMaterial != null)
                    playerRenderer.material = greenMaterial;
                break;
            case "0000ff": // Blue
                if (blueMaterial != null)
                    playerRenderer.material = blueMaterial;
                break;
            case "ffffff": // White - keep original
                break;
        }
    }
}
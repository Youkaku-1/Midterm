using UnityEngine;

[CreateAssetMenu(fileName = "playerColor", menuName = "Player/Color")]
public class playerColorStorage : ScriptableObject
{

    public string hexValue = "#FFFFFF";

    public Color GetColor()
    {
        Color color;
        if (ColorUtility.TryParseHtmlString(hexValue, out color))
        {
            return color;
        }
        else
        {
            Debug.LogWarning($"Invalid hex value: {hexValue}. Defaulting to white.");
            return Color.white;
        }
    }
}

using UnityEngine;


public class playerColorSet : MonoBehaviour
{
    public playerColorStorage colorData;

    void Awake()
    {
        GetComponent<Renderer>().material.color = colorData.GetColor();
        Debug.Log("color changed");
    }
}
    
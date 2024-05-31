using UnityEngine;

public class ExampleColorScript : MonoBehaviour
{
    public void SetColor(Color color)
    {
        GetComponent<Renderer>().material.color = color;
    }
    public Color GetColor()
    {
        return GetComponent<Renderer>().material.color;
    }
}

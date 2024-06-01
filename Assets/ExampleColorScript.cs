<<<<<<< HEAD
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
=======
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
>>>>>>> 12fa110b21928b31eb4877f8d5b5df0b12480307

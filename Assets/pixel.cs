<<<<<<< HEAD
using UnityEngine;

public class Pixel : MonoBehaviour
{    
    public void Setup(Vector3 vector, Color color)
    {
        Vector3 = vector;
        Color = color;
        RendererPixel = GetComponent<Renderer>();
        Draw(vector, color);
    }
    public Vector3 Vector3 { get; set; }
    public Color Color { get; set; }
    public Renderer RendererPixel { get; set; }

    private void Draw(Vector3 vector_, Color color)
    {  
        transform.position = vector_;

        if (RendererPixel != null) RendererPixel.material.color = color;
        else Debug.LogError("No Renderer found on this GameObject");
    }

    public void Clear()
    {        
        gameObject.SetActive(false);
    }
}
=======
using UnityEngine;

public class Pixel : MonoBehaviour
{    
    public void Setup(Vector3 vector, Color color)
    {
        Vector3 = vector;
        Color = color;
        RendererPixel = GetComponent<Renderer>();
        Draw(vector, color);
    }
    public Vector3 Vector3 { get; set; }
    public Color Color { get; set; }
    public Renderer RendererPixel { get; set; }

    private void Draw(Vector3 vector_, Color color)
    {  
        transform.position = vector_;

        if (RendererPixel != null) RendererPixel.material.color = color;
        else Debug.LogError("No Renderer found on this GameObject");
    }

    public void Clear()
    {        
        gameObject.SetActive(false);
    }
}
>>>>>>> 12fa110b21928b31eb4877f8d5b5df0b12480307

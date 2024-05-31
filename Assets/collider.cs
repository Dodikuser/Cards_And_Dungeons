using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collider : MonoBehaviour
{
    public Card MyCard;

    public void Setup(Vector3 vector)
    {
        Vector3 = vector;       
        RendererPixel = GetComponent<Renderer>();
        GoToPosition(vector);
    }
    public Vector3 Vector3 { get; set; }   
    public Renderer RendererPixel { get; set; }

    private void GoToPosition(Vector3 vector_)
    {
        transform.position = vector_;             
    }  
}

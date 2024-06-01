<<<<<<< HEAD
using System;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    public GameObject MyObject { get; set; }
    public Vector3 startPosition { get; set; }
    public Vector3 endPosition { get; set; }
    public Quaternion startRotation { get; set; }
    public Quaternion endRotation { get; set; }
    public bool ChangeAngle = true; 
    public float duration { get; set; }
    private float currentTime = 0.0f;
    private int randIndex;

    public bool animationNow = true;
    public bool TextMod = false;

    public Func<float, float, float> calculationFormula;

    public enum mods
    {
        NormalSharp,
        NormalSmooth
    }
    public mods Mod = mods.NormalSmooth;
    
    public void Initialize(GameObject Object, Vector3 endVector, Quaternion endRot, float _duration)
    {
        MyObject = Object;
        startPosition = MyObject.transform.position;
        startRotation = MyObject.transform.rotation;
        endPosition = endVector;
        endRotation = endRot;
        duration = _duration;

    }

    public void StartInterpolation()
    {
        if (TextMod) PixelSpawner.StackAnime.Add(this);
        currentTime = 0f;
        randIndex = PixelSpawner.random.Next(1, (Mod == mods.NormalSmooth) ? 100 : 1000);
        startPosition = MyObject.transform.position;
        startRotation = MyObject.transform.rotation;
        if(Mod == mods.NormalSmooth) duration += (float)randIndex / 100f;
    }

    void Update()
    {
        if (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            if (currentTime > duration)
                currentTime = duration;

            float t = 0;
            switch (Mod)
            {
                case mods.NormalSmooth:
                    t = Mathf.Sqrt(currentTime / duration);
                    break;
                case mods.NormalSharp:
                    t = Mathf.Pow(currentTime / duration * 1000 / randIndex, 1 + 1 / randIndex);
                    break;
            }

            MyObject.transform.position = Vector3.Lerp(startPosition, endPosition, t);
            if (ChangeAngle) MyObject.transform.rotation = Quaternion.Slerp(startRotation, endRotation, t);
        }
        else if (!animationNow)
        {
            if (TextMod) PixelSpawner.StackAnime.Remove(this);
            enabled = false;
        }
    }
}
=======
using System;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    public GameObject MyObject { get; set; }
    public Vector3 startPosition { get; set; }
    public Vector3 endPosition { get; set; }
    public Quaternion startRotation { get; set; }
    public Quaternion endRotation { get; set; }
    public bool ChangeAngle = true; 
    public float duration { get; set; }
    private float currentTime = 0.0f;
    private int randIndex;

    public bool animationNow = true;
    public bool TextMod = false;

    public Func<float, float, float> calculationFormula;

    public enum mods
    {
        NormalSharp,
        NormalSmooth
    }
    public mods Mod = mods.NormalSmooth;
    
    public void Initialize(GameObject Object, Vector3 endVector, Quaternion endRot, float _duration)
    {
        MyObject = Object;
        startPosition = MyObject.transform.position;
        startRotation = MyObject.transform.rotation;
        endPosition = endVector;
        endRotation = endRot;
        duration = _duration;

    }

    public void StartInterpolation()
    {
        if (TextMod) PixelSpawner.StackAnime.Add(this);
        currentTime = 0f;
        randIndex = PixelSpawner.random.Next(1, (Mod == mods.NormalSmooth) ? 100 : 1000);
        startPosition = MyObject.transform.position;
        startRotation = MyObject.transform.rotation;
        if(Mod == mods.NormalSmooth) duration += (float)randIndex / 100f;
    }

    void Update()
    {
        if (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            if (currentTime > duration)
                currentTime = duration;

            float t = 0;
            switch (Mod)
            {
                case mods.NormalSmooth:
                    t = Mathf.Sqrt(currentTime / duration);
                    break;
                case mods.NormalSharp:
                    t = Mathf.Pow(currentTime / duration * 1000 / randIndex, 1 + 1 / randIndex);
                    break;
            }

            MyObject.transform.position = Vector3.Lerp(startPosition, endPosition, t);
            if (ChangeAngle) MyObject.transform.rotation = Quaternion.Slerp(startRotation, endRotation, t);
        }
        else if (!animationNow)
        {
            if (TextMod) PixelSpawner.StackAnime.Remove(this);
            enabled = false;
        }
    }
}
>>>>>>> 12fa110b21928b31eb4877f8d5b5df0b12480307

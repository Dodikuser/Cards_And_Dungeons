using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AnimationManager;
using static Card;
using static UnityEngine.UI.Image;

public class AnimationScript : MonoBehaviour
{
    public Card Card { get; set; }

    public bool startAnime = false;
    public float rotate = 0f;
    private Dictionary<GameObject, (Vector3, Quaternion)> StartInfo = new Dictionary<GameObject, (Vector3, Quaternion)>();
    public GameObject CardObject;
    public mods MyMod = mods.NormalSmooth;
    public float MyDuration = 0.5f;

    void Update()
    {
        if (startAnime) SinglMove(MyDuration);
    }
    public void SetStartInfo()
    {
        foreach (Transform childTransform in CardObject.transform)
        {
            GameObject child = childTransform.gameObject;
            //StartInfo.Add(child, (child.transform.localPosition, child.transform.localRotation));
            StartInfo[child] = (child.transform.localPosition, child.transform.localRotation);
        }
    }
    public void SinglMove(float duration)
    {
        Transform centerPoint = CardObject.transform.Find("CenterPoint");
        if (rotate != 0f) centerPoint.transform.Rotate(0, rotate, 0);
        foreach (Transform childTransform in CardObject.transform)
        {
            if (childTransform.name != "CenterPoint")
            {
                GameObject child = childTransform.gameObject;
                AnimationManager anime = child.GetComponent<AnimationManager>();
                if (anime == null) anime = child.AddComponent<AnimationManager>();
                anime.Mod = MyMod;
                Vector3 localPos = StartInfo[child].Item1;
                Quaternion localRot = StartInfo[child].Item2;

                Vector3 newPosition = centerPoint.TransformPoint(localPos);
                Quaternion newRotation = centerPoint.rotation * localRot;

                anime.Initialize(child, newPosition, newRotation, duration);
                anime.StartInterpolation();
            }
        }
    }
    public void SinglMove(GameObject child, Vector3 newPosition, Quaternion newRotation, float duration)
    {
        AnimationManager anime = child.GetComponent<AnimationManager>();
        if (anime == null) anime = child.AddComponent<AnimationManager>();
        anime.ChangeAngle = false;

        anime.Initialize(child, newPosition, newRotation, duration);
        anime.StartInterpolation();
    }
    public IEnumerator PerformActionsWithDelays()
    {
        StartAnimator();
        Transform centerPoint = CardObject.transform.Find("CenterPoint");

        //centerPoint.position = new Vector3(centerPoint.position.x, centerPoint.position.y, centerPoint.position.z - 25);
        SinglMove(centerPoint.gameObject, new Vector3(centerPoint.position.x, centerPoint.position.y, -25), Quaternion.identity, 0.3f);
        AnimationManager anime = centerPoint.gameObject.GetComponent<AnimationManager>();
        MyMod = mods.NormalSharp;
        startAnime = true;
        rotate = 6f;

        yield return StartCoroutine(WaitTicks(70));
        rotate = 20f;
        MyMod = mods.NormalSmooth;
        //Card.PixelManager.SelectTheLine(PixelSpawner.OnlyCards[PixelSpawner.OnlyCards.Count - 1].Ymain);
        yield return StartCoroutine(WaitTicks(26));
        int originY = PixelSpawner.OnlyCards[PixelSpawner.OnlyCards.Count - 1].Ymain;
        if (Card.Ymain == originY && PixelSpawner.nowLocation == PixelSpawner.location.Chest)
        {
            Card.Enabled = true;
            Card.borderCondition = condition.InChest;
            Card.DrawBorder(Color.green, null, false);
        }
        if (Card == PixelSpawner.selectedCard && PixelSpawner.nowLocation == PixelSpawner.location.Dungon) Card.DrawBorder(Color.yellow);
        yield return StartCoroutine(WaitTicks(26));
        rotate = 0f;
        centerPoint.rotation = Quaternion.identity;
        startAnime = false;
        yield return StartCoroutine(WaitTicks(100));
        centerPoint.position = new Vector3(centerPoint.position.x, centerPoint.position.y, -1);
        PixelSpawner.ChangeParentPosition(centerPoint.transform.parent.gameObject, centerPoint.transform.position);
        SinglMove(0.5f);
        yield return StartCoroutine(WaitTicks(150));
        StopAnimator();
    }
    public IEnumerator LeavePerformActionsWithDelays()
    {
        StartAnimator();
        Transform centerPoint = CardObject.transform.Find("CenterPoint");
        //centerPoint.position = new Vector3(centerPoint.position.x, centerPoint.position.y, -100);
        SinglMove(centerPoint.gameObject, new Vector3(centerPoint.position.x, centerPoint.position.y, -100), Quaternion.identity, 1f);
        MyMod = mods.NormalSharp;
        startAnime = true;
        rotate = 4f;
        //SinglMove(5f);
        yield return StartCoroutine(WaitTicks(100));
        StopAnimator();
    }
    public IEnumerator Leave()
    {
        StartAnimator();
        Transform centerPoint = CardObject.transform.Find("CenterPoint");
        centerPoint.position = new Vector3(centerPoint.position.x, centerPoint.position.y, -7);
        SinglMove(0.01f);
        yield return StartCoroutine(WaitTicks(30));

        MyMod = mods.NormalSharp;
        centerPoint.position = new Vector3(centerPoint.position.x, centerPoint.position.y, 1.2f);
        SinglMove(1f);
        //rotate = 4f;
        //SinglMove(5f);
        yield return StartCoroutine(WaitTicks(100));
        StopAnimator();
    }
    public IEnumerator ShowAnimation()
    {
        StartAnimator();
        Card.Enabled = false;
        Card.actionsForCard = actions.Visible;
        Card.DrawCard(true, null, false);
        foreach (Pixel pixel in Card.ImagePixlels)
        {
            pixel.transform.GetComponent<MeshRenderer>().enabled = false;
        }
        
        Card.CenterPoint.transform.rotation = new Quaternion(0, 180, 0, 0);
        Transform centerPoint = CardObject.transform.Find("CenterPoint");
        centerPoint.rotation = new Quaternion(0, 180, 0, 0);
        SinglMove(centerPoint.gameObject, new Vector3(centerPoint.position.x, centerPoint.position.y, -10), Quaternion.identity, 0.3f);
        startAnime = true;
        rotate = 12f;
        yield return StartCoroutine(WaitTicks(9));
        foreach (Pixel pixel in Card.ImagePixlels)
        {
            pixel.transform.GetComponent<MeshRenderer>().enabled = true;
        }
        yield return StartCoroutine(WaitTicks(9));
        rotate = 0f;
        centerPoint.rotation = Quaternion.identity;
        //yield return StartCoroutine(WaitTicks(4));
        //startAnime = false;
        //centerPoint.position = new Vector3(centerPoint.position.x, centerPoint.position.y, -1);
        //PixelSpawner.ChangeParentPosition(centerPoint.transform.parent.gameObject, centerPoint.transform.position);
        //SinglMove(0.5f);

        SinglMove(centerPoint.gameObject, new Vector3(centerPoint.position.x, centerPoint.position.y, -1), Quaternion.identity, 0.01f);
        yield return StartCoroutine(WaitTicks(50));
        
        Card.Enabled = true;
        startAnime = false;
        centerPoint.rotation = new Quaternion(0, 0, 0, 0);
        Card.MyAnimator.SetStartInfo();
        StopAnimator();
    }

    public IEnumerator AddToInventoryAnimation(Vector3 pisitionInInventory)
    {
        StartAnimator();
        Card.PixelManager.EnableControl = false;
        Card.VisibleRender();
        Transform centerPoint = CardObject.transform.Find("CenterPoint");
        MyMod = mods.NormalSmooth;
        //MyDuration = 0f;
        startAnime = true;
        SinglMove(centerPoint.gameObject, new Vector3(100, 50, -36), Quaternion.identity, 0.1f);
        yield return StartCoroutine(WaitTicks(50));
        startAnime = false;
        //SinglMove(centerPoint.gameObject, pisitionInInventory, Quaternion.identity, 0.1f);
        centerPoint.position = pisitionInInventory;
        SinglMove(0.1f);
        yield return StartCoroutine(WaitTicks(100));
        Card.PixelManager.EnableControl = true;
        StopAnimator();
    }
    public void ToCenterAnimation()
    {
        SinglMove(Card.CenterPoint, new Vector3(100, 50, -36), Quaternion.identity, 0.1f);
    }


    IEnumerator WaitTicks(int ticks)
    {
        for (int i = 0; i < ticks; i++)
        {
            yield return new WaitForEndOfFrame();
        }
    }

    public void StopAnimator()
    {
        startAnime = false;
        foreach (Transform childTransform in gameObject.transform)
        {
            AnimationManager manager = childTransform.GetComponent<AnimationManager>();
            if (manager != null) manager.animationNow = false;
        }
        enabled = false;
    }
    public void StartAnimator()
    {
        enabled = true;
        foreach (Transform childTransform in gameObject.transform)
        {
            AnimationManager manager = childTransform.GetComponent<AnimationManager>();
            if (manager != null)
            {
                manager.enabled = true;
                manager.animationNow = true;
            }
        }
    }

}

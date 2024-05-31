using System.Collections.Generic;
using UnityEngine;

public class EditorSettings
{
    public HashSet<GameObject> editorObjects = new HashSet<GameObject>();

    public List<Color> defaultColors = new List<Color>
    {
        Color.yellow,
        Color.green,
        Color.blue,
        Color.cyan,
        Color.magenta,
        Color.red,        
        Color.gray,       
        Color.white,
        Color.black,
        new Color(0,0,0,0.5f),
    };

    public GameObject activPixelColor;
    public GameObject polzunok;
    public GameObject pereponka;
    public GameObject exampleColor;

    public GameObject activPereponka;
    public Vector3 cursorPosition;

    private GameObject pereponkaR;
    private GameObject pereponkaG;
    private GameObject pereponkaB;

    public enum ToolMods
    {
        defaulBrash,
        filling,
        pipette
    }
    public ToolMods editorMod = ToolMods.defaulBrash;

    public void Init(GameObject activPixelColor_, GameObject polzunok_, GameObject pereponka_, GameObject exampleColor_)
    {
        activPixelColor = activPixelColor_;
        polzunok = polzunok_;
        pereponka = pereponka_;
        exampleColor = exampleColor_;
    }
    public void SetUpSettings()
    {
        PixelSpawner.InstantiateObject(polzunok, new Vector3(65f, 72f, -1f), Quaternion.identity, editorObjects);
        PixelSpawner.InstantiateObject(polzunok, new Vector3(71f, 72f, -1f), Quaternion.identity, editorObjects);
        PixelSpawner.InstantiateObject(polzunok, new Vector3(77f, 72f, -1f), Quaternion.identity, editorObjects);

        PixelSpawner.text.DrawText(PixelSpawner.MyEditorCard.PixelManager, "R", 63, 81, Color.red, 1, PixelSpawner.MyEditorCard.PixelManager.AllText);
        PixelSpawner.text.DrawText(PixelSpawner.MyEditorCard.PixelManager, "G", 69, 81, Color.green, 1, PixelSpawner.MyEditorCard.PixelManager.AllText);
        PixelSpawner.text.DrawText(PixelSpawner.MyEditorCard.PixelManager, "B", 76, 81, Color.blue, 1, PixelSpawner.MyEditorCard.PixelManager.AllText);

        activPixelColor = PixelSpawner.InstantiateObject(activPixelColor, new Vector3(77f, 72f, -1f), Quaternion.identity, editorObjects);

        pereponkaR = PixelSpawner.InstantiateObject(pereponka, new Vector3(66.02f, 68f, -2f), Quaternion.identity, editorObjects);
        pereponkaG = PixelSpawner.InstantiateObject(pereponka, new Vector3(71.91f, 68f, -2f), Quaternion.identity, editorObjects);
        pereponkaB = PixelSpawner.InstantiateObject(pereponka, new Vector3(77.75f, 68f, -2f), Quaternion.identity, editorObjects);

        activPixelColor.transform.position = new Vector3(86f, 71f, -1f);

        for (int i = 0; i < defaultColors.Count; i++)
        {
            GameObject defaultColor = PixelSpawner.InstantiateObject(exampleColor, new Vector3(112f + i*3, 80f, -1f), Quaternion.identity, editorObjects);
            defaultColor.GetComponent<Renderer>().material.color = defaultColors[i];
        }
    }
    public void CheckRGB()
    {
        float R = CalculateColor(pereponkaR);
        float G = CalculateColor(pereponkaG);
        float B = CalculateColor(pereponkaB);
        activPixelColor.GetComponent<Renderer>().material.color = new Color(R, G, B);
    }
    private float CalculateColor(GameObject _object)
    {
        return (_object.transform.position.y - 63) / 17;
    }

    public void Synchronize()
    {
        pereponkaR.transform.position = new Vector3(pereponkaR.transform.position.x, CalculateY(activPixelColor.GetComponent<Renderer>().material.color.r), pereponkaR.transform.position.z);
        pereponkaG.transform.position = new Vector3(pereponkaG.transform.position.x, CalculateY(activPixelColor.GetComponent<Renderer>().material.color.g), pereponkaG.transform.position.z);
        pereponkaB.transform.position = new Vector3(pereponkaB.transform.position.x, CalculateY(activPixelColor.GetComponent<Renderer>().material.color.b), pereponkaB.transform.position.z);
    }
    private float CalculateY(float color)
    {
        return color * 17 + 63;
    }

    public void ClearSettings(HashSet<GameObject> hashSet)
    {
        foreach (GameObject @object in hashSet)
        {
            PixelSpawner.DestroyObject(@object);
        }
        hashSet.Clear();
    }
}

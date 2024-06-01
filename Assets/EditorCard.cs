<<<<<<< HEAD
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using SFB;
using System;


public class EditorCard : Card
{
    public GameObject pixelPrefab;
    public Dictionary<(int, int), Pixel> saveColors = new Dictionary<(int, int), Pixel>();

    public List<Color[,]> saveList = new List<Color[,]>();   
    public int saveIndex = 0;

    public EditorCard(PixelSpawner pixelSpawner, int x, int y, string value = "")
                 : base(pixelSpawner, x, y, value) { }    
    public void ClearOrFill()
    {
        for (int i = 0; i < colors.GetLength(0); i++)
        {
            for (int j = 0; j < colors.GetLength(1); j++)
            {
                colors[i, j] = Color.black;
            }
        }
        AddSave();
    }
    public void DrawField(bool fadeBlack = true)
    {
        saveColors.Clear();
        ClaerHashSet(ImagePixlels);
        for (int i = 0, y_ = Hight; i < colors.GetLength(0); i++, y_--)
        {
            for (int j = 0; j < colors.GetLength(1); j++)
            {
                Pixel pixel = PixelManager.CreatePixel(Xmain + 1 + j, Ymain - 1 + y_, colors[i, j], ImagePixlels, Zmain, CenterPoint, pixelPrefab);
                saveColors.Add((i, j), pixel);
                EditorPixelInfo editorPixelInfo = pixel.transform.gameObject.GetComponent<EditorPixelInfo>();
                editorPixelInfo.Xcolors = i;
                editorPixelInfo.Ycolors = j;
            }
        }
        foreach (Pixel pixel in ImagePixlels)
        {
            Color color = pixel.RendererPixel.material.color;
            if (fadeBlack && color == Color.black)
            {
                color.a = 0.5f;
                pixel.RendererPixel.material.color = color;
            }
        }
    }
    public static void DrawSinglPixel(Pixel pixel)
    {
        pixel.RendererPixel.material.color = PixelSpawner.MySettings.activPixelColor.GetComponent<Renderer>().material.color;
    }

    public void Save(string outputFileName)
    {
        string paths = StandaloneFileBrowser.SaveFilePanel("Save File", "", "myfile", "txt");
        if (paths == "") return;
        Color[,] colors = new Color[17, 10];
        for (int i = 0; i < colors.GetLength(0); i++)
        {
            for (int j = 0; j < colors.GetLength(1); j++)
            {
                Color color = saveColors[(i, j)].RendererPixel.material.color;
                color.a = 1f;
                colors[i, j] = color;
            }
        }

        //outputFileName += ".txt";

        using (StreamWriter writer = new StreamWriter(paths))
        {
            for (int i = 0; i < colors.GetLength(0); i++)
            {
                writer.Write("{");
                for (int j = 0; j < colors.GetLength(1); j++)
                {
                    writer.Write(colors[i, j].r + ", ");
                    writer.Write(colors[i, j].g + ", ");
                    writer.Write(colors[i, j].b + ", ");
                    writer.Write(colors[i, j].a);

                    if (j < colors.GetLength(1) - 1)
                    {
                        writer.Write(", ");
                    }
                }
                writer.Write("}");
                writer.WriteLine();
            }
        }
    }
    public static Color[,] Load(string fileName)
    {
        if (fileName == "")
        {
            string[] pathsArray = StandaloneFileBrowser.OpenFilePanel("Open File", "", "txt", false);
            fileName = string.Join(", ", pathsArray);
            if (fileName == "") return null;
        }
        Color[,] tempColors = new Color[17, 10];

        string[] lines = File.ReadAllLines(fileName);
        string str = lines[0][1].ToString();
        int result;
        if (int.TryParse(str, out result)) return DefaultLoad(fileName);

        for (int i = 0, k = 0; i < lines.Length; i++, k += 2)
        {
            string[] colorStrings = lines[i].Trim('{', '}', ',', ' ').Split(",");
            for (int j = 0; j < colorStrings.Length; j++)
            {
                colorStrings[j] = colorStrings[j].Trim();
            }

            for (int j = 0; j < colorStrings.Length; j++)
            {
                tempColors[k, j] = convertDictinary[colorStrings[j]];
                tempColors[k + 1, j] = convertDictinary[colorStrings[j]];
            }
        }
        return tempColors;
    }
    private static Color[,] DefaultLoad(string fileName)
    {

        Color[,] colors = new Color[17, 10];

        string[] lines = File.ReadAllLines(fileName);

        for (int i = 0; i < lines.Length; i++)
        {
            string[] colorComponents = lines[i].Replace("{", "").Replace("}", "").Split(',');

            for (int j = 0; j < colorComponents.Length; j += 4)
            {
                float r = float.Parse(colorComponents[j]);
                float g = float.Parse(colorComponents[j + 1]);
                float b = float.Parse(colorComponents[j + 2]);
                float a = float.Parse(colorComponents[j + 3]);

                colors[i, j / 4] = new Color(r, g, b, a);
            }
        }

        return colors;
    }
    public void UpdateField()
    {
        for (int i = 0; i < colors.GetLength(0); i++)
        {
            for (int j = 0; j < colors.GetLength(1); j++)
            {
                Color color = saveColors[(i, j)].RendererPixel.material.color;
                color.a = 1f;
                colors[i, j] = color;
            }
        }
    }

    public override void DrawBorder(Color color, int? limit = null, bool setCenter = true)
    {
        if (CenterPoint == null) PixelManager.SetCenter(this);
        if (MyAnimator == null)
        {
            MyAnimator = PixelSpawner.Centers[this].GetComponent<AnimationScript>();
            MyAnimator.CardObject = PixelSpawner.Centers[this];
            MyAnimator.Card = this;
        }

        if (BorderPixels.Count == 0)
        {
            for (int x = Xmain, y = Ymain, i = 0; y < Ymain + Hight - 1 || x < Xmain + Weight + 1; x++, y++, i++)
            {
                if (y < Ymain + Hight - 1)
                {
                    PixelManager.CreatePixel(Xmain, y + 1, color, BorderPixels, Zmain, CenterPoint);
                    PixelManager.CreatePixel(Xmain + Weight, y + 1, color, BorderPixels, Zmain, CenterPoint);
                }

                if (x < Xmain + Weight + 1)
                {
                    PixelManager.CreatePixel(x, Ymain, color, BorderPixels, Zmain, CenterPoint);
                    PixelManager.CreatePixel(x, Ymain + Hight, color, BorderPixels, Zmain, CenterPoint);
                }
            }
        }
        else Repaint(BorderPixels, color);

        if (actionsForCard != actions.InInventory && setCenter)
            CenterPoint.transform.position = new Vector3(Xmain + Weight / 2, Ymain + Hight / 2, -1);
    }
    public void Filling(int x, int y)
    {
        UpdateField();
        Color fillingColor = PixelSpawner.MySettings.activPixelColor.GetComponent<Renderer>().material.color;
        List<string> keyStack = new List<string>();
        List<string> readyToFill = new List<string>();

        string firsKey = x + ";" + y;
        keyStack.Add(firsKey);
        readyToFill.Add(firsKey);
        Color originColor = colors[x, y];

        while (keyStack.Count > 0)
        {
            string[] parts = keyStack[0].Split(';');
            int x_ = int.Parse(parts[0]);
            int y_ = int.Parse(parts[1]);

            if (y_ > 0) CheckKey(x_ + ";" + y_, x_, y_ - 1);
            if (y_ < colors.GetLength(1) - 1) CheckKey(x_ + ";" + y_, x_, y_ + 1);

            if (x_ > 0) CheckKey(x_ + ";" + y_, x_ - 1, y_);
            if (x_ < colors.GetLength(0) - 1) CheckKey(x_ + ";" + y_, x_ + 1, y_);

        }
        foreach (string key in readyToFill)
        {
            string[] parts = key.Split(';');
            int x_ = int.Parse(parts[0]);
            int y_ = int.Parse(parts[1]);
            colors[x_, y_] = fillingColor;
        }
        DrawField();
        void CheckKey(string originKey, int x, int y)
        {
            string key = x + ";" + y;
            if (colors[x, y] == originColor && !readyToFill.Contains(key))
            {
                keyStack.Add(key);
                readyToFill.Add(key);
            }
            keyStack.Remove(originKey);
        }
    }

    public void Cancel()
    {
        if (saveIndex < saveList.Count - 1)
        {
            Color[,] frame = new Color[17, 10];
            ++saveIndex;
            Array.Copy(saveList[saveIndex], frame, saveList[saveIndex].Length);
            colors = frame;
            DrawField();
        }
    }
    public void Return()
    {
        if (saveIndex > 0)
        {
            Color[,] frame = new Color[17, 10];
            --saveIndex;
            Array.Copy(saveList[saveIndex], frame, saveList[saveIndex].Length);
            colors = frame;
            DrawField();
        }
    }
    public void AddSave()
    {
        if (saveList.Count == 0 || colors != saveList[0])
        {
            Color[,] saveFrame = new Color[17, 10];
            Array.Copy(colors, saveFrame, colors.Length);
            saveList.Insert(0, saveFrame);
        }
    }
    public void ClearCanceledSaves()
    {
        int startIndex = 0;
        int count = saveIndex;
        saveList.RemoveRange(startIndex, count);
        saveIndex = 0;       
    }
}
=======
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using SFB;
using System;


public class EditorCard : Card
{
    public GameObject pixelPrefab;
    public Dictionary<(int, int), Pixel> saveColors = new Dictionary<(int, int), Pixel>();

    public List<Color[,]> saveList = new List<Color[,]>();   
    public int saveIndex = 0;

    public EditorCard(PixelSpawner pixelSpawner, int x, int y, string value = "")
                 : base(pixelSpawner, x, y, value) { }    
    public void ClearOrFill()
    {
        for (int i = 0; i < colors.GetLength(0); i++)
        {
            for (int j = 0; j < colors.GetLength(1); j++)
            {
                colors[i, j] = Color.black;
            }
        }
        AddSave();
    }
    public void DrawField(bool fadeBlack = true)
    {
        saveColors.Clear();
        ClaerHashSet(ImagePixlels);
        for (int i = 0, y_ = Hight; i < colors.GetLength(0); i++, y_--)
        {
            for (int j = 0; j < colors.GetLength(1); j++)
            {
                Pixel pixel = PixelManager.CreatePixel(Xmain + 1 + j, Ymain - 1 + y_, colors[i, j], ImagePixlels, Zmain, CenterPoint, pixelPrefab);
                saveColors.Add((i, j), pixel);
                EditorPixelInfo editorPixelInfo = pixel.transform.gameObject.GetComponent<EditorPixelInfo>();
                editorPixelInfo.Xcolors = i;
                editorPixelInfo.Ycolors = j;
            }
        }
        foreach (Pixel pixel in ImagePixlels)
        {
            Color color = pixel.RendererPixel.material.color;
            if (fadeBlack && color == Color.black)
            {
                color.a = 0.5f;
                pixel.RendererPixel.material.color = color;
            }
        }
    }
    public static void DrawSinglPixel(Pixel pixel)
    {
        pixel.RendererPixel.material.color = PixelSpawner.MySettings.activPixelColor.GetComponent<Renderer>().material.color;
    }

    public void Save(string outputFileName)
    {
        string paths = StandaloneFileBrowser.SaveFilePanel("Save File", "", "myfile", "txt");
        if (paths == "") return;
        Color[,] colors = new Color[17, 10];
        for (int i = 0; i < colors.GetLength(0); i++)
        {
            for (int j = 0; j < colors.GetLength(1); j++)
            {
                Color color = saveColors[(i, j)].RendererPixel.material.color;
                color.a = 1f;
                colors[i, j] = color;
            }
        }

        //outputFileName += ".txt";

        using (StreamWriter writer = new StreamWriter(paths))
        {
            for (int i = 0; i < colors.GetLength(0); i++)
            {
                writer.Write("{");
                for (int j = 0; j < colors.GetLength(1); j++)
                {
                    writer.Write(colors[i, j].r + ", ");
                    writer.Write(colors[i, j].g + ", ");
                    writer.Write(colors[i, j].b + ", ");
                    writer.Write(colors[i, j].a);

                    if (j < colors.GetLength(1) - 1)
                    {
                        writer.Write(", ");
                    }
                }
                writer.Write("}");
                writer.WriteLine();
            }
        }
    }
    public static Color[,] Load(string fileName)
    {
        if (fileName == "")
        {
            string[] pathsArray = StandaloneFileBrowser.OpenFilePanel("Open File", "", "txt", false);
            fileName = string.Join(", ", pathsArray);
            if (fileName == "") return null;
        }
        Color[,] tempColors = new Color[17, 10];

        string[] lines = File.ReadAllLines(fileName);
        string str = lines[0][1].ToString();
        int result;
        if (int.TryParse(str, out result)) return DefaultLoad(fileName);

        for (int i = 0, k = 0; i < lines.Length; i++, k += 2)
        {
            string[] colorStrings = lines[i].Trim('{', '}', ',', ' ').Split(",");
            for (int j = 0; j < colorStrings.Length; j++)
            {
                colorStrings[j] = colorStrings[j].Trim();
            }

            for (int j = 0; j < colorStrings.Length; j++)
            {
                tempColors[k, j] = convertDictinary[colorStrings[j]];
                tempColors[k + 1, j] = convertDictinary[colorStrings[j]];
            }
        }
        return tempColors;
    }
    private static Color[,] DefaultLoad(string fileName)
    {

        Color[,] colors = new Color[17, 10];

        string[] lines = File.ReadAllLines(fileName);

        for (int i = 0; i < lines.Length; i++)
        {
            string[] colorComponents = lines[i].Replace("{", "").Replace("}", "").Split(',');

            for (int j = 0; j < colorComponents.Length; j += 4)
            {
                float r = float.Parse(colorComponents[j]);
                float g = float.Parse(colorComponents[j + 1]);
                float b = float.Parse(colorComponents[j + 2]);
                float a = float.Parse(colorComponents[j + 3]);

                colors[i, j / 4] = new Color(r, g, b, a);
            }
        }

        return colors;
    }
    public void UpdateField()
    {
        for (int i = 0; i < colors.GetLength(0); i++)
        {
            for (int j = 0; j < colors.GetLength(1); j++)
            {
                Color color = saveColors[(i, j)].RendererPixel.material.color;
                color.a = 1f;
                colors[i, j] = color;
            }
        }
    }

    public override void DrawBorder(Color color, int? limit = null, bool setCenter = true)
    {
        if (CenterPoint == null) PixelManager.SetCenter(this);
        if (MyAnimator == null)
        {
            MyAnimator = PixelSpawner.Centers[this].GetComponent<AnimationScript>();
            MyAnimator.CardObject = PixelSpawner.Centers[this];
            MyAnimator.Card = this;
        }

        if (BorderPixels.Count == 0)
        {
            for (int x = Xmain, y = Ymain, i = 0; y < Ymain + Hight - 1 || x < Xmain + Weight + 1; x++, y++, i++)
            {
                if (y < Ymain + Hight - 1)
                {
                    PixelManager.CreatePixel(Xmain, y + 1, color, BorderPixels, Zmain, CenterPoint);
                    PixelManager.CreatePixel(Xmain + Weight, y + 1, color, BorderPixels, Zmain, CenterPoint);
                }

                if (x < Xmain + Weight + 1)
                {
                    PixelManager.CreatePixel(x, Ymain, color, BorderPixels, Zmain, CenterPoint);
                    PixelManager.CreatePixel(x, Ymain + Hight, color, BorderPixels, Zmain, CenterPoint);
                }
            }
        }
        else Repaint(BorderPixels, color);

        if (actionsForCard != actions.InInventory && setCenter)
            CenterPoint.transform.position = new Vector3(Xmain + Weight / 2, Ymain + Hight / 2, -1);
    }
    public void Filling(int x, int y)
    {
        UpdateField();
        Color fillingColor = PixelSpawner.MySettings.activPixelColor.GetComponent<Renderer>().material.color;
        List<string> keyStack = new List<string>();
        List<string> readyToFill = new List<string>();

        string firsKey = x + ";" + y;
        keyStack.Add(firsKey);
        readyToFill.Add(firsKey);
        Color originColor = colors[x, y];

        while (keyStack.Count > 0)
        {
            string[] parts = keyStack[0].Split(';');
            int x_ = int.Parse(parts[0]);
            int y_ = int.Parse(parts[1]);

            if (y_ > 0) CheckKey(x_ + ";" + y_, x_, y_ - 1);
            if (y_ < colors.GetLength(1) - 1) CheckKey(x_ + ";" + y_, x_, y_ + 1);

            if (x_ > 0) CheckKey(x_ + ";" + y_, x_ - 1, y_);
            if (x_ < colors.GetLength(0) - 1) CheckKey(x_ + ";" + y_, x_ + 1, y_);

        }
        foreach (string key in readyToFill)
        {
            string[] parts = key.Split(';');
            int x_ = int.Parse(parts[0]);
            int y_ = int.Parse(parts[1]);
            colors[x_, y_] = fillingColor;
        }
        DrawField();
        void CheckKey(string originKey, int x, int y)
        {
            string key = x + ";" + y;
            if (colors[x, y] == originColor && !readyToFill.Contains(key))
            {
                keyStack.Add(key);
                readyToFill.Add(key);
            }
            keyStack.Remove(originKey);
        }
    }

    public void Cancel()
    {
        if (saveIndex < saveList.Count - 1)
        {
            Color[,] frame = new Color[17, 10];
            ++saveIndex;
            Array.Copy(saveList[saveIndex], frame, saveList[saveIndex].Length);
            colors = frame;
            DrawField();
        }
    }
    public void Return()
    {
        if (saveIndex > 0)
        {
            Color[,] frame = new Color[17, 10];
            --saveIndex;
            Array.Copy(saveList[saveIndex], frame, saveList[saveIndex].Length);
            colors = frame;
            DrawField();
        }
    }
    public void AddSave()
    {
        if (saveList.Count == 0 || colors != saveList[0])
        {
            Color[,] saveFrame = new Color[17, 10];
            Array.Copy(colors, saveFrame, colors.Length);
            saveList.Insert(0, saveFrame);
        }
    }
    public void ClearCanceledSaves()
    {
        int startIndex = 0;
        int count = saveIndex;
        saveList.RemoveRange(startIndex, count);
        saveIndex = 0;       
    }
}
>>>>>>> 12fa110b21928b31eb4877f8d5b5df0b12480307

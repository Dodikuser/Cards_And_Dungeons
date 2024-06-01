<<<<<<< HEAD
using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;


[Serializable]
public class Card
{
    public Card(PixelSpawner pixelSpawner, int x, int y, string value = "", int z = -1)
    {
        Xmain = x;
        Ymain = y;
        Zmain = z;
        filePath = "Old_Sptites/sprites/army/power2/Knight_Prince.txt";
        Value = value;
        Enabled = false;
        PixelManager = pixelSpawner;

    }
    [JsonIgnore]
    public PixelSpawner PixelManager { get; set; }

    public int Xmain { get; set; }
    public int Ymain { get; set; }
    public int Zmain { get; set; }
    public virtual string Value { get; set; }
    public string filePath { get; set; }
    public bool Enabled { get; set; }

    public int Hight = 18;
    public int Weight = 11;

    [JsonIgnore]
    public Color[,] colors = new Color[17, 10]{
        { Color.red, Color.green, Color.HSVToRGB(50,30,16), Color.blue, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue},
        { Color.red, Color.green, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue},
        { Color.red, Color.green, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue},
        { Color.red, Color.green, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue},
        { Color.red, Color.green, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue},
        { Color.red, Color.green, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue},
        { Color.red, Color.green, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue},
        { Color.red, Color.green, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue},
        { Color.red, Color.green, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue},
        { Color.red, Color.green, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue},
        { Color.red, Color.green, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue},
        { Color.red, Color.green, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue},
        { Color.red, Color.green, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue},
        { Color.red, Color.green, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue},
        { Color.red, Color.green, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue},
        { Color.red, Color.green, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue},
        { Color.red, Color.green, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue},

    };

    public enum actions
    {
        Hide,
        Visible,
        Deleted,
        InInventory,
    }
    public enum condition
    {
        Hide,
        EnabletToSElect,
        Select,
        EnabletToSOpen,
        InChest
    }
    public actions actionsForCard = actions.Hide;
    public condition borderCondition = condition.Hide;

    [JsonIgnore]
    static Dictionary<condition, Color> borderColors = new Dictionary<condition, Color>
        {
            { condition.Hide, Color.gray },
            { condition.Select, Color.red },
            { condition.EnabletToSElect, Color.yellow },
            { condition.EnabletToSOpen, Color.HSVToRGB(0f, 0f, 0.5f) },
            { condition.InChest, Color.green }
        };
    [JsonIgnore]
    public HashSet<Pixel> ImagePixlels = new HashSet<Pixel>();
    [JsonIgnore]
    public HashSet<Pixel> BorderPixels = new HashSet<Pixel>();
    [JsonIgnore]
    public HashSet<Pixel> FramePixels = new HashSet<Pixel>();
    [JsonIgnore]
    public HashSet<Pixel> ValuePixels = new HashSet<Pixel>();

    [JsonIgnore]
    public GameObject Collider;
    [JsonIgnore]
    public GameObject CenterPoint;
    [JsonIgnore]
    public AnimationScript MyAnimator;

    public bool select = false;
    public bool enabletToSelect = false;
    public bool locked = false;
    public bool InCace = false;

    [JsonIgnore]
    public static Dictionary<string, Color> convertDictinary = new Dictionary<string, Color>() {
        {"Black", Color.black },
        {"Red", Color.red },
        {"Green", Color.green },
        {"White", Color.white },
        {"Yellow", Color.yellow },
        {"Blue", Color.blue },
        {"Cyan", Color.cyan },
        {"Magenta", Color.magenta },
        {"Gray", Color.gray },
        {"DarkRed", Color.HSVToRGB(0f, 1f, 0.5f)},      //  расный с половиной €ркости
        {"DarkGreen", Color.HSVToRGB(120f/360f, 1f, 0.5f)}, // «еленый с половиной €ркости
        {"DarkBlue", Color.HSVToRGB(240f/360f, 1f, 0.5f)}, // —иний с половиной €ркости
        {"DarkCyan", Color.HSVToRGB(180f/360f, 1f, 0.5f)}, // ÷иан с половиной €ркости
        {"DarkMagenta", Color.HSVToRGB(300f/360f, 1f, 0.5f)}, // ћаджента с половиной €ркости
        {"DarkYellow", Color.HSVToRGB(60f/360f, 1f, 0.5f)}, // ∆елтый с половиной €ркости
        {"DarkGray", Color.HSVToRGB(0f, 0f, 0.5f)}, // —ерый с половиной €ркости
    };
    public void SetCollider()
    {
        PixelManager.CreateCollider(this);
    }
    public void ClaerHashSet(HashSet<Pixel> hashSet)
    {
        foreach (Pixel pixel in hashSet)
        {
            PixelSpawner.DestroyObject(pixel.gameObject);
        }
        hashSet.Clear();
    }
    public void ClearCard()
    {
        ClaerHashSet(BorderPixels);
        ClaerHashSet(ImagePixlels);
        ClaerHashSet(FramePixels);
        ClaerHashSet(ValuePixels);
    }

    public Color[,] DeShifr(string file)
    {
        Color[,] tempColors = new Color[17, 10];

        string[] lines = File.ReadAllLines(file);
        string str = lines[0][1].ToString();
        int result;
        if (int.TryParse(str, out result) )
        {
            //Debug.Log("It can be open as Color[]");
            return EditorCard.Load(file);
        }
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

    public void DrawCard(bool ignorBlack = true, int? limit = null, bool setInfo = true)
    {
        string file = filePath;

        if (actionsForCard != actions.InInventory) DrawBorder(Color.gray);

        if (actionsForCard != actions.Hide)
        {
            if (PixelManager.AllSprites.ContainsKey(file))
            {
                colors = PixelManager.AllSprites[file];
            }
            else
            {
                colors = DeShifr(file);
                PixelManager.AllSprites.Add(file, colors);
            }

            for (int i = 0, y_ = Hight; i < colors.GetLength(0); i++, y_--)
            {
                for (int j = 0; j < colors.GetLength(1); j++)
                {
                    if (limit == null || j < limit)
                        PixelManager.CreatePixel(Xmain + 1 + j, Ymain - 1 + y_, colors[i, j], ImagePixlels, Zmain, CenterPoint);
                }
            }
            int offset = (limit != null) ? 11 - (int)limit : 0;
            DrowValue(Color.green, offset);
        }
        if (select)
        {
            //Thread.Sleep(100);
            Select(PixelSpawner.selectColor);
        }
        if (setInfo) MyAnimator.SetStartInfo();

    }
    public virtual void DrowValue(Color color, int offsetX)
    {
        //PixelSpawner.text.DrawText(PixelManager, Value, Xmain + 1, Ymain + 1, color, 1, ImagePixlels, -2);
        PixelSpawner.text.DrawText(PixelManager, Value, Xmain + 8 - (Value.Length - 1) - offsetX, Ymain + Hight - 3, color, 1, ValuePixels, Zmain-1, CenterPoint);
        MyAnimator.SetStartInfo();
    }

    public virtual void DrawBorder(Color color, int? limit = null, bool setCenter = true)
    {
        if (Collider == null) SetCollider();
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
                    if (limit == null) PixelManager.CreatePixel(Xmain + Weight, y + 1, color, BorderPixels, Zmain, CenterPoint);
                }

                if (x < Xmain + Weight + 1)
                {
                    if (limit == null || i < limit)
                    {
                        PixelManager.CreatePixel(x, Ymain, color, BorderPixels, Zmain, CenterPoint);
                        PixelManager.CreatePixel(x, Ymain + Hight, color, BorderPixels, Zmain, CenterPoint);
                    }
                }
            }
            MyAnimator.SetStartInfo();
        }
        else Repaint(BorderPixels, color);

        if (BorderPixels.Count <= 90 && actionsForCard == actions.Deleted)
        {
            for (int x = Xmain, y = Ymain + 18, i = 0; y > Ymain + Hight - 1 || x < Xmain + Weight - 1; x++, y -= 2, i++)
            {
                if (x < Xmain + Weight + 1)
                {
                    PixelManager.CreatePixel((i > 4) ? x + 1 : x, y, color, BorderPixels, Zmain-1, CenterPoint);
                    PixelManager.CreatePixel((i > 4) ? x + 2 : x + 1, y, color, BorderPixels, Zmain-1, CenterPoint);
                    if (y - 1 >= Ymain) PixelManager.CreatePixel((i > 4) ? x + 2 : x + 1, y - 1, color, BorderPixels, Zmain-1, CenterPoint);
                    //if (i == 5) PixelManager.CreatePixel(x, y, color, BorderPixels,-2);
                }
            }
            for (int x = Xmain, y = Ymain, i = 0; y < Ymain + Hight - 2 && x < Xmain + Weight + 1; x++, y += 2, i++)
            {
                PixelManager.CreatePixel((i > 4) ? x + 1 : x, y, color, BorderPixels, Zmain - 1, CenterPoint);
                PixelManager.CreatePixel((i > 4) ? x + 2 : x + 1, y, color, BorderPixels, Zmain - 1, CenterPoint);
                if (y + 1 <= Ymain + 18) PixelManager.CreatePixel((i > 4) ? x + 2 : x + 1, y + 1, color, BorderPixels, Zmain - 1, CenterPoint);
                if (i == 5) PixelManager.CreatePixel(x + 1, y - 1, color, BorderPixels, Zmain - 1, CenterPoint);
            }
            MyAnimator.SetStartInfo();
        }
        if (actionsForCard != actions.InInventory && setCenter)
        {
            Collider.transform.localScale = new Vector3(this.Weight + 1, this.Hight + 1, 1);
            CenterPoint.transform.position = new Vector3(Xmain + Weight / 2, Ymain + Hight / 2, -1);
        }
    }
    public virtual void DrawFrame(Color color, bool select = false)
    {
        if (FramePixels.Count == 0 || FramePixels.Count <= 30)
        {
            if (select) ClaerHashSet(FramePixels);

            for (int x = Xmain - 1; x < Xmain + Weight; x++)
            {
                if (!select || !(x > Xmain + 1) || (x > Xmain + Weight - 4))
                {
                    PixelManager.CreatePixel(x + 1, Ymain - 1, color, FramePixels, Zmain, CenterPoint);
                    PixelManager.CreatePixel(x + 1, Ymain - 1 + Hight + 2, color, FramePixels, Zmain, CenterPoint);
                }

            }
            for (int y = Ymain - 1; y < Ymain + Hight + 2; y++)
            {
                if (!select || !(y > Ymain + 2) || (y > Ymain + Hight - 3))
                {
                    PixelManager.CreatePixel(Xmain - 1, y, color, FramePixels, Zmain, CenterPoint);
                    PixelManager.CreatePixel(Xmain - 1 + Weight + 2, y, color, FramePixels, Zmain, CenterPoint);
                }
            }
            MyAnimator.SetStartInfo();
        }
        else Repaint(FramePixels, color);

    }

    public virtual void Select(Color selectedColor, bool drawSeleck = true)
    {
        if (PixelSpawner.nowLocation == PixelSpawner.location.Chest)
        {
            foreach (Card card in PixelSpawner.OnlyCards)
            {
                if (card.locked) card.DrawFrame(Color.gray);
            }
        }
        if (drawSeleck)
        {
            DrawBorder(selectedColor, null, false);
            DrawFrame(Color.HSVToRGB(0f, 1f, 0.5f), (actionsForCard == actions.InInventory) ? false : true);          
        }

        select = true;
        PixelSpawner.selectedCard = this;
        if (Enabled && actionsForCard == actions.Hide && PixelSpawner.nowLocation != PixelSpawner.location.Chest)
        {
            PixelManager.SelectEnable();
        }
    }
    public void UnSelect(Color selectedColor)
    {
        if (!Enabled)
        {
            DrawBorder(selectedColor);
            enabletToSelect = false;
        }
        else EnabledCard();

        if (actionsForCard == actions.Visible || (actionsForCard == actions.Deleted && PixelSpawner.nowLocation != PixelSpawner.location.Chest)) VisibleRender();
        select = false;
        if (actionsForCard != actions.InInventory && FramePixels.Count <= 30) ClaerHashSet(FramePixels);
        //if (locked) DrawBorder(Color.gray);
    }

    public void EnabledCard(ConsoleColor selectedColor = ConsoleColor.Yellow)
    {
        if (actionsForCard == actions.Hide)
        {
            if (borderCondition != condition.InChest) borderCondition = condition.EnabletToSElect;
            UpdateCondition();

            Enabled = true;
            this.Enabled = true;
        }
    }
    public void UpdateCondition()
    {
        if (borderColors.TryGetValue(borderCondition, out var color))
        {
            DrawBorder(color);
        }
    }
    public void Repaint(HashSet<Pixel> hashSet, Color color)
    {
        foreach (Pixel pixel in hashSet)
        {
            pixel.RendererPixel.material.color = color;
        }
    }

    public virtual void VisibleRender(int? limit = null) { }
    public virtual void Interactions() { }

}
=======
using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;


[Serializable]
public class Card
{
    public Card(PixelSpawner pixelSpawner, int x, int y, string value = "", int z = -1)
    {
        Xmain = x;
        Ymain = y;
        Zmain = z;
        filePath = "Old_Sptites/sprites/army/power2/Knight_Prince.txt";
        Value = value;
        Enabled = false;
        PixelManager = pixelSpawner;

    }
    [JsonIgnore]
    public PixelSpawner PixelManager { get; set; }

    public int Xmain { get; set; }
    public int Ymain { get; set; }
    public int Zmain { get; set; }
    public virtual string Value { get; set; }
    public string filePath { get; set; }
    public bool Enabled { get; set; }

    public int Hight = 18;
    public int Weight = 11;

    [JsonIgnore]
    public Color[,] colors = new Color[17, 10]{
        { Color.red, Color.green, Color.HSVToRGB(50,30,16), Color.blue, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue},
        { Color.red, Color.green, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue},
        { Color.red, Color.green, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue},
        { Color.red, Color.green, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue},
        { Color.red, Color.green, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue},
        { Color.red, Color.green, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue},
        { Color.red, Color.green, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue},
        { Color.red, Color.green, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue},
        { Color.red, Color.green, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue},
        { Color.red, Color.green, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue},
        { Color.red, Color.green, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue},
        { Color.red, Color.green, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue},
        { Color.red, Color.green, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue},
        { Color.red, Color.green, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue},
        { Color.red, Color.green, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue},
        { Color.red, Color.green, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue},
        { Color.red, Color.green, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue, Color.blue},

    };

    public enum actions
    {
        Hide,
        Visible,
        Deleted,
        InInventory,
    }
    public enum condition
    {
        Hide,
        EnabletToSElect,
        Select,
        EnabletToSOpen,
        InChest
    }
    public actions actionsForCard = actions.Hide;
    public condition borderCondition = condition.Hide;

    [JsonIgnore]
    static Dictionary<condition, Color> borderColors = new Dictionary<condition, Color>
        {
            { condition.Hide, Color.gray },
            { condition.Select, Color.red },
            { condition.EnabletToSElect, Color.yellow },
            { condition.EnabletToSOpen, Color.HSVToRGB(0f, 0f, 0.5f) },
            { condition.InChest, Color.green }
        };
    [JsonIgnore]
    public HashSet<Pixel> ImagePixlels = new HashSet<Pixel>();
    [JsonIgnore]
    public HashSet<Pixel> BorderPixels = new HashSet<Pixel>();
    [JsonIgnore]
    public HashSet<Pixel> FramePixels = new HashSet<Pixel>();
    [JsonIgnore]
    public HashSet<Pixel> ValuePixels = new HashSet<Pixel>();

    [JsonIgnore]
    public GameObject Collider;
    [JsonIgnore]
    public GameObject CenterPoint;
    [JsonIgnore]
    public AnimationScript MyAnimator;

    public bool select = false;
    public bool enabletToSelect = false;
    public bool locked = false;
    public bool InCace = false;

    [JsonIgnore]
    public static Dictionary<string, Color> convertDictinary = new Dictionary<string, Color>() {
        {"Black", Color.black },
        {"Red", Color.red },
        {"Green", Color.green },
        {"White", Color.white },
        {"Yellow", Color.yellow },
        {"Blue", Color.blue },
        {"Cyan", Color.cyan },
        {"Magenta", Color.magenta },
        {"Gray", Color.gray },
        {"DarkRed", Color.HSVToRGB(0f, 1f, 0.5f)},      //  расный с половиной €ркости
        {"DarkGreen", Color.HSVToRGB(120f/360f, 1f, 0.5f)}, // «еленый с половиной €ркости
        {"DarkBlue", Color.HSVToRGB(240f/360f, 1f, 0.5f)}, // —иний с половиной €ркости
        {"DarkCyan", Color.HSVToRGB(180f/360f, 1f, 0.5f)}, // ÷иан с половиной €ркости
        {"DarkMagenta", Color.HSVToRGB(300f/360f, 1f, 0.5f)}, // ћаджента с половиной €ркости
        {"DarkYellow", Color.HSVToRGB(60f/360f, 1f, 0.5f)}, // ∆елтый с половиной €ркости
        {"DarkGray", Color.HSVToRGB(0f, 0f, 0.5f)}, // —ерый с половиной €ркости
    };
    public void SetCollider()
    {
        PixelManager.CreateCollider(this);
    }
    public void ClaerHashSet(HashSet<Pixel> hashSet)
    {
        foreach (Pixel pixel in hashSet)
        {
            PixelSpawner.DestroyObject(pixel.gameObject);
        }
        hashSet.Clear();
    }
    public void ClearCard()
    {
        ClaerHashSet(BorderPixels);
        ClaerHashSet(ImagePixlels);
        ClaerHashSet(FramePixels);
        ClaerHashSet(ValuePixels);
    }

    public Color[,] DeShifr(string file)
    {
        Color[,] tempColors = new Color[17, 10];

        string[] lines = File.ReadAllLines(file);
        string str = lines[0][1].ToString();
        int result;
        if (int.TryParse(str, out result) )
        {
            //Debug.Log("It can be open as Color[]");
            return EditorCard.Load(file);
        }
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

    public void DrawCard(bool ignorBlack = true, int? limit = null, bool setInfo = true)
    {
        string file = filePath;

        if (actionsForCard != actions.InInventory) DrawBorder(Color.gray);

        if (actionsForCard != actions.Hide)
        {
            if (PixelManager.AllSprites.ContainsKey(file))
            {
                colors = PixelManager.AllSprites[file];
            }
            else
            {
                colors = DeShifr(file);
                PixelManager.AllSprites.Add(file, colors);
            }

            for (int i = 0, y_ = Hight; i < colors.GetLength(0); i++, y_--)
            {
                for (int j = 0; j < colors.GetLength(1); j++)
                {
                    if (limit == null || j < limit)
                        PixelManager.CreatePixel(Xmain + 1 + j, Ymain - 1 + y_, colors[i, j], ImagePixlels, Zmain, CenterPoint);
                }
            }
            int offset = (limit != null) ? 11 - (int)limit : 0;
            DrowValue(Color.green, offset);
        }
        if (select)
        {
            //Thread.Sleep(100);
            Select(PixelSpawner.selectColor);
        }
        if (setInfo) MyAnimator.SetStartInfo();

    }
    public virtual void DrowValue(Color color, int offsetX)
    {
        //PixelSpawner.text.DrawText(PixelManager, Value, Xmain + 1, Ymain + 1, color, 1, ImagePixlels, -2);
        PixelSpawner.text.DrawText(PixelManager, Value, Xmain + 8 - (Value.Length - 1) - offsetX, Ymain + Hight - 3, color, 1, ValuePixels, Zmain-1, CenterPoint);
        MyAnimator.SetStartInfo();
    }

    public virtual void DrawBorder(Color color, int? limit = null, bool setCenter = true)
    {
        if (Collider == null) SetCollider();
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
                    if (limit == null) PixelManager.CreatePixel(Xmain + Weight, y + 1, color, BorderPixels, Zmain, CenterPoint);
                }

                if (x < Xmain + Weight + 1)
                {
                    if (limit == null || i < limit)
                    {
                        PixelManager.CreatePixel(x, Ymain, color, BorderPixels, Zmain, CenterPoint);
                        PixelManager.CreatePixel(x, Ymain + Hight, color, BorderPixels, Zmain, CenterPoint);
                    }
                }
            }
            MyAnimator.SetStartInfo();
        }
        else Repaint(BorderPixels, color);

        if (BorderPixels.Count <= 90 && actionsForCard == actions.Deleted)
        {
            for (int x = Xmain, y = Ymain + 18, i = 0; y > Ymain + Hight - 1 || x < Xmain + Weight - 1; x++, y -= 2, i++)
            {
                if (x < Xmain + Weight + 1)
                {
                    PixelManager.CreatePixel((i > 4) ? x + 1 : x, y, color, BorderPixels, Zmain-1, CenterPoint);
                    PixelManager.CreatePixel((i > 4) ? x + 2 : x + 1, y, color, BorderPixels, Zmain-1, CenterPoint);
                    if (y - 1 >= Ymain) PixelManager.CreatePixel((i > 4) ? x + 2 : x + 1, y - 1, color, BorderPixels, Zmain-1, CenterPoint);
                    //if (i == 5) PixelManager.CreatePixel(x, y, color, BorderPixels,-2);
                }
            }
            for (int x = Xmain, y = Ymain, i = 0; y < Ymain + Hight - 2 && x < Xmain + Weight + 1; x++, y += 2, i++)
            {
                PixelManager.CreatePixel((i > 4) ? x + 1 : x, y, color, BorderPixels, Zmain - 1, CenterPoint);
                PixelManager.CreatePixel((i > 4) ? x + 2 : x + 1, y, color, BorderPixels, Zmain - 1, CenterPoint);
                if (y + 1 <= Ymain + 18) PixelManager.CreatePixel((i > 4) ? x + 2 : x + 1, y + 1, color, BorderPixels, Zmain - 1, CenterPoint);
                if (i == 5) PixelManager.CreatePixel(x + 1, y - 1, color, BorderPixels, Zmain - 1, CenterPoint);
            }
            MyAnimator.SetStartInfo();
        }
        if (actionsForCard != actions.InInventory && setCenter)
        {
            Collider.transform.localScale = new Vector3(this.Weight + 1, this.Hight + 1, 1);
            CenterPoint.transform.position = new Vector3(Xmain + Weight / 2, Ymain + Hight / 2, -1);
        }
    }
    public virtual void DrawFrame(Color color, bool select = false)
    {
        if (FramePixels.Count == 0 || FramePixels.Count <= 30)
        {
            if (select) ClaerHashSet(FramePixels);

            for (int x = Xmain - 1; x < Xmain + Weight; x++)
            {
                if (!select || !(x > Xmain + 1) || (x > Xmain + Weight - 4))
                {
                    PixelManager.CreatePixel(x + 1, Ymain - 1, color, FramePixels, Zmain, CenterPoint);
                    PixelManager.CreatePixel(x + 1, Ymain - 1 + Hight + 2, color, FramePixels, Zmain, CenterPoint);
                }

            }
            for (int y = Ymain - 1; y < Ymain + Hight + 2; y++)
            {
                if (!select || !(y > Ymain + 2) || (y > Ymain + Hight - 3))
                {
                    PixelManager.CreatePixel(Xmain - 1, y, color, FramePixels, Zmain, CenterPoint);
                    PixelManager.CreatePixel(Xmain - 1 + Weight + 2, y, color, FramePixels, Zmain, CenterPoint);
                }
            }
            MyAnimator.SetStartInfo();
        }
        else Repaint(FramePixels, color);

    }

    public virtual void Select(Color selectedColor, bool drawSeleck = true)
    {
        if (PixelSpawner.nowLocation == PixelSpawner.location.Chest)
        {
            foreach (Card card in PixelSpawner.OnlyCards)
            {
                if (card.locked) card.DrawFrame(Color.gray);
            }
        }
        if (drawSeleck)
        {
            DrawBorder(selectedColor, null, false);
            DrawFrame(Color.HSVToRGB(0f, 1f, 0.5f), (actionsForCard == actions.InInventory) ? false : true);          
        }

        select = true;
        PixelSpawner.selectedCard = this;
        if (Enabled && actionsForCard == actions.Hide && PixelSpawner.nowLocation != PixelSpawner.location.Chest)
        {
            PixelManager.SelectEnable();
        }
    }
    public void UnSelect(Color selectedColor)
    {
        if (!Enabled)
        {
            DrawBorder(selectedColor);
            enabletToSelect = false;
        }
        else EnabledCard();

        if (actionsForCard == actions.Visible || (actionsForCard == actions.Deleted && PixelSpawner.nowLocation != PixelSpawner.location.Chest)) VisibleRender();
        select = false;
        if (actionsForCard != actions.InInventory && FramePixels.Count <= 30) ClaerHashSet(FramePixels);
        //if (locked) DrawBorder(Color.gray);
    }

    public void EnabledCard(ConsoleColor selectedColor = ConsoleColor.Yellow)
    {
        if (actionsForCard == actions.Hide)
        {
            if (borderCondition != condition.InChest) borderCondition = condition.EnabletToSElect;
            UpdateCondition();

            Enabled = true;
            this.Enabled = true;
        }
    }
    public void UpdateCondition()
    {
        if (borderColors.TryGetValue(borderCondition, out var color))
        {
            DrawBorder(color);
        }
    }
    public void Repaint(HashSet<Pixel> hashSet, Color color)
    {
        foreach (Pixel pixel in hashSet)
        {
            pixel.RendererPixel.material.color = color;
        }
    }

    public virtual void VisibleRender(int? limit = null) { }
    public virtual void Interactions() { }

}
>>>>>>> 12fa110b21928b31eb4877f8d5b5df0b12480307

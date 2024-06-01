<<<<<<< HEAD
using System.Collections.Generic;
using static PixelSpawner;
using UnityEngine;
using System.IO;
using System.Linq;

internal class MenuCard : Card
{
    public MenuCard(PixelSpawner pixelSpawner, int x, int y, string value = "", string text = "XXXXX")
                : base(pixelSpawner, x, y, value)
    {
        filePath = "Old_Sptites/0.txt";
        Hight = 10;
        Weight = 51;
        textInside = text;
    }
    public string textInside { get; set; }
    public HashSet<Pixel> textHashSet { get; set; }

    public override void VisibleRender(int? limit = null)
    {
        DrawBorder(Color.HSVToRGB(300f / 360f, 1f, 0.5f), null);
    }
    public void AddMenuCard(int offsetX = 0)
    {
        this.actionsForCard = actions.Visible;
        this.VisibleRender();
        this.Enabled = true;
        string key = (this.Xmain + 2).ToString() + "; " + (this.Ymain + 4).ToString();
        PixelSpawner.Cards.Add(key, this);
        PixelSpawner.OnlyCards.Add(this);
        DrawText(Color.HSVToRGB(60f / 360f, 1f, 0.5f), offsetX);
    }
    public void DrawText(Color color, int offsetX = 0)
    {
        PixelSpawner.text.DrawText(PixelManager, textInside, Xmain + (Weight / 2) - (textInside.Length * 5) / 2 + offsetX, (Ymain + (Hight / 2) - 3), color, 1, PixelManager.AllText, -1, CenterPoint);
        MyAnimator.SetStartInfo();
    }
}
internal class StartCard : MenuCard
{
    public StartCard(PixelSpawner pixelSpawner, int x, int y, string value = "", string text = "START")
                : base(pixelSpawner, x, y, value)
    {
        textInside = text;
    }

    public override void Interactions()
    {
        AudioManager.StopBackgroundMusic();
        AudioManager.PlaySound("startGame", 1f);
        nowLocation = location.Dungon;
        stats.MyStatistics["CountOfRuns"]++;
        PixelSpawner.Inventory.CleenAll();
        PixelSpawner.Inventory.CardInventory[0].Clear();
        PixelSpawner.Inventory.CardInventory[1].Clear();
        PixelSpawner.Inventory.CardInventory[2].Clear();
        dungenLevel = 0;
        coin.Value = 30;
        countKillcard = 10;
        coin.DrowCoin();
        coin.DrowValue();
        PixelManager.NextLevl();

    }
}
internal class ReStartCard : StartCard
{
    public ReStartCard(PixelSpawner pixelSpawner, int x, int y, string value = "", string text = "RESTART")
                : base(pixelSpawner, x, y, value)
    {
        textInside = text;
    }
}
internal class ExitCard : MenuCard
{
    public ExitCard(PixelSpawner pixelSpawner, int x, int y, string value = "", string text = "EXIT")
                : base(pixelSpawner, x, y, value)
    {
        textInside = text;
    }
    public override void Interactions()
    {
        AudioManager.PlaySound("menuButton", 1f);
        QuitGame();
    }
}
internal class SettingsCard : MenuCard
{
    public SettingsCard(PixelSpawner pixelSpawner, int x, int y, string value = "", string text = "SETTINGS")
                : base(pixelSpawner, x, y, value)
    {
        textInside = text;
    }
    public override void Interactions()
    {
        AudioManager.PlaySound("menuButton", 1f);
        nowLocation = location.Settings;
        PixelManager.NextLevl();
    }
}
internal class ContinueCard : MenuCard
{
    public ContinueCard(PixelSpawner pixelSpawner, int x, int y, string value = "", string text = "CONTINUE")
                : base(pixelSpawner, x, y, value)
    {
        textInside = text;
    }
    public override void Interactions()
    {
        if (File.Exists(Application.persistentDataPath + "/saves/data.json"))
        {
            nowLocation = location.LoadSave;
            PixelManager.NextLevl();
        }

    }
    public override void VisibleRender(int? limit = null)
    {
        if (File.Exists(Application.persistentDataPath + "/saves/data.json")) DrawBorder(Color.HSVToRGB(300f / 360f, 1f, 0.5f));
        else DrawBorder(Color.HSVToRGB(0f, 0f, 0.5f));
    }
}
internal class ToMenuCard : MenuCard
{
    public ToMenuCard(PixelSpawner pixelSpawner, int x, int y, string value = "", string text = "GO TO MENU")
                : base(pixelSpawner, x, y, value)
    {
        textInside = text;
        Weight = 56;
    }
    public override void Interactions()
    {
        AudioManager.PlaySound("menuButton", 1f);
        nowLocation = location.Menu;
        PixelManager.NextLevl();
    }
}

internal class VolumeCard : MenuCard
{
    public VolumeCard(PixelSpawner pixelSpawner, int x, int y, string value = "", string text = "+", bool plus = true)
                : base(pixelSpawner, x, y, value)
    {
        Hight = 8;
        Weight = 8;
        if (!plus) textInside = "-";
        else textInside = text;
        Plus = plus;
    }
    private bool Plus { get; set; }

    public override void Interactions()
    {
        AudioManager.PlaySound("menuButton", 1f);
        if (Plus && volume < 14)
        {            
            AddNewPolosochka();
            volume += 1;
        }
        else if (!Plus && volume > 0)
        {
            volume -= 1;
            ClaerValumeHashSet(valumeText.Last());

        }
        AudioManager.UpdateAllSoundVolumes();        

    }
    public void ClaerValumeHashSet(HashSet<Pixel> hashSet)
    {
        foreach (Pixel pixel in hashSet)
        {
            PixelSpawner.DestroyObject(pixel.gameObject);
        }
        hashSet.Clear();
        PixelSpawner.valumeText.Remove(PixelSpawner.valumeText.Last());
    }
    public void AddNewPolosochka()
    {
        HashSet<Pixel> polosochka = new HashSet<Pixel>();
        PixelSpawner.valumeText.Add(polosochka);
        PixelSpawner.text.DrawText(PixelManager, "_", 80 + (int)(PixelSpawner.volume * 5), 80, Color.gray, 1, polosochka);
    }

}
internal class StatisticCard : MenuCard
{
    public StatisticCard(PixelSpawner pixelSpawner, int x, int y, string value = "", string text = "Statistics")
                : base(pixelSpawner, x, y, value)
    {
        textInside = text;
        Weight = 60;
    }
    public override void Interactions()
    {
        AudioManager.PlaySound("menuButton", 1f);
        nowLocation = location.Ststistics;
        PixelManager.NextLevl();

    }
}

internal class MouseControl : MenuCard
{
    public MouseControl(PixelSpawner pixelSpawner, int x, int y, string value = "", string text = "Mouse")
                : base(pixelSpawner, x, y, value)
    {
        textInside = text;
        Weight = 30;
    }
    public override void Interactions()
    {
        AudioManager.PlaySound("menuButton", 1f);
        controlModNow = controlMod.Mouse;
    }
}
internal class KeyboardControl : MenuCard
{
    public KeyboardControl(PixelSpawner pixelSpawner, int x, int y, string value = "", string text = "Keyboard")
                : base(pixelSpawner, x, y, value)
    {
        textInside = text;
        Weight = 45;
    }
    public override void Interactions()
    {
        AudioManager.PlaySound("menuButton", 1f);
        controlModNow = controlMod.Keyboard;
    }
}

internal class Editor : MenuCard
{
    public Editor(PixelSpawner pixelSpawner, int x, int y, string value = "", string text = "Editor")
                : base(pixelSpawner, x, y, value)
    {
        textInside = text;
        Weight = 45;
    }
    public override void Interactions()
    {
        AudioManager.PlaySound("menuButton", 1f);
        nowLocation = location.Editor;
        PixelManager.NextLevl();
    }
}
=======
using System.Collections.Generic;
using static PixelSpawner;
using UnityEngine;
using System.IO;
using System.Linq;

internal class MenuCard : Card
{
    public MenuCard(PixelSpawner pixelSpawner, int x, int y, string value = "", string text = "XXXXX")
                : base(pixelSpawner, x, y, value)
    {
        filePath = "Old_Sptites/0.txt";
        Hight = 10;
        Weight = 51;
        textInside = text;
    }
    public string textInside { get; set; }
    public HashSet<Pixel> textHashSet { get; set; }

    public override void VisibleRender(int? limit = null)
    {
        DrawBorder(Color.HSVToRGB(300f / 360f, 1f, 0.5f), null);
    }
    public void AddMenuCard(int offsetX = 0)
    {
        this.actionsForCard = actions.Visible;
        this.VisibleRender();
        this.Enabled = true;
        string key = (this.Xmain + 2).ToString() + "; " + (this.Ymain + 4).ToString();
        PixelSpawner.Cards.Add(key, this);
        PixelSpawner.OnlyCards.Add(this);
        DrawText(Color.HSVToRGB(60f / 360f, 1f, 0.5f), offsetX);
    }
    public void DrawText(Color color, int offsetX = 0)
    {
        PixelSpawner.text.DrawText(PixelManager, textInside, Xmain + (Weight / 2) - (textInside.Length * 5) / 2 + offsetX, (Ymain + (Hight / 2) - 3), color, 1, PixelManager.AllText, -1, CenterPoint);
        MyAnimator.SetStartInfo();
    }
}
internal class StartCard : MenuCard
{
    public StartCard(PixelSpawner pixelSpawner, int x, int y, string value = "", string text = "START")
                : base(pixelSpawner, x, y, value)
    {
        textInside = text;
    }

    public override void Interactions()
    {
        AudioManager.StopBackgroundMusic();
        AudioManager.PlaySound("startGame", 1f);
        nowLocation = location.Dungon;
        stats.MyStatistics["CountOfRuns"]++;
        PixelSpawner.Inventory.CleenAll();
        PixelSpawner.Inventory.CardInventory[0].Clear();
        PixelSpawner.Inventory.CardInventory[1].Clear();
        PixelSpawner.Inventory.CardInventory[2].Clear();
        dungenLevel = 0;
        coin.Value = 30;
        countKillcard = 10;
        coin.DrowCoin();
        coin.DrowValue();
        PixelManager.NextLevl();

    }
}
internal class ReStartCard : StartCard
{
    public ReStartCard(PixelSpawner pixelSpawner, int x, int y, string value = "", string text = "RESTART")
                : base(pixelSpawner, x, y, value)
    {
        textInside = text;
    }
}
internal class ExitCard : MenuCard
{
    public ExitCard(PixelSpawner pixelSpawner, int x, int y, string value = "", string text = "EXIT")
                : base(pixelSpawner, x, y, value)
    {
        textInside = text;
    }
    public override void Interactions()
    {
        AudioManager.PlaySound("menuButton", 1f);
        QuitGame();
    }
}
internal class SettingsCard : MenuCard
{
    public SettingsCard(PixelSpawner pixelSpawner, int x, int y, string value = "", string text = "SETTINGS")
                : base(pixelSpawner, x, y, value)
    {
        textInside = text;
    }
    public override void Interactions()
    {
        AudioManager.PlaySound("menuButton", 1f);
        nowLocation = location.Settings;
        PixelManager.NextLevl();
    }
}
internal class ContinueCard : MenuCard
{
    public ContinueCard(PixelSpawner pixelSpawner, int x, int y, string value = "", string text = "CONTINUE")
                : base(pixelSpawner, x, y, value)
    {
        textInside = text;
    }
    public override void Interactions()
    {
        if (File.Exists(Application.persistentDataPath + "/saves/data.json"))
        {
            nowLocation = location.LoadSave;
            PixelManager.NextLevl();
        }

    }
    public override void VisibleRender(int? limit = null)
    {
        if (File.Exists(Application.persistentDataPath + "/saves/data.json")) DrawBorder(Color.HSVToRGB(300f / 360f, 1f, 0.5f));
        else DrawBorder(Color.HSVToRGB(0f, 0f, 0.5f));
    }
}
internal class ToMenuCard : MenuCard
{
    public ToMenuCard(PixelSpawner pixelSpawner, int x, int y, string value = "", string text = "GO TO MENU")
                : base(pixelSpawner, x, y, value)
    {
        textInside = text;
        Weight = 56;
    }
    public override void Interactions()
    {
        AudioManager.PlaySound("menuButton", 1f);
        nowLocation = location.Menu;
        PixelManager.NextLevl();
    }
}

internal class VolumeCard : MenuCard
{
    public VolumeCard(PixelSpawner pixelSpawner, int x, int y, string value = "", string text = "+", bool plus = true)
                : base(pixelSpawner, x, y, value)
    {
        Hight = 8;
        Weight = 8;
        if (!plus) textInside = "-";
        else textInside = text;
        Plus = plus;
    }
    private bool Plus { get; set; }

    public override void Interactions()
    {
        AudioManager.PlaySound("menuButton", 1f);
        if (Plus && volume < 14)
        {            
            AddNewPolosochka();
            volume += 1;
        }
        else if (!Plus && volume > 0)
        {
            volume -= 1;
            ClaerValumeHashSet(valumeText.Last());

        }
        AudioManager.UpdateAllSoundVolumes();        

    }
    public void ClaerValumeHashSet(HashSet<Pixel> hashSet)
    {
        foreach (Pixel pixel in hashSet)
        {
            PixelSpawner.DestroyObject(pixel.gameObject);
        }
        hashSet.Clear();
        PixelSpawner.valumeText.Remove(PixelSpawner.valumeText.Last());
    }
    public void AddNewPolosochka()
    {
        HashSet<Pixel> polosochka = new HashSet<Pixel>();
        PixelSpawner.valumeText.Add(polosochka);
        PixelSpawner.text.DrawText(PixelManager, "_", 80 + (int)(PixelSpawner.volume * 5), 80, Color.gray, 1, polosochka);
    }

}
internal class StatisticCard : MenuCard
{
    public StatisticCard(PixelSpawner pixelSpawner, int x, int y, string value = "", string text = "Statistics")
                : base(pixelSpawner, x, y, value)
    {
        textInside = text;
        Weight = 60;
    }
    public override void Interactions()
    {
        AudioManager.PlaySound("menuButton", 1f);
        nowLocation = location.Ststistics;
        PixelManager.NextLevl();

    }
}

internal class MouseControl : MenuCard
{
    public MouseControl(PixelSpawner pixelSpawner, int x, int y, string value = "", string text = "Mouse")
                : base(pixelSpawner, x, y, value)
    {
        textInside = text;
        Weight = 30;
    }
    public override void Interactions()
    {
        AudioManager.PlaySound("menuButton", 1f);
        controlModNow = controlMod.Mouse;
    }
}
internal class KeyboardControl : MenuCard
{
    public KeyboardControl(PixelSpawner pixelSpawner, int x, int y, string value = "", string text = "Keyboard")
                : base(pixelSpawner, x, y, value)
    {
        textInside = text;
        Weight = 45;
    }
    public override void Interactions()
    {
        AudioManager.PlaySound("menuButton", 1f);
        controlModNow = controlMod.Keyboard;
    }
}

internal class Editor : MenuCard
{
    public Editor(PixelSpawner pixelSpawner, int x, int y, string value = "", string text = "Editor")
                : base(pixelSpawner, x, y, value)
    {
        textInside = text;
        Weight = 45;
    }
    public override void Interactions()
    {
        AudioManager.PlaySound("menuButton", 1f);
        nowLocation = location.Editor;
        PixelManager.NextLevl();
    }
}
>>>>>>> 12fa110b21928b31eb4877f8d5b5df0b12480307

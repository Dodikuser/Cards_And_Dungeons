using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using System.Linq;

[Serializable]
public class Arrmy : Card
{
    public Arrmy(PixelSpawner pixelSpawner, int x, int y, string value = "")
                : base(pixelSpawner, x, y, value)
    {      
        int randomCard;

        if (Convert.ToInt32(value) <= 5)
        {
            randomCard = PixelSpawner.random.Next(0, (int)PixelSpawner.listOfArrmySpritsPower1.LongCount());
            filePath = PixelSpawner.listOfArrmySpritsPower1[randomCard];
        }
        else if (Convert.ToInt32(value) > 5)
        {
            randomCard = PixelSpawner.random.Next(0, (int)PixelSpawner.listOfArrmySpritsPower2.LongCount());
            filePath = PixelSpawner.listOfArrmySpritsPower2[randomCard];
        }
    }

    public override void VisibleRender(int? limit = null)
    {
        DrawBorder(Color.HSVToRGB(60f / 360f, 1f, 0.5f), limit);
    }

    public override void Interactions()
    {

        if (actionsForCard == actions.Hide)
        {
            if (PixelSpawner.animationEnable) MyAnimator.StartCoroutine(MyAnimator.ShowAnimation());
            else
            {
                actionsForCard = actions.Visible;
                DrawCard();
            }
            PixelManager.SelectNear();
            if (PixelSpawner.nowLocation != PixelSpawner.location.Chest) AudioManager.PlaySound("openLine", 1f);
        }
        else if (actionsForCard == actions.Visible && !PixelSpawner.Inventory.CardInventory[1].Contains(this))
        {
            int thisValue = Convert.ToInt32(Value);

            if (PixelSpawner.Inventory.CardInventory[1].Count == 0)
            {
                PixelSpawner.Inventory.AddCard(1, this);
                if (PixelSpawner.controlModNow == PixelSpawner.controlMod.Keyboard) PixelSpawner.FindCard(PixelSpawner.axes.Up, true, false, true);
                AudioManager.PlaySound("army", 1f);
                PixelSpawner.stats.MyStatistics["CountOfPickUp"]++;
            }
            else
            {
                int lastValue = Convert.ToInt32(PixelSpawner.Inventory.CardInventory[1][PixelSpawner.Inventory.CardInventory[1].Count - 1].Value);

                if (lastValue + 1 == thisValue || lastValue + 2 == thisValue)
                {
                    PixelSpawner.Inventory.AddCard(1, this);
                    if (PixelSpawner.controlModNow == PixelSpawner.controlMod.Keyboard) PixelSpawner.FindCard(PixelSpawner.axes.Up, true, false, true);
                    AudioManager.PlaySound("army", 1f);
                    PixelSpawner.stats.MyStatistics["CountOfPickUp"]++;
                }
                else AudioManager.PlaySound("error", 1f);
            }
        }
    }
}

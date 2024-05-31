using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using System.Linq;

[Serializable]
public class HP : Card
{
    public HP(PixelSpawner pixelSpawner, int x, int y, string value = "")
                : base(pixelSpawner, x, y, value)
    {      
        int randomCard = PixelSpawner.random.Next(0, (int)PixelSpawner.listOfHpSprits.LongCount());
        filePath = PixelSpawner.listOfHpSprits[randomCard];
    }

    public override void VisibleRender(int? limit = null)
    {
        DrawBorder(Color.HSVToRGB(0f, 1f, 0.5f), limit);
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
        else if (actionsForCard == actions.Visible && !PixelSpawner.Inventory.CardInventory[0].Contains(this))
        {
            int thisValue = Convert.ToInt32(Value);
            if (PixelSpawner.Inventory.CardInventory[0].Count == 0)
            {
                PixelSpawner.Inventory.AddCard(0, this);
                if (PixelSpawner.controlModNow == PixelSpawner.controlMod.Keyboard) PixelSpawner.FindCard(PixelSpawner.axes.Up, true, false, true);
                AudioManager.PlaySound("hp", 1f);
                PixelSpawner.stats.MyStatistics["CountOfPickUp"]++;
            }
            else
            {
                int lastValue = Convert.ToInt32(PixelSpawner.Inventory.CardInventory[0][PixelSpawner.Inventory.CardInventory[0].Count - 1].Value);

                if (lastValue + 1 == thisValue || lastValue - 1 == thisValue)
                {
                    PixelSpawner.Inventory.AddCard(0, this);
                    if (PixelSpawner.controlModNow == PixelSpawner.controlMod.Keyboard) PixelSpawner.FindCard(PixelSpawner.axes.Up, true, false, true);
                    AudioManager.PlaySound("hp", 1f);
                    PixelSpawner.stats.MyStatistics["CountOfPickUp"]++;
                }
                else AudioManager.PlaySound("error", 1f);
            }
        }
    }
}

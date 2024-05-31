using UnityEngine;

public class Door : Card
{
    public Door(PixelSpawner pixelSpawner, int x, int y, string value = "")
                : base(pixelSpawner, x, y, value)
    {
        filePath = "Old_Sptites/door.txt";
    }
    public override void VisibleRender(int? limit = null)
    {
        DrawBorder(Color.HSVToRGB(120f / 360f, 1f, 0.5f));
    }

    public override void Interactions()
    {

        if (actionsForCard == actions.Hide)
        {
            actionsForCard = actions.Visible;
            DrawCard();
            PixelManager.SelectNear();
        }
        else if (actionsForCard == actions.Visible)
        {           
            //enabletToSelect = false;
            if (PixelSpawner.nowLocation == PixelSpawner.location.Shop) PixelSpawner.nowLocation = PixelSpawner.location.Chest;
            //UnSelect(PixelSpawner.unSelectColor);
            AudioManager.StopBackgroundMusic();
            AudioManager.PlaySound("next", 1f);
            PixelManager.NextLevl();
        }

    }
}

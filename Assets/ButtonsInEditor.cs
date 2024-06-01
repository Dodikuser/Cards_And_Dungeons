<<<<<<< HEAD
﻿using System;
using UnityEngine;


internal class EditorButton : ToMenuCard
{
    public EditorButton(PixelSpawner pixelSpawner, int x, int y, string value = "", string text = "")
                 : base(pixelSpawner, x, y, value)
    {      
    }    
    public override void DrawFrame(Color color, bool select = false)
    {
        Repaint(BorderPixels, color);
    }
}

internal class Escape : EditorButton
{
    public Escape(PixelSpawner pixelSpawner, int x, int y, string value = "", string text = "Escape")
                 : base(pixelSpawner, x, y, value)
    {
        textInside = text;
        Weight = 35;
        Hight = 8;
    }
    public override void Interactions()
    {
        Vector3 cameraPoistion = new Vector3(98.21f, 56.9f, -109.9f);
        PixelManager.SinglAnimation(PixelManager.Camera.gameObject, cameraPoistion, Quaternion.identity, 0.5f);
        PixelSpawner.MySettings.ClearSettings(PixelSpawner.MySettings.editorObjects);
        base.Interactions();
    }   
}
internal class SaveButton : EditorButton
{
    public SaveButton(PixelSpawner pixelSpawner, int x, int y, string value = "", string text = "save")
                 : base(pixelSpawner, x, y, value)
    {
        textInside = text;
        Weight = 35;
        Hight = 8;
    }
    public override void Interactions()
    {
        PixelSpawner.MyEditorCard.Save("");
    }
}
internal class LoadButton : EditorButton
{
    public LoadButton(PixelSpawner pixelSpawner, int x, int y, string value = "", string text = "load")
                 : base(pixelSpawner, x, y, value)
    {
        textInside = text;
        Weight = 35;
        Hight = 8;
    }
    public override void Interactions()
    {
        Color[,] temp = EditorCard.Load("");
        if (temp != null)
        {
            PixelSpawner.MyEditorCard.ClearCanceledSaves();
            Array.Copy(temp, PixelSpawner.MyEditorCard.colors, temp.Length);
            PixelSpawner.MyEditorCard.DrawField();
            PixelSpawner.MyEditorCard.AddSave();
        }
    }
}
internal class ClearButton : EditorButton
{
    public ClearButton(PixelSpawner pixelSpawner, int x, int y, string value = "", string text = "clear")
                 : base(pixelSpawner, x, y, value)
    {
        textInside = text;
        Weight = 35;
        Hight = 8;
    }
    public override void Interactions()
    {
        PixelSpawner.MyEditorCard.ClearOrFill();
        PixelSpawner.MyEditorCard.DrawField();
    }
}
internal class ModButton : EditorButton
{
    public Color signatureColor = Color.yellow;

    public ModButton(PixelSpawner pixelSpawner, int x, int y, string value = "", string text = "")
                 : base(pixelSpawner, x, y, value)
    {
        textInside = text;
        Weight = 8;
        Hight = 8;
    }
    public override void Interactions()
    {
        foreach (Card button in PixelSpawner.OnlyCards)
        {
            if (button is ModButton)
            {
                ModButton modButton = (ModButton)button;
                modButton.signatureColor = Color.yellow;
                modButton.VisibleRender();              
            }
        }
       signatureColor = PixelSpawner.selectColor;
    }
    public override void VisibleRender(int? limit = null)
    {
        DrawBorder(signatureColor, null);
    }
}
internal class DefaultBrash : ModButton
{   

    public DefaultBrash(PixelSpawner pixelSpawner, int x, int y, string value = "", string text = "✎")
                 : base(pixelSpawner, x, y, value)
    {
        textInside = text;
        Weight = 8;
        Hight = 8;
    }
    public override void Interactions()
    {
        base.Interactions();
        PixelSpawner.MySettings.editorMod = EditorSettings.ToolMods.defaulBrash;
    }
    
}
internal class Filling : ModButton
{

    public Filling(PixelSpawner pixelSpawner, int x, int y, string value = "", string text = "♜")
                 : base(pixelSpawner, x, y, value)
    {
        textInside = text;
        Weight = 8;
        Hight = 8;
    }
    public override void Interactions()
    {
        base.Interactions();
        PixelSpawner.MySettings.editorMod = EditorSettings.ToolMods.filling;
    }

}
internal class Pipette : ModButton
{

    public Pipette(PixelSpawner pixelSpawner, int x, int y, string value = "", string text = "✑")
                 : base(pixelSpawner, x, y, value)
    {
        textInside = text;
        Weight = 8;
        Hight = 8;
    }
    public override void Interactions()
    {
        base.Interactions();
        PixelSpawner.MySettings.editorMod = EditorSettings.ToolMods.pipette;
    }

}
internal class Cancel : ModButton
{

    public Cancel(PixelSpawner pixelSpawner, int x, int y, string value = "", string text = "⤴")
                 : base(pixelSpawner, x, y, value)
    {
        textInside = text;
        Weight = 8;
        Hight = 8;
    }
    public override void Interactions()
    {        
        PixelSpawner.MyEditorCard.Cancel();
    }

}
internal class Return : ModButton
{

    public Return(PixelSpawner pixelSpawner, int x, int y, string value = "", string text = "⤵")
                 : base(pixelSpawner, x, y, value)
    {
        textInside = text;
        Weight = 8;
        Hight = 8;
    }
    public override void Interactions()
    {        
        PixelSpawner.MyEditorCard.Return();
    }

=======
﻿using System;
using UnityEngine;


internal class EditorButton : ToMenuCard
{
    public EditorButton(PixelSpawner pixelSpawner, int x, int y, string value = "", string text = "")
                 : base(pixelSpawner, x, y, value)
    {      
    }    
    public override void DrawFrame(Color color, bool select = false)
    {
        Repaint(BorderPixels, color);
    }
}

internal class Escape : EditorButton
{
    public Escape(PixelSpawner pixelSpawner, int x, int y, string value = "", string text = "Escape")
                 : base(pixelSpawner, x, y, value)
    {
        textInside = text;
        Weight = 35;
        Hight = 8;
    }
    public override void Interactions()
    {
        Vector3 cameraPoistion = new Vector3(98.21f, 56.9f, -109.9f);
        PixelManager.SinglAnimation(PixelManager.Camera.gameObject, cameraPoistion, Quaternion.identity, 0.5f);
        PixelSpawner.MySettings.ClearSettings(PixelSpawner.MySettings.editorObjects);
        base.Interactions();
    }   
}
internal class SaveButton : EditorButton
{
    public SaveButton(PixelSpawner pixelSpawner, int x, int y, string value = "", string text = "save")
                 : base(pixelSpawner, x, y, value)
    {
        textInside = text;
        Weight = 35;
        Hight = 8;
    }
    public override void Interactions()
    {
        PixelSpawner.MyEditorCard.Save("");
    }
}
internal class LoadButton : EditorButton
{
    public LoadButton(PixelSpawner pixelSpawner, int x, int y, string value = "", string text = "load")
                 : base(pixelSpawner, x, y, value)
    {
        textInside = text;
        Weight = 35;
        Hight = 8;
    }
    public override void Interactions()
    {
        Color[,] temp = EditorCard.Load("");
        if (temp != null)
        {
            PixelSpawner.MyEditorCard.ClearCanceledSaves();
            Array.Copy(temp, PixelSpawner.MyEditorCard.colors, temp.Length);
            PixelSpawner.MyEditorCard.DrawField();
            PixelSpawner.MyEditorCard.AddSave();
        }
    }
}
internal class ClearButton : EditorButton
{
    public ClearButton(PixelSpawner pixelSpawner, int x, int y, string value = "", string text = "clear")
                 : base(pixelSpawner, x, y, value)
    {
        textInside = text;
        Weight = 35;
        Hight = 8;
    }
    public override void Interactions()
    {
        PixelSpawner.MyEditorCard.ClearOrFill();
        PixelSpawner.MyEditorCard.DrawField();
    }
}
internal class ModButton : EditorButton
{
    public Color signatureColor = Color.yellow;

    public ModButton(PixelSpawner pixelSpawner, int x, int y, string value = "", string text = "")
                 : base(pixelSpawner, x, y, value)
    {
        textInside = text;
        Weight = 8;
        Hight = 8;
    }
    public override void Interactions()
    {
        foreach (Card button in PixelSpawner.OnlyCards)
        {
            if (button is ModButton)
            {
                ModButton modButton = (ModButton)button;
                modButton.signatureColor = Color.yellow;
                modButton.VisibleRender();              
            }
        }
       signatureColor = PixelSpawner.selectColor;
    }
    public override void VisibleRender(int? limit = null)
    {
        DrawBorder(signatureColor, null);
    }
}
internal class DefaultBrash : ModButton
{   

    public DefaultBrash(PixelSpawner pixelSpawner, int x, int y, string value = "", string text = "✎")
                 : base(pixelSpawner, x, y, value)
    {
        textInside = text;
        Weight = 8;
        Hight = 8;
    }
    public override void Interactions()
    {
        base.Interactions();
        PixelSpawner.MySettings.editorMod = EditorSettings.ToolMods.defaulBrash;
    }
    
}
internal class Filling : ModButton
{

    public Filling(PixelSpawner pixelSpawner, int x, int y, string value = "", string text = "♜")
                 : base(pixelSpawner, x, y, value)
    {
        textInside = text;
        Weight = 8;
        Hight = 8;
    }
    public override void Interactions()
    {
        base.Interactions();
        PixelSpawner.MySettings.editorMod = EditorSettings.ToolMods.filling;
    }

}
internal class Pipette : ModButton
{

    public Pipette(PixelSpawner pixelSpawner, int x, int y, string value = "", string text = "✑")
                 : base(pixelSpawner, x, y, value)
    {
        textInside = text;
        Weight = 8;
        Hight = 8;
    }
    public override void Interactions()
    {
        base.Interactions();
        PixelSpawner.MySettings.editorMod = EditorSettings.ToolMods.pipette;
    }

}
internal class Cancel : ModButton
{

    public Cancel(PixelSpawner pixelSpawner, int x, int y, string value = "", string text = "⤴")
                 : base(pixelSpawner, x, y, value)
    {
        textInside = text;
        Weight = 8;
        Hight = 8;
    }
    public override void Interactions()
    {        
        PixelSpawner.MyEditorCard.Cancel();
    }

}
internal class Return : ModButton
{

    public Return(PixelSpawner pixelSpawner, int x, int y, string value = "", string text = "⤵")
                 : base(pixelSpawner, x, y, value)
    {
        textInside = text;
        Weight = 8;
        Hight = 8;
    }
    public override void Interactions()
    {        
        PixelSpawner.MyEditorCard.Return();
    }

>>>>>>> 12fa110b21928b31eb4877f8d5b5df0b12480307
}
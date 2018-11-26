using Godot;
using System.Collections.Generic;

public class Crosshairs: Control
{
    Dictionary<string, TextureRect> crosshairsNames = new Dictionary<string, TextureRect>();

    public override void _Ready()
    {
        Godot.Collections.Array crosshairsList = this.GetChildren();
        for(int i = 0, leni = crosshairsList.Count; i < leni; i++)
        {
            TextureRect crosshair = ((TextureRect)crosshairsList[i]);
            crosshairsNames.Add(crosshair.GetName(), crosshair);
        }
    }

    public void HideAll(bool excludeBase = false)
    {
        foreach(KeyValuePair<string, TextureRect> crosshair in crosshairsNames)
        {
            if(excludeBase == false || crosshair.Key != "Base")
            {
                crosshair.Value.Hide();
            }
        }
    }

    public void ChangeCrosshair(string crosshairName)
    {
        HideAll(true);
        if(crosshairName.Contains("Knife") == false)
        {
            crosshairsNames["Base"].Show();
        }
        if(crosshairsNames.ContainsKey(crosshairName))
        {
            crosshairsNames[crosshairName].Show();
        }
    }

    public void ShowCrosshair(string crosshairName)
    {
        if(crosshairsNames.ContainsKey(crosshairName))
        {
            crosshairsNames[crosshairName].Show();
        }
    }

    public void HideCrosshair(string crosshairName)
    {
        if(crosshairsNames.ContainsKey(crosshairName))
        {
            crosshairsNames[crosshairName].Hide();
        }
    }
}
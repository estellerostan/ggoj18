using Godot;

public class Menu_timer: Timer
{
    bool lightAsc = true;

    private void _OnMenuTimer_timeout()
    {
        SpotLight spotFront = (SpotLight)GetParent().GetNode("Spot_front");
        Color spotColor = spotFront.GetColor();

        if(lightAsc)
        {
            if(spotColor.v >= 1)
            {
                lightAsc = false;
            }
            else
            {
                spotColor.v += 0.005F;
            }
        }
        else
        {
            if(spotColor.v <= 0)
            {
                lightAsc = true;
            }
            else
            {
                spotColor.v -= 0.005F;
            }
        }

        spotFront.SetColor(spotColor);
    }
}
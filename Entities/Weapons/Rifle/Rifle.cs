using System.Collections.Generic;

public class Rifle: Weapon
{
    public Rifle()
    {
        DAMAGE = new Dictionary<string, int>() {
            {"head", 4}, {"body", 4}, {"leg", 4}
        };
        TYPE = "firearm";
    }
}

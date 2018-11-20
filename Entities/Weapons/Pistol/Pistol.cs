using System.Collections.Generic;

public class Pistol: Weapon
{
    public Pistol()
    {
        DAMAGE = new Dictionary<string, int>() {
            {"head", 15}, {"body", 15}, {"leg", 15}
        };
        TYPE = "firearm";
    }
}
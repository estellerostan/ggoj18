using System.Collections.Generic;

public class Pistol: Weapon
{
    public Pistol()
    {
        Dictionary<string, int> DAMAGE = new Dictionary<string, int>() {
            {"head", 15}, {"body", 15}, {"leg", 15}
        };
        TYPE = "firearm";
    }
}
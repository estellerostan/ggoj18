using System.Collections.Generic;

public class Knife: Weapon
{
    public Knife()
    {
        Dictionary<string, int> DAMAGE = new Dictionary<string, int>() {
            {"head", 0}, {"body", 40}, {"leg", 40}
        };
        TYPE = "melee";
    }
}
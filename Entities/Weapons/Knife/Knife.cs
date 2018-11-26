using System.Collections.Generic;

public class Knife: Weapon
{
    public override void _Ready()
    {
        InitWeapon();
        DAMAGE = new Dictionary<string, int>() {
            {"head", 0}, {"body", 32}, {"leg", 0}
        };
        TYPE = "melee";
        MAX_AMMO = 1;
    }
}
using System.Collections.Generic;

public class Rifle: Weapon
{
    public override void _Ready()
    {
        InitWeapon();
        DAMAGE = new Dictionary<string, int>() {
            {"head", 100}, {"body", 4}, {"leg", 2}
        };
        TYPE = "firearm";
        MAX_AMMO = 40;
    }
}

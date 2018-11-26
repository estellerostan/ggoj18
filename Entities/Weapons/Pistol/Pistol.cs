using System.Collections.Generic;

public class Pistol: Weapon
{
    public override void _Ready()
    {
        InitWeapon();
        DAMAGE = new Dictionary<string, int>() {
            {"head", 100}, {"body", 16}, {"leg", 8}
        };
        TYPE = "firearm";
        MAX_AMMO = 10;
    }
}
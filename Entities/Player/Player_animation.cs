using System.Collections.Generic;

public class Player_animation: Entity_animation
{
    public Player_animation()
    {
        STATES = new Dictionary<string, List<string>>() {
            {"Idle_unarmed", new List<string>{"Knife_equip", "Pistol_equip", "Rifle_equip", "Idle_unarmed"}},

            {"Pistol_equip", new List<string>{"Pistol_idle"}},
            {"Pistol_fire", new List<string>{"Pistol_idle"}},
            {"Pistol_idle", new List<string>{"Pistol_fire", "Pistol_reload", "Pistol_unequip", "Pistol_idle"}},
            {"Pistol_reload", new List<string>{"Pistol_idle"}},
            {"Pistol_unequip", new List<string>{"Idle_unarmed"}},

            {"Rifle_equip", new List<string>{"Rifle_idle"}},
            {"Rifle_fire", new List<string>{"Rifle_idle"}},
            {"Rifle_idle", new List<string>{"Rifle_fire", "Rifle_reload", "Rifle_unequip", "Rifle_idle"}},
            {"Rifle_reload", new List<string>{"Rifle_idle"}},
            {"Rifle_unequip", new List<string>{"Idle_unarmed"}},

            {"Knife_equip", new List<string>{"Knife_idle"}},
            {"Knife_fire", new List<string>{"Knife_idle"}},
            {"Knife_idle", new List<string>{"Knife_fire", "Knife_unequip", "Knife_idle"}},
            {"Knife_unequip", new List<string>{"Idle_unarmed"}}
        };

        ANIMATION_SPEED = new Dictionary<string, float>() {
            {"Idle_unarmed", 1},

            {"Pistol_equip", 1.4F},
            {"Pistol_fire", 1.8F},
            {"Pistol_idle", 1},
            {"Pistol_reload", 1},
            {"Pistol_unequip", 1.4F},

            {"Rifle_equip", 2},
            {"Rifle_fire", 6},
            {"Rifle_idle", 1},
            {"Rifle_reload", 1.45F},
            {"Rifle_unequip", 2},

            {"Knife_equip", 1},
            {"Knife_fire", 1.35F},
            {"Knife_idle", 1},
            {"Knife_unequip", 1}
        };
    }
}
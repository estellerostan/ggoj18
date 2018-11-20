using Godot;
using System.Collections.Generic;

public class Weapon: Spatial
{
    public Dictionary<string, int> DAMAGE = new Dictionary<string, int>() {
        {"head", 0}, {"body", 0}, {"legs", 0}
    };
    protected string TYPE = "firearm";

    Crosshairs crosshairs;
    public dynamic entityNode = null;

    public bool isWeaponEnabled = false;
    float crosshairTimer = -1;

    public override void _Ready()
    {
        crosshairs = (Crosshairs)GetNode("../../../HUD/Crosshairs");
    }

    public override void _Process(float delta)
    {
        if(crosshairTimer > -1)
        {
            if(crosshairTimer < 1)
            {
                crosshairTimer += delta;
            }
            else
            {
                crosshairTimer = -1;
                crosshairs.HideCrosshair(GetType().Name + "_hit");
            }
        }
    }

    public void AttackAction()
    {
        if(TYPE == "firearm")
        {
            RayCast ray = (RayCast)GetNode("Ray_cast");
            ray.ForceRaycastUpdate();

            if(ray.IsColliding())
            {
                dynamic body = ray.GetCollider();

                if(body.GetType().Name != entityNode.GetType().Name && body.HasMethod("AttackHit"))
                {
                    Godot.Collections.Array targetGroups = body.GetChild(ray.GetColliderShape()).GetGroups();

                    if(targetGroups.Count > 0)
                    {
                        crosshairs.ShowCrosshair(GetType().Name + "_hit");
                        body.AttackHit(DAMAGE[(string)targetGroups[0]], ray.GetGlobalTransform());
                    }
                }
            }
        }
        else
        {
            Area area = (Area)GetNode("Area");
            Godot.Collections.Array bodies = area.GetOverlappingBodies();

            foreach(dynamic body in bodies)
            {
                if(body.GetType().Name != entityNode.GetType().Name && body.HasMethod("AttackHit"))
                {
                    Godot.Collections.Array targetGroups = body.GetGroups(); // TODO: shape ?

                    if(targetGroups.Count > 0)
                    {
                        crosshairs.ShowCrosshair(GetType().Name + "_hit");
                        body.AttackHit(DAMAGE[(string)targetGroups[0]], area.GetGlobalTransform());
                    }
                }
            }
        }

        crosshairTimer = 0;
    }

    public bool EquipWeapon()
    {
        string weaponName = GetType().Name;

        if(entityNode.entityAnimation.currentState == weaponName + "_idle")
        {
            isWeaponEnabled = true;
            return true;
        }

        if(entityNode.entityAnimation.currentState == "Idle_unarmed")
        {
            entityNode.entityAnimation.SetAnimation(weaponName + "_equip");
            crosshairs.ChangeCrosshair(weaponName + "_idle");
        }

        return false;
    }

    public bool UnequipWeapon()
    {
        string weaponName = GetType().Name;

        if(entityNode.entityAnimation.currentState == weaponName + "_idle")
        {
            entityNode.entityAnimation.SetAnimation(weaponName + "_unequip");
            crosshairs.HideAll();
        }

        if(entityNode.entityAnimation.currentState == "Idle_unarmed")
        {
            isWeaponEnabled = false;
            return true;
        }

        return false;
    }
}

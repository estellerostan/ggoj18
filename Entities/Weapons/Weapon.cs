using Godot;
using System.Collections.Generic;

public class Weapon: Spatial
{
    protected Dictionary<string, int> DAMAGE = new Dictionary<string, int>() {
        {"head", 0}, {"body", 0}, {"legs", 0}
    };
    protected string TYPE = "firearm";

    Crosshairs crosshairs;
    public dynamic entityNode = null;

    public bool isWeaponEnabled = false;

    public override void _Ready()
    {
        crosshairs = (Crosshairs)GetNode("../../../HUD/Crosshairs");
        return;
    }

    public void AttackAction()
    {
        GD.Print("=============");
        if(TYPE == "firearm")
        {
            RayCast ray = (RayCast)GetNode("Ray_cast");
            ray.ForceRaycastUpdate();

            if(ray.IsColliding())
            {
                dynamic body = ray.GetCollider();
GD.Print(body.GetType().Name);
GD.Print(body.HasMethod("AttackHit"));
                if(body.GetType().Name != entityNode.GetType().Name && body.HasMethod("AttackHit"))
                {
                    GD.Print("/////////");
                    GD.Print(((Node)body).GetGroups());

                    crosshairs.ShowCrosshair(this.GetType().Name + "_hit");
                    body.AttackHit(DAMAGE[((string)((Node)body).GetGroups()[0])], ray.GetGlobalTransform());
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
                    crosshairs.ShowCrosshair(this.GetType().Name + "_hit");
                    body.AttackHit(DAMAGE[((string)((Node)body).GetGroups()[0])], area.GetTransform());
                }
            }
        }

        crosshairs.HideCrosshair(this.GetType().Name + "_hit");
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

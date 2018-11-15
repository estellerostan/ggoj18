using Godot;
using System.Collections.Generic;

public class Weapon: Spatial
{
    protected Dictionary<string, int> DAMAGE = new Dictionary<string, int>() {
        {"head", 0}, {"body", 0}, {"leg", 0}
    };
    protected string TYPE = "firearm";

    public Entity entityNode = null;

    public bool isWeaponEnabled = false;

    public override void _Ready()
    {
        return;
    }

    public void AttackAction()
    {
        GD.Print("Attack - TYPE: " + TYPE);
        if(TYPE == "firearm")
        {
            RayCast ray = (RayCast)GetNode("Ray_cast");
            ray.ForceRaycastUpdate();

            if(ray.IsColliding())
            {
                Object body = ray.GetCollider();

                if(body == entityNode)
                {
                    return;
                }
                else if(body.HasMethod("AttackHit"))
                {
                    ((Entity)body).AttackHit(DAMAGE, ray.GetGlobalTransform);
                }
            }
        }
        else
        {
            Area area = (Area)GetNode("Area");
            Godot.Collections.Array bodies = area.GetOverlappingBodies();

            foreach(PhysicsBody body in bodies)
            {
                if(body == entityNode)
                {
                    continue;
                }
                else if(body.HasMethod("AttackHit"))
                {
                    ((Entity)body).AttackHit(DAMAGE, area.GetGlobalTransform);
                }
            }
        }
    }

    public bool EquipWeapon()
    {
        string weaponName = this.GetType().Name;

        if(entityNode.entityAnimation.currentState == weaponName + "_idle")
        {
            isWeaponEnabled = true;
            return true;
        }

        if(entityNode.entityAnimation.currentState == "Idle_unarmed")
        {
            entityNode.entityAnimation.SetAnimation(weaponName + "_equip");
        }

        return false;
    }

    public bool UnequipWeapon()
    {
        string weaponName = this.GetType().Name;

        if(entityNode.entityAnimation.currentState == weaponName + "_idle")
        {
            if(entityNode.entityAnimation.currentState != weaponName + "_unequip")
            {
                entityNode.entityAnimation.SetAnimation(weaponName + "_unequip");
            }
        }

        if(entityNode.entityAnimation.currentState == "Idle_unarmed")
        {
            isWeaponEnabled = false;
            return true;
        }

        return false;
    }
}

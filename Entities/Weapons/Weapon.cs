using Godot;
using System.Collections.Generic;

public class Weapon: Spatial
{
    protected Dictionary<string, int> DAMAGE = new Dictionary<string, int>() {
        {"head", 0}, {"body", 0}, {"legs", 0}
    };
    public string TYPE = "melee";
    protected int MAX_AMMO = 0;

    Crosshairs crosshairs;
    public dynamic entityNode = null;

    public bool isWeaponEnabled = false;
    public bool canReload = true;
    public int loadedAmmo = 0;
    public int spareAmmo = 200;
    float crosshairTimer = -1;

    protected void InitWeapon()
    {
        if(HasNode("../../../HUD/Crosshairs"))
        {
            crosshairs = (Crosshairs)GetNode("../../../HUD/Crosshairs");
        }
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
        string entityName = entityNode.GetType().Name;
        string weaponName = GetType().Name;

        if(TYPE == "firearm")
        {
            RayCast ray = (RayCast)GetNode("Ray_cast");
            ray.ForceRaycastUpdate();

            loadedAmmo -= 1;

            if(ray.IsColliding())
            {
                dynamic body = ray.GetCollider();
                if(body.GetType().Name != entityName && body.HasMethod("AttackHit"))
                {
                    Godot.Collections.Array targetGroups = body.GetChild(ray.GetColliderShape()).GetGroups();
                    if(targetGroups.Count > 0)
                    {
                        string group = (string)targetGroups[0];
                        if(DAMAGE.ContainsKey(group))
                        {
                            if(entityName == "Player")
                            {
                                crosshairs.ShowCrosshair(weaponName + "_hit");
                            }
                            crosshairTimer = 0;
                            body.AttackHit(DAMAGE[group], ray.GetGlobalTransform());
                        }
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
                if(body.GetType().Name != entityName && body.HasMethod("AttackHit"))
                {
                    if(entityName == "Player")
                    {
                        crosshairs.ShowCrosshair(weaponName + "_hit");
                    }
                    crosshairTimer = 0;
                    body.AttackHit(DAMAGE["body"], area.GetGlobalTransform());
                }
            }
        }

        entityNode.CreateSound(weaponName + "_fire");
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
            if(entityNode.GetType().Name == "Player")
            {
                crosshairs.ChangeCrosshair(weaponName + "_idle");
            }
        }

        return false;
    }

    public bool UnequipWeapon()
    {
        string weaponName = GetType().Name;

        if(entityNode.entityAnimation.currentState == weaponName + "_idle")
        {
            entityNode.entityAnimation.SetAnimation(weaponName + "_unequip");
            if(entityNode.GetType().Name == "Player")
            {
                crosshairs.HideAll();
            }
        }

        if(entityNode.entityAnimation.currentState == "Idle_unarmed")
        {
            isWeaponEnabled = false;
            return true;
        }

        return false;
    }

    public bool ReloadWeapon()
    {
        string weaponName = GetType().Name;
        bool canReload = false;

        if(entityNode.entityAnimation.currentState == weaponName + "_idle")
        {
            canReload = true;
        }

        if(spareAmmo <= 0 || loadedAmmo == MAX_AMMO)
        {
            canReload = false;
        }

        if(canReload == true)
        {
            int ammoNeeded = MAX_AMMO - loadedAmmo;

            if(spareAmmo >= ammoNeeded)
            {
                spareAmmo -= ammoNeeded;
                loadedAmmo = MAX_AMMO;
            }
            else
            {
                loadedAmmo += spareAmmo;
                spareAmmo = 0;
            }

            entityNode.entityAnimation.SetAnimation(weaponName + "_reload");

            return true;
        }

        return false;
    }
}

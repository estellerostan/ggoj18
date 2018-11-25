using Godot;
using System.Collections.Generic;

public class Player: KinematicBody
{
    static float MOUSE_SENSITIVITY = 0.05F;
    static float GRAVITY = -24.8F;
    static float ACCELERATION = 4.5F;
    static int DEACCELERATION = 16;
    static int MAX_SPEED = 20;
    static int SPRINT_ACCEL = 18;
    static int MAX_SPRINT_SPEED = 30;
    static int JUMP_SPEED = 18;
    static List<string> WEAPON_NAME = new List<string> { "UNARMED", "KNIFE", "PISTOL", "RIFLE" };
    static Dictionary<string, int> WEAPON_NUMBER = new Dictionary<string, int>() {
        {"UNARMED", 0}, {"KNIFE", 1}, {"PISTOL", 2}, {"RIFLE", 3}
    };

    PackedScene audioPlayer = (PackedScene)GD.Load("res://Scenes/Common/AudioPlayer.tscn");
    Camera camera;
    Spatial rotationBodyTop;
    public Entity_animation entityAnimation;
    Label UIStatus;
    Dictionary<string, Weapon> weapons = new Dictionary<string, Weapon>() {
        {"UNARMED", null}, {"KNIFE", null}, {"PISTOL", null}, {"RIFLE", null}
    };
    SpotLight flashlight;

    string currentWeaponName = "UNARMED";
    string changingWeaponName = "UNARMED";
    bool changingWeapon = false;
    bool reloadingWeapon = false;

    Vector3 velocity = new Vector3();
    bool isSprinting = false;
    int health = 100;

    public override void _Ready()
    {
        Input.SetMouseMode(Input.MouseMode.Captured);

        camera = (Camera)GetNode("Rotation_body_top/Eyes");
        rotationBodyTop = (Spatial)GetNode("Rotation_body_top");
        entityAnimation = (Player_animation)GetNode("Rotation_body_top/Model/Player_animation");
        UIStatus = (Label)GetNode("HUD/Panel/Satus");
        flashlight = (SpotLight)GetNode("Rotation_body_top/Flashlight");

        entityAnimation.callbackFunction = GD.FuncRef(this, "Attack");

        weapons["KNIFE"] = (Knife)GetNode("Rotation_body_top/Weapons/Knife");
        weapons["PISTOL"] = (Pistol)GetNode("Rotation_body_top/Weapons/Pistol");
        weapons["RIFLE"] = (Rifle)GetNode("Rotation_body_top/Weapons/Rifle");

        Vector3 gunAimPos = ((Spatial)GetNode("Rotation_body_top/Weapons_aim")).GetGlobalTransform().origin;

        foreach(KeyValuePair<string, Weapon> weapon in weapons)
        {
            if(weapon.Value != null)
            {
                weapon.Value.entityNode = this;
                weapon.Value.LookAt(gunAimPos, new Vector3(0, 1, 0));
                weapon.Value.RotateObjectLocal(new Vector3(0, 1, 0), Mathf.Deg2Rad(180));
            }
        }
    }

    public override void _Input(InputEvent @event)
    {
        if(@event is InputEventMouseMotion && Input.GetMouseMode() == Input.MouseMode.Captured)
        {
            InputEventMouseMotion mouseMotion = (InputEventMouseMotion)@event;
            rotationBodyTop.RotateX(Mathf.Deg2Rad(mouseMotion.Relative.y * MOUSE_SENSITIVITY));
            RotateY(Mathf.Deg2Rad(mouseMotion.Relative.x * MOUSE_SENSITIVITY * -1));

            Vector3 cameraRotation = rotationBodyTop.GetRotationDegrees();
            cameraRotation.x = Mathf.Clamp(cameraRotation.x, -70, 70);
            rotationBodyTop.SetRotationDegrees(cameraRotation);
        }
    }

    public override void _PhysicsProcess(float delta)
    {
        Vector3 direction = ProcessInput();
        ProcessMovement(direction, delta);

        ProcessChangingWeapon();
        ProcessReloading();

        ProcessUI();
    }

    public void CreateSound(string soundName)
    {
        AudioPlayer audioClone = (AudioPlayer)audioPlayer.Instance();
        Node sceneRoot = (Node)GetTree().GetRoot().GetChildren()[0];
        sceneRoot.AddChild(audioClone);
        audioClone.PlaySound(soundName);
    }

    Vector3 ProcessInput()
    {
        Vector3 direction = new Vector3();
        Transform camXform = camera.GetGlobalTransform();

        Vector2 inputMovementVector = new Vector2();

        if(Input.IsActionPressed("movement_forward"))
        {
            inputMovementVector.y += 1;
        }
        if(Input.IsActionPressed("movement_backward"))
        {
            inputMovementVector.y -= 1;
        }
        if(Input.IsActionPressed("movement_left"))
        {
            inputMovementVector.x -= 1;
        }
        if(Input.IsActionPressed("movement_right"))
        {
            inputMovementVector.x += 1;
        }

        inputMovementVector = inputMovementVector.Normalized();

        direction += -camXform.basis.z.Normalized() * inputMovementVector.y;
        direction += camXform.basis.x.Normalized() * inputMovementVector.x;

        if(Input.IsActionPressed("movement_sprint"))
        {
            isSprinting = true;
        }
        else
        {
            isSprinting = false;
        }

        if(IsOnFloor() && Input.IsActionJustPressed("movement_jump"))
        {
            velocity.y = JUMP_SPEED;
        }

        int weaponChangeNumber = WEAPON_NUMBER[currentWeaponName];

        if(Input.IsKeyPressed((int)KeyList.Key1) || Input.IsActionPressed("Key1"))
        {
            weaponChangeNumber = 0;
        }
        if(Input.IsKeyPressed((int)KeyList.Key2) || Input.IsActionPressed("Key2"))
        {
            weaponChangeNumber = 1;
        }
        if(Input.IsKeyPressed((int)KeyList.Key3) || Input.IsActionPressed("Key3"))
        {
            weaponChangeNumber = 2;
        }
        if(Input.IsKeyPressed((int)KeyList.Key4) || Input.IsActionPressed("Key4"))
        {
            weaponChangeNumber = 3;
        }

        if(Input.IsActionJustPressed("shift_weapon_positive"))
        {
            weaponChangeNumber += 1;
        }
        if(Input.IsActionJustPressed("shift_weapon_negative"))
        {
            weaponChangeNumber -= 1;
        }

        weaponChangeNumber = Mathf.Clamp(weaponChangeNumber, 0, WEAPON_NAME.Count - 1);

        if(changingWeapon == false && reloadingWeapon == false && WEAPON_NAME[weaponChangeNumber] != currentWeaponName)
        {
            changingWeaponName = WEAPON_NAME[weaponChangeNumber];
            changingWeapon = true;
        }

        if(Input.IsActionPressed("attack") && reloadingWeapon == false && changingWeapon == false)
        {
            Weapon currentWeapon = weapons[currentWeaponName];
            if(currentWeapon != null && currentWeapon.loadedAmmo > 0)
            {
                string weaponName = currentWeapon.GetType().Name;

                if(entityAnimation.currentState == weaponName + "_idle")
                {
                    entityAnimation.SetAnimation(weaponName + "_fire");
                }
            }
        }

        if(Input.IsActionJustPressed("reload") && reloadingWeapon == false && changingWeapon == false)
        {
            Weapon currentWeapon = weapons[currentWeaponName];
            if(currentWeapon != null && currentWeapon.canReload == true && entityAnimation.currentState != currentWeapon.GetType().Name + "_reload")
            {
                reloadingWeapon = true;
            }
        }

        if(Input.IsActionJustPressed("flashlight"))
        {
            if(flashlight.IsVisibleInTree())
            {
                flashlight.Hide();
            }
            else
            {
                flashlight.Show();
            }
        }

        return direction;
    }

    void ProcessMovement(Vector3 direction, float delta)
    {
        direction.y = 0;
        direction = direction.Normalized();

        velocity.y += delta * GRAVITY;

        Vector3 horizontalVelocity = velocity;
        horizontalVelocity.y = 0;

        Vector3 target = direction;

        if(isSprinting)
        {
            target *= MAX_SPRINT_SPEED;
        }
        else
        {
            target *= MAX_SPEED;
        }

        float acceleration = DEACCELERATION;
        if(direction.Dot(horizontalVelocity) > 0)
        {
            if(isSprinting)
            {
                acceleration = SPRINT_ACCEL;
            }
            else
            {
                acceleration = ACCELERATION;
            }
        }

        horizontalVelocity = horizontalVelocity.LinearInterpolate(target, acceleration * delta);
        velocity.x = horizontalVelocity.x;
        velocity.z = horizontalVelocity.z;
        velocity = MoveAndSlideWithSnap(velocity, new Vector3(0, 0.05F, 0), new Vector3(0, 1, 0), false, false, 4, Mathf.Deg2Rad(40));
    }

    void ProcessChangingWeapon()
    {
        if(changingWeapon == true)
        {
            Weapon currentWeapon = weapons[currentWeaponName];
            bool weaponUnequipped = false;

            if(currentWeapon == null)
            {
                weaponUnequipped = true;
            }
            else
            {
                if(currentWeapon.isWeaponEnabled == true)
                {
                    weaponUnequipped = currentWeapon.UnequipWeapon();
                }
                else
                {
                    weaponUnequipped = true;
                }
            }

            if(weaponUnequipped == true)
            {
                Weapon weaponToEquip = weapons[changingWeaponName];
                bool weaponEquiped = false;

                if(weaponToEquip == null)
                {
                    weaponEquiped = true;
                }
                else
                {
                    if(weaponToEquip.isWeaponEnabled == false)
                    {
                        weaponEquiped = weaponToEquip.EquipWeapon();
                    }
                    else
                    {
                        weaponEquiped = true;
                    }
                }

                if(weaponEquiped == true)
                {
                    changingWeapon = false;
                    currentWeaponName = changingWeaponName;
                    changingWeaponName = "";
                }
            }
        }
    }

    void ProcessReloading()
    {
        if(reloadingWeapon == true)
        {
            Weapon currentWeapon = weapons[currentWeaponName];
            if(currentWeapon != null)
            {
                currentWeapon.ReloadWeapon();
            }
            reloadingWeapon = false;
        }
    }

    void ProcessUI()
    {
        Weapon currentWeapon = weapons[currentWeaponName];
        if(currentWeapon == null || currentWeapon.TYPE == "melee")
        {
            UIStatus.SetText("HEALTH: " + health);
        }
        else
        {
            UIStatus.SetText("HEALTH: " + health + "\nAMMO: " + currentWeapon.loadedAmmo + "/" + currentWeapon.spareAmmo);
        }
    }

    public void Attack()
    {
        if(changingWeapon == false)
        {
            weapons[currentWeaponName].AttackAction();
        }
    }

    public void AttackHit(Dictionary<string, int> damage, Transform getGlobalTransform)
    {
        return;
    }
}
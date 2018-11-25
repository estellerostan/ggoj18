using Godot;

public class Enemy: RigidBody
{
    int BASE_BULLET_BOOST = 9;

    public Entity_animation entityAnimation;
    protected Weapon weapon;
    Particles deathParticles;

    int health = 100;
    bool isDeath = false;

    protected void InitEnemy()
    {
        // entityAnimation = (Player_animation)GetNode("Rotation_body_top/Model/Player_animation");
        // entityAnimation.callbackFunction = GD.FuncRef(this, "Attack");
        deathParticles = (Particles)GetNode("Death_particles");
    }

    //public override void _Process(float delta)
    //{
    //}

    public override void _PhysicsProcess(float delta)
    {
        if(deathParticles != null)
        {
            if(deathParticles.IsEmitting())
            {
                isDeath = true;
            }
            else if(isDeath)
            {
                QueueFree();
            }
        }
    }

    public void Attack()
    {
        weapon.AttackAction();
    }

    public void AttackHit(int damage, Transform globalTransform)
    {
        Vector3 directionVector = globalTransform.basis.z.Normalized() * BASE_BULLET_BOOST;
        ApplyImpulse((globalTransform.origin - GetGlobalTransform().origin).Normalized(), directionVector * damage);

        health -= damage;
        GD.Print("health: " + health);

        if(health <= 0)
        {
            SetMode(ModeEnum.Static);
            SetRotation(new Vector3(0, 0, 0));
            deathParticles.Show();
            deathParticles.SetEmitting(true);

            Godot.Collections.Array childrens = GetChildren();

            foreach(dynamic child in childrens)
            {
                if(child.GetName() != "Death_particles")
                {
                    child.Hide();
                }
            }
        }
    }
}
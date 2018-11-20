using Godot;

public class Enemy: RigidBody
{
    int BASE_BULLET_BOOST = 9;

    int health = 100;

    public void AttackHit(int damage, Transform globalTransform)
    {
        Vector3 directionVector = globalTransform.basis.z.Normalized() * BASE_BULLET_BOOST;
        ApplyImpulse((globalTransform.origin - GetGlobalTransform().origin).Normalized(), directionVector * damage);

        health -= damage;
        GD.Print("health: " + health);

        if(health <= 0)
        {
            QueueFree();
        }
    }
}
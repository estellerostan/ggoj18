using Godot;
using System.Collections.Generic;

public class Enemy: RigidBody
{
    int BASE_BULLET_BOOST = 9;

    int health = 100;

    public void AttackHit(int damage, Transform globalTransform)
    {
        GD.Print("1111111");
        Vector3 directionVector = globalTransform.basis.z.Normalized() * BASE_BULLET_BOOST;
        GD.Print("2222222");
        ApplyImpulse((globalTransform.origin - GetGlobalTransform().origin).Normalized(), directionVector * damage);
        GD.Print("3333333");
    }
}
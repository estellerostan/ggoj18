using Godot;
using System.Collections.Generic;

public class Enemy: RigidBody
{
    int BASE_BULLET_BOOST = 9;

    public void AttackHit(Dictionary<string, int> damage, Transform globalTransform)
    {
        string bodyPart = "body"; // TODO: a faire
        Vector3 directionVector = globalTransform.basis.z.Normalized() * BASE_BULLET_BOOST;

        ApplyImpulse((globalTransform.origin - GetGlobalTransform().origin).Normalized(), directionVector * damage[bodyPart]);
    }
}
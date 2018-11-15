using Godot;
using System;
using System.Collections.Generic;

public class Entity: KinematicBody
{
    public Entity_animation entityAnimation;

    public void AttackHit(Dictionary<string, int> damage, Func<Transform> getGlobalTransform)
    {
        throw new NotImplementedException();
    }
}
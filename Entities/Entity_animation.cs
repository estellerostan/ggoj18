using Godot;
using System.Collections.Generic;

public class Entity_animation: AnimationPlayer
{
    protected Dictionary<string, List<string>> STATES;
    protected Dictionary<string, float> ANIMATION_SPEED;

    public string currentState = null;
    public FuncRef callbackFunction = null;

    public override void _Ready()
    {
        SetAnimation("Idle_unarmed");
    }

    public bool SetAnimation(string animationName)
    {
        if(animationName == currentState)
        {
            GD.PrintErr("Player_animation -- WARNING: animation is already ", animationName);
            return true;
        }

        if(HasAnimation(animationName) == true)
        {
            if(currentState != null)
            {
                if(STATES[currentState].Contains(animationName))
                {
                    currentState = animationName;
                    this.Play(animationName, -1, ANIMATION_SPEED[animationName]);
                    return true;
                }
                else
                {
                    GD.PrintErr("Player_animation -- WARNING: Cannot change to ", animationName, " from ", currentState);
                    return false;
                }
            }
            else
            {
                currentState = animationName;
                this.Play(animationName, -1, ANIMATION_SPEED[animationName]);
                return true;
            }
        }

        return false;
    }

    private void _OnPlayerAnimationAnimation_finished(string animName)
    {
        if(currentState == "Idle_unarmed")
        {
            return;
        }
        else if(currentState == "Knife_equip")
        {
            SetAnimation("Knife_idle");
        }
        else if(currentState == "Knife_idle")
        {
            return;
        }
        else if(currentState == "Knife_fire")
        {
            SetAnimation("Knife_idle");
        }
        else if(currentState == "Knife_unequip")
        {
            SetAnimation("Idle_unarmed");
        }
        else if(currentState == "Pistol_equip")
        {
            SetAnimation("Pistol_idle");
        }
        else if(currentState == "Pistol_idle")
        {
            return;
        }
        else if(currentState == "Pistol_fire")
        {
            SetAnimation("Pistol_idle");
        }
        else if(currentState == "Pistol_unequip")
        {
            SetAnimation("Idle_unarmed");
        }
        else if(currentState == "Pistol_reload")
        {
            SetAnimation("Pistol_idle");
        }
        else if(currentState == "Rifle_equip")
        {
            SetAnimation("Rifle_idle");
        }
        else if(currentState == "Rifle_idle")
        {
            return;
        }
        else if(currentState == "Rifle_fire")
        {
            SetAnimation("Rifle_idle");
        }
        else if(currentState == "Rifle_unequip")
        {
            SetAnimation("Idle_unarmed");
        }
        else if(currentState == "Rifle_reload")
        {
            SetAnimation("Rifle_idle");
        }
    }

    public void AnimationCallback()
    {
        if(callbackFunction == null)
        {
            GD.PrintErr("Player_animation -- WARNING: No callback function for the animation to call!");
        }
        else
        {
            GD.Print("callfunc");
            callbackFunction.CallFunc();
        }
    }
}

public class Enemy1: Enemy
{
    public override void _Ready()
    {
        InitEnemy();
        weapon = (Pistol)GetNode("Pistol");
    }
}
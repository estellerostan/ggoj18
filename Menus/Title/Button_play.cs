using Godot;

public class Button_play: Button
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    //Called when the node enters the scene tree for the first time.
    //public override void _Ready()
    //{

    //}

    //Called every frame. 'delta' is the elapsed time since the previous frame.
    //public override void _Process(float delta)
    //{

    //}

    private void _OnButtonPlay_pressed()
    {
        CallDeferred(nameof(DeferredGotoScene));
    }

    public void DeferredGotoScene()
    {
        Node oldScene = GetTree().GetCurrentScene();
        PackedScene nextScene = (PackedScene)GD.Load("res://Scenes/Level0/Level0.tscn");
        GetTree().ChangeSceneTo(nextScene);
        //GetTree().SetCurrentScene(nextScene.Instance());
        oldScene.Free();
    }
}
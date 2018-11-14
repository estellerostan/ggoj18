using Godot;

public class Global: Node
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    //public override void _Ready()
    //{

    //}

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if(Input.IsActionJustPressed("ui_cancel"))
        {
            //if (popup == null)
            //{
            //    popup = POPUP_SCENE.instance();

            //    popup.get_node("Button_quit").connect("pressed", self, "popup_quit");
            //    popup.connect("popup_hide", self, "popup_closed");
            //    popup.get_node("Button_resume").connect("pressed", self, "popup_closed");

            //    canvas_layer.add_child(popup);
            //    popup.popup_centered();

            //    Input.set_mouse_mode(Input.MOUSE_MODE_VISIBLE);

            //    get_tree().paused = true;
            //}
            GetTree().Quit();
        }
    }
}

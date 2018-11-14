using Godot;

public class Player: KinematicBody
{
    float MOUSE_SENSITIVITY = 0.05F;
    float GRAVITY = -24.8F;
    float ACCELERATION = 4.5F;
    int DEACCELERATION = 16;
    int MAX_SPEED = 20;
    int JUMP_SPEED = 18;

    Camera camera;
    Spatial rotationBodyTop;
    Vector3 velocity = new Vector3();


    public override void _Ready()
    {
        camera = (Camera)GetNode("Rotation_body_top/Eyes");
        rotationBodyTop = (Spatial)GetNode("Rotation_body_top");
        Input.SetMouseMode(Input.MouseMode.Captured);
    }

    public override void _PhysicsProcess(float delta)
    {
        Vector3 wishedForDirection = new Vector3();
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

        wishedForDirection += -camXform.basis.z.Normalized() * inputMovementVector.y;
        wishedForDirection += camXform.basis.x.Normalized() * inputMovementVector.x;

        if(IsOnFloor())
        {
            if(Input.IsActionJustPressed("movement_jump"))
            {
                velocity.y = JUMP_SPEED;
            }
        }

        wishedForDirection.y = 0;
        wishedForDirection = wishedForDirection.Normalized();

        velocity.y += delta * GRAVITY;

        Vector3 horizontalVelocity = velocity;
        horizontalVelocity.y = 0;

        Vector3 target = wishedForDirection * MAX_SPEED;

        float accel = DEACCELERATION;
        if(wishedForDirection.Dot(horizontalVelocity) > 0)
        {
            accel = ACCELERATION;
        }

        horizontalVelocity = horizontalVelocity.LinearInterpolate(target, accel * delta);
        velocity.x = horizontalVelocity.x;
        velocity.z = horizontalVelocity.z;
        velocity = MoveAndSlideWithSnap(velocity, new Vector3(0, 0.05F, 0), new Vector3(0, 1, 0), false, false, 4, Mathf.Deg2Rad(40));
    }

    public override void _Input(InputEvent @event)
    {
        if(@event is InputEventMouseMotion && Input.GetMouseMode() == Input.MouseMode.Captured)
        {
            InputEventMouseMotion mouseMotion = (InputEventMouseMotion)@event;
            rotationBodyTop.RotateX(Mathf.Deg2Rad(mouseMotion.Relative.y * MOUSE_SENSITIVITY));
            this.RotateY(Mathf.Deg2Rad(mouseMotion.Relative.x * MOUSE_SENSITIVITY * -1));

            Vector3 camera_rot = rotationBodyTop.GetRotationDegrees();
            camera_rot.x = Mathf.Clamp(camera_rot.x, -70, 70);
            rotationBodyTop.SetRotationDegrees(camera_rot);
        }
    }
}
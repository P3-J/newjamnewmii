using Godot;
using System;

public partial class Player : CharacterBody2D
{

	Node2D AimSpotParent;
	Sprite2D GunSprite;
	public const float Speed = 300.0f;
	public const float JumpVelocity = -400.0f;

    public override void _Ready()
    {
        base._Ready();

		AimSpotParent = GetNode<Node2D>("aimspotparent");
		GunSprite = GetNode<Sprite2D>("aimspotparent/aimspot/gun");
    }
    public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
		{
			velocity.Y = JumpVelocity;
		}

		Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		if (direction != Vector2.Zero)
		{
			velocity.X = direction.X * Speed;
			velocity.Y = direction.Y * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			velocity.Y = Mathf.MoveToward(Velocity.Y, 0, Speed);
		}

		Velocity = velocity;
		MoveAndSlide();
	}

    public override void _Process(double delta)
    {
        aimSpotLookAtParent(GetViewport().GetCamera2D().GetGlobalMousePosition());
    }


    public void aimSpotLookAtParent(Vector2 MousePosition){
		AimSpotParent.LookAt(MousePosition);

		if (MousePosition.X > Position.X){
			GunSprite.FlipV = false;
		} else {
			GunSprite.FlipV = true;
		}

	}
}

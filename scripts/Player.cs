using Godot;
using System;

public partial class Player : CharacterBody2D
{

	[Export]
    public PackedScene BulletScene;

	Node2D AimSpotParent;
	Sprite2D GunSprite;
	Marker2D BulletSpot; // where bullet should aim towards
	Marker2D BulletSpawnPoint;

	public const float Speed = 300.0f;
	public const float JumpVelocity = -400.0f;

    public override void _Ready()
    {
        base._Ready();

		AimSpotParent = GetNode<Node2D>("aimspotparent");
		GunSprite = GetNode<Sprite2D>("aimspotparent/aimspot/gun");
		BulletSpot = GetNode<Marker2D>("aimspotparent/aimspot/gun/bulletSpot");
		BulletSpawnPoint = GetNode<Marker2D>("aimspotparent/aimspot/gun/bulletSpawnPoint");

		
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

    public override void _Input(InputEvent @event)
    {
        if (Input.IsActionPressed("lmb")){
			ShootBullet();
		}
    }

	private void ShootBullet(){
		Bullet bullet = (Bullet)BulletScene.Instantiate();

		Vector2 Direction = (BulletSpot.GlobalPosition - BulletSpawnPoint.GlobalPosition).Normalized();
		bullet.Direction = Direction;

		bullet.Position = BulletSpawnPoint.GlobalPosition;
		bullet.LookAt(BulletSpot.GlobalPosition);
		GetTree().CurrentScene.AddChild(bullet);
		
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

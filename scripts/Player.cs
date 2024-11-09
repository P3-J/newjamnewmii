using System;
using System.Security.Cryptography.X509Certificates;
using Godot;

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

    public const double BulletSpreadMax = 0.2;

    // In milliseconds
    public const int FireRate = 50;

    // Arbitrarily large value to always fire after init
    public ulong LastBulletTime = 0;

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
        if (Input.IsActionPressed("lmb") && (Time.GetTicksMsec() > LastBulletTime + FireRate))
        {
            LastBulletTime = Time.GetTicksMsec();
            ShootBullet();
        }
    }

    private void ShootBullet()
    {
        Bullet bullet = (Bullet)BulletScene.Instantiate();

        Vector2 Direction = (
            BulletSpot.GlobalPosition - BulletSpawnPoint.GlobalPosition
        ).Normalized();

        var rand = new Random();
        // 0 - no spread
        // -1 or 1 - spread direction
        int SpreadDirectionMultiplier = rand.Next(-1, 2);
        double RndDouble = rand.NextDouble();
        float SpreadValue = (float)(RndDouble * BulletSpreadMax * SpreadDirectionMultiplier);
        bullet.Direction = Direction.Rotated(SpreadValue);

        bullet.Position = BulletSpawnPoint.GlobalPosition;
        bullet.LookAt(BulletSpot.GlobalPosition);
        GetTree().CurrentScene.AddChild(bullet);
    }

    public void aimSpotLookAtParent(Vector2 MousePosition)
    {
        AimSpotParent.LookAt(MousePosition);

        if (MousePosition.X > Position.X)
        {
            GunSprite.FlipV = false;
        }
        else
        {
            GunSprite.FlipV = true;
        }
    }
}

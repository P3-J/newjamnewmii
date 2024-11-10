using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Godot;

public partial class Player : CharacterBase
{
    [Export]
    public PackedScene BulletScene;

    Node2D AimSpotParent;
    Sprite2D GunSprite;
    Marker2D BulletSpot; // where bullet should aim towards
    Marker2D BulletSpawnPoint;
    AnimatedSprite2D PlayerSprites;
    AnimationPlayer WalkingAnimController;
    Node2D GunChildrenBurgerFlipper;

    Camera2D ViewPortCamera; // not real cam just viewport
    public const float JumpVelocity = -400.0f;

    public const double BulletSpreadMax = 0.1;

    // In milliseconds
    public const int FireRate = 400;

    // Arbitrarily large value to always fire after init
    public ulong LastBulletTime = 0;

    public override void _Ready()
    {
        base._Ready();

        AimSpotParent = GetNode<Node2D>("aimspotparent");
        GunSprite = GetNode<Sprite2D>("aimspotparent/NodeToFlipAllChildren/gun");
        BulletSpot = GetNode<Marker2D>("aimspotparent/NodeToFlipAllChildren/gun/bulletSpot");
        BulletSpawnPoint = GetNode<Marker2D>(
            "aimspotparent/NodeToFlipAllChildren/gun/bulletSpawnPoint"
        );
        PlayerSprites = GetNode<AnimatedSprite2D>("sprite");
        WalkingAnimController = GetNode<AnimationPlayer>("feet/walkinganimcontroller");

        GunChildrenBurgerFlipper = GetNode<Node2D>("aimspotparent/NodeToFlipAllChildren");
        ViewPortCamera = GetViewport().GetCamera2D();
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector2 velocity = Velocity;

        if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
        {
            velocity.Y = JumpVelocity;
        }

        Vector2 direction = Input.GetVector("left", "right", "up", "down");
        SetPlayerSpriteDirection(direction); // stupid pidevalt checkida ja mitte siis kui inputi pariselt vajutatakse aga whatever

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

    private void SetPlayerSpriteDirection(Vector2 direction)
    {
        // Values use X vector
        Dictionary<float, string> WalkDirectionDict =
            new()
            {
                { 1f, "walkright" },
                { -1f, "walkleft" },
                { 0f, "walkupdown" },
            };
        // Values use Y vector
        Dictionary<float, string> LookDirectionDict =
            new()
            {
                { 1f, "front" },
                { -1f, "back" },
                { 0f, "side" },
            };

        // Standing still
        if (direction == Vector2.Zero)
        {
            PlayerSprites.Play("front");
            WalkingAnimController.Play("RESET");
            return;
        }

        float WalkDirection = (float)Math.Round(direction.X);

        float LookDirection;
        if (Time.GetTicksMsec() < LastBulletTime + FireRate)
        {
            // gun direction
            LookDirection = 1f;
        }
        else
        {
            LookDirection = (float)Math.Round(direction.Y);
        }

        // Diagonals
        if (direction.X != 0 && direction.Y != 0)
        {
            PlayerSprites.Play(LookDirectionDict[LookDirection]);
            WalkingAnimController.Play(WalkDirectionDict[WalkDirection]);
            return;
        }

        // Left/Right/Up/Down
        PlayerSprites.Play(LookDirectionDict[LookDirection]);
        WalkingAnimController.Play(WalkDirectionDict[WalkDirection]);
        PlayerSprites.FlipH = direction.X < 0;
        return;
    }

    public override void _Process(double delta)
    {
        aimSpotLookAtParent(ViewPortCamera.GetGlobalMousePosition());
        if (Input.IsActionPressed("lmb") && (Time.GetTicksMsec() > LastBulletTime + FireRate))
        {
            LastBulletTime = Time.GetTicksMsec();
            ShootBullet();
        }
    }

    private void ShootBullet()
    {
        Bullet bullet = (Bullet)BulletScene.Instantiate();
        bullet.Position = BulletSpawnPoint.GlobalPosition;
        bullet.BulletOwner = this;

        Vector2 Direction = (
            BulletSpot.GlobalPosition - BulletSpawnPoint.GlobalPosition
        ).Normalized();

        var rand = new Random();
        // 0 - no spread
        // -1 or 1 - spread direction
        int SpreadDirectionMultiplier = rand.Next(-1, 2);
        float SpreadValue = (float)(
            rand.NextDouble() * BulletSpreadMax * SpreadDirectionMultiplier
        );

        Vector2 spreadDirection = Direction.Rotated(SpreadValue);
        bullet.Direction = spreadDirection;
        bullet.LookAt(bullet.Position + spreadDirection);
        GetTree().CurrentScene.AddChild(bullet);
    }

    public void aimSpotLookAtParent(Vector2 MousePosition)
    {
        AimSpotParent.LookAt(MousePosition);

        if (MousePosition.X > Position.X)
        {
            GunChildrenBurgerFlipper.Scale = new(1, 1); // genius level shit
        }
        else
        {
            GunChildrenBurgerFlipper.Scale = new(1, -1);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Godot;

public partial class Player : CharacterBase
{
    [Export]
    public PackedScene BulletScene;

    [Export]
    public Playerui playeruicontroller;
    Node2D AimSpotParent;
    Sprite2D GunSprite;
    Marker2D BulletSpot; // where bullet should aim towards
    Marker2D BulletSpawnPoint;
    AnimatedSprite2D PlayerSprites;
    AnimationPlayer WalkingAnimController;
    Node2D GunChildrenBurgerFlipper;
    AnimationPlayer PlayerGeneralAnims;
    AudioStreamPlayer GunShotSound;
    AudioStreamPlayer explosionSound;
    Sprite2D explosionsprite;

    Camera2D ViewPortCamera; // not real cam just viewport

    public const double BulletSpreadMax = 0.1;

    // In milliseconds
    public ulong LastBulletTime = 0;

    public override void _Ready()
    {
        base._Ready();
        globals.player = this; // global ref for enemies

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
        PlayerGeneralAnims = GetNode<AnimationPlayer>("animations/AnimationPlayer");
        GunShotSound = GetNode<AudioStreamPlayer>("sounds/shoot");
        explosionSound = GetNode<AudioStreamPlayer>("deathnode/explosionsound");
        explosionsprite = GetNode<Sprite2D>("deathnode/explosion");

        playeruicontroller.HpBarController(CurrentHp, MaxHp);
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector2 velocity = Velocity;

        if (!canMove){ return;}

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

    public override void Die()
    {
        base.Die();
        if (!IsInGroup("Player")){return;}
        canMove = false;
        globals.currentFloor = 0;
        PlayerSprites.Visible = false;
        GunSprite.Visible = false;
        explosionsprite.Visible = true;
        explosionSound.Play();
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
            WalkingAnimController.Play("RESET");
        }

        // Use AimSpotParent to have Player look in the direction of the mouse
        float LookDirection = (float)Math.Round(Mathf.Sin(AimSpotParent.Rotation));
        PlayerSprites.Play(LookDirectionDict[LookDirection]);
        // Needed for flipping Player when looking to the left
        float FlipDirection = Mathf.Cos(AimSpotParent.Rotation);
        PlayerSprites.FlipH = FlipDirection < 0;

        // Walking direction based on input vector
        float WalkDirection = (float)Math.Round(direction.X);
        WalkingAnimController.Play(WalkDirectionDict[WalkDirection]);
    }

    public override void _Process(double delta)
    {
        aimSpotLookAtParent(ViewPortCamera.GetGlobalMousePosition());
        if (
            Input.IsActionPressed("lmb")
            && (Time.GetTicksMsec() > LastBulletTime + charBaseFireRate)
        )
        {
            LastBulletTime = Time.GetTicksMsec();
            ShootBullet();
        }
    }

    private void ShootBullet()
    {
        if (!canMove){return;}
        Bullet bullet = (Bullet)BulletScene.Instantiate();
        bullet.Position = BulletSpawnPoint.GlobalPosition;
        bullet.BulletOwner = this;
        bullet.dmg = charBaseDmg;

        Vector2 Direction = (
            BulletSpot.GlobalPosition - BulletSpawnPoint.GlobalPosition
        ).Normalized();

        Random random = new();
        // 0 - no spread
        // -1 or 1 - spread direction
        int SpreadDirectionMultiplier = random.Next(-1, 2);
        float SpreadValue = (float)(
            random.NextDouble() * charBaseSpreadMulti * SpreadDirectionMultiplier
        );

        Vector2 spreadDirection = Direction.Rotated(SpreadValue);
        bullet.Direction = spreadDirection;
        bullet.LookAt(bullet.Position + spreadDirection);
        GetTree().CurrentScene.AddChild(bullet);

        PlayerGeneralAnims.SpeedScale = 800f / charBaseFireRate;
        PlayerGeneralAnims.Play("shoot");
        GunShotSound.Play();
    }


    public void _on_explosionsound_finished(){
        string roomFile = "res://scenes/levels/mainmenu.tscn";
		GetTree().ChangeSceneToFile(roomFile); 
    }


    public override void TakeDmg(int dmg)
    {
        base.TakeDmg(dmg);
        playeruicontroller.HpBarController(CurrentHp, MaxHp);
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

using System;
using Godot;

public partial class Bullet : Area2D
{
    public Vector2 Direction = new(0, 0);
    public float Speed = 50f;

    public int dmg = 1;

    public const float AliveTime = 1.5f;

    public Node BulletOwner;

    private Timer _timer;

    public override void _Process(double delta)
    {
        Position += Direction * Speed * (float)delta;
    }

    public override void _Ready()
    {
        base._Ready();

        Timer timer = new() { WaitTime = AliveTime, OneShot = true };
        AddChild(timer);
        timer.Timeout += QueueFree;
        timer.Start();
    }

    private void _on_area_entered(Area2D area) { }

    private void _on_body_entered(Node2D node)
    {
        if (
            BulletOwner.IsInGroup("Player") && node.IsInGroup("Enemy")
            || BulletOwner.IsInGroup("Enemy") && node.IsInGroup("Player")
        )
        {
            GD.Print("Test");

            // Call take damage here
        }
        if (node is TileMap)
        {
            QueueFree();
        }
    }
}

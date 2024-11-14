using System;
using Godot;

public partial class Bullet : Area2D
{
    public Vector2 Direction = new(0, 0);
    public float Speed = 450f;

    public const float AliveTime = 1.5f;

    public Node BulletOwner;

    public float dmg;

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

    private void _on_body_entered(Node2D node)
    {
        if (node is not CharacterBase character)
        {
            QueueFree();
        }
    }

    private void _on_area_shape_entered(Rid rid, Area2D area, int shapeindex, int localshapeindex)
    {
        if (
            BulletOwner.IsInGroup("Player") && area.IsInGroup("Enemy")
            || BulletOwner.IsInGroup("Enemy") && area.IsInGroup("Player")
        )
        {
            area.GetParent<CharacterBase>().TakeDmg(dmg: dmg);
            QueueFree();
        }
    }
}

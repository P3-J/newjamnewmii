using System;
using Godot;

public partial class Bullet : Area2D
{
    public Vector2 Direction = new(0, 0);
    public float Speed = 450f;

    public const float AliveTime = 1.5f;

    public Node BulletOwner;

    public int dmg;

    public float wallPenChance;

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
        // check for wall penetration chance
        if (wallPenChance < 1f)
        {
            Random random = new();
            float randomFloat = random.Next(0, 1001) / 1000f;
            if (randomFloat > wallPenChance)
            {
                return;
            }
        }
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
            GD.Print(area.GetParent().Name);

            CharacterBase parent = area.GetParent<CharacterBase>();
            GD.Print(parent);
            parent.TakeDmg(dmg: dmg);

            QueueFree();
        }
    }
}

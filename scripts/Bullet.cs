using System;
using Godot;

public partial class Bullet : Area2D
{
    public Vector2 Direction = new(0, 0);
    public float Speed = 200f;

    public int dmg = 1;

    public float AliveTime = 3f;

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
}

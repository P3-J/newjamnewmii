using Godot;
using System;

public partial class Bullet : Area2D
{
	public Vector2 Direction = new(0, 0);
	public float Speed = 600f;
	public override void _Process(double delta)
	{
		Position += Direction * Speed * (float)delta;
	}
}

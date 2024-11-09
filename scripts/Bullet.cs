using Godot;
using System;

public partial class Bullet : Area2D
{
	public Vector2 Direction = new(0, 0);
	public float Speed = 100f;

	public int dmg = 1;
	public override void _Process(double delta)
	{
		Position += Direction * Speed * (float)delta;
	}

}


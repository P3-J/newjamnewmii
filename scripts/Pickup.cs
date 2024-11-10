using Godot;
using System;

public partial class Pickup : Node2D
{
	public SignalBus sgbus;
	public override void _Ready()
	{
		Visible = false;
		sgbus = GetNode<SignalBus>("/root/SignalBus");
	}

	private void _on_area_2d_body_entered(Node2D body){

		if (body.IsInGroup("Player")){
			sgbus.EmitSignal("PlayerOnPickUp");
			QueueFree();
		}

	}
}

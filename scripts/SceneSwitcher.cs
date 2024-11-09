using Godot;
using System;

public partial class SceneSwitcher : Area2D
{
	private SignalBus sgbus;
	public override void _Ready()
	{
		sgbus = GetNode<SignalBus>("/root/SignalBus");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void _on_body_entered(Node2D body){

		if (body.IsInGroup("Player")){
			sgbus.EmitSignal("SwitchScene");
		}

	}
}

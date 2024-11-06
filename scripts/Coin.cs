using Godot;
using System;

public partial class Coin : Area2D
{

	private SignalBus sgbus;
	
	public override void _Ready(){
		sgbus = GetNode<SignalBus>("/root/SignalBus");
	}

	private void _on_body_entered(Node2D body){
		// for fun passib selle enda kaasa, koos signaaliga
		sgbus.EmitSignal("PlayerTouchedCoin", this);
	}

}

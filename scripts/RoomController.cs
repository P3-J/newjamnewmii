using Godot;
using System;

public partial class RoomController : Node2D
{

	private SignalBus sgbus;
	private Globals globals;
	public override void _Ready()
	{

		sgbus = GetNode<SignalBus>("/root/SignalBus");
		globals = GetNode<Globals>("/root/Globals");

		sgbus.Connect("SwitchScene", new Callable(this, nameof(SwitchScene)));

	}

	public void SwitchScene(){
		switch (globals.currentFloor + 1){

			case 2:
				globals.currentFloor++;
				GetTree().ChangeSceneToFile("res://scenes/levels/room_2.tscn");
				break;
			

		}
	}
}

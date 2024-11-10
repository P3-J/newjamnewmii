using Godot;
using System;

public partial class RoomController : Node2D
{

	private SignalBus sgbus;
	private Globals globals;
	Node2D sorter;

	[Export] Pickup pickup;
	public override void _Ready()
	{

		sgbus = GetNode<SignalBus>("/root/SignalBus");
		globals = GetNode<Globals>("/root/Globals");
		sorter = GetNode<Node2D>("sorter");

		sgbus.Connect("SwitchScene", new Callable(this, nameof(SwitchScene)));
		sgbus.Connect("EnemyHasDied", new Callable(this, nameof(CheckIfAllEnemiesAreDead)));

		CheckIfAllEnemiesAreDead(); // will not print acc data at the start
	}

	public void SwitchScene(){
		switch (globals.currentFloor + 1){

			case 2:
				globals.currentFloor++;
				GetTree().ChangeSceneToFile("res://scenes/levels/room_2.tscn");
				break;
		}
	}

	private void CheckIfAllEnemiesAreDead(){
		// maybe wait like 2sec to take into account enemies that might spawn enemies
		GD.Print("Enemies left:", sorter.GetChildCount() - 3); // -3  removed enemy, nav, player
		if (sorter.GetChildCount() - 1 <= 2){ // -1 because not enough time to register the removed enemy node
			pickup.Visible = true;
			GD.Print("Room cleared");
		}
		
	}

}

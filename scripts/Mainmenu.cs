using Godot;
using System;

public partial class Mainmenu : Node2D
{

	private Globals globals;
	public void _on_button_pressed(){
	    globals = GetNode<Globals>("/root/Globals");
		globals.currentFloor = 1;
		string roomFile = "res://scenes/levels/room.tscn";
		GetTree().ChangeSceneToFile(roomFile); 
	}
}

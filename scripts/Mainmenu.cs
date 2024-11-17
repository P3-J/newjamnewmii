using Godot;
using System;

public partial class Mainmenu : Node2D
{
	public void _on_button_pressed(){
		string roomFile = "res://scenes/levels/room.tscn";
		GetTree().ChangeSceneToFile(roomFile); 
	}
}

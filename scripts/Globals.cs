using Godot;
using System;

public partial class Globals : Node
{
	
    public int currentFloor = 1;
    public CharacterBase player; // Global Ref to player
    public int[] ActiveTraits = new int[] {};

}

using Godot;
using System;

public partial class SignalBus : Node
{
	[Signal]
    public delegate void PlayerTouchedCoinEventHandler(); // nimes peab olema EventHandler - stupid aga jahm

}

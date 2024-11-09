using System;
using Godot;

public partial class SignalBus : Node
{
    [Signal]
    public delegate void PlayerTouchedCoinEventHandler(); // nimes peab olema EventHandler - stupid aga jahm

    [Signal]
    public delegate void SwitchSceneEventHandler(); 

}

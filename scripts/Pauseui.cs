using Godot;
using System;

public partial class Pauseui : Control
{
    public override void _Input(InputEvent @event)
    {
        if (Input.IsActionJustPressed("esc")){
			GetTree().Paused = !GetTree().Paused;
			Visible = !Visible;
		}
    }
}

using System;
using Godot;

public partial class Enemy : CharacterBase
{
    public override void _Ready()
    {
        base._Ready();
        hpbar = GetNode<ProgressBar>("hp");
        hpbar.MaxValue = MaxHp;
        hpbar.Value = MaxHp;
        CurrentHp = MaxHp;
    }
}

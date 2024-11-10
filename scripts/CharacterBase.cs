using System;
using Godot;

public partial class CharacterBase : CharacterBody2D
{
    [Export]
    public int MaxHp = 10;

    [Export]
    public float Speed = 200.0f;

    public int CurrentHp;

    public ProgressBar hpbar;

    public void TakeDmg(int dmg)
    {
        CurrentHp -= dmg;
        hpbar.Value = CurrentHp;

        if (CurrentHp <= 0)
        {
            QueueFree();
        }
    }
}

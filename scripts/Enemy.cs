using System;
using Godot;

public partial class Enemy : CharacterBase
{
    ProgressBar hpbar;

    public override void _Ready()
    {
        base._Ready();

        hpbar = GetNode<ProgressBar>("hp");

        hpbar.MaxValue = MaxHp;
        hpbar.Value = MaxHp;
        CurrentHp = MaxHp;
    }

    private void _on_area_2d_area_entered(Area2D area)
    {
        if (area.IsInGroup("bullet"))
        {
            Bullet bullet = (Bullet)area;
            TakeDmg(bullet.dmg);
            area.QueueFree();
        }
    }

    private void TakeDmg(int dmg)
    {
        CurrentHp -= dmg;
        hpbar.Value = CurrentHp;

        if (CurrentHp <= 0)
        {
            QueueFree();
        }
    }
}

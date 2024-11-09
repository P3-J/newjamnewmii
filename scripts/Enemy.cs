using Godot;
using System;

public partial class Enemy : CharacterBody2D
{

	ProgressBar hpbar;
	private int maxhp = 10;
	private int currenthp = 10;

    public override void _Ready()
    {
        base._Ready();

		hpbar = GetNode<ProgressBar>("hp");

		hpbar.MaxValue = maxhp;
		hpbar.Value = maxhp;

    }

    private void _on_area_2d_area_entered(Area2D area){
		
		if (area.IsInGroup("bullet")){
			
			Bullet bullet = (Bullet)area;
			TakeDmg(bullet.dmg);
			area.QueueFree();
		}
	}

	private void TakeDmg(int dmg){
		currenthp -= dmg;
		hpbar.Value = currenthp;

		if (currenthp <= 0){
			QueueFree();
		}

	}

}

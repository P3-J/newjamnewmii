using System;
using Godot;

public partial class CharacterBase : CharacterBody2D
{
    [Export] public int MaxHp = 10;
    [Export] public float Speed  = 200.0f;

    public int CurrentHp;
    

}

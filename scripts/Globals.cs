using System;
using System.Collections.Generic;
using Godot;

public partial class Globals : Node
{
    public int currentFloor = 1;
    public CharacterBase player; // Global Ref to player

    public List<int> ActiveTraits = new();

    public float globalPlayerStatMulti = 1f;

    public float globalEnemyStatMulti = 0.8f;

    public float globalProjSizeMulti = 1f;

    public float globalDamageMulti = 1f;

    public float globalHealthMulti = 1f;

    public float globalCharSizeMulti = 1f;

    public float globalWallPenetrationChance = 1f;

    public float globalProjSpeedMulti = 1f;

    // Delay between bullets in milliseconds
    public float globalFireRateMulti = 50f;

    // Used for bullet.Direction.Rotated()
    public float globalSpreadMulti = 0.15f;
}

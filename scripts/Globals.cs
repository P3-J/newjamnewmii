using System;
using System.Collections.Generic;
using Godot;

public partial class Globals : Node
{
    public int currentFloor = 1;
    public CharacterBase player; // Global Ref to player

    public List<int> ActiveTraits = new();

    public int extraDamage = 0;

    public float globalEnemyStatMulti = 0.8f;

    public float globalProjSizeMulti = 1f;

    public int globalDamageMulti = 1;

    public int extraHealth = 0;

    public float globalCharSizeMulti = 1f;

    public float globalWallPenetrationChance = 0f;

    public float globalProjSpeedMulti = 1f;

    public float globalPlayerStatMulti = 1f;

    // Delay between bullets in milliseconds
    public float globalFireRate = 800f;

    // Used for bullet.Direction.Rotated()
    public float globalSpreadMulti = 0.15f;
}

using System;
using System.Collections.Generic;
using Godot;

public partial class Globals : Node
{
    public int currentFloor = 1;
    public CharacterBase player; // Global Ref to player

    public List<int> ActiveTraits = new();

    public float globalProjMulti = 1f;

    public float globalDamageMulti = 1f;

    public float globalHealthMulti = 1f;

    public float globalCharSizeMulti = 1f;

    public float globalWallPenetrationChance = 1f;
}

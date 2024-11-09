using System;
using System.ComponentModel;
using Godot;



public class Traits
{
    // class to access traits?
    // class to store trait info?

    public void ApplyTrait(CharacterBody2D Character, int TraitNumber){

        switch (TraitNumber){
            case (1):
                // glass cannon for example
                ApplyGlassCannon(Character);
                break;
        }

    }

    public string GetTraitDesc(int TraitNumber){

        switch (TraitNumber){
            case (1):
                return "HP / 2 || DMG * 2";
            default:
                return "Was not able to retrive TraitDesc";
        }

    }


    private void ApplyGlassCannon(CharacterBody2D Character)
    {
        CharacterBase BaseChar = (CharacterBase)Character;
        BaseChar.MaxHp -= 5;
    }

}

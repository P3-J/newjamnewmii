using System;
using System.ComponentModel;
using Godot;

public class Traits
{
    // class to access traits?
    // class to store trait info?

    // inc this 
    public static int TraitCount {get; set;} = 4;

    public void ApplyTrait(CharacterBody2D Character, int TraitNumber){

        switch (TraitNumber){
            case (1):
                // glass cannon for example
                ApplyGlassCannon(Character);
                break;
        }

    }

    public static string[] GetTraitDescAndName(int TraitNumber){

        switch (TraitNumber){
            case 1:
                return new string[] {"glass cannon effect", "glass cannon"};
            case 2:
                return new string[] {"trait2", "trait2 text"};
            case 3:
                return new string[] {"trait3", "trait3 text"};
            case 4:
                return new string[] {"trait4", "trait4 text"};
            default:
                return new string[] {"error with retrival", "error with retrival"};
        }

    }


    private void ApplyGlassCannon(CharacterBody2D Character)
    {
        CharacterBase BaseChar = (CharacterBase)Character;
        BaseChar.MaxHp -= 5;
    }

}

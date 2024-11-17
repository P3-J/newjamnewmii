using System;
using System.ComponentModel;
using Godot;

public class TraitBase
{
    // class to access traits?
    // class to store trait info?


    // inc this
    public static int TraitCount { get; set; } = 7;

    public static void ApplyTrait(Globals globals, int TraitNumber)
    {
        switch (TraitNumber)
        {
            case 1:
                // glass cannon for example
                ApplyProjSizeTrait(globals);
                break;

            case 2:
                ApplyDmgTrait(globals);
                break;
            case 3:
                ApplyCharSizeTrait(globals);
                break;

            case 4:
                ApplyHpAndCharSizeTrait(globals);
                break;
            case 5:
                ApplyWallPenetrationTrait(globals);
                break;
            case 6:
                ApplyProjSpeedTrait(globals);
                break;
            case 7:
                ApplyFireRateTrait(globals);
                break;
        }
    }

    public static string[] GetTraitDescAndName(int TraitNumber)
    {
        switch (TraitNumber)
        {
            case 1:
                return new string[] { "+25% to Projectile Size\n+2 to Damage", "Size matters" };
            case 2:
                return new string[] { "+2 to Damage\n+2 to HP", "RAGE" };
            case 3:
                return new string[] { "-40% to Character Size", "Mini-me, Mini-them" };
            case 4:
                return new string[] { "+4 to HP\n +20% to Character Size", "Yeah, I work out" };
            case 5:
                return new string[]
                {
                    "+30% to chance for bullets to penetrate walls\n +1 to Damage",
                    "Walls can't stop me, or them!",
                };
            case 6:
                return new string[]
                {
                    "+30% to Projectile Speed\n+1 to Damage",
                    "Fast and furious",
                };
            case 7:
                return new string[] { "+40% to Fire rate\n-15% to Accuracy", "Rambo" };
            default:
                return new string[] { "error with retrival", "error with retrival" };
        }
    }

    public static string GetTraitIcon(int TraitNumber)
    {
        switch (TraitNumber)
        {
            case 1:
                return "res://textures/traiticons/projsize.png";
            case 2:
                return "res://textures/traiticons/ragetrait.png";
            case 3:
                return "res://textures/traiticons/minitrait.png";
            case 4:
                return "res://textures/traiticons/workouttrait.png";
            case 5:
                return "res://textures/traiticons/walltrait.png";
            case 6:
                return "res://textures/traiticons/fastnfurioustrait.png";
            case 7:
                return "res://textures/traiticons/rambotrait.png";

            default:
                return "res://icon.svg";
        }
    }

    private static void ApplyProjSizeTrait(Globals globals)
    {
        globals.globalProjSizeMulti *= 1.25f;
        globals.extraDamage += 2;
    }

    private static void ApplyDmgTrait(Globals globals)
    {
        globals.extraHealth += 2;
        globals.extraDamage += 2;
    }

    private static void ApplyCharSizeTrait(Globals globals)
    {
        globals.globalCharSizeMulti *= 0.6f;
    }

    private static void ApplyHpAndCharSizeTrait(Globals globals)
    {
        globals.globalCharSizeMulti *= 1.2f;
        globals.extraHealth += 4;
    }

    private static void ApplyWallPenetrationTrait(Globals globals)
    {
        globals.globalWallPenetrationChance += 0.3f;
        globals.extraDamage += 1;
    }

    private static void ApplyProjSpeedTrait(Globals globals)
    {
        globals.globalProjSpeedMulti *= 1.3f;
        globals.extraDamage += 1;
    }

    private static void ApplyFireRateTrait(Globals globals)
    {
        // firerate = time between bullets
        globals.globalFireRate *= 0.6f;
        globals.globalSpreadMulti *= 1.15f;
    }
}

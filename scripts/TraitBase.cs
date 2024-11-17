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
                return new string[] { "+25% to Projectile Size\n+1 extra damage", "Size matters" };
            case 2:
                return new string[] { "+2 to Damage\n+2 to HP", "RAGE" };
            case 3:
                return new string[] { "-20% to Character Size", "Mini-me, Mini-them" };
            case 4:
                return new string[] { "+2 to HP\n +10% to Character Size", "Yeah, I work out" };
            case 5:
                return new string[]
                {
                    "+10% to chance for bullets to penetrate walls",
                    "Walls can't stop me! (or them)",
                };
            case 6:
                return new string[]
                {
                    "+15% to Projectile Speed\n+1 to Damage",
                    "Fast...but not furious",
                };
            case 7:
                return new string[] { "+20% to Fire rate\n-15% to Accuracy", "Rambo" };
            default:
                return new string[] { "error with retrival", "error with retrival" };
        }
    }

    private static void ApplyProjSizeTrait(Globals globals)
    {
        globals.globalProjSizeMulti *= 1.25f;
        globals.extraDamage += 1;
    }

    private static void ApplyDmgTrait(Globals globals)
    {
        globals.extraHealth += 2;
        globals.extraDamage += 2;
    }

    private static void ApplyCharSizeTrait(Globals globals)
    {
        globals.globalCharSizeMulti *= 0.8f;
    }

    private static void ApplyHpAndCharSizeTrait(Globals globals)
    {
        globals.globalCharSizeMulti *= 1.1f;
        globals.extraHealth += 2;
    }

    private static void ApplyWallPenetrationTrait(Globals globals)
    {
        globals.globalWallPenetrationChance *= 0.90f;
    }

    private static void ApplyProjSpeedTrait(Globals globals)
    {
        globals.globalProjSpeedMulti *= 1.15f;
        globals.extraDamage += 1;
    }

    private static void ApplyFireRateTrait(Globals globals)
    {
        // firerate = time between bullets
        globals.globalFireRate -= 200f;
        globals.globalSpreadMulti *= 1.15f;
    }
}

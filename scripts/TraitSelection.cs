using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Godot;

public partial class TraitSelection : Control
{
    [Export]
    TraitCard card1;

    [Export]
    TraitCard card2;

    [Export]
    TraitCard card3;

    private TraitCard CurrentCard;
    public Globals globals;
    public SignalBus sgbus;
    public Control VisiblityParent;

    public CharacterBase characterBase;

    private TraitCard[] cards;

    public override void _Ready()
    {
        VisiblityParent = GetNode<Control>("visparent");
        VisiblityParent.Visible = false;

        globals = GetNode<Globals>("/root/Globals");
        sgbus = GetNode<SignalBus>("/root/SignalBus");

        card1.Connect("CurrentlySelectedCard", new Callable(this, nameof(SelectCard)));
        card2.Connect("CurrentlySelectedCard", new Callable(this, nameof(SelectCard)));
        card3.Connect("CurrentlySelectedCard", new Callable(this, nameof(SelectCard)));

        sgbus.Connect("PlayerOnPickUp", new Callable(this, nameof(StartTraitSelection)));

        cards = new TraitCard[] { card1, card2, card3 };
    }

    public void StartTraitSelection()
    {
        GenerateTraitCards();

        GetTree().Paused = true;
        VisiblityParent.Visible = true;
    }

    private void GenerateTraitCards()
    {
        // this should be from a list of avail traits, as the same one should not be able to be picked again


        List<int> availableTraits = Enumerable.Range(1, TraitBase.TraitCount).ToList();
        //

        foreach (TraitCard card in cards)
        {
            // needs to be made better, uses error card if traitcard count is larger that available traits
            if (availableTraits.Count == 0)
            {
                card.SetTraitCardToBe(0);
                return;
            }
            Random random = new Random();
            int randomIndex = random.Next(0, availableTraits.Count);
            int randomTrait = availableTraits[randomIndex];
            availableTraits.Remove(randomTrait);
            card.SetTraitCardToBe(randomTrait);
        }
    }

    public void SelectCard(TraitCard card)
    {
        CurrentCard = card;
    }

    private void _on_button_pressed()
    {
        if (CurrentCard != null)
        {
            TraitBase.ApplyTrait(TraitNumber: CurrentCard.TraitNumber, globals: globals);
            globals.ActiveTraits.Add(CurrentCard.TraitNumber);
            GetTree().Paused = false;
            VisiblityParent.Visible = false;
            GD.Print("currently active traits:", string.Join(", ", globals.ActiveTraits));

            sgbus.EmitSignal("NewTraitSelected");
        }
    }
}

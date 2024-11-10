using Godot;
using System;
using System.Linq;

public partial class TraitSelection : Control
{
	[Export] TraitCard card1;
	[Export] TraitCard card2;
	[Export] TraitCard card3;

	private TraitCard CurrentCard;
	public Globals globals;
	public SignalBus sgbus;
	public Control VisiblityParent;

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

		cards = new TraitCard[] {card1, card2, card3};

	}


	public void StartTraitSelection(){

		GenerateTraitCards();

		GetTree().Paused = true;
        VisiblityParent.Visible = true;
	}

    private void GenerateTraitCards()
    {
		// this should be from a list of avail traits, as the same one should not be able to be picked again
        int TotalTraits = Traits.TraitCount;

		foreach (TraitCard card in cards)
        {
            Random random = new Random();
    		int randomPick = random.Next(1, TotalTraits + 1);
			card.SetTraitCardToBe(randomPick);
		}
    }
    public void SelectCard(TraitCard card){
		CurrentCard = card;
	}
	
	private void _on_button_pressed(){
		if (CurrentCard != null){
            globals.ActiveTraits = globals.ActiveTraits.Append(CurrentCard.TraitNumber).ToArray();
			GetTree().Paused = false;
			VisiblityParent.Visible = false;
			GD.Print("currently active traits:", string.Join(", ", globals.ActiveTraits));
		}
	}



}

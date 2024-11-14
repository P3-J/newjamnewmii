using System;
using Godot;

public partial class TraitCard : ColorRect
{
    Sprite2D Icon;
    RichTextLabel Name;
    RichTextLabel Effect;
    ColorRect HoverIndicator;
    private bool Highlighted = false;
    public int TraitNumber;
    public SignalBus sgbus;

    [Signal]
    public delegate void CurrentlySelectedCardEventHandler();

    public override void _Ready()
    {
        sgbus = GetNode<SignalBus>("/root/SignalBus");
        Icon = GetNode<Sprite2D>("icon");
        Name = GetNode<RichTextLabel>("name");
        Effect = GetNode<RichTextLabel>("effect");
        HoverIndicator = GetNode<ColorRect>("hoverindicator");

        sgbus.Connect("UnHighlightOtherCards", new Callable(this, nameof(Unhighlight)));
    }

    public override void _Input(InputEvent @event)
    {
        if (Input.IsActionJustPressed("lmb") && Highlighted)
        {
            HoverIndicator.Visible = true;
            sgbus.EmitSignal("UnHighlightOtherCards", this);
            EmitSignal("CurrentlySelectedCard", this);
        }
    }

    public void Unhighlight(TraitCard card)
    {
        if (card != this)
        {
            HoverIndicator.Visible = false;
        }
    }

    public void SetTraitCardToBe(int traitNumber)
    {
        string[] info = TraitBase.GetTraitDescAndName(traitNumber);
        Effect.Text = info[0];
        Name.Text = info[1];
        TraitNumber = traitNumber;
    }

    public void _on_mouse_entered()
    {
        Highlighted = true;
    }

    public void _on_mouse_exited()
    {
        Highlighted = false;
    }
}

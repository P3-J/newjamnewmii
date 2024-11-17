using System;
using Godot;

public partial class TraitCard : Panel
{
    Sprite2D Icon;
    RichTextLabel traitName;
    RichTextLabel Effect;
    private bool Highlighted = false;
    public int TraitNumber;
    public SignalBus sgbus;

    [Signal]
    public delegate void CurrentlySelectedCardEventHandler();

    public override void _Ready()
    {
        sgbus = GetNode<SignalBus>("/root/SignalBus");
        Icon = GetNode<Sprite2D>("icon");
        traitName = GetNode<RichTextLabel>("name");
        Effect = GetNode<RichTextLabel>("text");

        sgbus.Connect("UnHighlightOtherCards", new Callable(this, nameof(Unhighlight)));
    }

    public override void _Input(InputEvent @event)
    {
        if (Input.IsActionJustPressed("lmb") && Highlighted)
        {
            sgbus.EmitSignal("UnHighlightOtherCards", this);
            EmitSignal("CurrentlySelectedCard", this);
        }
    }

    public void Unhighlight(TraitCard card)
    {
        if (card != this)
        {
            SelfModulate = Colors.White;
        }
        else
        {
            SelfModulate = Colors.Gold;
        }
    }

    public void SetTraitCardToBe(int traitNumber)
    {
        string pathToIcon = TraitBase.GetTraitIcon(traitNumber);
        Texture2D iconTexture = (Texture2D)GD.Load(pathToIcon);
        Icon.Texture = iconTexture;
        Icon.Scale = new Vector2(0.484f, 0.453f);

        string[] info = TraitBase.GetTraitDescAndName(traitNumber);
        Effect.Text = info[0];
        traitName.Text = info[1];
        TraitNumber = traitNumber;
    }

    public void _on_mouse_entered()
    {
        Modulate = Colors.Gray;
        Highlighted = true;
    }

    public void _on_mouse_exited()
    {
        Modulate = Colors.White;
        Highlighted = false;
    }
}

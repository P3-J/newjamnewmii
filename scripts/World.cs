using System;
using Godot;

public partial class World : Node2D
{
    SignalBus sgbus;
    RichTextLabel cointext;

    private int CoinCount = 0;

    public override void _Ready()
    {
        sgbus = GetNode<SignalBus>("/root/SignalBus");

        sgbus.Connect("PlayerTouchedCoin", new Callable(this, nameof(IncCoinCount)));

        cointext = GetNode<RichTextLabel>("ui/coins"); // nagu naha getNode tehes peab uuesti type utlema, overall see on viis kuidas childile saada access.
    }

    private void IncCoinCount(Area2D Coin)
    {
        GD.Print("Player touched coin");
        Coin.QueueFree(); //boom coin access saame eemaldada

        CoinCount++;
        cointext.Text = "Coins: " + CoinCount; // crazy int ja string on fine lmaoo (js looking at you)
    }
}


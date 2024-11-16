using Godot;
using System;

public partial class Playerui : Control
{
	
	ProgressBar hpbar;
	RichTextLabel hptext;

	public override void _Ready()
	{

		hpbar = GetNode<ProgressBar>("hpbar");
		hptext = GetNode<RichTextLabel>("hptext");

	}


	public void HpBarController(int cHp, int maxHp){

		hpbar.MaxValue = maxHp;
		hpbar.Value = cHp;

		int newSize = (int)Math.Round(10f * maxHp);
		hpbar.Size = new Vector2(newSize, hpbar.Size.Y);
		hptext.Text = cHp.ToString() + "/" + maxHp.ToString();
		
	}

}

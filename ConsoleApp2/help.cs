using System;

public class help : item
{
	public help() : base("help", new string[] { "tutorial", "advice" }, 0, 0)
	{
	}
    public override void activate(player player)
    {
        Console.WriteLine("");
        typer typer = new typer("This will display help later.");
        typer.start();
    }
}
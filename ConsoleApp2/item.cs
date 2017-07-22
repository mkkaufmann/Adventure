using System;

public class item
{
    public string name;
    public string[] aliases;
    public int weight;
    public int value;
    public bool isVisible = true;
	public item(string nameParam,string[] aliasesParam = null, int weightParam = 0, int valueParam = 0)
	{
        name = nameParam;
        weight = weightParam;
        value = valueParam;
        aliases = aliasesParam;
	}
    public virtual void PickUp(player player)
    {
        player.inventory.Add(this);
        player.currentRoom.items.Remove(this);
        Console.WriteLine("");
        typer typer = new typer("You picked up: " + this.name + ".");
        typer.start();
    }
    public virtual void activate(player player)
    {
        Console.WriteLine("");
        player.inventory.Remove(this);
        player.points += this.value;
        typer typer = new typer("You used: "+this.name+".");
        typer.start();
    }
    public virtual void Drop(player player)
    {
        Console.WriteLine("");
        player.inventory.Remove(this);
        player.currentRoom.items.Add(this);
        typer typer = new typer("You dropped: " + this.name + ".");
        typer.start();
    }
}

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
    public virtual void activate(player player)
    {
        player.inventory.Remove(this);
        Console.WriteLine("You used: {0}.",name);
    }
}

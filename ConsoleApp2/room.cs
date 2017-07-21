using System;
using System.Collections.Generic;

public class room
{
    public room north;
    public room south;
    public room east;
    public room west;
    public List<item> items = new List<item>();
    public List<enemy> enemies = new List<enemy>();
    public string enterMessage;
    public bool isNull;
	public room(string enterMessageParam = "")
	{
        enterMessage = Environment.NewLine + enterMessageParam;
        isNull = false;
	}
    public void enter()
    {
        typer typer = new typer(enterMessage);
        typer.start();
    }
    public void attach(string side,room roomToAttach)
    {
        switch (side)
        {
            case "north":
                north = roomToAttach;
                roomToAttach.south = this;
                break;
            case "south":
                south = roomToAttach;
                roomToAttach.north = this;
                break;
            case "east":
                east = roomToAttach;
                roomToAttach.west = this;
                break;
            case "west":
                west = roomToAttach;
                roomToAttach.east = this;
                break;
            default:
                Console.WriteLine("There is a bug. Please notify the developer.");
                break;
        }
    }
    public string[] getItemNames()
    {
        List<string> returnVal = new List<string>();
        foreach(item item in items)
        {
            returnVal.Add(item.name);
        }
        return returnVal.ToArray();
    }
    public bool checkIfItem(string text)
    {
        foreach (item item in items)
        {
            if (text == item.name)
            {
                return true;
            }
            if (!(item.aliases is null))
            {
                foreach (string alias in item.aliases)
                {
                    if (alias == text)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }
    public item getItem(string text)
    {
        foreach (item item in items)
        {
            if (text == item.name)
            {
                return item;
            }
            if (!(item.aliases is null))
            {
                foreach (string alias in item.aliases)
                {
                    if (alias == text)
                    {
                        return item;
                    }
                }
            }
        }
        Console.WriteLine("An error occurred. Please contact the developer. Info: could not find weapon with this name.");
        return new item("ERROR 404");
    }
}

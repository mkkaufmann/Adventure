using System;
using System.Collections.Generic;

public class player
{
    public string name;
    public int points = 0;
    public List<item> inventory = new List<item>();
    public int maxInventoryWeight;
    public int health = 100;
    public int maxHealth = 100;
    public weapon equippedWeapon;
    public room currentRoom;
    public enemy currentEnemy = null;
	public player()
	{
	}
    public string[] getItemNames()
    {
        List<string> returnVal = new List<string>();
        foreach (item item in inventory)
        {
            returnVal.Add(item.name);
        }
        return returnVal.ToArray();
    }
    public bool checkIfItem(string text)
    {
        foreach(item item in inventory)
        {
            if (text == item.name)
            {
                return true;
            }
            if (!(item.aliases is null))
            {
                foreach(string alias in item.aliases)
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
        foreach (item item in inventory)
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
    public void attack(enemy enemy)
    {
        enemy.health -= equippedWeapon.damage;
    }
}

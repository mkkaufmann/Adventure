using System;
using System.Collections.Generic;
public class enemy
{
    public int health;
    public string name;
    public weapon weapon;
    public weapon defaultWeapon = new weapon("Sword",damageParam:1);

    public enemy(int healthParam = 100, weapon weaponParam = null)
    {
        health = healthParam;
        if (weaponParam is null)
        {
            weapon = defaultWeapon;
        }
        else
        {
            weapon = weaponParam;
        }
	}
    public virtual void attack(player player)
    {
        List<String> chances = new List<string>();
        for(int i = 0; i < player.equippedWeapon.damage; i++)
        {
            chances.Add("player");
        }
        for (int i = 0; i < weapon.damage; i++)
        {
            chances.Add("enemy");
        }
        Random random = new Random();
        int num = random.Next(chances.Count - 1);
        if (chances[num] == "enemy")
        {

        }
        else
        {

        }
    }
}

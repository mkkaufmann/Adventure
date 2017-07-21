using System;

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
    public void attack(player player)
    {
        player.health -= weapon.damage;
    }
}

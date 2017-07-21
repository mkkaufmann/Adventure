using System;

public class weapon : item
{
    public int damage;
	public weapon(string nameParam, string[] aliasesParam = null, int weightParam = 0, int valueParam = 0,int damageParam = 1) : base(nameParam, aliasesParam, weightParam, valueParam)
	{
        damage = damageParam;
	}
}

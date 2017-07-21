using System;

public class wall : room
{
    
	public wall(string enterMessageParam = "Sorry, there is a wall there.")
	{
        enterMessage = Environment.NewLine + enterMessageParam;
        isNull = true;
	}
}

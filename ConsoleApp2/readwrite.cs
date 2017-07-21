using System;
using System.IO;

public class ReadWrite
{
    static string path;
    private StringReader reader = new StringReader(path);
    private StringWriter writer = new StringWriter();
	public ReadWrite(string pathParam = "leaderboard.txt")
	{
        path = pathParam;
	}

}

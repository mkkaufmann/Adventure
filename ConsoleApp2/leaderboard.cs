using System;
using System.IO;

public class Leaderboard
{
    static string path;
    private StreamReader reader;
    private StreamWriter writer;
	public Leaderboard(string pathParam = "leaderboard.txt")
	{
        path = pathParam;
        //writer = new StreamWriter(path);
        
    }
    public string Read()
    {
        reader = new StreamReader(path);
        string returnVal ="";
        try
        {
            string currentLine;
            while((currentLine = reader.ReadLine()) != null)
            {
                returnVal += currentLine + Environment.NewLine;
            }
        }
        catch(Exception e)
        {
            Console.WriteLine(e.Message);
        }
        return returnVal;
    }
}

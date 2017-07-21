using System;
using System.IO;
using System.Collections.Generic;

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
                returnVal += currentLine + '`';
            }
        }
        catch(Exception e)
        {
            Console.WriteLine(e.Message);
        }
        return returnVal;
    }
    private List<string[]> F\ormatAsList()
    {
        string unformatted = this.Read();
        List<string[]> lines = new List<string[]>();

        foreach (string line in unformatted.Split('`'))
        {
            if (line != "")
            {
                lines.Add(line.Split(','));
            }
        }
        return lines;
    }
    public void AddScore(player player)
        //very 
    {
        List<string[]> lines = F\ormatAsList();
        foreach (string[] line in lines)
        {
            lines
        }
    }
    public string Display()
    {
        string returnVal = "";
        string unformatted = this.Read();
        List<string[]> lines = F\ormatAsList();

        foreach (string line in unformatted.Split('`'))
        {
            if (line != "")
            {
                lines.Add(line.Split(','));
            }
        }
        List<int> longestLengths = new List<int>();
        if (lines.Count > 0)
        {
            for(int i = 0; i < lines[0].Length; i++)
            {
                longestLengths.Add(0);
                foreach(string[] line in lines)
                {
                    if (line[i].Length > longestLengths[i])
                    {
                        longestLengths[i] = line[i].Length;
                    }
                }
            }
            //format evenly
            foreach(string[] line in lines)
            {
                for(int i = 0; i < line.Length; i++)
                {
                    while (line[i].Length < longestLengths[i])
                    {
                        line[i] += " ";
                    }
                    returnVal += line[i];
                    if(i != line.Length - 1)
                    {
                        returnVal += "|";
                    }
                }
                if(lines.IndexOf(line)!= lines.Count - 1)
                {
                    returnVal += Environment.NewLine;
                }
            }
            
        }
        return returnVal;
    }
}

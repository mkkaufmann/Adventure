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
        reader.Close();
        return returnVal;
    }
    private List<string[]> FormatAsList()
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
    {
        //very narrow function for adding scores to the leaderboard
        List<string[]> lines = FormatAsList();
        List<int> scores = new List<int>();

        foreach (string[] line in lines)
        {
            scores.Add(int.Parse(line[1]));
        }
        foreach(int score in scores)
        {
            if(player.points > score)
            {
                lines.Insert(scores.IndexOf(score), new string[]{ player.name,player.points.ToString()});
                lines.RemoveAt(lines.Count-1);
                break;
            }
        }
        this.WriteToFile(lines);
    }
    public void WriteToFile(List<string[]> lines)
    {
        writer = new StreamWriter(path,false);
        foreach (string[] line in lines)
        {
            string formattedLine = "";
            for (int i = 0; i < line.Length - 1; i++)
            {
                formattedLine += line[i] + ",";
            }
            formattedLine += line[line.Length - 1];
            writer.WriteLine(formattedLine);
        }
        writer.Close();
    }
    public string Display()
    {
        string returnVal = "";
        string unformatted = this.Read();
        List<string[]> lines = FormatAsList();

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

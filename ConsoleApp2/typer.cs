using System;
using System.Timers;

public class typer
{
    public string text;
    public char[] charArray;
    private int currentIndex = 0;
    private Timer timer = new Timer();
	public typer(string textParam)
	{
        text = textParam;
        charArray = text.ToCharArray();
        timer.Interval = 40;
        timer.Elapsed += new System.Timers.ElapsedEventHandler(writeChar);
	}
    public bool start()
    {
        timer.Start();
        while (timer.Enabled)
        {
        }
        return true;
    }
    public void writeChar(object sender, System.Timers.ElapsedEventArgs e)
    {
        if (currentIndex < charArray.Length)
        {
            Console.Write(charArray[currentIndex]);
            currentIndex++;
        }
        else
        {
            Console.WriteLine("");
            timer.Stop();
        }
    }
}

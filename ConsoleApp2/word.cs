using System;
using System.Collections.Generic;

public class word
{
    public string text;
    public string[] aliases;
    public word[] nextWords;
	public word(string textParam,string[] aliasesParam = null, word[] nextWordsParam = null)
	{
        text = textParam;
        aliases = aliasesParam;
        nextWords = nextWordsParam;
	}
    public string[] wordsReturn()
    {
        List<string> returnVal = new List<string>();
        returnVal.Add(text);
        if (!(aliases is null))
        {
            for(int i = 0; i < aliases.Length; i++)
            {
                returnVal.Add(aliases[i]);
            }
        }
        return returnVal.ToArray();
    }
    public string[] nextWordsString()
    {
        List<string> returnVal = new List<string>();
        if (!(nextWords is null))
        {
            foreach (word nextWord in nextWords)
            {
                returnVal.Add(nextWord.text);
                if (!(nextWord.aliases is null))
                {
                    for(int j = 0; j < nextWord.aliases.Length; j++)
                    {
                        returnVal.Add(nextWord.aliases[j]);
                    }
                }
            }
        }
        return returnVal.ToArray();
    }
    public bool checkForEquiv(string textToCheck)
    {
        if (text == textToCheck)
        {
            return true;
        }
        if (!(aliases is null))
        {
            foreach (string alias in aliases)
            {
                if (textToCheck == alias)
                {
                    return true;
                }
            }
        }
        return false;
    }
}

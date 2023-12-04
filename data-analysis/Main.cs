using System;
using System.IO;
using System.Reflection;

namespace dataanalysis;

public class Program
{
    public static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("No file specified");
            Console.ReadKey();
            return;
        }
        
        string filePath = args[0];
        
        FileReader reader = new FileReader();

        string data = reader.FileToString(filePath);

        reader.StringToWords(data);

        reader.DisplayWords();

    }
    

}

public interface IFileReader
{
    string FileToString(string path);
}

public class FileReader : IFileReader
{
    
    public List<string> Words = new List<string>();
    
    public string FileToString(string path)
    {
        string data = "";

        try
        {
            data = File.ReadAllText(path);

        }
        catch (IOException e)
        {
            Console.WriteLine($"Error reading the file: {e}");
        }

        return data;
    }

    public void StringToWords(string dataString)
    {

        string currentWord = "";
        
        foreach (char letter in dataString)
        {
            
            if (letter == ' ' || letter.Equals('\n'))
            {
                addWord(currentWord);
                currentWord = "";
            }
            else if (IsEnglishLetter(letter))

            {
                currentWord += letter;
            }
        }

        addWord(currentWord);

    }

    private static bool IsEnglishLetter(char c)
    {
        return ((c >= 'A' & c <= 'Z') ||
                (c >= 'a' & c <= 'z') ||
                (char.IsDigit(c)));
    }

    private void addWord(string currentWord)
    {
        if (currentWord != "")
        {
            Words.Add(currentWord.ToLower());
        }
    }

    public void DisplayWords()
    {
        foreach (string word in Words)
        {
            Console.WriteLine(word);
        }
    }
    

}


// ../testing/data/sample1.txt
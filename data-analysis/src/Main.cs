using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;




namespace dataanalysis;
public class Program
{
    private static string? Path { get; set; }
    private static bool Multithreading { get; set; }

    public Program()
    {
        Path = "";
        Multithreading = false;
    }
    
    public static void Main(string[] args)
    {

        if (!HandleArguments(args))
        {
            return;
        }
        
        Console.WriteLine(Path);
        
        FileReader reader = new FileReader();

        string data = reader.FileToString(Path);

        reader.StringToWords(data);

        List<string> wordList = reader.Words;
        
        Stopwatch stopwatch = new Stopwatch();

        stopwatch.Start();

        float mean;

        if (Multithreading)
        {
            MultiThreadingProcessor test = new MultiThreadingProcessor(wordList);
            mean = test.Mean();
        }
        else
        {
            SingleThreadProcessor test = new SingleThreadProcessor();
            mean = test.GetMean(wordList);

        }
        
        stopwatch.Stop();
        
        Console.WriteLine($"Time elapsed: {stopwatch.Elapsed}");
        
        Console.WriteLine($"Mean: {mean}");
        
        stopwatch.Stop();
        
    }

    private static bool HandleArguments(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("No arguments specified. You must specify a path");
            Console.ReadKey();
            return false;
        }

        foreach (string arg in args)
        {
            string prefix = "path=";
            
            if (CheckForPath(arg, prefix))
            {
                Path = arg.Substring(prefix.Length);
            }

            prefix = "multithreading=";
            
            if (CheckForPath(arg, prefix))
            {
                if (!Boolean.TryParse(arg.Substring(prefix.Length), out bool result))
                {
                    Console.WriteLine("multithreading must equal to equal true or false.");
                    return false;
                }

                Multithreading = result;
            }
        }

        if (Path == null)
        {
            Console.WriteLine("You must specify a path");
            return false;
        }

        return true;
    }

    private static bool CheckForPath(string argument, string prefix)
    {
        if (argument.Substring(0, prefix.Length).ToLower() != prefix)
        {
            return false;
        }
        return true;

    }
    

    private static bool MultiThreading(string parsedArguments)
    {
        return false;
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
                (c == '-') ||
                (char.IsDigit(c)));
    }

    private void addWord(string currentWord)
    {
        if (currentWord == "") return;

        int start = 0;
        
        int last = currentWord.Length - 1;
        
        while (currentWord[start] == '-')
        {
            start++;
        }
        
        while (currentWord[last] == '-')
        {
            last--;
        }
        
        {
            Words.Add(currentWord.Substring(start, last - start + 1).ToLower());
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

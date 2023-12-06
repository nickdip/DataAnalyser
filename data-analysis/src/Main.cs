using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;


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
        
        Stopwatch stopwatch = new Stopwatch();
        
        var rand = new Random();

        List<string> words = new List<string>{};
        
        for (int ctr = 0; ctr <= 10000000; ctr++)
        {
            words.Add(rand.Next().ToString());
        }

        stopwatch.Start();
        
        WordProcessor test = new WordProcessor(words);
        
        stopwatch.Stop();
        
        Console.WriteLine($"Time elapsed (threading): {stopwatch.Elapsed}");

        int sum = 0; 
        
        stopwatch.Start();
        
        foreach (string word in words)
        {
            sum += word.Length;
        }
        
        Console.WriteLine($"Mean: { (float) sum / words.Count}");
        
        stopwatch.Stop();
        
        Console.WriteLine($"Time elapsed (not-threading): {stopwatch.Elapsed}");
        
        
        
        
        

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

public class WordProcessor
{
    public static List<string> Words { get; set; }
    private static readonly ManualResetEvent _doneEvent = new ManualResetEvent(false);
    private static float _totalLength = 0;
    private static int _completedTasks = 0;
    private static int _wordCount = 0;
    private static readonly object LockObj = new object();

    public WordProcessor(List<string> providedWords)
    {
        Words = providedWords; 
        ThreadPool.SetMaxThreads(Environment.ProcessorCount, Environment.ProcessorCount);
        
        int sublistSize = 10000; 
        int totalWords = Words.Count;
        int startIndex = 0;

        while (startIndex < totalWords)
        {
            int count = Math.Min(sublistSize, totalWords - startIndex);
            List<string> sublist = Words.GetRange(startIndex, count);
            ThreadPool.QueueUserWorkItem(state => ProcessSublist(sublist));

            startIndex += sublistSize;
        }
        
        
        _doneEvent.WaitOne();
        
        Console.WriteLine($"Mean: {_wordCount / _totalLength}");
    }
    

    private static void WordProcess(object word)
    {
        Increment(word);
    }
    
    private static void ProcessSublist(List<string> sublist)
    {
        BatchProcess batch = new BatchProcess(sublist);
        
        _wordCount += batch.Sum;
        
        _totalLength += batch.Count;
    
        // Assuming _completedTasks is incremented after each word is processed
        lock (LockObj)
        {
            _completedTasks += sublist.Count;

            if (_completedTasks == Words.Count)
            {
                _doneEvent.Set();
            }
        }
    }
    
    
    private static void Increment(object state)
    {
        string word = (string) state;
        int length = word.Length;
        
        lock (LockObj)
        {
            _totalLength += length;
            _wordCount++;
            _completedTasks++;

            if (_completedTasks == _wordCount)
            {
                _doneEvent.Set();
            }
        }
    }

    private static float Mean()
    {
        return (float)_totalLength / _wordCount;
    }
    
    
}

public class BatchProcess
{
    public float Count { get; set; }
    public int Sum { get; set; }

    public BatchProcess(List<string> words)
    {
        Count = 0;
        Sum = 0;
        foreach (string word in words)
        {
            Sum += word.Length;
            Count++;
        }
    }
    
}

// public class Analysis : WordProcessor
// {
//     public float AvgWordCount { get; private set; }
//
//     public Analysis(List<string> providedWords) : base(providedWords)
//     {
//         AvgWordCount = AvgWordLength();
//     }
//
//     private float AvgWordLength()
//     {
//         if (Words.Count == 0) return 0;
//             
//         float sum = 0;
//         
//         foreach (string word in Words)
//         {
//             sum += word.Length;
//         }
//
//         return sum / Words.Count; 
//     }
//     
//     
//     
// }

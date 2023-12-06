
using dataanalysis;

public class MultiThreadingProcessor
{
    private static List<string>? Words { get; set; }
    private static readonly ManualResetEvent _doneEvent = new ManualResetEvent(false);
    private static float _totalLength = 0;
    private static int _completedTasks = 0;
    private static int _wordCount = 0;
    private static readonly object LockObj = new object();

    public MultiThreadingProcessor(List<string> providedWords)
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



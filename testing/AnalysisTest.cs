using dataanalysis;

public class SingleThreadTest
{
    [Fact]
    public void AvgWordLength_ReturnsTheWordLengthForOneWord()
    {
        List<string> testWords = new List<string> { "Test" };
        
        var analysisTest =  new SingleThreadProcessor(testWords);

        var expected = 4;

        var output = analysisTest.GetMean();
        
        Assert.Equal(expected, output);
    }
    
    [Fact]
    public void AvgWordLength_ReturnsTheWordLengthForMultipleWords()
    {
        List<string> testWords = new List<string> { "This", "is", "a", "test" };
        
        var analysisTest =  new SingleThreadProcessor(testWords);

        var expected = ("This".Length + "is".Length + "a".Length + "test".Length) / (float) testWords.Count;

        var output = analysisTest.GetMean();
        
        Assert.Equal(expected, output);
    }
    
    
}

public class MultiThreadTest
{
    [Fact]
    public void AvgWordLength_ReturnsTheWordLengthForOneWord()
    {
        List<string> testWords = new List<string> { "Test" };
        
        var analysisTest =  new MultiThreadingProcessor(testWords);

        var expected = 4;

        var output = analysisTest.Mean();
        
        Assert.Equal(expected, output);
    }
    
    [Fact]
    public void AvgWordLength_ReturnsTheWordLengthForMultipleWords()
    {
        List<string> testWords = new List<string> { };

        var random = new Random();
        
        for (int i = 0; i < 100000; i++)
        {
            int randomNumber = random.Next(0, 1000000);
            testWords.Add(randomNumber.ToString());
        }
        
        var analysisTest =  new MultiThreadingProcessor(testWords);


        float expected = 0;
        
        foreach (string numberword in testWords)
        {
            expected += (float) numberword.Length / testWords.Count;
        }

        double expectedRounded = Math.Round(expected, 1);
        
        var output = analysisTest.Mean();
        
        double outputRounded = Math.Round(output, 1);
        
        Assert.Equal(expectedRounded, outputRounded);
    }
}
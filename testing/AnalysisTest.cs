using dataanalysis;

public class AnalysisTest
{
    [Fact]
    public void AvgWordLength_ReturnsTheWordLengthForOneWord()
    {
        List<string> testWords = new List<string> { "Test" };
        
        var analysisTest =  new Analysis(testWords);

        var expected = 4;

        var output = analysisTest.AvgWordCount;
        
        Assert.Equal(expected, output);
    }
    
    [Fact]
    public void AvgWordLength_ReturnsTheWordLengthForMultipleWords()
    {
        List<string> testWords = new List<string> { "This", "is", "a", "test" };
        
        var analysisTest =  new Analysis(testWords);

        var expected = ("This".Length + "is".Length + "a".Length + "test".Length) / (float) 4;

        var output = analysisTest.AvgWordCount;
        
        Assert.Equal(expected, output);
    }
    
    
}
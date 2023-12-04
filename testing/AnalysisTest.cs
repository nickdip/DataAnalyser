using dataanalysis;

public class AnalysisTest
{
    [Fact]
    public void AvgWordLength_ReturnsTheWordLengthForOneWord()
    {
        List<string> testWords = new List<string> { "Test" };
        
        var analysisTest =  new Analysis(testWords);

        var expected = 4;

        var output = analysisTest.AvgWordLength();
        
        Assert.Equal(expected, output);
    }
}
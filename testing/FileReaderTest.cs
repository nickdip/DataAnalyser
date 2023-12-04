using dataanalysis;
using NSubstitute;

namespace testing;

public class FileReaderTest
{
        
    [Fact]
    public void StringToWords_ReturnsCorrectArrayOfWords()
    {
        var readerTest = new FileReader();

        string input = "This is a test";

        var expected = new List<string> { "this", "is", "a", "test" };
        
        readerTest.StringToWords(input);
        
        Assert.Equal(readerTest.Words, expected);
    }
    
    [Fact]
    public void StringToWords_ReturnsCorrectArrayOfWordsWithPunc()
    {
        var readerTest = new FileReader();

        string input = "This, is a test!";

        var expected = new List<string> { "this", "is", "a", "test" };
        
        readerTest.StringToWords(input);
        
        Assert.Equal(readerTest.Words, expected);
    }
    
    [Fact]
    public void StringToWords_ReturnsCorrectArrayOfWordsWithNumbers()
    {
        var readerTest = new FileReader();

        string input = "This is 1 test!";

        var expected = new List<string> { "this", "is", "1", "test" };
        
        readerTest.StringToWords(input);
        
        Assert.Equal(readerTest.Words, expected);
    }
    
    [Fact]
    public void StringToWords_ReturnsCorrectArrayOfWordsWithHyphenedWords()
    {
        var readerTest = new FileReader();

        string input = "This is a hyphened-test!";

        var expected = new List<string> { "this", "is", "a", "hyphened-test" };
        
        readerTest.StringToWords(input);
        
        Assert.Equal(readerTest.Words, expected);
    }
    
    [Fact]
    public void StringToWords_ReturnsCorrectArrayOfWordsWithRandomHyphens()
    {
        var readerTest = new FileReader();

        string input = "This is a ---hyphened-test----";

        var expected = new List<string> { "this", "is", "a", "hyphened-test" };
        
        readerTest.StringToWords(input);
        
        Assert.Equal(readerTest.Words, expected);
    }

    [Fact]
    public void StringToWords_ReturnsCorrectArrayOfWordsWithMultipleLines()
    
    {
        var readerTest = new FileReader();

        string input =  @"
                        This
                        is
                        a
                        test";

        var expected = new List<string> { "this", "is", "a", "test" };
        
        readerTest.StringToWords(input);
        
        Assert.Equal(readerTest.Words, expected);
    }
    
    [Fact]
    public void StringToWords_ReturnsCorrectArrayOfWordsWithMultipleSpaces()
    
    {
        var readerTest = new FileReader();

        string input = "This       is     a       test";

        var expected = new List<string> { "this", "is", "a", "test" };
        
        readerTest.StringToWords(input);
        
        Assert.Equal(readerTest.Words, expected);
    }
    
    
    

}
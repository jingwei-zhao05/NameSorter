using Xunit;
using System;
using System.IO;

public class NameSorterTests : IDisposable
{
    private readonly string _validInputFile;
    private readonly string _invalidInputFile;
    private readonly string _emptyInputFile;

    public NameSorterTests()
    {
        // Set up the paths for the input files
        _validInputFile = "valid-names.txt";
        _invalidInputFile = "invalid-names.docx";
        _emptyInputFile = "empty-names.txt";

        // Create the valid input file
        File.WriteAllText(_validInputFile, "John Smith\nJane Doe\n");

        // Create the empty input file
        File.WriteAllText(_emptyInputFile, string.Empty);
    }

    public void Dispose()
    {
        // Delete the created input files
        File.Delete(_validInputFile);
        File.Delete(_emptyInputFile);
    }


    [Fact]
    public void SortNames_InvalidInputFile_ThrowsException()
    {
        // Arrange
        string[] args = new[] { _invalidInputFile };

        // Act & Assert
        Assert.Throws<FileNotFoundException>(() => Program.Main(args));
    }

    [Fact]
    public void SortNames_EmptyInputFile_ThrowsException()
    {
        // Arrange
        string[] args = new[] { _emptyInputFile };

        // Act & Assert
        Assert.Throws<InvalidDataException>(() => Program.Main(args));
    }

    [Fact]
    public void SortNames_ShouldSortNamesCorrectly()
    {
        // Arrange
        List<Name> names = new List<Name>
        {
            new Name("Jingwei Zhao"),
            new Name("Janet Parsons"),
            new Name("Vaughn Lewis"),
            new Name("Adonis Julius Archer"),
            new Name("Adonis Jane Archer"),
            new Name("Janet Nolan")
        };
        
        List<Name> expectedSortedNames = new List<Name>
        {
            new Name("Adonis Jane Archer"),
            new Name("Adonis Julius Archer"),
            new Name("Vaughn Lewis"),
            new Name("Janet Nolan"),
            new Name("Janet Parsons"),
            new Name("Jingwei Zhao")
        };
        
        // Act
        names.Sort();
        
        // Assert
        Assert.Equal(expectedSortedNames, names);
    }


    [Fact]
    public void Name_WithValidGivenNameNumbers_ConstructsSuccessfully()
    {
        // Arrange
        string fullName = "Tim Horton Wilson";
        
        // Act
        Name name = new Name(fullName);
        
        // Assert
        Assert.Equal("Tim Horton", string.Join(" ", name.GivenNames));
        Assert.Equal("Wilson", name.LastName);
    }

    [Fact]
    public void Name_WithZeroGivenName_ThrowsArgumentException()
    {
        // Arrange
        string fullName = "Zhao";

        // Act and Assert
        Assert.Throws<ArgumentException>(() => new Name(fullName));
    }
    
    [Fact]
    public void Name_WithInvalidGivenNameNumbers_ThrowsArgumentException()
    {
        // Arrange
        string fullName = "Vincent Jingwei Frank Winnie Zhao";

        // Act and Assert
        Assert.Throws<ArgumentException>(() => new Name(fullName));
    }

    [Fact]
    public void CompareTo_WithSameNames_ReturnsZero()
    {
        // Arrange
        Name name1 = new Name("Jingwei Zhao");
        Name name2 = new Name("Jingwei Zhao");

        // Act
        int result = name1.CompareTo(name2);

        // Assert
        Assert.Equal(0, result);
    }

    [Fact]
    public void CompareTo_WithDifferentLastName_ReturnsCorrectComparisonResult()
    {
        // Arrange
        Name name1 = new Name("Frank Zhang");
        Name name2 = new Name("Jingwei Zhao");

        // Act
        int result = name1.CompareTo(name2);

        // Assert
        Assert.Equal(-1, result);
    }

    [Fact]
    public void CompareTo_WithSameLastNameButDifferentGivenName_ReturnsCorrectComparisonResult()
    {
        // Arrange
        Name name1 = new Name("Jingwei Zhao");
        Name name2 = new Name("Vincent Zhao");

        // Act
        int result = name1.CompareTo(name2);

        // Assert
        Assert.Equal(-12, result);
    }

    [Fact]
    public void CompareTo_WithSameLastNameButDifferentNumberOfGivenNames_ReturnsCorrectComparisonResult()
    {
        // Arrange
        Name name1 = new Name("Jingwei Vincent Zhao");
        Name name2 = new Name("Jingwei Zhao");

        // Act
        int result = name1.CompareTo(name2);

        // Assert
        Assert.Equal(1, result);
    }
}
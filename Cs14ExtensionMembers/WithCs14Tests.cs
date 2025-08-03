using Shouldly;
using System.Reflection;
using System.Text;

namespace Cs14ExtensionMembers.After;
public enum FileSystemEntryType
{
    Directory,
    File
}

public static class StringExtensions
{
    extension(string? receiver)
    {
        public string? FirstCharToUpper()
        {
            if (string.IsNullOrEmpty(receiver))
            {
                return receiver;
            }
            else
            {
                return string.Concat(receiver[0..1].ToUpper(), receiver.AsSpan(1));
            }
        }

        public bool IsEmptyField
        {
            get => string.IsNullOrEmpty(receiver) || receiver == "N/A";
        }
    }

    extension(string?)
    {
        public static string? Create(string? pattern, int count)
        {
            if (pattern == null)
            {
                return null;
            }
            
            if (pattern.Length == 0 || count <= 0)
            {
                return string.Empty;
            }

            var builder = new StringBuilder(pattern.Length * count);
            for (int i = 0; i < count; i++)
            {
                builder.Append(pattern);
            }
            return builder.ToString();
        }

        public static string? operator *(string? pattern, int count)
        {
            return string.Create(pattern, count);
        }
    }
}


public static class DoubleExtensions
{
    extension(double)
    {
        public static double One => 1.0;
    }
}

public static class FloatExtension
{
    extension(float)
    {
        public static float One => 1.0f;
    }
}

public static class PathExtensions
{
    extension(Path)
    {
        public static FileSystemEntryType? GetEntryType(string path)
        {
            if (Directory.Exists(path))
            {
                return FileSystemEntryType.Directory;
            }

            if (File.Exists(path))
            {
                return FileSystemEntryType.File;
            }

            return null;
        }
    }
}

public class WithCs14Tests
{
    [TestCase(null, null)]
    [TestCase("", "")]
    [TestCase("a", "A")]
    [TestCase("foo", "Foo")]
    public void ConvertsFirstCharToUpper(string? input, string? expected)
    {
        input.FirstCharToUpper().ShouldBe(expected);
    }

    [TestCase(null, true)]
    [TestCase("", true)]
    [TestCase("N/A", true)]
    [TestCase("foo", false)]
    public void DeterminesIfStringIsAnEmptyField(string? input, bool expected)
    {
        input.IsEmptyField.ShouldBe(expected);
    }

    [TestCase(null, 1, null)]
    [TestCase("", 1, "")]
    [TestCase("foo", 0, "")]
    [TestCase("foo", 1, "foo")]
    [TestCase("foo", 3, "foofoofoo")]
    public void CreatesANewString(string? pattern, int count, string? expected)
    {
        string.Create(pattern, count).ShouldBe(expected);
    }

    [TestCase(null, 1, null)]
    [TestCase("", 1, "")]
    [TestCase("foo", 0, "")]
    [TestCase("foo", 1, "foo")]
    [TestCase("foo", 3, "foofoofoo")]
    public void MultiplicationRepeatsAString(string? pattern, int count, string? expected)
    {
        (pattern * count).ShouldBe(expected);
    }

    [Test]
    public void ReturnsDoubleOne()
    {
        double.One.ShouldBe(1);
    }

    [Test]
    public void ReturnsFloatOne()
    {
        float.One.ShouldBe(1);
    }

    [Test]
    public void ReturnsDirectoryForValidDirectory()
    {
        Path.GetEntryType(TestContext.CurrentContext.WorkDirectory).ShouldBe(FileSystemEntryType.Directory);
    }

    [Test]
    public void ReturnsFileForValidFile()
    {
        Path.GetEntryType(Assembly.GetExecutingAssembly().Location).ShouldBe(FileSystemEntryType.File);
    }

    [Test]
    public void ReturnsNullForInvalidPath()
    {
        Path.GetEntryType("invalid").ShouldBeNull();
    }
}

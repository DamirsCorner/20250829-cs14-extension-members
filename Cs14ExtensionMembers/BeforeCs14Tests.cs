using Shouldly;

namespace Cs14ExtensionMembers.Before;

public static class StringExtensions
{
    public static string? FirstCharToUpper(this string? receiver)
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
}

public class BeforeCs14Tests
{
    [TestCase(null, null)]
    [TestCase("", "")]
    [TestCase("a", "A")]
    [TestCase("foo", "Foo")]
    public void ConvertsFirstCharToUpper(string? input, string? expected)
    {
        input.FirstCharToUpper().ShouldBe(expected);
    }
}

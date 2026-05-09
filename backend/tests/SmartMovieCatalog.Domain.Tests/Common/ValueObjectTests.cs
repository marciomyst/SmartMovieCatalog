using SmartMovieCatalog.Domain.Common;

namespace SmartMovieCatalog.Domain.Tests.Common;

public sealed class ValueObjectTests
{
    [Fact]
    public void Equals_WithSameTypeAndSameComponents_ReturnsTrue()
    {
        SampleValueObject left = new("same", 7);
        SampleValueObject right = new("same", 7);

        Assert.True(left.Equals(right));
        Assert.Equal(left.GetHashCode(), right.GetHashCode());
    }

    [Fact]
    public void Equals_WithDifferentComponents_ReturnsFalse()
    {
        SampleValueObject left = new("left", 7);
        SampleValueObject right = new("right", 8);

        Assert.False(left.Equals(right));
    }

    [Fact]
    public void Equals_WithDifferentRuntimeType_ReturnsFalse()
    {
        SampleValueObject sample = new("same", 7);
        OtherValueObject other = new("same", 7);

        Assert.False(sample.Equals(other));
    }

    private sealed class SampleValueObject : ValueObject
    {
        private readonly string _name;
        private readonly int _version;

        public SampleValueObject(string name, int version)
        {
            _name = name;
            _version = version;
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return _name;
            yield return _version;
        }
    }

    private sealed class OtherValueObject : ValueObject
    {
        private readonly string _name;
        private readonly int _version;

        public OtherValueObject(string name, int version)
        {
            _name = name;
            _version = version;
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return _name;
            yield return _version;
        }
    }
}

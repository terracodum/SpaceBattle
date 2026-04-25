namespace StarWars.Lib;

public class Vector
{
    private readonly int[] elements;
    public Vector(int[] elements)
    {
        this.elements = elements;
    }

    public int[] GetElements()
    {
        return elements;
    }

    public static Vector operator +(Vector cv1, Vector cv2)
    {
        if (cv1.elements.Length != cv2.elements.Length)
        {
            throw new ArgumentException("Vectors length is different");
        }

        var result = cv1.elements.Zip(cv2.elements, (a, b) => a + b).ToArray();

        return new Vector(result);
    }

    public static bool operator ==(Vector cv1, Vector cv2)
    {
        if (ReferenceEquals(cv1, cv2))
        {
            return true;
        }

        if (cv1 is null || cv2 is null)
        {
            return false;
        }

        return cv1.Equals(cv2);
    }

    public static bool operator !=(Vector cv1, Vector cv2)
    {
        return !(cv1 == cv2);
    }

    public override bool Equals(object? obj)
    {
        if (obj is not Vector other)
        {
            return false;
        }

        if (elements.Length != other.elements.Length)
        {
            return false;
        }

        return elements.SequenceEqual(other.elements);
    }

    public override int GetHashCode() => elements.Aggregate(0, HashCode.Combine);
}

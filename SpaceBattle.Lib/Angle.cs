namespace StarWars.Lib;

public class Angle
{
    private readonly int _numerator;
    private static readonly int Denominator = 8;

    public Angle(int num)
    {
        _numerator = (num % Denominator + Denominator) % Denominator;
    }
    public int GetNumerator()
    {
        return _numerator;
    }

    public static implicit operator double(Angle angle)
    {
        return ((double)angle._numerator / Denominator) * 2 * Math.PI;
    }

    public static Angle operator +(Angle a1, Angle a2)
    {
        return new Angle(a1._numerator + a2._numerator);
    }

    public static bool operator ==(Angle a1, Angle a2)
    {
        return a1.Equals(a2);
    }

    public static bool operator !=(Angle a1, Angle a2)
    {
        return !(a1 == a2);
    }

    public override bool Equals(object? obj)
    {
        if (obj is not Angle other)
        {
            return false;
        }

        return _numerator.Equals(other._numerator);
    }

    public override int GetHashCode()
    {
        return _numerator.GetHashCode();
    }
}


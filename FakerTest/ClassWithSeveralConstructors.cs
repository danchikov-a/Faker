namespace FakerTest;

public class ClassWithSeveralConstructors
{
    private int i;
    private double d;

    public ClassWithSeveralConstructors(int i)
    {
        this.i = i;
    }

    public ClassWithSeveralConstructors(int i, double d)
    {
        this.i = i;
        this.d = d;
    }

    public int GetI()
    {
        return i;
    }
    
    public double GetD()
    {
        return d;
    }
}
namespace FakerTest;

public class SimpleClass
{
    public int i;
    public double d;

    public int IWithPrivateSet
    {
        get;
        private set;
    }
    
    public int IWithPublicSet
    {
        get; set;
    }
}
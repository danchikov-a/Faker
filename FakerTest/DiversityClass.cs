namespace FakerTest;

public class DiversityClass
{
    public int i;
    public double d;
    public float f;
    public long l;
    public string s;
    public DateOnly date;
    public DateTime dateTime;
    
    public List<double> foos;
    
    public List<double> Foos
    {
        set;
        get;
    }

    private int iPr;
    public SimpleClass simpleClass;

    
    public int GetIPr()
    {
        return iPr;
    }
    
    public DiversityClass()
    {
    }

    public int I
    {
        set;
        get;
    }
}
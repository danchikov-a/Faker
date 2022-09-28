using System.Text;

namespace Faker.Generator.impl;

class StringGenerator : IGenerator
{
    private const int MIN_LENGTH = 0;
    private const int MAX_LENGTH = 255;
    private const int FIRST_NORMAL_SYMBOL = 33;
    private const int LAST_NORMAL_SYMBOL = 90;
    
    public override object Generate()
    {
        string str = "";
        int textLength = random.Next(MIN_LENGTH, MAX_LENGTH);
        
        string symbols = Encoding.ASCII.GetString(
            Enumerable.Range(FIRST_NORMAL_SYMBOL, LAST_NORMAL_SYMBOL)
            .Select(n => (byte) n)
            .ToArray()
        );
   
        for (int i = 0; i < textLength; i++)
        {
            str += symbols[random.Next(0, symbols.Length - 1)];
        }

        return str;
    }
}
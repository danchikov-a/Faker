namespace Faker.Generator.impl;

class DateGenerator : IGenerator
{
    private const int MAX_DAYS = 28;
    private const int MAX_MONTHS = 12;
    private const int MAX_YEARS = 9999;
   
    public override object Generate()
    {
        int day = random.Next(1, MAX_DAYS);
        int month = random.Next(1, MAX_MONTHS);
        int year = random.Next(1, MAX_YEARS);
        
        return new DateOnly(year, month, day);
    }
}
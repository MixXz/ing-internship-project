using Bogus;
using VacaYAY.API.Contracts;
using VacaYAY.API.Entities;

namespace VacaYAY.API.Services;

public class DataGenService : IDataGenService
{
    public DataGenService() { }

    public List<Employee> GenerateEmployees(int count)
    {
        var faker = new Faker<Employee>()
                        .RuleFor(e => e.FirstName, f => f.Person.FirstName)
                        .RuleFor(e => e.LastName, f => f.Person.LastName)
                        .RuleFor(e => e.Address, f => f.Address.FullAddress())
                        .RuleFor(e => e.IDNumber, f => f.Random.Number(100000, 999999).ToString())
                        .RuleFor(e => e.DaysOffNumber, f => f.Random.Number(0, 100))
                        .RuleFor(e => e.EmployeeStartDate, f => f.Date.Past())
                        .RuleFor(e => e.EmployeeEndDate, f => f.Date.Recent(30))
                        .RuleFor(e => e.InsertDate, f => f.Date.Past())
                        .RuleFor(e => e.Position, f => f.PickRandom(GeneratePositions(count)))
                        .RuleFor(e => e.Email, (f, e) => f.Internet.Email(e.FirstName.ToLower(), e.LastName.ToLower()));

        return faker.Generate(count);
    }

    public List<Position> GeneratePositions(int count)
    {
        var positions = new List<Position>();

        for(int i = 0; i < count; i++)
        {
            positions.Add(GeneratePosition());
        }

        return positions;
    }

    public Position GeneratePosition()
    {
        string[] captions = {
            "Software Engineer",
            "Project Manager",
            "Data Analyst",
            "Marketing Specialist",
            "Sales Representative",
            "HR Manager"
        };

        string[] descriptions = {
            "Responsible for developing software applications.",
            "Leading project teams and ensuring project success.",
            "Analyzing and interpreting data to drive insights.",
            "Executing marketing campaigns and strategies.",
            "Promoting and selling products or services.",
            "Managing HR operations and employee relations."
        };

        int id = new Random().Next(0, captions.Length);

        return new Position
        {
            ID = id,
            Caption = captions[id],
            Description = descriptions[id]
        };
    }
}

using Bogus;
using Bogus.DataSets;
using ConsoleApp;
using ConsoleApp.Models;

namespace ConsoleClient;

public class PtientCreator
{
    private static readonly List<String> femaleSurnames = new(10)
    {
        "Сергеевна", "Владимировна", "Семёновна", "Дмитриевна", "Александровна",
        "Ивановна", "Генадьевна", "Анатольевна", "Петровна", "Кузьминична"
    };

    private static readonly List<String> maleSurnames = new(10)
    {
        "Сергеевич", "Владимирович", "Семёновнович", "Дмитриевич", "Александрович",
        "Иванович", "Генадьевич", "Анатольевич", "Петрович", "Валерьевич"
    };


    static public List<Patient> CreatePatients(int count)
    {
        string locale = "ru";
        var patients = new List<Patient>(count);

        var random = new Random();

        var patientGenerator = new Faker<Patient>(locale)
            .StrictMode(true)
            .RuleFor(i => i.Gender, f => f.PickRandom<Genders>())
            .RuleFor(i => i.BirthDate, f => f.Person.DateOfBirth)
            .RuleFor(i => i.Active, f => f.Random.Bool())
            .Ignore(i => i.Name);

        var nameFemaleGenerator = new Faker<PatientName>(locale)
            .StrictMode(true)
            .RuleFor(i => i.Family, f => f.Name.LastName(Name.Gender.Female))
            .RuleFor(i => i.Use, set => "official")
            .RuleFor(i => i.Given, f => f.Make(1, () => f.Name.FirstName(Name.Gender.Female)));

        var nameMaleGenerator = new Faker<PatientName>(locale)
            .StrictMode(true)
            .RuleFor(i => i.Family, f => f.Name.LastName(Name.Gender.Male))
            .RuleFor(i => i.Use, set => "official")
            .RuleFor(i => i.Given, f => f.Make(1, () => f.Name.FirstName(Name.Gender.Male)));


        for (int i = 0; count > i; i++)
        {
            var facePatient = patientGenerator.Generate();

            if(facePatient.Gender == Genders.Male)
            {
                facePatient.Name = nameMaleGenerator.Generate();

                int index = random.Next(maleSurnames.Count);
                facePatient.Name.Given.Add(maleSurnames[index]);
            }
            else if (facePatient.Gender == Genders.Female)
            {
                facePatient.Name = nameFemaleGenerator.Generate();
                int index = random.Next(femaleSurnames.Count);
                facePatient.Name.Given.Add(femaleSurnames[index]);
            }
            else
            {
                facePatient.Name = nameMaleGenerator.Generate();
                int index = random.Next(femaleSurnames.Count);
                facePatient.Name.Given.Add(femaleSurnames[index]);
            }

            patients.Add(facePatient);
        }

        return patients;
    }
}

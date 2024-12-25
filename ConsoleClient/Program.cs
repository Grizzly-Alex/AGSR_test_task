using ConsoleApp.Models;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;


namespace ConsoleClient;

class Program
{
    static readonly HttpClient Client = new HttpClient();    


    static async Task Main(string[] args)
    {
        Url url = await GetUrl();

        if (string.IsNullOrEmpty(url.Scheme) && string.IsNullOrEmpty(url.Host))
            return;

        Client.BaseAddress = new Uri($"{url.Scheme}://{url.Host}");
        Client.DefaultRequestHeaders.Accept.Clear();
        Client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

        var patientList = PtientCreator.CreatePatients(100);

        await PostBatch(patientList, url.Path);

        Console.ReadLine();
         
    }


    private static async Task<Url> GetUrl()
    {
        string basePath = AppDomain.CurrentDomain.BaseDirectory;
        string urlPath = Path.Combine(basePath, "url.json");

        using (FileStream fs = File.OpenRead(urlPath))
        {
            var options = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            };

            return await JsonSerializer.DeserializeAsync<Url>(fs, options)
                ?? throw new FileNotFoundException();
        }
    }


    private static async Task PostBatch(List<Patient> patients, string path)
    {
        if (!patients.Any())
            Console.WriteLine("The patient list is empty");

        if (string.IsNullOrEmpty(path))
            Console.WriteLine("Request path not provided");


        try
        {
            var response = await Client.PostAsJsonAsync(path, patients);
            response.EnsureSuccessStatusCode();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Adding patients successfully! Status code {response.StatusCode}");

        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Error.WriteLine($"Adding patients error: {ex.Message}");
        }

        Console.ResetColor();
    }
}

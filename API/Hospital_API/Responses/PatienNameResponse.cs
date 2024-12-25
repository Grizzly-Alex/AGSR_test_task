namespace Hospital_API.Responses
{
    public record class PatienNameResponse(Guid Id, string Use, string Family, List<string> Given);
}

using Hospital_API.Constants;


namespace Hospital_API.Responses;

public record class PatientResponse(
    PatienNameResponse Name,
    Genders Gender,
    DateTime BirthDate,
    bool Active);
namespace Hospital_API.Constants;

public enum Genders : byte
{
    Unknown = 0,
    Male = 1,
    Female = 2,
    Other = 3,
}

public enum SearchingPrefix : byte
{
    Eq = 1,
    Ne = 2,
    Lt = 3,
    Gt = 4,
    Ge = 5,
    Le = 6,
    Sa = 7,
    Eb = 8,
    Ap = 9,
}
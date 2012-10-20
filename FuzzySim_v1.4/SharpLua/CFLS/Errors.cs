
namespace CFLS
{
    public static class Errors
    {
      public static string[]  CFLS_errortext =    
        {
            "00-All is well",
            "01-Range must not be greater than 10,000",
            "02-Range must not be less than than 0.01",
            "03-Fuzzy set is missing low range entry",
            "04-Fuzzy set is missing high range entry",
            "05-High range is lower than low range",
            "06-Fuzzy set must have a minimum of two points",
            "07-Index is out of range",
            "08-Fuzzy set contains no points",
            "09-Fuzzy set pointer is null",
            "10-Input Fuzzy set is invalid",
            "11-Ranges of fuzzy sets must be the same",
            "12-World value is out of range",
            "13-Set value is outside range 0..1",
            "14-Set already has maximum number of points",
            "15-Divisor is zero",
            "16-Opcode out of range",
            "17-Clip range is 0 .. 1",
            "18-Set value range is 0 .. 1",
            "19-Set has two low range entries",
            "20-Set has two high range entries",
            ""
        };
    }
}

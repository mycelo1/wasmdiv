namespace wasmdiv.Interfaces;

public interface IAppState
{
    string? BaseUrl { get; set; }
    double Dividend { get; set; }
    double Divisor { get; set; }
    int DecimalDigits { get; set; }
    bool NeedCalc { get; set; }
    string? Result { get; }
    void Calc();
}
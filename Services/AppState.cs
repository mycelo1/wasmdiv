using System.Globalization;
using System.Text;

namespace wasmdiv.Services;

public class AppState : IAppState
{
    public string? BaseUrl { get; set; }
    public double Dividend { get; set; } = 10;
    public double Divisor { get; set; } = 10;
    public int DecimalDigits { get; set; } = 2;
    public bool NeedCalc { get; set; } = true;
    public string? Result { get {return _result;} }
    private string? _result;

    public void Calc()
    {
        var resultText = new StringBuilder();
        char decimalChar = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator[0];

        double useDividend = Dividend;
        double currentDivisor = Divisor;
        int divisorDigitsAfterPoint = 0;
        int parallelFactor = 1;

        while (currentDivisor != Math.Truncate(currentDivisor))
        {
            useDividend = Math.Round(useDividend * 10, 5);
            currentDivisor = Math.Round(currentDivisor * 10, 5);
            parallelFactor *= 10;
            divisorDigitsAfterPoint++;
        }

        long useDivisor = Convert.ToInt64(Math.Round(currentDivisor));
        useDividend = Math.Round(useDividend, DecimalDigits);

        long effectiveDividend = Convert.ToInt64(Math.Round(useDividend * Math.Pow(10, DecimalDigits)));
        string dividendText = effectiveDividend.ToString();
        int dividendTextLength = dividendText.Length;
        int decimalPointPosition = dividendTextLength - DecimalDigits;

        int currentDigit;
        long currentRemainder;
        long currentDividend = 0;
        var divisionChain = new StringBuilder();

        if (useDividend >= useDivisor)
        {
            currentDigit = dividendTextLength;
            currentRemainder = Int64.MaxValue;
            do
            {
                currentDigit--;
                if (currentDigit >= 0)
                {
                        currentRemainder = Convert.ToInt64(dividendText.Substring(0, currentDigit + 1));
                }
                else
                {
                        currentRemainder = 0;
                }
            }
            while ((currentDigit >= 0) && (currentRemainder >= useDivisor));
            currentDigit++;
        }
        else
        {
            currentDigit = decimalPointPosition - 1;
            currentRemainder = Convert.ToInt64(Math.Truncate(useDividend / 10));
        }

        resultText.Append(' ');
        resultText.AppendLine(new String('-', currentDigit + 1));

        resultText.Append(' ');
        resultText.Append(dividendText);
        resultText.Append('|');
        resultText.AppendLine($"{useDivisor}");

        resultText.Append(new String(' ', decimalPointPosition));
        resultText.Append(decimalChar);
        resultText.Append(new String(' ', dividendTextLength - decimalPointPosition));
        resultText.Append('+');
        resultText.AppendLine(new String('-', dividendTextLength + 1));

        var operationText = new StringBuilder();

        while (currentDigit < dividendText.Length)
        {
            currentDividend = (currentRemainder * 10) + Convert.ToInt64(dividendText[currentDigit].ToString());

            long currentResultDigit = currentDividend / useDivisor;
            long currentSubtractor = currentResultDigit * useDivisor;
            currentRemainder = currentDividend - currentSubtractor;

            string currentDividendText;
            if ((currentDividend < 10) && (currentDigit > 0))
            {
                currentDividendText = $"0{currentDividend}";
            }
            else
            {
                currentDividendText = currentDividend.ToString();
            }

            int currentDividendLength = currentDividendText.Length;
            int currentSubtractorLength = currentSubtractor.ToString().Length;
            int currentRemainderLength = currentRemainder.ToString().Length;

            if (currentDigit == decimalPointPosition)
            {
                if (operationText.Length == 0)
                {
                    operationText.Append('0');
                }
                operationText.Append(decimalChar);
            }

            if ((currentResultDigit != 0) || (operationText.Length == 0) || (operationText.ToString().IndexOf(decimalChar) > 0) || (Convert.ToInt64("0" + operationText.ToString()) > 0))
            {
                operationText.Append(currentResultDigit.ToString());
            }

            int padding = currentDigit - currentDividendLength + 2;

            divisionChain.Append(new String(' ', padding));
            divisionChain.AppendLine(currentDividendText);

            divisionChain.Append(new String(' ', padding - 1));
            divisionChain.Append('-');
            divisionChain.Append(new String(' ', currentDividendLength - currentSubtractorLength));
            divisionChain.Append($"{currentSubtractor}");
            divisionChain.Append(' ');
            divisionChain.AppendLine($"({currentResultDigit} x {useDivisor})");

            divisionChain.Append(new String(' ', padding));
            divisionChain.Append(new String('-', currentDividendLength));
            divisionChain.Append('v');
            divisionChain.AppendLine();
            currentDigit++;
        }

        divisionChain.Append(new String(' ', Math.Max(currentDigit - currentRemainder.ToString().Length + 1, 1)));
        divisionChain.AppendLine($"{currentRemainder}");

        resultText.Append(new String(' ', dividendTextLength + 2));
        resultText.AppendLine(operationText.ToString());
        resultText.AppendLine(divisionChain.ToString());
        _result = resultText.ToString();
    }
}
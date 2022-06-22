namespace docs.Classes;

public static class MoneyConverter
{
    public enum TextCase { Nominative/*Кто? Что?*/, Genitive/*Кого? Чего?*/, Dative/*Кому? Чему?*/, Accusative/*Кого? Что?*/, Instrumental/*Кем? Чем?*/, Prepositional/*О ком? О чём?*/ };

    static readonly string zero = "ноль";
    static readonly string firstMale = "один";
    static readonly string firstFemale = "одна";
    static readonly string firstFemaleAccusative = "одну";
    static readonly string firstMaleGenetive = "одно";
    static readonly string secondMale = "два";
    static readonly string secondFemale = "две";
    static readonly string secondMaleGenetive = "двух";
    static readonly string secondFemaleGenetive = "двух";

    static readonly string[] from3till19 = { "", "три", "четыре", "пять", "шесть", "семь", "восемь", "девять", "десять", "одиннадцать", "двенадцать", "тринадцать", "четырнадцать", "пятнадцать", "шестнадцать", "семнадцать", "восемнадцать", "девятнадцать" };
    static readonly string[] from3till19Genetive = { "", "трех", "четырех", "пяти", "шести", "семи", "восьми", "девяти", "десяти", "одиннадцати", "двенадцати", "тринадцати", "четырнадцати", "пятнадцати", "шестнадцати", "семнадцати", "восемнадцати", "девятнадцати" };
    static readonly string[] tens = { "", "двадцать", "тридцать", "сорок", "пятьдесят", "шестьдесят", "семьдесят", "восемьдесят", "девяносто" };
    static readonly string[] tensGenetive = { "", "двадцати", "тридцати", "сорока", "пятидесяти", "шестидесяти", "семидесяти", "восьмидесяти", "девяноста" };
    static readonly string[] hundreds = { "", "сто", "двести", "триста", "четыреста", "пятьсот", "шестьсот", "семьсот", "восемьсот", "девятьсот" };
    static readonly string[] hundredsGenetive = { "", "ста", "двухсот", "трехсот", "четырехсот", "пятисот", "шестисот", "семисот", "восьмисот", "девятисот" };
    static readonly string[] thousands = { "", "тысяча", "тысячи", "тысяч" };
    static readonly string[] thousandsAccusative = { "", "тысячу", "тысячи", "тысяч" };
    static readonly string[] millions = { "", "миллион", "миллиона", "миллионов" };
    static readonly string[] billions = { "", "миллиард", "миллиарда", "миллиардов" };
    static readonly string[] trillions = { "", "трилион", "трилиона", "триллионов" };
    static readonly string[] rubles = { "", "рубль", "рубля", "рублей" };
    static readonly string[] copecks = { "", "копейка", "копейки", "копеек" };

    /// <summary>
    /// Десять тысяч рублей 67 копеек
    /// </summary>
    public static string CurrencyToTxt(decimal _amount)
    {
        //Десять тысяч рублей 67 копеек
        long rublesAmount = (long)Math.Floor(_amount);
        long copecksAmount = (long)Math.Round(_amount * 100) % 100;
        int lastRublesDigit = LastDigit(rublesAmount);
        int lastCopecksDigit = LastDigit(copecksAmount);

        string s = NumeralsToTxt(rublesAmount, TextCase.Nominative, true) + " ";

        if (IsPluralGenitive(lastRublesDigit))
        {
            s += rubles[3] + " ";
        }
        else if (IsSingularGenitive(lastRublesDigit))
        {
            s += rubles[2] + " ";
        }
        else
        {
            s += rubles[1] + " ";
        }

        s += string.Format("{0:00} ", copecksAmount);

        if (IsPluralGenitive(lastCopecksDigit))
        {
            s += copecks[3] + " ";
        }
        else if (IsSingularGenitive(lastCopecksDigit))
        {
            s += copecks[2] + " ";
        }
        else
        {
            s += copecks[1] + " ";
        }
        var result = s.Trim();
        return string.Concat(result[0].ToString().ToUpper(), result.AsSpan(1));
    }

    /// <summary>
    /// реализовано для падежей: именительный (nominative), родительный (Genitive),  винительный (accusative)
    /// </summary>
    public static string NumeralsToTxt(long _sourceNumber, TextCase _case, bool _isMale)
    {
        if (_sourceNumber == 0)
            return zero;

        string s = "";
        long number = _sourceNumber;
        int remainder;
        int power = 0;

        if (number >= (long)Math.Pow(10, 15) || number < 0)
        {
            return "";
        }

        while (number > 0)
        {
            remainder = (int)(number % 1000);
            number /= 1000;

            switch (power)
            {
                case 12:
                    s = MakeText(remainder, hundreds, tens, from3till19, secondMale, firstMale, trillions) + s;
                    break;
                case 9:
                    s = MakeText(remainder, hundreds, tens, from3till19, secondMale, firstMale, billions) + s;
                    break;
                case 6:
                    s = MakeText(remainder, hundreds, tens, from3till19, secondMale, firstMale, millions) + s;
                    break;
                case 3:
                    s = _case switch
                    {
                        TextCase.Accusative => MakeText(remainder, hundreds, tens, from3till19, secondFemale, firstFemaleAccusative, thousandsAccusative) + s,
                        _ => MakeText(remainder, hundreds, tens, from3till19, secondFemale, firstFemale, thousands) + s,
                    };
                    break;
                default:
                    string[] powerArray = Array.Empty<string>();
                    s = _case switch
                    {
                        TextCase.Genitive => MakeText(remainder, hundredsGenetive, tensGenetive, from3till19Genetive, _isMale ? secondMaleGenetive : secondFemaleGenetive, _isMale ? firstMaleGenetive : firstFemale, powerArray) + s,
                        TextCase.Accusative => MakeText(remainder, hundreds, tens, from3till19, _isMale ? secondMale : secondFemale, _isMale ? firstMale : firstFemaleAccusative, powerArray) + s,
                        _ => MakeText(remainder, hundreds, tens, from3till19, _isMale ? secondMale : secondFemale, _isMale ? firstMale : firstFemale, powerArray) + s,
                    };
                    break;
            }
            power += 3;
        }
        return s.Trim();
    }

    static string MakeText(int _digits, string[] _hundreds, string[] _tens, string[] _from3till19, string _second, string _first, string[] _power)
    {
        string s = "";
        int digits = _digits;

        if (digits >= 100)
        {
            s += _hundreds[digits / 100] + " ";
            digits %= 100;
        }
        if (digits >= 20)
        {
            s += _tens[digits / 10 - 1] + " ";
            digits %= 10;
        }

        if (digits >= 3)
        {
            s += _from3till19[digits - 2] + " ";
        }
        else if (digits == 2)
        {
            s += _second + " ";
        }
        else if (digits == 1)
        {
            s += _first + " ";
        }

        if (_digits != 0 && _power.Length > 0)
        {
            digits = LastDigit(_digits);

            if (IsPluralGenitive(digits))
            {
                s += _power[3] + " ";
            }
            else if (IsSingularGenitive(digits))
            {
                s += _power[2] + " ";
            }
            else
            {
                s += _power[1] + " ";
            }
        }

        return s;
    }

    static bool IsPluralGenitive(int _digits)
    {
        if (_digits >= 5 || _digits == 0)
            return true;

        return false;
    }

    static bool IsSingularGenitive(int _digits)
    {
        if (_digits >= 2 && _digits <= 4)
            return true;

        return false;
    }

    static int LastDigit(long _amount)
    {
        long amount = _amount;

        if (amount >= 100)
            amount %= 100;

        if (amount >= 20)
            amount %= 10;

        return (int)amount;
    }
}

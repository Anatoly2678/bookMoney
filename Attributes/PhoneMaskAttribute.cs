namespace BookMoney.Attributes;

using System.ComponentModel.DataAnnotations;

public class PhoneMaskAttribute : ValidationAttribute
{
    private const string Pattern = @"^\+7\d{10}$";

    public PhoneMaskAttribute() : base("Номер телефона должен быть в формате: +7XXXXXXXXXX")
    {
    }

    public override bool IsValid(object value)
    {
        if (value == null) return true; // RequiredAttribute обработает null

        if (value is string phone)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(phone, Pattern);
        }

        return false;
    }
}
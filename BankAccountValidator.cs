using BankAccountValidation;
using System.Collections.Generic;
using System.Linq;

namespace BankAccountValidator
{
    public static class BankAccountValidator
    {
        internal record CheckDigitBankCalcExceptions(string FromCheckDigit, string ToCheckDigit)
        {
            internal CheckDigitBankCalcExceptions() : this(default, default) { }
        };

        internal record BankValitationRules(int AgencyNumberLenght, int[] AgencyDigitMultipliers, int AccountNumberLenght, int[] AccountDigitMultipliers, List<CheckDigitBankCalcExceptions> CheckDigitBankCalcExceptions)
        {
            internal BankValitationRules() : this(default, default, default, default, default) { }
        }

        internal static List<string> Errors { get; set; } = new();

        public static List<string> Validate(this BankAccount bankAccount)
        {
            BankValitationRules bankValitationRules;

            switch (bankAccount.BankCode)
            {
                case "001":
                    bankValitationRules = new()
                    {
                        AgencyNumberLenght = 4,
                        AgencyDigitMultipliers = new int[] { 5, 4, 3, 2 },
                        AccountNumberLenght = 8,
                        AccountDigitMultipliers = new int[] { 9, 8, 7, 6, 5, 4, 3, 2 },
                        CheckDigitBankCalcExceptions = new List<CheckDigitBankCalcExceptions>() { new("10", "X"), new("11", "0") }
                    };
                    break;
                default:
                    Errors.Add(string.IsNullOrWhiteSpace(bankAccount.BankCode) ? "Código Bancário Obrigatório" : "Banco não Suportado");
                    return Errors;
            }

            ValidateAgencyNumber(bankAccount.AgencyNumber, bankAccount.AgencyNumberCheckDigit, bankValitationRules);
            ValidateAccountNumber(bankAccount.AccountNumber, bankAccount.AccountNumberCheckDigit, bankValitationRules);

            return Errors;
        }

        private static void ValidateAgencyNumber(string AgencyNumber, string AgencyNumberCheckDigit, BankValitationRules bankValitationRules)
        {
            if (AgencyNumber.Length != bankValitationRules.AgencyNumberLenght)
            {
                Errors.Add($"Agência inválida: deve conter {bankValitationRules.AgencyNumberLenght} dígitos.");
                return;
            }

            string calculatedCheckDigit = CalculateMod11CheckDigit(AgencyNumber, bankValitationRules.AgencyDigitMultipliers, bankValitationRules.CheckDigitBankCalcExceptions);

            if (AgencyNumberCheckDigit != calculatedCheckDigit)
                Errors.Add($"Agência inválida: dígito verificador incorreto.");
        }

        private static void ValidateAccountNumber(string AccountNumber, string AccountNumberCheckDigit, BankValitationRules bankValitationRules)
        {
            if (AccountNumber.Length != bankValitationRules.AccountNumberLenght)
            {
                Errors.Add($"Conta inválida: deve conter {bankValitationRules.AccountNumberLenght} dígitos.");
                return;
            }

            string calculatedCheckDigit = CalculateMod11CheckDigit(AccountNumber, bankValitationRules.AccountDigitMultipliers, bankValitationRules.CheckDigitBankCalcExceptions);

            if (AccountNumberCheckDigit != calculatedCheckDigit)
                Errors.Add($"Conta inválida: dígito verificador incorreto.");
        }

        private static string CalculateMod11CheckDigit(string number, int[] multipliers, List<CheckDigitBankCalcExceptions> checkDigitBankCalcExceptions)
        {
            var sum = 0;
            for (int i = 0; i < multipliers.Length; i++)
            {
                sum += multipliers[i] * int.Parse(number[i].ToString());
            }

            int remainder = (sum % 11);

            string checkDigit = (11 - remainder).ToString();

            CheckDigitBankCalcExceptions checkDigitBankCalcException = checkDigitBankCalcExceptions.SingleOrDefault(e => e.FromCheckDigit == checkDigit);

            return checkDigitBankCalcException is null ? checkDigit : checkDigitBankCalcException.ToCheckDigit;
        }
    }
}
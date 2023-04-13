using BankAccountValidator;
using System;
using System.Collections.Generic;

namespace BankAccountValidation
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Digite o código do banco:");
            string bankCode = Console.ReadLine();

            Console.WriteLine("Digite o número da agência:");
            string agency = Console.ReadLine();

            Console.WriteLine("Digite o dígito verificador da agência:");
            string agencyDigit = Console.ReadLine();

            Console.WriteLine("Digite o número da conta:");
            string account = Console.ReadLine();

            Console.WriteLine("Digite o dígito verificador da conta:");
            string accountDigit = Console.ReadLine();

            BankAccount bankAccount = new()
            {
                BankCode = bankCode,
                AgencyNumber = agency,
                AgencyNumberCheckDigit = agencyDigit,
                AccountNumber = account,
                AccountNumberCheckDigit = accountDigit
            };

            List<string> errors = bankAccount.Validate();

            if (errors.Count == 0)
            {
                Console.WriteLine("Os dados da conta bancária são válidos!");
            }
            else
            {
                Console.WriteLine("Os seguintes erros foram encontrados:");

                foreach (string error in errors)
                {
                    Console.WriteLine(error);
                }
            }

            Console.ReadLine();
        }
    }
}

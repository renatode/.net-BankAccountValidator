namespace BankAccountValidation
{
    public class BankAccount
    {
        public string BankCode { get; set; }
        public string AgencyNumber { get; set; }
        public string AgencyNumberCheckDigit { get; set; }
        public string AccountNumber { get; set; }
        public string AccountNumberCheckDigit { get; set; }
    }
}

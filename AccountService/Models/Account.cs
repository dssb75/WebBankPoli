namespace AccountService.Models
{
    public class Account
    {
        public int Id { get; set; }
        public string Owner { get; set; } = string.Empty;
        public decimal Balance { get; set; }
    }
}

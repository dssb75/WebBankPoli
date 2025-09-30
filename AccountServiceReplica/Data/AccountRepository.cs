using AccountServiceReplica.Models;

namespace AccountServiceReplica.Data
{
    public static class AccountRepository
    {
        private static readonly List<Account> _accounts = new List<Account>
        {
            new Account { Id = 1, Owner = "Alice", Balance = 1200 },
            new Account { Id = 2, Owner = "Bob", Balance = 800 }
        };

        public static IEnumerable<Account> GetAll() => _accounts;

        public static Account? GetById(int id) => _accounts.FirstOrDefault(a => a.Id == id);

        public static void Add(Account account) => _accounts.Add(account);

        public static void Update(Account updatedAccount)
        {
            var existing = GetById(updatedAccount.Id);
            if (existing != null)
            {
                existing.Owner = updatedAccount.Owner;
                existing.Balance = updatedAccount.Balance;
            }
        }

        public static void Delete(int id)
        {
            var account = GetById(id);
            if (account != null)
            {
                _accounts.Remove(account);
            }
        }
    }
}

using SandPersistence;

namespace SandPersistenceTest
{
    internal class Program
    {
        private static void Main()
        {
            using (var db = new SandDbContext())
            {
                var account = db.Accounts.Create();

                account.Username = "231blargs";
                account.Password = "blarg";

                db.Accounts.Add(account);

                db.SaveChanges();
            }
        }
    }
}
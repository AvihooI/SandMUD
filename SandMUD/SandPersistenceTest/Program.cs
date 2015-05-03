using SandPersistence;

namespace SandPersistenceTest
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            using (var db = new SandDbContext())
            {
                var account = db.Accounts.Create();

                account.Username = "AvihooI";
                account.Password = "blarg";

                db.SaveChanges();
            }
        }
    }
}
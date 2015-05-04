using System.Data.Entity;
using SandModel;

namespace SandPersistence
{
    public class SandDbContext : DbContext
    {
        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Character> Characters { get; set; }
        public virtual DbSet<TemplateItem> TemplateItems { get; set; }
        public virtual DbSet<InventoryItem> InventoryItems { get; set; }
        public virtual DbSet<Npc> Npcs { get; set; }
        public virtual DbSet<PlayerCharacter> PlayerCharacters { get; set; }
        public virtual DbSet<Room> Rooms { get; set; }
        public virtual DbSet<Script> Scripts { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>().Property(p => p.Username)
                .IsRequired()
                .HasMaxLength(15);

            modelBuilder.Entity<Account>().HasKey(p => p.Username); //Make usernames unique

            //Usernames are required and have a maximum length of 15
            modelBuilder.Entity<Account>().Property(p => p.Password)
                .IsRequired()
                .HasMaxLength(10);
            //Passwords are required and have a maximum length of 10

            modelBuilder.Entity<Character>().Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(20);

            modelBuilder.Entity<Character>().Property(p => p.ShortName)
                .HasMaxLength(15);

            //Table per hierarchy for Character
            modelBuilder.Entity<Character>()
                .Map<PlayerCharacter>(m => m.Requires("Type").HasValue("PlayerCharacter"))
                .Map<Npc>(m => m.Requires("Type").HasValue("Npc"));

            modelBuilder.Entity<TemplateItem>().Property(p => p.ShortName)
                .HasMaxLength(15);

            modelBuilder.Entity<TemplateItem>().Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(20);
        }
    }
}
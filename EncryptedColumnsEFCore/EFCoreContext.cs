using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EncryptedColumnsEFCore;

public class EfCoreContext : DbContext
{
    public DbSet<Produto> Produtos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(
            @"Server=(local);Database=EFEncryptedColumn;Integrated Security=true;TrustServerCertificate=true");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var converter = new ValueConverter<string, string>(
            encryptedData => EncrypterHelper.EncryptData(encryptedData),
            decryptedData => EncrypterHelper.DecryptData(decryptedData)
        );

        modelBuilder.Entity<Produto>()
            .Property(p => p.Nome)
            .HasConversion(converter);

        base.OnModelCreating(modelBuilder);
    }
}
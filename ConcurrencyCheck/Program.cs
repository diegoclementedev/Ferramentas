using System.ComponentModel.DataAnnotations;
using ConcurrencyCheck;
using Microsoft.EntityFrameworkCore;

/*

Conflitos de Simultaneidade:

- Simultaneidade otimista: pressupõe que os conflitos de simultaneidade são relativamente raros. Não bloqueia os dados antecipadamente.
- Simultaneidade pessimista: bloqueiam os dados antecipadamente e só então prossiga para modificá-los

*/

using (var setupContext = new DemonstracaoContext())
{
    setupContext.Database.EnsureDeleted();
    setupContext.Database.EnsureCreated();

    setupContext.Produtos.Add(new Produto { Nome = "Mouse", Preco = 10 });
    setupContext.Produtos.Add(new Produto { Nome = "Teclado", Preco = 15 });

    setupContext.SaveChanges();
}

#region Cenário Sem Concorrência

try
{
    using var context = new DemonstracaoContext();

    var produto = context.Produtos.Single(b => b.Nome == "Mouse");
    produto.Nome = "Fone";
    context.SaveChanges();

    Console.WriteLine("Alteração completada com sucesso.");
}
catch (Exception exception)
{
    Console.WriteLine(exception);
    throw;
}

#endregion


#region Cenário Com Concorrência

try
{
    using var context = new DemonstracaoContext();

    var produto1 = context.Produtos.Single(b => b.Nome == "Fone");
    produto1.Nome = "Câmera";

    var produto2 = context.Produtos.Single(b => b.Nome == "Teclado");
    produto2.Nome = "Microfone";

    //Simulando a concorrência
    using (var context2 = new DemonstracaoContext())
    {
        var produto3 = context2.Produtos.Single(b => b.Nome == "Fone");
        produto3.Nome = "Monitor";
        context2.SaveChanges();
    }

    Console.WriteLine("Ao tentar salvar será lançada uma exceção do tipo DbUpdateConcurrencyException");
    context.SaveChanges();
}
catch (DbUpdateConcurrencyException bbUpdateConcurrencyException)
{
    //Trate o conflito aqui
    Console.WriteLine(bbUpdateConcurrencyException);
    throw;
}
catch (Exception exception)
{
    Console.WriteLine(exception);
    throw;
}

#endregion


namespace ConcurrencyCheck
{
public class DemonstracaoContext : DbContext
{
    public DbSet<Produto> Produtos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(
            @"Server=(local); User Id=[User]; Password=[Password]; Database=EFSaving.Concurrency; TrustServerCertificate=true");
    }
}

public class Produto
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public decimal Preco { get; set; }
    [ConcurrencyCheck] //Token de simultaneidade (Campo de controle de simultaneidade)
    public byte[] Versao { get; set; }
}
}
using System.ComponentModel.DataAnnotations;
using ConcurrencyCheck;
using Microsoft.EntityFrameworkCore;

/*

Conflitos de Simultaneidade:

- Simultaneidade otimista: pressupõe que os conflitos de simultaneidade são relativamente raros. Não bloqueia os dados antecipadamente.
- Simultaneidade pessimista: bloqueiam os dados antecipadamente e só então prossiga para modificá-los

*/

using (var setupContext = new PersonContext())
{
    setupContext.Database.EnsureDeleted();
    setupContext.Database.EnsureCreated();

    setupContext.People.Add(new Person { FirstName = "John", LastName = "Doe" });
    setupContext.People.Add(new Person { FirstName = "Marie", LastName = "Jane" });

    setupContext.SaveChanges();
}

try
{
    SuccessfulUpdate();
    ConcurrencyFailure();
}
catch (DbUpdateConcurrencyException bbUpdateConcurrencyException)
{
    Console.WriteLine(bbUpdateConcurrencyException);
    throw;
}
catch (Exception exception)
{
    Console.WriteLine(exception);
    throw;
}



// Sem concorrência
static void SuccessfulUpdate()
{
    using var context = new PersonContext();

    var person = context.People.Single(b => b.FirstName == "John");
    person.FirstName = "Paul";
    context.SaveChanges();

    Console.WriteLine("The change completed successfully.");
}

// Com concorrência
static void ConcurrencyFailure()
{
    using var context = new PersonContext();

    var person = context.People.Single(b => b.FirstName == "Marie");
    person.FirstName = "Stephanie";

    var person1 = context.People.Single(b => b.FirstName == "Paul");
    person1.FirstName = "Ronald";

    //Simulando a concorrência
    using (var context2 = new PersonContext())
    {
        var person2 = context2.People.Single(b => b.FirstName == "Marie");
        person2.FirstName = "Rachel";
        context2.SaveChanges();
    }

    Console.WriteLine("SaveChanges should now throw: DbUpdateConcurrencyException");
    context.SaveChanges();
}

namespace ConcurrencyCheck
{
    public class PersonContext : DbContext
    {
        public DbSet<Person> People { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                @"Server=(local); User Id=sa; Password=200521001gtiT; Database=EFSaving.Concurrency; TrustServerCertificate=true");
        }
    }

    public class Person
    {
        public int PersonId { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        [ConcurrencyCheck] //Token de simultaneidade (Solução para Simultaneidade otimista)
        public byte[] Version { get; set; }
    }
}
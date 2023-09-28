using EncryptedColumnsEFCore;

using var context = new EfCoreContext();
context.Database.EnsureDeleted();
context.Database.EnsureCreated();

context.Produtos.Add(new Produto { Nome = "Mouse" });
context.Produtos.Add(new Produto { Nome = "Teclado" });

context.SaveChanges();

var produtos = context.Produtos.ToList();
Console.WriteLine(produtos);
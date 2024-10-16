
namespace BigBang
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthorization();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            // Fun Facts sample data
            var facts = new List<FunFact>
        {
            new FunFact(1, "Physics", "The universe is expanding.", "Hubble's Law", "Sheldon"),
            new FunFact(2, "Comic Books", "Batman has no superpowers.", "DC Comics", "Leonard"),
            new FunFact(3, "Space", "There are more stars in the universe than grains of sand on Earth.", "Astronomy Magazine", "Raj"),
            new FunFact(4, "Pop Culture", "The first comic book superhero was Superman.", "Action Comics", "Howard"),
            new FunFact(5, "Physics", "Light takes approximately 8 minutes and 20 seconds to travel from the Sun to Earth.", "NASA", "Sheldon"),
            new FunFact(6, "Space", "A day on Venus is longer than a year on Venus.", "NASA", "Raj"),
            new FunFact(7, "Comic Books", "Spider-Man first appeared in Amazing Fantasy #15 in 1962.", "Marvel Comics", "Leonard"),
            new FunFact(8, "Pop Culture", "The phrase 'Beam me up, Scotty' was never actually said in Star Trek.", "Star Trek", "Howard"),
            new FunFact(9, "Physics", "The concept of a black hole was first proposed by John Michell in 1783.", "Philosophical Transactions", "Sheldon"),
            new FunFact(10, "Space", "One million Earths could fit inside the Sun.", "NASA", "Raj"),
            new FunFact(11, "Comic Books", "Wonder Woman was created by psychologist William Moulton Marston.", "DC Comics", "Penny"),
            new FunFact(12, "Pop Culture", "The Simpsons hold the record for the longest-running American sitcom.", "FOX", "Leonard")
        };

            // GET endpoints
            app.MapGet("/facts", () =>
            {
                var message = "As Sheldon would say, 'If you're not part of the solution, you're part of the precipitate.'";
                return new { Facts = facts, Message = message };
            }).WithName("GetFacts").WithOpenApi();

            app.MapGet("/facts/{id}", (int id) =>
            {
                var fact = facts.FirstOrDefault(f => f.Id == id);
                return fact is not null ? Results.Ok(fact) : Results.NotFound();
            }).WithName("GetFactById").WithOpenApi();

            app.MapGet("/facts/category/{category}", (string category) =>
            {
                var filteredFacts = facts.Where(f => f.Category.Equals(category, StringComparison.OrdinalIgnoreCase)).ToList();
                return filteredFacts.Any() ? Results.Ok(filteredFacts) : Results.NotFound();
            }).WithName("GetFactsByCategory").WithOpenApi();

            // POST endpoint
            app.MapPost("/facts", (FunFact fact) =>
            {
                fact = fact with { Id = facts.Count + 1 }; // Assign a new ID
                facts.Add(fact);
                return Results.Created($"/facts/{fact.Id}", fact);
            }).WithName("CreateFact").WithOpenApi();

            // PUT endpoint
            app.MapPut("/facts/{id}", (int id, FunFact updatedFact) =>
            {
                var index = facts.FindIndex(f => f.Id == id);
                if (index != -1)
                {
                    facts[index] = updatedFact with { Id = id }; // Keep the same ID
                    return Results.NoContent();
                }
                return Results.NotFound();
            }).WithName("UpdateFact").WithOpenApi();

            // DELETE endpoint
            app.MapDelete("/facts/{id}", (int id) =>
            {
                var fact = facts.FirstOrDefault(f => f.Id == id);
                if (fact != null)
                {
                    facts.Remove(fact);
                    return Results.NoContent();
                }
                return Results.NotFound();
            }).WithName("DeleteFact").WithOpenApi();
            app.Run();
        }
        public record FunFact(int Id, string Category, string Fact, string Source, string Character);
    }
}


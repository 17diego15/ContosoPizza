using ContosoPizza.Data;
using ContosoPizza.Services;
using ContosoPizza.Business;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var isRunningInDocker = Environment.GetEnvironmentVariable("DOCKER_CONTAINER") == "true";
var keyString = isRunningInDocker ? "ServerDB_Docker" : "ServerDB_Local";
var connectionString = builder.Configuration.GetConnectionString(keyString);


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Services.AddSingleton<IPizzaRepository,PizzaRepository>();
//builder.Services.AddSingleton<IPedidoRepository,PedidoRepository>();
//builder.Services.AddSingleton<IUsuarioRepository,UsuarioRepository>();
//builder.Services.AddSingleton<IIngredientesRepository,IngredientesRepository>();
builder.Services.AddDbContext<ContosoPizzaContext>(options =>
    options.UseSqlServer(connectionString));



builder.Services.AddScoped<PedidoService>();
builder.Services.AddScoped<IPedidoRepository, PedidosEFRRepository>();

//Ingredientes
builder.Services.AddScoped<IngredientesService>();
builder.Services.AddScoped<IIngredientesRepository, IngredientesEFRRepository>();
//Pizza
builder.Services.AddScoped<PizzaService>();
builder.Services.AddScoped<IPizzaRepository, PizzaEFRRepository>();

builder.Services.AddScoped<UsuarioService>();
builder.Services.AddScoped<IUsuarioRepository, UsuariosEFRReposirory>();


var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();


//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

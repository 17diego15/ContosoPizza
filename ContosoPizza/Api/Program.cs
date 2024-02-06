using ContosoPizza.Data;
using ContosoPizza.Services;
using ContosoPizza.Business;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Services.AddSingleton<IPizzaRepository,PizzaRepository>();
//builder.Services.AddSingleton<IPedidoRepository,PedidoRepository>();
//builder.Services.AddSingleton<IUsuarioRepository,UsuarioRepository>();
//builder.Services.AddSingleton<IIngredientesRepository,IngredientesRepository>();

var connectionString = builder.Configuration.GetConnectionString("ServerDB");
builder.Services.AddScoped<IPizzaRepository, PizzaSqlRepository>(serviceProvider => 
    new PizzaSqlRepository(connectionString));

builder.Services.AddScoped<IIngredientesRepository, IngredientesSqlRepository>(serviceProvider => 
    new IngredientesSqlRepository(connectionString));

 builder.Services.AddScoped<IUsuarioRepository, UsuarioSqlRepository>(serviceProvider => 
     new UsuarioSqlRepository(connectionString));


builder.Services.AddScoped<PizzaService>();
//builder.Services.AddScoped<PedidoService>();  //añadir despues
builder.Services.AddScoped<UsuarioService>();
builder.Services.AddScoped<IngredientesService>();

var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();


//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

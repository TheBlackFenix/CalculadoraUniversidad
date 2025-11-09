var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
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

// Endpoint principal: recibe JSON con la operación
app.MapPost("/api/Calculadora", (Operation op) =>
{
    double result = op.Operacion.ToLower() switch
    {
        "+" => op.A + op.B,
        "-" => op.A - op.B,
        "*" => op.A * op.B,
        "/" => op.B == 0 ? double.NaN : op.A / op.B,
        _ => double.NaN
    };

    // Validaciones simples
    if (double.IsNaN(result) && op.Operacion.ToLower() != "/")
        return Results.BadRequest(new { error = "Operación no soportada. Usa +|-|*|/" });

    if (op.Operacion.ToLower() == "/" && op.B == 0)
        return Results.BadRequest(new { error = "División entre cero no permitida" });

    return Results.Json(new
    {
        operation = op.Operacion,
        a = op.A,
        b = op.B,
        result
    });
})
.WithName("ObtenerOperacion")
.WithOpenApi();

app.Run();

public record Operation
{
    public string Operacion { get; init; } = ""; 
    public double A { get; init; }
    public double B { get; init; }
}

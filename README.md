# 🧮 Calculadora API – Sistemas Distribuidos (.NET)

## 📘 Descripción general

Este proyecto implementa una **API REST** simple que realiza **operaciones aritméticas básicas** (suma, resta, multiplicación y división).  
La aplicación está desarrollada en **C# con .NET 8 (Minimal API)** y expone un servicio HTTP que recibe datos en formato **JSON** y devuelve el resultado en una respuesta JSON.

El propósito es comprender los fundamentos del manejo de **estructuras**, **serialización JSON** y **comunicación HTTP** dentro de un sistema distribuido.

---

## 👩‍💻 Requisitos previos

- Tener instalado **.NET SDK 8.0 o superior**
- Un editor de código (Visual Studio Code, Visual Studio o Rider)
- Herramienta para probar la API: Postman, curl o navegador.

---

## ⚙️ Estructura de datos (Operation.cs)

```csharp
public record Operation
{
    public string Op { get; init; } = "";  // "add" | "sub" | "mul" | "div"
    public double A { get; init; }
    public double B { get; init; }
}
```

---

## 🚀 API y servidor HTTP (Program.cs)

```csharp
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => Results.Json(new { status = "ok", service = "calc-api" }));

app.MapPost("/api/calc", (Operation op) =>
{
    double result = op.Op.ToLower() switch
    {
        "+" => op.A + op.B,
        "-" => op.A - op.B,
        "*" => op.A * op.B,
        "/" => op.B == 0 ? double.NaN : op.A / op.B,
        _     => double.NaN
    };

    if (double.IsNaN(result) && op.Operacion.ToLower() != "/")
        return Results.BadRequest(new { error = "Operación no soportada. Usa +|-|*|/" });

    if (op.Op.ToLower() == "div" && op.B == 0)
        return Results.BadRequest(new { error = "División entre cero no permitida" });

    return Results.Json(new { op = op.Op, a = op.A, b = op.B, result });
});

app.Run();
```

---

## 🧾 Ejemplos de uso (HTTP JSON)

### ✅ Suma

**POST /api/calc**

```json
{
  "operacion": "+",
  "a": 12,
  "b": 8
}
```

**Respuesta:**

```json
{
  "operacion": "+",
  "a": 12,
  "b": 8,
  "result": 20
}
```

### ⚠️ Error – División entre cero

```json
{
  "operacion": "/",
  "a": 10,
  "b": 0
}
```

**Respuesta:**

```json
{ "error": "División entre cero no permitida" }
```

---

## 🧠 Diagrama de flujo

```mermaid
flowchart TD
    A[Inicio] --> B[Recibir JSON: {op,a,b}]
    B --> C{op válida? add|sub|mul|div}
    C -- No --> E[HTTP 400: Operación no soportada] --> F[Fin]
    C -- Sí --> D{op == div y b == 0?}
    D -- Sí --> G[HTTP 400: División entre cero] --> F
    D -- No --> H[Calcular resultado]
    H --> I[Construir respuesta JSON]
    I --> J[HTTP 200 con resultado]
    J --> F
```

## 🧩 Pruebas sugeridas

| Operación      | Entrada JSON                        | Resultado esperado |
| -------------- | ----------------------------------- | ------------------ |
| Suma           | `{ "op": "+", "a": 5, "b": 3 }`     | 8                  |
| Resta          | `{ "op": "-", "a": 10, "b": 4 }`    | 6                  |
| Multiplicación | `{ "op": "*mul*", "a": 7, "b": 6 }` | 42                 |
| División       | `{ "op": "/", "a": 8, "b": 2 }`     | 4                  |

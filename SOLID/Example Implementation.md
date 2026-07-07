# 🏗️ SOLID en Acción: Aplicando los 5 Principios Juntos en C# y ASP.NET Core

Hasta ahora estudiamos cada principio SOLID por separado. Sin embargo, **en proyectos reales nunca se aplican de forma aislada**.

Todos trabajan juntos para crear software flexible, mantenible y fácil de extender.

En este ejemplo construiremos un pequeño módulo de **registro de miembros** para una aplicación como **MultiplyNet**, aplicando simultáneamente los cinco principios.

---

# 🎯 Escenario

Cuando un nuevo miembro se registra queremos:

1. Guardarlo en la base de datos.
2. Enviarle una notificación de bienvenida.
3. Registrar un log.
4. Poder cambiar fácilmente entre SQL, PostgreSQL o MongoDB.
5. Poder cambiar entre Email, SMS o WhatsApp sin modificar la lógica principal.

---

# ❌ Código mal diseñado

```csharp
public class MemberService
{
    public void Register(Member member)
    {
        // Guardar en SQL
        Console.WriteLine("Guardando en SQL");

        // Enviar Email
        Console.WriteLine("Enviando Email");

        // Registrar Log
        Console.WriteLine("Registrando Log");
    }
}
```

## ❌ Problemas

- Hace demasiadas cosas (**Viola SRP**).
- Depende directamente de SQL (**Viola DIP**).
- Depende directamente del Email (**Viola DIP**).
- Si cambia la base de datos debemos modificar la clase (**Viola OCP**).
- Si queremos enviar SMS debemos modificar la clase (**Viola OCP**).

Prácticamente rompe todos los principios SOLID.

---

# ✅ Aplicando SOLID

## Paso 1. Modelo

```csharp
public class Member
{
    public string Name { get; set; } = string.Empty;
}
```

---

# 🟢 SRP — Cada clase tiene una sola responsabilidad

## Repositorio

```csharp
public interface IMemberRepository
{
    void Save(Member member);
}
```

### SQL

```csharp
public class SqlMemberRepository : IMemberRepository
{
    public void Save(Member member)
    {
        Console.WriteLine($"Guardando {member.Name} en SQL...");
    }
}
```

### MongoDB

```csharp
public class MongoMemberRepository : IMemberRepository
{
    public void Save(Member member)
    {
        Console.WriteLine($"Guardando {member.Name} en MongoDB...");
    }
}
```

Cada repositorio únicamente guarda miembros.

---

## Servicio de notificaciones

```csharp
public interface INotificationService
{
    void SendWelcome(Member member);
}
```

### Email

```csharp
public class EmailNotification : INotificationService
{
    public void SendWelcome(Member member)
    {
        Console.WriteLine($"Enviando Email a {member.Name}");
    }
}
```

### SMS

```csharp
public class SmsNotification : INotificationService
{
    public void SendWelcome(Member member)
    {
        Console.WriteLine($"Enviando SMS a {member.Name}");
    }
}
```

Cada clase tiene únicamente la responsabilidad de enviar una notificación.

---

## Servicio de Logs

```csharp
public interface ILoggerService
{
    void Log(string message);
}
```

```csharp
public class ConsoleLogger : ILoggerService
{
    public void Log(string message)
    {
        Console.WriteLine($"LOG: {message}");
    }
}
```

Su única responsabilidad es registrar información.

---

# Servicio principal

```csharp
public class MemberService
{
    private readonly IMemberRepository _repository;
    private readonly INotificationService _notification;
    private readonly ILoggerService _logger;

    public MemberService(
        IMemberRepository repository,
        INotificationService notification,
        ILoggerService logger)
    {
        _repository = repository;
        _notification = notification;
        _logger = logger;
    }

    public void Register(Member member)
    {
        _repository.Save(member);

        _notification.SendWelcome(member);

        _logger.Log($"Miembro registrado: {member.Name}");
    }
}
```

---

# 🟢 ¿Dónde aplicamos SRP?

Cada clase tiene una única responsabilidad.

| Clase | Responsabilidad |
|--------|-----------------|
| `MemberService` | Coordinar el registro del miembro |
| `SqlMemberRepository` | Guardar miembros en SQL |
| `MongoMemberRepository` | Guardar miembros en MongoDB |
| `EmailNotification` | Enviar correos |
| `SmsNotification` | Enviar SMS |
| `ConsoleLogger` | Registrar logs |

**Una clase = Una responsabilidad.**

---

# 🟢 ¿Dónde aplicamos OCP?

Supongamos que mañana queremos usar PostgreSQL.

Simplemente agregamos otra implementación.

```csharp
public class PostgreSqlRepository : IMemberRepository
{
    public void Save(Member member)
    {
        Console.WriteLine($"Guardando {member.Name} en PostgreSQL...");
    }
}
```

¿Modificamos `MemberService`?

**No.**

---

Ahora queremos usar WhatsApp.

```csharp
public class WhatsAppNotification : INotificationService
{
    public void SendWelcome(Member member)
    {
        Console.WriteLine($"Enviando WhatsApp a {member.Name}");
    }
}
```

¿Modificamos `MemberService`?

**No.**

Simplemente agregamos nuevas implementaciones.

---

# 🟢 ¿Dónde aplicamos LSP?

`MemberService` solamente conoce este contrato.

```csharp
IMemberRepository
```

No sabe si realmente está trabajando con:

- SQL
- MongoDB
- PostgreSQL

Todos implementan el mismo contrato.

```text
                IMemberRepository
                       ▲
      ┌────────────────┼────────────────┐
      │                │                │
 SqlRepository   MongoRepository   PostgreSqlRepository
```

Todos pueden sustituirse entre sí.

Lo mismo ocurre con las notificaciones.

```text
             INotificationService
                      ▲
        ┌─────────────┼──────────────┐
        │             │              │
     Email         SMS         WhatsApp
```

Cualquier implementación puede reemplazar a otra.

---

# 🟢 ¿Dónde aplicamos ISP?

Observa nuestras interfaces.

```csharp
public interface IMemberRepository
{
    void Save(Member member);
}
```

---

```csharp
public interface ILoggerService
{
    void Log(string message);
}
```

---

```csharp
public interface INotificationService
{
    void SendWelcome(Member member);
}
```

Cada interfaz tiene una única responsabilidad.

No existe algo como:

```csharp
public interface IApplicationService
{
    void Save();

    void Export();

    void Print();

    void SendEmail();

    void Login();

    void Backup();
}
```

Las interfaces son pequeñas, específicas y fáciles de implementar.

---

# 🟢 ¿Dónde aplicamos DIP?

Observa el constructor de `MemberService`.

```csharp
public MemberService(
    IMemberRepository repository,
    INotificationService notification,
    ILoggerService logger)
{
    _repository = repository;
    _notification = notification;
    _logger = logger;
}
```

Nunca hace esto.

```csharp
new SqlMemberRepository();
```

Ni esto.

```csharp
new EmailNotification();
```

Ni esto.

```csharp
new ConsoleLogger();
```

Depende únicamente de interfaces.

---

# Integración con ASP.NET Core

Registramos las dependencias.

```csharp
builder.Services.AddScoped<IMemberRepository, SqlMemberRepository>();

builder.Services.AddScoped<INotificationService, EmailNotification>();

builder.Services.AddScoped<ILoggerService, ConsoleLogger>();

builder.Services.AddScoped<MemberService>();
```

Luego simplemente inyectamos `MemberService`.

```csharp
public class MemberController : ControllerBase
{
    private readonly MemberService _service;

    public MemberController(MemberService service)
    {
        _service = service;
    }

    [HttpPost]
    public IActionResult Register(Member member)
    {
        _service.Register(member);

        return Ok();
    }
}
```

---

# ¿Qué pasa si cambia el negocio?

## Cambiar SQL por MongoDB

Solo cambiamos una línea.

```csharp
builder.Services.AddScoped<IMemberRepository, MongoMemberRepository>();
```

---

## Cambiar Email por SMS

```csharp
builder.Services.AddScoped<INotificationService, SmsNotification>();
```

---

## Cambiar el Logger

```csharp
builder.Services.AddScoped<ILoggerService, FileLogger>();
```

No modificamos `MemberService`.

---

# Arquitectura Final

```text
                        MemberController
                               │
                               ▼
                        MemberService
                               │
      ┌────────────────────────┼────────────────────────┐
      ▼                        ▼                        ▼
IMemberRepository     INotificationService       ILoggerService
      ▲                        ▲                        ▲
      │                        │                        │
 SqlRepository         EmailNotification       ConsoleLogger
 MongoRepository       SmsNotification         FileLogger
 PostgreRepository     WhatsAppNotification    ElasticLogger
```

---

# Resumen de SOLID

| Principio | ¿Dónde se aplica? |
|-----------|-------------------|
| **S — Single Responsibility** | Cada clase tiene una sola responsabilidad (`MemberService`, `SqlRepository`, `EmailNotification`, `ConsoleLogger`). |
| **O — Open/Closed** | Agregamos nuevas implementaciones sin modificar `MemberService`. |
| **L — Liskov Substitution** | Cualquier implementación puede sustituir a otra (`SqlRepository`, `MongoRepository`, etc.). |
| **I — Interface Segregation** | Interfaces pequeñas y especializadas (`IMemberRepository`, `ILoggerService`, `INotificationService`). |
| **D — Dependency Inversion** | `MemberService` depende de interfaces, no de clases concretas. |

---

# 🎓 Conclusión

Este ejemplo demuestra cómo los cinco principios SOLID trabajan juntos para construir una aplicación desacoplada, mantenible y escalable.

En proyectos reales con **ASP.NET Core**, esta forma de diseñar el código es muy común y sirve como base para arquitecturas como:

- Clean Architecture
- Onion Architecture
- Hexagonal Architecture
- Domain-Driven Design (DDD)

Dominar este enfoque te permitirá desarrollar software empresarial de alta calidad y comprender con mayor facilidad patrones de diseño como **Repository**, **Strategy**, **Factory**, **Decorator** y **Mediator**, ya que todos se apoyan, en mayor o menor medida, en los principios SOLID.
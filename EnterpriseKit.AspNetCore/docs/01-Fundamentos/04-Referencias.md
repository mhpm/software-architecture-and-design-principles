# Referencias entre proyectos

## Objetivo

Definir una dirección clara de las dependencias para mantener un sistema desacoplado, mantenible y fácil de evolucionar.

## Regla principal

> Las dependencias siempre apuntan hacia el centro de la arquitectura.

```text
API
        │
        ▼
Application
        │
        ▼
Domain
```

## Responsabilidades

| Proyecto | Responsabilidad |
|----------|-----------------|
| EnterpriseKit.Api | Punto de entrada y configuración |
| EnterpriseKit.Application | Casos de uso y orquestación |
| EnterpriseKit.Domain | Reglas de negocio |
| EnterpriseKit.Persistence | Persistencia con EF Core |
| EnterpriseKit.Infrastructure | Servicios externos |
| EnterpriseKit.Identity | Autenticación y autorización |
| EnterpriseKit.SharedKernel | Componentes compartidos |
| EnterpriseKit.Contracts | Contratos públicos |

## Buenas prácticas

- El dominio no depende de infraestructura.
- La API actúa como Composition Root.
- Las interfaces se definen en capas internas y se implementan en capas externas.
- Evitar dependencias circulares.

## Errores comunes

- Inyectar `DbContext` en controladores.
- Colocar lógica de negocio en la API.
- Referenciar Entity Framework desde el dominio.
- Crear referencias bidireccionales entre proyectos.

## Conceptos clave

- Clean Architecture
- Dependency Rule
- Composition Root
- Dependency Inversion (SOLID)
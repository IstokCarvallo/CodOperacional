# CodOperacional

![Plataforma](https://img.shields.io/badge/Plataforma-.NET%208-512BD4)
![Arquitectura](https://img.shields.io/badge/Arquitectura-Clean%20Architecture-1F6FEB)
![Backend](https://img.shields.io/badge/API-REST-5C2D91)
![Frontend](https://img.shields.io/badge/Web-Blazor%20WebAssembly-0078D4)
![Desktop](https://img.shields.io/badge/Desktop-WPF%20MVVM-107C10)
![BaseDatos](https://img.shields.io/badge/Base%20de%20Datos-SQL%20Server-CC2927)
![Seguridad](https://img.shields.io/badge/Seguridad-JWT%20%7C%20Refresh%20Token-success)
![Cloud](https://img.shields.io/badge/Cloud-Azure-0089D6)
![Estado](https://img.shields.io/badge/Estado-En%20Desarrollo-orange)

---

## Resumen Ejecutivo

**CodOperacional** es una plataforma empresarial orientada a la administración y actualización del **Código Operacional de packing y cuarteles productivos**, permitiendo operar desde clientes Web y Desktop sobre una plataforma centralizada y segura.

La solución busca consolidar la lógica de negocio en un único backend, reducir duplicidad funcional y habilitar una evolución tecnológica escalable y mantenible.

---

# Visión General de la Solución

```mermaid
flowchart LR

U[Usuario]

subgraph Clientes

W[FrontCodOperacional<br/>Blazor WebAssembly]

D[DesktopCodOperacional<br/>WPF + MVVM]

end

subgraph Servicios

API[APISegura<br/>ASP.NET Core 8 REST]

AUTH[JWT + Refresh Token]

end

subgraph Persistencia

SQL[(SQL Server)]

end

subgraph Azure

STATIC[Azure Static Web Apps]

APP[Azure App Service]

end

U --> W

U --> D

W --> API

D --> API

API --> AUTH

API --> SQL

STATIC --> W

APP --> API
```

---

# Principios Arquitectónicos

La plataforma fue diseñada siguiendo principios orientados a soluciones empresariales:

* Single Source of Truth
* API First
* Clean Architecture
* Separation of Concerns
* Security by Design
* Stateless Backend
* Escalabilidad Horizontal
* Bajo Acoplamiento
* Alta Cohesión

---

# Componentes de la Solución

---

# 🔐 APISegura

Backend responsable de exponer capacidades del dominio mediante contratos REST seguros.

## Responsabilidades

* Autenticación y autorización
* Gestión centralizada de reglas de negocio
* Exposición de endpoints REST
* Administración de sesiones
* Integración con clientes Web y Desktop
* Acceso a datos SQL Server

---

## Tecnologías

| Área          | Tecnología    |
| ------------- | ------------- |
| Plataforma    | .NET 8        |
| API           | ASP.NET Core  |
| Persistencia  | SQL Server    |
| Seguridad     | JWT           |
| Sesión        | Refresh Token |
| Contratos     | REST          |
| Documentación | Swagger       |

---

## Arquitectura Lógica

```mermaid
flowchart TB

Controllers

Application

Domain

Infrastructure

Controllers --> Application

Application --> Domain

Application --> Infrastructure

Infrastructure --> Domain
```

---

## Estructura

```text
APISegura
│
├── Controllers
├── Application
│   ├── DTOs
│   ├── Services
│   └── UseCases
│
├── Domain
│   ├── Entities
│   ├── Interfaces
│   └── Rules
│
├── Infrastructure
│   ├── Persistence
│   ├── Security
│   └── ExternalServices
│
└── Configuration
```

---

# 🌐 FrontCodOperacional

Cliente Web encargado de la interacción operacional desde navegador.

## Capacidades

* Consumo desacoplado vía REST
* Gestión segura de sesión
* Navegación basada en roles
* Componentización UI
* Operación distribuida

---

## Tecnologías

| Área         | Tecnología         |
| ------------ | ------------------ |
| Interfaz     | Blazor WebAssembly |
| Componentes  | Razor              |
| Seguridad    | JWT                |
| Comunicación | HttpClient         |

---

## Modelo de Interacción

```mermaid
sequenceDiagram

actor Usuario

participant Web

participant API

Usuario->>Web: Solicitud

Web->>API: HTTPS + JWT

API-->>Web: Respuesta

Web-->>Usuario: Renderizado
```

---

# 🖥 DesktopCodOperacional

Aplicación Windows para operación local y productividad.

## Capacidades

* Arquitectura MVVM
* Consumo seguro del backend
* Separación View / ViewModel
* Integración desacoplada

---

## Tecnologías

| Área         | Tecnología |
| ------------ | ---------- |
| Interfaz     | WPF        |
| Arquitectura | MVVM       |
| Plataforma   | .NET 8     |
| Integración  | REST       |

---

## Arquitectura Desktop

```mermaid
flowchart LR

Views

ViewModels

Services

API

Views --> ViewModels

ViewModels --> Services

Services --> API
```

---

# Modelo de Seguridad

La plataforma implementa autenticación basada en tokens con renovación controlada.

```mermaid
sequenceDiagram

participant Cliente

participant API

participant Auth

Cliente->>API: Login

API->>Auth: Validación

Auth-->>API: JWT + Refresh

API-->>Cliente: Acceso

Cliente->>API: Renovación

API-->>Cliente: Nuevo Token
```

---

## Controles de Seguridad

| Control              | Estado       |
| -------------------- | ------------ |
| JWT                  | Implementado |
| Refresh Token        | Implementado |
| HTTPS                | Implementado |
| Roles                | Implementado |
| Renovación de sesión | Implementado |

---

# Arquitectura de Despliegue

```mermaid
flowchart LR

Repo[Repositorio]

Build[Build]

WEB[Azure Static Web Apps]

API[Azure App Service]

SQL[(SQL Server)]

Repo --> Build

Build --> WEB

Build --> API

API --> SQL
```

---

## Infraestructura Azure

| Servicio              | Propósito    |
| --------------------- | ------------ |
| Azure Static Web Apps | Front Web    |
| Azure App Service     | Backend      |
| SQL Server            | Persistencia |

---

# Configuración de Desarrollo

## Restaurar dependencias

```bash
dotnet restore
```

---

## Compilar

```bash
dotnet build
```

---

## Ejecutar Backend

```bash
cd APISegura
dotnet run
```

Swagger:

```text
https://localhost:xxxx/swagger
```

---

## Ejecutar Front Web

```bash
cd FrontCodOperacional
dotnet run
```

---

## Ejecutar Desktop

```text
Abrir solución en Visual Studio
Ejecutar DesktopCodOperacional
```

---

# Hoja de Ruta

## Fundación

* [x] Arquitectura inicial
* [x] Backend centralizado
* [x] Cliente Web
* [x] Cliente Desktop

---

## Seguridad

* [x] JWT
* [x] Refresh Token
* [x] Control por Roles

---

## Plataforma

* [ ] Observabilidad
* [ ] Auditoría
* [ ] Métricas

---

## Escalabilidad

* [ ] CI/CD
* [ ] Automatización de despliegue
* [ ] Alta disponibilidad
* [ ] Escalamiento horizontal

---

# Gobierno Técnico

## Versionado

Semantic Versioning

```text
MAJOR.MINOR.PATCH
```

---

## Estrategia de Ramas

```text
main
develop
feature/*
release/*
hotfix/*
```

---

# Estado del Proyecto

**Estado:** En Desarrollo
**Objetivo actual:** Consolidación funcional, endurecimiento de seguridad y preparación para despliegue productivo.

---

© CodOperacional — Plataforma Operacional Empresarial

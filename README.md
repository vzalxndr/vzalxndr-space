# VzalxndrSpace (Enterprise Architecture Showcase)

> **Status:** 🚧 Work in Progress (Development)

## 📖 Overview
VzalxndrSpace is a decoupled, client-server system designed to demonstrate modern enterprise architectural patterns. The project consists of a secure RESTful API backend and a reactive desktop client, focusing on scalability, clean code principles, and strict separation of concerns.

## 🏗️ Architecture
The solution is divided into two primary layers communicating via stateless HTTP requests:
* **Core API:** A robust backend service built with .NET 8, handling state management, data persistence, and secure access.
* **Desktop Client:** A modern WPF application utilizing the MVVM (Model-View-ViewModel) pattern for a maintainable and reactive user interface.

## 💻 Tech Stack & Tools
* **Backend:** C#, ASP.NET Core 8, Entity Framework Core
* **Frontend:** WPF (Windows Presentation Foundation), CommunityToolkit.Mvvm
* **Database:** PostgreSQL (Containerized)
* **Security:** JWT (JSON Web Tokens) Authentication, User Secrets
* **Infrastructure:** Docker, Dependency Injection (DI)

## 🚀 Key Technical Features
* **API-First Design:** Fully decoupled frontend and backend for future cross-platform scaling.
* **Secure Authentication:** Implementation of JWT-based stateless authorization.
* **Containerized Environment:** The database runs in Docker for consistent development and deployment across all environments.
* **Modern UI Patterns:** Heavy use of Data Binding, IoC containers, and Source Generators to eliminate boilerplate code.

---
*Note: The specific business domain and internal processing logic are currently obfuscated during the active development phase.*
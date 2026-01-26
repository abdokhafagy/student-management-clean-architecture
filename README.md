# Student Management System – Clean Architecture

A production-ready **ASP.NET Core Web API** built using **Clean Architecture** and **SOLID principles**.

---

## 🚀 Tech Stack
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- Clean Architecture
- SOLID Principles
- AutoMapper
- Dependency Injection

---

## 🧱 Architecture Overview
This solution follows **Clean Architecture** with strict dependency rules and clear separation of concerns:

- **Domain** – Core business entities and contracts
- **Application** – Use cases, business logic, DTOs
- **Infrastructure** – EF Core, repositories, database access
- **API (Presentation)** – HTTP endpoints and composition root

📌 Dependencies always point inward.

---

## 📐 Architecture Validation
The architecture has been reviewed and validated against Clean Architecture best practices.

📄 See: **`CleanArchitecture-Review.md`**

---

## 📁 Project Structure
src/
├── StudentManagement.Domain
├── StudentManagement.Application
├── StudentManagement.Infrastructure
└── StudentManagement.Api

yaml
Copy code

---

## ✨ Key Features
- Clean separation of concerns
- Thin, orchestration-only controllers
- Repository pattern with EF Core fully isolated
- DTO-based communication between layers
- Production-ready and scalable architecture

---

## 🧠 Author
**Abdelrahman Khafagy**  
.NET Backend Engineer  
📍 Egypt  
🔗 LinkedIn: https://www.linkedin.com/in/abdelrahman-khafagy-497803262/

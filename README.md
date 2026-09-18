# 🏥 Poliklinika (Clinic Management System)

A desktop application for managing patient records and medical appointments, built with **C#**, **WPF**, and **MS SQL Server**.

## ✨ Features
- **Patient Portal:** Secure login/registration and personal profile management.
- **Appointment Booking:** Select doctors, services, and schedule visits.
- **Medical History:** View past diagnoses and treatment records.
- **Modern UI:** Custom-styled XAML interface with a clean, user-friendly design.
- **Data Security:** Parameterized SQL queries to prevent injections.

## ️ Tech Stack
| Technology | Description |
| :--- | :--- |
| **C# / .NET** | Core logic and backend |
| **WPF (XAML)** | Desktop GUI framework |
| **MS SQL Server** | Relational database management |
| **ADO.NET** | Database connectivity |

## 🚀 Getting Started

### Prerequisites
- Visual Studio 2019/2022
- .NET Framework 4.8 or .NET 6+
- MS SQL Server (LocalDB or Express)

### Installation
1. **Clone the repository:**
   ```bash
   git clone https://github.com/your-username/poliklinika.git
   cd poliklinika
2. Setup Database:
   Run the provided SQL script (Database.sql) in SSMS to create tables and seed data.
   Update the connection string in DatabaseHelper.cs to match your local server.
3. Run the App:
   Open Poliklinika.sln in Visual Studio.
   Press F5 to build and run.
   
📂 Project Structure
Poliklinika/
├── Helpers/       # DatabaseHelper.cs (DB logic)
├── Views/         # XAML Windows (Login, Main, Booking)
├── Assets/        # Images and Icons
└── App.xaml       # Global styles & entry point

👤 Author
Maria Firsova
📧 firsova-maria16@yandex.ru

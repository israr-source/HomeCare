# HomeCare – Home Service Booking System

HomeCare is a modern web-based home service booking platform developed using **ASP.NET Core MVC**. It connects customers with professional service providers through a secure, role-based system, allowing users to browse services, create bookings, manage jobs, and submit reviews.

The project is designed as a final-year university project and demonstrates the implementation of a complete multi-role web application using modern ASP.NET Core technologies.

---

## Features

### Customer
- Register and log in securely
- Browse available services
- View service details and pricing
- Book home services
- Cancel pending bookings
- View booking history
- Submit reviews after completed services
- Manage personal profile

### Service Provider
- Register with a service category
- View and update profile
- Change service category
- View available jobs matching their category
- Accept service requests
- Complete assigned jobs
- View job history
- View dashboard statistics

### Administrator
- Secure admin dashboard
- Manage users
- Manage service categories
- Manage services
- Set service prices
- Edit and delete services
- View and manage bookings
- Monitor overall platform activity

---

## Booking Workflow

1. Customer registers and logs in.
2. Customer browses available services.
3. Customer creates a booking.
4. Booking status becomes **Pending**.
5. Providers belonging to the matching category can view the booking.
6. The first provider who accepts the booking is assigned to it.
7. Booking status changes to **Accepted**.
8. Provider completes the service.
9. Booking status changes to **Completed**.
10. Customer submits a review.

---

## User Roles

The application supports three user roles:

- Customer
- Service Provider
- Administrator

Role-based authorization is implemented using **ASP.NET Core Identity**.

---

## Technologies Used

### Backend
- ASP.NET Core MVC (.NET 8)
- C#
- Entity Framework Core
- ASP.NET Core Identity

### Frontend
- HTML5
- CSS3
- Bootstrap 5
- JavaScript
- Razor Views

### Database
- Microsoft SQL Server

### Development Tools
- Visual Studio
- Git
- GitHub

---

## Architecture

The application follows the **Model-View-Controller (MVC)** architecture.

```
Browser
    │
    ▼
Controllers
    │
    ▼
Business Logic
    │
    ▼
Entity Framework Core
    │
    ▼
SQL Server Database
```

---

## Database Overview

Main database entities include:

- Users (ASP.NET Identity)
- Service Categories
- Services
- Bookings
- Reviews

Entity Framework Core Code-First migrations are used for database management.

---

## Project Structure

```
HomeCare
│
├── Areas/
│   └── Identity/
├── Controllers/
├── Data/
├── Migrations/
├── Models/
├── ViewModels/
├── Views/
├── wwwroot/
│   ├── css/
│   ├── js/
│   └── images/
├── Program.cs
└── appsettings.json
```

---

## User Interface

- Responsive design
- Modern homepage
- Premium authentication pages
- Responsive service catalog
- Booking management
- Role-based dashboards
- Profile management
- Mobile-friendly interface

---

## Security Features

- ASP.NET Core Identity Authentication
- Role-Based Authorization
- Password Hashing
- Anti-Forgery Token Protection
- Server-Side Validation
- Client-Side Validation
- Secure Entity Framework Core Data Access

---

## Booking Status

The system supports the following booking states:

- Pending
- Accepted
- Completed
- Cancelled

---

## Review System

Customers can submit one review per completed booking.

Features include:

- Rating
- Comments
- Duplicate review prevention
- Provider rating calculation

---

## Installation

### Clone the repository

```bash
git clone https://github.com/your-username/HomeCare.git
```

### Navigate to the project

```bash
cd HomeCare
```

### Restore packages

```bash
dotnet restore
```

### Update the database

```bash
dotnet ef database update
```

### Run the project

```bash
dotnet run
```

or launch using Visual Studio.

---

## Configuration

Update the SQL Server connection string inside:

```
appsettings.json
```

Example:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER;Database=HomeCareDB;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

---

## Screenshots

Add screenshots of the following pages:

- Home Page
- Login Page
- Customer Dashboard
- Provider Dashboard
- Admin Dashboard
- Service Catalog
- Booking Page

---

## Future Improvements

Possible enhancements include:

- Online payment integration
- Email notifications
- SMS notifications
- Google Maps integration
- Real-time chat
- Push notifications
- Mobile application
- AI-based provider recommendations
- Service scheduling
- Provider availability calendar

---

## License

This project is provided for educational purposes.

# Fitessa

Fitessa is a fitness-focused web application designed to help users manage their training routines and exercises. This project was developed using ASP.NET Core MVC (.NET 8) as part of the SoftUni project defense.

## Project Description

Fitessa allows users to browse a database of exercises, create personalized workout plans, and track their fitness journey. The application features a robust role-based system, separating standard user functionalities from administrative control. It demonstrates the implementation of CRUD operations, Entity Framework Core relationships, and secure user authentication.

## Screenshots

| Home Page | Pricing Catalog |
|:---:|:---:|
| <<img width="400" alt="home" src="https://github.com/user-attachments/assets/592b7d84-72b8-42c5-a290-5ffb68df8ba6" />| <img width="400" alt="pricing" src="https://github.com/user-attachments/assets/fd72bca7-2dd7-42b7-9002-22d8cb8405b2" />
 |
| *Landing page view* | *List of available plans* |

| Workout Planner | Dashboard |
|:---:|:---:|
|  <img width="400" alt="creat a plan" src="https://github.com/user-attachments/assets/079903a8-0201-4bdb-b85a-ebe233f0a817" /> | <img width="400" alt="dashboard" src="https://github.com/user-attachments/assets/766d52e3-3c8f-452e-9d6c-461557c041ed" /> |
| *User workout creation interface* | *management area* |

## Features

* **User Authentication:** Secure registration and login system using ASP.NET Core Identity.
* **Exercise Management:** Users can browse exercises filtered by muscle group or difficulty.
* **Workout Planning:** Functionality to create, edit, and delete personal workout routines.
* **Administration Area:** Dedicated area for administrators to manage users, add new exercises to the system, and oversee content.
* **Responsive Design:** Built with Bootstrap to ensure the application works on desktop and mobile devices.
* **Data Validation:** Server-side and client-side validation to ensure data integrity.

## Technologies Used

* **Framework:** ASP.NET Core MVC (.NET 8)
* **Database:** Microsoft SQL Server
* **ORM:** Entity Framework Core
* **Frontend:** HTML, CSS, Bootstrap, JavaScript

## Getting Started

Follow these steps to set up the project locally.

### Prerequisites

* .NET 8 SDK
* Microsoft SQL Server
* Visual Studio 2022

### Installation

1.  **Clone the repository**
    ```bash
    git clone [https://github.com/nataliboeva/Fitessa.git](https://github.com/nataliboeva/Fitessa.git)
    ```

2.  **Configure the Database**
    Open the `appsettings.json` file and update the `DefaultConnection` string to match your local SQL Server configuration.

    ```json
    "ConnectionStrings": {
      "DefaultConnection": "Server=.;Database=Fitessa;Trusted_Connection=True;MultipleActiveResultSets=true"
    }
    ```

3.  **Apply Migrations**
    Open the Package Manager Console or a terminal in the project directory and run the following command to create the database and apply the schema:

    ```bash
    dotnet ef database update
    ```

4.  **Run the Application**
    ```bash
    dotnet run
    ```

## User Roles

* **Guest:** Can view the home page and general information.
* **User:** Can browse exercises, create workouts, and manage their profile.
* **Administrator:** Has full access to manage all data entities (exercises, categories, users).

## License

This project is created for educational purposes.

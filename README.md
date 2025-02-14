#### FilmStudioSFF

**FilmStudioSFF** is a web application designed for managing film rentals between film studios. It allows studios to log in, rent available films, and manage their rented films. The application includes features for authentication, film management, and rental tracking.

---

## Table of Contents

1. [Features](#features)
2. [Technologies Used](#technologies-used)
3. [Setup and Installation](#setup-and-installation)
4. [Running the Application](#running-the-application)
5. [API Endpoints](#api-endpoints)
5. [Usage](#usage)

---

## Features

- **User Authentication**: Login and registration for film studios.
- **Film Management**: Add, view, and manage films and their copies.
- **Rental System**: Rent available films and track rented films.
- **Admin Role**: Admins can manage films and studios.
- **In-Memory Database**: Uses an in-memory database for development and testing.

---

## Technologies Used

- **Backend**:
  - ASP.NET Core
  - Entity Framework Core
  - In-Memory Database
- **Frontend**:
  - HTML, CSS, JavaScript
- **Authentication**:
  - JWT (JSON Web Tokens)
- **Tools**:
  - Visual Studio / Visual Studio Code
  - Git

---

## Setup and Installation

### Prerequisites

- [.NET 6 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)
- [Visual Studio](https://visualstudio.microsoft.com/) or [Visual Studio Code](https://code.visualstudio.com/)
- [Git](https://git-scm.com/)

### Steps

1. **Clone the Repository**:
   ```bash
   git clone https://github.com/666belly/FilmStudioSFF.git
   cd FilmStudioSFF

2. **Set Up the Database**:
The project uses an in-memory database, so no additional setup is required for the database.

3. *Run the Application*:
Start the application using the following command:

   ```bash
    dotnet run

4. *Access the Application*:
Open your browser and navigate to:

Copy
http://localhost:5145

### Running the Application
## Backend

1. **Start the backend server**:

bash
Copy
dotnet run
The backend will run on http://localhost:5000.

2. **Use tools like Postman or Swagger UI to test the API endpoints.**

## Frontend
1. **Open the index.html file in the wwwroot folder to access the frontend.**

2. **Use the login and registration pages to interact with the application.**

### API Endpoints
## Authentication
**POST /api/user/login: Login for film studios.**

**POST /api/user/register: Register a new film studio.**

## Films
**GET /api/films: Get a list of all films.**

**POST /api/films: Add a new film (admin only).**

**GET /api/films/{id}: Get details of a specific film.**

**PUT /api/films/{id}: Update a film (admin only).**

**DELETE /api/films/{id}: Delete a film (admin only).**

## Rentals
**POST /api/filmstudio/rent: Rent a film to a studio.**

**POST /api/filmstudio/return: Return a rented film.**

## Studios
**GET /api/filmstudio: Get a list of all film studios.**

**GET /api/filmstudio/{id}: Get details of a specific studio.**


### Usage
## Login
1. **Navigate to the login page.**

2. **Enter your username and password.**

3. **Click Login.**

## Register
1. **Navigate to the registration page.**

2. **Fill in the required details (name, username, email, password, etc.).**

3. **Click Register.**

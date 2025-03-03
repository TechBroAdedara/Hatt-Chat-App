# Hatt Chat

## Description

Hatt Chat is a real-time chat application built using .NET and ASP.NET Core. It allows users to sign in with a username and send messages to other users in a seamless and interactive environment.

## Features

- User authentication and session management.
- Private and group chat functionality.
- RESTful API for managing users and messages.
- Secure data storage with Entity Framework Core.
- MySQL database support.

## Technologies Used

- **.NET 9+** - Backend framework.
- **ASP.NET Core** - Web API development.
- **Entity Framework Core** - ORM for database operations.
- **MySQL** - Database for user and message storage.

## Installation

### Prerequisites

Ensure you have the following installed:

- .NET SDK 9+
- MySQL Server

### Steps

1. Clone the repository:
   ```sh
   git clone https://github.com/yourusername/hatt-chat.git
   cd hatt-chat
   ```
2. Restore dependencies:
   ```sh
   dotnet restore
   ```
3. Configure the database connection in `appsettings.json`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=localhost;Database=HattChatDB;User Id=root;Password=yourpassword;"
     }
   }
   ```
4. Apply database migrations:
   ```sh
   dotnet ef database update
   ```
5. Run the application:
   ```sh
   dotnet run
   ```


## Contributing

Contributions are welcome! Feel free to open an issue or submit a pull request.


## Contact

For any inquiries, reach out to [courageadedara@gmail.com] or visit the repository at [https://github.com/TechBroAdedara/hatt-chat-app].

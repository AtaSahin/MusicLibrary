# MusicLibraryApp
<div style="text-align: center,text-align: center">
<img src="https://raw.githubusercontent.com/devicons/devicon/master/icons/csharp/csharp-original.svg" alt="csharp" width="40" height="40"/>
<img src="https://upload.wikimedia.org/wikipedia/commons/e/ee/.NET_Core_Logo.svg" alt="dotnet" width="40" height="40"/>
  <div/>
    
## ⚡What is Music Library App?

- Music Library App is a .NET Core 7 web application that leverages the Last.fm API to fetch and display music tracks. 
- The application supports user registration, authentication, and authorization, with different roles such as admin, moderator, verified user, and standart user.
- Users can create playlists, add songs, and administrators have additional capabilities like assigning roles, deleting users, and assigning moderators.

## 🔥Features

- User Registration and Authentication
- Role-based Authorization (Admin, Moderator, Verified User, User)
- Last.fm API Integration for Music Track Information
- Playlist Management (Add, Remove Songs)
- Email Service
- RabbitMQ Background email service
- English and Turkish Language options
- Roles (Admin,Moderator,Verified User,User,Rejected User)
- Admin Capabilities:
  - Assigning Users as Verified
  - Assigning Roles (Admin, Moderator)
  - Deleting Users
  - Adding Songs to the Playlist
- Moderator Capabilities:
  - Adding Songs to the Playlist
- User Capabilities:
  - Creating and Managing Playlists
  - Adding and Removing Songs from Playlists
- Verified User Capabilities:
  - Completely same as with "User" but They have "Verified" badge
- Rejected User:
  - Can not view any feature of the application except "Warning Page"
 

## Technologies Used
- .NET Core 7
- C#
- Last.fm API
- HTML/CSS
- Bootstrap
- JS
## Popular Libraries Used
- RateLimit
- AutoMapper
- Localization
- NLog
- RabbitMQ

## ⚠️ Limitations
- You cannot trrigger any button/function more than 20 within 1 min. Rate Limit Library set into project. It will direct you to error page.

## Prerequisites

Before running the application, make sure you have the following installed:

- .NET 7 SDK
- Visual Studio
- MsSQL

## Docker Usage 🧬

To run this project using Docker, follow these steps:

1. Ensure that Docker is installed on your machine.

2. Open a terminal and navigate to the root directory of the project.

3. Build the Docker image:

    ```bash
    docker build -t my-project .
    ```

   Replace `my-project` with a suitable tag for your Docker image.

4. Run the Docker container:

    ```bash
    docker run -p 8080:80 -p 443:443 my-project
    ```

   Adjust the port mappings (`-p`) according to your application's needs.

5. Access the application in your web browser:

    ```
    http://localhost:8080
    ```

   If you specified different ports in the `docker run` command, adjust the URL accordingly.



## 🧬 Running locally for development

1. Clone the repository:

   ```bash
   https://github.com/AtaSahin/MusicLibrary.git
   
cd MusicLibraryApp

Update the appsettings.json file with your database connection string.

Run the database migrations:
database update



Test Cases can be run by:
npm test


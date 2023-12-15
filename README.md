# MusicLibraryApp

## Overview

MusicLibraryApp is a .NET Core 7 web application that leverages the Last.fm API to fetch and display music tracks. The application supports user registration, authentication, and authorization, with different roles such as admin, moderator, verified user, and standart user. Users can create playlists, add songs, and administrators have additional capabilities like assigning roles, deleting users, and assigning moderators.

## Features

- User Registration and Authentication
- Role-based Authorization (Admin, Moderator, Verified User, User)
- Last.fm API Integration for Music Track Information
- Playlist Management (Add, Remove Songs)
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
 

## Technologies Used
- .NET Core 7
- C#
- Last.fm API
- HTML/CSS
- Bootstrap
- JS

## Prerequisites

Before running the application, make sure you have the following installed:

- .NET 7 SDK
- Visual Studio
- MsSQL

## Setup

cd MusicLibraryApp

Update the appsettings.json file with your database connection string.

Run the database migrations:
database update

1. Clone the repository:

   ```bash
   https://github.com/AtaSahin/MusicLibrary.git

OVERVIEW:
=========
The ToDoAPI solution comprises of the code base for building a persistent REST ToDo List API. The ToDo List API supports complete lifecycle management of a ToDo task, including
its creation, retrieval, update and deletion while storing the data in a persistent database. 

TECHNOLOGY STACK:
=================
C#, ASP.NET Core 2.1
Model to database mapping: Microsoft Entity Framework
Integrated development environment (IDE): Microsoft Visual Studio 2017
Unit Testing: XUnit
Persistent database: SQLite

DATABASE:
=========
The database being used in the API is a SQLite database named Todo.db. This database contains two tables, ToDoItems and Users, created using the 
dbo.Table.sql file available under folder DatabaseScript. The ToDoItems table contains 2 dummy Todo items and is the repository for all ToDoItems,
while the Users table is used for user management for performing authentication. It contains 2 dummy username/passwords:
Admin/pass and Dev/devpass

USAGE OF THE API:
=================
Upon deploying and launching the API successfully, which can either be done via the ASP.NET Core app on NGinx and Apache for Linux and IIS for Windows,
the client can perform the following REST operations on the API: GET (All items/by ID), POST, PUT and DELETE.

TOKEN BASED AUTHENTICATION: 
==========================
The API uses a Symmetric Key based Bearer Authentication mechanism using tokens to perform any operation, so the first task that needs to be done before consuming the API is to pass in a valid username and password to the API via a POST command on the following URL: https://localhost:44329/api/auth/token

Token details: Issuer: todoapi.com, Audience: todoapp, EncryptionKey: dummykey:dskskdkndansadnmdad23324^%&%^&%^&%
The above details are stored in file appsettings.json in the solution. 
A dummy username/password that can be used is: Admin/pass

USER INTERFACE:
===============
For the user interface, I have used HTML and Jquery for building the login page as well as the main Todo list management screen, using very basic styling.

CHOICE OF APPROACH:
===================
I used this approach because I had worked on WebAPIs in the past and thus am more comfortable using this ASP.NET project type than other types.
Also, using this project makes it convenient to use the model-view-controller pattern which is already built-in.
For persistent database, I felt SQLite would be the most convenient to use as compared to SQL Server express as it is very lightweight and less complex.
For authentication, I was familiar with the symmetric key based mechanism that generates a token, and hence went ahead with it.

DESIGN PATTERNS:
================
In this project the primary design pattern I have used is the Model-View-Controller (MVC) pattern, in which, the views are defined in the HTML files under webroot,
which are the login and Todo list management screens, the controller handles the REST operations for Todo list as well as Authentication, and the Model contains
entities for Todo items and users that are mapped to the database.

POSSIBLE IMPROVEMENTS IF MORE TIME WAS GIVEN:
=============================================
Loose coupling using interfaces instead of direct references to the model and controller classes.
A lot more features in the Todo list management user interface, like filtering items on different fields, i.e. ID, Due Date, Status, etc.
Implementing pagination in the application for retrieving and displaying a large numnber of Todo items.
A much better and sleek user interface using CSS styling, as well as designing a proper layout for all the controls.
We can add a username/password to the SQLite database to make it secure which is the case in a production environment.
In edit mode, the existing due date for a todo item is not able to bind to the DueDate datepicker for some reason, which needs to be looked into.

OTHER THINGS:
=============
I had implemented GET methods for fetching items by IDs as well as other fields, had also written test cases for the same, but unfortunately
because of a time crunch could not implement it in the application. Also, out of 3 days, I only got roughly one day to work on this task due to 
my college commitments, and hence couldn't implement a lot of other things in the application.



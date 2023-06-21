# ResultTrackerAPI

## About the project
- This is a small scale Personal API project created by me with the aim of being able to adding and asign pupils and their results to a centralised Database. The aim behind this project was for me to practice skills in C# and ASP.Net. This project will begin as an API and over the next few weeks I will gradually build it up and add a foward facing UI utilising bootstrap and ASP.NET web api's. 

## Current functionality: 
- CRUD functionality for subjects (e.g. AQA Maths)
- CRUD functionality for topics (e.g. year 4 Fractions) 
- CRUD functionality for adding Result data. (e.g. grades with attached teacher notes assigned a topic and a subject)
- User roles are added for Teacher,Student and Admin (To be utilised in a later MVC web interface application).
- JWT token functionality for user authorisation, this is implemented in swaggers UI. 

## Features still pending: 
- Unit tests have been added for Topic,subject and TestResults. NUnit tests still required for token authentication repository.
- Unit tests created for Topic controller. Other controllers NUnit tests still required.

## Future additions:
- Additional sorting functionality for pupil test results.
- Possible addition of a TeachingClass class to add pupils and teachers to.

## Running the solution:  
The solution was built in visual studio code and has the neccessary dependencies to run. When the application is run through Https the swagger UI will be displayed. Where in a user can be created and logged in utilsing the auth controller. When logged in a JWT token is assigned to authenticate the user.

-JSB-22

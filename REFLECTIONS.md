## REST

For my version of the project FilmStudioSFF, I have implemented a REST API  to manage the application - with the purpose of managing resources related to a film studio. The project adheres to basic REST principles, in the fact that it is indeed stateless, resource based and that it uses HTTP methods for its operations. Following, I will motivate my decisions regarding implementation of endpoints and also resources - meaning interfaces and classes.

The API is basically centered around a few key resources:
#### Film: Representing movie related operations.
### Studio: Representing studio related operations.
#### User: Representing user related operations, or admin rather. 


The resources are managed using EntityFrameworkCore, with an InMemory-database, FilmStudioDbContext. The database is then seeded with mock data, to be able to test the application, using the DataSeeder.cs class.

The API consists of a few endpoints in order to be able to interact with these resources, and these endpoints are implemented into the respective controllers - FilmController, StudioController and UserController, which then handles the operations logic.

I have also chosen to implement Swagger, which in this case makes for an easy UI way to test the different endpoints. Although I have come to thin Postman is a more efficient and reliable way to test the endpoints.


## Implementation

### Internal Models
The internal models in this project are the classes in the backend, used to represent the different entities, being films, film studios and users. Using EntityFrameworkCore, the models are mapped to the in-memory-database FilmStudioSFF. They also contain all of the properties needed and relationships for the logic. 
Examples of this:

#### Film:
	Properties: FilmId, Title, Director, Description, Genre, Year, IsAvailable, FilmCopies.
    Relationship explanation: A film can have multiple FilmCopies, and each FilmCopy is associated with a FilmStudio.
#### FilmStudio:
    Properties: FilmStudioId, City, RentedFilms, Username, Role, etc.
    Relationships: A film studio can rent multiple films, and each rented film is associated with a FilmCopy.
#### User:
    Properties: UserId, Username, Password, Role.
    Relationships: Different users have a specific access depending on their role (e.g., admin or filmstudio).


The internal models also contain properties and relationships that are not really exposed via the API. An example of this is the Password property, it is used internally for authentication, and is only ever exposed in API responses hashed so that no sensitive data is directly displayed.

### Visible Resources
The API also has exposed resources that are more compiled for the client side's frontend. Technically they represent a subset of the internal models and only provide the information that would be necessary for the user. 

An example of this from the FilmStudio resource:
#### Properties: 
		FilmStudioId, Password, RentedFilms, Username, Role, etc.
#### How it differs from the internal model:
		Password is never exposed in its unhashed form. 
			


### Visible and internal models
One of the main reasons, and arguably also the most important one, would be for security. Sensitive information, again as aforementioned, sensitive information such as a password, internal IDs and similar, are not exposed without being either hashed or simply not exposed at all. Mainly reducing the risk of data leaks, thru only giving the client necessary information. Granted in this there are no major data leaks to occur. 


Another reason for the responses from the API to be simplified, is to make it easier for the clients to consume, there is no reason to overwhelm the client with unnecessary details unrelated to their user experience. 
It is also of interest to limit the amount of data in the API responses in interest of improving the performance of the program. 

Besides this, I have also used Data Transfer Objects, or DTOs, in my project as a simple class to represent the data exposed. 


## Safety

For my interpretation of the project, I have implemented a few security measures to protect data and restrict information access. Following I will go thru how security has been implemented, including how information is limited and controlled in the API, how login and logout function in the interface, and also role based authorization is used via attributes.

Firstly, to make sure only authorized users have access to specific information, a few methods have been used.
#### JWT (JSON Web Tokens) for authentication. 
	When a user is trying to log in and does so successfully, a JWT is generated, that most importantly contains the user's role. This is sent with the API request as an Authorization header. It is then validated to ensure it is both valid and that the user actually has permission for the action. 


I have also used Role based access control as a security measure. Users having either a role as Admin or Filmstudio, where the API endpoints are then protected based on these roles. An example of this is how only Admins can create or delete films, which Filmstudios cannot do. 
This has been implemented using [Authorize(Roles = "admin")] and [Authorize(Roles = "filmstudio")] attributes. 


Another security measure is the limitation of sensitive information being exposed. Passwords as an example is only ever returned hashed. 



 ### Login and Logout in the Client Interface

When a user logs in via the login form, the username and password are simply sent to the API for authentication. If the login is successful, a JWT is generated and sent back to the client, where it is stored in the localStorage. The token is then used to authorize subsequent API requests. 


For further security measures in the Interface, CORS restrictions could be implemented. In this case the API is configured to AllowAllOrigins. But for a more secure CORS implementation, it could be configured to only allow requests from specific domains, and thus prevent suspicious sources to access the API.

As for the returns of passwords, they are always hashed. This is done using BCrypt, and happens before the password is stored in the database. Known for being difficult to decrypt. 


To summarize, I would like to argue that the application has enough security measures in place as of the moment. JWT authentication, role based access control, hashed password function etc. Login and logout are handled with localStorage, and sensitive information is limited to minimize data compromisation. 


## Issues

As I had a lot of difficulties throughout this project I also wanted to include this section to point at them. Particularly my challenge was when trying to integrate the frontend with the backend, meaning accessing the API correctly. 
While most of the API endpoints do work correctly when tested via Postman, I had a significant amount of difficulties when trying to fetch data from the frontend for certain parts of the program. 
One of my biggest and persistent issues has been dealing with undefined values, when making API calls. Examples of ways this manifested was would be in fetching rented films or attempting to rent a film, although it works via Postman, I could not manage to get it to work via the frontend to begin with, where the studioId and filmId kept returning as undefined. This of course results in error messages such as 400 Bad request. 
I do however have an understanding for the fact that studioId and filmId were not being correctly retrieved from the localStorage, or passed incorrectly to the frontend. Even after trying to add additional logging to ensure these values are being correctly passed and set. 
Also debugging these issues came to be a difficult task, as the errors have been quite intertwined. In the end I found out that the issue causing the localStorage problem was due to a property naming issue. The rent option technically does work, and does add the films to the list - but the page does not update automatically, the user has to reload the page for the films to appear, but they do appear after doing so. To finalize my project and add generate a rent button for each film instead of the current option with a form, I would again have needed to give myself more time.


In the beginning I also faced some issues with inconsistent API responses. Mainly regarding the GET api/filmstudio/rented-films endpoint, where it kept returning an empty list instead and or 404 Not found. This however did come to an end when I discovered, via console logging, that the rented films were not being saved correctly and could thus be resolved. 


To summarize, while the backend API functions correctly when tested via Postman, integrating it with the frontend came to be more of a challenge than I expected and would have needed more time than I originally estimated. 
By adding logging to my code, I was able to resolve a great part of the challenges. For the future, I will for sure keep in mind to plan even more time for the frontend part of the project as well so that I can prevent similar problems in other projects. 

## REST

For my version of the project FilmStudioSFF, I have implemented a REST API  to manage the application - with the purpose of managing resources related to a film studio. The project adheres to basic REST principles, in the fact that it is indeed stateless, resource based and that it uses HTTP methods for its operations. Following, I will motivate my decisions regarding implementation of endpoints and also resources - meaning interfaces and classes.

The API is basically centered around a few key resources:
	Film: Representing movie related operations.
	Studio: Representing studio related operations.
	User: Representing user related operations, or admin rather. 


The resources are managed using EntityFrameworkCore, with an InMemory-database, FilmStudioDbContext. The database is then seeded with mock data, to be able to test the application, using the DataSeeder.cs class.


### Implementation

### Safety
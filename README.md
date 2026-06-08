Separation of Layers

Added Repository pattern for data access

Added Service layer for the DI and infra call

Added Endpoints separated endpoints for Update/Insert changed from minimal API

Added Fluent Validators and Global exception and Problem details to keep the same error details for user

Added Jwt auth for the user works with admin and password for the write role and with any data for read roles

Added Test project to test all of them



1.Changes are made inline with clean architecture with unit testability and maintainability of the code

2.Efcore chosen for this built in change tracking easy CRUD operations, integrated with .NET core and supports LINQ clean maintainable queries  also it provides inbuilt Uow and Repository pattern dapper for this project would be an extra and time is not enough this would be used when using large complex queries which was not required now.

3.CQRS is not applied because its small CRUD application and would have applied it was more complex operation with mediator

separating command and query adding the commandhandler and request for create and update seemed like creating too many files and unnecessary complication 

4.I wanted to handle the filters but did not have time add policy authentication for the method add versioning give room to add filters, middleware etc so went for Controller API

5.Added Jwt authentication and authorization token based system giving privilege access to write endpoints

6.Used Repository pattern for testability, DTO pattern preventing domain exposure, Dependency injection loose coupling for mocking tests

7.with more time would add versioning improved auth with filter and policy based authentication refresh token support, i tried global exception handling for first time with problem details , add response caching for the get method, add serilog, health check try polly with circuit breaker for the get calls.


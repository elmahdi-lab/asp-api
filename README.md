# docker-compose up
It would start a webserver that listen on port 8443, swagger is kept even if the project environment is in production.

I cannot take all the time I want for the technical test, that's why I decided to make a list of changes I would have done if it was a real project. I'll go over the multiple enhancements I would have done and then some difficulties I encoutered and how I did solve them.

### Relationships
The first time I looked into the technical test I already had in mind the relations I need to setup for the models, one clinic would have many patients and many providers. Getting them from the database should be very easy. My strength is Symfony so it would have been an easy task to setup the one to many mapping with annotations. Entity Framework and C# in general is something I work with but not in a day to day basis, so I had to research a little bit. I encountered an issue where my provider has many appointments, but each linked appointment references the provider and I get a very big nested object that throws an exception. Sadly I couldn't find a solution quickly and I removed bidirectional relations.

### Dependency Injection
Putting the database context inside a service like AvailabilityService which would have containted all the logic to determine if a provider is available, same would have been done for an AppointmentService.

### Availability
I first thought I would have to generate free appointment slots based on the provider availability, but then if I think large scale, that would overload the database before even needing the data. So the solution was to check if we have already an appointment for the provided timeslot or if there is a conflict, yes it will add a query to the server, but in my opinion it is lighter then prefilling the database.

### Functional and unit testing
As I am more familiar with mocha and phpunit, I will write in plain language the tests I would have done:

- Assert the start and end minutes of the availabilities and appointments are quarters only (15 value will pass, 16 value will fail)
- Assert each model exists after adding them functionnaly.
- Assert a provider with an availability of 8 -> 12 cannot add a second one with these slots (8-12, 6-12, 9-12, 6-10, 9-14)
- Assert an appointment 9-10am in the previous availability cannot add a second one for the same provider with these slots (9-10, 8-10,9-11, 9:15-9:45, 8-12)

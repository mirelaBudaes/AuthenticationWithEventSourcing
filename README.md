Authentication with Event Sourcing

I will explain the code next to the requirements. Ik gebruik Engels omdat het sneller is om te schrijven.

<!-- Opdracht:

 Een gebruiker moet zich kunnen registeren op basis van een e-mailadres, 
 met behulp van dit e-mailadres moet de gebruiker zich in de toekomst 
 kunnen inloggen op het systeem met een wachtwoord. -->
The web application (.net core mvc) Authentication.Web has a Register action (in the Authentication controller). Here can a user be added.
If the email address is already in the DB, we get an event saved and an error message.

<!-- Er is zowel een NoSQL event store (let op dit is géén key-value store) 
aanwezig als een SQL database. Beide zijn via het netwerk te bereiken. -->
In order to test, I added the 2 databases. Used a local sql db and Dapper as an ORM (worked with it a lot in my last job).
As NoSql I used LiteDB as it had a very quick start. Each of them is accessed through a separate project (EventStore and SqlStore).
The NoSql db stores the events. Events can only be added.
The Sql stores the current version of the user, no history included.


<!-- Er moet een dotnet library worden gemaakt welke zowel gebruikt kan 
 worden vanuit een ASP.Net core MVC website als via een .NET framework console applicatie op basis van event sourcing.
Gebruik voor de version control git. En als taal C#. -->
The library is Authentication.Library with a main public class AuthenticationService.
I added a ASP.Net core MVC website already, which serves as the UI from point 3.

When a request comes, the controller actions are calling the methods in AuthenticationService. 
This calls EventSourceManager( from Authentication.Command project), which creates Events, saves them and then replays them to sync with the SQL database.
For displaying the user data (not the history), we call UserRepository class from Authentication.Query project. 
This is how I used the CQRS pattern for Event Sourcing. It is a simpler version then the "Deluxe" one presented in the PluralSight course I followed,
but I wanted to implement most requirements first. 


1)
<!-- Ga er van uit dat er een NoSQL event store aanwezig is via het netwerk bereikbaar waarbij :
 - op basis van een key alle events voor deze key terug krijgt;
 - op basis van een key je één of meerdere events kan toevoegen.
 Definieer voor bovenstaande een interface welke je later kan gebruiken. -->

Dat is IAuthenticationEventRepository and AuthenticationEventRepository implementation.

2)
<!-- Voeg aan de dotnet library de volgende functionaliteit toe waarbij gebruik wordt gemaakt van de eerder gemaakte interface.
 - gebruiker kunnen aanmaken op basis van e-mailadres; -->
AuthenticationService.RegisterUser(string emailAddress). 
Run the website, go to Register link.
<!-- - gebruiker moet zijn e-mailadres kunnen wijzigen, een wijziging van 
 een e-mailadres kan echter niet meteen gebeuren omdat deze eerst geverifieerd moet worden; -->
Run the website, click on a user. You'll be taken to User page.
AuthenticationService.RequestChangeEmail(Guid userId, string newEmailAddress) will create the EmailChangeRequested event. 
The Sql data doesn't change, because the email has to be verified first.
When the email will be verified, we can raise the EmailChanged event (this is not done).

<!-- - gebruiker kunnen verwijderen, eventuele gegevens die we van de gebruiker hebben 
 moeten na 30 dagen binnen het systeem worden verwijderd. -->
Didn't have time to do this one. The plan is:
- create UserUnregistered event. 
- On replay delete the user from the sql db.
- create a task for 30 days later (maybe by using Bus with saga and handlers this would have been easier)

<!--Binnen het systeem moeten alle e-mailadressen uniek zijn. -->
When a user registers with an existing email, an EmailUniqueValidationFailed event is raised and saved in the NoSql db.
The Sql one is not changed.

<!-- Het verifiëren van e-mailadressen hoeft niet gemaakt te worden, maar de library 
 moet wel de mogelijkheid hebben om aan te geven dat een e-mailadres is geverifieerd. -->
This can be done from the User page. An event is raised and on Replay, the Sql is updated..

3)
<!-- Maak een UI (dit mag ook een simpele console app zijn) waarbij we een gebruiker 
 op basis van een e-mailadres of een gedeelte van een e-mailadres kunnen opzoeken. -->
The UI is the Authentication.Web, but because of time I didn't do the search.
You can see some MVC skills in the existing controllers from there and search in the Dapper repos.
The plan would be:
- In the HomeController add a new action. Update the view with a form and a text box. (This you can see done in the other controllers)
- search in the actual Sql db (extend UserRepository class and use Dapper to search)
- show the User endpoint when found. There you can already see the current Sql data and the history of events for that user.


Thinks to considered:
- I didn't have previous working experience with CQRS, Event Sourcing or NoSql.
I chose a simple solution that would implement Event Sourcing with CQRS, but there are many ways to do it.
- Time used: 2 half days for research, 1 and a half day for design and implementation.


If I had time, I would have:
- Used a differnt pattern for event sourcing. I would have used the Bus which has sagas, commands and handlers. 
This way my feeling is the deletion of user data after 30 days would have been easier to add. 
Also, the actual email change would have been done when the new email was verified.
- refactored the AuthenticationService class. Now it's doing too many things and it might be hard to use as a library
- added EmailAddress validation
- added Unit Tests

- done some renaming and cleaning (No need for Authentication controller, Register name was enough) 
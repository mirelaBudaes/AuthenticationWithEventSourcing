Authentication with Event Sourcing


Opdracht:

Een gebruiker moet zich kunnen registeren op basis van een e-mailadres, 
met behulp van dit e-mailadres moet de gebruiker zich in de toekomst 
kunnen inloggen op het systeem met een wachtwoord.
Er is zowel een NoSQL event store (let op dit is géén key-value store) 
aanwezig als een SQL database. Beide zijn via het netwerk te bereiken.
Er moet een dotnet library worden gemaakt welke zowel gebruikt kan 
worden vanuit een ASP.Net core MVC website als via een .NET framework console applicatie op basis van event sourcing.
Gebruik voor de version control git. En als taal C#.

1)
Ga er van uit dat er een NoSQL event store aanwezig is via het netwerk bereikbaar waarbij :
- op basis van een key alle events voor deze key terug krijgt;
- op basis van een key je één of meerdere events kan toevoegen.
Definieer voor bovenstaande een interface welke je later kan gebruiken.

2)
Voeg aan de dotnet library de volgende functionaliteit toe waarbij gebruik wordt gemaakt van de eerder gemaakte interface.
- gebruiker kunnen aanmaken op basis van e-mailadres;
- gebruiker moet zijn e-mailadres kunnen wijzigen, een wijziging van 
een e-mailadres kan echter niet meteen gebeuren omdat deze eerst geverifieerd moet worden;
- gebruiker kunnen verwijderen, eventuele gegevens die we van de gebruiker hebben 
moeten na 30 dagen binnen het systeem worden verwijderd.
Binnen het systeem moeten alle e-mailadressen uniek zijn.
Het verifiëren van e-mailadressen hoeft niet gemaakt te worden, maar de library 
moet wel de mogelijkheid hebben om aan te geven dat een e-mailadres is geverifieerd.

3)
Maak een UI (dit mag ook een simpele console app zijn) waarbij we een gebruiker 
op basis van een e-mailadres of een gedeelte van een e-mailadres kunnen opzoeken.
# Webapp Projekt
*Alexander Doppelbauer*

### App starten:  
cd dieselskandal-portal  
docker compose up --build  
=> localhost auf Port 4200  

### Dummy Account:
Email = jane.doe@test.de  
Passwort = 1234  

Es gibt aktuell kein vollständiges Identity-System  
=> Nur vorhandene Accounts funktionieren  

### Frontend E2E Tests:
#### 2 Login Tests, 1 Home Button Test 
#### 1 Test [ Login -> Auftrag anlegen -> Auftrag in Liste finden und löschen ]
cd frontend  
npm install  
npx cypress open
=> E2E

### Backend Tests:
#### 7 einfache Tests zu den Controllern
cd Webapp_Dieselskandal.Tests  
dotnet test

#### Abhängigkeiten:
- Docker Desktop
- Node.js
- .NET 10 SDK

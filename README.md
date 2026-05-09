# TrialTask:
#### Alexander Doppelbauer

### App starten:  
docker compose up --build

### Dummy Account:
Email = jane.doe@test.de  
Passwort = 1234

### Frontend E2E Tests:
cd frontend  
npm install
npx cypress open

### Backend Tests:
cd Webapp_Dieselskandal.Tests  
dotnet test

#### Abhängigkeiten:
- Docker Desktop
- Node.js
- .NET 10 SDK
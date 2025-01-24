# LibrIO

LibrIO est une application de gestion de bibliothèque développée en .NET 9 avec C# 13.0. Cette application permet de gérer les membres de la bibliothèque, y compris la création, la modification, la suppression et la recherche de membres.



## Prérequis

- .NET 9 SDK
- Visual Studio 2022


## Installation
1. Clonez le dépôt :

`git clone https://github.com/aiden-ouarghi/bibliotheque-simplon.git
cd LibrIO`

2. Ouvrez le projet dans Visual Studio 2022.

Restaurez les packages NuGet :

`dotnet restore`

Mettez à jour la base de données :

`Add-Migration init
Update-Database`

L'API sera accessible à l'adresse https://localhost:7226 en développement.


## API Endpoints

### Membres

- `GET /membres` : Récupère la liste de tous les membres.
- `POST /membres` : Crée un nouveau membre.
- `GET /membres/{id}` : Récupère les informations d'un membre spécifique par son ID.
- `PUT /membres/{id}` : Met à jour les informations d'un membre existant par son ID.
- `DELETE /membres/{id}` : Supprime un membre par son ID.

### Employés

- `GET /employes` : Récupère la liste de tous les employés.
- `POST /employes` : Crée un nouvel employé.
- `GET /employes/{id}` : Récupère les informations d'un employé spécifique par son ID.
- `PUT /employes/{id}` : Met à jour les informations d'un employé existant par son ID.
- `DELETE /employes/{id}` : Supprime un employé par son ID.

### Livres
- `GET /livres` : Récupère la liste de tous les livres.
- `POST /livres` : Crée un nouveau livre.
- `GET /livres/{id}` : Récupère les informations d'un livre spécifique par son ID.
- `PUT /livres/{id}` : Met à jour les informations d'un livre existant par son ID.
- `DELETE /livres/{id}` : Supprime un livre par son ID.

### Auteurs
- `GET /auteurs` : Récupère la liste de tous les auteurs.
- `POST /auteurs` : Crée un nouvel auteur.
- `GET /auteurs/{id}` : Récupère les informations d'un auteur spécifique par son ID.
- `PUT /auteurs/{id}` : Met à jour les informations d'un auteur existant par son ID.
- `DELETE /auteurs/{id}` : Supprime un auteur par son ID.

### Catégories
- `GET /categories` : Récupère la liste de toutes les catégories.
- `POST /categories` : Crée une nouvelle catégorie.
- `GET /categories/{id}` : Récupère les informations d'une catégorie spécifique par son ID.
- `PUT /categories/{id}` : Met à jour les informations d'une catégorie existante par son ID.
- `DELETE /categories/{id}` : Supprime une catégorie par son ID.

### Emprunts
- `GET /emprunts` : Récupère la liste de tous les emprunts.
- `POST /emprunts` : Crée un nouvel emprunt.
- `GET /emprunts/{id}` : Récupère les informations d'un emprunt spécifique par son ID.
- `PUT /emprunts/{id}` : Met à jour les informations d'un emprunt existant par son ID.
- `DELETE /emprunts/{id}` : Supprime un emprunt par son ID.

### Genres
- `GET /genres` : Récupère la liste de tous les genres.
- `POST /genres` : Crée un nouveau genre.
- `GET /genres/{id}` : Récupère les informations d'un genre spécifique par son ID.
- `PUT /genres/{id}` : Met à jour les informations d'un genre existant par son ID.
- `DELETE /genres/{id}` : Supprime un genre par son ID.


## Swagger
L'API utilise Swagger pour la documentation. En mode développement, vous pouvez accéder à la documentation Swagger à l'adresse https://localhost:7226/index.html.

## Contribuer

Les contributions sont les bienvenues ! Veuillez soumettre une pull request ou ouvrir une issue pour discuter des changements que vous souhaitez apporter.


    

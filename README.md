## Back-End Challenge - GIORDANO DE BRITO

This is a challenge project. A REST API made in C# connected to a Postgres database. The objective of this project was to try to build a project with two technologies I am not familiar with, thus learning something which can be applied to other future projects.

### Requirements

- A Linux-running machine (Preferably)
- Docker
- Docker compose plugin

### Tech stack

- ASP.NET 8.0
- PostgresSQL
- Node.js

### Preliminary Configuration

Before running the project it is first necessary to make some adjustments on certain files.
There are a few files to take a look on first, for just running the project as-is will cause all manner of errors to sprout.

#### Docker-compose.yml

One of the very first files to look for into is the _docker-compose.yml_ at the root of the project.

I will just point out here a few lines worth devoting extra attention to.
First one is the default environment variables for the Postgres database.

it is already pre-set by default in the yaml.

```yaml
environment:
    # For testing purposes only. Do NOT use in production.
    POSTGRES_USER: master
    POSTGRES_PASSWORD: 123
    POSTGRES_DB: main
```

As the comment already warns, it is to be used only for testing, ideally, one would set a strong password and a not-so-obvious username.

### How To Run

Run the docker build command below on the root folder, that is, where the three main folders are located. After the project has been built, just run the second command below, to make it go live.

```bash
sudo docker compose build
sudo docker compose up -d
```

The base node frontend application runs on port 3000, now the backend application is set the run on port 5000.
The database is pre-set to run on port 5432, which is the default port for Postgres.
These settings can be changed in the project's main **.env** file.

### Author

Giordano de Brito - 2025
[@GIOdeBrito](https://github.com/GIOdeBrito)

### References

https://github.com/WL-Consultings/challenges/tree/main/backend
https://tembo.io/docs/getting-started/postgres_guides/connecting-to-postgres-with-c-sharp
https://www.npgsql.org/doc/api/Npgsql.NpgsqlConnection.html
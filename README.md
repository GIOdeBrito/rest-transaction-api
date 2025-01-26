## Back-End Challenge - GIORDANO DE BRITO

This is a challenge project. A REST API made in C# connected to a Postgres database. The objective of this project was to test my skills with two technologies I am not familiar with, thus learning something which can be applied to other future projects.

### Requirements
- A Linux-running machine (Preferably)
- Docker, Docker Compose

### Tech stack
- ASP.NET 8.0
- PostgresSQL

### How To Run

To start the project, run the command below on the root folder, where the services subfolders lie.

```bash
sudo docker-compose build
```

The backend application is set the run on port 5000, whilst the database is pre-set to run on port 5432. These settings can be changed in the docker-compose.yml file.

### Author

Giordano de Brito - 2025
[@GIOdeBrito](https://github.com/GIOdeBrito)

### References

https://github.com/WL-Consultings/challenges/tree/main/backend
https://tembo.io/docs/getting-started/postgres_guides/connecting-to-postgres-with-c-sharp
https://www.npgsql.org/doc/api/Npgsql.NpgsqlConnection.html
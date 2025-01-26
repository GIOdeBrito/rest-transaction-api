# Use PostgresSQL 17 image
FROM postgres:17
# Weird default postgre port
EXPOSE 5432

#COPY postgres-db/scripts/initialize.sh /docker-entrypoint-initdb.d/
COPY postgres-db/scripts/init.sql /docker-entrypoint-initdb.d/

CMD ["postgres"]
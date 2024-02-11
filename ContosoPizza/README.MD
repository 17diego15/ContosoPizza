#Descargar la imagen
docker pull diego1708/contosopizza

#Ejecutar
docker run -p 24957:80 diego1708/contosopizza

#Base de datos
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=<YourStrong@Passw0rd>" -p 1433:1433 --name sql1 --hostname sql1 -d mcr.microsoft.com/mssql/server:2019-CU21-ubuntu-20.04



## Setup local environment (API+DB)
`docker-compose up --build --force-recreate -d`


Readme
Setup local environment (API+DB)
docker-compose up --build --force-recreate -d

Setup database
Connect to database and run manually the script of init-db.sql (PRO OPTIONs:

https://stackoverflow.com/questions/69941444/how-to-have-docker-compose-init-a-sql-server-database)
https://www.softwaredeveloper.blog/initialize-mssql-in-docker-container
Save database
docker commit & docker push
dotnet tool install --global dotnet-ef --version 7.
Migraciones: dotnet ef migrations add InitialCreate -p ./Data/ContosoPizza.Data.csproj -s ./Api/ContosoPizza.Api.csproj 

dotnet ef database update -p ./Data/ContosoPizza.Data.csproj -s ./Api/ContosoPizza.Api.csproj 
dotnet ef database drop -p ./Data/ContosoPizza.Data.csproj -s ./Api/ContosoPizza.Api.csproj
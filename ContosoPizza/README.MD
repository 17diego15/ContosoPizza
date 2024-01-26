#Descargar la imagen
docker pull diego1708/contosopizza

#Ejecutar
docker run -p 24957:80 diego1708/contosopizza

#Base de datos
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=<YourStrong@Passw0rd>" -p 1433:1433 --name sql1 --hostname sql1 -d mcr.microsoft.com/mssql/server:2019-CU21-ubuntu-20.04
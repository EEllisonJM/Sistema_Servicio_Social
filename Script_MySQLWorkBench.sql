#Guardar script
#Crear la base de datos servicio_social
CREATE DATABASE IF NOT EXISTS servicio_social;

#Seleccionar la base de datos
USE servicio_social;

#Crear tabla Usuario
CREATE TABLE IF NOT EXISTS Usuario (
            idUsuario int not null AUTO_INCREMENT,
            nombre varchar(60) not null,    
            password varchar (60) not null,
            PRIMARY KEY(idUsuario)    
)ENGINE=INNODB;

#Crear tabla Alumno
CREATE TABLE IF NOT EXISTS Alumno (
            numControl int not null,    
            nombre varchar(60) not null,    
            carrera varchar (60) not null,
            sexo varchar(1) not null,
            e_mail varchar(60) not null,
            porcentajeAvance int not null,
            semestre int not null,
            PRIMARY KEY(numControl)    
)ENGINE=INNODB;

#Crear tabla Carta_Presentacion
CREATE TABLE Carta_Presentacion (
            numExpediente int not null,
            anio int not null,
            numControl int not null references Alumno(numControl),
            nombreDependencia varchar(60) not null,
            direccionDependencia text not null,
            programa text not null,
            jefeDireccion varchar(60) not null,
            puestoJefeDireccion varchar(60) not null,
            leyenda text not null,
            PRIMARY KEY(numExpediente, anio)
)ENGINE=INNODB;
#Insertar usuarios
INSERT INTO usuario (nombre,password) VALUES('juan', '12345');
INSERT INTO usuario (nombre,password) VALUES('admin', '12345&/()=');
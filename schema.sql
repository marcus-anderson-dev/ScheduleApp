-- Creates the client_schedule database and its six tables. Run this first, then setup.sql for sample data.

CREATE DATABASE IF NOT EXISTS client_schedule;
USE client_schedule;

CREATE TABLE IF NOT EXISTS `country` (
  `countryId` int NOT NULL AUTO_INCREMENT,
  `country` varchar(50) NOT NULL,
  `createDate` datetime NOT NULL,
  `createdBy` varchar(40) NOT NULL,
  `lastUpdate` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `lastUpdateBy` varchar(40) NOT NULL,
  PRIMARY KEY (`countryId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE IF NOT EXISTS `city` (
  `cityId` int NOT NULL AUTO_INCREMENT,
  `city` varchar(50) NOT NULL,
  `countryId` int NOT NULL,
  `createDate` datetime NOT NULL,
  `createdBy` varchar(40) NOT NULL,
  `lastUpdate` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `lastUpdateBy` varchar(40) NOT NULL,
  PRIMARY KEY (`cityId`),
  KEY `countryId` (`countryId`),
  CONSTRAINT `city_ibfk_1` FOREIGN KEY (`countryId`) REFERENCES `country` (`countryId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE IF NOT EXISTS `address` (
  `addressId` int NOT NULL AUTO_INCREMENT,
  `address` varchar(50) NOT NULL,
  `address2` varchar(50) NOT NULL,
  `cityId` int NOT NULL,
  `postalCode` varchar(10) NOT NULL,
  `phone` varchar(20) NOT NULL,
  `createDate` datetime NOT NULL,
  `createdBy` varchar(40) NOT NULL,
  `lastUpdate` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `lastUpdateBy` varchar(40) NOT NULL,
  PRIMARY KEY (`addressId`),
  KEY `cityId` (`cityId`),
  CONSTRAINT `address_ibfk_1` FOREIGN KEY (`cityId`) REFERENCES `city` (`cityId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE IF NOT EXISTS `customer` (
  `customerId` int NOT NULL AUTO_INCREMENT,
  `customerName` varchar(45) NOT NULL,
  `addressId` int NOT NULL,
  `active` tinyint(1) NOT NULL,
  `createDate` datetime NOT NULL,
  `createdBy` varchar(40) NOT NULL,
  `lastUpdate` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `lastUpdateBy` varchar(40) NOT NULL,
  PRIMARY KEY (`customerId`),
  KEY `addressId` (`addressId`),
  CONSTRAINT `customer_ibfk_1` FOREIGN KEY (`addressId`) REFERENCES `address` (`addressId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE IF NOT EXISTS `user` (
  `userId` int NOT NULL AUTO_INCREMENT,
  `userName` varchar(50) NOT NULL,
  `password` varchar(50) NOT NULL,
  `active` tinyint NOT NULL,
  `createDate` datetime NOT NULL,
  `createdBy` varchar(40) NOT NULL,
  `lastUpdate` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `lastUpdateBy` varchar(40) NOT NULL,
  PRIMARY KEY (`userId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE IF NOT EXISTS `appointment` (
  `appointmentId` int NOT NULL AUTO_INCREMENT,
  `customerId` int NOT NULL,
  `userId` int NOT NULL,
  `title` varchar(255) NOT NULL,
  `description` text NOT NULL,
  `location` text NOT NULL,
  `contact` text NOT NULL,
  `type` text NOT NULL,
  `url` varchar(255) NOT NULL,
  `start` datetime NOT NULL,
  `end` datetime NOT NULL,
  `createDate` datetime NOT NULL,
  `createdBy` varchar(40) NOT NULL,
  `lastUpdate` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `lastUpdateBy` varchar(40) NOT NULL,
  PRIMARY KEY (`appointmentId`),
  KEY `userId` (`userId`),
  KEY `appointment_ibfk_1` (`customerId`),
  CONSTRAINT `appointment_ibfk_1` FOREIGN KEY (`customerId`) REFERENCES `customer` (`customerId`),
  CONSTRAINT `appointment_ibfk_2` FOREIGN KEY (`userId`) REFERENCES `user` (`userId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

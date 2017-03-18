/*CREATE DATABASE CEAEDB
ON
PRIMARY (NAME = 'CEAEDB',
FILENAME = 'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\CEAEDB.mdf',
SIZE = 10,
MAXSIZE = 500,
FILEGROWTH = 10)
LOG ON
(NAME = 'RATV3Log',
FILENAME = 'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\CEAEDBLog.ldf',
SIZE = 5,
MAXSIZE = 100,
FILEGROWTH = 5);
GO
*/

USE CEAEDB

CREATE TABLE Users
(
UserID int IDENTITY  NOT NULL PRIMARY KEY,
Account varchar(50) NOT NULL UNIQUE,
Password varchar(50) NOT NULL,
FirstName varchar(50) NOT NULL,
LastName varchar(50) NOT NULL,
Title varchar(50) NULL,
ImgPath varchar(250) NULL,
Email varchar(300) NOT NULL,
PhoneNumber varchar(20) NULL,
Administrator varchar(50) NOT NULL

)
CREATE TABLE Contacts
(
ContactID int IDENTITY  NOT NULL PRIMARY KEY,
Email varchar(50) NOT NULL,
SignInDate date NULL

)

CREATE TABLE Questions
(
QuestionID int IDENTITY  NOT NULL PRIMARY KEY,
Title varchar(50) NULL,
Text varchar(max) NOT NULL,
)
CREATE TABLE Answers
(
AnswerID int IDENTITY  NOT NULL PRIMARY KEY,
Title varchar(50) NULL,
Text varchar(max) NOT NULL,
)

CREATE TABLE AnswersQuestions
(
AnswerID int NOT NULL,
QuestionID int  NOT NULL,
Value varchar(50) NULL,
Status varchar(50) NULL
CONSTRAINT PK_TestRequest 
PRIMARY KEY (AnswerID, QuestionID)

CONSTRAINT FK_AnswerID FOREIGN KEY (AnswerID) 
REFERENCES CEAEDB.dbo.Answers (AnswerID),
CONSTRAINT FK_QuestionID FOREIGN KEY (QuestionID) 
REFERENCES CEAEDB.dbo.Questions (QuestionID),
)
CREATE TABLE Causes
(
CauseID int IDENTITY  NOT NULL PRIMARY KEY,
Title varchar(50) NULL,
Text varchar(max) NOT NULL,
StartDate date NULL,
EndDate date NULL,
ImgPath varchar(250) NULL,
)

Create Table TestResults(
TestResultID int IDENTITY NOT NULL PRIMARY KEY,
Date date NOT NULL,
UserID int NULL,
ContactID int NULL,
Status varchar(50)

CONSTRAINT FK_UserID FOREIGN KEY (UserID) 
REFERENCES CEAEDB.dbo.Users (UserID) ON DELETE SET NULL,
CONSTRAINT FK_ContactID FOREIGN KEY (ContactID) 
REFERENCES CEAEDB.dbo.Contacts (ContactID) ON DELETE SET NULL,
)

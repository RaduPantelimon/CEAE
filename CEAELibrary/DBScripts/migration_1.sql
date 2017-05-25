USE CEAEDB;

ALTER TABLE Questions ALTER COLUMN [Text] varchar(max) NULL;
ALTER TABLE Questions ALTER COLUMN [Title] varchar(50) NOT NULL;
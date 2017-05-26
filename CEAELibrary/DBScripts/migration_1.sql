USE CEAEDB;

ALTER TABLE Questions ALTER COLUMN [Text] varchar(max) NULL;
ALTER TABLE Questions ALTER COLUMN [Title] varchar(max) NOT NULL;
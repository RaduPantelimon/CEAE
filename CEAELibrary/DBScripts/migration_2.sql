USE CEAEDB;

ALTER TABLE Questions ADD QuestionOrder int DEFAULT 0;

GO

USE CEAEDB;

UPDATE Questions SET QuestionOrder=QuestionID;

GO
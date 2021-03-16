USE DB;

DROP TABLE IF EXISTS logs;

CREATE TABLE logs(

logID INTEGER NOT NULL IDENTITY(0030000,2), -- PRIMARY KEY,
--creationDate DATETIME,
--event Varchar(20), --example 192/168/1/1
message text, -- 
loglevel Varchar(20),
dateTime Varchar(50),

-- primary key
CONSTRAINT logs_PK PRIMARY KEY(logID,loglevel),

);

INSERT INTO logs(message, loglevel, dateTime) VALUES ('THIS IS A TEST', '0', '2019');

select * from logs;
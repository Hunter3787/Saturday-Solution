USE DB;

DROP TABLE IF EXISTS reviews;

CREATE TABLE reviews(

entityId int NOT NULL IDENTITY(0030000,2), -- PRIMARY KEY,
--creationDate DATETIME,
--event Varchar(20), --example 192/168/1/1
username Varchar(50),
message TEXT, -- 
star int,
imagebuffer VARBINARY(MAX),
filepath VARCHAR(50),
datetime VARCHAR(50),

-- primary key
CONSTRAINT reviews_PK PRIMARY KEY(entityId),

);

INSERT INTO reviews(username, message, star, imagebuffer, filepath, datetime) VALUES ('Serge','THIS IS A TEST', '5',CAST(null AS VARBINARY(MAX)), 'test','2019');

select * from reviews;
USE DB;

DROP TABLE IF EXISTS reviews;

CREATE TABLE reviews(

entityId varchar(50), -- PRIMARY KEY,
--creationDate DATETIME,
--event Varchar(20), --example 192/168/1/1
username Varchar(50),
message TEXT, -- 
star VARCHAR(20),
imagepath VARBINARY(MAX),
datetime Varchar(50),

-- primary key
CONSTRAINT reviews_PK PRIMARY KEY(entityId),

);

INSERT INTO reviews(entityId, username, message, star, imagepath, datetime) VALUES (0,'Serge','THIS IS A TEST', '5', null,'2019');

select * from reviews;
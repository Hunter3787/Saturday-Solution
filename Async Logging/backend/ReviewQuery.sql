USE DB;

DROP TABLE IF EXISTS reviews;

CREATE TABLE reviews(

reviewID INTEGER NOT NULL IDENTITY(0030000,2), -- PRIMARY KEY,
--creationDate DATETIME,
--event Varchar(20), --example 192/168/1/1
message text, -- 
star Varchar(20),

-- primary key
CONSTRAINT reviews_PK PRIMARY KEY(reviewID),

);

INSERT INTO reviews(message, star) VALUES ('THIS IS A TEST', '0');
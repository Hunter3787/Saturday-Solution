
USE AutoBuildDB;

DROP TABLE IF EXISTS dummy;
DROP TABLE IF EXISTS sess;
DROP TABLE IF EXISTS logs;
DROP TABLE IF EXISTS page_sess;
DROP TABLE IF EXISTS userDetails;



create table dummy(

 dummyID INTEGER NOT NULL IDENTITY(100,2), -- PRIMARY KEY,
 fname VARCHAR(20),
 lname VARCHAR(20),
 CONSTRAINT dummy_PK PRIMARY KEY(dummyID)
 
 );



  -- sessions -> logs ( 1..1-> 1..*)
 CREATE TABLE sess(

 -- what im trying to do is have a agent ID that has a 
 -- certain patern: example: 300XXX
 agentID INTEGER NOT NULL IDENTITY(300000,1),
 username VARCHAR(20),
 startSession TIMESTAMP,
 endSession TIMESTAMP,
 ipAdd binary(4),

 CONSTRAINT sess_PK PRIMARY KEY (agentID)
 -- will be defining FK and PK and unique values

 );
 


 -- sessions -> logs ( 1..1-> 1..*)
 CREATE TABLE logs(

logID INTEGER, -- PRIMARY KEY,
event Varchar(20), --example 192/168/1/1
message text, -- 
creationOfLog TIMESTAMP,

CONSTRAINT	logs_sessions FOREIGN KEY
(logID) REFERENCES sess (agentID)

);


 -- sessions -> page_sess ( 1..1-> 1..*)
CREATE TABLE page_sess(

 -- what im trying to do is have a agent ID that has a 
 -- certain patern: example: 300XXX
 sessID INTEGER,
 pageID INTEGER NOT NULL IDENTITY(400000,1),
 startPageSession TIMESTAMP,
 endPageSession TIMESTAMP,

 CONSTRAINT page_sess_PK PRIMARY KEY (sessID, pageID),

 CONSTRAINT page_sessions FOREIGN KEY (sessID) 
 REFERENCES sess (agentID)
 -- will be defining FK and PK and unique values

 );


 CREATE TABLE userDetails(
 username varchar(20),
 email varchar(20),
 passwordHash BINARY(64) NOT NULL,
 fName varchar(20),
 lName varchar(20),
 registrationDate TIMESTAMP,
 -- creating a enum
 roles varchar(15) NOT NULL CHECK (roles IN ('BASIC' , 'ADMIN', 'DEVOLOPER', 'VENDOR'))
 -- pcBuild?
 -- shelfID?
 );

 -- https://www.mssqltips.com/sqlservertip/4037/storing-passwords-in-a-secure-way-in-a-sql-server-database/


 -- https://docs.microsoft.com/en-us/sql/t-sql/data-types/data-types-transact-sql?view=sql-server-ver15

 CREATE TABLE shelves(
 shelfID INTEGER NOT NULL IDENTITY(2300000,1),
 nameOfShelf VARCHAR(20),
 isPCBuild BIT, -- to represent boolean data, bit can be 1,0 or NULL
 CONSTRAINT shelves_PK PRIMARY KEY(shelfID)
 );

 -- this is generic?
 CREATE TABLE products(
 productID INTEGER NOT NULL IDENTITY(900000,2),
 manuName VARCHAR(20),
 model VARcHAR(20),
 typeOfComp VARCHAR(20),
 properties VARCHAR(20), -- how to fix this?
 CONSTRAINT products_PK PRIMARY KEY (productID)
 );

 
 CREATE TABLE prodo_shelf(
 productID INTEGER,
 shelfID INTEGER,
 position INTEGER,

 CONSTRAINT prodo_shelf_FK FOREIGN KEY (productID) 
 REFERENCES products(productID),
 CONSTRAINT prodo_FK FOREIGN KEY (productID) 
 REFERENCES products(productID),
 CONSTRAINT shelf_FK FOREIGN KEY (shelfID) 
 REFERENCES shelves(shelfID)
 );


insert into dummy VALUES ('zein', 'farha');

SELECT * FROM dummy;

SELECT fname FROM dummy;


SELECT count(*) FROM dummy;
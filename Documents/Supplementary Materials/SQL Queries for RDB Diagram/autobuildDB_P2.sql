
USE AutoBuildDB_P2;
DROP TABLE IF EXISTS posts;
DROP TABLE IF EXISTS logs;
DROP TABLE IF EXISTS sess;
DROP TABLE IF EXISTS shelves;
DROP TABLE IF EXISTS uploadedImages;
DROP TABLE IF EXISTS publishedbuilds;

DROP TABLE IF EXISTS pcBuilds;
DROP TABLE IF EXISTS userAccounts;


-- a following schema of Logs and Sessions: "Admin Data" will be created.
  -- sessions -> logs ( 0..1-> 1..*)
 CREATE TABLE sess(

 -- what im trying to do is have a agent ID that has a 
 -- certain patern: example: 300XXX need to read more into it
 visitorID INTEGER NOT NULL IDENTITY,
 pageID INTEGER, -- NOT NULL IDENTITY(500000,1),
 username VARCHAR(20),
 beginAt DATETIME,
 endAt DATETIME,
 ipAddress binary(4),

 CONSTRAINT sess_PK PRIMARY KEY (visitorID)
 -- will be defining FK and PK and unique values

 );

 -- logs --> sessions ( 1..*-> 0..1)
 CREATE TABLE logs(

logID INTEGER NOT NULL IDENTITY(000000,2), -- PRIMARY KEY,
causeID INTEGER, -- foriegn key from sessions visitorID, -- PRIMARY KEY,
creationDate DATETIME,
event Varchar(20), --example 192/168/1/1
message text, -- 
tag Varchar(20),


-- primary key
CONSTRAINT logs_PK PRIMARY KEY(logID, causeID, creationDate),
-- foriegn key
CONSTRAINT	logs_sessions FOREIGN KEY
(causeID) REFERENCES sess (visitorID)

);



 -- sessions --> userAccounts ( 1..*-> 0..1)
 -- shelves  --> userAccounts ( 0..*-> 1..1)

 CREATE TABLE userAccounts(

 userID INTEGER NOT NULL IDENTITY,
 username varchar(20),
 email varchar(20),
 passwordHash BINARY(64) NOT NULL,
 fName varchar(20),
 lName varchar(20),
 createdAt DATETIME,
 createdby Varchar(20),
 modifiedAt DATETIME,
 modifiedBy Varchar(20),
 -- creating a enum
 -- accountTypes represented as an enumeration:
 roley varchar(15) NOT NULL CHECK (roley IN ('BASIC' , 'ADMIN', 'DEVOLOPER', 'VENDOR')),

-- primary key
CONSTRAINT userAccounts_PK PRIMARY KEY(userID)
 );


 -- userAccounts -> shelves (1..1 -> 0..*)
 CREATE TABLE shelves(
 shelfID INTEGER NOT NULL IDENTITY(2300000,1),
 userIDFK INTEGER  NOT NULL,
 nameOfShelf VARCHAR(20),
 createdAt DATETIME,
 createdby Varchar(20),
 modifiedAt DATETIME,
 modifiedBy Varchar(20),

 CONSTRAINT shelves_PK PRIMARY KEY(shelfID),

 CONSTRAINT	userAccounts_shelves_FK FOREIGN KEY
(userIDFK) REFERENCES userAccounts (userID)

 );

 
 -- userAccounts -> PCbuids (1..1 -> 0..50)
 -- PCbuilds -> PublishedBuilds (1..1 -> 0..1)


 CREATE TABLE pcBuilds(
 userIDFK INTEGER NOT NULL, -- foriegn key from userAccount

 pcBuildID INTEGER NOT NULL IDENTITY, --PK
 buildName Varchar(20),
 createdAt DATETIME,
 createdby Varchar(20),
 modifiedAt DATETIME,
 modifiedBy Varchar(20),
 position INTEGER,

 CONSTRAINT pcBuilds_PK PRIMARY KEY(pcBuildID),
 constraint pcBuilds_Unique UNIQUE ( userIDFK),

 CONSTRAINT userAccounts_pcBuilds_FK FOREIGN KEY
 (userIDFK) REFERENCES userAccounts (userID)

 );
 -- userAccounts -> PCbuids (1..1 -> 0..50)
 -- PCbuilds -> publishedBuilds (1..1 -> 0..1)

 CREATE TABLE publishedbuilds(

 userIDpb INTEGER NOT NULL,
 pcBuildIDFK INTEGER, -- FK from the PCbuild
 createdAt DATETIME,
 createdby Varchar(20),
 modifiedAt DATETIME,
 modifiedBy Varchar(20),
 uploadedPicture INTEGER NOT NULL IDENTITY(0900000,1), -- fk from Uploaded images

 CONSTRAINT pb_uploadedPicture_unique UNIQUE (uploadedPicture),

 CONSTRAINT pb_pcBuilds_FK FOREIGN KEY 
 (pcBuildIDFK) REFERENCES pcBuilds (pcBuildID),

 CONSTRAINT pb_pcBuilds_FK_2 FOREIGN KEY 
 (userIDpb) REFERENCES pcBuilds (userIDFK),
 
 CONSTRAINT pb_PK PRIMARY KEY( pcBuildIDFK)

 );
 
 -- uplaodedImages -> publishedBuilds ( 0..* -> 1..1)
 CREATE TABLE uploadedImages(
 imageID INTEGER, --FK
 createdAt DATETIME,
 createdby Varchar(20),
 modifiedAt DATETIME,
 modifiedBy Varchar(20),
 position INTEGER,

 CONSTRAINT uploadedImages_PK PRIMARY KEY(imageID),
 
 CONSTRAINT pb_uploadedImages_FK FOREIGN KEY
 (imageID) REFERENCES publishedbuilds(uploadedPicture),

 );


 -- publishedbuilds -> posts ( 1..1 -> 0..*)
 -- userAccounts    -> posts (1..1 -> 0..*)
 CREATE TABLE posts(

 userIDFK INTEGER, -- FORIEGN KEY
 postTect  text,
 createdAt DATETIME,
 createdby Varchar(20),
 modifiedAt DATETIME,
 modifiedBy Varchar(20),

 -- ratings represented as an enumeration:
 rating varchar(15) NOT NULL CHECK (rating IN ('5 stars' , '4 stars', '3 stars', '2 stars', '1 star', '0 stars')),


 CONSTRAINT posts_userAccount_FK FOREIGN KEY 
 (userIDFK) REFERENCES userAccounts (userID),

 CONSTRAINT posts_PK PRIMARY KEY (userIDFK)

 );

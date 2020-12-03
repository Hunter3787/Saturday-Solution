
USE AutoBuild_V3;

DROP TABLE IF EXISTS posts;
DROP TABLE IF EXISTS logs;
DROP TABLE IF EXISTS sess;
DROP TABLE IF EXISTS shelves;
DROP TABLE IF EXISTS uploadedImages;
DROP TABLE IF EXISTS publishedbuilds;

DROP TABLE IF EXISTS pcBuilds;
DROP TABLE IF EXISTS userAccounts;


 -- sessions --> userAccounts ( 1..*-> 0..1)
 -- shelves  --> userAccounts ( 0..*-> 1..1)

 CREATE TABLE userAccounts(

 userID INTEGER NOT NULL IDENTITY(0010000,2),
 username varchar(30),
 email varchar(30),
 passwordHash varchar,
 firstName varchar(20),
 lastName varchar(20),
 registrationDate Date,

 -- creating a enum
 -- accountTypes represented as an enumeration:
 roley varchar(15) NOT NULL CHECK (roley IN ('BASIC' , 'ADMIN', 'DEVOLOPER', 'VENDOR')),

-- primary key
CONSTRAINT userAccounts_PK PRIMARY KEY(userID)

 );



-- a following schema of Logs and Sessions: "Admin Data" will be created.
  -- sessions -> logs ( 0..1-> 1..*)
 CREATE TABLE sess(

 -- what im trying to do is have a agent ID that has a 
 -- certain patern: example: 300XXX need to read more into it
 visitorID INTEGER NOT NULL IDENTITY(0020000,2),
 pageID INTEGER, -- NOT NULL IDENTITY, need to wait if this is a CK
 userID INTEGER NOT NULL,
 beginAt DATETIME,
 endAt DATETIME,
 ipAddress binary(4),

 CONSTRAINT sess_PK PRIMARY KEY (visitorID),
 -- will be defining FK and PK and unique values
 
CONSTRAINT	sessions_account FOREIGN KEY
(userID) REFERENCES userAccounts (userID)

 );

 -- logs --> sessions ( 1..*-> 0..1)
 CREATE TABLE logs(

logID INTEGER, -- PRIMARY KEY,
causeID INTEGER NOT NULL IDENTITY(0030000,2), -- foriegn key from sessions visitorID, -- PRIMARY KEY,
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


 -- userAccounts -> PCbuids (1..1 -> 0..50)
 -- PCbuilds -> PublishedBuilds (1..1 -> 0..1)


 CREATE TABLE pcBuilds(

 userID INTEGER NOT NULL, -- foriegn key from userAccount -- PK
 pcBuildID INTEGER NOT NULL IDENTITY (0040000,2), --PK
 buildName Varchar(20),
 createdAt DATETIME,
 createdby Varchar(20),
 modifiedAt DATETIME,
 modifiedBy Varchar(20),
 position INTEGER,

 CONSTRAINT pcBuilds_PK PRIMARY KEY(pcBuildID, userID),

 CONSTRAINT pcBuilds_unique UNIQUE (userID, buildName),

 CONSTRAINT userAccounts_pcBuilds_FK FOREIGN KEY
 (userID) REFERENCES userAccounts (userID)

 );


 -- userAccounts -> PCbuids (1..1 -> 0..50)
 -- PCbuilds -> publishedBuilds (1..1 -> 0..1)


 CREATE TABLE publishedbuilds(

 publishID INTEGER NOT NULL IDENTITY (0050000,2),
 userID INTEGER NOT NULL,
 pcBuildID INTEGER NOT NULL, -- FK from the PCbuild
 createdAt DATETIME,
 createdby Varchar(20),
 modifiedAt DATETIME,
 modifiedBy Varchar(20),

 CONSTRAINT pb_PK PRIMARY KEY(publishID),

 CONSTRAINT pb_pcBuilds_FK FOREIGN KEY 
 (pcBuildID, userID) REFERENCES pcBuilds (pcBuildID,userID),
 );
 
 -- uplaodedImages -> publishedBuilds ( 0..* -> 1..1)
 CREATE TABLE uploadedImages(

 imageName Varchar(20), --FK
 publishID INTEGER NOT NULL, -- FK from the PCbuild
 createdAt DATETIME,
 createdby Varchar(20),
 modifiedAt DATETIME,
 modifiedBy Varchar(20),

 CONSTRAINT uploadedImages_PK PRIMARY KEY(imageName, publishID),
 
 CONSTRAINT pb_uploadedImages_FK FOREIGN KEY
 ( publishID) REFERENCES publishedbuilds(publishID),

 );

 
 -- publishedbuilds -> posts ( 1..1 -> 0..*)
 -- userAccounts    -> posts (1..1 -> 0..*)
 CREATE TABLE posts(

 userAccountID INTEGER NOT NULL,
 publishID INTEGER NOT NULL,

 postText  text,
 createdAt DATETIME,
 createdby Varchar(20),
 modifiedAt DATETIME,
 modifiedBy Varchar(20),

 -- ratings represented as an enumeration:
 rating varchar(15) NOT NULL CHECK (rating IN ('5 stars' , '4 stars', '3 stars', '2 stars', '1 star', '0 stars')),

 CONSTRAINT posts_userAccount_FK FOREIGN KEY 
 (userAccountID) REFERENCES userAccounts (userID),

 CONSTRAINT posts_publishedbuilds_FK_1 FOREIGN KEY 
 (publishID) REFERENCES publishedbuilds (publishID),

  CONSTRAINT post_PK PRIMARY KEY(userAccountID, publishID),
 

 );


 -- userAccounts -> shelves (1..1 -> 0..*)
 CREATE TABLE shelves(
 shelfID INTEGER NOT NULL  IDENTITY (0060000,2),
 userID INTEGER  NOT NULL,
 nameOfShelf VARCHAR(20) NOT NULL, --ck 
 createdAt DATETIME,
 createdby Varchar(20),
 modifiedAt DATETIME,
 modifiedBy Varchar(20),

 CONSTRAINT shelves_PK PRIMARY KEY(shelfID, userID),

 
 CONSTRAINT shelves_unique UNIQUE (userID, nameOfShelf),

 CONSTRAINT	userAccounts_shelves_FK FOREIGN KEY
(userID) REFERENCES userAccounts (userID)

 );

 




CREATE DATABASE AutoBuild_V3;


USE AutoBuild_V3;

DROP TABLE IF EXISTS posts;
DROP TABLE IF EXISTS logs;
DROP TABLE IF EXISTS sess;
DROP TABLE IF EXISTS shelves;
DROP TABLE IF EXISTS uploadedImages;

DROP TABLE IF EXISTS userCredentials;
DROP TABLE IF EXISTS buildComments;

DROP TABLE IF EXISTS publishedbuilds;
DROP TABLE IF EXISTS pcBuilds;
DROP TABLE IF EXISTS userAccounts;


 -- sessions --> userAccounts ( 1..*-> 0..1)
 -- shelves  --> userAccounts ( 0..*-> 1..1)

 CREATE TABLE userAccounts(

 userID BIGINT NOT NULL IDENTITY(0010000,2),
 email varchar(30),
 firstName varchar(20),
 lastName varchar(20),
 createdAt DATETIME,
 createdby DATETIME,
 modifiedAt DATETIME,
 modifiedBy DATETIME,
-- primary key
CONSTRAINT userAccounts_PK PRIMARY KEY(userID)

 );

 -- userAccounts (1..!1) -> UserCredentials (1..1)
  CREATE TABLE userCredentials(

 userCredID BIGINT NOT NULL, 
 username varchar(30),
 passwordHash varchar(20),
 modifiedAt DATETIME,
 modifiedBy DATETIME,
 -- creating a enum
 -- accountTypes represented as an enumeration:
 roley varchar(15) NOT NULL CHECK (roley IN ('BASIC' , 'ADMIN', 'DEVOLOPER', 'VENDOR')),

-- primary key
CONSTRAINT userCredentials_PK PRIMARY KEY(userCredID),

--CONSTRAINT userCredentials_CK UNIQUE (userCredID),

CONSTRAINT	userCredentials_UserAccount_FK FOREIGN KEY
(userCredID) REFERENCES userAccounts(userID)

 );



-- a following schema of Logs and Sessions: "Admin Data" will be created.
  -- sessions -> logs ( 0..1-> 1..*)
 CREATE TABLE sess(

 -- what im trying to do is have a agent ID that has a 
 -- certain patern: example: 300XXX need to read more into it
 visitorID BIGINT NOT NULL IDENTITY(0020000,2),
 pageID INTEGER, -- NOT NULL IDENTITY, need to wait if this is a CK
 userID BIGINT NOT NULL,
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

logID BIGINT NOT NULL IDENTITY(0030000,2),-- PRIMARY KEY,
visitorID BIGINT , -- foriegn key from sessions visitorID, -- PRIMARY KEY,
creationDate DATETIME,
event Varchar(20), --example 192/168/1/1
message Varchar(20), -- 
tag Varchar(20),


-- primary key
CONSTRAINT logs_PK PRIMARY KEY(logID),
-- foriegn key
CONSTRAINT	logs_sessions FOREIGN KEY
(visitorID) REFERENCES sess (visitorID)

);


 -- userAccounts -> PCbuids (1..1 -> 0..50)
 -- PCbuilds -> PublishedBuilds (1..1 -> 0..1)


 CREATE TABLE pcBuilds(

 userID BIGINT NOT NULL, -- foriegn key from userAccount -- PK
 pcBuildID  BIGINT NOT NULL IDENTITY (0040000,2), --PK
 buildName Varchar(20),
 createdAt DATETIME,
 createdby DATETIME,
 modifiedAt DATETIME,
 modifiedBy DATETIME,
 position INTEGER,

 CONSTRAINT pcBuilds_PK PRIMARY KEY(pcBuildID),

 CONSTRAINT pcBuilds_unique UNIQUE (userID,pcBuildID),

 CONSTRAINT userAccounts_pcBuilds_FK FOREIGN KEY
 (userID) REFERENCES userAccounts (userID)

 );


 -- userAccounts -> PCbuids (1..1 -> 0..50)
 -- PCbuilds -> publishedBuilds (1..1 -> 0..1)


 CREATE TABLE publishedbuilds(

 publishID  BIGINT NOT NULL IDENTITY (0050000,2),
 pcBuildID  BIGINT NOT NULL, -- FK from the PCbuild
 createdAt DATETIME,
 createdby DATETIME,
 modifiedAt DATETIME,
 modifiedBy DATETIME,

 CONSTRAINT publichedBuilds_PK PRIMARY KEY(publishID),

 CONSTRAINT publichedBuilds_unique UNIQUE (pcBuildID),

 CONSTRAINT pb_pcBuilds_FK FOREIGN KEY 
 (pcBuildID) REFERENCES pcBuilds(pcBuildID)
 
 );
 
 -- uplaodedImages -> publishedBuilds ( 0..* -> 1..1)
 CREATE TABLE uploadedImages(

 imageID BIGINT NOT NULL IDENTITY (0090000,2),
 publishID BIGINT NOT NULL, -- FK from the PCbuild
 createdAt DATETIME,
 createdby DATETIME,
 modifiedAt DATETIME,
 modifiedBy DATETIME,

 CONSTRAINT uploadedImages_PK PRIMARY KEY(imageID),
 
  CONSTRAINT uploadedImages_CK UNIQUE(publishID),
 

 CONSTRAINT pb_uploadedImages_FK FOREIGN KEY
 ( publishID) REFERENCES publishedbuilds(publishID),

 );

 
 -- publishedbuilds -> posts ( 1..1 -> 0..*)
 -- userAccounts    -> posts (1..1 -> 0..*)
 CREATE TABLE posts(

 postID BIGINT NOT NULL IDENTITY (0190000,2),
 userAccountID BIGINT NOT NULL,
 publishID BIGINT NOT NULL,
 createdAt DATETIME,
 createdby DATETIME,
 modifiedAt DATETIME,
 modifiedBy DATETIME,

 CONSTRAINT posts_userAccount_FK FOREIGN KEY 
 (userAccountID) REFERENCES userAccounts (userID),

 CONSTRAINT posts_publishedbuilds_FK_2 FOREIGN KEY 
 (publishID) REFERENCES publishedbuilds (publishID),

  CONSTRAINT post_PK PRIMARY KEY(postID),
 

 );

 CREATE TABLE buildComments(

 postID BIGINT NOT NULL,
 commentText VARCHAR(MAX),

 -- http://www.sql-server-helper.com/tips/tip-of-the-day.aspx?tkey=3C79B0FC-5F4E-4358-9BE4-051397604BC6&tkw=how-to-limit-a-varchar-column-to-10,000-characters

 CONSTRAINT comment_length CHECK( DATALENGTH([commentText]) <= 10000),
  -- ratings represented as an enumeration:
 rating varchar(15) NOT NULL CHECK (rating IN ('5 stars' , '4 stars', '3 stars', '2 stars', '1 star', '0 stars')),
  
  CONSTRAINT buildComments_PK PRIMARY KEY(postID),
 
  CONSTRAINT posts_buildComments_FK FOREIGN KEY 
 (postID) REFERENCES posts (postID)

 );




 -- userAccounts -> shelves (1..1 -> 0..*)
 CREATE TABLE shelves(

 shelfID BIGINT NOT NULL  IDENTITY (0400000,2),
 userID  BIGINT NOT NULL,
 nameOfShelf VARCHAR(20) NOT NULL, --ck 
 createdAt DATETIME,
 createdby DATETIME,
 modifiedAt DATETIME,
 modifiedBy DATETIME,

 CONSTRAINT shelves_PK PRIMARY KEY(shelfID),

 
 CONSTRAINT shelves_CK UNIQUE (nameOfShelf),

 CONSTRAINT	userAccounts_shelves_FK FOREIGN KEY
(userID) REFERENCES userAccounts (userID)

 );

 



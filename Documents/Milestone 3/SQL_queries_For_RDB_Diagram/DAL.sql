
USE AutoBuild_V3

DROP TABLE IF EXISTS userAccounts;
DROP TABLE IF EXISTS userCredentials;


 -- userAccounts (1..1) -> UserCredentials (1..1)

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

 -- userAccounts (1..1) -> UserCredentials (1..1)
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


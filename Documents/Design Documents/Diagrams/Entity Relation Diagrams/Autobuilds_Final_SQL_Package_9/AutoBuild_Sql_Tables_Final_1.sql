
-- THIS FILE CONTAINS THE TABLES TO AUTOBUILD, FOR NOW READONLY UNLESS ISSUES WERE TO HAPPEN (WITH INSERTS)
--
USE DB
-- THE PRODUCTS SIDE:
DROP TABLE IF EXISTS Product_Relations;
DROP TABLE IF EXISTS Products_Specs;
DROP TABLE IF EXISTS Vendor_UserAccount_Junction
DROP TABLE IF EXISTS Vendor_Product_Junction
DROP TABLE IF EXISTS Vendor_Product_Reviews_Junction


DROP TABLE IF EXISTS UserPermissions
DROP TABLE IF EXISTS UserCredentials


DROP TABLE IF EXISTS Logs
--
DROP TABLE IF EXISTS UserSessions


DROP TABLE IF EXISTS Reviews

DROP TABLE IF EXISTS Likes_Association


DROP TABLE IF EXISTS Mostpopularbuilds
DROP TABLE IF EXISTS Save_Product_Shelf
DROP TABLE IF EXISTS Save_Product_Build
DROP TABLE IF EXISTS PcBuilds


DROP TABLE IF EXISTS Shelves
DROP TABLE IF EXISTS PcBuilds


DROP TABLE IF EXISTS MappingHash

DROP TABLE IF EXISTS UserAccounts
DROP TABLE IF EXISTS vendorClub
DROP TABLE IF EXISTS Products





--- THE PARENT OF IT ALL

 -- sessions --> userAccounts ( 1..*-> 0..1)
 -- shelves  --> userAccounts ( 0..*-> 1..1)
 -- Vendor_UserAccount_Junction (0..*)  --> (1..1) UserAccounts
 
 -- UserAccounts (1..1) -> UserCredentials (1..1)
 -- UserAccounts (1..1) -> (1..1)
 -- UserAccounts (1..1) -> (1..1)
 -- UserAccounts (1..1) -> (1..1)
 -- UserAccounts (1..1) -> (1..1)
 -- UserAccounts (1..1) -> (1..1)
 -- UserAccounts (1..1) -> (1..1)

  CREATE TABLE UserAccounts(
 userID BIGINT NOT NULL IDENTITY(0,2),
 email varchar(30),
 firstName varchar(20),
 lastName varchar(20),
 createdAt DATETIME,
 createdBy varchar(20),
 modifiedAt DATETIME,
 modifiedBy varchar(20),
-- primary key
CONSTRAINT UserAccounts_PK PRIMARY KEY(userID),
CONSTRAINT UserAccounts_CK UNIQUE(email)
 );

-- AS OF 04/29 TO GAURD AGAINST DATA BEING DELETED SHOULD
-- USER DELETE THEIR ACCOUNT.

-- MappingHash(1..1) -> 
CREATE TABLE MappingHash(

 userHashID BINARY(64) NOT NULL, 
 -- this will be a generated PK
 -- (the email) + userDateCreated + system internal (userID generated)
 userID BIGINT NOT NULL,

 CONSTRAINT MappingHash_CK UNIQUE (userID),
 CONSTRAINT MappingHash_PK PRIMARY KEY (userHashID),
 CONSTRAINT MappingHash_FK FOREIGN KEY (userID)
REFERENCES UserAccounts(userID) ON DELETE CASCADE);

------------------------ Products Side of things: --------------------------


 -- the parenet of all
 -- Products_Table (0..1) -> (0..*) products relation table (this is not implemented at the mmnt)
 -- Products_Table (1..1) -> (0..*) Products_Specs
 -- Products_Table (0...*)-> (0..1) vendorClub
 CREATE TABLE Products(
 productID BIGINT NOT NULL IDENTITY(0,1),
 productType varchar(50),
 manufacturerName varchar(50),
 modelNumber varchar(50) NOT NULL,
CONSTRAINT Products_CK UNIQUE(modelNumber),
CONSTRAINT Products_PK PRIMARY KEY(productID)
 );

--> Products_Specs (0..*)  -> (1..1) Products_Table 
  CREATE TABLE Products_Specs(
 product_spec_ID BIGINT NOT NULL IDENTITY(0,2),
 productID BIGINT NOT NULL,
 productSpecs varchar(100) NOT NULL,
 productSpecsValue varchar(5000) NOT NULL,
 CONSTRAINT Products_Specs_PK PRIMARY KEY (productID ,productSpecs),
CONSTRAINT	products_specs_FK FOREIGN KEY
(productID) REFERENCES Products(productID)

 );

 --> Product_Relations (0..*)  -> (0..1) Products_Table 
 CREATE TABLE Product_Relations(
 productID BIGINT NOT NULL,
 relatedProductID BIGINT NOT NULL,
 CONSTRAINT	products_FK_1 FOREIGN KEY
(productID) REFERENCES Products(productID),
CONSTRAINT	products_FK_2 FOREIGN KEY
( relatedProductID) REFERENCES Products(productID),
 CONSTRAINT Product_Relations_PK PRIMARY KEY (productID , relatedProductID)
);
 --
-- VendorClub (0..1) -> (0..*) Vendor_Product_Junction
-- VendorClub (0..1) -> (0..*) Vendor_product_reviews
-- VendorClub (0..1) -> (0..*) UserAccount_Vendor_Junction
-- VendorClub (0..1) ->
  CREATE TABLE VendorClub(
  vendorID BIGINT NOT NULL IDENTITY(0,2),
  vendorName varchar(50) NOT NULL,
  createdAt DATETIME,
  CONSTRAINT vendorClub_PK PRIMARY KEY(vendorID),
  CONSTRAINT Product_Relations_CK UNIQUE(vendorName),
 );
 
--  -> Vendor_UserAccount_Junction (0..*)  --> (1..1) UserAccounts
--  -> Vendor_UserAccount_Junction (0..*)  --> (0..1) VendorClub 
 CREATE TABLE Vendor_UserAccount_Junction(
  userID BIGINT NOT NULL,
   vendorID BIGINT NOT NULL,
CONSTRAINT UserAccount_Vendor_PK PRIMARY KEY(userID, vendorID),
CONSTRAINT	UserAccount_Vendor_FK_1 FOREIGN KEY
(vendorID) REFERENCES VendorClub(vendorID),
CONSTRAINT	UserAccount_Vendor_FK_2 FOREIGN KEY
(userID) REFERENCES UserAccounts(userID));

 -- Vendor_Product_Junction( 0..*) -> (0..1) Products
 -- Vendor_Product_Junction( 0..*) -> (0..1) VendorClub
 CREATE TABLE Vendor_Product_Junction(
 
 vendorID BIGINT NOT NULL,
 productID BIGINT NOT NULL,
 productName varchar(500),
 vendorImageUrl varchar(200),
 VendorLinkURL varchar(200),
 productStatus BIT,
-- CHECK (productStatus IN ('AVAILABLE', 'NOT AVAILABLE')),
 productPrice decimal(10,2),
 reviews varchar(20),
 rating varchar(10),

 CONSTRAINT testbool_status_CK CHECK ( productStatus IN (1,0)),

 CONSTRAINT Vendor_Product_PK PRIMARY KEY(vendorID, productID),

 CONSTRAINT	Vendor_Product_Vendor_FK FOREIGN KEY
(vendorID) REFERENCES VendorClub(vendorID),

 CONSTRAINT	Vendor_Product_Product_FK FOREIGN KEY
(productID) REFERENCES Products(productID));


 -- Vendor_Product_Reviews( 0..*) -> (0..1) Products
 -- Vendor_Product_Reviews( 0..*) -> (0..1) VendorClub
 CREATE TABLE Vendor_Product_Reviews_Junction(

 vendorID BIGINT NOT NULL,
 productID BIGINT NOT NULL,

 reviewerName varchar(50) NOT NULL,
 reviewStarRating varchar(10) NOT NULL,
 reviewContent varchar(8000) NOT NULL,
 reviewDate varchar(50) NOT NULL,

CONSTRAINT Vendor_Product_Reviews_PK PRIMARY KEY (vendorID, productID, reviewDate),
CONSTRAINT Vendor_Product_Reviews_Vendor_FK FOREIGN KEY (vendorID)
REFERENCES VendorClub(vendorID),
CONSTRAINT Vendor_Product_Reviews_Product_FK FOREIGN KEY (productID)
REFERENCES Products(productID));


------------------------ End Of Products Side of things: --------------------------


------------------------ Permissions Side of things: --------------------------

 -- UserAccounts (1..1) -> UserCredentials (1..1)
  CREATE TABLE UserCredentials(
  userHashID BINARY(64) NOT NULL, 
 username varchar(50) NOT NULL,
 passwordHash varchar(20) NOT NULL,
 createdAt DATETIME,
 modifiedAt DATETIME,
 modifiedBy DATETIME,
 locked BIT NOT NULL,
 userRole varchar(20) NOT NULL,
CONSTRAINT UserCredentials_PK PRIMARY KEY( userHashID ),
CONSTRAINT	UserCredentials_UserAccount_FK FOREIGN KEY
(userHashID) REFERENCES MappingHash(userHashID)
ON DELETE CASCADE,
CONSTRAINT UserCredentials_CK UNIQUE(username)
);

  -- UserPermissions (0..1) -> UserCredentials (0..*)
 CREATE TABLE UserPermissions(
  userHashID BINARY(64) NOT NULL,
  permission varchar(10) NOT NULL CHECK (permission IN
('CREATE' , 'UPDATE', 'DELETE', 'BLOCK','NONE','READ_ONLY', 'EDIT', 'ALL','WRITE', 'SAVE')),
  
  scopeOfPermission varchar(20),
  
  CONSTRAINT userPermissions_PK PRIMARY KEY(userHashID, permission,scopeOfPermission),
  CONSTRAINT userPermissions_FK FOREIGN KEY(userHashID) REFERENCES userCredentials(userHashID) ON DELETE CASCADE );


------------------------ End of Permissions Side of things: -------------------


------------------------- Logs Side of things: --------------------------------

CREATE TABLE Logs(

userHashID BINARY(64),
logID INTEGER NOT NULL IDENTITY(0030000,2), -- PRIMARY KEY,

creationDate DATETIME, -- need for uad
eventType  Varchar(20), -- this can be like logging out or logging in etc.
eventValue Varchar(20), -- this can be like logging out or logging in etc.
message text, -- 
loglevel Varchar(20),
dateTime Varchar(50),

-- primary key
CONSTRAINT logs_PK PRIMARY KEY(logID,loglevel),

CONSTRAINT	logs_sessions FOREIGN KEY (userHashID) 
REFERENCES MappingHash(userHAshID));

------------------------- end of Logs Side of things: --------------------------------

-------------------------Builds Side of things ---------------------------------

 -- UserAccount (1..1) -> (1..50) PcBuilds
 -- PcBuilds(1..1) -> (0..1) Mostpopularbuilds
 -- PcBuilds(1..1) -> (0..*) Product_Build
  CREATE TABLE PcBuilds(

 userID BIGINT NOT NULL, -- foriegn key from userAccount -- PK
 buildID  BIGINT NOT NULL IDENTITY (0,2), --PK
 buildName Varchar(20),
 -- DATETIME - format: YYYY-MM-DD HH:MI:SS
 createdAt DATETIME,
 createdby DATETIME,
 modifiedAt DATETIME,
 modifiedBy DATETIME,
 position INTEGER,

 
 CONSTRAINT PcBuilds_CK UNIQUE (userID,buildName), -- changed per nick 

 CONSTRAINT pcBuilds_PK PRIMARY KEY(buildID),

 CONSTRAINT UserAccounts_PcBuilds_FK FOREIGN KEY
 (userID) REFERENCES UserAccounts (userID)

 );


 -- userAccounts -> PCbuids (1..1 -> 0..50)
 -- PCbuilds -> publishedBuilds (1..1 -> 0..1)
 CREATE TABLE Mostpopularbuilds(

 postID  BIGINT NOT NULL IDENTITY(0030000,2), -- PRIMARY KEY,
 LikeIncrementor INT,
 
 userHashID BINARY(64), -- NOT NULL, chenge later

 buildID  BIGINT, -- NOT NULL, -- FK from the PCbuild
 BuildTypeValue INT, -- this references nicks enum for builds
 Title VARCHAR(50),
 Description VARCHAR(50), -- change to fit 10,000 chars
 BuildImagePath VARCHAR(MAX),

 datetime VARCHAR(50),

 CONSTRAINT Mostpopularbuilds_PK PRIMARY KEY(postID),
 CONSTRAINT Mostpopularbuilds_Hashing_FK 
 FOREIGN KEY (userHashID)references  MappingHash(userHashID)

-- CONSTRAINT Mostpopularbuilds_PcBuilds_FK FOREIGN KEY (buildID) REFERENCES PcBuilds(buildID) ON DELETE CASCADE
);

 -- the likes tables thing:
 CREATE TABLE Likes_Association(

 postID  BIGINT NOT NULL,
 userHashID BINARY(64) NOT NULL,
 CONSTRAINT Likes_Association_PK PRIMARY KEY(postID, userHashID),


 CONSTRAINT Likes_Mostpopularbuilds_FK FOREIGN KEY
 (postID) REFERENCES Mostpopularbuilds(postID),

  CONSTRAINT Likes_UserAccounts_FK FOREIGN KEY
 (userHashID) REFERENCES MappingHash(userHashID)
 );


 -- userAccounts -> shelves (1..1 -> 0..*)
 CREATE TABLE Shelves(

 shelfID BIGINT NOT NULL  IDENTITY (0400000,2),
 userID  BIGINT NOT NULL,
 nameOfShelf VARCHAR(20) NOT NULL, 
 createdAt DATETIME,
 createdby VARCHAR(20),
 modifiedAt DATETIME,
 modifiedBy VARCHAR(20),
 CONSTRAINT shelves_PK PRIMARY KEY(shelfID),
 
 CONSTRAINT shelves_CK UNIQUE (nameOfShelf,userID), -- changed per nick 

 CONSTRAINT	userAccounts_shelves_FK FOREIGN KEY
(userID) REFERENCES UserAccounts (userID)

 );



-------------------------End of builds side of things ---------------------------
-------------------------Saving to a build             ---------------------------

 -- the likes tables thing:
 CREATE TABLE Save_Product_Build(

 buildID  BIGINT NOT NULL,
 productID BIGINT NOT NULL,
 quantity  INT NOT NULL,

 CONSTRAINT Save_Product_Build_PK PRIMARY KEY(buildID , productID),

 CONSTRAINT Save_Build_FK FOREIGN KEY
 (buildID ) REFERENCES PcBuilds(buildID),
  CONSTRAINT Product_Build_FK FOREIGN KEY
 (productID) REFERENCES Products(productID)
 );

 -- the likes tables thing:
 CREATE TABLE Save_Product_Shelf(

 shelfID  BIGINT NOT NULL,
 productID BIGINT NOT NULL,
 quantity  INT NOT NULL,
 itemIndex INT NOT NULL,
 CONSTRAINT Save_Product_Shelf_PK PRIMARY KEY(shelfID, productID, itemIndex),

 CONSTRAINT Save_Shelf_FK FOREIGN KEY
 (shelfID ) REFERENCES Shelves(shelfID),
  CONSTRAINT Product_Shelf_FK FOREIGN KEY
 (productID) REFERENCES Products(productID)
 );


------------------------- End of Saving to a build      ---------------------------



-------------------------Reviews/ comments Side of things ------------------------


CREATE TABLE Reviews(

entityId BIGINT NOT NULL IDENTITY(0030000,2), -- PRIMARY KEY,
postID  BIGINT , -- can be null
 
userHashID BINARY(64),

username Varchar(50),
message TEXT, -- 
star int,
filepath VARCHAR(MAX),
datetime Datetime,

-- primary key
CONSTRAINT reviews_PK PRIMARY KEY(entityId),

 CONSTRAINT reviews_Mostpopularbuilds_FK 
 FOREIGN KEY (postID) REFERENCES MostpopularBuilds(postID),

 CONSTRAINT reviews_mappingHash_FK FOREiGN KEY
 (userHashID) REFERENCES MappingHash(userHashID));



 ---------------------------------- End of Reviews/ comments Side of things --------------------


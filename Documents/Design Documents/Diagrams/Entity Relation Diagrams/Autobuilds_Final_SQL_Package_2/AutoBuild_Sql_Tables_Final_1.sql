
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



DROP TABLE IF EXISTS UploadedImages 
DROP TABLE IF EXISTS BuildComments
DROP TABLE IF EXISTS Comments


DROP TABLE IF EXISTS Likes_Association



DROP TABLE IF EXISTS Mostpopularbuilds
DROP TABLE IF EXISTS Save_Product_Shelf
DROP TABLE IF EXISTS Save_Product_Build
DROP TABLE IF EXISTS PcBuilds


DROP TABLE IF EXISTS Shelves
DROP TABLE IF EXISTS PcBuilds
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
 userCredID BIGINT NOT NULL,
 username varchar(30) NOT NULL,
 passwordHash varchar(20) NOT NULL,
 modifiedAt DATETIME,
 modifiedBy DATETIME,
CONSTRAINT UserCredentials_PK PRIMARY KEY(userCredID),
CONSTRAINT	UserCredentials_UserAccount_FK FOREIGN KEY
(userCredID) REFERENCES UserAccounts(userID),
CONSTRAINT UserCredentials_CK UNIQUE(username));

 -- UserPermissions (0..1) -> UserCredentials (0..*)

------ CREATE TABLE UserPermissions(
------  userID BIGINT,
------  permission varchar(10) NOT NULL CHECK (permission IN
------('CREATE' , 'UPDATE', 'DELETE', 'BLOCK','NONE','READ_ONLY', 'EDIT', 'ALL','WRITE', 'SAVE')),
------  scopeOfPermission varchar(20),
------  CONSTRAINT userPermissions_PK PRIMARY KEY(userID, permission,scopeOfPermission),
------  CONSTRAINT userPermissions_FK FOREIGN KEY(userID) REFERENCES userCredentials(userCredID)
------ );

  -- UserPermissions (0..1) -> UserCredentials (0..*)
 CREATE TABLE UserPermissions(
  userID BIGINT,
  nameOfpermission varchar(20) NOT NULL,
  permission varchar(10) NOT NULL CHECK (permission IN
('CREATE' , 'UPDATE', 'DELETE', 'BLOCK','NONE','READ_ONLY', 'EDIT', 'ALL','WRITE', 'SAVE')),
  scopeOfPermission varchar(20),
  CONSTRAINT userPermissions_PK PRIMARY KEY(userID, permission,scopeOfPermission,nameOfpermission),
  CONSTRAINT userPermissions_FK FOREIGN KEY(userID) REFERENCES userCredentials(userCredID)
 );


------------------------ End of Permissions Side of things: -------------------


------------------------- Logs Side of things: --------------------------------


-- a following schema of Logs and Sessions: "Admin Data" will be created.
  -- sessions -> logs ( 0..1-> 1..*)
 CREATE TABLE UserSessions(

 -- what im trying to do is have a agent ID that has a
 -- certain patern: example: 300XXX need to read more into it
 visitorID BIGINT NOT NULL IDENTITY(0020000,2),
 pageID INTEGER, -- NOT NULL IDENTITY, need to wait if this is a CK
 userID BIGINT NOT NULL,
 beginAt DATETIME,
 endAt DATETIME,
 ipAddress binary(4),

 CONSTRAINT UserSessions_PK PRIMARY KEY (visitorID),
 -- will be defining FK and PK and unique values

CONSTRAINT	sessions_userAccount FOREIGN KEY
(userID) REFERENCES userAccounts (userID)

 ); 

CREATE TABLE Logs(
visitorID BIGINT NOT NULL,
logID INTEGER NOT NULL IDENTITY(0030000,2), -- PRIMARY KEY,
--creationDate DATETIME,
--event Varchar(20), --example 192/168/1/1
message text, -- 
loglevel Varchar(20),
dateTime Varchar(50),

-- primary key
CONSTRAINT logs_PK PRIMARY KEY(logID,loglevel),

CONSTRAINT	logs_sessions FOREIGN KEY (visitorID) REFERENCES UserSessions(visitorID)

);




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
 CONSTRAINT pcBuilds_PK PRIMARY KEY(buildID),
 CONSTRAINT UserAccounts_PcBuilds_FK FOREIGN KEY
 (userID) REFERENCES UserAccounts (userID)

 );


 -- userAccounts -> PCbuids (1..1 -> 0..50)
 -- PCbuilds -> publishedBuilds (1..1 -> 0..1)
 CREATE TABLE Mostpopularbuilds(

 publishID  BIGINT NOT NULL IDENTITY(0030000,2), -- PRIMARY KEY,
 buildID  BIGINT NOT NULL, -- FK from the PCbuild
 Title VARCHAR(50),
 Description VARCHAR(50), -- change to fit 10,000 chars
 BuildImagePath VARCHAR(50),
 BuildTypeValue INT, -- this references nicks enum for builds
 createdAt DATETIME,

 CONSTRAINT Mostpopularbuilds_PK PRIMARY KEY(publishID),

 CONSTRAINT Mostpopularbuilds_PcBuilds_FK FOREIGN KEY
 (buildID) REFERENCES PcBuilds(buildID));


 -- the likes tables thing:
 CREATE TABLE Likes_Association(

 publishID  BIGINT NOT NULL,
 userID BIGINT NOT NULL,
  CONSTRAINT Likes_Association_PK PRIMARY KEY(publishID, userID),

 CONSTRAINT Likes_Mostpopularbuilds_FK FOREIGN KEY
 (publishID ) REFERENCES Mostpopularbuilds(publishID),
  CONSTRAINT Likes_UserAccounts_FK FOREIGN KEY
 (userID ) REFERENCES UserAccounts(userID)
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

 CONSTRAINT shelves_CK UNIQUE (nameOfShelf),

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
 CONSTRAINT Save_Product_Build_PK PRIMARY KEY(buildID , productID, quantity),

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
 CONSTRAINT Save_Product_Shelf_PK PRIMARY KEY(shelfID, productID, quantity),

 CONSTRAINT Save_Shelf_FK FOREIGN KEY
 (shelfID ) REFERENCES Shelves(shelfID),
  CONSTRAINT Product_Shelf_FK FOREIGN KEY
 (productID) REFERENCES Products(productID)
 );


------------------------- End of Saving to a build      ---------------------------



-------------------------Reviews/ comments Side of things ------------------------



 -- publishedbuilds -> Comments(1..1 -> 0..*)
 -- userAccounts    -> Comments(1..1 -> 0..*)
 -- Comments        -> BuildComments (1..1 -> 0..1)
 
 CREATE TABLE Comments(

 commentID BIGINT NOT NULL IDENTITY (0,1),
 userID BIGINT NOT NULL,
 publishID BIGINT NOT NULL,
 createdAt DATETIME,
 CONSTRAINT posts_UserAccount_FK FOREIGN KEY
 (userID) REFERENCES UserAccounts (userID),

 CONSTRAINT posts_Mostpopularbuilds_FK_2 FOREIGN KEY
 (publishID) REFERENCES Mostpopularbuilds (publishID),

  CONSTRAINT post_PK PRIMARY KEY(commentID)
  );

  
 -- Comments  -> BuildComments (1..1 -> 0..1)
 CREATE TABLE BuildComments(

 commentID BIGINT NOT NULL,
 commentText VARCHAR(MAX),
 -- http://www.sql-server-helper.com/tips/tip-of-the-day.aspx?tkey=3C79B0FC-5F4E-4358-9BE4-051397604BC6&tkw=how-to-limit-a-varchar-column-to-10,000-characters
 
 CONSTRAINT comment_length CHECK( DATALENGTH([commentText]) <= 10000),
  -- ratings represented as an enumeration:
  rating  BIT,

  CONSTRAINT testbool_rating_CK CHECK ( rating IN (1,0)),
  CONSTRAINT BuildComments_PK PRIMARY KEY(commentID),
  CONSTRAINT comments_buildComments_FK FOREIGN KEY
  (commentID) REFERENCES Comments(commentID));


   -- uplaodedImages -> build comments ( 0..5 -> 0..1)
 CREATE TABLE UploadedImages(

 imageID BIGINT NOT NULL IDENTITY (0000000,2),
 commentID BIGINT NOT NULL,
 createdAt DATETIME,
 createdby DATETIME,
 modifiedAt DATETIME,
 modifiedBy DATETIME,

 CONSTRAINT uploadedImages_PK PRIMARY KEY(imageID),
 CONSTRAINT pb_uploadedImages_FK FOREIGN KEY
 (commentID) REFERENCES BuildComments(commentID),

 );

 --

 ---------------------------------- End of Reviews/ comments Side of things --------------------


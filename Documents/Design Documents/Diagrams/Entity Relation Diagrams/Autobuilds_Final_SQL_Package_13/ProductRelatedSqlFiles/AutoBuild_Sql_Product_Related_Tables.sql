
 -------------------------------DO NOT EXECUTE THIS IS NOT FOR YOU GO BACK-----------------------------------------

USE DB;

DROP TABLE IF EXISTS EmailList 
DROP TABLE IF EXISTS Save_Product_Shelf
DROP TABLE IF EXISTS Save_Product_Build
DROP TABLE IF EXISTS Save_Product_MPB
 
DROP TABLE IF EXISTS Product_Relations;
DROP TABLE IF EXISTS Products_Specs;

DROP TABLE IF EXISTS Vendor_Product_Junction;
DROP TABLE IF EXISTS Vendor_UserAccount_Junction;
DROP TABLE IF EXISTS Vendor_Product_Reviews_Junction;

DROP TABLE IF EXISTS vendorClub;

DROP TABLE IF EXISTS Products;

------------------------ Products Side of things: --------------------------


 -- the parenet of all
 -- Products_Table (0..1) -> (0..*) products relation table (this is not implemented at the mmnt)
 -- Products_Table (1..1) -> (0..*) Products_Specs
 -- Products_Table (0...*)-> (0..1) vendorClub
 CREATE TABLE Products(
 productID BIGINT NOT NULL IDENTITY(0,1),
 productType varchar(50),
 manufacturerName varchar(50),
 productName varchar(500),
 imageUrl varchar(200),
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
(productID) REFERENCES Products(productID) ON DELETE CASCADE

 );

 --> Product_Relations (0..*)  -> (0..1) Products_Table 
 CREATE TABLE Product_Relations(
 productID BIGINT NOT NULL,
 relatedProductID BIGINT NOT NULL,
 CONSTRAINT	products_FK_1 FOREIGN KEY
(productID) REFERENCES Products(productID),
CONSTRAINT	products_FK_2 FOREIGN KEY
( relatedProductID) REFERENCES Products(productID) ON DELETE CASCADE,
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
 
 -- Vendor_Product_Junction( 0..*) -> (0..1) Products
 -- Vendor_Product_Junction( 0..*) -> (0..1) VendorClub
 CREATE TABLE Vendor_Product_Junction(
 
 vendorID BIGINT NOT NULL,
 productID BIGINT NOT NULL,
 listingName varchar(500),
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
(vendorID) REFERENCES VendorClub(vendorID) ON DELETE CASCADE,

 CONSTRAINT	Vendor_Product_Product_FK FOREIGN KEY
(productID) REFERENCES Products(productID) ON DELETE CASCADE );


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
REFERENCES VendorClub(vendorID) ON DELETE CASCADE,
CONSTRAINT Vendor_Product_Reviews_Product_FK FOREIGN KEY (productID)
REFERENCES Products(productID)  ON DELETE CASCADE);


------------------------ End Of Products Side of things: --------------------------

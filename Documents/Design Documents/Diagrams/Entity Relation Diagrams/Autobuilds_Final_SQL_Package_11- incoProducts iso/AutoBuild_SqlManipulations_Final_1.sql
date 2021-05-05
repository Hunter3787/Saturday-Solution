
-- THE _SQLMANIPULATIONS YOU CAN THINK OF AS A PLAYGORUD TO TEST THE SQL BEFORE IT BECOMES CONCRETE.


use productDB

-- Nicks Portion:

 
 INSERT INTO Products (productType, productName,manufacturerName) VALUES 
 (  'TYPE 1', 'PRODUCT NAME 1 ', 'MANU#1'),
 (  'TYPE 2', 'PRODUCT NAME 2 ', 'MANU#2'), 
 (  'TYPE 3', 'PRODUCT NAME 3 ', 'MANU#3'),
 (  'TYPE 4', 'PRODUCT NAME 4 ', 'MANU#4');


 INSERT INTO VendorClub(vendorName) VALUES 
 ( 'vend 1'), 
 ( 'vend 2'), 
 ( 'vend 3'), 
 ( 'vend 4'), 
 ( 'vend 5');
  INSERT INTO Vendor_Product_Junction (vendorID,productID,productPrice) VALUES 
 ( 0, 0, 399.00),
 ( 2, 1,   3.00),
 ( 4, 2,   9.00),
 ( 6, 3,  99.00),
 ( 8, 1, 100.00),
 ( 0, 2, 200.00);

CREATE VIEW ProductCompatibility AS
SELECT 
Products.productName AS 'Product Name',
RelatedProductInfo.productName AS 'Compatible Product Name'
		FROM Products AS Products 
			INNER JOIN Product_relations_Table AS RelatedProduct
			ON Products.productID = RelatedProduct.productID
			INNER JOIN Products AS RelatedProductInfo ON
			RelatedProduct.productIDCompatible = RelatedProductInfo.productID;


DROP TABLE IF EXISTS PBUDGET
			-- START  OF POPULATION OF DATA
CREATE TABLE PBUDGET(
 productType varchar(20),
 productPrice float
);

CREATE TYPE PBtYPE AS TABLE
(
 productType varchar(20),
 productPrice float
);

DECLARE @TYPEBUDGET AS PBtYPE

DROP PROCEDURE IF EXISTS INSERT_PBtYPE;
-- going to create  PROCEDURE TO STORE DATA INTO THIS TABLE:
	CREATE PROCEDURE INSERT_PBtYPE
		(@TYPEBUDGET PBtYPE  READONLY)
		AS  
		INSERT INTO PBUDGET
		SELECT * FROM @TYPEBUDGET 



		
 SELECT * FROM VendorClub;
 SELECT * FROM Products;
 SELECT * FROM Vendor_Product_Junction;
 SELECT * FROM PBUDGET;

 SELECT P.productType, VP.productPrice
	FROM Products p INNER JOIN Vendor_Product_Junction VP
	on p.productID = VP.productID

DROP PROCEDURE IF EXISTS Search_ProductBudget 

DECLARE @TYPEBUDGET AS PBtYPE

CREATE PROCEDURE Search_ProductBudget
(@TYPEBUDGET PBtYPE READONLY)
AS 
SELECT P.productType, VP.productPrice
	FROM Products P INNER JOIN Vendor_Product_Junction VP 
	on P.productID = VP.productID
	WHERE P.productType IN (SELECT TY.productType FROM @TYPEBUDGET TY)
		AND VP.productPrice IN (SELECT TY.productPrice FROM @TYPEBUDGET TY)
	
			DECLARE @DEMO AS PBtYPE
			INSERT INTO @DEMO VALUES ('TYPE 1', 399)
			INSERT INTO @DEMO VALUES ('TYPE 4', 100)
			INSERT INTO @DEMO VALUES ('TYPE 2', 3)
			INSERT INTO @DEMO VALUES ('TYPE 3', 10)
		EXECUTE dbo.INSERT_PBtYPE @DEMO
		
		EXECUTE dbo.Search_ProductBudget @DEMO
	
	
 SELECT P.productType, VP.productPrice
	FROM Products p INNER JOIN Vendor_Product_Junction VP
	on p.productID = VP.productID
	
 SELECT * FROM PBUDGET;


 DECLARE @TYPEBUDGET AS PBtYPE

 SELECT P.productType, VP.productPrice
	FROM Products P INNER JOIN Vendor_Product_Junction VP 
	on P.productID = VP.productID
	WHERE P.productType IN (SELECT TY.productType FROM @TYPEBUDGET TY)
		AND VP.productPrice IN (SELECT TY.productPrice FROM @TYPEBUDGET TY)



 SELECT * FROM VendorClub;
 SELECT * FROM Products;
 SELECT * FROM Vendor_Product_Junction;



 --end of nicks portion 



 ---- Authentication procedures. 
 USE productDB;



  Create FUNCTION getUserID
 (
	@EMAIL VARCHAR(30)
	)
 RETURNS BIGINT AS
 BEGIN
 RETURN(
 SELECT ua.userID FROM UserAccounts ua 
 INNER JOIN UserCredentials uc  
 on ua.userID = uc.userCredID
 where ua.email= @EMAIL)
 END

  
 CREATE PROCEDURE retrievePermissions(
 @USERNAME VARCHAR(30),
 @PASSHASH VARCHAR(20)
 )
 AS 
 BEGIN 
 SELECT cred.userCredID,cred.username, perm.permission, perm.scopeOfPermission
	FROM DB.DBO.UserCredentials cred 
	INNER JOIN DB.DBO.UserPermissions perm on cred.userCredID = perm.userID
	where cred.username = @USERNAME AND cred.passwordHash = @PASSHASH; 
 END 

 EXEC retrievePermissions @USERNAME= 'Zeina', @PASSHASH = 'PassHash'


 -- connor reg package
-- Connor step one retrieve userID by user Email (more safer)
 CREATE FUNCTION getUserID2(
 @EMAIL VARCHAR(30) 
 )
 RETURNS BIGINT AS
 BEGIN
 RETURN(
 SELECT ua.userID FROM userAccounts ua 
 where ua.email= @EMAIL)
 END



 -- step two make use of a procedure part 2 ( more secure).
 CREATE PROCEDURE UpdatePassword(
 @USEREMAIL VARCHAR(30),
 @PASSHASH VARCHAR(20)
)
 AS 
BEGIN
DECLARE  @UserID varchar(30)
SET @UserID  = DBO.getUserID(@USEREMAIL)
UPDATE userCredentials 
SET userCredentials.passwordHash = @PASSHASH
	WHERE userCredentials.userCredID = @userID 
 END 

-- how do i execute this???


 EXEC retrievePermissions @USERNAME= 'Zeina', @PASSHASH = 'PassHashNew'

 -- inserts:







 
---- INSERTS:



-- INSERT STATEMENTS
 SET IDENTITY_INSERT userAccounts ON
 INSERT INTO userAccounts (userID, email,firstName, lastName, createdAt) VALUES 
 (2, 'ZeinabFarhat@gmail.com', 'Zeinab', 'Farhat','03/21/2021'),
 (4, 'SERGE@gmail.com', 'Sir', 'Je','01/18/2021'),
 (6, 'KoolTrini@gmail.com', 'Kool ', 'Trini','03/21/2021'),
 (8, 'InuRest@gmail.com', 'Inu', 'Rest','03/21/2021'),
 (10, 'goodBoy@gmail.com', 'Good', 'Boy','05/18/2019'),
 (12, 'kingPenny@gmail.com', 'Penny', 'King','02/21/2020'),
 (14, 'JessyJay@gmail.com', 'Jay', 'Jessi','01/14/2019'),
 (16, 'hosehose@gmail.com', 'hose', 'james','03/21/2021'),
 (18, 'jamesBree@gmail.com', 'James', 'Brea','03/21/2021'),
 (20, 'FayJr@gmail.com', 'Fay', 'Junior','01/18/2019'),
 (22, 'cloudRay@gmail.com', 'Cloud', 'Ray','02/18/2018'),
 (24, 'juicyCarter@gmail.com', 'juicy', 'carter','02/18/2018'),
 (26, 'Shein@gmail.com', 'shay', 'mitchel','01/10/2010'),
 (28, 'butterCup@gmail.com', 'Cup', 'better','02/18/2021'),
 (30, 'jamesjoe@gmail.com', 'james', 'joe','03/04/2021'),
 (32, 'pepper@gmail.com', 'pep', 'Ering','03/21/2020'),
 (34, 'jeremia@gmail.com', 'jerry', 'mai','03/04/2020'),
 (36, 'penelopeHope@gmail.com', 'penelope', 'Hope','03/16/2018');
  SET IDENTITY_INSERT userAccounts OFF
 
 INSERT INTO userCredentials (userCredID, username, passwordHash, modifiedAt) VALUES 
 (2, 'Zeina', 'PassHash','03/21/2021'),
 (4, 'SERGE', 'PassHash','01/18/2021'),
 (6, 'KoolTrini', 'PassHash','03/21/2021'),
 (8, 'InuRes', 'PassHash','03/21/2021'),
 (10, 'goodBoy399', 'PassHash','05/18/2019'),
 (12, 'kingPeni393', 'PassHash','02/21/2020'),
 (14, 'JessyJayJay', 'PassHash','01/14/2019'),
 (16, 'hoseho', 'PassHash', '03/21/2021'),
 (18, 'jamesBre343', 'PassHash','03/21/2021'),
 (20, 'Fay79oo', 'PassHash','01/18/2019'),
 (22, 'clo5udy', 'PassHash','02/18/2018'),
 (24, 'juCarter', 'PassHash','02/18/2018'),
 (26, 'Shene', 'PassHash','01/10/2010'),
 (28, 'butter500', 'PassHash','02/18/2021'),
 (30, 'joejamey', 'PassHash','03/04/2021'),
 (32, 'pepper', 'PassHash','03/21/2020'),
 (34, 'jermia', 'PassHash','03/04/2020'),
 (36, 'peneHope', 'PassHash','03/16/2018');


  INSERT INTO userPermissions(userID, permission, scopeOfPermission) VALUES
 (2, 'READ_ONLY', 'AUTOBUILD'),
 (2, 'DELETE', 'SELF'),
 (2, 'UPDATE', 'SELF'),
 (2, 'EDIT', 'SELF'),
 (2, 'CREATE', 'REVIEWS'),
 (2, 'DELETE', 'SELF_REVIEWS'),
 (2, 'UPDATE', 'SELF_REVIEWS');
   INSERT INTO userPermissions(userID, permission, scopeOfPermission) VALUES
 (4, 'READ_ONLY', 'AUTOBUILD'),
 (4, 'DELETE', 'SELF'),
 (4, 'UPDATE', 'SELF'),
 (4, 'EDIT', 'SELF'),
 (4, 'CREATE', 'REVIEWS'),
 (4, 'DELETE', 'SELF_REVIEWS'),
 (4, 'UPDATE', 'SELF_REVIEWS');
  INSERT INTO userPermissions(userID, permission, scopeOfPermission) VALUES
 (6, 'CREATE', 'ANY USER'),
 (6, 'DELETE', 'ANY USERS'),
 (6, 'UPDATE', 'ANY USERS');




 --
 
	-- so what i want to do is upon insertion of the userAccounts
	-- I want to automatically creat the corresponding child references 

	-- https://www.sqlservertutorial.net/sql-server-triggers/sql-server-create-trigger/ 

	--- 1) specify name of trigger:
	CREATE OR Replace TRIGGER UserAccounts_Session
	AFTER INSERT ON UserAccounts 
	FOR EACH ROW
	BEGIN 
		INSERT INTO UserSessions ( userID)
		VALUES ( :NEW.userID );
		END;

	---------------------------


	
	select * from Mostpopularbuilds where UserHashID  = DB.DBO.GetUserHashIDbyUsername('SERGE');
	select * from UserCredentials;




	 SELECT COUNT(*) FROM Mostpopularbuilds MP  WHERE 
	   DAY('2019-05-04') = DAY(CAST( GETDATE() AS DATETIME)) AND
	   MONTH('2019-05-04') = MONTH(CAST( GETDATE() AS DATETIME)) AND 
	   YEAR('2019-05-04') = YEAR(CAST( GETDATE() AS DATETIME)) AND
	   MP.userHashID = DB.DBO.GetUserHashIDbyUsername('clo5udy')
	   
	 SELECT CAST( GETDATE() AS datetimeoffset ) ;
	  SELECT MONTH('2019-05-04');
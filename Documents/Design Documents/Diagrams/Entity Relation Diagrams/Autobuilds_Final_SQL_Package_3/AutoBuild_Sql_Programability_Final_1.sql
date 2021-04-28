 
 -- THE SQL PROGRAMABILITY FILE IS WHERE ANY USER DEFINED , TRIGGERS, PROCEDURES, FUNCTIONS, ETC GOES!!


 
USE DB

 -- 1) AUTOBUILD FUNCTIONS:
		 GO
			 Create FUNCTION GetUserIDByEmail
			 (@EMAIL VARCHAR(30)
			)
			 RETURNS BIGINT AS
			 BEGIN
			 RETURN(
			 SELECT ua.userID FROM UserAccounts ua INNER JOIN UserCredentials uc  
			 on ua.userID = uc.userCredID where ua.email= @EMAIL)
			 END
		 GO


		 GO
			 CREATE FUNCTION  GetUserIDbyUsername
			 ( 
			 @USERNAME VARCHAR(30))
			 RETURNS BIGINT AS
			 BEGIN
			 RETURN(
			 SELECT uc.userCredID FROM UserCredentials uc
			 where uc.username= @USERNAME)
			 END
		 GO



-- 3) DECLARING THE PROCEDURES:  

 ---- A) Authentication procedures. 

 -- PROCUDURE: FOR RETRIEVING THE USER PERMISSIONS UPON PASSING USERNAME AND PASSWORD
		GO
			CREATE PROCEDURE RetrievePermissions(
 @USERNAME VARCHAR(30),
 @PASSHASH VARCHAR(20))
 AS 
 BEGIN 
 SELECT cred.username, perm.permission, perm.scopeOfPermission, cred.locked
	FROM DB.DBO.UserCredentials cred 
	INNER JOIN DB.DBO.UserPermissions perm on cred.userCredID = perm.userID
	where cred.username = @USERNAME AND cred.passwordHash = @PASSHASH
 END 
 GO
 -- END OF PROCEDURE
 DROP PROCEDURE RetrievePermissions;

 -- TESTING THE RetrievePermissions PROCUDURE
 EXEC retrievePermissions @USERNAME= 'Zeina', @PASSHASH = 'PassHash';


 ---- END OF Authentication procedures. 



--- B) REG PACKAGE PROCEDURES (FOR CONNOR)


 -- TYPES AS TABLES:

 -- THE CLAIMS TYPE USED BY RegisterAccount PROCEDURE
		GO
			CREATE TYPE CLAIMSTYPE AS TABLE
		(permission varchar(10) NOT NULL 
		CHECK 
		(permission IN ('CREATE' , 'UPDATE', 'DELETE', 'BLOCK','NONE','READ_ONLY', 'EDIT', 'ALL','WRITE', 'SAVE')),
		scopeOfPermission varchar(20) NOT NULL);
		GO

			DROP PROCEDURE RegisterAccount;



		DECLARE @PERMISSIONS AS CLAIMSTYPE;
-- THIS IS THE PROEDURE FOR REGISTRATION OF USER
		GO
			CREATE PROCEDURE RegisterAccount
			(@LOCKED BIT = 0, -- Default to unlocked
			 @PERMISSIONS CLAIMSTYPE READONLY,
			 @ROLES varchar(20) = 'BASIC', -- default to BASIC 
			 @USERNAME varchar(30),
			 @PASSWORD varchar(20),
			 @EMAIL varchar(30),
			 @FNAME varchar(20),
			 @LNAME varchar(20),
			 @CREATEDATE DATETIME)
			AS 
		BEGIN 
			DECLARE @USERID AS TABLE (ID BIGINT);
		-- THERE IS A SEQUENT TO THINGS, USERACCOUNT -> CREDENTIAL -> THEN THE PERMSISIONS (PK -> FKS)
		-- https://www.c-sharpcorner.com/UploadFile/f82e9a/output-clause-in-sql-server/

		-- STEP 1 : INSERT THE NECESSARY DATA INTO THE USER ACCOUNTS TABLE
			 INSERT INTO userAccounts ( email,firstName, lastName, createdAt) 
			 -- THE USERID IDENTITY WILL AUTOMATICALLY UPDATE SO WE NEED TO STORE IT FOR REFERENCE OF FK!!!
			 OUTPUT INSERTED.userID INTO @USERID
			 VALUES (@EMAIL, @FNAME, @LNAME,@CREATEDATE);
			 -- still thinking bout this I don tlike i t

		-- STEP 2:  INSERT INTO THE CRED TABLE, NOTICE HOW WE MADE USE OF THE @USERID TABLE FOR REFERENCE
			 INSERT INTO userCredentials( userCredID, username, passwordHash, userRole, locked)
			 VALUES ((SELECT UI.ID FROM @USERID  UI), @USERNAME, @PASSWORD, @ROLES, @LOCKED);

		-- STEP 3: INSERT ALL THE DATA FROM THE DATABLE INTO THE USERPERMISSIONS TABLE (NOTICE THE REFERENCE TO @USERID)
			 INSERT INTO userPermissions (userID, permission, scopeOfPermission)
			 SELECT (SELECT UI.ID FROM @USERID  UI), p.permission, p.scopeOfPermission
			 FROM @PERMISSIONS p;
			
		END

		GO
-- END OF PROEDURE FOR REGISTRATION OF USER

-- TESTING PROCEDURE RegisterAccount  -------

		GO
			DECLARE @DEMOCLAIMS AS CLAIMSTYPE  -- @DEMO CLAIMS MIMICS THE PERMISSIONS TO BE PASSED AS A DATATABLE
			INSERT INTO @DEMOCLAIMS VALUES ( 'READ_ONLY', 'AUTOBUILD');
			INSERT INTO @DEMOCLAIMS VALUES ( 'DELETE', 'SELF');
			INSERT INTO @DEMOCLAIMS VALUES ( 'UPDATE', 'SELF');
			INSERT INTO @DEMOCLAIMS VALUES ( 'EDIT', 'SELF');
			INSERT INTO @DEMOCLAIMS VALUES ( 'CREATE', 'REVIEWS');
			INSERT INTO @DEMOCLAIMS VALUES ( 'DELETE', 'SELF_REVIEWS');
			INSERT INTO @DEMOCLAIMS VALUES ( 'UPDATE', 'SELF_REVIEWS');
			EXECUTE DBO.RegisterAccount
			@PERMISSIONS =  @DEMOCLAIMS,
			@USERNAME = 'NEW_USERNAME1',
			--@ROLES = 'BASIC',
			@PASSWORD = 'HASHED+PASSWORD2',
			@EMAIL = 'NEW_EMAIL1@GMAIL.COM' ,
			@FNAME = 'FIRSTNAME',
			@LNAME = 'LASTNAME' ,
			@CREATEDATE = '04-12-2021'
		
		GO 
		GO	-- OUTPUTTING REGISTRATION PROCEDURE EFFECTS:
			SELECT 
			UA.userID AS 'ACCOUNT ID', 
			UP.userID AS 'PERMISSIONS TABLE ID', 
			UC.userCredID AS 'CREDENTIALS ID', 
			UA.email, UA.firstName, UA.lastName,
			UA.createdAt,  UC.username, UC.passwordHash , UC.userRole
			FROM userAccounts UA INNER JOIN userCredentials UC 
			ON UA.userID = UC.userCredID
			INNER JOIN userPermissions UP ON UC.userCredID = UP.userID

			-- https://www.w3schools.com/sql/sql_groupby.asp 
			GROUP BY UA.userID, UA.email, UA.firstName, UA.lastName, UC.userRole,
			UA.createdAt, UC.userCredID, UC.username, UC.passwordHash, UP.userID;
		
		GO
		SELECT * FROM UserPermissions;
		SELECT * FROM UserCredentials;
		
-- END OF PROEDURE 

DROP PROCEDURE UpdateUserPermissions;
--- C) UpdateUserPermissions1 PROCEDURE (FOR CONNOR), THIS IS THE PROEDURE FOR UPDATING OF THE PERMISSIONS 
		GO
			CREATE PROCEDURE UpdateUserPermissions
			(-- WHAT IS IT THAT i WANT TO PASS?
			 @PERMISSIONS CLAIMSTYPE READONLY,
			 @USERNAME varchar(30))
			AS 
		BEGIN 
			DECLARE  @UserID varchar(30)
			SET @UserID  = DB.DBO.GetUserIDbyUsername(@USERNAME)
			DELETE FROM userPermissions where userPermissions.userID = @UserID;
			-- after deleting those permissions do and insert of the new set of permissions 
			INSERT INTO userPermissions (userID, permission, scopeOfPermission)
			SELECT (@UserID), p.permission, p.scopeOfPermission FROM @PERMISSIONS p;
		END

		GO

-- END OF PROEDURE FOR UPDATING OF THE PERMISSIONS -----

--- TESTING  UpdateUserPermissions1 PROCEDURE!!  -------
		GO
				DECLARE @DEMOCLAIMS AS CLAIMSTYPE 
				INSERT INTO @DEMOCLAIMS VALUES ( 'BLOCK', 'N0 SCOPE');
				INSERT INTO @DEMOCLAIMS VALUES ( 'READ_ONLY', 'AUTOBUILD');
				INSERT INTO @DEMOCLAIMS VALUES ( 'DELETE', 'SELF');
				INSERT INTO @DEMOCLAIMS VALUES ( 'UPDATE', 'SELF');
				INSERT INTO @DEMOCLAIMS VALUES ( 'EDIT', 'SELF');
				--INSERT INTO @DEMOCLAIMS VALUES ( 'CREATE', 'REVIEWS');
				--INSERT INTO @DEMOCLAIMS VALUES ( 'DELETE', 'SELF_REVIEWS');
				--INSERT INTO @DEMOCLAIMS VALUES ( 'UPDATE', 'SELF_REVIEWS');
				EXECUTE DBO.UpdateUserPermissions
				 @USERNAME = 'NEW_USERNAME1',
				 @PERMISSIONS =  @DEMOCLAIMS; -- HERE I SPECIFIED THE USERNAME (BUT THIS
				-- WILL BE RETRIEVED FROM THE THREAD.)
		GO
				-- TO SEE IF IT WORKED AS PLANNED
				SELECT 
				UP.userID AS 'PERMISSIONS TABLE ID', UP.permission, UP.scopeOfPermission
				FROM userPermissions UP 
				WHERE UP.userID = 38;


--- END OF  UpdateUserPermissions1 PROCEDURE1 !!  -------







DROP PROCEDURE UpdateUserPermissions2; 
--- C2) UpdateUserPermissions2 PROCEDURE (FOR CONNOR)
-- THIS ACKNOWLEDGES THE UPDATE FOR THE ROLE AS WELL IN USERCREDENTIALS TABLE
		GO

			CREATE PROCEDURE UpdateUserPermissions2
			(-- WHAT IS IT THAT i WANT TO PASS?
			 @PERMISSIONS CLAIMSTYPE READONLY,
			 @USERNAME varchar(30),
			 @ROLE varchar(20))
			AS 
		BEGIN 
			DECLARE  @UserID varchar(30)
			SET @UserID  = DB.DBO.GetUserIDbyUsername(@USERNAME)
			DELETE FROM userPermissions where userPermissions.userID = @UserID;
			UPDATE UserCredentials SET UserCredentials.userRole = @ROLE where userCredID = @UserID;
			-- after deleting those permissions do and insert of the new set of permissions 
			INSERT INTO userPermissions (userID, permission, scopeOfPermission)
			SELECT (@UserID), p.permission, p.scopeOfPermission FROM @PERMISSIONS p;
		END

		GO

-- END OF PROEDURE FOR UPDATING OF THE PERMISSIONS ----- 
--- TESTING  UpdateUserPermissions PROCEDURE!!  -------
		GO
				DECLARE @DEMOCLAIMS AS CLAIMSTYPE 
				INSERT INTO @DEMOCLAIMS VALUES ( 'BLOCK', 'N0 SCOPE');
				INSERT INTO @DEMOCLAIMS VALUES ( 'READ_ONLY', 'AUTOBUILD');
				INSERT INTO @DEMOCLAIMS VALUES ( 'DELETE', 'SELF');
				INSERT INTO @DEMOCLAIMS VALUES ( 'UPDATE', 'SELF');
				INSERT INTO @DEMOCLAIMS VALUES ( 'EDIT', 'SELF');
				--INSERT INTO @DEMOCLAIMS VALUES ( 'CREATE', 'REVIEWS');
				--INSERT INTO @DEMOCLAIMS VALUES ( 'DELETE', 'SELF_REVIEWS');
				--INSERT INTO @DEMOCLAIMS VALUES ( 'UPDATE', 'SELF_REVIEWS');
				EXECUTE DBO.UpdateUserPermissions2
				 @USERNAME = 'NEW_USERNAME1',-- HERE I SPECIFIED THE USERNAME (BUT THIS
				-- WILL BE RETRIEVED FROM THE THREAD
				 @PERMISSIONS =  @DEMOCLAIMS,
				 @ROLE = 'NEW ROLE!!';
		GO
				-- TO SEE IF IT WORKED AS PLANNED
				SELECT 
				UP.userID AS 'PERMISSIONS TABLE ID', UP.permission, UP.scopeOfPermission, UC.userRole
				FROM userPermissions UP  inner join UserCredentials UC
				on UP.userID = UC.userCredID
				WHERE UP.userID = 38;


--- END OF  UpdateUserPermissions2 PROCEDURE !!  -------


-- D) GetAllProductsByVendor2  FILTERING PRODUCTS STUFF (PER DANNY)


 -- TYPES AS TABLES: this is modeling the incoming parameter for the procedure GetAllProductsByVendor2
		 GO
		 CREATE TYPE Filters AS TABLE
		(
		 productType varchar(30),
		 FilterOn bit
		);
		GO

-- PROCEDURE TO GET PRODUCTS PER VENDOR ---
		GO
		 create procedure GetAllProductsByVendor2(
		 @USERNAME varchar(30),
		 @FilterList Filters READONLY,
		 @ORDER varchar(30)
		 )  
		 AS 
		BEGIN
			DECLARE  @USERID varchar(30)
			SET @USERID  = db.dbo.GetUserIDbyUsername(@USERNAME)
			select vp.productName, p.ProductType, vp.vendorImageURL, vp.productStatus, v.vendorName, vp.VendorLinkURL, p.modelNumber, vp.productPrice 
			from (((vendor_product_junction as vp inner join products as p on vp.productID = p.productID) inner join vendorclub as v on vp.vendorID = v.vendorID) 
					inner join Vendor_UserAccount_Junction vuj on vuj.vendorID = v.vendorID)
					where vuj.userID = @USERID
					and exists (select * from @FilterList fl where p.productType = fl.productType and fl.filterOn = 1)
					order by 
					case when @order = 'asc' then
					vp.productPrice end asc,
					case when @order = 'desc' then
					vp.productPrice end desc
		END

		GO

-- END OF PROCEDURE GetAllProductsByVendor2  ---


-- TESTING PRODUCT PROCEDURES
		GO
		DECLARE @FilterList AS Filters

		DECLARE @DEMO AS Filters
			INSERT INTO @DEMO VALUES ('motherboard', 1);
			INSERT INTO @DEMO VALUES ('case', 1);
			INSERT INTO @DEMO VALUES ('cpu', 1);
			exec GetAllProductsByVendor2 @username = 'new egg', @filterlist = @DEMO, @order = 'asc'
		GO
-- END OF TESTING PRODUCT PREOCEDURES 


		
-- END OF -- GetAllProductsByVendor2




-- NICKS PRODUCTS DEMO:

	select * from Products;
	select * from Vendor_Product_Junction;

GO
CREATE TYPE BuilderType AS TABLE 
(
 productType varchar(50),
 productPrice decimal(10,2)
);
GO

DECLARE @PRODUCTS AS BuilderType;

GO
CREATE PROCEDURE Search_ProductBudget
( @PRODUCTS BuilderType READONLY)
AS
BEGIN 
select vp.VendorLinkURL , vp.productPrice from Products  P INNER JOIN  Vendor_Product_Junction VP  ON P.productID = VP.productID
	WHERE EXISTS 
	(SELECT * FROM @PRODUCTS PP WHERE 
	 PP.productType = P.productType -- delta (never above )
	 AND (PP.productPrice BETWEEN VP.productPrice - 10 AND VP.productPrice + 10)) 
	--PP.productPrice = VP.productPrice)
END 
GO



drop procedure Search_ProductBudget;
--- TESTING  UpdateUserPermissions PROCEDURE!!  -------
		GO
				DECLARE @DEMOBUILDER AS  BuilderType
				INSERT INTO @DEMOBUILDER VALUES ( 'cpu', 200.00);
				INSERT INTO @DEMOBUILDER VALUES ( 'power supply', 160.00);
				INSERT INTO @DEMOBUILDER VALUES ( 'CASE', 90.00);
				--INSERT INTO @DEMOBUILDER VALUES ( 'motherboard', 500.00);
				--INSERT INTO @DEMOBUILDER VALUES ( 'power supply', 40.99);
				--INSERT INTO @DEMOBUILDER VALUES ( 'cpu', 200.00);
				--INSERT INTO @DEMOBUILDER VALUES ( 'cpu', 200.00);
				EXECUTE Search_ProductBudget  @PRODUCTS =  @DEMOBUILDER; -- HERE I SPECIFIED THE USERNAME (BUT THIS
				-- WILL BE RETRIEVED FROM THE THREAD.)
		GO
		
				-- TO SEE IF IT WORKED AS PLANNED
				
	select * from Products  P INNER JOIN  Vendor_Product_Junction VP  ON P.productID = VP.productID
	WHERE P.productType = 'power supply';
	select * from Products;
	select * from Vendor_Product_Junction;



-- FOR UAD:
-- SPECIFICALLY ADDRESSING THE LOGS:

-- 

-- GOAL CREATE A PROCEDURE FOR LOG TO ADDRESS:
-- PASSING THE PAGE ID FROM THE THREAD, BECAUSE PAGE VISITS WILL GENERATE AN PAGE ID FOR THE CONTROLLER BEING CALLED
-- (IDEA 1) THERE WILL BE AN INTITAL LOG CREATED -> THAT MUST IN TURN CREATE THE SESSIONS TABLE 
-- (IDEA 2) REGARDLESS OF WHETHER LOGS ARE CREATED WHEN A REQUEST COMES THROUGH THERE MUST BE A SESSIONS TABLE CREATE FOR THAT PAGE ID, 
-- THEN RETURNED BACK TO BE SAVED ON THE THREAD IT THE SESSIONS ID PRIMARY KEY! IN TURN WHEN A LOG IS TO BE CREATED FOR THAT 'USER'
-- ON THE THREAD IT REFERENCES THE SESSIONS ID TO GO TO IN THE CASE OF GUEST ELSE IT REFERENCES THE USER ACCOUNT IN CASE OF REGISTERED USER
-- 




-- UAD Procedures:
DROP PROCEDURE GetAllAnalytics;

GO
CREATE PROCEDURE GetAllAnalytics
AS
BEGIN
-- Bar graph 1: The number of accounts (Y Axis) held
-- amongst account types (Label : Vendor, Admin,
-- Basic , Devoloper) *no legends used

select uc.userRole AS 'X_Value', count(distinct uc.userCredID)AS 'Y_Value', 'N/A' AS 'Legend'
from UserCredentials uc group by uc.userRole;

END


-- BAR GRAPH 2 : Percentage usage of Autobuild components 
-- Y-Axis: usage can be time, or page visits maybe, X-Axis:
-- is the Autobuild component
BEGIN
SELECT US.pageID AS 'X_Value' , COUNT(US.pageID) AS 'Y_Value', 'N/A' AS 'Legend'
			FROM UserSessions US GROUP BY  US.pageID,  US.pageID;

END
-- Line chart 1: Number of registrations (Y Axis) that
-- took place every month (x - axis) and the label show
-- the type of user
BEGIN

SELECT MONTH(UA.createdAt) AS 'X_Value', count(distinct ua.userID) AS 'Y_Value',
CASE WHEN ISNULL(UC.userRole, 'empty')  = 'empty' THEN 'N/A'
     WHEN UC.userRole != 'NULL' THEN UC.userRole
END
AS 'Legend' 
--U.nameOfpermission AS 'Legend'
FROM UserCredentials UC  right join UserAccounts UA ON UC.userCredID = UA.userID
group by  UC.userRole ,MONTH(ua.createdAt);


END

BEGIN

-- Bar Graph 3: Average Session duration(yaxis) of user (shown as labels) *no legends used


SELECT graph3.userRole AS 'X_Value' , SUM(graph3.TimeSpent) AS  'Y_Value', 'N/A' AS 'Legend'
FROM (

SELECT US.userID ,UC.userRole, (DATEDIFF(HOUR,US.beginAt, US.endAt )) AS 'TimeSpent'
FROM UserAccounts UA inner join 
UserSessions US on UA.userID = US.userID
inner join UserCredentials  UC on  UC.userCredID = US.userID
GROUP BY US.userID, UC.userRole, US.endAt, US.beginAt
) AS graph3
GROUP BY graph3.userRole

END 

BEGIN 

-- LINE CHART 3:Most frequently viewed Autobuild view per
-- month. Number of views as Y-axis , Time on X-axis, and legends are
-- the Autobuild views

SELECT  
	MONTH (US.beginAt) AS 'X_Value',
	us.pageID AS 'Legend',
	COUNT(US.pageID) as 'Y_Value'
	FROM UserSessions US 
	group by  us.pageID, MONTH (US.beginAt);


END

GO
--

DROP PROCEDURE GetAllAnalytics;

Create procedure getProducts

as

begin 
	--select * from Products p;
	--select * from Products_Specs;
	
select  count( ps.productID)
from products p inner join Products_Specs ps on p.productID = ps.productID
group by p.productID

	
select  p.manufacturerName, p.modelNumber ,ps.productSpecs, ps.productSpecsValue
from products p inner join Products_Specs ps on p.productID = ps.productID

end

drop procedure getProducts;

	select * from Products p;
	select * from Products_Specs;


select p.productID, count( ps.productID)
from products p inner join Products_Specs ps on p.productID = ps.productID
group by p.productID
 
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
			 SELECT ua.userID FROM UserAccounts ua where ua.email= @EMAIL)
			 END
		 GO


		 GO
			 CREATE FUNCTION  GetUserIDbyUsername
			 ( 
			 @USERNAME VARCHAR(30))
			 RETURNS BIGINT AS
			 BEGIN
			 RETURN(
			 SELECT ua.userID FROM UserCredentials uc
			 INNER JOIN MappingHash mh
			 ON mh.userHashID = uc.userHashID
			 INNER JOIN UserAccounts ua
			 ON ua.userID = mh.userID
			 where uc.username= @USERNAME)
			 END
		 GO

		 GO
			 CREATE FUNCTION  GetUserHashIDbyUsername
			 ( 
			 @USERNAME VARCHAR(30))
			 RETURNS BINARY(64) AS
			 BEGIN
			 RETURN(
			 SELECT UC.userHashID FROM MappingHash MH INNER JOIN UserCredentials UC
			 ON UC.userHashID = MH.userHashID
			 WHERE UC.username = @USERNAME)
			 END
		 GO

-- 3) DECLARING THE PROCEDURES:  

 ---- A) Authentication procedures. 

 -- PROCUDURE: FOR RETRIEVING THE USER PERMISSIONS UPON PASSING USERNAME AND PASSWORD

 DROP PROCEDURE RetrievePermissions;


 -- PROCUDURE: FOR RETRIEVING THE USER PERMISSIONS UPON PASSING USERNAME AND PASSWORD
		DROP PROCEDURE RetrievePermissions;
		GO
			CREATE PROCEDURE RetrievePermissions(
 @USERNAME VARCHAR(30),
 @PASSHASH VARCHAR(20))
 AS 
 BEGIN 
 SELECT cred.username, perm.permission, perm.scopeOfPermission, cred.locked
	FROM DB.DBO.UserCredentials cred 
	INNER JOIN DB.DBO.UserPermissions perm on cred.userHashID = perm.userHashID 
	where cred.username = @USERNAME AND cred.passwordHash = @PASSHASH
 END 
 GO
 -- END OF PROCEDURE

 -- TESTING THE RetrievePermissions PROCUDURE
 EXEC RetrievePermissions @USERNAME= 'SERGE', @PASSHASH = 'PassHash';


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



	
-- REGISTRATION PROCEDURE PART 2 WITH HASH

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

		-- STEP 2: INSERT THE NECESSARY DATA INTO THE HASHMAPPING TABLE:
		--https://docs.microsoft.com/en-us/sql/t-sql/functions/hashbytes-transact-sql?view=sql-server-ver15 
		    DECLARE @HASHBYTES BINARY(64) --limit put it 
			SET @HASHBYTES = HASHBYTES('SHA2_256', CONCAT(@CREATEDATE, @USERNAME,(SELECT UI.ID FROM @USERID  UI)))
			INSERT INTO MappingHash (userID, userHashID) VALUES ((SELECT UI.ID FROM @USERID  UI), @HASHBYTES)

		-- STEP 2:  INSERT INTO THE CRED TABLE, NOTICE HOW WE MADE USE OF THE @USERID TABLE FOR REFERENCE
			 INSERT INTO userCredentials( userHashID, username, passwordHash, userRole, locked)
			 VALUES (@HASHBYTES, @USERNAME, @PASSWORD, @ROLES, @LOCKED);

		-- STEP 3: INSERT ALL THE DATA FROM THE DATABLE INTO THE USERPERMISSIONS TABLE (NOTICE THE REFERENCE TO @USERID)
			 INSERT INTO userPermissions (userHashID, permission, scopeOfPermission)
			 SELECT @HASHBYTES, p.permission, p.scopeOfPermission
			 FROM @PERMISSIONS p;
			
		END

		GO

		-- TESTING PROCEDURE RegisterAccount2  -------

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
		SELECT * FROM MappingHash
		select * from UserAccounts UA inner join MappingHash MH on UA.userID = MH.userID;
		SELECT * FROM UserPermissions;
		SELECT * FROM UserCredentials;

-- END OF REGISTRATION PROCEDURE PART TWO







--- END OF  UpdateUserPermissions2 PROCEDURE !!  -------



DROP PROCEDURE UpdateUserPermissions; 
--- C3) UpdateUserPermissions3 PROCEDURE (FOR CONNOR)
-- THIS ACKNOWLEDGES THE UPDATE FOR THE ROLE AS WELL IN USERCREDENTIALS TABLE
		GO

			CREATE PROCEDURE UpdateUserPermissions
			(-- WHAT IS IT THAT i WANT TO PASS?
			 @PERMISSIONS CLAIMSTYPE READONLY,
			 @USERNAME varchar(30),
			 @USERROLE varchar(20))
			AS 
		BEGIN 
			DECLARE @UserHashID Binary(64)
			SET @UserHashID  = DB.DBO.GetUserHashIDbyUsername(@USERNAME)
			DELETE FROM userPermissions where userPermissions.userHashID = @UserHashID;
			UPDATE UserCredentials SET UserCredentials.userRole = @USERROLE where userHashID = @UserHashID;
			-- after deleting those permissions do and insert of the new set of permissions 
			INSERT INTO userPermissions (userHashID, permission, scopeOfPermission)
			SELECT (@UserHashID), p.permission, p.scopeOfPermission FROM @PERMISSIONS p;
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
				EXECUTE DBO.UpdateUserPermissions
				 @USERNAME = 'NEW_USERNAME1',-- HERE I SPECIFIED THE USERNAME (BUT THIS
				-- WILL BE RETRIEVED FROM THE THREAD
				 @PERMISSIONS =  @DEMOCLAIMS,
				 @USERROLE = 'NEW ROLE!!';
		GO

		select * from UserPermissions UP inner join UserCredentials UC
				on UP.userHashID = UC.userHashID;


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

 -- TYPES AS TABLES:

 -- THE LOGS TYPE USED BY INSERT LOGS PROCEDURE
		GO
			CREATE TYPE LOGSTYPE AS TABLE(
		message text, -- 
		loglevel Varchar(20),
		dateTime Varchar(50));
		GO

		GO
			


DROP PROCEDURE INSERTLOGS;

GO
CREATE PROCEDURE INSERTLOGS
(
@USERNAME varchar(30) = 'N/A' , -- USERNAME SET IN THE THREAD BUT IS OPTIONAL IN CASE OF GUEST USER.
@LOGS LOGSTYPE READONLY,
@SESSIONSID BIGINT, -- THIS WILL BE GENERATED IN THE C# CODE 
@PAGEID INT = 0, -- THIS WILL COME FROM THE ENUMERATIONS ON THE CONTROLLERS.
@BEGINTIME DATETIME)
AS 
BEGIN 
-- STEP (1) IS THIS SESSION LINKED TO A USER ACCOUNT?
	DECLARE  @USERID varchar(30)
			SET @USERID  = db.dbo.GetUserIDbyUsername(@USERNAME)
	DECLARE @NUMBEROFROWS INTEGER 
			SET @NUMBEROFROWS  = db.dbo.IsSessionExist(@SESSIONSID)
		-- THE SESSIONSID EXISTS SO THIS MEANS MORE LOGS UNDER THE SOME SESSION
		-- SO IT IS ENOUGH TO INSERT INTO THE LOG TABLE AND 
		-- REFERENCE THE PK
		IF @NUMBEROFROWS = 1
			BEGIN
			INSERT INTO Logs(visitorID, message, loglevel, dateTime)
			SELECT (@SESSIONSID),L.message, L.loglevel, L.dateTime FROM @LOGS L;
			END
		ELSE
			BEGIN
				IF @USERID IS NOT NULL
					BEGIN 
						-- THIS MEANS THAT THERE IS A USER CURRENTLY ON
						-- THE THREAD AND WE WILL ADD A SESSIONS ROW TO 
						-- REFERENCE THEIR ACCOUNT 
						
						 SET IDENTITY_INSERT UserSessions ON
						INSERT INTO UserSessions (visitorID, pageID, userID, beginAt)
						VALUES (@SESSIONSID, @PAGEID, @USERID, @BEGINTIME);
						SET IDENTITY_INSERT UserSessions OFF
						-- NOW THAT THE SESSIONS ROW IS CREATED FOR THE USER
						-- WE WILL ADD THE LOG UNDER THAT SESSION:
						INSERT INTO Logs(visitorID, message, loglevel, dateTime)
						SELECT @SESSIONSID,L.message, L.loglevel, L.dateTime FROM @LOGS L;
					END
				ELSE --(JUST INSERT INTO USER SESSIONS WITH USERID -> NULL)
					BEGIN
						-- THIS IS FOR GUEST SO JUST ADD SESSION AND LOG
						
						SET IDENTITY_INSERT UserSessions ON
						INSERT INTO UserSessions (visitorID, pageID, beginAt)
						VALUES (@SESSIONSID, @PAGEID, @BEGINTIME)
						SET IDENTITY_INSERT UserSessions OFF
						-- NOW THAT THE SESSIONS ROW IS CREATED FOR THE USER
						-- WE WILL ADD THE LOG UNDER THAT SESSION:
						INSERT INTO Logs(visitorID, message, loglevel, dateTime)
						SELECT @SESSIONSID,L.message, L.loglevel, L.dateTime FROM @LOGS L
					END
			END
END
GO
-- TESTING THE INSERT LOGS PROCEDURE
		GO
				DECLARE @lOG AS  LOGSTYPE
				INSERT INTO @lOG VALUES ('THIS IS A TEST1', '0', '03/21/2021');
				--INSERT INTO @lOG VALUES ('THIS IS A TEST2', '0', '03/22/2021');
				--INSERT INTO @lOG VALUES ('THIS IS A TEST3', '0', '03/23/2021'); 

				EXECUTE INSERTLOGS  @LOGS =  @LOG, @SESSIONSID = 26000, @BEGINTIME = '03/21/2021', @USERNAME = 'hoseho'; -- 'hoseho' has userid 16
		GO

		--
-- END OF TESTING THE INSERT LOGS PROCEDURE




select * from Logs;

-- UAD Procedures:
DROP PROCEDURE GetAllAnalytics;

GO
CREATE PROCEDURE GetAllAnalytics
AS
BEGIN
-- Bar graph 1: The number of accounts (Y Axis) held
-- amongst account types (Label : Vendor, Admin,
-- Basic , Devoloper) *no legends used

-- VERSION 2
select uc.userRole AS 'X_Value', count(distinct uc.userHashID)AS 'Y_Value', 'N/A' AS 'Legend'
from UserCredentials uc group by uc.userRole;


END


BEGIN
-- BAR GRAPH 2 : Percentage usage of Autobuild components 
-- Y-Axis: usage can be time, or page visits maybe, X-Axis:
-- is the Autobuild component
-- VERSION 2
SELECT 
CASE 
     WHEN (L.eventValue IS NULL) THEN -99
     ELSE L.eventValue 
END
AS 'X_Value'  ,
COUNT(L.userHashID) AS 'Y_Value', 
'N/A' AS 'Legend'
FROM Logs L 
where L.eventType = 'VIEW_PAGE_EVENT'
GROUP BY  L.eventValue;


END

-- Line chart 1: Number of registrations (Y Axis) that
-- took place every month (x - axis) and the label show
-- the type of user
BEGIN

-- VERSION 2
SELECT MONTH(UA.createdAt) AS 'X_Value', 
count(distinct ua.userID) AS 'Y_Value', 
UC.userRole AS 'Legend' 
FROM UserCredentials UC  right join MappingHash MH ON UC.userHashID = MH.userHashID
inner join UserAccounts UA on UA.userID = MH.userID
group by  UC.userRole ,MONTH(UA.createdAt);

--SELECT MONTH(UC.createdAt) AS 'X_Value', count(distinct UC.userHashID) AS 'Y_Value', UC.userRole AS 'Legend' 
--U.nameOfpermission AS 'Legend'
--FROM UserCredentials UC  right join MappingHash MH ON UC.userHashID = MH.userHashID
--group by  UC.userRole ,MONTH(UC.createdAt);

END

BEGIN

-- Bar Graph 3: Average Session duration(yaxis) of user (shown as labels) *no legends used

-- VERSION 2
SELECT graph3.userRole AS 'X_Value' , SUM(graph3.TimeSpent) AS  'Y_Value', 'N/A' AS 'Legend'
FROM (

SELECT logins.userHashID ,UC.userRole, (DATEDIFF(HOUR,Logins.creationDate, Logout.creationDate )) AS 'TimeSpent'
FROM MappingHash MH
inner join UserCredentials  UC on  UC.userHashID = MH.userHashID
inner join Logs Logins on Logins.userHashID = MH.userHashID
inner join Logs Logout on Logout.userHashID = Logins.userHashID
WHERE Logins.eventType ='LOGIN_EVENT' and Logout.eventType ='LOGOUT_EVENT'
GROUP BY Logins.userHashID, UC.userRole, Logins.creationDate, Logout.creationDate,UC.username 
) AS graph3
GROUP BY graph3.userRole

END 

BEGIN 

-- LINE CHART 3:
-- Most frequently viewed Autobuild view ( Number of views as Y-axis) per
-- month (X-axis) . and legends are the Autobuild views

-- VERSION 2
	SELECT  
	MONTH (L.creationDate) AS 'X_Value',
	CASE 
     WHEN (L.eventValue IS NULL) THEN 'N/A'
     ELSE L.eventValue
	 END
	 AS 'Legend' ,
	COUNT(L.eventType) as 'Y_Value'
	FROM Logs L 
	WHERE L.eventType = 'VIEW_PAGE_EVENT'
	group by  L.eventValue, MONTH (L.creationDate);


END

GO
--
	GO 
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
GO

drop procedure getProducts;

	select * from Products p;
	select * from Products_Specs;


select p.productID, count( ps.productID)
from products p inner join Products_Specs ps on p.productID = ps.productID
group by p.productID

go


-- for sirage:



--- insert like:
		 
USE DB;

DROP PROCEDURE InsertLikes; 
--- C3) UpdateUserPermissions3 PROCEDURE (FOR CONNOR)
-- THIS ACKNOWLEDGES THE UPDATE FOR THE ROLE AS WELL IN USERCREDENTIALS TABLE
		GO

			CREATE PROCEDURE  InsertLikes
			(-- WHAT IS IT THAT i WANT TO PASS?
			 @POSTID  VARCHAR(100),
			 @USERNAME varchar(30))
			AS 
		BEGIN 
			DECLARE @UserHashID Binary(64)
			SET @UserHashID  = DB.DBO.GetUserHashIDbyUsername(@USERNAME)
			INSERT INTO Likes_Association(userHashID, postID)
			VALUES (@UserHashID, @POSTID);
		END

		-- testing the insert likes:
		GO


		GO
				EXECUTE INSERTLIKES  @POSTID = 30016 , @USERNAME = 'hoseho'; -- 'hoseho' has userid 16

				SELECT * FROM Likes_Association;
		GO


		DROP PROCEDURE InsertReview; 
--- C3) UpdateUserPermissions3 PROCEDURE (FOR CONNOR)
-- THIS ACKNOWLEDGES THE UPDATE FOR THE ROLE AS WELL IN USERCREDENTIALS TABLE
		GO
			CREATE PROCEDURE   InsertReview
			(-- WHAT IS IT THAT i WANT TO PASS?
			 @MESSAGE TEXT,
			 @STAR INT,
			 @FILEPATH  VARCHAR(MAX) = 'N/A',
			 @USERNAME varchar(50) = NULL,
			 @DATETIME DATETIME)
			AS 
		BEGIN 
			DECLARE @UserHashID Binary(64)
			SET @UserHashID  = DB.DBO.GetUserHashIDbyUsername(@USERNAME)
			INSERT INTO reviews(userHashID , message, star,  filepath, datetime)
			VALUES (@UserHashID, @MESSAGE, @STAR, @FILEPATH, @DATETIME);
		END
		GO

		
		GO
				EXECUTE InsertReview 
				@MESSAGE = 'THIS IS TEST',
				@STAR = '8',
				@USERNAME = 'SERGE',
				@DATETIME = '03/21/2021'; -- 'hoseho' has userid 16

				
				EXECUTE InsertReview 
				@MESSAGE = 'THIS IS TEST',
				@STAR = '6',
				@DATETIME = '03/30/2010'; -- 'hoseho' has userid 16

				SELECT * FROM Reviews;
		GO



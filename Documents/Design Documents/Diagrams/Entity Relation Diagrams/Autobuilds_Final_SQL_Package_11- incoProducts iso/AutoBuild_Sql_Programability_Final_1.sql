 
 -- THE SQL PROGRAMABILITY FILE IS WHERE ANY USER DEFINED , TRIGGERS, PROCEDURES, FUNCTIONS, ETC GOES!!
 
 
USE DB;

 -- 1) AUTOBUILD FUNCTIONS:
 
		 DROP FUNCTION IF EXISTS GetUserIDByEmail;
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

		 
		 DROP FUNCTION IF EXISTS GetUserIDbyUsername;
		 GO
			 CREATE FUNCTION  GetUserIDbyUsername
			 ( 
			 @USERNAME VARCHAR(30))
			 RETURNS BIGINT AS
			 BEGIN
			 RETURN(
			 SELECT mh.userID FROM UserCredentials uc
			 INNER JOIN MappingHash mh
			 ON mh.userHashID = uc.userHashID
			 where uc.username= @USERNAME)
			 END
		 GO
		
		
		DROP FUNCTION IF EXISTS GetUserHashIDbyUsername;
		 GO
			 CREATE FUNCTION  GetUserHashIDbyUsername
			 ( 
			 @USERNAME VARCHAR(30))
			 RETURNS BINARY(32) AS
			 BEGIN
			 RETURN(
			 SELECT UC.userHashID FROM MappingHash MH INNER JOIN UserCredentials UC
			 ON UC.userHashID = MH.userHashID
			 WHERE UC.username = @USERNAME)
			 END
		 GO

	
	
	    DROP FUNCTION IF EXISTS GetUsernameByHashID;
		  GO
			 CREATE FUNCTION  GetUsernameByHashID
			 ( 
			 @USERHASHID BINARY(32))
			 RETURNS VARCHAR(30) AS
			 BEGIN
			 RETURN(
			 SELECT UC.username 
			 FROM MappingHash MH INNER JOIN UserCredentials UC
			 ON UC.userHashID = MH.userHashID
			 WHERE MH.userHashID = @USERHASHID)
			 END
		 GO



-- 3) DECLARING THE PROCEDURES:  

 ---- A) Authentication procedures. 

 -- PROCUDURE: FOR RETRIEVING THE USER PERMISSIONS UPON PASSING USERNAME AND PASSWORD
;


 -- PROCUDURE: FOR RETRIEVING THE USER PERMISSIONS UPON PASSING USERNAME AND PASSWORD
DROP PROCEDURE IF EXISTS RetrievePermissions;

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
 EXEC RetrievePermissions @USERNAME= 'Zeina', @PASSHASH = 'PassHash';


 ---- END OF Authentication procedures. 




 -- START OF DELETE ACCOUNT PROCEDURE:

 -- B) DELETIONG AN ACCOUNT :
DROP PROCEDURE IF EXISTS DeleteAccount;

GO
CREATE PROCEDURE DeleteAccount(
@USERNAME VARCHAR(50),
@DATEDELETE DATETIMEOFFSET)
AS 
BEGIN 
			DECLARE @UserHashID BINARY(32)
			SET @UserHashID  = DB.DBO.GetUserHashIDbyUsername(@USERNAME)
			
			DECLARE @UserID BIGINT
			SET @UserID  = DB.DBO.GetUserIDbyUsername(@USERNAME)

			DECLARE @NewUserName varchar(50)  --limit put it 
			SET @NewUserName =  CONCAT(@DATEDELETE, @USERNAME)

			-- SET THE FORIEGN KEY TO NULL BEFORE 
			UPDATE MappingHash
			SET userID = NULL,
			isExistingAccount = 0
			WHERE UserID =  @UserID;

			-- STEP ONE: DELETE THE USERACCOUNT TABLE:
			DELETE FROM UserAccounts WHERE UserAccounts.userID = @UserID;
			--STEP TWO: UPDATE THOSE UNIQUE ATTRIBUTES THAT 
			-- IDENTITFY THE USER: (USERNAME AND PASSWORD) IN  CREDENTIALS TABLES 
		
			UPDATE UserCredentials 
			SET username = @NewUserName,
				passwordHash = 'N/A'
			WHERE userHashID = @UserHashID;
			
			--SELECT MP.isExistingAccount FROM MappingHash MP WHERE MP.userHashID = @UserHashID;
			
END
GO

-- TESTIKNG DeleteAccount OUT!!\
			GO

			EXEC DeleteAccount @USERNAME = 'joejamey', @DATEDELETE = '2020-12-12 11:30:30.12345';

			GO

--- LETS SEE RESULTS:
			GO

			SELECT * FROM UserAccounts;
			SELECT * FROM UserCredentials WHERE UserCredentials.userHashID = 0x5553455249445F32000000000000000000000000000000000000000000000000; 
			
			SELECT * FROM MappingHash WHERE MappingHash.userHashID = 0x5553455249445F32000000000000000000000000000000000000000000000000;

			--UPDATE MappingHash SET userID = NULL WHERE UserID = 16; 
			-- DELETE FROM UserAccounts WHERE UserAccounts.userID = 2;

			GO

 -- END OF START OF DELETE ACCOUNT PROCEDURE:


--- B)REGISTRATION  PROCEDURES


DROP PROCEDURE IF EXISTS UpdateUserPermissions; 
DROP PROCEDURE IF EXISTS RegisterAccount;
 -- TYPES AS TABLES:
 DROP TYPE IF EXISTS CLAIMSTYPE;
 -- THE CLAIMS TYPE USED BY RegisterAccount PROCEDURE
		GO
			CREATE TYPE CLAIMSTYPE AS TABLE
		(permission varchar(10) NOT NULL 
		 CHECK (permission IN
				('Create' , 'Update', 'Delete' ,
				 'Block'  ,'None'   , 'ReadOnly', 
				 'Edit'   , 'All'   , 'Write'   , 'Save')),
  scopeOfPermission varchar(20) NOT NULL);
		GO


		DECLARE @PERMISSIONS AS CLAIMSTYPE;
-- THIS IS THE PROEDURE FOR REGISTRATION OF USER

	SELECT * FROM UserAccounts WHERE USERID =  
	DB.DBO.GetUserIDbyUsername('NEW_USERNAME1');;

	SELECT * FROM UserCredentials WHERE userHashID =  
	DB.DBO.GetUserHashIDbyUsername('NEW_USERNAME1');;

	DELETE FROM UserAccounts  
	WHERE USERID =  DB.DBO.GetUserIDbyUsername('NEW_USERNAME1');

	-- need to delete the previous created useername
	EXEC DeleteAccount @USERNAME = 'NEW_USERNAME1', @DATEDELETE =  '2020-04-12 11:30:30.1234567';

-- REGISTRATION PROCEDURE PART 2 WITH HASH


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
			
		    DECLARE @HASHBYTES BINARY(32) --limit put it 
			SET @HASHBYTES = HASHBYTES('SHA2_256', CONCAT(@CREATEDATE, @USERNAME,(SELECT UI.ID FROM @USERID  UI)));
			INSERT INTO MappingHash (userID, userHashID, isExistingAccount) 
			VALUES ((SELECT UI.ID FROM @USERID  UI), @HASHBYTES, 1);

			 
		-- STEP 2:  INSERT INTO THE CRED TABLE, NOTICE HOW WE MADE USE OF THE @USERID TABLE FOR REFERENCE
			 INSERT INTO userCredentials (userHashID, username, passwordHash, userRole, locked)
			 VALUES (@HASHBYTES, @USERNAME, @PASSWORD, @ROLES, @LOCKED);
			 

		-- STEP 3: INSERT ALL THE DATA FROM THE DATABLE INTO THE USERPERMISSIONS TABLE (NOTICE THE REFERENCE TO @USERID)
			 INSERT INTO userPermissions(userHashID, permission, scopeOfPermission)
			 SELECT @HASHBYTES, p.permission, p.scopeOfPermission
			 FROM @PERMISSIONS p;
			 
		END
		GO

		-- TESTING PROCEDURE RegisterAccount2  -------

		GO
			DECLARE @DEMOCLAIMS AS CLAIMSTYPE  -- @DEMO CLAIMS MIMICS THE PERMISSIONS TO BE PASSED AS A DATATABLE
			INSERT INTO @DEMOCLAIMS VALUES ( 'ReadOnly', 'AutoBuild');
			INSERT INTO @DEMOCLAIMS VALUES ( 'Delete', 'Self');
			INSERT INTO @DEMOCLAIMS VALUES ( 'Update', 'Self');
			INSERT INTO @DEMOCLAIMS VALUES ( 'Edit', 'Self');
			INSERT INTO @DEMOCLAIMS VALUES ( 'Create', 'Reviews');
			INSERT INTO @DEMOCLAIMS VALUES ( 'Delete', 'SelfReviews');
			INSERT INTO @DEMOCLAIMS VALUES ( 'Update', 'SelfReviews');
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
			DECLARE @UserHashID BINARY(32)
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
				INSERT INTO @DEMOCLAIMS VALUES ( 'Block', 'None');
				INSERT INTO @DEMOCLAIMS VALUES ( 'ReadOnly', 'AutoBuild');
				INSERT INTO @DEMOCLAIMS VALUES ( 'Delete', 'Self');
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
 

DROP PROCEDURE IF EXISTS GetAllProductsByVendor2;

 DROP TYPE IF EXISTS Filters;
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
            DECLARE  @USERHASHID  BINARY(32)
            SET @USERHASHID  = db.dbo.GetUserHashIDbyUsername(@USERNAME)
            select vp.listingName, p.ProductType, vp.vendorImageURL, vp.productStatus, v.vendorName, vp.VendorLinkURL, p.modelNumber, vp.productPrice 
            from (((vendor_product_junction as vp inner join products as p on vp.productID = p.productID) inner join vendorclub as v on vp.vendorID = v.vendorID) 
                    inner join Vendor_UserAccount_Junction vuj on vuj.vendorID = v.vendorID)
                    where vuj.userHashID = @USERHASHID
                    and exists (select * from @FilterList fl where p.productType = fl.productType and fl.filterOn = 1)
                    order by 
                    case when @order = 'price_asc' then
                    vp.productPrice end asc,
                    case when @order = 'price_desc' then
                    vp.productPrice end desc,
                    case when @order = 'name_asc' then
                    p.productName end asc,
                    case when @order = 'name_desc' then
                    p.productName end desc
        END

        GO

-- END OF PROCEDURE GetAllProductsByVendor2  ---


-- TESTING PRODUCT PROCEDURES
        GO
        DECLARE @FilterList AS Filters

        DECLARE @DEMO AS Filters
            INSERT INTO @DEMO VALUES ('motherboard', 1);
            INSERT INTO @DEMO VALUES ('case', 1);
            INSERT INTO @DEMO VALUES ('CPU', 1);
            exec GetAllProductsByVendor2 @username = 'newegg123', @filterlist = @DEMO, @order = 'price_asc'
        GO
-- END OF TESTING PRODUCT PREOCEDURES 


		
-- END OF -- GetAllProductsByVendor2




-- NICKS PRODUCTS DEMO:

	select * from Products;
	select * from Vendor_Product_Junction;

	

drop procedure IF EXISTS Search_ProductBudget;

DROP TYPE IF EXISTS  BuilderType;
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

DROP PROCEDURE IF EXISTS INSERTLOGS;
 -- TYPES AS TABLES:
 DROP TYPE IF EXISTS LOGSTYPE;

 -- THE LOGS TYPE USED BY INSERT LOGS PROCEDURE
		GO
			CREATE TYPE LOGSTYPE AS TABLE(
		message text, -- 
		loglevel Varchar(20),
		dateTime Varchar(50));
		GO

		GO
			

------GO
------CREATE PROCEDURE INSERTLOGS
------(
------@USERNAME varchar(30) = 'N/A' , -- USERNAME SET IN THE THREAD BUT IS OPTIONAL IN CASE OF GUEST USER.
------@LOGS LOGSTYPE READONLY,
------@SESSIONSID BIGINT, -- THIS WILL BE GENERATED IN THE C# CODE 
------@PAGEID INT = 0, -- THIS WILL COME FROM THE ENUMERATIONS ON THE CONTROLLERS.
------@BEGINTIME DATETIME)
------AS 
------BEGIN 
-------- STEP (1) IS THIS SESSION LINKED TO A USER ACCOUNT?
------	DECLARE  @USERID varchar(30)
------			SET @USERID  = db.dbo.GetUserIDbyUsername(@USERNAME)
------	DECLARE @NUMBEROFROWS INTEGER 
------			SET @NUMBEROFROWS  = db.dbo.IsSessionExist(@SESSIONSID)
------		-- THE SESSIONSID EXISTS SO THIS MEANS MORE LOGS UNDER THE SOME SESSION
------		-- SO IT IS ENOUGH TO INSERT INTO THE LOG TABLE AND 
------		-- REFERENCE THE PK
------		IF @NUMBEROFROWS = 1
------			BEGIN
------			INSERT INTO Logs(visitorID, message, loglevel, dateTime)
------			SELECT (@SESSIONSID),L.message, L.loglevel, L.dateTime FROM @LOGS L;
------			END
------		ELSE
------			BEGIN
------				IF @USERID IS NOT NULL
------					BEGIN 
------						-- THIS MEANS THAT THERE IS A USER CURRENTLY ON
------						-- THE THREAD AND WE WILL ADD A SESSIONS ROW TO 
------						-- REFERENCE THEIR ACCOUNT 
						
------						 SET IDENTITY_INSERT UserSessions ON
------						INSERT INTO UserSessions (visitorID, pageID, userID, beginAt)
------						VALUES (@SESSIONSID, @PAGEID, @USERID, @BEGINTIME);
------						SET IDENTITY_INSERT UserSessions OFF
------						-- NOW THAT THE SESSIONS ROW IS CREATED FOR THE USER
------						-- WE WILL ADD THE LOG UNDER THAT SESSION:
------						INSERT INTO Logs(visitorID, message, loglevel, dateTime)
------						SELECT @SESSIONSID,L.message, L.loglevel, L.dateTime FROM @LOGS L;
------					END
------				ELSE --(JUST INSERT INTO USER SESSIONS WITH USERID -> NULL)
------					BEGIN
------						-- THIS IS FOR GUEST SO JUST ADD SESSION AND LOG
						
------						SET IDENTITY_INSERT UserSessions ON
------						INSERT INTO UserSessions (visitorID, pageID, beginAt)
------						VALUES (@SESSIONSID, @PAGEID, @BEGINTIME)
------						SET IDENTITY_INSERT UserSessions OFF
------						-- NOW THAT THE SESSIONS ROW IS CREATED FOR THE USER
------						-- WE WILL ADD THE LOG UNDER THAT SESSION:
------						INSERT INTO Logs(visitorID, message, loglevel, dateTime)
------						SELECT @SESSIONSID,L.message, L.loglevel, L.dateTime FROM @LOGS L
------					END
------			END
------END
------GO
-------- TESTING THE INSERT LOGS PROCEDURE
------		GO
------				DECLARE @lOG AS  LOGSTYPE
------				INSERT INTO @lOG VALUES ('THIS IS A TEST1', '0', '03/21/2021');
------				--INSERT INTO @lOG VALUES ('THIS IS A TEST2', '0', '03/22/2021');
------				--INSERT INTO @lOG VALUES ('THIS IS A TEST3', '0', '03/23/2021'); 

------				EXECUTE INSERTLOGS  @LOGS =  @LOG, @SESSIONSID = 26000, @BEGINTIME = '03/21/2021', @USERNAME = 'hoseho'; -- 'hoseho' has userid 16
------		GO

		--
-- END OF TESTING THE INSERT LOGS PROCEDURE




select * from Logs;



DROP VIEW IF EXISTS PageViewsPerMonth;

DROP VIEW IF EXISTS RegPerMonthByType;

DROP VIEW IF EXISTS AvgSessPerAccntType;

DROP VIEW IF EXISTS VisitsPerFeature;

DROP VIEW IF EXISTS NumberOfAccountTypes;
-- UAD GRAPH VIEWS:

-- Bar graph 1: The number of accounts (Y Axis) held
-- amongst account types (Label : Vendor, Admin,
-- Basic , Devoloper) *no legends used

-- VERSION 2
	GO
	CREATE VIEW NumberOfAccountTypes AS
		select 
		'Number Of Accounts' AS YTitle,
		'Account Types' AS XTitle,
		'' AS LegendTitle,
		uc.userRole AS 'X_Value', 
		count(distinct uc.userHashID)AS 'Y_Value', 
		'N/A' AS 'Legend'
		from UserCredentials uc group by uc.userRole;

		
	GO

	SELECT * FROM NumberOfAccountTypes;
-- BAR GRAPH 2 : Percentage usage of Autobuild components 
-- Y-Axis: usage can be time, or page visits maybe, X-Axis:
-- is the Autobuild component
-- VERSION 2
GO 

CREATE VIEW VisitsPerFeature AS
	SELECT
		'Number Of Visitations' AS YTitle,
		'AutoBuild Component' AS XTitle,
		'' AS LegendTitle,
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
GO

-- Line chart 1: Number of registrations (Y Axis) that
-- took place every month (x - axis) and the label show
-- the type of user
GO

CREATE VIEW RegPerMonthByType AS
	-- VERSION 2	
	SELECT
		'Number Of Registrations' AS YTitle,
		'Time: Month' AS XTitle,
		'AccountType' AS LegendTitle, 

	MONTH(UA.createdAt) AS 'X_Value', 
	count(distinct ua.userID) AS 'Y_Value', 
	UC.userRole AS 'Legend' 
	FROM UserCredentials UC  right join MappingHash MH ON UC.userHashID = MH.userHashID
	inner join UserAccounts UA on UA.userID = MH.userID
	group by  UC.userRole ,MONTH(UA.createdAt);

--SELECT MONTH(UC.createdAt) AS 'X_Value', count(distinct UC.userHashID) AS 'Y_Value', UC.userRole AS 'Legend' 
--U.nameOfpermission AS 'Legend'
--FROM UserCredentials UC  right join MappingHash MH ON UC.userHashID = MH.userHashID
--group by  UC.userRole ,MONTH(UC.createdAt);
GO

GO

-- Bar Graph 3: Average Session duration(yaxis) of user (shown as labels) *no legends used
-- VERSION 2
CREATE VIEW AvgSessPerAccntType AS
	SELECT
		'Session Duration (Time: Hour)' AS YTitle,
		'AccountType' AS XTitle,
		'' AS LegendTitle, 
	graph3.userRole AS 'X_Value' , SUM(graph3.TimeSpent) AS  'Y_Value', 'N/A' AS 'Legend'
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
GO

GO

-- LINE CHART 3:
-- Most frequently viewed Autobuild view ( Number of views as Y-axis) per
-- month (X-axis) . and legends are the Autobuild views

-- VERSION 2
CREATE VIEW PageViewsPerMonth AS
	SELECT
		'Number Of Views' AS YTitle,
		'Time: Month' AS XTitle,
		'AutoBuild Component' AS LegendTitle, 
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
GO


--

DROP PROCEDURE IF EXISTS getProducts;
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
		
		GO
			select * from Products p;
			select * from Products_Specs;


		select p.productID, count( ps.productID)
		from products p inner join Products_Specs ps on p.productID = ps.productID
		group by p.productID

		GO


------------------------------------------ Insert Like Procedure -------------------------------------------------

DROP PROCEDURE IF EXISTS InsertLikes; 
--- C3) UpdateUserPermissions3 PROCEDURE (FOR CONNOR)
-- THIS ACKNOWLEDGES THE UPDATE FOR THE ROLE AS WELL IN USERCREDENTIALS TABLE
		GO
		CREATE PROCEDURE  InsertLikes
			(-- WHAT IS IT THAT i WANT TO PASS?
			 @POSTID  VARCHAR(100),
			 @USERNAME varchar(30) = null)
			AS 
		BEGIN 
			DECLARE @UserHashID BINARY(32)
			SET @UserHashID  = DB.DBO.GetUserHashIDbyUsername(@USERNAME)
			-- INSERT INTO Likes_Association(userHashID, postID)
			-- VALUES (@UserHashID, @POSTID);
			
			INSERT INTO Likes_Association (userHashID, postID)
			SELECT @UserHashID, @POSTID
			WHERE NOT EXISTS ( SELECT 1 FROM Likes_Association LA WHERE LA.postID = @POSTID);

		END
		BEGIN
		update Mostpopularbuilds set LikeIncrementor = (SELECT COUNT(*)
		FROM Likes_Association LA WHERE LA.postID = @POSTID) 
		WHERE Mostpopularbuilds.postID = @POSTID;

		END

		-- testing the insert likes:
		GO

		DELETE FROM Likes_Association WHERE  Likes_Association.postID = 30018;

		GO
		select * from UserAccounts;
		select * from UserCredentials;

		EXECUTE INSERTLIKES  @POSTID = 30018 , @USERNAME = 'SERGE';

		EXECUTE INSERTLIKES  @POSTID = 30018 , @USERNAME = 'KoolTrini'; -- 'hoseho' has userid 16
				
				SELECT * FROM Likes_Association;
		GO

	
	
	
	---------------------------------------- Insert Review Procedure ------------------------------------------------- 

		DROP PROCEDURE IF EXISTS InsertReview;
	
	--- C3) UpdateUserPermissions3 PROCEDURE (FOR CONNOR)
	-- THIS ACKNOWLEDGES THE UPDATE FOR THE ROLE AS WELL IN USERCREDENTIALS TABLE

select * from Mostpopularbuilds;

select * from Reviews;

		GO
			CREATE PROCEDURE   InsertReview
			(-- WHAT IS IT THAT i WANT TO PASS?
			 @POSTID BIGINT = NULL,
			 @MESSAGE TEXT,
			 @STARRATING INT,
			 @USERNAME varchar(50) = NULL,
			 @FILEPATH  VARCHAR(MAX) = 'N/A',
			 @DATETIME VARCHAR(MAX))
			AS 
		BEGIN 
			DECLARE @UserHashID BINARY(32)
			SET @UserHashID  = DB.DBO.GetUserHashIDbyUsername(@USERNAME)
			INSERT INTO reviews(userHashID , message, star,  ReviewImagePath, datetime, postID)
			VALUES (@UserHashID, @MESSAGE, @STARRATING, @FILEPATH, @DATETIME, @POSTID);
		END
		GO

		
		GO
				EXECUTE InsertReview 
				@postID = 30012,
				@MESSAGE = 'THIS IS TEST',
				@STARRATING = '8',
				@USERNAME = 'SERGE',
				@DATETIME = '03/21/2021'; -- 'hoseho' has userid 16

				
				EXECUTE InsertReview 
				@MESSAGE = 'THIS IS TEST',
				@STARRATING = '6',
				@DATETIME = '03/30/2010'; -- 'hoseho' has userid 16

				SELECT * FROM Reviews;
		GO





--------------------------------------------------------------------------------------------------------

-------------------------------- Most Popular Builds View -------------------------------------------------- 

	
	
DROP VIEW IF EXISTS MostPopularBuildsView

GO
		CREATE VIEW MostPopularBuildsView AS
			SELECT 
			MB.postID, 
			MB.Title, 
			MB.BuildImagePath, 
			MB.LikeIncrementor,
			MB.BuildTypeValue,
			MB.Description,
			MB.datetime,
			CASE 
			 WHEN (SELECT UC.username FROM UserCredentials UC WHERE UC.userHashID = MB.userHashID) IS NULL THEN 'N/A'
			 ELSE (SELECT UC.username FROM UserCredentials UC WHERE UC.userHashID = MB.userHashID)
			 END AS 'USERNAME' 
			 FROM 
			Mostpopularbuilds MB;
GO

			SELECT * FROM MostPopularBuildsView WHERE BuildTypeValue = 1 ORDER BY LikeIncrementor ASC;


			SELECT * from MostPopularBuildsView WHERE postID = 30000;
			

--------------------------------------------------------------------------------------------------------

----------------------------------- Create Build Procedure --------------------------------------------- 

	

	
DROP PROCEDURE IF EXISTS  CreateMostPopularBuild 
GO
CREATE PROCEDURE CreateMostPopularBuild 
(
@Username varchar(50) = NULL, -- SEE TABLES CHANGE TO is null
@Title VARCHAR(50), 
@Description VARCHAR(50),
@LikeIncrementor INT,
@BuildTypeValue INT, 
@BuildImagePath VARCHAR(MAX),
@DateTime VARCHAR(50))
AS 
BEGIN 

	--SELECT TOP 1 MP.datetime FROM Mostpopularbuilds MP   ORDER BY MP.datetime DESC;

	   BEGIN
	   DECLARE @NUMBEROFROWS  INT
	   SET @NUMBEROFROWS  =  (
	   SELECT COUNT(*) FROM Mostpopularbuilds MP  WHERE 
	   DAY(MP.datetime) = DAY(CAST( GETDATE() AS VARCHAR(50))) AND
	   MONTH(MP.datetime) = MONTH(CAST( GETDATE() AS VARCHAR(50)))  AND 
	   YEAR(MP.datetime) = YEAR(CAST( GETDATE() AS DATETIME)) AND
	   MP.userHashID = DB.DBO.GetUserHashIDbyUsername(@Username)
	   )

	   END 

	   IF(@NUMBEROFROWS = 3)
	   BEGIN 
	   PRINT 0;
	   END
	  ELSE 
	   BEGIN 
	   
	-- STEP ONE GET THE HASHID FOR THE USER TO EXTRACT LINK THE MPB TO THE USERACCOUNT
	
			DECLARE @UserHashID BINARY(32)
			SET @UserHashID  = DB.DBO.GetUserHashIDbyUsername(@USERNAME);

	-- STEP TWO 
		INSERT INTO Mostpopularbuilds (userHashID, Title, Description, LikeIncrementor,
		BuildTypeValue, BuildImagePath, datetime)
		VALUES 
		(@UserHashID, @Title, @Description, @LikeIncrementor, @BuildTypeValue,
		@BuildImagePath, @DateTime);

		PRINT 1;
	   END 
END 
-- TESTING IT:
GO

go
	EXEC CreateMostPopularBuild 
	@Username  = 'clo5udy', -- SEE TABLES CHANGE TO is null
	@Title = 'POSTBUILDx', 
	@Description = 'desc',
	@LikeIncrementor = 501,
	@BuildTypeValue = 1, 
	@BuildImagePath = 'C:/Test/Directory' ,
	@DateTime =  '2021-05-04';
go

	
	-------------------------------------------------------------------------------------------------------
	------------------------------------------ View for Reviews Table--------------------------------------

	DROP VIEW IF EXISTS ReviewsView
	go
	CREATE VIEW ReviewsView  AS
	SELECT 
	RV.entityId,
	RV.postID,
	RV.message,
	RV.ReviewImagePath,
	RV.star,
	RV.datetime,
	DBO.GetUsernameByHashID(RV.userHashID) as Username 
	FROM Reviews RV
	go

	select * from ReviewsView;

	
	-------------------------------------------------------------------------------------------------------


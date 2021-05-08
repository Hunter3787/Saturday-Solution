 
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
--------------------------------------UPDATE USER NAME , EMAIL AND PASSSOWRD HERE-----------------------------------------------------------

----------------------------------------------------------------------------------------------

	DROP PROCEDURE IF EXISTS UpdatePassword;
	GO
	CREATE PROCEDURE UpdatePassword(
	 @USEREMAIL VARCHAR(30),
	 @PASSHASH CHAR(60),
	 @MODIFYDATE DATETIME
	)
	 AS 
	BEGIN
		DECLARE  @UserID varchar(30)
		SET @UserID  = dbo.GetUserIDByEmail(@USEREMAIL)

		UPDATE UserCredentials
		SET UserCredentials.passwordHash = @PASSHASH, UserCredentials.modifiedAt = @MODIFYDATE
		FROM UserCredentials
		INNER JOIN MappingHash
		ON MappingHash.userHashID = UserCredentials.userHashID
		INNER JOIN UserAccounts
		ON UserAccounts.userID = MappingHash.userID
			WHERE UserAccounts.userID = @userID;

		UPDATE UserAccounts
		SET UserAccounts.modifiedAt = @MODIFYDATE
		FROM UserAccounts
			WHERE UserAccounts.userID = @userID;
	 END 

	 GO

	 DROP PROCEDURE IF EXISTS UpdateEmail;

	 GO
	CREATE PROCEDURE UpdateEmail(
	 @ACTIVEEMAIL VARCHAR(30),
	 @INPUTEMAIL VARCHAR(30),
	 @MODIFYDATE DATETIME
	)
	 AS 
	BEGIN
		DECLARE  @UserID varchar(30)
		SET @UserID  = dbo.GetUserIDByEmail(@ACTIVEEMAIL)
		UPDATE UserAccounts
		SET UserAccounts.email = @INPUTEMAIL, UserAccounts.modifiedAt = @MODIFYDATE
		FROM UserAccounts
			WHERE UserAccounts.userID = @userID 

		UPDATE UserCredentials
		SET UserCredentials.modifiedAt = @MODIFYDATE
		FROM UserCredentials
		INNER JOIN MappingHash
		ON MappingHash.userHashID = UserCredentials.userHashID
		INNER JOIN UserAccounts
		ON UserAccounts.userID = MappingHash.userID
			WHERE UserAccounts.userID = @userID;
	 END 
	 GO

	 DROP PROCEDURE IF EXISTS UpdateUsername;


	 GO
	CREATE PROCEDURE UpdateUsername(
	 @USEREMAIL VARCHAR(30),
	 @USERNAME VARCHAR(20),
	 @MODIFYDATE DATETIME
	)
	 AS 
	BEGIN
		DECLARE  @UserID varchar(30)
		SET @UserID  = dbo.GetUserIDByEmail(@USEREMAIL)
		UPDATE UserCredentials
		SET UserCredentials.username = @USERNAME, UserCredentials.modifiedAt = @MODIFYDATE
		FROM UserCredentials
		INNER JOIN MappingHash
		ON MappingHash.userHashID = UserCredentials.userHashID
		INNER JOIN UserAccounts
		ON UserAccounts.userID = MappingHash.userID
			WHERE UserAccounts.userID = @userID

		UPDATE UserAccounts
		SET UserAccounts.modifiedAt = @MODIFYDATE
		FROM UserAccounts
			WHERE UserAccounts.userID = @userID 
	 END 
	 GO

 

----------------------------------------------------------------------------------------------
----------------------------------------------------------------------------------------------

----------------------------------------------------------------------------------------------

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
 SELECT cred.username, perm.permission, perm.scopeOfPermission, cred.locked, cred.emailConfirmed
	FROM DB.DBO.UserCredentials cred 
	INNER JOIN DB.DBO.UserPermissions perm on cred.userHashID = perm.userHashID 
	where cred.username = @USERNAME AND cred.passwordHash = @PASSHASH
 END 
 GO
 -- END OF PROCEDURE

 -- TESTING THE RetrievePermissions PROCUDURE
 EXEC RetrievePermissions @USERNAME= 'new egg', @PASSHASH = 'PassHash';

 select * from UserCredentials


 
DROP PROCEDURE IF EXISTS RetrievePermissionsLogin;

		GO
			CREATE PROCEDURE RetrievePermissionsLogin(
 @USERNAME VARCHAR(30))
 AS 
 BEGIN 
 SELECT cred.username, perm.permission, perm.scopeOfPermission, cred.locked, cred.passwordHash
	FROM DB.DBO.UserCredentials cred 
	INNER JOIN DB.DBO.UserPermissions perm on cred.userHashID = perm.userHashID 
	where cred.username = @USERNAME
 END 
 GO
 -- END OF PROCEDURE

 -- TESTING THE RetrievePermissions PROCUDURE
 EXEC RetrievePermissionsLogin @USERNAME= 'crkobel';



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
			DECLARE @tmp DATETIMEOFFSET
			SET @tmp = GETDATE()
			EXEC DeleteAccount @USERNAME = 'joejamey', @DATEDELETE = @tmp;--'2020-05-12 11:30:30.12567' ;

			GO

--- LETS SEE RESULTS:
			GO

			SELECT * FROM UserAccounts;
			SELECT * FROM UserCredentials;
			SELECT * FROM MappingHash;
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
	DB.DBO.GetUserIDbyUsername('NEW_USERNAME1');

	SELECT * FROM UserCredentials WHERE userHashID =  
	DB.DBO.GetUserHashIDbyUsername('NEW_USERNAME1');

	SELECT * FROM UserCredentials;
	-- need to delete the previous created useername
	DECLARE @tmp DATETIMEOFFSET
	SET @tmp = GETDATE()
	EXEC DeleteAccount @USERNAME = 'NEW_USERNAME1', @DATEDELETE = @tmp; -- '2020-04-12 11:30:30.12567';

-- REGISTRATION PROCEDURE PART 2 WITH HASH

DROP PROCEDURE IF EXISTS RegisterAccount; 
		GO
			CREATE PROCEDURE RegisterAccount
			(@LOCKED BIT = 0, -- Default to unlocked
			 @PERMISSIONS CLAIMSTYPE READONLY,
			 @EMAILCONFIRMED BIT = 0, -- Default to unlocked
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
			 INSERT INTO userCredentials (userHashID, username, passwordHash, userRole, locked, emailConfirmed)
			 VALUES (@HASHBYTES, @USERNAME, @PASSWORD, @ROLES, @LOCKED, @EMAILCONFIRMED);

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
			--@ROLES = 'Basic',
			@PASSWORD = 'HASHED+PASSWORD2',
			@EMAIL = 'NEW_EMAIL1@GMAIL.COM' ,
			@FNAME = 'FIRSTNAME' ,
			@LNAME = 'LASTNAME' ,
			@CREATEDATE = '04-12-2021'
		
		GO 
		SELECT * FROM MappingHash
		select * from UserAccounts UA inner join MappingHash MH on UA.userID = MH.userID;
		SELECT * FROM UserPermissions;
		SELECT * FROM UserCredentials;

-- END OF REGISTRATION PROCEDURE PART TWO







--- END OF  UpdateUserPermissions2 PROCEDURE !!  -------


DROP PROCEDURE IF EXISTS UpdateuserPermissions;
--- C3) UpdateUserPermissions3 PROCEDURE (FOR CONNOR)
-- THIS ACKNOWLEDGES THE UPDATE FOR THE ROLE AS WELL IN USERCREDENTIALS TABLE
		GO

			CREATE PROCEDURE UpdateUserPermissions
			(-- WHAT IS IT THAT i WANT TO PASS?
			 @PERMISSIONS CLAIMSTYPE READONLY,
			 @USERNAME varchar(30),
			 @USERROLE varchar(20),
			 @MODIFYDATE DATETIME)
			AS 
		BEGIN 
			DECLARE @UserHashID BINARY(32)
			SET @UserHashID  = DB.DBO.GetUserHashIDbyUsername(@USERNAME)
			DELETE FROM userPermissions where userPermissions.userHashID = @UserHashID;

			UPDATE UserCredentials SET UserCredentials.userRole = @USERROLE, UserCredentials.modifiedAt = @MODIFYDATE where userHashID = @UserHashID;
			UPDATE UserAccounts SET UserAccounts.modifiedAt = @MODIFYDATE FROM UserAccounts
			INNER JOIN MappingHash ON MappingHash.userID = UserAccounts.userID
			WHERE MappingHash.userHashID = @UserHashID;
			-- after deleting those permissions do and insert of the new set of permissions 
			INSERT INTO userPermissions (userHashID, permission, scopeOfPermission)
			SELECT (@UserHashID), p.permission, p.scopeOfPermission FROM @PERMISSIONS p;
		END

		GO


-- END OF PROEDURE FOR UPDATING OF THE PERMISSIONS ----- 
--- TESTING  UpdateUserPermissions PROCEDURE!!  -------
		GO
		DECLARE @tmp DATETIMEOFFSET
		SET @tmp = GETDATE()
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
				 @MODIFYDATE =@tmp,
				 @USERROLE = 'NEW ROLE!!';
		GO

		select * from UserPermissions UP inner join UserCredentials UC
				on UP.userHashID = UC.userHashID;


--- END OF  UpdateUserPermissions2 PROCEDURE !!  -------


-- D) GetAllProductsByVendorAccount    FILTERING PRODUCTS STUFF (PER DANNY)


 -- TYPES AS TABLES: this is modeling the incoming parameter for the procedure GetAllProductsByVendor2
 

DROP PROCEDURE IF EXISTS GetAllProductsByVendorAccount;

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
         create procedure GetAllProductsByVendorAccount(
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

-- END OF PROCEDURE GetAllProductsByVendorAccount    ---


-- TESTING PRODUCT PROCEDURES
        GO
        DECLARE @FilterList AS Filters

        DECLARE @DEMO AS Filters
            INSERT INTO @DEMO VALUES ('motherboard', 1);
            INSERT INTO @DEMO VALUES ('case', 1);
            INSERT INTO @DEMO VALUES ('CPU', 1);
            exec GetAllProductsByVendorAccount   @username = 'newegg123', @filterlist = @DEMO, @order = 'price_asc'
        GO
-- END OF TESTING PRODUCT PREOCEDURES 


		
-- END OF -- GetAllProductsByVendorAccount  




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

GO
CREATE PROCEDURE INSERTLOGS
(
@USERNAME varchar(30)   = 'N/A' , -- USERNAME SET IN THE THREAD BUT IS OPTIONAL IN CASE OF GUEST USER.
@EVENTTYPE  Varchar(20) = 'N/A' , -- this can be like logging out or logging in etc.
@EVENTVALUE Varchar(20) = 'N/A', -- this can be like logging out or logging in etc.
@MESSAGE varchar(max), -- 
@LOGLEVEL Varchar(20),
@DATETIME DATETIME )
AS 
BEGIN 
-- STEP (1) IS THIS SESSION LINKED TO A USER ACCOUNT?
	DECLARE  @USERHASHID BINARY(32)
			SET @USERHASHID  = db.dbo.GetUserHashIDbyUsername(@USERNAME)
		-- THE SESSIONSID EXISTS SO THIS MEANS MORE LOGS UNDER THE SOME SESSION
		-- SO IT IS ENOUGH TO INSERT INTO THE LOG TABLE AND 
		-- REFERENCE THE PK
			BEGIN
				IF @USERHASHID IS NOT NULL
					BEGIN 
					INSERT INTO Logs
						(message, loglevel, eventType, eventValue, creationDate, userHashID)
						VALUES (@MESSAGE,@LOGLEVEL, @EVENTTYPE, @EVENTVALUE, @DATETIME, @USERHASHID)
						
					END
				ELSE --(JUST INSERT INTO USER SESSIONS WITH USERID -> NULL)
					BEGIN
						-- if it is a guest user then store as an stand alone log
						INSERT INTO Logs
						(message, loglevel, eventType, eventValue, creationDate, userHashID)
						VALUES (@MESSAGE,@LOGLEVEL, @EVENTTYPE, @EVENTVALUE, @DATETIME, null)
					END
			END
END
GO

-- TESTING THE INSERT LOGS PROCEDURE
		GO
		DECLARE @tmp DATETIMEOFFSET
		SET @tmp = GETDATE()
			EXEC INSERTLOGS @USERNAME = 'Zeina', @MESSAGE = 'THIS IS A TEST', @LOGLEVEL = 'INFORMATION', @DATETIME = @tmp;
		GO

		select * from UserAccounts ua inner join MappingHash mp on mp.userID = ua.userID
		where mp.userHashID = db.dbo.GetUserHashIDbyUsername('Zeina');
 --END OF TESTING THE INSERT LOGS PROCEDURE

 
	SELECT * FROM Logs where userHashID = CONVERT(BINARY(32),'USERID_2');
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
		 WHEN (L.eventValue IS NULL) THEN 'N/A'
		 ELSE L.eventValue 
	END
	AS 'X_Value'  ,
	COUNT(L.userHashID) AS 'Y_Value', 
	'N/A' AS 'Legend'
	FROM Logs L 
	where L.eventType = 'VIEW_PAGE_EVENT' or   L.eventType = 'ViewPageEvent'
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
	CASE 
     WHEN (UC.userRole IS NULL) THEN 'N/A'
     ELSE UC.userRole
	 END
	 AS 'Legend'
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
	WHERE Logins.eventType ='LOGIN_EVENT' or  Logins.eventType = 'LoginEvent'
	  and Logout.eventType ='LOGOUT_EVENT' or Logout.eventType ='LogoutEvent'
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

delete from Mostpopularbuilds where Mostpopularbuilds.Title = 'POSTBUILDx';


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

	-- updating the shelf order per changes
	
	DROP  PROCEDURE  IF EXISTS UpdateShelfNumber
	DROP TYPE IF EXISTS ItemPerIndex ;
 -- THE CLAIMS TYPE USED BY RegisterAccount PROCEDURE
		GO
		CREATE TYPE ItemPerIndex AS TABLE
		(  oldIndex INT,
		   newIndex INT);
		GO

	GO
	CREATE PROCEDURE UpdateShelfNumber(
	@Item ItemPerIndex READONLY,
	@USERNAME VARCHAR(50) )
	AS
	BEGIN
	declare @temp INT
	SET @temp = -1;
	UPDATE Save_Product_Shelf
	SET itemIndex = (SELECT IT.newIndex FROM @Item IT where itemIndex = IT.oldIndex)
	WHERE shelfID = 
	(SELECT S.shelfID FROM Shelves S WHERE S.userID = DB.DBO.GetUserIDbyUsername(@USERNAME))
	END
	GO

	-- 
	-- testing this: 
		GO
				DECLARE @INDEXES AS  ItemPerIndex
				INSERT INTO @INDEXES  VALUES ( 2, 1);
				INSERT INTO @INDEXES  VALUES ( 1, 2);
				INSERT INTO @INDEXES  VALUES ( 5, 3);
				INSERT INTO @INDEXES  VALUES ( 4, 4);
				INSERT INTO @INDEXES  VALUES ( 3, 5);
				EXECUTE UpdateShelfNumber  @ITEM =  @INDEXES, @USERNAME = 'Zeina';
		GO
		
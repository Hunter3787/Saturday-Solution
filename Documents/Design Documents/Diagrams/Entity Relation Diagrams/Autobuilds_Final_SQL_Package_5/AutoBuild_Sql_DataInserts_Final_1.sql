-- THESE ARE THE DATA INSERTS OF AUTOBUILD TO POPULATE THE DATA IN ORDER TO TEST .
---- INSERTS:
USE DB;
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
 

 INSERT INTO MappingHash (userID,userHashID) VALUES 
 (2 ,CONVERT(Binary(64),'USERID_2')),
 (4 ,CONVERT(Binary(64),'USERID_4')),
 (6 ,CONVERT(Binary(64),'USERID_6')),
 (8 ,CONVERT(Binary(64),'USERID_8')),
 (10,CONVERT(Binary(64),'USERID_10')),
 (12,CONVERT(Binary(64),'USERID_12')),
 (14,CONVERT(Binary(64),'USERID_14')),
 (16,CONVERT(Binary(64),'USERID_16')),
 (18,CONVERT(Binary(64),'USERID_18')),
 (20,CONVERT(Binary(64),'USERID_20')),
 (22,CONVERT(Binary(64),'USERID_22')),
 (24,CONVERT(Binary(64),'USERID_24')),
 (26,CONVERT(Binary(64),'USERID_26')),
 (28,CONVERT(Binary(64),'USERID_28')),
 (30,CONVERT(Binary(64),'USERID_30')),
 (32,CONVERT(Binary(64),'USERID_32')),
 (34,CONVERT(Binary(64),'USERID_34')),
 (36,CONVERT(Binary(64),'USERID_36'));


INSERT INTO userCredentials (userHashID, username, passwordHash, modifiedAt, locked, userRole) VALUES 
 (CONVERT(Binary(64),'USERID_2'), 'Zeina', 'PassHash','03/21/2021', '0', 'basic'),
 (CONVERT(Binary(64),'USERID_4'), 'SERGE', 'PassHash','01/18/2021', '0', 'basic'),
 (CONVERT(Binary(64),'USERID_6'), 'KoolTrini', 'PassHash','03/21/2021', '0', 'basic'),
 (CONVERT(Binary(64),'USERID_8'), 'InuRes', 'PassHash','03/21/2021', '0', 'basic'),
 (CONVERT(Binary(64),'USERID_10'), 'goodBoy399', 'PassHash','05/18/2019', '0', 'basic'),
 (CONVERT(Binary(64),'USERID_12'), 'kingPeni393', 'PassHash','02/21/2020', '0', 'ADMIN'),
 (CONVERT(Binary(64),'USERID_14'), 'JessyJayJay', 'PassHash','01/14/2019', '0', 'basic'),
 (CONVERT(Binary(64),'USERID_16'), 'hoseho', 'PassHash', '03/21/2021', '0', 'basic'),
 (CONVERT(Binary(64),'USERID_18'), 'jamesBre343', 'PassHash','03/21/2021', '1', 'basic'),
 (CONVERT(Binary(64),'USERID_20'), 'Fay79oo', 'PassHash','01/18/2019', '0', 'basic'),
 (CONVERT(Binary(64),'USERID_22'), 'clo5udy', 'PassHash','02/18/2018', '0', 'basic'),
 (CONVERT(Binary(64),'USERID_24'), 'juCarter', 'PassHash','02/18/2018', '0', 'basic'),
 (CONVERT(Binary(64),'USERID_26'), 'Shene', 'PassHash','01/10/2010', '0', 'basic'),
 (CONVERT(Binary(64),'USERID_28'), 'butter500', 'PassHash','02/18/2021', '0', 'basic'),
 (CONVERT(Binary(64),'USERID_30'), 'joejamey', 'PassHash','03/04/2021', '0', 'basic'),
 (CONVERT(Binary(64),'USERID_32'), 'pepper', 'PassHash','03/21/2020', '0', 'basic'),
 (CONVERT(Binary(64),'USERID_34'), 'jermia', 'PassHash','03/04/2020', '0', 'basic'),
 (CONVERT(Binary(64),'USERID_36'), 'peneHope', 'PassHash','03/16/2018', '0', 'basic');


  INSERT INTO UserPermissions(userHashID, permission,scopeOfPermission)VALUES
 (CONVERT(Binary(64),'USERID_2'), 'READ_ONLY', 'AUTOBUILD'),
 (CONVERT(Binary(64),'USERID_2'), 'DELETE', 'SELF'),
 (CONVERT(Binary(64),'USERID_2'),'UPDATE', 'SELF'),
 (CONVERT(Binary(64),'USERID_2'), 'EDIT', 'SELF'),
 (CONVERT(Binary(64),'USERID_2'), 'CREATE', 'REVIEWS'),
 (CONVERT(Binary(64),'USERID_2'), 'DELETE', 'SELF_REVIEWS'),
 (CONVERT(Binary(64),'USERID_2'), 'UPDATE', 'SELF_REVIEWS');
   INSERT INTO UserPermissions(userHashID , permission,scopeOfPermission) VALUES
 (CONVERT(Binary(64),'USERID_4'), 'READ_ONLY', 'AUTOBUILD'),
 (CONVERT(Binary(64),'USERID_4'), 'DELETE', 'SELF'),
 (CONVERT(Binary(64),'USERID_4'),'UPDATE', 'SELF'),
 (CONVERT(Binary(64),'USERID_4'), 'EDIT', 'SELF'),
 (CONVERT(Binary(64),'USERID_4'), 'CREATE', 'REVIEWS'),
 (CONVERT(Binary(64),'USERID_4'), 'DELETE', 'SELF_REVIEWS'),
 (CONVERT(Binary(64),'USERID_4'), 'UPDATE', 'SELF_REVIEWS');
  INSERT INTO UserPermissions(userHashID, permission,scopeOfPermission) VALUES
 (CONVERT(Binary(64),'USERID_6'), 'READ_ONLY', 'AUTOBUILD'),
 (CONVERT(Binary(64),'USERID_6'), 'DELETE', 'SELF'),
 (CONVERT(Binary(64),'USERID_6'),'UPDATE', 'SELF');
  INSERT INTO UserPermissions(userHashID, permission,scopeOfPermission) VALUES
 (CONVERT(Binary(64),'USERID_8'), 'READ_ONLY', 'AUTOBUILD'),
 (CONVERT(Binary(64),'USERID_8'), 'DELETE', 'SELF'),
 (CONVERT(Binary(64),'USERID_8'),'UPDATE', 'SELF');
 INSERT INTO UserPermissions(userHashID, permission,scopeOfPermission) VALUES
 (CONVERT(Binary(64),'USERID_10'), 'READ_ONLY', 'AUTOBUILD'),
 (CONVERT(Binary(64),'USERID_10'), 'DELETE', 'SELF'),
 (CONVERT(Binary(64),'USERID_10'),'UPDATE', 'SELF'),
 (CONVERT(Binary(64),'USERID_10'), 'EDIT', 'SELF'),
 (CONVERT(Binary(64),'USERID_10'), 'CREATE', 'REVIEWS'),
 (CONVERT(Binary(64),'USERID_10'), 'DELETE', 'SELF_REVIEWS'),
 (CONVERT(Binary(64),'USERID_10'), 'UPDATE', 'SELF_REVIEWS');

  INSERT INTO UserPermissions(userHashID, permission,scopeOfPermission) VALUES
 (CONVERT(Binary(64),'USERID_12'),  'ALL', 'ALL');



 --
 select MP.userHashID, UA.firstName + Space(1) +UA.lastName AS FullName, UA.email As Email,
  UC.username AS UserName,
 UP.permission +Space(1) + UP.scopeOfPermission AS PermissionsScope
 , uc.modifiedAt from
	UserAccounts UA inner join MappingHash MP on UA.userID = MP.userID
	inner join UserCredentials UC on UC.userHashID = MP.userHashID
	inner join UserPermissions UP on UP.userHashID = UC.userHashID 


	INSERT INTO Logs (visitorID, message, loglevel, event, creationDate)
	VALUES 
	(20000,'COMPLETED TASK  ON PAGE ID 1', 'INFORMATIONAL'),
	(20004,'COMPLETED TASK  ON PAGE ID 2', 'INFORMATIONAL'),
	(20002,'ERROR OCCURED   ON PAGE ID 3', 'WARNING'),
	(20006,'COMPLETED TASK  ON PAGE ID 0', 'INFORMATIONAL'),
	(20008,'COMPLETED TASK  ON PAGE ID 1', 'INFORMATIONAL'),
	(20010,'ERROR OCCURED   ON PAGE ID 2', 'WARNING');

	
	SELECT * FROM UserSessions US
	INNER JOIN UserAccounts UA ON US.userID = UA.userID 
	INNER JOIN Logs L ON US.visitorID = L.visitorID;

	-----------------------------------------------------------



	select * from Products;
	select * from Vendor_Product_Junction;

	SELECT * FROM UserPermissions
	INNER JOIN UserCredentials
	ON UserCredentials.userCredID = UserPermissions.userID
	WHERE UserID = 6;

	SELECT * FROM UserPermissions
	INNER JOIN UserCredentials
	ON UserCredentials.userCredID = UserPermissions.userID
	WHERE UserID = 38;

	SELECT * FROM UserCredentials;

	SELECT * FROM UserAccounts
	INNER JOIN UserCredentials
	ON UserCredentials.userCredID = UserAccounts.userID
	INNER JOIN UserPermissions
	ON UserPermissions.userID = UserCredentials.userCredID;
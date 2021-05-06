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
 
 INSERT INTO userCredentials 
 (userCredID, username, passwordHash, modifiedAt) VALUES 
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


 -------- INSERT INTO userPermissions(userID, permission, scopeOfPermission) VALUES
 --------(2, 'READ_ONLY', 'AUTOBUILD'),
 --------(2, 'DELETE', 'SELF'),
 --------(2, 'UPDATE', 'SELF'),
 --------(2, 'EDIT', 'SELF'),
 --------(2, 'CREATE', 'REVIEWS'),
 --------(2, 'DELETE', 'SELF_REVIEWS'),
 --------(2, 'UPDATE', 'SELF_REVIEWS');
 --------  INSERT INTO userPermissions(userID, permission, scopeOfPermission) VALUES
 --------(4, 'READ_ONLY', 'AUTOBUILD'),
 --------(4, 'DELETE', 'SELF'),
 --------(4, 'UPDATE', 'SELF'),
 --------(4, 'EDIT', 'SELF'),
 --------(4, 'CREATE', 'REVIEWS'),
 --------(4, 'DELETE', 'SELF_REVIEWS'),
 --------(4, 'UPDATE', 'SELF_REVIEWS');
 -------- INSERT INTO userPermissions(userID, permission, scopeOfPermission) VALUES
 --------(6, 'CREATE', 'ANY USER'),
 --------(6, 'DELETE', 'ANY USERS'),
 --------(6, 'UPDATE', 'ANY USERS');



  INSERT INTO UserPermissions(userID, nameOfpermission, permission,scopeOfPermission)VALUES
 (2,'BASIC',  'READ_ONLY', 'AUTOBUILD'),
 (2,'BASIC',  'DELETE', 'SELF'),
 (2, 'BASIC', 'UPDATE', 'SELF'),
 (2,'BASIC',  'EDIT', 'SELF'),
 (2,'BASIC',  'CREATE', 'REVIEWS'),
 (2,'BASIC',  'DELETE', 'SELF_REVIEWS'),
 (2,'BASIC',  'UPDATE', 'SELF_REVIEWS');
   INSERT INTO UserPermissions(userID, nameOfpermission , permission,scopeOfPermission) VALUES
 (4,'BASIC',  'READ_ONLY', 'AUTOBUILD'),
 (4,'BASIC',  'DELETE', 'SELF'),
 (4, 'BASIC', 'UPDATE', 'SELF'),
 (4,'BASIC',  'EDIT', 'SELF'),
 (4,'BASIC',  'CREATE', 'REVIEWS'),
 (4, 'BASIC', 'DELETE', 'SELF_REVIEWS'),
 (4, 'BASIC', 'UPDATE', 'SELF_REVIEWS');
  INSERT INTO UserPermissions(userID, nameOfpermission, permission,scopeOfPermission) VALUES
 (6,'ADMIN',  'CREATE', 'ANY USER'),
 (6,'ADMIN',  'DELETE', 'ANY USERS'),
 (6, 'ADMIN',  'UPDATE', 'ANY USERS');
 
  INSERT INTO UserPermissions(userID, nameOfpermission, permission,scopeOfPermission) VALUES
 (8,'VENDOR',  'CREATE', 'ANY USER'),
 (8,'VENDOR',  'DELETE', 'ANY USERS'),
 (8, 'VENDOR',  'UPDATE', 'ANY USERS');
 INSERT INTO UserPermissions(userID, nameOfpermission, permission,scopeOfPermission) VALUES
 (10,'BASIC',  'READ_ONLY', 'AUTOBUILD'),
 (10,'BASIC',  'DELETE', 'SELF'),
 (10, 'BASIC', 'UPDATE', 'SELF'),
 (10,'BASIC',  'EDIT', 'SELF'),
 (10,'BASIC',  'CREATE', 'REVIEWS'),
 (10, 'BASIC', 'DELETE', 'SELF_REVIEWS'),
 (10, 'BASIC', 'UPDATE', 'SELF_REVIEWS');



 --
 select UA.firstName + Space(1) +UA.lastName AS FullName, UA.email As Email, UC.username AS UserName, UP.permission +Space(1) + UP.scopeOfPermission AS PermissionsScope
 , uc.modifiedAt from
	UserAccounts UA inner join UserCredentials UC on UA.userID = UC.userCredID
	inner join UserPermissions UP on UC.userCredID = UP.userID
	--
	-- need to specify some pageID enums exactly 
	INSERT INTO UserSessions (userID, pageID, beginAt, endAt)
	VALUES
	(2,1,'03/21/2021 11:00 AM','03/21/2021 1:00 PM'),
	(2,3, '03/21/2021 2:00 PM','03/21/2021 3:00 PM'),
	(2,2,'03/21/2021 4:00 PM','03/21/2021 5:00 PM'),
	(4,0,'03/24/2021 3:00 PM','03/24/2021 4:00 PM'),
	(4,1,'03/24/2021 5:00 PM','03/24/2021 6:00 PM'),
	(4,2,'03/24/2021 7:00 PM','03/24/2021 8:00 PM');
	INSERT INTO UserSessions (userID, pageID, beginAt, endAt)
	VALUES
	(6,1,'05/21/2021 10:00 AM','05/21/2021 1:00 PM'),
	(6,3, '05/21/2021 2:00 PM','05/21/2021 3:00 PM'),
	(6,2,'05/21/2021 12:00 PM','05/21/2021 5:00 PM'),
	(8,0,'06/24/2021 3:00 PM','06/24/2021 4:00 PM'),
	(8,1,'07/24/2021 5:00 PM','07/24/2021 6:00 PM'),
	(8,2,'08/24/2021 7:00 PM','08/24/2021 8:00 PM');

	INSERT INTO Logs (visitorID, message, loglevel)
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
use productDB
 
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

 select UA.firstName + Space(1) +UA.lastName AS FullName, UA.email As Email, UC.username AS UserName, UP.permission +Space(1) + UP.scopeOfPermission AS PermissionsScope
 From
	UserAccounts UA inner join UserCredentials UC on UA.userID = UC.userCredID
	inner join UserPermissions UP on UC.userCredID = UP.userID

	-- need to specify some pageID enums exactly 
	INSERT INTO UserSessions (userID, pageID)
	VALUES
	(2,1),
	(2,3),
	(2,2),
	(4,0),
	(4,1),
	(4,2);

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


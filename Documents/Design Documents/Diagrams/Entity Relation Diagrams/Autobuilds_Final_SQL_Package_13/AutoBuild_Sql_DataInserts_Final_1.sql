-- THESE ARE THE DATA INSERTS OF AutoBuild TO POPULATE THE DATA IN ORDER TO TEST .
---- INSERTS:
USE DB;
-- INSERT STATEMENTS
 SET IDENTITY_INSERT userAccounts ON
 INSERT INTO userAccounts (userID, email,firstName, lastName, CreatedAt) VALUES 
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
 (36, 'penelopeHope@gmail.com', 'penelope', 'Hope','03/16/2018'),
 (38, 'nick@gmail.com', 'nick', 'Hope','03/16/2018');

  SET IDENTITY_INSERT userAccounts OFF
 

 INSERT INTO MappingHash (userID,userHashID) VALUES 
 (2 ,CONVERT(BINARY(32),'USERID_2')),
 (4 ,CONVERT(BINARY(32),'USERID_4')),
 (6 ,CONVERT(BINARY(32),'USERID_6')),
 (8 ,CONVERT(BINARY(32),'USERID_8')),
 (10,CONVERT(BINARY(32),'USERID_10')),
 (12,CONVERT(BINARY(32),'USERID_12')),
 (14,CONVERT(BINARY(32),'USERID_14')),
 (16,CONVERT(BINARY(32),'USERID_16')),
 (18,CONVERT(BINARY(32),'USERID_18')),
 (20,CONVERT(BINARY(32),'USERID_20')),
 (22,CONVERT(BINARY(32),'USERID_22')),
 (24,CONVERT(BINARY(32),'USERID_24')),
 (26,CONVERT(BINARY(32),'USERID_26')),
 (28,CONVERT(BINARY(32),'USERID_28')),
 (30,CONVERT(BINARY(32),'USERID_30')),
 (32,CONVERT(BINARY(32),'USERID_32')),
 (34,CONVERT(BINARY(32),'USERID_34')),
 (36,CONVERT(BINARY(32),'USERID_36')),
 (38,CONVERT(BINARY(32),'USERID_38'));

 
INSERT INTO userCredentials (userHashID, username, passwordHash, modifiedAt, locked,  emailConfirmed, userRole) VALUES 
 (CONVERT(BINARY(32),'USERID_2'), 'Zeina', '$2a$11$D2j9T9rPSoHUGyEMEcVIn.FZX6FS7elKOxhivTlrYGb4FZrIWwJHq','03/21/2021', '0', 0, 'BasicRole'),
 (CONVERT(BINARY(32),'USERID_4'), 'SERGE', '$2a$11$D2j9T9rPSoHUGyEMEcVIn.FZX6FS7elKOxhivTlrYGb4FZrIWwJHq','01/18/2021', '0', 0, 'SystemAdmin'),
 (CONVERT(BINARY(32),'USERID_6'), 'KoolTrini', '$2a$11$D2j9T9rPSoHUGyEMEcVIn.FZX6FS7elKOxhivTlrYGb4FZrIWwJHq','03/21/2021', '0', 0, 'BasicRole'),
 (CONVERT(BINARY(32),'USERID_8'), 'InuRes', '$2a$11$D2j9T9rPSoHUGyEMEcVIn.FZX6FS7elKOxhivTlrYGb4FZrIWwJHq','03/21/2021', '0', 0, 'VendorRole'),
 (CONVERT(BINARY(32),'USERID_10'), 'goodBoy399', '$2a$11$D2j9T9rPSoHUGyEMEcVIn.FZX6FS7elKOxhivTlrYGb4FZrIWwJHq','05/18/2019', '0', 0, 'SystemAdmin'),
 (CONVERT(BINARY(32),'USERID_12'), 'kingPeni393', '$2a$11$D2j9T9rPSoHUGyEMEcVIn.FZX6FS7elKOxhivTlrYGb4FZrIWwJHq','02/21/2020', '0', 0, 'SystemAdmin'),
 (CONVERT(BINARY(32),'USERID_14'), 'JessyJayJay', '$2a$11$D2j9T9rPSoHUGyEMEcVIn.FZX6FS7elKOxhivTlrYGb4FZrIWwJHq','01/14/2019', '0', 0, 'BasicRole'),
 (CONVERT(BINARY(32),'USERID_16'), 'hoseho', '$2a$11$D2j9T9rPSoHUGyEMEcVIn.FZX6FS7elKOxhivTlrYGb4FZrIWwJHq', '03/21/2021', '0', 0, 'BasicRole'),
 (CONVERT(BINARY(32),'USERID_18'), 'jamesBre343', '$2a$11$D2j9T9rPSoHUGyEMEcVIn.FZX6FS7elKOxhivTlrYGb4FZrIWwJHq','03/21/2021', '1', 0, 'BasicRole'),
 (CONVERT(BINARY(32),'USERID_20'), 'Fay79oo', '$2a$11$D2j9T9rPSoHUGyEMEcVIn.FZX6FS7elKOxhivTlrYGb4FZrIWwJHq','01/18/2019', '0', 0, 'BasicRole'),
 (CONVERT(BINARY(32),'USERID_22'), 'clo5udy', '$2a$11$D2j9T9rPSoHUGyEMEcVIn.FZX6FS7elKOxhivTlrYGb4FZrIWwJHq','02/18/2018', '0', 0, 'SystemAdmin'),
 (CONVERT(BINARY(32),'USERID_24'), 'juCarter', '$2a$11$D2j9T9rPSoHUGyEMEcVIn.FZX6FS7elKOxhivTlrYGb4FZrIWwJHq','02/18/2018', '0', 0, 'BasicRole'),
 (CONVERT(BINARY(32),'USERID_26'), 'Shene', '$2a$11$D2j9T9rPSoHUGyEMEcVIn.FZX6FS7elKOxhivTlrYGb4FZrIWwJHq','01/10/2010', '0', 0, 'BasicRole'),
 (CONVERT(BINARY(32),'USERID_28'), 'butter500', '$2a$11$D2j9T9rPSoHUGyEMEcVIn.FZX6FS7elKOxhivTlrYGb4FZrIWwJHq','02/18/2021', '0',  0,'BasicRole'),
 (CONVERT(BINARY(32),'USERID_30'), 'joejamey', '$2a$11$D2j9T9rPSoHUGyEMEcVIn.FZX6FS7elKOxhivTlrYGb4FZrIWwJHq','03/04/2021', '0',  0,'BasicRole'),
 (CONVERT(BINARY(32),'USERID_32'), 'pepper', '$2a$11$D2j9T9rPSoHUGyEMEcVIn.FZX6FS7elKOxhivTlrYGb4FZrIWwJHq','03/21/2020', '0',  0,'VendorRole'),
 (CONVERT(BINARY(32),'USERID_34'), 'jermia', '$2a$11$D2j9T9rPSoHUGyEMEcVIn.FZX6FS7elKOxhivTlrYGb4FZrIWwJHq','03/04/2020', '0',  0,'BasicRole'),
 (CONVERT(BINARY(32),'USERID_36'), 'peneHope', '$2a$11$D2j9T9rPSoHUGyEMEcVIn.FZX6FS7elKOxhivTlrYGb4FZrIWwJHq','03/16/2018', '0',  0,'BasicRole'),
 (CONVERT(BINARY(32),'USERID_38'), 'Nick', '$2a$11$D2j9T9rPSoHUGyEMEcVIn.FZX6FS7elKOxhivTlrYGb4FZrIWwJHq','03/16/2018', '0',  0,'BasicRole');




  INSERT INTO UserPermissions(userHashID, permission,scopeOfPermission)VALUES
 (CONVERT(BINARY(32),'USERID_2'), 'ReadOnly', 'AutoBuild'),
 (CONVERT(BINARY(32),'USERID_2'), 'Delete', 'Self'),
 (CONVERT(BINARY(32),'USERID_2'),'Update', 'Self'),
 (CONVERT(BINARY(32),'USERID_2'), 'Edit', 'Self'),
 (CONVERT(BINARY(32),'USERID_2'), 'Create', 'Reviews'),
 (CONVERT(BINARY(32),'USERID_2'), 'Delete', 'SelfReviews'),
 (CONVERT(BINARY(32),'USERID_2'), 'Update', 'SelfReviews');
   INSERT INTO UserPermissions(userHashID , permission,scopeOfPermission) VALUES
 (CONVERT(BINARY(32),'USERID_4'), 'ReadOnly', 'AutoBuild'),
 (CONVERT(BINARY(32),'USERID_4'), 'Delete', 'Self'),
 (CONVERT(BINARY(32),'USERID_4'),'Update', 'Self'),
 (CONVERT(BINARY(32),'USERID_4'), 'Edit', 'Self'),
 (CONVERT(BINARY(32),'USERID_4'), 'Create', 'Reviews'),
 (CONVERT(BINARY(32),'USERID_4'), 'Delete', 'SelfReviews'),
 (CONVERT(BINARY(32),'USERID_4'), 'Update', 'SelfReviews');
  INSERT INTO UserPermissions(userHashID, permission,scopeOfPermission) VALUES
 (CONVERT(BINARY(32),'USERID_6'), 'ReadOnly', 'AutoBuild'),
 (CONVERT(BINARY(32),'USERID_6'), 'Delete', 'Self'),
 (CONVERT(BINARY(32),'USERID_6'),'Update', 'Self');
  INSERT INTO UserPermissions(userHashID, permission,scopeOfPermission) VALUES
 (CONVERT(BINARY(32),'USERID_8'), 'ReadOnly', 'AutoBuild'),
 (CONVERT(BINARY(32),'USERID_8'), 'Delete', 'Self'),
 (CONVERT(BINARY(32),'USERID_8'),'Update', 'Self');
 INSERT INTO UserPermissions(userHashID, permission,scopeOfPermission) VALUES
 (CONVERT(BINARY(32),'USERID_10'), 'ReadOnly', 'AutoBuild'),
 (CONVERT(BINARY(32),'USERID_10'), 'Delete', 'Self'),
 (CONVERT(BINARY(32),'USERID_10'),'Update', 'Self'),
 (CONVERT(BINARY(32),'USERID_10'), 'Edit', 'Self'),
 (CONVERT(BINARY(32),'USERID_10'), 'Create', 'Reviews'),
 (CONVERT(BINARY(32),'USERID_10'), 'Delete', 'SelfReviews'),
 (CONVERT(BINARY(32),'USERID_10'), 'Update', 'SelfReviews');

  INSERT INTO UserPermissions(userHashID, permission,scopeOfPermission) VALUES
 (CONVERT(BINARY(32),'USERID_12'),  'All', 'All');

   INSERT INTO UserPermissions(userHashID, permission,scopeOfPermission)VALUES
 (CONVERT(BINARY(32),'USERID_38'), 'ReadOnly', 'AutoBuild'),
 (CONVERT(BINARY(32),'USERID_38'), 'Delete', 'Self'),
 (CONVERT(BINARY(32),'USERID_38'),'Update', 'Self'),
 (CONVERT(BINARY(32),'USERID_38'), 'Edit', 'Self'),
 (CONVERT(BINARY(32),'USERID_38'), 'Create', 'Reviews'),
 (CONVERT(BINARY(32),'USERID_38'), 'Delete', 'SelfReviews'),
 (CONVERT(BINARY(32),'USERID_38'), 'Update', 'SelfReviews');




 SELECT * FROM UserPermissions;

--LETS FOCUS FOR ONE USER:
	INSERT INTO Logs( userHashID, message, loglevel, creationDate,eventType, eventValue)
	VALUES
	-- LOGS FOR USERID_2 (ROLE: Basic)
	( CONVERT(BINARY(32),'USERID_2'), 'LOGIN_SUCCESS', 'INFORMATION', '03/21/2021 1:00 PM', 'LOGIN_EVENT', NULL), -- USER 2 LOGS IN
	( CONVERT(BINARY(32),'USERID_2'), 'VIEW_CALLED', 'INFORMATION', '03/21/2021 3:00 PM', 'VIEW_PAGE_EVENT', 'AnalyticsPage'), -- USER VISITS A PAGE (WILL GO BY PAGE IDs) -- PAGE VISIT  1
	( CONVERT(BINARY(32),'USERID_2'), 'LOGOUT_SUCCESS', 'INFORMATION', '03/21/2021 7:00 PM', 'LOGOUT_EVENT', NULL), -- LOGS OUT AT 7PM
	
	-- LOGS FOR USERID_4 (ROLE: Admin)
	( CONVERT(BINARY(32),'USERID_4'), 'LOGIN_SUCCESS' , 'INFORMATION', '03/23/2021 10:00 AM', 'LOGIN_EVENT', NULL), -- LOGIN IN AT 2 PM
	( CONVERT(BINARY(32),'USERID_4'), 'VIEW_CALLED'   , 'INFORMATION', '03/23/2021 1:00 PM', 'VIEW_PAGE_EVENT', 'MostPopularBuildPage'), -- PAGE VISIT  2
	( CONVERT(BINARY(32),'USERID_4'), 'VIEW_CALLED'   , 'INFORMATION', '03/23/2021 1:30 PM', 'VIEW_PAGE_EVENT', 'VendorLinkingPage'), -- PAGE VISIT  3
	( CONVERT(BINARY(32),'USERID_4'), 'VIEW_CALLED'   , 'INFORMATION', '03/23/2021 2:00 PM', 'VIEW_PAGE_EVENT', 'MostPopularBuildPage'), -- PAGE VISIT  2
	( CONVERT(BINARY(32),'USERID_4'), 'VIEW_CALLED'   , 'INFORMATION', '03/23/2021 3:00 PM', 'VIEW_PAGE_EVENT', 'VendorLinkingPage'), -- PAGE VISIT  3
	( CONVERT(BINARY(32),'USERID_4'), 'LOGOUT_SUCCESS', 'INFORMATION', '03/23/2021 6:00 PM', 'LOGOUT_EVENT', NULL), -- LOGS OUT AT 6PM
	
	-- LOGS FOR USERID_6 (ROLE: Basic)
	( CONVERT(BINARY(32),'USERID_6'), 'LOGIN_SUCCESS' , 'INFORMATION', '03/24/2021 1:00 PM', 'LOGIN_EVENT', NULL),
	( CONVERT(BINARY(32),'USERID_6'), 'VIEW_CALLED'   , 'INFORMATION', '03/24/2021 1:00 PM', 'VIEW_PAGE_EVENT', 'AnalyticsPage'),
	( CONVERT(BINARY(32),'USERID_6'), 'VIEW_CALLED'   , 'INFORMATION', '03/24/2021 1:30 PM', 'VIEW_PAGE_EVENT', 'VendorLinkingPage'),
	( CONVERT(BINARY(32),'USERID_6'), 'VIEW_CALLED'   , 'INFORMATION', '03/24/2021 2:00 PM', 'VIEW_PAGE_EVENT', 'AnalyticsPage'),
	( CONVERT(BINARY(32),'USERID_6'), 'VIEW_CALLED'   , 'INFORMATION', '03/24/2021 3:00 PM', 'VIEW_PAGE_EVENT', 'ProfilePage'),
	( CONVERT(BINARY(32),'USERID_6'), 'LOGOUT_SUCCESS', 'INFORMATION', '03/24/2021 5:00 PM', 'LOGOUT_EVENT', NULL),

	-- LOGS FOR USERID_8 (ROLE: Vendor)
	( CONVERT(BINARY(32),'USERID_8'), 'LOGIN_SUCCESS' , 'INFORMATION', '03/23/2021 1:00 PM', 'LOGIN_EVENT', NULL),
	( CONVERT(BINARY(32),'USERID_8'), 'VIEW_CALLED'   , 'INFORMATION', '03/23/2021 1:00 PM', 'VIEW_PAGE_EVENT', 'ProfilePage'),
	( CONVERT(BINARY(32),'USERID_8'), 'VIEW_CALLED'   , 'INFORMATION', '03/23/2021 1:30 PM', 'VIEW_PAGE_EVENT', 'VendorLinkingPage'),
	( CONVERT(BINARY(32),'USERID_8'), 'VIEW_CALLED'   , 'INFORMATION', '03/23/2021 2:00 PM', 'VIEW_PAGE_EVENT', 'MostPopularBuildPage'),
	( CONVERT(BINARY(32),'USERID_8'), 'VIEW_CALLED'   , 'INFORMATION', '03/23/2021 3:00 PM', 'VIEW_PAGE_EVENT', 'ProfilePage'),
	( CONVERT(BINARY(32),'USERID_8'), 'LOGOUT_SUCCESS', 'INFORMATION', '03/23/2021 4:00 PM', 'LOGOUT_EVENT', NULL);

	SELECT * FROM Logs where userHashID = CONVERT(BINARY(32),'USERID_12');

	-----------------------------------------------------------
use DB
INSERT INTO Mostpopularbuilds(Title,Description,LikeIncrementor,BuildTypeValue,BuildImagePath, DateTime) 
VALUES ('Test Title1', 'Test Description', 80, 1,'C:/Test/Directory', '2019'),
		('Test Title2', 'Test Description', 20, 2,'C:/Test/Directory', '2019'),
		('Test Title3', 'Test Description', 1, 3,'C:/Test/Directory', '2019'),
		('Test Title4', 'Test Description', 800, 3,'C:/Test/Directory', '2019'),
		('Test Title5', 'Test Description', 65, 3,'C:/Test/Directory', '2019'),
		('Test Title6', 'Test Description', 234, 2,'C:/Test/Directory', '2019');
INSERT INTO Mostpopularbuilds(userHashID ,Title,Description,LikeIncrementor,BuildTypeValue,BuildImagePath, DateTime) 
VALUES
        (CONVERT(BINARY(32),'USERID_4'),'Test Title7', 'Test Description', 90, 2,'C:/Test/Directory', '2019'),
		(CONVERT(BINARY(32),'USERID_4'),'Test Title8', 'Test Description', 23, 1,'C:/Test/Directory', '2019'),
		(CONVERT(BINARY(32),'USERID_4'),'Test Title9', 'Test Description', 123, 3,'C:/Test/Directory', '2019'),
		(CONVERT(BINARY(32),'USERID_4'),'Test Title10', 'Test Description', 1000, 3,'C:/Test/Directory', '2019'),
		(CONVERT(BINARY(32),'USERID_4'),'Test Title11', 'Test Description', 10000, 2,'C:/Test/Directory', '2019'),
		(CONVERT(BINARY(32),'USERID_4'),'Test Title12', 'Test Description', 5, 1,'C:/Test/Directory', '2019'),
		(CONVERT(BINARY(32),'USERID_4'),'Test Title13', 'Test Description', 20, 2,'C:/Test/Directory', '2019');


INSERT INTO Mostpopularbuilds(userHashID ,Title,Description,LikeIncrementor,BuildTypeValue,BuildImagePath, DateTime) 
VALUES
        (CONVERT(BINARY(32),'USERID_22'),'POST BUILD 1', 'Test Description1', 500, 2,'C:/Test/Directory', '2021-05-04'),
		(CONVERT(BINARY(32),'USERID_22'),'POST BUILD 2', 'Test Description2', 500, 1,'C:/Test/Directory', '2021-05-03'),
		(CONVERT(BINARY(32),'USERID_22'),'POST BUILD 3', 'Test Description3', 500, 3,'C:/Test/Directory', '2021-05-03');
		
	
		select * from Mostpopularbuilds;


		
INSERT INTO Reviews(postID, userHashID, message, star,  ReviewImagePath, datetime) 
VALUES (30002, CONVERT(BINARY(32),'USERID_4'),'THIS IS A TEST', '5', 'test','2019');

select * from reviews;



INSERT INTO Likes_Association(userHashID, postID)
VALUES 
 ( CONVERT(BINARY(32),'USERID_4'), 30006),
  ( CONVERT(BINARY(32),'USERID_4'), 30010);

 SELECT * FROM Likes_Association;



 ---------------------------------- inserts for post publication to the most popular builds 

 -- USER CONVERT(BINARY(32),'USERID_26') HAS 7 BUILD IN HIS SAVED PC BUILDS
 INSERT INTO  PcBuilds (userID, buildName, position, createdAt, modifiedAt)
VALUES	 ( 26, 'BUILD A' , 1, '2021-05-04', '2021-06-04'),
		 ( 26, 'BUILD B' , 2, '2021-05-04', '2021-06-04'), 
		 ( 26, 'BUILD C' , 3, '2021-05-04', '2021-06-04'),
		 ( 26, 'BUILD D' , 4, '2021-05-04', '2021-06-04'),
		 ( 26, 'BUILD E' , 5, '2021-05-04', '2021-06-04'),
		 ( 26, 'BUILD F' , 6, '2021-05-04', '2021-06-04'),
		 ( 26, 'BUILD G' , 7, '2021-05-04', '2021-06-04');

		 -- 
--INSERT INTO Mostpopularbuilds 
--(userHashID ,Title,Description,LikeIncrementor,BuildTypeValue,BuildImagePath, DateTime) 
--VALUES (CONVERT(BINARY(32),'USERID_26')





 select *  from UserAccounts
 --
 select * from UserCredentials
 select * from UserPermissions where userHashID = CONVERT(BINARY(32),'new egg');
	-----------------------------vendor inserts-----------------------------------


insert into UserAccounts(email, firstname, lastname,createdAt) values('newegg@newegg.com', 'new', 'egg','03/21/2021')
insert into MappingHash (userHashID, userID) values(CONVERT(BINARY(32),'new egg'), (select userID from UserAccounts where email = 'newegg@newegg.com'))
insert into UserCredentials(userHashID, username, passwordHash, locked, emailConfirmed, userRole) values
((select userHashID from MappingHash where userID = (select userID from userAccounts where email = 'newegg@newegg.com')), 'new egg', 'PassHash', 0, 0,'VendorRole')


   INSERT INTO UserPermissions(userHashID, permission,scopeOfPermission)VALUES
 (CONVERT(BINARY(32),'new egg'), 'ReadOnly', 'AutoBuild'),
 (CONVERT(BINARY(32),'new egg'), 'Delete', 'Self'),
 (CONVERT(BINARY(32),'new egg'),'Update', 'Self'),
 (CONVERT(BINARY(32),'new egg'), 'Edit', 'Self'),
 (CONVERT(BINARY(32),'new egg'), 'Create', 'Reviews'),
 (CONVERT(BINARY(32),'new egg'), 'Delete', 'SelfReviews'),
 (CONVERT(BINARY(32),'new egg'), 'Update', 'SelfReviews'),
 (CONVERT(BINARY(32),'new egg'), 'Create', 'Products'),
 (CONVERT(BINARY(32),'new egg'), 'Delete', 'VendorProducts'),
 (CONVERT(BINARY(32),'new egg'), 'Update', 'VendorProducts');


 select * from Vendor_UserAccount_Junction;

insert into VendorClub(vendorName) values ('new egg');

insert into Vendor_UserAccount_Junction(userHashID, vendorID)values
((select userHashID from MappingHash where userID 
= (select userID from userAccounts where email = 'newegg@newegg.com')),
(select vendorID from vendorclub where vendorname = 'new egg'))

---------------------------------------POPULATE SHELFS ----------------------------
INSERT INTO Shelves( userID, nameOfShelf)
VALUES 
(28 , 'TESTER 28' ),
(2 , 'TESTER 2' ),
(30 , 'TESTER 30' ),
(28 , 'TESTER 28_2' ),
(4 , 'TESTER 4' ),
(6 , 'TESTER 6' ),
(6 , 'TESTER 6_2' ),
(8 , 'TESTER 8' );
INSERT INTO Products (modelNumber,productName)
VALUES 
('MODEL_1', 'PRODUCT_1'),
('MODEL_2', 'PRODUCT_2'),
('MODEL_3', 'PRODUCT_3'),
('MODEL_4', 'PRODUCT_4'),
('MODEL_5', 'PRODUCT_5'),
('MODEL_6', 'PRODUCT_6'),
('MODEL_7', 'PRODUCT_7'),
('MODEL_8', 'PRODUCT_8'),
('MODEL_9', 'PRODUCT_9');

INSERT INTO Save_Product_Shelf (productID, shelfID, itemIndex)
VALUES 
(1,400002,1),
(8,400002,2),
(4,400002,3),
(6,400002,4),
(3,400002,5);

SELECT * FROM Shelves;
SELECT * FROM Products;
SELECT * FROM Save_Product_Shelf;



SELECT * FROM UserAccounts
INNER JOIN MappingHash
ON MappingHash.userID = UserAccounts.userID
INNER JOIN UserCredentials
ON UserCredentials.userHashID = MappingHash.userHashID;

SELECT * FROM MappingHash;
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
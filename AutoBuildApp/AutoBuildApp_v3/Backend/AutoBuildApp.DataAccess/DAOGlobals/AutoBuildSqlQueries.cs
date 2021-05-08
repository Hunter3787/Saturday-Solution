using System;
namespace AutoBuildApp.DataAccess
{
    public static class AutoBuildSqlQueries
    {
        #region ShelfDAO Queries
        public const string INSERT_SHELF =
            "INSERT INTO Shelves(userID, nameOfShelf) " +
           "VALUES( " +
           "(SELECT UA.userID " +
           "FROM UserAccounts UA " +
           "INNER JOIN MappingHash MH ON UA.userID = MH.userID " +
           "INNER JOIN UserCredentials UC ON MH.userHashID = UC.userHashID " +
           "WHERE username = @USERNAME) , @SHELFNAME);";

        public const string DELETE_SHELF =
            "DELETE FROM Shelves " +
            "WHERE nameOfShelf = @SHELFNAME " +
            "AND userID = (SELECT UA.userID " +
            "FROM UserAccounts UA " +
            "INNER JOIN MappingHash MH ON UA.userID = MH.userID " +
            "INNER JOIN UserCredentials UC ON MH.userHashID = UC.userHashID " +
            "WHERE username = @USERNAME);";

        public const string ADD_COMPONENT =
            "INSERT INTO Save_Product_Shelf (shelfID, productID, quantity, itemIndex) " +
            "VALUES(" +
            "(SELECT shelfID " +
            "FROM Shelves " +
            "WHERE nameOfShelf = @SHELFNAME), " +
            "(SELECT productID " +
            "FROM Products " +
            "WHERE modelNumber = @MODELNUMBER), " +
            "@QUANTITY, " +
            "(SELECT COUNT(itemIndex) " +
            "FROM Save_Product_Shelf " +
            "WHERE shelfID = " +
            "(SELECT shelfID " +
            "FROM Shelves " +
            "WHERE nameOfShelf = @SHELFNAME)));";

        public const string REMOVE_COMPONENT =
            "DELETE FROM Save_Product_Shelf " +
            "WHERE itemIndex = @ITEMINDEX " +
            "AND shelfId = " +
            "(SELECT shelfID " +
            "FROM Shelves S " +
            "WHERE nameOfShelf = @SHELFNAME " +
            "AND userID = " +
            "(SELECT UA.userID " +
            "FROM UserAccounts UA " +
            "INNER JOIN MappingHash MH ON UA.userID = MH.userID " +
            "INNER JOIN UserCredentials UC ON MH.userHashID = UC.userHashID " +
            "WHERE username = @USERNAME));";

        public const string UPDATE_QUANTITY =
            "UPDATE Save_product_Shelf " +
            "SET quantity = @QUANTITY " +
            "WHERE itemIndex = @ITEMINDEX " +
            "AND shelfID = " +
            "(SELECT shelfID " +
            "FROM Shelves " +
            "WHERE userID = (SELECT UA.userID FROM UserAccounts UA " +
            "INNER JOIN MappingHash MH ON UA.userID = MH.userID " +
            "INNER JOIN UserCredentials UC ON MH.userHashID = UC.userHashID " +
            "WHERE username = @USERNAME) " +
            "AND nameOfShelf = @SHELFNAME);";

        public const string UPDATE_SHELF_NAME =
            "UPDATE Shelves " +
            "SET nameOfShelf = @NEWSHELFNAME " +
            "WHERE shelfID = (SELECT shelfID " +
            "FROM Shelves " +
            "WHERE userID = " +
            "(SELECT UA.userID " +
            "FROM UserAccounts UA " +
            "INNER JOIN MappingHash MH ON UA.userID = MH.userID " +
            "INNER JOIN UserCredentials UC ON MH.userHashID = UC.userHashID " +
            "WHERE username = @USERNAME) " +
            "AND nameOfShelf = @OLDSHELFNAME);";

        public const string UPDATE_SHELF_ORDER = "";// TODO

        public const string GET_ALL_SHELVES_BY_USERNAME =
            "SELECT S.nameOfShelf, SPS.quantity, SPS.itemIndex , P.productType, P.modelNumber, P.manufacturerName " +
            "FROM Shelves S " +
            "LEFT JOIN Save_Product_Shelf SPS ON S.shelfID = SPS.shelfID " +
            "LEFT JOIN Products P ON P.productId = SPS.productID " +
            "WHERE userID = " +
            "(SELECT UA.userID  " +
            "FROM UserAccounts UA " +
            "INNER JOIN MappingHash MH ON UA.userID = MH.userID " +
            "INNER JOIN UserCredentials UC ON MH.userHashID = UC.userHashID " +
            "WHERE username = @USERNAME) " +
            "ORDER BY nameOfShelf, itemIndex;";

        public const string GET_SHELF_BY_NAME_AND_USER =
            "SELECT S.nameOfShelf, SPS.quantity, SPS.itemIndex , P.productType, P.modelNumber, P.manufacturerName " +
            "FROM Shelves S " +
            "LEFT JOIN Save_Product_Shelf SPS ON S.shelfID = SPS.shelfID " +
            "LEFT JOIN Products P ON P.productId = SPS.productID " +
            "WHERE userID = " +
            "(SELECT UA.userID " +
            "FROM UserAccounts UA " +
            "INNER JOIN MappingHash MH ON UA.userID = MH.userID " +
            "INNER JOIN UserCredentials UC ON MH.userHashID = UC.userHashID " +
            "WHERE username = @USERNAME) " +
            "AND nameOfShelf = @SHELFNAME " +
            "ORDER BY itemIndex;";
        #endregion

        #region BuildDAO
        public const string GET_ALL_BUILDS_BY_USER =
            "SELECT buildName, productType, manufacturerName, modelNumber, quantity " +
            "FROM PcBuilds PC " +
            "LEFT OUTER JOIN Save_Product_Build SPB ON PC.buildID = SPB.buildID " +
            "LEFT OUTER JOIN  Products P ON SPB.productID = P.productID " +
            "WHERE userID = " +
            "(SELECT UA.userID FROM UserAccounts UA " +
            "INNER JOIN MappingHash MH ON UA.userID = MH.userID " +
            "INNER JOIN UserCredentials UC ON MH.userHashID = UC.userHashID " +
            "WHERE username = @USERNAME) " +
            "ORDER BY modifiedAt DESC, position ASC;";

        public const string GET_BUILD_BY_NAME_AND_USER =
            "SELECT productType, modelNumber, manufacturerName, quantity " +
            "FROM Products P " +
            "INNER JOIN Save_Product_Build PSB ON P.productID = PSB.productID " +
            "WHERE PSB.buildID = " +
            "(SELECT buildID " +
            "FROM PcBuilds " +
            "WHERE buildName = @BUILDNAME " +
            "AND userID = " +
            "(SELECT UA.userID " +
            "FROM UserAccounts UA " +
            "INNER JOIN MappingHash MH ON UA.userID = MH.userID " +
            "INNER JOIN UserCredentials UC ON MH.userHashID = UC.userHashID " +
            "WHERE username = @USERNAME));";

        public const string DELETE_BUILD =
            "DELETE FROM PcBuilds " +
            "WHERE buildID = " +
            "(SELECT buildID " +
            "FROM PcBuilds " +
            "WHERE buildName = @BUILDNAME " +
            "AND userID = " +
            "(SELECT UA.userID " +
            "FROM UserAccounts UA " +
            "INNER JOIN MappingHash MH ON UA.userID = MH.userID " +
            "INNER JOIN UserCredentials UC ON MH.userHashID = UC.userHashID " +
            "WHERE username = @USERNAME));";

        public const string INSERT_EMPTY_BUILD =
            "INSERT INTO PcBuilds(userID, buildName, createdAt, createdby, position, imagePath) " +
            "VALUES(" +
            "(SELECT UA.userID " +
            "FROM UserAccounts UA " +
            "INNER JOIN MappingHash MH ON UA.userID = MH.userID " +
            "INNER JOIN UserCredentials UC ON MH.userHashID = UC.userHashID " +
            "WHERE username = @USERNAME), " +
            "@BUILDNAME, " +
            "@DATETIME, " +
            "@USERNAME, " +
            "(SELECT MAX(position) + 1 " +
            "FROM PcBuilds " +
            "WHERE userID = " +
            "(SELECT UA.userID " +
            "FROM UserAccounts UA " +
            "INNER JOIN MappingHash MH ON UA.userID = MH.userID " +
            "INNER JOIN UserCredentials UC ON MH.userHashID = UC.userHashID " +
            "WHERE username = @USERNAME)), " +
            "@IMAGEPATH);";

        public const string ADD_TO_BUILD =
            "INSERT INTO Save_Product_Build(buildID, productID, quantity) " +
            "VALUES(" +
            "(SELECT buildID " +
            "FROM PcBuilds " +
            "WHERE buildName = @BUILDNAME " +
            "AND userID = " +
            "(SELECT UA.userID " +
            "FROM UserAccounts UA " +
            "INNER JOIN MappingHash MH ON UA.userID = MH.userID " +
            "INNER JOIN UserCredentials UC ON MH.userHashID = UC.userHashID " +
            "WHERE username = @USERNAME)), " +
            "(SELECT productID " +
            "FROM Products " +
            "WHERE modelNumber = @MODELNUMBER), " +
            "@QUANTITY);";

        public const string PUBLISH_BUILD =
            "INSERT INTO Save_Product_MPB(postID, productID, quantity) " +
            "SELECT " +
            "(SELECT postID " +
            "FROM Mostpopularbuilds " +
            "WHERE Title = 'Title'), productID, quantity " +
            "FROM Save_Product_Build " +
            "WHERE buildID = " +
            "(SELECT buildID " +
            "FROM PcBuilds " +
            "WHERE buildName = @BUILDNAME " +
            "AND userID = " +
            "(SELECT UA.userID " +
            "FROM UserAccounts UA " +
            "INNER JOIN MappingHash MH ON UA.userID = MH.userID " +
            "INNER JOIN UserCredentials UC ON MH.userHashID = UC.userHashID " +
            "WHERE username = @USERNAME));";

        public const string UPDATE_BUILD =
            "UPDATE Save_Product_Build " +
            "SET productID = " +
            "(SELECT productID " +
            "FROM Products " +
            "WHERE modelNumber = @TOADD) " +
            "WHERE buildID = " +
            "(SELECT buildID " +
            "FROM PcBuilds " +
            "WHERE buildName = @BUILDNAME " +
            "AND userID = " +
            "(SELECT UA.userID " +
            "FROM UserAccounts UA " +
            "INNER JOIN MappingHash MH ON UA.userID = MH.userID " +
            "INNER JOIN UserCredentials UC ON MH.userHashID = UC.userHashID " +
            "WHERE username = @USERNAME)) " +
            "AND productID = " +
            "(SELECT productID FROM Products " +
            "WHERE modelNumber = @TOREPLACE);";
        #endregion
    }
}

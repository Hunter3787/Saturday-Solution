import React from "react";
import "./userAccountManager.css";

/*
Table view of all users of the system.
Sidenav modified to allow searching(?).
Table Collumns:
    Username
    Email
    FirstName
    LastName
    accountType
    lockoutStatus
    Verified
    modifiedBy
    modifiedDate

Allow sorting by any category.
Button:
    Disable user
    Activate User
    Delete User
*/
function AccountManager(){
    return(
    <div>
    <h2>THIS IS THE USER MANAGEMENT PAGE! </h2></div>
    );
}

export default AccountManager;
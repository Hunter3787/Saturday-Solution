const uri = 'https://localhost:5001/usermanagement';
var users = [];
let token = ' ';
const fetchRequest = {
    method: 'GET',
    mode: 'cors',
    headers: {
        'Accept' : 'application/json',
        'Content-Type': 'application/json',
        'Authorization': 'bearer ' + token
    }
};


// This function will call a fetch request.
async function getUsers() {
    fetch(uri, fetchRequest) // fetches the default URI
        .then(response => response.json()) // Will receive a response from the default response.json.
        .then(data => displayUsers(data)) // will call the display items function.
        .then(console.log("reloaded"))
        .catch(error => console.error('Unable to get items.', error)); // will catch an error and print the appropriate error message in console.
    //refreshData;
}
var main = document.getElementById("listOfDivs");
// This function will display items received from the http response.
function displayUsers(data) {

  main.innerHTML = '';

  // Create div that will be appended to the list of divs
  var innerListOfDivs = document.createElement('div');
  innerListOfDivs.classList.add('innerDiv');

  // Create div that imitates a table
  var divTable = document.createElement('div');
  divTable.classList.add('div-table');

  // Create header div
  var divRow = document.createElement('div');
  divRow.classList.add('div-table-row');

  // Create username header div
  var UsernameHeaderDiv = document.createElement('div');
  UsernameHeaderDiv.classList.add('div-table-col');
  UsernameHeaderDiv.classList.add('center');
  UsernameHeaderDiv.style="width:150px";
  UsernameHeaderDiv.innerHTML="Username";

  // Create email header div
  var EmailHeaderDiv = document.createElement('div');
  EmailHeaderDiv.classList.add('div-table-col');
  EmailHeaderDiv.classList.add('center');
  EmailHeaderDiv.style="width:250px";
  EmailHeaderDiv.innerHTML="Email";

  // Create firstname header div
  var FirstnameHeaderDiv = document.createElement('div');
  FirstnameHeaderDiv.classList.add('div-table-col');
  FirstnameHeaderDiv.classList.add('center');
  FirstnameHeaderDiv.style="width:130px";
  FirstnameHeaderDiv.innerHTML="First Name";

  // Create lastname header div
  var LastnameHeaderDiv = document.createElement('div');
  LastnameHeaderDiv.classList.add('div-table-col');
  LastnameHeaderDiv.classList.add('center');
  LastnameHeaderDiv.style="width:130px";
  LastnameHeaderDiv.innerHTML="Last Name";

  // Create createdat header div
  var CreatedAtHeaderDiv = document.createElement('div');
  CreatedAtHeaderDiv.classList.add('div-table-col');
  CreatedAtHeaderDiv.classList.add('center');
  CreatedAtHeaderDiv.style="width:100px";
  CreatedAtHeaderDiv.innerHTML="Created At";

  // Create modifiedat header div
  var ModifiedAtHeaderDiv = document.createElement('div');
  ModifiedAtHeaderDiv.classList.add('div-table-col');
  ModifiedAtHeaderDiv.classList.add('center');
  ModifiedAtHeaderDiv.style="width:100px";
  ModifiedAtHeaderDiv.innerHTML="Modified At";

  // Create role header div
  var RoleHeaderDiv = document.createElement('div');
  RoleHeaderDiv.classList.add('div-table-col');
  RoleHeaderDiv.classList.add('center');
  RoleHeaderDiv.style="width:130px";
  RoleHeaderDiv.innerHTML="Role";

  // Create LockState header div
  var LockStateHeaderDiv = document.createElement('div');
  LockStateHeaderDiv.classList.add('div-table-col');
  LockStateHeaderDiv.style="width:100px";
  LockStateHeaderDiv.innerHTML="Lock State";

  // Create Locked header div
  var LockHeaderDiv = document.createElement('div');
  LockHeaderDiv.classList.add('div-table-col');
  LockHeaderDiv.style="width:50px";
  LockHeaderDiv.innerHTML="Lock";

  // Create a permissions header div
  var PermissionsHeaderDiv = document.createElement('div');
  PermissionsHeaderDiv.classList.add('div-table-col');
  PermissionsHeaderDiv.classList.add('center');
  PermissionsHeaderDiv.style="width:175px";
  PermissionsHeaderDiv.innerHTML="Permissions";

  // New Save button div
  var SaveHeaderDiv = document.createElement('div');
  SaveHeaderDiv.classList.add('div-table-col');
  SaveHeaderDiv.classList.add('center');
  SaveHeaderDiv.style="width:125px";
  SaveHeaderDiv.innerHTML="Save lock state and permissions";

  // New delete button div
  var DeleteHeaderDiv = document.createElement('div');
  DeleteHeaderDiv.classList.add('div-table-col');
  DeleteHeaderDiv.classList.add('center');
  DeleteHeaderDiv.style="width:100px";
  DeleteHeaderDiv.innerHTML="Delete User";



  // Append all the header divs to the first row
  divRow.appendChild(UsernameHeaderDiv);
  divRow.appendChild(EmailHeaderDiv);
  divRow.appendChild(FirstnameHeaderDiv);
  divRow.appendChild(LastnameHeaderDiv);
  divRow.appendChild(CreatedAtHeaderDiv);
  divRow.appendChild(ModifiedAtHeaderDiv);
  divRow.appendChild(RoleHeaderDiv);
  divRow.appendChild(LockStateHeaderDiv);
  divRow.appendChild(LockHeaderDiv);
  divRow.appendChild(PermissionsHeaderDiv);
  divRow.appendChild(SaveHeaderDiv);
  divRow.appendChild(DeleteHeaderDiv);

  // Append each layer to the parent layer
  divTable.appendChild(divRow);
  innerListOfDivs.appendChild(divTable);
  main.append(innerListOfDivs);

  // Create the div that will populate all the rows
  var allDivRows = document.createElement('div');
  allDivRows.classList.add('allProducts');
  divTable.appendChild(allDivRows);


  data.forEach(item => {

    // Create the new row div
    var newDivRow = document.createElement('div');

    // New name div
    var newDivUsername = document.createElement('div');
    newDivUsername.classList.add('div-table-col');
    newDivUsername.style="width:150px";
    // User name text
    var UserNameText = document.createElement('text');
    UserNameText.innerHTML = item["userName"];
    newDivUsername.append(UserNameText);
    newDivRow.appendChild(newDivUsername);

    // New email div
    var newDivEmail = document.createElement('div');
    newDivEmail.classList.add('div-table-col');
    newDivEmail.style="width:250px";
    // User email text
    var UserEmailText = document.createElement('text');
    UserEmailText.innerHTML = item["email"];
    newDivEmail.append(UserEmailText);
    newDivRow.appendChild(newDivEmail);

    // New firstname div
    var newDivFirstname = document.createElement('div');
    newDivFirstname.classList.add('div-table-col');
    newDivFirstname.style="width:130px";
    // User firstname text
    var UserFirstnameText = document.createElement('text');
    UserFirstnameText.innerHTML = item["firstName"];
    newDivFirstname.append(UserFirstnameText);
    newDivRow.appendChild(newDivFirstname);

    // New lastname div
    var newDivLastname = document.createElement('div');
    newDivLastname.classList.add('div-table-col');
    newDivLastname.style="width:130px";
    // User lastname text
    var UserLastnameText = document.createElement('text');
    UserLastnameText.innerHTML = item["lastName"];
    newDivLastname.append(UserLastnameText);
    newDivRow.appendChild(newDivLastname);

    // New createdat div
    var newDivCreatedAt = document.createElement('div');
    newDivCreatedAt.classList.add('div-table-col');
    newDivCreatedAt.style="width:100px";
    // User created text
    var UserCreatedAtText = document.createElement('text');
    UserCreatedAtText.innerHTML = item["createdAt"];
    newDivCreatedAt.append(UserCreatedAtText);
    newDivRow.appendChild(newDivCreatedAt);

    // New modifiedat div
    var newDivModifiedAt = document.createElement('div');
    newDivModifiedAt.classList.add('div-table-col');
    newDivModifiedAt.style="width:100px";
    // User modifiedat text
    var UserModifiedAtText = document.createElement('text');
    UserModifiedAtText.innerHTML = item["modifiedAt"];
    newDivModifiedAt.append(UserModifiedAtText);
    newDivRow.appendChild(newDivModifiedAt);

    // New role div
    var newDivRole = document.createElement('div');
    newDivRole.classList.add('div-table-col');
    newDivRole.style="width:130px";
    // User role text
    var UserRoleText = document.createElement('text');
    UserRoleText.innerHTML = item["userRole"];
    newDivRole.append(UserRoleText);
    newDivRow.appendChild(newDivRole);

    // New lockState div
    var newDivLockState = document.createElement('div');
    newDivLockState.classList.add('div-table-col');
    newDivLockState.style="width:100px";
    // User role text
    var UserLockStateText = document.createElement('text');
    UserLockStateText.innerHTML = item["lockState"];
    newDivLockState.append(UserLockStateText);
    newDivRow.appendChild(newDivLockState);

    // New locked div
    var newDivLocked = document.createElement('div');
    newDivLocked.classList.add('div-table-col');
    newDivLocked.style="width:50px";
    // Locked checkbox
    var LockedCheckbox = document.createElement('input');
    LockedCheckbox.type = "checkbox";
    LockedCheckbox.id = item["userName"];
    LockedCheckbox.style = "  transform: scale(2);";
    if(item["lockState"] == true) {
        LockedCheckbox.checked = true
    } else {
        LockedCheckbox.checked = false;
    }
    //LockedCheckbox.checked = item["lockState"];
    //console.log(item["lockState"]);
    // if(item["locked"].lock == ) {
    //     LockedCheckbox.checked;
    // } else {
    //     LockedCheckbox.value = true;
    // }
    newDivLocked.append(LockedCheckbox);
    newDivRow.appendChild(newDivLocked);

    // New permissions div
    var newDivPermission = document.createElement('div');
    newDivPermission.classList.add('div-table-col');
    newDivPermission.style="width:175px";
    let PermissionsDropdown = document.querySelector('.PermissionsDropdown').cloneNode(true);
    newDivPermission.appendChild(PermissionsDropdown);
    newDivRow.appendChild(newDivPermission);

    // New save button div
    var newSaveDiv = document.createElement('div');
    newSaveDiv.classList.add('div-table-col');
    newSaveDiv.classList.add('center');
    newSaveDiv.style="width:125px";
    // Creates a delete button and appends it
    var SaveButton = document.createElement('button');
    SaveButton.innerHTML = "save";
    // When the save button is clicked, submit the input
    SaveButton.addEventListener('click', () => {
        saveState(item["userName"], PermissionsDropdown.options[PermissionsDropdown.selectedIndex].text);
    })

    // SaveButton.setAttribute("onclick", saveState(item["userName"], LockedCheckbox.checked));    
    newSaveDiv.append(SaveButton);
    newDivRow.appendChild(newSaveDiv);

    // New delete button div
    var newDeleteDiv = document.createElement('div');
    newDeleteDiv.classList.add('div-table-col');
    newDeleteDiv.classList.add('center');
    newDeleteDiv.style="width:100px";
    // Creates a delete button and appends it
    var DeleteButton = document.createElement('button');
    DeleteButton.innerHTML = "delete";
    DeleteButton.addEventListener('click', () => {
        deleteUser(item["userName"]);
    })

    newDeleteDiv.append(DeleteButton);
    newDivRow.appendChild(newDeleteDiv);

    
    

    divTable.appendChild(newDivRow);
  })
  
  users = data;
}



// Submits the add item to the back end
async function saveState(username, permission) {
var lockState = document.getElementById(username).checked;

var formData = new FormData();
// const item = {
//     username : username,
//     lockstate : lockstate
// }
// Prepares the formData to be sent to the back end
formData.append('username', username);
formData.append('lockstate', lockState);
// console.log(lockstate);
// console.log(username);
await fetch("https://localhost:5001/usermanagement/lock", {
method: 'PUT',
mode: 'cors',
headers: {
'Accept': 'application/json',
'Authorization': 'bearer ' + token
},
//body: JSON.stringify(item)
body: formData
})
// Display the output message to the screen
// .then(response => {
// if(response.ok) {
// alert('correct');
// }
// else {
// alert('error')
// }
// })
// Refresh the items
.then(await savePermission(username, permission));
//.then(getUsers());
}

async function savePermission(username, permission) {
    var formData = new FormData();
    formData.append('username', username);
    formData.append('permission', permission);
    console.log(permission);

    await fetch("https://localhost:5001/usermanagement/permission", {
method: 'PUT',
mode: 'cors',
headers: {
'Accept': 'application/json',
'Authorization': 'bearer ' + token
},
//body: JSON.stringify(item)
body: formData
})
// Display the output message to the screen
.then(response => {
// if(response.ok) {
// alert('correct');
// }
// else {
// alert('error')
// }
})
// Refresh the items
.then(await displayUsers());
}

async function deleteUser(username) {
    var formData = new FormData();
    formData.append('username', username);

    // Confirmation button
  var result = confirm("Want to delete?");
  if(!result) {
    return;
  }

    await fetch("https://localhost:5001/usermanagement/user", {
method: 'DELETE',
mode: 'cors',
headers: {
'Accept': 'application/json',
'Authorization': 'bearer ' + token
},
//body: JSON.stringify(item)
body: formData
})
// Display the output message to the screen
.then(response => {
// if(response.ok) {
// alert('correct');
// }
// else {
// alert('error')
// }
})
// Refresh the items
.then(await displayUsers());
}
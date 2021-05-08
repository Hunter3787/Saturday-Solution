const uri ='https://localhost:5001/login';
let todos = [];
var JWT_Token = ' ';
const token = 'YOUR_TOKEN_HERE';

var loginButton = document.getElementById("Login")
    loginButton.addEventListener("click", authenticate)

function authenticate() {
  console.log("first");
  const addNameTextbox = document.getElementById('add-username');
  const addPasswordTextbox = document.getElementById('add-password');

  var url = new URL("https://localhost:5001/login"),
  UserCred = {
    Username : addNameTextbox.value.trim(),
    Password : addPasswordTextbox.value.trim(),
  }
  Object.keys(UserCred).forEach(key => {
    url.searchParams.append(key, UserCred[key])
  })

  fetch(url, {
    method: 'POST',
    headers: {
      'Accept': 'application/json',
      'Content-Type': 'application/json',
      'Authorization': 'bearer ' + JWT_Token
    },
    body: JSON.stringify(UserCred)
  })
    .then(response => response.json())
    .then(data => displayResponse(data))
    .then(response => getItem())
    .catch(error => console.error('Unable to Authenticate.', error));
}

function displayResponse(id)
{
  console.log("we are here")
  var getBody = document.getElementById("Response");
  console.log(id);
  var text =  document.createTextNode(JSON.stringify(id));
  alert(id);
  getBody.appendChild(text);
}

//let JWT = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJBdXRvYnVpbGQiLCJzdWIiOiJBdXRvYnVpbGQgVXNlciIsImF1ZCI6IlVTIiwiaWF0IjoxNjIwNDQzODA0LCJleHAiOjE2MjEwNDg2MDQsIm5iZiI6MTYyMTA0ODYwNCwiVXNlcm5hbWUiOiJGeXJlR3V5IiwiVXNlckNMYWltcyI6W3siUGVybWlzc2lvbiI6IkNyZWF0ZSIsIlNjb3BlT2ZQZXJtaXNzaW9ucyI6IlJldmlld3MifSx7IlBlcm1pc3Npb24iOiJEZWxldGUiLCJTY29wZU9mUGVybWlzc2lvbnMiOiJTZWxmIn0seyJQZXJtaXNzaW9uIjoiRGVsZXRlIiwiU2NvcGVPZlBlcm1pc3Npb25zIjoiU2VsZlJldmlld3MifSx7IlBlcm1pc3Npb24iOiJFZGl0IiwiU2NvcGVPZlBlcm1pc3Npb25zIjoiU2VsZiJ9LHsiUGVybWlzc2lvbiI6IlJlYWRPbmx5IiwiU2NvcGVPZlBlcm1pc3Npb25zIjoiQXV0b0J1aWxkIn0seyJQZXJtaXNzaW9uIjoiVXBkYXRlIiwiU2NvcGVPZlBlcm1pc3Npb25zIjoiU2VsZiJ9LHsiUGVybWlzc2lvbiI6IlVwZGF0ZSIsIlNjb3BlT2ZQZXJtaXNzaW9ucyI6IlNlbGZSZXZpZXdzIn1dfQ.LS4ITSAgVV3y0DsTyVhE0uOo2_GC4w5LbOV547RGSME";

function  getItem(){
  const fetchRequest = {
    method: 'GET',
    mode:'cors',
  headers: {
    'Accept': 'application/json',
    'Content-Type': 'application/json',
     'Authorization': 'bearer ' + JWT
  } };
  let customRequest = Object.assign( fetchRequest,{ method: 'GET' });
   fetch(url, customRequest) // fetches the default URI
      .then(response => response.json()) // Will receive a response from the default response.json.
      .then(response => displayResponse(response)) // will call the display items function.
      .then(response=> alert(response))
      .then(console.log("reloaded"))
      .catch(error => console.error('Unable to get items.', error)); // will catch an error and print the appropriate error message in console.
 
}

// // remove all console stuff -sir
// const url ='https://localhost:5001/login';

// const fetchRequest = {
//   method: 'GET',
//   mode:'cors',
// headers: {
//   'Accept': 'application/json',
//   'Content-Type': 'application/json',
//   // 'Authorization': 'Bearer ' + ' '
// } };

// let JWT ='eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJBdXRvYnVpbGQiLCJzdWIiOiJBdXRvYnVpbGQgVXNlciIsImF1ZCI6IlVTIiwiaWF0IjoxNjIwMDMyMTM4LCJleHAiOjE2MjA2MzY5MzgsIm5iZiI6MTYyMDYzNjkzOCwiVXNlcm5hbWUiOiJraW5nUGVuaTM5MyIsIlVzZXJDTGFpbXMiOlt7IlBlcm1pc3Npb24iOiJBTEwiLCJzY29wZU9mUGVybWlzc2lvbnMiOiJBTEwifV19.-en1y6qb2I-LfMbXPLCEQaiU3VAF2xaSDmbGS0Dba3g'
// function  getItem(){

//   const fetchRequest = {
//     method: 'GET',
//     mode:'cors',
//   headers: {
//     'Accept': 'application/json',
//     'Content-Type': 'application/json',
//      'Authorization': 'Bearer ' + JWT
//   } };
//   let customRequest = Object.assign( fetchRequest,{ method: 'GET' });
//    fetch(url, customRequest) // fetches the default URI
//       .then(response => response.json()) // Will receive a response from the default response.json.
//       .then(response => displayResponse(response)) // will call the display items function.
//       .then(response=> alert(response))
//       .then(console.log("reloaded"))
//       .catch(error => console.error('Unable to get items.', error)); // will catch an error and print the appropriate error message in console.
 
// }

// let Reg = document.getElementById('loginForm');
// Reg.addEventListener("submit", () =>  checkCredentials()); // lambda function for redirecting on click.

//  function checkCredentials() {
//   const addNameTextbox = document.getElementById('add-username');
//   const addHashTextbox = document.getElementById('add-passHash');
//   // this represent the usercredentials object
//   const UserCred = {
//     Username : addNameTextbox.value.trim(),
//     Password : addHashTextbox.value.trim()
//   };

// let customRequest = Object.assign( 
//   fetchRequest,{ method: 'POST' , body : JSON.stringify(UserCred) });

//    fetch(url, customRequest)
//     .then(response => response.json())
//     .then(response => displayResponse(response))
//     .then(response =>  getItem())
//     .catch(error => console.error('Unable to Authenticate.', error));
// }

// // add notes
// function displayResponse(id)
// {
//   console.log("we are here")
//   var getBody = document.getElementById("Response");
//   console.log(id);
//   var text =  document.createTextNode(JSON.stringify(id));
//   alert(id);
//   getBody.appendChild(text);

// }

// // https://www.w3schools.com/howto/howto_js_toggle_password.asp
function myFunction() {
  var x = document.getElementById("add-password");
  if (x.type === "password") {
    x.type = "text";
  } else {
    x.type = "password";
  }
} 

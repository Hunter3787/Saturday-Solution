
// remove all console stuff -sir
const url ='https://localhost:5001/authentication';

const fetchRequest = {
  method: 'GET',
  mode:'cors',
headers: {
  'Accept': 'application/json',
  'Content-Type': 'application/json',
  // 'Authorization': 'Bearer ' + ' '
} };

let JWT ='eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJBdXRvYnVpbGQiLCJzdWIiOiJBdXRvYnVpbGQgVXNlciIsImF1ZCI6IlVTIiwiaWF0IjoxNjIwMDMyMTM4LCJleHAiOjE2MjA2MzY5MzgsIm5iZiI6MTYyMDYzNjkzOCwiVXNlcm5hbWUiOiJraW5nUGVuaTM5MyIsIlVzZXJDTGFpbXMiOlt7IlBlcm1pc3Npb24iOiJBTEwiLCJzY29wZU9mUGVybWlzc2lvbnMiOiJBTEwifV19.-en1y6qb2I-LfMbXPLCEQaiU3VAF2xaSDmbGS0Dba3g'
function  getItem(){

  const fetchRequest = {
    method: 'GET',
    mode:'cors',
  headers: {
    'Accept': 'application/json',
    'Content-Type': 'application/json',
     'Authorization': 'Bearer ' + JWT
  } };
  let customRequest = Object.assign( fetchRequest,{ method: 'GET' });
   fetch(url, customRequest) // fetches the default URI
      .then(response => response.json()) // Will receive a response from the default response.json.
      .then(response => displayResponse(response)) // will call the display items function.
      .then(response=> alert(response))
      .then(console.log("reloaded"))
      .catch(error => console.error('Unable to get items.', error)); // will catch an error and print the appropriate error message in console.
 
}

let Reg = document.getElementById('loginForm');
Reg.addEventListener("submit", () =>  checkCredentials()); // lambda function for redirecting on click.

 function checkCredentials() {
  const addNameTextbox = document.getElementById('add-username');
  const addHashTextbox = document.getElementById('add-passHash');
  // this represent the usercredentials object
  const UserCred = {
    Username : addNameTextbox.value.trim(),
    Password : addHashTextbox.value.trim()
  };

let customRequest = Object.assign( 
  fetchRequest,{ method: 'POST' , body : JSON.stringify(UserCred) });

   fetch(url, customRequest)
    .then(response => response.json())
    .then(response => displayResponse(response))
    .then(response =>  getItem())
    .catch(error => console.error('Unable to Authenticate.', error));

  
}

// add notes
function displayResponse(id)
{
  console.log("we are here")
  var getBody = document.getElementById("Response");
  console.log(id);
  var text =  document.createTextNode(JSON.stringify(id));
  alert(id);
  getBody.appendChild(text);

}

// https://www.w3schools.com/howto/howto_js_toggle_password.asp
function myFunction() {
  var x = document.getElementById("add-passHash");
  if (x.type === "password") {
    x.type = "text";
  } else {
    x.type = "password";
  }
} 

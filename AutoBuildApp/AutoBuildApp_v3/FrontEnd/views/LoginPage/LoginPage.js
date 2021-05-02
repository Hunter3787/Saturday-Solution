
// remove all console stuff -sir
const url ='https://localhost:5001/authentication';

const fetchRequest = {
  method: 'GET',
  mode:'cors',
headers: {
  'Accept': 'application/json',
  'Content-Type': 'application/json',
  //'Access-Control-Allow-Origin' : '*',
  // 'Authorization': 'Bearer ' + ' '
} };

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

let customRequest = Object.assign( fetchRequest,{ method: 'POST' , body : JSON.stringify(UserCred) });

    fetch(url, customRequest)
    .then(response => response.json())
    .then(response => displayResponse(response))
    .catch(error => console.error('Unable to Authenticate.', error));

}
// add notes
function displayResponse(id)
{
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

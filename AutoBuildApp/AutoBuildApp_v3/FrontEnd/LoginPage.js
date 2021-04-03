//const uri1 = 'https://localhost:44363/api/TodoItems';
const uri ='https://localhost:5001/authentication';
let todos = [];
const token = 'YOUR_TOKEN_HERE';

  //https://gomakethings.com/using-oauth-with-fetch-in-vanilla-js/
  // https://www.digitalocean.com/community/tutorials/how-to-use-the-javascript-fetch-api-to-get-data

var msg = 'Hello world';

console.log(msg);
function getItems() {
  fetch(uri)
    .then(response => response.json())
    .then(data => _displayItems(data))
    .catch(error => console.error('Unable to get items.', error));
}

function checkCredentials() {
  console.log('Verifying credentials');
  const addNameTextbox = document.getElementById('add-username');
  const addHashTextbox = document.getElementById('add-passHash');
  // this represent the usercredentials object
  const UserCred = {
    Username : addNameTextbox.value.trim(),
    Password : addHashTextbox.value.trim()
  };

  fetch(`https://localhost:5001/authentication/UserCred`, {
    method: 'POST',
    headers: {
      'Accept': 'application/json',
      'Content-Type': 'application/json'
      
    },
    body: JSON.stringify(UserCred)
  })
    .then(response => response.json())
    .then(response => displayToken(response))
    .then(() => {  getItems();  })
    .catch(error => console.error('Unable to Authenticate.', error));
    
  closeInput();

  return false;
}
function closeInput() {
  document.getElementById('editForm').style.display = 'none';
}

// add notes
function displayToken(id)
{
  var getBody = document.getElementById("JWT-TOKEN");
  console.log(id);
  var text =  document.createTextNode(JSON.stringify(id));
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

const uri ='https://localhost:5001/registration';
let todos = [];
var JWT_Token = ' ';
const token = 'YOUR_TOKEN_HERE';

var registerButton = document.getElementById("RegisterUser")
    registerButton.addEventListener("click", register)

function register() {
    console.log("hey");
    const addNameTextbox = document.getElementById('add-username');
    const addFirstTextbox = document.getElementById('add-firstname');
    const addLastTextbox = document.getElementById('add-lastname');
    const addEmailTextbox = document.getElementById('add-email');
    const addHashTextbox = document.getElementById('add-password');
    const addHash2Textbox = document.getElementById('add-passwordCheck');
    
    var url = new URL("https://localhost:5001/registration"),
    UserCred = {
      Username : addNameTextbox.value.trim(),
      Firstname : addFirstTextbox.value.trim(), 
      Lastname : addLastTextbox.value.trim(),
      Email : addEmailTextbox.value.trim(),
      Password : addHashTextbox.value.trim(),
      PasswordCheck : addHash2Textbox.value.trim(),
    }
    Object.keys(UserCred).forEach(key => {
        url.searchParams.append(key, UserCred[key])
    })

    fetch(url, {
      method: 'POST',
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json',
        'Authorization': 'Bearer ' + JWT_Token
      },
      body: JSON.stringify(UserCred)
    })
      .then(response => response.json())
      .then(response => displayResponse(response))
      .catch(error => console.error('Unable to Authenticate.', error));
  
    return false;
  }

  function displayResponse(id)
{
    alert(id);
}

function changePage() {
  window.location.href = "LoginPage.html"
}
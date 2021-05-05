
let todos = [];
var JWT_Token = ' ';
const token = 'YOUR_TOKEN_HERE';

var updateUsername = document.getElementById("updateUsername")
    updateUsername.addEventListener("click", UpdateUserName)

function UpdateUserName() {
    const addNameTextbox = document.getElementById('add-username');
    
    var url = new URL("https://localhost:5001/usermanagement/username"),
    UserCred = {
      Username : addNameTextbox.value.trim(),

    }
    Object.keys(UserCred).forEach(key => {
        url.searchParams.append(key, UserCred[key])
    })

    fetch(url, {
      method: 'PUT',
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
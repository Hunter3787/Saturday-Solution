const uri ='http://localhost:8081/registration';
let todos = [];
var JWT_Token = ' ';
const token = 'YOUR_TOKEN_HERE';

var registerButton = document.getElementById("RegisterUser")
    registerButton.addEventListener("click", register)

function register() {
    const addNameTextbox = document.getElementById('add-username');
    const addFirstTextbox = document.getElementById('add-firstname');
    const addLastTextbox = document.getElementById('add-lastname');
    const addEmailTextbox = document.getElementById('add-email');
    const addHashTextbox = document.getElementById('add-password');
    const addHash2Textbox = document.getElementById('add-passwordCheck');
    
    var url = new URL("http://localhost:8081/registration"),
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
      //.catch(error => console.error('Unable to Authenticate.', error));
    return false;
  }

  function displayResponse(id)
{
    alert(id);
    changePageHome();
}

function changePage() {
  window.location.href = "LoginPage.html"
}

function getCookie(name) {
  var nameEQ = name + "=";
  var ca = document.cookie.split(';');
  for(var i=0;i < ca.length;i++) {
      var c = ca[i];
      while (c.charAt(0)==' ') c = c.substring(1,c.length);
      if (c.indexOf(nameEQ) == 0) return c.substring(nameEQ.length,c.length);
  }
  return null;
}

function changePageHome() {
  window.location.href = "http://127.0.0.1:5501/views/Recommender/Recommender.html"
}
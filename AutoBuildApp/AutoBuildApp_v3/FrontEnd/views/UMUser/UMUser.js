let token = ' ';

var updateUsername = document.getElementById("updateUsername")

    updateUsername.addEventListener('click', () => {
      var username = document.getElementById("add-username").value;
      UpdateUsername(username, "crkobel@verizon.net");
  })

function UpdateUsername(username, activeEmail) {
    var formData = new FormData
    
    formData.append('username', username);
    formData.append('activeEmail', activeEmail);

    fetch("https://localhost:5001/usermanagement/username", {
      method: 'PUT',
      mode: 'cors',
      headers: {
        'Accept': 'application/json',
        'Authorization': 'bearer ' + token
      },
      body: formData
    })
      // Display the output message to the screen
      .then(response => response.json())
      .then(response => displayResponse(response))
      .catch(error => console.error('Unable to Authenticate.', error));
  
    return false;
  }


  var updateEmail = document.getElementById("updateEmail")

    updateEmail.addEventListener('click', () => {
      var inputEmail = document.getElementById("add-email").value;
      UpdateEmail(inputEmail, "bunnyB@gmail.com");
  })

function UpdateEmail(inputEmail, activeEmail) {
    var formData = new FormData
    
    formData.append('inputEmail', inputEmail);
    formData.append('activeEmail', activeEmail);

    fetch("https://localhost:5001/usermanagement/email", {
      method: 'PUT',
      mode: 'cors',
      headers: {
        'Accept': 'application/json',
        'Authorization': 'bearer ' + token
      },
      body: formData
    })
      // Display the output message to the screen
      .then(response => response.json())
      .then(response => displayResponse(response))
      .catch(error => console.error('Unable to Authenticate.', error));
  
    return false;
  }


  var updatePassword = document.getElementById("updatePassword")

    updatePassword.addEventListener('click', () => {
      var password = document.getElementById("add-password").value;
      var passwordCheck = document.getElementById("add-passwordCheck").value;
      UpdatePassword(password, passwordCheck, "crkobel@verizon.net");
  })

function UpdatePassword(password, passwordCheck, activeEmail) {
    var formData = new FormData
    
    formData.append('password', password);
    formData.append('passwordCheck', passwordCheck);
    formData.append('activeEmail', activeEmail);
    console.log(password);
    console.log(passwordCheck);
    console.log(activeEmail);

    fetch("https://localhost:5001/usermanagement/password", {
      method: 'PUT',
      mode: 'cors',
      headers: {
        'Accept': 'application/json',
        'Authorization': 'bearer ' + token
      },
      body: formData
    })
      // Display the output message to the screen
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

let token = ' '



var updateUsername = document.getElementById("updateUsername")

    updateUsername.addEventListener('click', () => {
      var username = document.getElementById("add-username").value;
      UpdateUsername(username, "SERGE");
  })

function UpdateUsername(username, activeUsername) {
    var formData = new FormData
    
    formData.append('username', username);
    formData.append('activeUsername', activeUsername);

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
      UpdateEmail(inputEmail, "SERGE");
  })

function UpdateEmail(inputEmail, activeUsername) {
    var formData = new FormData
    
    formData.append('inputEmail', inputEmail);
    formData.append('activeUsername', activeUsername);

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
      var encryptedPassword = Encrypt(document.getElementById("add-password").value);
      var encryptedPasswordCheck = Encrypt(document.getElementById("add-passwordCheck").value);

      UpdatePassword(encryptedPassword, encryptedPasswordCheck, "SERGE");
  })

function UpdatePassword(password, passwordCheck, activeUsername) {
    var formData = new FormData
    
    formData.append('password', password);
    formData.append('passwordCheck', passwordCheck);
    formData.append('activeUsername', activeUsername);

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
    window.location.href = "RegisteredUsersList.html"
  }

  function Encrypt(str) {
      var KEY = "12345678900000001234567890000000";//32 bit
      var IV = "1234567890000000";//16 bits
var key = CryptoJS.enc.Utf8.parse(KEY);
var iv = CryptoJS.enc.Utf8.parse(IV);

var encrypted = '';

var srcs = CryptoJS.enc.Utf8.parse(str);
encrypted = CryptoJS.AES.encrypt(srcs, key, {
 iv: iv,
 mode: CryptoJS.mode.CBC,
 padding: CryptoJS.pad.Pkcs7
});

return encrypted.ciphertext.toString();
}
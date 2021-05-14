
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

    fetch("http://localhost:8081/usermanagement/username", {
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

    fetch("http://localhost:8081/usermanagement/email", {
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

    fetch("http://localhost:8081/usermanagement/password", {
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

var logoutUser = document.getElementById("Logout")

    logoutUser.addEventListener('click', () => {
      eraseCookieFromAllPaths("JWT");
      alert("Logged out");
      changePageHome();
  })

  function eraseCookieFromAllPaths(name) {
    // This function will attempt to remove a cookie from all paths.
    var pathBits = location.pathname.split('/');
    var pathCurrent = ' path=';
  
    // do a simple pathless delete first.
    document.cookie = name + '=; expires=Thu, 01-Jan-1970 00:00:01 GMT;';
  
    for (var i = 0; i < pathBits.length; i++) {
        pathCurrent += ((pathCurrent.substr(-1) != '/') ? '/' : '') + pathBits[i];
        document.cookie = name + '=; expires=Thu, 01-Jan-1970 00:00:01 GMT;' + pathCurrent + ';';
    }
  }

  function changePageHome() {
    window.location.href = "http://127.0.0.1:5501/views/Recommender/Recommender.html"
  }

  
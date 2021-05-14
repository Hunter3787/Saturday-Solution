
let token = ' '
let activeUsernameCookie = getCookie("Username");


var updateUsername = document.getElementById("updateUsername")

    updateUsername.addEventListener('click', () => {
      var username = document.getElementById("add-username").value;
      UpdateUsername(username, activeUsernameCookie);
  })

function UpdateUsername(username, activeUsername) {
    var formData = new FormData
    
    formData.append('username', username);
    formData.append('activeUsername', activeUsername);
    setCookie("Username", username, 7);
    activeUsernameCookie = getCookie("Username");
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
      UpdateEmail(inputEmail, activeUsernameCookie);
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

      UpdatePassword(encryptedPassword, encryptedPasswordCheck, activeUsernameCookie);
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

  var deleteMyself = document.getElementById("Delete")

    deleteMyself.addEventListener('click', () => {
      deleteSelf(activeUsernameCookie);
      eraseCookieFromAllPaths("JWT");
      eraseCookieFromAllPaths("Username");
      changePageHome();
  })

  function deleteSelf(username) {
    var formData = new FormData();
    formData.append('username', username);

    // Confirmation button
  var result = confirm("Are you sure you want to delete?");
  var result2 = confirm("This acction cannot be undone, confirm again");
  if(!result) {
    return;
  }
  if(!result2) {
    return;
  }

    fetch("http://localhost:8081/usermanagement/self", {
method: 'DELETE',
mode: 'cors',
headers: {
'Accept': 'application/json',
'Authorization': 'bearer ' + token
},
//body: JSON.stringify(item)
body: formData
})
// Display the output message to the screen
.then(response => response.json())
.then(response => displayResponse(response))
.catch(error => console.error('Unable to Authenticate.', error));
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
      eraseCookieFromAllPaths("Username");
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

  function getCookie(cname) {
    var jwt = "";
    var name = cname + "=";
    var decodedCookie = decodeURIComponent(document.cookie);
    var ca = decodedCookie.split(';');
    for(var i = 0; i <ca.length; i++) {
      var c = ca[i];
      while (c.charAt(0) == ' ') {
        c = c.substring(1);
      }
      if (c.indexOf(name) == 0) {
        jwt =  c.substring(name.length, c.length);
        jwt = jwt.replace(/"/g,"");
        return jwt;
      }
    }
    return "";
  }

  function setCookie(cname, cvalue, exdays) {
    var d = new Date();
    d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
    var expires = "expires="+d.toUTCString();
    document.cookie = cname + "=" + cvalue + ";" + expires + ";path=/";
  }
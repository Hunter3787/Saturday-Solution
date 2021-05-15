const uri ='http://localhost:8081/login';
let todos = [];
var JWT_Token = ' ';
const token = 'YOUR_TOKEN_HERE';

var loginButton = document.getElementById("Login")
    loginButton.addEventListener("click", () => {
      authenticate();
    })

function authenticate() {
  const addNameTextbox = document.getElementById('add-username');
  const addPasswordTextbox = document.getElementById('add-password');

  var url = new URL("http://localhost:8081/login"),
  UserCred = {
    Username : addNameTextbox.value.trim(),
    Password : addPasswordTextbox.value.trim(),
  }
  Object.keys(UserCred).forEach(key => {
    url.searchParams.append(key, UserCred[key])
  })
  eraseCookieFromAllPaths("Username");
  setCookie("Username", UserCred.Username, 7);
  fetch(url, {
    method: 'POST',
    headers: {
      'Accept': 'application/json',
      'Content-Type': 'application/json',
      'Authorization': 'bearer ' + JWT_Token
    },
    body: JSON.stringify(UserCred)
  })
    .then(response => response.json())
    .then(response => displayResponse(response))
    .catch(error => console.error('Unable to Authenticate.', error));
}

function displayResponse(id)
{
  deleteAllCookies();
  eraseCookieFromAllPaths("JWT");
  if (id == "Invalid username or password" || id == "Account is locked") {
    alert(id)
  } else {
    setCookie("JWT", JSON.stringify(id), 7);
    alert("Login successful")
    id = "";
    changePageHome();
  }
}



// // https://www.w3schools.com/howto/howto_js_toggle_password.asp
function myFunction() {
  var x = document.getElementById("add-password");
  if (x.type === "password") {
    x.type = "text";
  } else {
    x.type = "password";
  }
} 


function setCookie(cname, cvalue, exdays) {
  var d = new Date();
  d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
  var expires = "expires="+d.toUTCString();
  document.cookie = cname + "=" + cvalue + ";" + expires + ";path=/";
}

function deleteAllCookies() {
  var cookies = document.cookie.split(";");

  for (var i = 0; i < cookies.length; i++) {
      var cookie = cookies[i];
      var eqPos = cookie.indexOf("=");
      var name = eqPos > -1 ? cookie.substr(0, eqPos) : cookie;
      document.cookie = name + "=;expires=Thu, 01 Jan 1970 00:00:00 GMT";
  }
}

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
  window.location.href = "http://127.0.0.1:5501/views/MPBMain/MPB.html"
}
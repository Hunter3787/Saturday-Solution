const uri ='http://ec2-13-52-186-63.us-west-1.compute.amazonaws.com:8081/login';
let todos = [];
let jwt_token = getCookie("JWT");
const token = 'YOUR_TOKEN_HERE';

var loginButton = document.getElementById("Login")
    loginButton.addEventListener("click", () => {
      authenticate();
    })

function authenticate() {
  const addNameTextbox = document.getElementById('add-username');
  const addPasswordTextbox = document.getElementById('add-password');

  var url = new URL("http://ec2-13-52-186-63.us-west-1.compute.amazonaws.com:8081/login"),
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
      'Authorization': 'bearer ' + jwt_token
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
  window.location.href = "/views/Recommender/Recommender.html"
}

var profilePage = document.getElementById("profilePage")

profilePage.addEventListener('click', async () => {
  if (jwt_token == "" || jwt_token == "Invalid username or password" || jwt_token == "Account is locked") {
    alert("You are not logged in");
  } else {
    if (await checkAdminPrivilege() == true) {
      changePageAdmin();
    } else {
      changePageNotAdmin();
    }
  }
})

async function checkAdminPrivilege() {
  var result = false;
  var url = "http://ec2-13-52-186-63.us-west-1.compute.amazonaws.com:8081/authentication/admin"
  await fetch(url, {
    method: 'POST',
    mode: 'cors',
    headers: {
      'Accept': 'application/json',
      'Content-Type': 'application/json',
      'Authorization': 'bearer ' + jwt_token
    },
  })
  .then(response => response.json())
    .then(data => result = data);
    return result;
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

function changePageAdmin() {
  window.location.href = "/views/UMUser(Admin)/UMUser(Admin).html"
}

function changePageNotAdmin() {
  window.location.href = "/views/UMUser/UMUser.html"
}

function hideButtons() {
  var x = document.getElementById("profilePage");
  var y = document.getElementById("loginPage");
  var z = document.getElementById("registrationPage");
  console.log("hello")
  if (jwt_token != "") {
    y.style.display = "none";
    z.style.display = "none";
  } else {
    x.style.display = "none";
  }
}
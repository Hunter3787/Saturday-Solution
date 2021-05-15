const uri ='http://ec2-13-52-186-63.us-west-1.compute.amazonaws.com:8081/registration';
let todos = [];
let jwt_token = getCookie("JWT");
const token = 'YOUR_TOKEN_HERE';

var registerButton = document.getElementById("RegisterUser")
    registerButton.addEventListener("click", register)

function register() {
    console.log(getCookie("JWT"));
    const addNameTextbox = document.getElementById('add-username');
    const addFirstTextbox = document.getElementById('add-firstname');
    const addLastTextbox = document.getElementById('add-lastname');
    const addEmailTextbox = document.getElementById('add-email');
    const addHashTextbox = document.getElementById('add-password');
    const addHash2Textbox = document.getElementById('add-passwordCheck');
    
    var url = new URL("http://ec2-13-52-186-63.us-west-1.compute.amazonaws.com:8081/registration"),
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
        'Authorization': 'Bearer ' + jwt_token
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
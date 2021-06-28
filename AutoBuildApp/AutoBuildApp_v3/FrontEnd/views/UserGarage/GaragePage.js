const uri = 'https://http://ec2-13-52-186-63.us-west-1.compute.amazonaws.com:5001/UserGarage/';
let posts = [];
let jwt_token = getCookie("JWT");
const fetchRequst = {
    method: 'GET',
    mode: 'cors',
    header: {
        'Accept': 'application/json',
        'Content-Type': 'application/json',
        'Authorization': 'bearer ' + jwt_token
    }
};

async function getGarageContents(){
    await fetch(uri + getFilterString(), fetchRequst)
        .then(response => response.json())
        .then(data => {
            console.log(data);
            displayItems(data)
            })
}

let shelfName = document.getElementById("shelf-dropdown");
shelfName.onchange = function(){

}

function displayElements(data){
    

}

function loadShelf(){
    
}

function getFilterString(){
    return "getShelf?shelfName='TacoBell'&username='Zeina'";
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
  if (jwt_token != "") {
    y.style.display = "none";
    z.style.display = "none";
  } else {
    x.style.display = "none";
  }
}

getGarageContents();
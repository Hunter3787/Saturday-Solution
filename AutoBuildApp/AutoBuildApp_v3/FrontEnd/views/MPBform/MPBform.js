let posts = [];
let jwt_token = getCookie("JWT");
const fetchRequest = {
    method: 'GET',
    mode: 'cors',
    headers: {
        'Authorization': 'bearer ' + jwt_token
    }
};

// sets a keyup event listener for the textarea element.
let counter = document.getElementById('add-description');
counter.addEventListener("keyup", () => textCounter(counter, 'counter', 10000));

let form = document.getElementById('publish-form');
 // lambda function for redirecting on click.
form.addEventListener("submit", () => postItem());

// this is the async post item function that posts a build to the DB
async function postItem() {

    // sets var equal to the FormData returned by the function.
    var postData = addItem();

    // sets a custom request overriding the const request.
    let customRequest = Object.assign(fetchRequest, {method: 'POST', body: postData});

    // get the endpoint from the config file.
    let endpoint = appConfigurations.Endpoints.MostPopularBuilds || '';

    // makes a fetch post request with the custom request.
    await fetch(endpoint, customRequest)

    // redirects the page to the main page after a submission.
    window.location.assign("../MPBmain/MPB.html")
}


// This function will add an item to the DB.
function addItem() {

    // Initializes a FormData object.
    let formData = new FormData();

    // initialize an object for the build type value to be stored in.
    let buildTypeValue;

    let title = document.getElementById('add-title'); // will get the value from the html element and store it.
    let description = document.getElementById('add-description'); // will get the value from the html element description and store it.
    let photo = document.getElementById("add-image").files[0]; // store the file in the photo variable.
    let buildType = document.getElementsByName("build-type"); // will get the value from the html element buildType and store it.

    // This for loop will iterate through the radio elements and find which one is checked.
    for (let i = 0; i < buildType.length; i++)
    {
        if (buildType[i].checked)
        {
            buildTypeValue = buildType[i].value
        }
    }

    // The next 6 lines will store the above data in the formData object.
    formData.append("title", title.value.trim());
    formData.append("description", description.value.trim());
    formData.append("buildType", buildTypeValue);
    formData.append("image", photo);

    // return the FormData object.
    return formData;
}

// This function acts as a counter for the textarea field to show how many characters remain.
function textCounter(field, field2, maxlimit) {
    let countfield = document.getElementById(field2);

    if (field.value.length > maxlimit) {
        field.value = field.value.substring(0, maxlimit);
        return false;
    } else {
        countfield.innerText = maxlimit - field.value.length;
    }
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
  var url = "http://localhost:8081/authentication/admin"
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
  window.location.href = "http://127.0.0.1:5501/views/UMUser(Admin)/UMUser(Admin).html"
}

function changePageNotAdmin() {
  window.location.href = "http://127.0.0.1:5501/views/UMUser/UMUser.html"
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

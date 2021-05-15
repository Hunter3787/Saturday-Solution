let posts = [];
let filterArray = [];
let jwt_token = getCookie("JWT");
const fetchRequest = {
    method: 'GET',
    mode: 'cors',
    headers: {
        'Accept' : 'application/json',
        'Content-Type': 'application/json',
        'Authorization': 'bearer ' + jwt_token
    }
};

// Adds an event listener for the key up event for a search filter.
let searchFilter = document.getElementById('sbn');
searchFilter.addEventListener("keyup", () => findByName(searchFilter));

// Adds an event listener for redirecting to a different page when the post build button is clicked.
let postBuild = document.getElementById('post-build-button');
postBuild.addEventListener("click", () => window.location.assign("../MPBform/MPBform.html"))

// Adds an event listener foreach radio element in the radio group of build type radio buttons.
let buildRadio = document.querySelectorAll('input[name="build"]').forEach((elem) => {
  elem.addEventListener("change", () => getItems())
})

// Adds an event listener foreach radio element in the radio group of likes radio buttons.
let likesRadio = document.querySelectorAll('input[name="likes"]').forEach((elem) => {
  elem.addEventListener("change", () => getItems())
})

// This function conducts a filter that will filter the builds by search key.
function findByName(filter)
{
  // sets the array equal to the entire array of posts
  let filterArray = posts;

  // sets a result equal to the filtered array by the passed in parameter
  let result = filterArray.filter(item => item.title.toLowerCase().includes(filter.value.toLowerCase()) || item.username.toLowerCase().includes(filter.value.toLowerCase()))

  // calls the display filtered data array function.
  displayFilteredData(result);
}

// This function gets the filter string if radio buttons are clicked.
function getFilterString()
{
  
  let queryString = "?"; // initialize query string with a ? to indicate a query.
  var buildType = ""; // initialize a null query value for build type since that is the default.
  var orderLikes = ""; // initialize a null query value for the order of likes since that is the default.

  // will get the value from the html element build and store it in ele.
  var ele = document.getElementsByName('build'); 

  // This for loop will iterate through the radio elements and find which one is checked.
  for (var i = 0; i < ele.length; i++)
  {
      if (ele[i].checked)
      {
          buildType = ele[i].value;
      }
  }

  // will get the value from the html element likes and store it in ele.
  var ele = document.getElementsByName('likes');

  // This for loop will iterate through the radio elements and find which one is checked.
  for (var i = 0; i < ele.length; i++)
  {
      if (ele[i].checked)
      {
          orderLikes = ele[i].value;
      }
  }

  if ((orderLikes === null || orderLikes === '') && (buildType === null || buildType === '')){
    return '';
  }

  // This creates a string that will be appended to the query string.
  const params = new URLSearchParams({
    buildType: buildType,
    orderLikes: orderLikes
  });

  // returns the full query string.
  return queryString + params.toString();
}

// // creates a function called process that will call the get items function.
// function process(){
//   getItems();
// }

// // Sets a loop timer of 3 seconds to call the process function -> getItems();
// function looping(){
//   setTimeout(process, 3000);
// }

// // creates a timer interval of 3 seconds to keep track of the current times, so no concurrency occurs.
// var refreshData = setInterval(looping, 3000);

// This function will call a fetch request.
async function getItems() {

  let endpoint = appConfigurations.Endpoints.MostPopularBuilds;
  
  if (endpoint === null || endpoint === ''){
    endpoint = appConfigurations.Endpoints.default;
  }
  else{
    endpoint = endpoint + getFilterString();
  }

  console.log(endpoint);

    await fetch(endpoint, fetchRequest) // fetches the default URI
        .then(function(response) {
           if(response.redirected){
             console.log(response.url)
            window.location.href = response.url
           } 
           return response.json()
          })
        .then(data => displayItems(data)) // will call the display items function.
        .then(() => findByName(searchFilter))
}

// This function will display items received from the http response.
function displayItems(data) {

    const innerDiv = document.getElementById('main'); // This will get the id of the form from the HTML.
    innerDiv.innerHTML = ''; // appends a null value to the inner HTML, as is not required.

    // This function will create a table, and append values for each column and iterate to the next row of items.
    data.forEach(item => {

        // These are the HTML elements that will be created by the following code.

        //     <div class="gridbuilds">
        //      <div class="blockbuild" onclick="location.href='../MPBpost/MPBpost.html';">
        //         <p>Title</p>
        //         <div class="buildimage">
        //             <img src="http://cdna.pcpartpicker.com/static/forever/images/userbuild/358512.a97c3b2732e2a4d83247e1105f455c63.512.jpg">
        //         </div>
        //         <p>Username</p>
        //         <p>Likes</p>
        //         <p>Build Type</p>
        //      </div>
        //     </div>

        // make the div for the grid of builds
        var gridbuilds = document.createElement('div');
        gridbuilds.classList.add('gridbuilds');

        // make the div for each individual build and append it to the build block.
        var blockbuild = document.createElement('div');
        blockbuild.classList.add('blockbuild');

        // lambda function for redirecting on click.
        blockbuild.addEventListener("click", () => {
          sessionStorage.setItem('buildId', item["entityId"]);
          window.location.assign("../MPBpost/MPBpost.html")
        }); 

        // create a title element, appends text to it, and then appends all to a build block.
        var title = document.createElement('p');
        var titletext = document.createTextNode("Title: "+ item["title"]);
        title.appendChild(titletext);
        blockbuild.appendChild(title);

        // create the div element that encapsulates the image and then appends all to a block build.
        var buildimage = document.createElement('div');
        buildimage.classList.add('buildimage');
        var image = document.createElement("img");
        image.src = item["buildImagePath"];
        buildimage.appendChild(image);
        blockbuild.appendChild(buildimage);

        // creates the username element, appends text to it, and then appends all to a build block.
        var username = document.createElement('p');
        var usernametext = document.createTextNode("User: " + item["username"]);
        username.appendChild(usernametext);
        blockbuild.appendChild(username);

        // creates the likes element, appends text to it, and then appends all to a build block.
        var likes = document.createElement('p');
        var likestext = document.createTextNode("Likes: " + item["likeIncrementor"]);
        likes.appendChild(likestext);
        blockbuild.appendChild(likes);

        // creates the build type element, appends text to it, and then appends all to a build block.
        var buildtype = document.createElement('p');


        var build = "None";

        if (item["buildType"] === 1)
          build = "Graphic Artist";
        else if (item["buildType"] === 2)
          build = "Gaming";
        else if (item["buildType"] === 3)
          build = "Word Processing";


        var buildtypetext = document.createTextNode("Build: " + build);
        buildtype.appendChild(buildtypetext);
        blockbuild.appendChild(buildtype);

        // appends the block to the grid of builds.
        gridbuilds.appendChild(blockbuild);

        // appends the grid of builds to the main
        innerDiv.appendChild(gridbuilds);
    });

  posts = data; // will store the data as an array in this variable for local access.
}


// This function will display items received from the filter data function.
function displayFilteredData(data) {

  const innerDiv = document.getElementById('main'); // This will get the id of the form from the HTML.
  innerDiv.innerHTML = ''; // appends a null value to the inner HTML, as is not required.

  // This function will create a table, and append values for each column and iterate to the next row of items.
  data.forEach(item => {

      // These are the HTML elements that will be created by the following code.

      //     <div class="gridbuilds">
      //      <div class="blockbuild" onclick="location.href='../MPBpost/MPBpost.html';">
      //         <p>Title</p>
      //         <div class="buildimage">
      //             <img src="http://cdna.pcpartpicker.com/static/forever/images/userbuild/358512.a97c3b2732e2a4d83247e1105f455c63.512.jpg">
      //         </div>
      //         <p>Username</p>
      //         <p>Likes</p>
      //         <p>Build Type</p>
      //      </div>
      //     </div>

      // make the div for the grid of builds
      var gridbuilds = document.createElement('div');
      gridbuilds.classList.add('gridbuilds');

      // make the div for each individual build and append it to the build block.
      var blockbuild = document.createElement('div');
      blockbuild.classList.add('blockbuild');

      // lambda function for redirecting on click.
      blockbuild.addEventListener("click", () => {
        sessionStorage.setItem('buildId', item["entityId"]);
        window.location.assign("../MPBpost/MPBpost.html")
      }); 

      // create a title element, appends text to it, and then appends all to a build block.
      var title = document.createElement('p');
      var titletext = document.createTextNode("Title: "+ item["title"]);
      title.appendChild(titletext);
      blockbuild.appendChild(title);

      // create the div element that encapsulates the image and then appends all to a block build.
      var buildimage = document.createElement('div');
      buildimage.classList.add('buildimage');
      var image = document.createElement("img");
      image.src = item["buildImagePath"];
      buildimage.appendChild(image);
      blockbuild.appendChild(buildimage);

      // creates the username element, appends text to it, and then appends all to a build block.
      var username = document.createElement('p');
      var usernametext = document.createTextNode("User: " + item["username"]);
      username.appendChild(usernametext);
      blockbuild.appendChild(username);

      // creates the likes element, appends text to it, and then appends all to a build block.
      var likes = document.createElement('p');
      var likestext = document.createTextNode("Likes: " + item["likeIncrementor"]);
      likes.appendChild(likestext);
      blockbuild.appendChild(likes);

      // creates the build type element, appends text to it, and then appends all to a build block.
      var buildtype = document.createElement('p');


      var build = "None";

      if (item["buildType"] === 1)
        build = "Graphic Artist";
      else if (item["buildType"] === 2)
        build = "Gaming";
      else if (item["buildType"] === 3)
        build = "Word Processing";


      var buildtypetext = document.createTextNode("Build: " + build);
      buildtype.appendChild(buildtypetext);
      blockbuild.appendChild(buildtype);

      // appends the block to the grid of builds.
      gridbuilds.appendChild(blockbuild);

      // appends the grid of builds to the main
      innerDiv.appendChild(gridbuilds);
  });

  filterArray = data; // will store the data as an array in this variable for local access.
}

//let initializeView = document.getElementById("initialize-view").innerHTML = getItems();

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

getItems();
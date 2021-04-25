const uri = 'https://localhost:5001/mostpopularbuilds/build/?buildId=30000';
let posts = [];
let token = ' ';
const fetchRequest = {
    method: 'GET',
    mode: 'cors',
    headers: {
        'Accept' : 'application/json',
        'Content-Type': 'application/json',
        'Authorization': 'bearer ' + token
    }
};

function process(){
    getItem();
}

function looping(){
    setTimeout(process, 3000);
}
  
var refreshData = setInterval(looping, 3000);

async function getItem() {
    await fetch(uri, fetchRequest) // fetches the default URI
        .then(response => response.json()) // Will receive a response from the default response.json.
        .then(item => displayItem(item)) // will call the display items function.
        .then(console.log("reloaded"))
        .catch(error => console.error('Unable to get items.', error)); // will catch an error and print the appropriate error message in console.
    refreshData;
}

function displayItem(item) {


    const innerDiv = document.getElementById('main'); // This will get the id of the form from the HTML.
    innerDiv.innerHTML = ''; // appends a null value to the inner HTML, so that when its reloaded, its not added.

    // <div class="buildheader">
    //     <h1 class="buildtitle">Title</h1>
    //     <h4 class="buildusername">Username</h4>
    //     <h5 class="buildtype">Build Type</h5>
    //     <h5 class="builddate">Date Published</h5>
    // </div>

    // These two lines will initialize a div element with the selected class.
    var buildHeader = document.createElement('div');
    buildHeader.classList.add("buildheader");

    // These five lines will create a header 4 element, append the class and title data from the http get request.
    var title = document.createElement('h1');
    title.classList.add('buildtitle');
    var titleText = document.createTextNode(item["title"]);
    title.appendChild(titleText);
    buildHeader.appendChild(title);

    // These five lines will create a header 4 element, append the class and username data from the http get request.
    var username = document.createElement('h4');
    username.classList.add('buildusername');
    var usernameText = document.createTextNode(item["username"]);
    username.appendChild(usernameText);
    buildHeader.appendChild(username);

    // initialize a string build type of None.
    var build = "None";

    // Assigns a string value to the build type integer value from the http get request.
    if (item["buildType"] === 1)
      build = "Graphic Artist";
    else if (item["buildType"] === 2)
      build = "Gaming";
    else if (item["buildType"] === 3)
      build = "Word Processing";

    // These five lines will create a header 5 element, append the class and build type data assigned above.    
    var buildType = document.createElement('h5');
    buildType.classList.add('buildtype');
    var buildTypeText = document.createTextNode(build);
    buildType.appendChild(buildTypeText);
    buildHeader.appendChild(buildType);

    // These five lines will create a header 5 element, append the class and dateTime data from the http get request.
    var buildDate = document.createElement('h5');
    buildDate.classList.add('buildtype');
    var buildDateText = document.createTextNode(item["dateTime"]);
    buildDate.appendChild(buildDateText);
    buildHeader.appendChild(buildDate);

    // appends the build header div element to the main div in the html.
    innerDiv.appendChild(buildHeader);
    
    // <div class="builddetails">
    //     <div class="buildimage">
    //         <img src="http://cdna.pcpartpicker.com/static/forever/images/userbuild/358512.a97c3b2732e2a4d83247e1105f455c63.512.jpg">
    //     </div>
    //     <h5>Likes:500</h5>
    //     <h1>Description</h1>
    //     <p class="builddescription">
    //         Description
    //     </p>
    // </div>

    // create a div element for build details (image, likes, and description).
    var buildDetails = document.createElement('div');
    buildDetails.classList.add("builddetails");

    // These five lines will create an img element, append the class and image path data from the http get request.
    var buildImage = document.createElement('div');
    buildImage.classList.add("buildimage");
    var image = document.createElement("img");
    image.src = item["buildImagePath"];
    buildImage.appendChild(image);
    buildDetails.appendChild(buildImage);

    // These four lines will create a header 5 element, and append the likes data from the http get request.
    var likes = document.createElement('h5');
    var likesText = document.createTextNode("Likes: " + item["likeIncrementor"]);
    likes.appendChild(likesText);
    buildDetails.appendChild(likes);

    // These four lines will create a header 1 element, and make 'title' text for the description section.
    var descriptionTitle = document.createElement('h1');
    var descriptionTitleText = document.createTextNode("Description");
    descriptionTitle.appendChild(descriptionTitleText);
    buildDetails.appendChild(descriptionTitle);

    // These five lines will create a p element, append the class and description data from the http get request.
    var description = document.createElement('p');
    description.classList.add("builddescription");
    var descriptionText = document.createTextNode(item["description"]);
    description.appendChild(descriptionText);
    buildDetails.appendChild(description);

    // append the build details div to the main div.
    innerDiv.appendChild(buildDetails);

    post = item; // will store the data as an array in this variable for local access.
}

// let initializeView = document.getElementById("initialize-view").innerHTML = getItem();
const uri = 'https://localhost:5001/mostpopularbuilds/';
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

// function process(){
//     getItem();
// }

// function looping(){
//     setTimeout(process, 3000);
// }
  
// var refreshData = setInterval(looping, 3000);

async function getItem() {

    let queryString = 'build/?buildId=';

    var buildId = sessionStorage.getItem('buildId');

    await fetch(uri + queryString + buildId, fetchRequest) // fetches the default URI
        .then(response => response.json()) // Will receive a response from the default response.json.
        .then(item => displayItem(item)) // will call the display items function.
        .then(console.log("reloaded"))
        .catch(error => console.error('Unable to get items.', error)); // will catch an error and print the appropriate error message in console.
    //refreshData;
}

async function addLike() {

    let dateTime = new Date();

    const like = {
        postId: sessionStorage.getItem('buildId').toString(),
        userId: dateTime.toUTCString() + ' ' + dateTime.getMilliseconds()
        //userId: "Tommy"
    };

    let customRequest = Object.assign(fetchRequest, {method: 'POST', body: JSON.stringify(like)});

    await fetch(uri + 'like', customRequest);
    
    window.location.reload();
}

function displayItem(item) {


    const innerDiv = document.getElementById('main'); // This will get the id of the form from the HTML.
    innerDiv.innerHTML = ''; // appends a null value to the inner HTML, so that when its reloaded, its not added.

    // This is the HTML that will be created in the header section of the view.
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
    var usernameText = document.createTextNode("User: " + item["username"]);
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
    var buildTypeText = document.createTextNode("Build: " + build);
    buildType.appendChild(buildTypeText);
    buildHeader.appendChild(buildType);

    // These five lines will create a header 5 element, append the class and dateTime data from the http get request.
    var buildDate = document.createElement('h5');
    buildDate.classList.add('builddate');
    var buildDateText = document.createTextNode("Date Created: " + item["dateTime"]);
    buildDate.appendChild(buildDateText);
    buildHeader.appendChild(buildDate);

    // appends the build header div element to the main div in the html.
    innerDiv.appendChild(buildHeader);
    
    // This is the HTML that will be created in the 'body' section of the view.
    // <div class="builddetails">
    //     <div class="buildimage">
    //         <img src="http://cdna.pcpartpicker.com/static/forever/images/userbuild/358512.a97c3b2732e2a4d83247e1105f455c63.512.jpg">
    //     </div>
    //     <h5>Likes:500</h5>
    //     <button class="like-btn" id="like-button">Like Build</button> 
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

    // These six lines dynamically creates a like button for an individual build post.
    var likeButton = document.createElement('button');
    likeButton.classList.add('like-btn');
    likeButton.setAttribute("id", "like-button");
    likeButton.textContent = 'Like Build';
    likeButton.addEventListener("click", () => addLike());
    buildDetails.appendChild(likeButton);

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

    console.log(innerDiv);

    // will store the data as an array in this variable for local access.
    post = item; 
}
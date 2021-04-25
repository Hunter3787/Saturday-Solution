const uri = 'https://localhost:5001/MostPopularBuilds';
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

let postBuild = document.getElementById('post-build-button');
postBuild.addEventListener("click", () => window.location.assign("../MPBform/MPBform.html")); // lambda function for redirecting on click.

function process(){
  getItems();
}

function looping(){
  setTimeout(process, 3000);
}

var refreshData = setInterval(looping, 3000);

async function getItems() {
    await fetch(uri, fetchRequest) // fetches the default URI
        .then(response => response.json()) // Will receive a response from the default response.json.
        .then(data => displayItems(data)) // will call the display items function.
        .then(console.log("reloaded"))
        .catch(error => console.error('Unable to get items.', error)); // will catch an error and print the appropriate error message in console.
    refreshData;
}


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
        blockbuild.addEventListener("click", () => window.location.assign("../MPBpost/MPBpost.html")); // lambda function for redirecting on click.

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

//let initializeView = document.getElementById("initialize-view").innerHTML = getItems();
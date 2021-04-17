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

function getItems() {
    fetch(uri, fetchRequest) // fetches the default URI
        .then(response => response.json()) // Will receive a response from the default response.json.
        .then(data => displayItems(data)) // will call the display items function.
        .catch(error => console.error('Unable to get items.', error)); // will catch an error and print the appropriate error message in console.
}

function displayItems(data) {

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
        var titletext = document.createTextNode("title: "+ item["title"]);
        title.appendChild(titletext);
        blockbuild.appendChild(title);

        // create the div element that encapsulates the image and then appends all to a block build.
        var buildimage = document.createElement('div');
        buildimage.classList.add('buildimage');
        var image = new Image(200,200);
        image.src = "http://cdna.pcpartpicker.com/static/forever/images/userbuild/358512.a97c3b2732e2a4d83247e1105f455c63.512.jpg"
        buildimage.appendChild(image);
        blockbuild.appendChild(buildimage);

        // creates the username element, appends text to it, and then appends all to a build block.
        var username = document.createElement('p');
        var usernametext = document.createTextNode("user: " + item["username"]);
        username.appendChild(usernametext);
        blockbuild.appendChild(username);

        // creates the likes element, appends text to it, and then appends all to a build block.
        var likes = document.createElement('p');
        var likestext = document.createTextNode("likes: " + item["likeIncrementor"]);
        likes.appendChild(likestext);
        blockbuild.appendChild(likes);

        // creates the build type element, appends text to it, and then appends all to a build block.
        var buildtype = document.createElement('p');
        var buildtypetext = document.createTextNode("build: " + item["buildType"]);
        buildtype.appendChild(buildtypetext);
        blockbuild.appendChild(buildtype);

        // appends the block to the grid of builds.
        gridbuilds.appendChild(blockbuild);

        // gets the div by id in order to append the grid to the html.
        var main = document.getElementById('main');
        main.appendChild(gridbuilds);
    });

  posts = data; // will store the data as an array in this variable for local access.
}
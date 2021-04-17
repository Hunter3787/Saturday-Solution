const uri = 'https://localhost:44317/reviewrating';
let posts = [];

function getItems() {
    fetch('https://localhost:44317/MostPopularBuilds') // fetches the default URI
        .then(response => response.json()) // Will revieve a response from the default response.json.
        .then(data => _displayItems(data)) // will call the display items function.
        .catch(error => console.error('Unable to get items.', error)); // will catch an error and print the appropriate error message in console.
}

function _displayItems(data) {

    const tBody = document.getElementsByClassName('main'); // This will get the id of the form from the HTML.
    tBody.innerHTML = ''; // appends a null value to the inner HTML, as is not required.

    // This function will create a table, and append values for each column and iterate to the next row of items.
    data.forEach(item => {

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

        // make the div for each individual build
        var blockbuild = document.createElement('div');
        blockbuild.classList.add('blockbuild');
        blockbuild.addEventListener("click", function() {
            location.href("../MPBpost/MPBpost.html");
        });

        var title = document.createElement('p');
        var titletext = document.createTextNode("Title");
        title.appendChild(titletext);
        blockbuild.appendChild(title);

        var buildimage = document.createElement('div');
        buildimage.classList.add('buildimage');
        var image = new Image(200,200);
        image.src = "http://cdna.pcpartpicker.com/static/forever/images/userbuild/358512.a97c3b2732e2a4d83247e1105f455c63.512.jpg"
        buildimage.appendChild(image);
        blockbuild.appendChild(buildimage);

        var username = document.createElement('p');
        var usernametext = document.createTextNode("Username");
        username.appendChild(usernametext);
        blockbuild.appendChild(username);

        var likes = document.createElement('p');
        var likestext = document.createTextNode("Likes");
        likes.appendChild(likestext);
        blockbuild.appendChild(likes);

        var buildtype = document.createElement('p');
        var buildtypetext = document.createTextNode("Build Type");
        buildtype.appendChild(buildtypetext);
        blockbuild.appendChild(buildtype);

        gridbuilds.appendChild(blockbuild);
    });

  posts = data; // will store the data as an array in this variable.
}
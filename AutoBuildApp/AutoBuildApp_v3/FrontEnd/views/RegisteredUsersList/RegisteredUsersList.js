const uri = 'https://localhost:5001/usermanagement';
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


// // Adds an event listener foreach radio element in the radio group of build type radio buttons.
// let buildRadio = document.querySelectorAll('input[name="build"]').forEach((elem) => {
//   elem.addEventListener("change", () => getItems())
// })

// // Adds an event listener foreach radio element in the radio group of likes radio buttons.
// let likesRadio = document.querySelectorAll('input[name="likes"]').forEach((elem) => {
//   elem.addEventListener("change", () => getItems())
// })



// creates a function called process that will call the get items function.
function process(){
  getItems();
}

// Sets a loop timer of 3 seconds to call the process function -> getItems();
function looping(){
  setTimeout(process, 3000);
}

// creates a timer interval of 3 seconds to keep track of the current times, so no concurrency occurs.
var refreshData = setInterval(looping, 3000);

// This function will call a fetch request.
async function getItems() {

    await fetch(uri, fetchRequest) // fetches the default URI
        .then(response => response.json()) // Will receive a response from the default response.json.
        .then(data => displayItems(data)) // will call the display items function.
        .then(console.log("reloaded"))
        .catch(error => console.error('Unable to get items.', error)); // will catch an error and print the appropriate error message in console.
    refreshData;
}

// This function will display items received from the http response.
function displayItems(data) {

    const innerDiv = document.getElementById('main'); // This will get the id of the form from the HTML.
    innerDiv.innerHTML = ''; // appends a null value to the inner HTML, as is not required.

    // This function will create a table, and append values for each column and iterate to the next row of items.
    data.forEach(item => {

        // make the div for the grid of builds
        var gridbuilds = document.createElement('div');
        gridbuilds.classList.add('gridbuilds');

        // make the div for each individual build and append it to the build block.
        var blockbuild = document.createElement('div');
        blockbuild.classList.add('blockbuild');


        // create a title element, appends text to it, and then appends all to a build block.

        var title = document.createElement('p');
        var titletext = document.createTextNode("userName: "+ item["userName"]);
        title.appendChild(titletext);
        blockbuild.appendChild(title);

        var title = document.createElement('p');
        var titletext = document.createTextNode("email: "+ item["email"]);
        title.appendChild(titletext);
        blockbuild.appendChild(title);

        var title = document.createElement('p');
        var titletext = document.createTextNode("firstName: "+ item["firstName"]);
        title.appendChild(titletext);
        blockbuild.appendChild(title);

        var title = document.createElement('p');
        var titletext = document.createTextNode("lastName: "+ item["lastName"]);
        title.appendChild(titletext);
        blockbuild.appendChild(title);

        var title = document.createElement('p');
        var titletext = document.createTextNode("createdAt: "+ item["createdAt"]);
        title.appendChild(titletext);
        blockbuild.appendChild(title);

        var title = document.createElement('p');
        var titletext = document.createTextNode("modifiedAt: "+ item["modifiedAt"]);
        title.appendChild(titletext);
        blockbuild.appendChild(title);

        var title = document.createElement('p');
        var titletext = document.createTextNode("userRole: "+ item["userRole"]);
        title.appendChild(titletext);
        blockbuild.appendChild(title);


        var build = "None";

        // appends the block to the grid of builds.
        gridbuilds.appendChild(blockbuild);

        // appends the grid of builds to the main
        innerDiv.appendChild(gridbuilds);
    });

}

//let initializeView = document.getElementById("initialize-view").innerHTML = getItems();
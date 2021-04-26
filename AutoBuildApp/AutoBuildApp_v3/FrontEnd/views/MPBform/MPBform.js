const uri = 'https://localhost:5001/MostPopularBuilds';
let posts = [];
let token = ' ';
const fetchRequest = {
    method: 'GET',
    mode: 'cors',
};

// sets a keyup event listener for the textarea element.
let counter = document.getElementById('add-description');
counter.addEventListener("keyup", () => textCounter(counter, 'counter', 10000));

let form = document.getElementById('publish-form');
 // lambda function for redirecting on click.
form.addEventListener("submit", () => {
    postItem();
});

// this is the async post item function that posts a build to the DB
async function postItem() {

    // sets var equal to the FormData returned by the function.
    var postData = addItem();

    // sets a custom request overriding the const request.
    let customRequest = Object.assign(fetchRequest, {method: 'POST', body: postData});

    // makes a fetch post request with the custom request.
    await fetch(uri, customRequest)

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
    let username = document.getElementById('add-username'); // will get the value from the html element and store it.
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
    formData.append("username", username.value.trim());
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
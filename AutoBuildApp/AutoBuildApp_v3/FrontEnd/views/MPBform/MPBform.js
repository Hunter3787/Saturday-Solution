const uri = 'https://localhost:5001/MostPopularBuilds';
let posts = [];
let token = ' ';
const fetchRequest = {
    method: 'GET',
    mode: 'cors',
};

// "textCounter(this,'counter',10000);"

let counter = document.getElementById('add-description');
counter.addEventListener("keyup", () => textCounter(counter, 'counter', 10000));

let form = document.getElementById('publish-form');
form.addEventListener("submit", () => addItem()); // lambda function for redirecting on click.

// This function will add an item to the DB.
function addItem() {

    // Initializes a FormData object.
    let formData = new FormData();

    let title = document.getElementById('add-title'); // will get the value from the html element and store it.
    let username = document.getElementById('add-username'); // will get the value from the html element and store it.
    let description = document.getElementById('add-description'); // will get the value from the html element description and store it.
    let photo = document.getElementById("add-image").files[0]; // store the file in the photo variable.

    // The next 6 lines will store the above data in the formData object.
    formData.append("username", username.value.trim());
    formData.append("title", title.value.trim());
    formData.append("description", description.value.trim());
    formData.append("buildType", 2);
    formData.append("buildImagePath", "C:/Test/Directory");
    formData.append("image", photo);

    // Overrides the constant fetchRequest with custom attributes.
    let customRequest = Object.assign(fetchRequest, {method: 'POST', body: formData});

    // Makes a fetch request to the controller with the specified attributes.
    fetch(uri, customRequest);
    
}

function textCounter(field, field2, maxlimit) {
    var countfield = document.getElementById(field2);

    if (field.value.length > maxlimit) {
        field.value = field.value.substring(0, maxlimit);
        return false;
    } else {
        countfield.innerText = maxlimit - field.value.length;
    }
}
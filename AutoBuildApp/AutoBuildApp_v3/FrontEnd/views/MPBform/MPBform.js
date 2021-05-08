let posts = [];
let token = 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJBdXRvYnVpbGQiLCJzdWIiOiJBdXRvYnVpbGQgVXNlciIsImF1ZCI6IlVTIiwiaWF0IjoxNjIwNDMzNDI2LCJleHAiOjE2MjEwMzgyMjYsIm5iZiI6MTYyMTAzODIyNiwiVXNlcm5hbWUiOiJTRVJHRSIsIlVzZXJDTGFpbXMiOlt7IlBlcm1pc3Npb24iOiJDcmVhdGUiLCJTY29wZU9mUGVybWlzc2lvbnMiOiJSZXZpZXdzIn0seyJQZXJtaXNzaW9uIjoiRGVsZXRlIiwiU2NvcGVPZlBlcm1pc3Npb25zIjoiU2VsZiJ9LHsiUGVybWlzc2lvbiI6IkRlbGV0ZSIsIlNjb3BlT2ZQZXJtaXNzaW9ucyI6IlNlbGZSZXZpZXdzIn0seyJQZXJtaXNzaW9uIjoiRWRpdCIsIlNjb3BlT2ZQZXJtaXNzaW9ucyI6IlNlbGYifSx7IlBlcm1pc3Npb24iOiJSZWFkT25seSIsIlNjb3BlT2ZQZXJtaXNzaW9ucyI6IkF1dG9CdWlsZCJ9LHsiUGVybWlzc2lvbiI6IlVwZGF0ZSIsIlNjb3BlT2ZQZXJtaXNzaW9ucyI6IlNlbGYifSx7IlBlcm1pc3Npb24iOiJVcGRhdGUiLCJTY29wZU9mUGVybWlzc2lvbnMiOiJTZWxmUmV2aWV3cyJ9XX0.ZRTrsLvZXhhKIaXbhUgbDAMBus7oIBj8qDKreA5cJck';
const fetchRequest = {
    method: 'GET',
    mode: 'cors',
    headers: {
        'Authorization': 'bearer ' + token
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
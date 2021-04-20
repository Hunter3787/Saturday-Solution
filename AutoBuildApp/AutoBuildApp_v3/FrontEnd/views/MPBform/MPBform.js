const uri = 'https://localhost:5001/MostPopularBuilds';
let posts = [];
let token = ' ';
const fetchRequest = {
    method: 'GET',
    mode: 'cors',
};

// This function will add an item to the DB.
function addItem() {
    var formData = new FormData();

    const title = document.getElementById('add-title'); // will get the value from the html element and store it.
    const username = document.getElementById('add-username'); // will get the value from the html element and store it.
    const description = document.getElementById('add-description'); // will get the value from the html element description and store it.

    var photo = document.getElementById("add-image").files[0];

    formData.append("username", username.value.trim());
    formData.append("title", title.value.trim());
    formData.append("description", description.value.trim());
    formData.append("buildType", 2);
    formData.append("buildImagePath", "C:/Test/Directory");

    formData.append("image", photo);

    let customRequest = Object.assign(fetchRequest, {method: 'POST', body: formData});

    fetch(uri, customRequest);
    
}

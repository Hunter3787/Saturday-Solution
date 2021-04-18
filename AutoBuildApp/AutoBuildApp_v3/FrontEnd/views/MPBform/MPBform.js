const uri = 'https://localhost:5001/MostPopularBuilds/image';
let posts = [];
let token = ' ';
const fetchRequest = {
    method: 'GET',
    mode: 'cors',
};

// This function will add an item to the DB.
function addItem() {

    const title = document.getElementById('add-title'); // will get the value from the html element and store it.
    const username = document.getElementById('add-username'); // will get the value from the html element and store it.
    const description = document.getElementById('add-description'); // will get the value from the html element description and store it.

    var photo = document.getElementById("add-image").files[0];

    var formData = new FormData();

    formData.append("files", photo);

    // const item =
    // {
    //   username: "username",
    //   title: "title",
    //   description: "description",
    //   buildType: 2,
    //   buildImagePath: "C:/Test/Directory"
    // };

    let customRequest = Object.assign(fetchRequest, {method: 'POST', body: formData});

    fetch(uri, customRequest);

}

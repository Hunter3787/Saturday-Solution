const uri = 'https://localhost:5001/mostpopularbuilds';
const reviewsUri = 'https://localhost:5001/reviewrating';
let posts = [];
let reviews = [];
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

function process(){
    getItem();
}

function looping(){
    setTimeout(process, 3000);
}
  
var refreshData = setInterval(looping, 3000);

async function getItem() {

    let queryString = 'build/?buildId=';

    var buildId = sessionStorage.getItem('buildId');

    const customGetRequest = {
        method: 'GET',
        mode: 'cors',
        headers: {
            'Accept' : 'application/json',
            'Content-Type': 'application/json',
            'Authorization': 'bearer ' + token
        }
    };

    const customRequest = Object.assign(fetchRequest, {method: 'GET'});

    console.log(fetchRequest);

    await fetch(`${uri}/${queryString}${buildId}`, customGetRequest) // fetches the default URI
        .then(response => response.json()) // Will receive a response from the default response.json.
        .then(item => displayItem(item)) // will call the display items function.
        .then(() => getReviews())
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

    let uriExtension = "like";

    await fetch(`${uri}/${uriExtension}`, customRequest)
        .then(() => getItem())
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

    // will store the data as an array in this variable for local access.
    post = item; 
}









//------------------------ The following code is for individual reviews and ratings ----------------------------------------//






// sets a keyup event listener for the textarea element.
var counter = document.getElementById('add-message');
counter.addEventListener("keyup", () => textCounter(counter, 'counter', 10000));

// sets a submit event listener for when the review form is submitted.
var reviewForm = document.getElementById('review-form');
reviewForm.addEventListener("submit", () => addReview());

// Adds an event listener for the update build form.
var updateForm = document.getElementById('update-form');
updateForm.addEventListener('submit', () => { 
    updateReview();
});

// sets up an event listener for the close input button.
var closeInputButton = document.getElementById('close-input');
closeInputButton.addEventListener('click', () => closeInput());

// This function will load the page by calling the functions below.
async function getReviews() {

    let uriExtension = 'build/?buildId=' + sessionStorage.getItem('buildId');

    await fetch(`${reviewsUri}/${uriExtension}`) // fetches the default URI
        .then(response => response.json()) // Will retrieve a response from the default response.json.
        .then(data => displayReviews(data)) // will call the display items function.
        .catch(error => console.error('Unable to get items.', error)); // will catch an error and print the appropriate error message in console.
}

// This function will add an item to the DB.
async function addReview() {

    let formData = new FormData();

    var starValue; // this variable will be used to store the value of the selected stars.

    let addUserNameTextbox = document.getElementById('add-username'); // will get the value from the html element and store it.
    let ele = document.getElementsByName('rate'); // will get the value from the html element and store it in ele.
    let addMessageTextbox = document.getElementById('add-message'); // will get the value from the html element message and store it.
    let photo = document.getElementById("add-image").files[0]; // store the file in the photo variable.

    // This for loop will iterate through the radio elements and find which one is checked.
    for (var i = 0; i < ele.length; i++)
    {
        if (ele[i].checked)
        {
            starValue = ele[i].value
        }
    }

    // The next 5 lines will store the above data in the formData object.
    formData.append("buildId", sessionStorage.getItem('buildId').toString());
    formData.append("username", addUserNameTextbox.value.trim());
    formData.append("starRating", parseInt(starValue));
    formData.append("message", addMessageTextbox.value.trim());
    formData.append("image", photo);


    const cooost = {
        method: 'GET',
        mode: 'cors',
    };

    let customRequest = Object.assign(cooost, {method: 'POST', body: formData});

    await fetch(reviewsUri, customRequest);

    getItem();
}

// this function will call the delete fetch method.
async function deleteReview(id) {

    let customRequest = Object.assign(fetchRequest, {method: 'DELETE'});

    await fetch(`${reviewsUri}/${id}`, customRequest);

    getItem();
}

// will display the edit form for the id that is specified.
function displayEditForm(id) {

    const item = reviews.find(item => item["entityId"] === id.toString()); // finds the item that the id is associated with.

    document.getElementById('edit-id').value = item.entityId // sets the table id value equal to the value specified.
    document.getElementById('edit-message').value = item.message; // sets the table message value equal to the value specified.
    document.getElementById('edit-starRating').value = item.starRating; // sets the table starRating value equal to the value specified.
    document.getElementById('editForm').style.display = 'block'; // sets the table dispaly value to block.
}

// This method will update items in the DB.
async function updateReview() {

    let formData = new FormData();

    var itemId = document.getElementById('edit-id').value; // This will get the id of the item that has just been updated.

    let photo = document.getElementById("edit-filePath").files[0]; // store the file in the photo variable.

    // These 5 lines make a form data object and append the following data to it.
    formData.append("entityId", itemId.toString());
    formData.append("starRating", parseInt(document.getElementById('edit-starRating').value));
    formData.append("message", document.getElementById('edit-message').value.trim());
    formData.append("image", photo);

    const gtgtgtg = {
        method: 'GET',
        mode: 'cors',
    };

    let customRequest = Object.assign(gtgtgtg, {method: 'PUT', body: formData});

    // This will fetch the PUT request to send the updated object.
    await fetch(reviewsUri, customRequest);

    closeInput(); // will close the form when it is complete.

    getItem();
}

// This method will change the edit form display from block to none.
function closeInput() {
    document.getElementById('editForm').style.display = 'none';
}

// This function will display all the items in the fetch GET method.
function displayReviews(data) {

    const allReviews = document.getElementById('all-reviews'); // This will get the id of the form from the HTML.
    allReviews.innerHTML = ''; // appends a null value to the inner HTML, as is not required.

    // This function will create a table, and append values for each column and iterate to the next row of items.
    data.forEach(item => {

      // This is the html code that will be created by the following code.
      // <div class="all-reviews" id="all-reviews">
      //   <div class="reviews-containter" id="reviews-container">
      //       <div class="reviews-username" id="reviews-username">Username</div>
      //       <div class="reviews-date" id="reviews-date">01-21-2020</div>
      //       <div class="reviews-message" id="reviews-message"><p>Lorem ipsum dolor</p></div>
      //       <div class="reviews-rating" id="reviews-rating">5 5 5 5 5</div>
      //       <div class="reviews-photo" id="reviews-photo"><img class="reviews-image" src="/assets/images/Reviews/yay.jpg" alt=""></div>
      //       <div class="reviews-edit" id="reviews-edit"><button type="button" class="edit-btn">edit</button></div>
      //       <div class="reviews-delete" id="reviews-delete"><button type="button" class="delete-btn">delete</button></div>
      //   </div>
      // </div>

      // instantiated the stars to null
      var stars = ""; 

      // Creates the container div that holds all divs inside of it.
      var reviewsContainer = document.createElement('div');
      reviewsContainer.classList.add('reviews-containter');
      reviewsContainer.id = "reviews-container";

      // Creates the div element containing the text for the username.
      var reviewsUsername = document.createElement('div');
      reviewsUsername.classList.add('reviews-username');
      reviewsUsername.id = "reviews-username";
      reviewsUsername.innerHTML = item["username"];
      reviewsContainer.appendChild(reviewsUsername);

      // Creates the div element containing the text for the DateTime.
      var reviewsDate = document.createElement('div');
      reviewsDate.classList.add('reviews-date');
      reviewsDate.id = "reviews-date";
      reviewsDate.innerHTML = item["dateTime"];
      reviewsContainer.appendChild(reviewsDate);

      // Creates the div element that contains a paragraph which contains the message string from the DB.
      var reviewsMessage = document.createElement('div');
      reviewsMessage.classList.add('reviews-message');
      reviewsMessage.id = "reviews-message";
      var reviewMessageParagraph = document.createElement('p');
      reviewMessageParagraph.innerHTML = item["message"];
      reviewsMessage.appendChild(reviewMessageParagraph);
      reviewsContainer.appendChild(reviewsMessage);

      // this for loop will iterate through the number and append star chars as necessary.
      for (var i = 0; i < item["starRating"]; i++) 
      {
          stars += String.fromCharCode(9733);
      }

      // Creates the div element with the appropriate star value calculated in the above for loop.
      var reviewsRating = document.createElement('div');
      reviewsRating.classList.add('reviews-rating');
      reviewsRating.id = "reviews-rating";
      reviewsRating.innerHTML = stars;
      reviewsContainer.appendChild(reviewsRating);

      // Creates the div element that contains the image element which takes in the build path of the image.
      var reviewsPhoto = document.createElement('div');
      reviewsPhoto.classList.add('reviews-photo');
      reviewsPhoto.id = "reviews-photo";
      var img = document.createElement('img');
      img.classList.add("reviews-image");
      img.src = item["imagePath"]; // item["imagePath"]
      reviewsPhoto.appendChild(img);
      reviewsContainer.appendChild(reviewsPhoto);

      // Creates a div element that contains an edit button which allows the use of the edit form functionality.
      var reviewsEdit = document.createElement('div');
      reviewsEdit.classList.add('reviews-edit');
      reviewsEdit.id = "reviews-edit";
      var editButton = document.createElement('button');
      editButton.classList.add("edit-btn");
      editButton.type = "button";
      editButton.innerHTML = "edit";
      editButton.setAttribute('onclick', `displayEditForm(${item.entityId})`);
      reviewsEdit.appendChild(editButton);
      reviewsContainer.appendChild(reviewsEdit);

      // Creates a div element that contains a delete button which allows the use of the delete form functionality.
      var reviewsDelete = document.createElement('div');
      reviewsDelete.classList.add('reviews-delete');
      reviewsDelete.id = "reviews-delete";
      var deleteButton = document.createElement('button');
      deleteButton.classList.add("delete-btn");
      deleteButton.type = "button";
      deleteButton.innerHTML = "delete";
      deleteButton.setAttribute('onclick', `deleteReview(${item.entityId})`);
      reviewsDelete.appendChild(deleteButton);
      reviewsContainer.appendChild(reviewsDelete);

      // appends the container inside of the main all reviews div element.
      allReviews.appendChild(reviewsContainer)

    });

    // will store the data as an array in this variable.
    reviews = data; 
}

// This function acts as a counter for the textarea field to show how many characters remain.
function textCounter(field, field2, maxlimit) {
    var countfield = document.getElementById(field2);
    if (field.value.length > maxlimit) {
        field.value = field.value.substring(0, maxlimit);
        return false;
    } else {
        countfield.value = maxlimit - field.value.length;
    }
}
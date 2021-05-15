/// <summary>
/// References used from file: Solution Items/References.txt 
/// [2-6]
/// </summary>

const reviewsUri = 'https://http://ec2-13-52-186-63.us-west-1.compute.amazonaws.com:5001/reviewrating';
let reviews = [];

// This function will load the page by calling the functions below.
function getReviews() {
    fetch('https://http://ec2-13-52-186-63.us-west-1.compute.amazonaws.com:5001/reviewrating') // fetches the default URI
        .then(response => response.json()) // Will revieve a response from the default response.json.
        .then(data => displayReviews(data)) // will call the display items function.
        .catch(error => console.error('Unable to get items.', error)); // will catch an error and print the appropriate error message in console.
}

// This function will add an item to the DB.
function addReview() {
    var starValue; // this variable will be used to store the value of the selected stars.
    const addUserNameTextbox = document.getElementById('add-username'); // will get the value from the html element and store it.
    var ele = document.getElementsByName('rate'); // will get the value from the html element and store it in ele.

    // This for loop will iterate through the radio elements and find which one is checked.
    for (var i = 0; i < ele.length; i++)
    {
        if (ele[i].checked)
        {
            starValue = ele[i].value
        }
    }
    const addMessageTextbox = document.getElementById('add-message'); // will get the value from the html element message and store it.
    const addFilePathTextbox = document.getElementById('add-imagepath'); // will get the value from the image path html element and store it.

    // this is the json object of ReviewRating and send the data through to the controller.
    const item =
    {
      username: addUserNameTextbox.value.trim(),
      starRating: parseInt(starValue),
      message: addMessageTextbox.value.trim(),
      filePath: addFilePathTextbox.value
    };

    // Will fetch this URI with the POST method.
    fetch('https://http://ec2-13-52-186-63.us-west-1.compute.amazonaws.com:5001/ReviewRating', {
        method: 'POST',
        mode: 'cors',
    headers: {
      'Accept': 'application/json',
      'Content-Type': 'application/json'
    },
    body: JSON.stringify(item) // will convert the const item into a json object.
  })
    .then(response => response.json())
    .then(() => {
      getReviews(); // will call get items to get items once the add item is made.
    })
    .catch(error => console.error('Unable to add item.', error)); // logs error if it is caught.
}

// this function will call the delete fetch method.
function deleteReview(id) {
    fetch(`${reviewsUri}/${id}`, {
        method: 'DELETE',
        mode: 'cors',
  })
  .catch(error => console.error('Unable to delete item.', error)); // logs a caught error
}

// will display the edit form for the id that is specified.
function displayEditForm(id) {

    const item = reviews.find(item => item["entityId"] === id.toString()); // finds the item that the id is associated with.

    document.getElementById('edit-id').value = item.entityId // sets the table id value equal to the value specified.
    document.getElementById('edit-message').value = item.message; // sets the table message value equal to the value specified.
    document.getElementById('edit-starRating').value = item.starRating; // sets the table starRating value equal to the value specified.
    document.getElementById('edit-filePath').value = item.imagePath; // sets the table filepath value equal to the value specified.
    document.getElementById('editForm').style.display = 'block'; // sets the table dispaly value to block.
}

// This method will update items in the DB.
function updateReview() {
    const itemId = document.getElementById('edit-id').value; // This will get the id of the item that has just been updated.

    // creates a json item of values that have been updated.
    const item =
    {
        entityId: itemId.toString(),
        starRating: parseInt(document.getElementById('edit-starRating').value),
        message: document.getElementById('edit-message').value.trim(),
        filePath: document.getElementById('edit-filePath').value
    };

    // This will fetch the PUT request to send the updated object.
    fetch('https://http://ec2-13-52-186-63.us-west-1.compute.amazonaws.com:5001/ReviewRating', {
        method: 'PUT',
        mode: 'cors',
    headers: {
      'Accept': 'application/json',
      'Content-Type': 'application/json'
    },
    body: JSON.stringify(item) // This will send the object as a json.
  })
  .catch(error => console.error('Unable to update item.', error)); // logs error if caught.

  closeInput(); // will close the form when it is complete.

  return false;
}

// This method will change the edit form display from block to none.
function closeInput() {
  document.getElementById('editForm').style.display = 'none';
}

// This function will displat all the items in the fetch GET method.
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
      img.src = "/assets/images/Reviews/yay.jpg"; // item["imagePath"]
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
      deleteButton.setAttribute('onclick', `deleteItem(${item.entityId})`);
      reviewsDelete.appendChild(deleteButton);
      reviewsContainer.appendChild(reviewsDelete);

      // appends the container inside of the main all reviews div element.
      allReviews.appendChild(reviewsContainer)

    });

    // will store the data as an array in this variable.
    reviews = data; 
}
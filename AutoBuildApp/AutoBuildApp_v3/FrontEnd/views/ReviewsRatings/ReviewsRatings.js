/// <summary>
/// References used from file: Solution Items/References.txt 
/// [2-6]
/// </summary>

const uri = 'https://localhost:5001/reviewrating';
let reviews = [];

// This function will load the page by calling the functions below.
function getItems() {
    fetch('https://localhost:5001/reviewrating') // fetches the default URI
        .then(response => response.json()) // Will revieve a response from the default response.json.
        .then(data => _displayItems(data)) // will call the display items function.
        .catch(error => console.error('Unable to get items.', error)); // will catch an error and print the appropriate error message in console.
}

// This function will add an item to the DB.
function addItem() {
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
    fetch('https://localhost:5001/ReviewRating', {
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
      getItems(); // will call get items to get items once the add item is made.
    })
    .catch(error => console.error('Unable to add item.', error)); // logs error if it is caught.
}

// this function will call the delete fetch method.
function deleteItem(id) {
    fetch(`${uri}/${id}`, {
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
function updateItem() {
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
    fetch('https://localhost:5001/ReviewRating', {
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
function _displayItems(data) {

    const tBody = document.getElementById('reviews'); // This will get the id of the form from the HTML.
    tBody.innerHTML = ''; // appends a null value to the inner HTML, as is not required.

    // This function will create a table, and append values for each column and iterate to the next row of items.
    data.forEach(item => {
        var stars = ""; // instantiated the stars to null

        // Created the initial table.
        var table = document.createElement("tr"); 

        // These four lines will create a new row and append the item username to the text and then to the first column and then the table.
        var col1 = document.createElement("td"); 
        var text1 = document.createTextNode(item["username"]); 
        col1.appendChild(text1);
        table.appendChild(col1);

        // These next block of lines will create a new row and append the star value to the text and then to the second column and then the table.
        var col2 = document.createElement("td"); 
        for (var i = 0; i < item["starRating"]; i++) // this for loop will iterate through the number and append star chars as necessary.
        {
            stars += String.fromCharCode(9733);
        }
        var stars = document.createTextNode(stars); 
        col2.appendChild(stars);
        table.appendChild(col2);

        // These four lines will create a new row and append the item message to the text and then to the third column and then the table.
        var col3 = document.createElement("td"); 
        var text2 = document.createTextNode(item["message"]); 
        col3.appendChild(text2);
        table.appendChild(col3);

        // These four lines will create a new row and append the item dateTime to the text and then to the fourth column and then the table.
        var col4 = document.createElement("td"); 
        var text3 = document.createTextNode(item["dateTime"]); 
        col4.appendChild(text3);
        table.appendChild(col4);

        // These four lines will create a new row and append the item filepath to the text and then to the fifth column and then the table.
        var col5 = document.createElement("td"); 
        var filePath = document.createElement("img");
        filePath.src = item["filePath"];
        col5.appendChild(filePath);
        table.appendChild(col5);

        // These four lines will create a new row and append the delete button with the function to the text and then to the sixth column and then the table.
        var col6 = document.createElement("td");
        var deleteButton = document.createElement("button");
        deleteButton.innerText = 'Delete';
        deleteButton.setAttribute('onclick', `deleteItem(${item.entityId})`);
        col6.appendChild(deleteButton);
        table.appendChild(col6);

        // These four lines will create a new row and append the edit button with the function to the text and then to the sixth column and then the table.
        var col7 = document.createElement("td");
        var editButton = document.createElement("button");
        editButton.innerText = 'Edit';
        editButton.setAttribute('onclick', `displayEditForm(${item.entityId})`);
        col7.appendChild(editButton);
        table.appendChild(col7);

        // This will get the ID of the table that will be filled.
        var element = document.getElementById("reviews-saved"); 
        element.appendChild(table);
    });

  reviews = data; // will store the data as an array in this variable.
}
const uri ='https://localhost:5001/vendorlinking';

var tokenDanny = 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJBdXRvYnVpbGQiLCJzdWIiOiJBdXRvYnVpbGQgVXNlciIsImF1ZCI6IlVTIiwiaWF0IjoxNjE4NDUzMzM2LCJleHAiOjE2Mjk2NzcyMzYsIm5iZiI6MTYyOTY3NzIzNiwiVXNlcm5hbWUiOiJkYW5ueSIsIlVzZXJDTGFpbXMiOlt7IlBlcm1pc3Npb24iOiJDUkVBVEUiLCJzY29wZU9mUGVybWlzc2lvbnMiOiJSRVZJRVdTIn0seyJQZXJtaXNzaW9uIjoiREVMRVRFIiwic2NvcGVPZlBlcm1pc3Npb25zIjoiU0VMRiJ9LHsiUGVybWlzc2lvbiI6IkRFTEVURSIsInNjb3BlT2ZQZXJtaXNzaW9ucyI6IlNFTEZfUkVWSUVXUyJ9LHsiUGVybWlzc2lvbiI6IkVESVQiLCJzY29wZU9mUGVybWlzc2lvbnMiOiJTRUxGIn0seyJQZXJtaXNzaW9uIjoiUkVBRF9PTkxZIiwic2NvcGVPZlBlcm1pc3Npb25zIjoiQVVUT0JVSUxEIn0seyJQZXJtaXNzaW9uIjoiVVBEQVRFIiwic2NvcGVPZlBlcm1pc3Npb25zIjoiU0VMRiJ9LHsiUGVybWlzc2lvbiI6IlVQREFURSIsInNjb3BlT2ZQZXJtaXNzaW9ucyI6IlNFTEZfUkVWSUVXUyJ9LHsiUGVybWlzc2lvbiI6IkNSRUFURSIsInNjb3BlT2ZQZXJtaXNzaW9ucyI6IlBST0RVQ1RTIn0seyJQZXJtaXNzaW9uIjoiVVBEQVRFIiwic2NvcGVPZlBlcm1pc3Npb25zIjoiVkVORE9SX1BST0RVQ1RTIn0seyJQZXJtaXNzaW9uIjoiREVMRVRFIiwic2NvcGVPZlBlcm1pc3Npb25zIjoiVkVORE9SX1BST0RVQ1RTIn1dfQ.242uVukArptSKQY6mQpxH_MRdhRO0uEhDdUA4U6qJc4';
var tokenNewEgg = 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJBdXRvYnVpbGQiLCJzdWIiOiJBdXRvYnVpbGQgVXNlciIsImF1ZCI6IlVTIiwiaWF0IjoxNjE4NDUzMzM2LCJleHAiOjE2Mjk2NzcyMzYsIm5iZiI6MTYyOTY3NzIzNiwiVXNlcm5hbWUiOiJuZXcgZWdnIiwiVXNlckNMYWltcyI6W3siUGVybWlzc2lvbiI6IkNSRUFURSIsInNjb3BlT2ZQZXJtaXNzaW9ucyI6IlJFVklFV1MifSx7IlBlcm1pc3Npb24iOiJERUxFVEUiLCJzY29wZU9mUGVybWlzc2lvbnMiOiJTRUxGIn0seyJQZXJtaXNzaW9uIjoiREVMRVRFIiwic2NvcGVPZlBlcm1pc3Npb25zIjoiU0VMRl9SRVZJRVdTIn0seyJQZXJtaXNzaW9uIjoiRURJVCIsInNjb3BlT2ZQZXJtaXNzaW9ucyI6IlNFTEYifSx7IlBlcm1pc3Npb24iOiJSRUFEX09OTFkiLCJzY29wZU9mUGVybWlzc2lvbnMiOiJBVVRPQlVJTEQifSx7IlBlcm1pc3Npb24iOiJVUERBVEUiLCJzY29wZU9mUGVybWlzc2lvbnMiOiJTRUxGIn0seyJQZXJtaXNzaW9uIjoiVVBEQVRFIiwic2NvcGVPZlBlcm1pc3Npb25zIjoiU0VMRl9SRVZJRVdTIn0seyJQZXJtaXNzaW9uIjoiQ1JFQVRFIiwic2NvcGVPZlBlcm1pc3Npb25zIjoiUFJPRFVDVFMifSx7IlBlcm1pc3Npb24iOiJVUERBVEUiLCJzY29wZU9mUGVybWlzc2lvbnMiOiJWRU5ET1JfUFJPRFVDVFMifSx7IlBlcm1pc3Npb24iOiJERUxFVEUiLCJzY29wZU9mUGVybWlzc2lvbnMiOiJWRU5ET1JfUFJPRFVDVFMifV19.idn4GGfnwpOgt8tmaUTvtGHLJIF8KoHSz4dWCI07bns';

let listOfDivs = document.getElementById('listOfDivs');

const fetchRequest = {
  method: 'GET',
  mode: 'cors',
  headers: {
    'Accept': 'application/json',
    'Content-Type': 'application/json',
    'Authorization': 'bearer ' + tokenDanny
  }
};

// Gets all model numbers, then populates the dropdown menu.
function getAllModelNumbers() {
  fetch(uri + '/modelNumbers', fetchRequest)
  .then(response => response.json())
  .then(data => saveModelNumbers(data));
}

// Clears the dropdown menu.
function removeOptions(dropdown) {
  var length = dropdown.options.length - 1;
  for(var i = length; i >= 0; i--) {
    dropdown.remove(i);
  }
  var first = document.createElement('option');
  first.textContent = "Model Number";
  dropdown.appendChild(first);
}

// Takes the data, clears the dropdown menu, and populates the options
function saveModelNumbers(data) {
  var dropdown = document.querySelector('.modelNumberDropDown');

  removeOptions(dropdown);

  data.forEach(item => {
    var modelNumber = document.createElement('option');
    modelNumber.textContent = item;

    dropdown.appendChild(modelNumber);
  })
}

 // Prepends a row and provides functionality for the user to input a new item
async function addItem() {
  var listOfRows = document.querySelector('.allProducts');
  var newDivRow = document.createElement('div');

  // New image div
  var newDivImage = document.createElement('div');
  newDivImage.classList.add('div-table-col');
  newDivImage.style="width:130px; line-height:10px";

  // Images for the small image and the pop out image
  var ImageUrlSmall = new Image(50,50);
  var ImageUrlLarge = new Image(200,200);
  newDivImage.appendChild(ImageUrlSmall);
  newDivImage.appendChild(document.createElement('br'))

  // Create input element of type file and append to the new image div
  var upload = document.createElement('input');
  upload.type = 'file';
  upload.style = "width:90px; margin-top:-50px";
  newDivImage.appendChild(upload);

  // When an image is selected and chosen, fire this event
  upload.addEventListener('input', (() => {
    var photo = upload.files[0];
    const reader = new FileReader();

    // When the image loads, update the small image and the popup image
    reader.addEventListener("load", function() {
      ImageUrlSmall.src = reader.result;
      ImageUrlLarge.src = reader.result;
    }, false);

    if(photo) {
      reader.readAsDataURL(photo);
    }
  }))

  ImageUrlLarge.classList.add('largePhoto');
  ImageUrlLarge.classList.add('displayNone');

  // Add the large image to the document
  document.body.append(ImageUrlLarge);

  // If the mouse hovers over or if the mouse moves out, we toggle the pop out image.
  ImageUrlSmall.addEventListener('mouseover', () => {
    ImageUrlLarge.classList.toggle('displayNone');
  })
  ImageUrlSmall.addEventListener('mouseout', () => {
    ImageUrlLarge.classList.toggle('displayNone');
  })
  
  // New model number div
  var newDivModelNumber = document.createElement('div');
  newDivModelNumber.classList.add('div-table-col');
  newDivModelNumber.style = "width:175px";

  // Creates a deep copy of the drop down,  shows it, and appends it to the model number div
  let modelNumberDropDown = document.querySelector('.modelNumberDropDown').cloneNode(true)
  modelNumberDropDown.classList.remove('displayNone');
  newDivModelNumber.appendChild(modelNumberDropDown);

  // New name div
  var newDivName = document.createElement('div');
  newDivName.classList.add('div-table-col');
  newDivName.style="width:500px";

  // Creates text area and appends to new name div
  var ProductNameTextArea = document.createElement('textArea');
  ProductNameTextArea.setAttribute('cols', 60);
  ProductNameTextArea.setAttribute('rows',3);
  newDivName.appendChild(ProductNameTextArea);

  // New url div
  var newDivUrl = document.createElement('div');
  newDivUrl.classList.add('div-table-col');
  newDivUrl.style="width:300px";

  // Creates text area and appends to new product url div
  var ProductUrlTextArea = document.createElement('textArea');
  ProductUrlTextArea.setAttribute('cols', 34);
  ProductUrlTextArea.setAttribute('rows',2);
  newDivUrl.appendChild(ProductUrlTextArea);

  // New availability div
  var newDivAvailability = document.createElement('div');
  newDivAvailability.classList.add('div-table-col');
  newDivAvailability.classList.add('center');
  newDivAvailability.style="width:100px";

  // Creates a checkbox and appends to the new availability div
  var AvailabilityCheckbox = document.createElement('input');
  AvailabilityCheckbox.type = "checkbox";
  AvailabilityCheckbox.style = "  transform: scale(2);";
  newDivAvailability.appendChild(AvailabilityCheckbox);
  
  // New price div
  var newDivPrice = document.createElement('div');
  newDivPrice.classList.add('div-table-col');
  newDivPrice.classList.add('center');
  newDivPrice.style="width:100px";
  
  // Creates input field for price and appends a dollar sign and the input to the price div
  var ProductPriceInput = document.createElement('input');
  ProductPriceInput.style.width = '70px';
  var priceDiv = document.createElement('div');
  var dollarElement = document.createElement('text');
  dollarElement.textContent = '$ ';
  priceDiv.appendChild(dollarElement);
  priceDiv.appendChild(ProductPriceInput);
  newDivPrice.appendChild(priceDiv);

  // New button div
  var buttonDiv = document.createElement('div');
  buttonDiv.classList.add('div-table-col');
  buttonDiv.classList.add('center');
  buttonDiv.style="width:120px";
  
  // Creates an add button and a cancel button and appends them
  var addItemButton = document.createElement('button');
  addItemButton.innerHTML = "add";
  addItemButton.style = "margin-right:10px";
  
  var cancelButton = document.createElement('button');
  cancelButton.innerHTML = "clear";
  buttonDiv.appendChild(addItemButton);
  buttonDiv.appendChild(cancelButton);

  // When the add item button is clicked, submit the input
  addItemButton.addEventListener('click', async () => {
    var photo = upload.files[0];
    await submitAddItem(modelNumberDropDown.options[modelNumberDropDown.selectedIndex].text,
      ProductNameTextArea.value, photo, AvailabilityCheckbox.checked, 
      ProductUrlTextArea.value, ProductPriceInput.value);
  })

  // Sets all the text fields to blank and sets the dropdown to the default value.
  cancelButton.addEventListener('click', () => {
    ProductNameTextArea.value = '';
    ProductUrlTextArea.value = '';
    ProductPriceInput.value = '';
    AvailabilityCheckbox.checked = false;
    modelNumberDropDown.selectedIndex = modelNumberDropDown.options[0];
  })

  // Append all divs (columns) to the div row
  newDivRow.appendChild(newDivImage);
  newDivRow.appendChild(newDivModelNumber);
  newDivRow.appendChild(newDivName);
  newDivRow.appendChild(newDivUrl);
  newDivRow.appendChild(newDivAvailability);
  newDivRow.appendChild(newDivPrice);
  newDivRow.appendChild(buttonDiv);
  
  // Prepend this to the list of rows
  listOfRows.prepend(newDivRow);
}

// Submits the add item to the back end
 async function submitAddItem(modelNumber, name, photo, availability,
                              url, price) {
  
  var formData = new FormData();

  // Prepares the formData to be sent to the back end
  formData.append('modelNumber', modelNumber);
  formData.append('name', name);
  formData.append('photo', photo);
  formData.append("availability", availability);
  formData.append("url", url);
  formData.append("price", price);

   await fetch(uri, {
    method: 'POST',
    mode: 'cors',
    headers: {
      'Accept': 'application/json',
      'Authorization': 'bearer ' + tokenNewEgg
    },
    body: formData
  })
  // Display the output message to the screen
  .then(response => {
    if(response.ok) {
      alert('correct');
    }
    else {
      alert('error')
    }
  })
  // Refresh the items
  .then(getItemsByFilter());
}

// Submits the edit item to the back end
async function submitEditItem(modelNumber, newName, photo, newImageUrl, 
                        newAvailability, newUrl, newPrice) {
  var formData = new FormData();

  formData.append('modelNumber', modelNumber);
  formData.append('name', newName);
  formData.append('photo', photo);
  formData.append('imageUrl', newImageUrl);
  formData.append("availability", newAvailability);
  formData.append("url", newUrl);
  formData.append("price", newPrice);

  await fetch(uri, {
    method: 'PUT',
    mode: 'cors',
    headers: {
      'Accept': 'application/json',
    },
    body:formData
  })
    // Display the output message to the screen
  .then(response => {
    if(response.ok) {
      alert('correct');
    }
    else {
      alert('error');
    }
 })
  // Refresh the items
 .then(getItemsByFilter());
  
}

// Requests a delete operation from the back end
async function submitDeleteItem(modelNumber, newDivRow) {

  // Confirmation button
  var result = confirm("Want to delete?");
  if(!result) {
    return;
  }
  // let customRequest = Object.assign(fetchRequest, {method: 'DELETE'});

  // await fetch(uri + '?modelNumber=' + modelNumber, customRequest)
  fetch(uri + '?modelNumber=' +modelNumber, {
    method: 'DELETE',
    mode: 'cors',
    headers: {
      'Accept': 'application/json',
      'Content-Type': 'application/json'
    },
  })
  .then(response => {
    if(response.ok) {
      alert('successful deletion');
    }
    else {
      alert('could not delete');
    }
  })
  .then(newDivRow.style ="display:none")
}


// function getItems() {
//   fetch(uri, fetchRequest)
//     .then(response => response.json())
//     .then(data => displayItemsFilter(data))
//     .catch(error => console.error('Unable to get items.', error));
    
// }

// function process(){
//   getItemsByFilter();
// }
// function looping(){
//   setTimeout(process, 3000);
// }

// var refreshData = setInterval(looping, 3000);

// Gets the items by a specified list of filters
 function getItemsByFilter() {
  var filtersQueryParameter = "?filtersString=";
  var priceQueryParameter = "&order=";
  var checkboxes = document.querySelectorAll('input[type=checkbox]');

  // For every checkbox, if it's checked, split the id and get the first split.
  // Append to the filters query parameter
  //    e.g. power supply-button => 'power supply'
  checkboxes.forEach(checkbox => {
    if(checkbox.checked) {
      filtersQueryParameter += checkbox.id.split('-')[0] + ",";
    }
  })

  // Get the order sort and append to the price query parameter
  var sort = document.querySelectorAll('input[type=radio]');
  sort.forEach(btn => {
    if(btn.checked) {
      priceQueryParameter += btn.id;
    }
  })

  fetch(uri + filtersQueryParameter + priceQueryParameter, fetchRequest)
  .then(response => response.json())
  .then(getAllModelNumbers())
  .then(async data => await displayItemsFilter(data));

  console.log('hey');
}

// Display all items with the set of filters
 async function displayItemsFilter(data) {

  // Reset the list of divs
  listOfDivs.innerHTML = '';

  // Create div that will be appended to the list of divs
  var innerListOfDivs = document.createElement('div');
  innerListOfDivs.classList.add('innerDiv');

  // Create div that imitates a table
  var divTable = document.createElement('div');
  divTable.classList.add('div-table');

  // Create header div
  var divRow = document.createElement('div');
  divRow.classList.add('div-table-row');

  // Create image header div
  var ImageHeaderDiv = document.createElement('div');
  ImageHeaderDiv.classList.add('div-table-col');
  ImageHeaderDiv.classList.add('center');
  ImageHeaderDiv.style="width:130px";
  ImageHeaderDiv.innerHTML="Image";

  // Create model number header div
  var ModelNumberHeaderDiv = document.createElement('div');
  ModelNumberHeaderDiv.classList.add('div-table-col');
  ModelNumberHeaderDiv.classList.add('center');
  ModelNumberHeaderDiv.style="width:175px";
  ModelNumberHeaderDiv.innerHTML="Model Number";

  // Create name header div
  var NameHeaderDiv = document.createElement('div');
  NameHeaderDiv.classList.add('div-table-col');
  NameHeaderDiv.classList.add('center');
  NameHeaderDiv.style="width:500px";
  NameHeaderDiv.innerHTML="Name";

  // Create url header div
  var UrlHeaderDiv = document.createElement('div');
  UrlHeaderDiv.classList.add('div-table-col');
  UrlHeaderDiv.classList.add('center');
  UrlHeaderDiv.style="width:300px";
  UrlHeaderDiv.innerHTML="Url";

  // Create availability header div
  var AvailabilityHeaderDiv = document.createElement('div');
  AvailabilityHeaderDiv.classList.add('div-table-col');
  AvailabilityHeaderDiv.classList.add('center');
  AvailabilityHeaderDiv.style="width:100px";
  AvailabilityHeaderDiv.innerHTML="Availability";

  // Create price header div
  var PriceHeaderDiv = document.createElement('div');
  PriceHeaderDiv.classList.add('div-table-col');
  PriceHeaderDiv.classList.add('center');
  PriceHeaderDiv.style="width:100px";
  PriceHeaderDiv.innerHTML="Price";

  // Create button header div
  var ButtonHeaderDiv = document.createElement('div');
  ButtonHeaderDiv.classList.add('div-table-col');
  ButtonHeaderDiv.style="width:120px";

  // Append all the header divs to the first row
  divRow.appendChild(ImageHeaderDiv);
  divRow.appendChild(ModelNumberHeaderDiv);
  divRow.appendChild(NameHeaderDiv);
  divRow.appendChild(UrlHeaderDiv);
  divRow.appendChild(AvailabilityHeaderDiv);
  divRow.appendChild(PriceHeaderDiv);
  divRow.appendChild(ButtonHeaderDiv);

  // Append each layer to the parent layer
  divTable.appendChild(divRow);
  innerListOfDivs.appendChild(divTable);
  listOfDivs.append(innerListOfDivs);
  
  // Create the div that will populate all the rows
  var allDivRows = document.createElement('div');
  allDivRows.classList.add('allProducts');
  divTable.appendChild(allDivRows);

  // Display the add item row before the full list of products
  await addItem();

  data.forEach(item => {

    // Create the new row div
    var newDivRow = document.createElement('div');

    // Create the new image div
    var newDivImage = document.createElement('div');
    newDivImage.classList.add('div-table-col');
    newDivImage.style = "line-height: 10px; width: 130px";

    // Create the images and append the small image to the image div
    var ImageUrlSmall = new Image(50,50);
    var ImageUrlLarge = new Image(200,200);
    newDivImage.appendChild(ImageUrlSmall);
    newDivImage.appendChild(document.createElement('br'))

    // Create input element of type file and append to the new image div
    var upload = document.createElement('input');
    upload.type = 'file';
    upload.style = "width:90px; margin-top:-50px";
    newDivImage.appendChild(upload);

    // When an image is selected and chosen, fire this event
    upload.addEventListener('input', (() => {
      var photo = upload.files[0];
      const reader = new FileReader();

      // When the image loads, update the small image and the popup image
      reader.addEventListener("load", function() {
        ImageUrlSmall.src = reader.result;
        ImageUrlLarge.src = reader.result;
      }, false);

      if(photo) {
        reader.readAsDataURL(photo);
      }
    }))

    // If the mouse hovers over or if the mouse moves out, we toggle the pop out image.
    ImageUrlSmall.addEventListener('mouseover', () => {
      ImageUrlLarge.classList.toggle('displayNone');
    })
    ImageUrlSmall.addEventListener('mouseout', () => {
      ImageUrlLarge.classList.toggle('displayNone');
    })
    
    ImageUrlLarge.classList.add('largePhoto');
    ImageUrlLarge.classList.add('displayNone');

    ImageUrlLarge.src = item["imageUrl"];
    ImageUrlSmall.src = item["imageUrl"];
    document.body.append(ImageUrlLarge);

    // New model number div
    var newDivModelNumber = document.createElement('div');
    newDivModelNumber.classList.add('div-table-col');
    newDivModelNumber.classList.add('header');
    newDivModelNumber.style="width:175px";

    // Model number text
    var ModelNumberText = document.createElement('text');
    ModelNumberText.innerHTML = item["modelNumber"];
    newDivModelNumber.appendChild(ModelNumberText);

    // New name div
    var newDivName = document.createElement('div');
    newDivName.classList.add('div-table-col');
    newDivName.style="width:500px";

    // Product name text
    var ProductNameText = document.createElement('text');
    ProductNameText.innerHTML = item["name"];
    newDivName.append(ProductNameText);
    
    // Product name text area to be edited
    var ProductNameTextArea = document.createElement('textArea');
    ProductNameTextArea.setAttribute('cols', 60);
    ProductNameTextArea.setAttribute('rows',3);
    ProductNameTextArea.innerHTML = item["name"];

    // New url div
    var newDivUrl = document.createElement('div');
    newDivUrl.classList.add('div-table-col');
    newDivUrl.style="width:300px";

    // Url text
    var UrlText = document.createElement('a');
    UrlText.innerHTML = item["url"];
    UrlText.href = item["url"];
    newDivUrl.append(UrlText);

    // Url text area to be edited
    var ProductUrlTextArea = document.createElement('textArea');
    ProductUrlTextArea.setAttribute('cols', 34);
    ProductUrlTextArea.setAttribute('rows',3);
    ProductUrlTextArea.innerHTML = item["url"];

    // New availability div
    var newDivAvailability = document.createElement('div');
    newDivAvailability.classList.add('div-table-col');
    newDivAvailability.classList.add('center');
    newDivAvailability.style="width:100px";

    // Availiability checkbox
    var AvailabilityCheckbox = document.createElement('input');
    AvailabilityCheckbox.type = "checkbox";
    AvailabilityCheckbox.style = "  transform: scale(2);";
    if(item["availability"] == true) {
      AvailabilityCheckbox.checked = true;
    }
    newDivAvailability.appendChild(AvailabilityCheckbox);    

    // New price div
    var newDivPrice = document.createElement('div');
    newDivPrice.classList.add('div-table-col');
    newDivPrice.classList.add('center');
    newDivPrice.style="width:100px";

    // Price text
    var ProductPriceText = document.createElement('text');
    ProductPriceText.innerHTML = item["price"].toFixed(2);

    // Price input to be edited
    var ProductPriceInput = document.createElement('input');
    ProductPriceInput.classList.add('center');
    ProductPriceInput.style.width = '70px';
    ProductPriceInput.value = item["price"].toFixed(2);

    // Create main price div
    var priceDiv = document.createElement('div');
    var dollarElement = document.createElement('text');
    dollarElement.textContent = '$'
    // Append dollar sign and product price text
    priceDiv.appendChild(dollarElement);
    priceDiv.appendChild(ProductPriceText);
    newDivPrice.appendChild(priceDiv);

    // New button div
    var buttonDiv = document.createElement('div');
    buttonDiv.classList.add('div-table-col');
    buttonDiv.classList.add('center');
    buttonDiv.style="width:120px";

    // Create edit button
    var editButton = document.createElement('button');
    editButton.innerHTML = "Edit";
    editButton.style = "margin-right:10px";
    buttonDiv.appendChild(editButton);

    var deleteButton = document.createElement('button');
    deleteButton.innerHTML = "Delete";
    buttonDiv.appendChild(deleteButton);
    deleteButton.addEventListener('click', async () => {
      await submitDeleteItem(item["modelNumber"], newDivRow);
    })
    // When the edit button is clicked, remove the text 
    //    and append the editable text for name, url, and price
    editButton.addEventListener('click', async () => {
      newDivName.removeChild(ProductNameText);
      newDivUrl.removeChild(UrlText);
      priceDiv.removeChild(ProductPriceText);
      newDivName.append(ProductNameTextArea);
      newDivUrl.appendChild(ProductUrlTextArea);
      priceDiv.appendChild(ProductPriceInput);


      var imageUrl = item["imageUrl"];

      // Create save button and clear the button div
      var saveButton = document.createElement('button');
      saveButton.innerHTML = "Save";
      saveButton.style = "margin-right:10px";
      buttonDiv.innerHTML = '';

      // When save is clicked, submit the edit item
      saveButton.addEventListener('click', async () => {
        // Get the image selected
        var image = upload.files[0];

        await submitEditItem(item["modelNumber"], ProductNameTextArea.value, 
        image, imageUrl, AvailabilityCheckbox.checked, 
        ProductUrlTextArea.value, ProductPriceInput.value);
      })
      buttonDiv.append(saveButton);
      buttonDiv.append(deleteButton);
    })

    newDivRow.appendChild(newDivImage);
    newDivRow.appendChild(newDivModelNumber);
    newDivRow.appendChild(newDivName);
    newDivRow.appendChild(newDivUrl);
    newDivRow.appendChild(newDivAvailability);
    newDivRow.appendChild(newDivPrice);
    newDivRow.appendChild(buttonDiv);

    allDivRows.appendChild(newDivRow);

    ImageUrlLarge.style.left = newDivImage.offsetLeft + 450 + 'px';
    ImageUrlLarge.style.top = newDivImage.offsetTop + 100 + 'px';
    var btnn = document.querySelector('.test2');
    btnn.addEventListener('click', () => {
      ImageUrlLarge.style.left = newDivImage.offsetLeft + 450 + 'px';
      ImageUrlLarge.style.top = newDivImage.offsetTop + 100 + 'px';
    })
    deleteButton.addEventListener('click', () => {
      ImageUrlLarge.style.left = newDivImage.x + 150 + 'px';
      ImageUrlLarge.style.top = newDivImage.y + 63 + 'px';
    })
  })
}

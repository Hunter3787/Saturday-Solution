//const uri1 = 'https://localhost:44363/api/TodoItems';
const uri ='https://localhost:5001/vendorlinking';

var tokenDanny = 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJBdXRvYnVpbGQiLCJzdWIiOiJBdXRvYnVpbGQgVXNlciIsImF1ZCI6IlVTIiwiaWF0IjoxNjE4NDUzMzM2LCJleHAiOjE2Mjk2NzcyMzYsIm5iZiI6MTYyOTY3NzIzNiwiVXNlcm5hbWUiOiJkYW5ueSIsIlVzZXJDTGFpbXMiOlt7IlBlcm1pc3Npb24iOiJDUkVBVEUiLCJzY29wZU9mUGVybWlzc2lvbnMiOiJSRVZJRVdTIn0seyJQZXJtaXNzaW9uIjoiREVMRVRFIiwic2NvcGVPZlBlcm1pc3Npb25zIjoiU0VMRiJ9LHsiUGVybWlzc2lvbiI6IkRFTEVURSIsInNjb3BlT2ZQZXJtaXNzaW9ucyI6IlNFTEZfUkVWSUVXUyJ9LHsiUGVybWlzc2lvbiI6IkVESVQiLCJzY29wZU9mUGVybWlzc2lvbnMiOiJTRUxGIn0seyJQZXJtaXNzaW9uIjoiUkVBRF9PTkxZIiwic2NvcGVPZlBlcm1pc3Npb25zIjoiQVVUT0JVSUxEIn0seyJQZXJtaXNzaW9uIjoiVVBEQVRFIiwic2NvcGVPZlBlcm1pc3Npb25zIjoiU0VMRiJ9LHsiUGVybWlzc2lvbiI6IlVQREFURSIsInNjb3BlT2ZQZXJtaXNzaW9ucyI6IlNFTEZfUkVWSUVXUyJ9LHsiUGVybWlzc2lvbiI6IkNSRUFURSIsInNjb3BlT2ZQZXJtaXNzaW9ucyI6IlBST0RVQ1RTIn0seyJQZXJtaXNzaW9uIjoiVVBEQVRFIiwic2NvcGVPZlBlcm1pc3Npb25zIjoiVkVORE9SX1BST0RVQ1RTIn0seyJQZXJtaXNzaW9uIjoiREVMRVRFIiwic2NvcGVPZlBlcm1pc3Npb25zIjoiVkVORE9SX1BST0RVQ1RTIn1dfQ.242uVukArptSKQY6mQpxH_MRdhRO0uEhDdUA4U6qJc4';
var tokenNewEgg = 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJBdXRvYnVpbGQiLCJzdWIiOiJBdXRvYnVpbGQgVXNlciIsImF1ZCI6IlVTIiwiaWF0IjoxNjE4NDUzMzM2LCJleHAiOjE2Mjk2NzcyMzYsIm5iZiI6MTYyOTY3NzIzNiwiVXNlcm5hbWUiOiJuZXcgZWdnIiwiVXNlckNMYWltcyI6W3siUGVybWlzc2lvbiI6IkNSRUFURSIsInNjb3BlT2ZQZXJtaXNzaW9ucyI6IlJFVklFV1MifSx7IlBlcm1pc3Npb24iOiJERUxFVEUiLCJzY29wZU9mUGVybWlzc2lvbnMiOiJTRUxGIn0seyJQZXJtaXNzaW9uIjoiREVMRVRFIiwic2NvcGVPZlBlcm1pc3Npb25zIjoiU0VMRl9SRVZJRVdTIn0seyJQZXJtaXNzaW9uIjoiRURJVCIsInNjb3BlT2ZQZXJtaXNzaW9ucyI6IlNFTEYifSx7IlBlcm1pc3Npb24iOiJSRUFEX09OTFkiLCJzY29wZU9mUGVybWlzc2lvbnMiOiJBVVRPQlVJTEQifSx7IlBlcm1pc3Npb24iOiJVUERBVEUiLCJzY29wZU9mUGVybWlzc2lvbnMiOiJTRUxGIn0seyJQZXJtaXNzaW9uIjoiVVBEQVRFIiwic2NvcGVPZlBlcm1pc3Npb25zIjoiU0VMRl9SRVZJRVdTIn0seyJQZXJtaXNzaW9uIjoiQ1JFQVRFIiwic2NvcGVPZlBlcm1pc3Npb25zIjoiUFJPRFVDVFMifSx7IlBlcm1pc3Npb24iOiJVUERBVEUiLCJzY29wZU9mUGVybWlzc2lvbnMiOiJWRU5ET1JfUFJPRFVDVFMifSx7IlBlcm1pc3Npb24iOiJERUxFVEUiLCJzY29wZU9mUGVybWlzc2lvbnMiOiJWRU5ET1JfUFJPRFVDVFMifV19.idn4GGfnwpOgt8tmaUTvtGHLJIF8KoHSz4dWCI07bns';
var cpuButton = document.querySelector('input[type=checkbox]');

const fetchRequest = {
  method: 'POST',
  mode: 'cors',
  headers: {
    'Accept': 'application/json',
    'Content-Type': 'application/json',
    'Authorization': 'bearer ' + tokenDanny
  }
};

function getAllModelNumbers() {

  fetch(uri + '/modelNumbers', {
    method: 'GET',
    mode: 'cors',
    headers: {
      'Accept': 'application/json',
      'Content-Type': 'application/json',
    },
  })
  .then(response => response.json())
  .then(data => SaveModelNumbers(data));
}

function fv() {
  RemoveOptions(document.querySelector('.modelNumberDropDown'));
}
function RemoveOptions(dropdown) {
  var length = dropdown.options.length - 1;
  for(var i = length; i >= 0; i--) {
    dropdown.remove(i);
  }
  var first = document.createElement('option');
  first.textContent = "Model Number";
  dropdown.appendChild(first);
}

function SaveModelNumbers(data) {
  var dropdown = document.querySelector('.modelNumberDropDown');
  RemoveOptions(dropdown);

  data.forEach(item => {
    var modelNumber = document.createElement('option');
    modelNumber.textContent = item;

    dropdown.appendChild(modelNumber);
  })
}

 async function addItem() {
  var listOfRows = document.querySelector('.allProducts');
  var newDivRow = document.createElement('div');

  var newDivImage = document.createElement('div');
  newDivImage.classList.add('div-table-col');
  newDivImage.classList.add('no-line-height');
  newDivImage.style="width:130px";

  var ImageUrlSmall = new Image(50,50);
  var ImageUrlLarge = new Image(200,200);
  newDivImage.appendChild(ImageUrlSmall);
  newDivImage.appendChild(document.createElement('br'))

  var upload = document.createElement('input');
  upload.type = 'file';
  upload.style = "width:90px; margin-top:-50px";
  newDivImage.appendChild(upload);

  upload.addEventListener('input', (async () => {
    console.log('hey');
    var photo = upload.files[0];
    const reader = new FileReader();
    reader.addEventListener("load", function() {
      ImageUrlSmall.src = reader.result;
      ImageUrlLarge.src = reader.result;
    }, false);
    if(photo) {
      reader.readAsDataURL(photo);
    }
    console.log('here first ' + photo)
  }))


  ImageUrlSmall.addEventListener('mouseover', () => {
    ImageUrlLarge.classList.toggle('displayNone');
  })
  ImageUrlSmall.addEventListener('mouseout', () => {
    ImageUrlLarge.classList.toggle('displayNone');
  })
  
  ImageUrlLarge.classList.add('largePhoto');
  ImageUrlLarge.classList.add('displayNone');

  // ImageUrlLarge.src = item["imageUrl"];
  document.body.append(ImageUrlLarge);

  // ImageUrlSmall.src = item["imageUrl"];

  // var newDivImage = document.createElement('div');
  // newDivImage.classList.add('div-table-col');
  // newDivImage.style="width:130px";
  // var ImageUrlLarge = new Image(200,200);

  // // newDivImage.addEventListener('mouseover', () => {
  // //   ImageUrlLarge.classList.toggle('displayNone');
  // // })
  // // newDivImage.addEventListener('mouseout', () => {
  // //   ImageUrlLarge.classList.toggle('displayNone');
  // // })
  
  // // ImageUrlLarge.classList.add('largePhoto');
  // // ImageUrlLarge.classList.add('displayNone');

  // // ImageUrlLarge.src = item["imageUrl"];
  // // document.body.append(ImageUrlLarge);

  // var upload = document.createElement('input');
  // upload.type = 'file';
  // upload.style = "width:90px; ";
  // newDivImage.appendChild(upload);

  // need? prob idk

  // newDivImage.src = "https://c1.neweggimages.com/ProductImage/19-113-677-V02.jpg";
  // newDivImage.classList.add('div-table-col');
  // newDivImage.classList.add('no-line-height');
  // newDivImage.style="width:130px";

  var newDivModelNumber = document.createElement('div');
  newDivModelNumber.classList.add('div-table-col');
  newDivModelNumber.style = "width:175px";
  let modelNumberDropDown = document.querySelector('.modelNumberDropDown').cloneNode(true)
  modelNumberDropDown.classList.remove('displayNone');
  newDivModelNumber.appendChild(modelNumberDropDown);

  var newDivName = document.createElement('div');
  newDivName.classList.add('div-table-col');
  newDivName.style="width:500px";

  var ProductNameTextArea = document.createElement('textArea');
  ProductNameTextArea.setAttribute('cols', 60);
  ProductNameTextArea.setAttribute('rows',3);
  newDivName.appendChild(ProductNameTextArea);

  var newDivUrl = document.createElement('div');
  newDivUrl.classList.add('div-table-col');
  newDivUrl.style="width:300px";

  var ProductUrlTextArea = document.createElement('textArea');
  ProductUrlTextArea.setAttribute('cols', 34);
  ProductUrlTextArea.setAttribute('rows',2);
  newDivUrl.appendChild(ProductUrlTextArea);

  var newDivAvailability = document.createElement('div');
  newDivAvailability.classList.add('div-table-col');
  newDivAvailability.classList.add('center');
  newDivAvailability.style="width:100px";

  var AvailabilityCheckbox = document.createElement('input');
  AvailabilityCheckbox.type = "checkbox";
  AvailabilityCheckbox.style = "  transform: scale(2);";
  newDivAvailability.appendChild(AvailabilityCheckbox);
  
  var newDivPrice = document.createElement('div');
  newDivPrice.classList.add('div-table-col');
  newDivPrice.classList.add('center');
  newDivPrice.style="width:100px";
  
  var ProductPriceInput = document.createElement('input');
  ProductPriceInput.style.width = '70px';
  var priceDiv = document.createElement('div');
  var dollarElement = document.createElement('text');
  dollarElement.textContent = '$ ';
  priceDiv.appendChild(dollarElement);
  priceDiv.appendChild(ProductPriceInput);
  newDivPrice.appendChild(priceDiv);

  var buttonDiv = document.createElement('div');
  buttonDiv.classList.add('div-table-col');
  buttonDiv.classList.add('center');
  buttonDiv.style="width:120px";
  var addItemButton = document.createElement('button');
  addItemButton.innerHTML = "add";
  addItemButton.style = "margin-right:10px";
  var cancelButton = document.createElement('button');
  cancelButton.innerHTML = "clear";
  buttonDiv.appendChild(addItemButton);
  buttonDiv.appendChild(cancelButton);

  // var imageUrl = item["imageUrl"];
  addItemButton.addEventListener('click', () => {
    // newDivRow.style ="display:none";
    var photo = upload.files[0];
    submitAddItem(modelNumberDropDown.options[modelNumberDropDown.selectedIndex].text,
      ProductNameTextArea.value, photo, AvailabilityCheckbox.checked, 
      ProductUrlTextArea.value, ProductPriceInput.value);

      // submitEditItem(item["modelNumber"], ProductNameTextArea.value, 
      // photo, imageUrl, AvailabilityCheckbox.checked, 
      // ProductUrlTextArea.value, ProductPriceInput.value);
  })

  cancelButton.addEventListener('click', () => {
    ProductNameTextArea.value = '';
    ProductUrlTextArea.value = '';
    ProductPriceInput.value = '';
    AvailabilityCheckbox.checked = false;
    modelNumberDropDown.selectedIndex = modelNumberDropDown.options[0];
    // newDivRow.style ="display:none";
  })

  newDivRow.appendChild(newDivImage);
  newDivRow.appendChild(newDivModelNumber);
  newDivRow.appendChild(newDivName);
  newDivRow.appendChild(newDivUrl);
  newDivRow.appendChild(newDivAvailability);
  newDivRow.appendChild(newDivPrice);
  newDivRow.appendChild(buttonDiv);
  
  listOfRows.prepend(newDivRow);

  // document.getElementById('fname').value = document.querySelector('.innerDiv ');
  
}

function editItem(item, newUl, tablef, modelNumber) {

  // console.log(tablef.rows.item(0).cells.item(1).innerHTML);
  var textArea1 = document.createElement('textarea');
  textArea1.setAttribute('cols', '25');
  textArea1.setAttribute('rows','6');
  textArea1.value = tablef.rows.item(0).cells.item(1).innerHTML;
  tablef.rows.item(0).deleteCell(1);
  tablef.rows.item(0).append(textArea1);

  var textArea2 = document.createElement('textarea');
  textArea2.setAttribute('cols', '25');
  textArea2.setAttribute('rows','2');
  textArea2.value = tablef.rows.item(1).cells.item(1).innerHTML;
  tablef.rows.item(1).deleteCell(1);
  tablef.rows.item(1).append(textArea2);

  var textArea3 = document.createElement('textarea');
  textArea3.setAttribute('cols', '25');
  textArea3.setAttribute('rows','1');
  textArea3.value = tablef.rows.item(2).cells.item(1).innerHTML;
  tablef.rows.item(2).deleteCell(1);
  tablef.rows.item(2).append(textArea3);

  var textArea4 = document.createElement('textarea');
  textArea4.setAttribute('cols', '25');
  textArea4.setAttribute('rows','4');
  textArea4.value = tablef.rows.item(3).cells.item(1).innerHTML;
  tablef.rows.item(3).deleteCell(1);
  tablef.rows.item(3).append(textArea4);

  var textArea5 = document.createElement('textarea');
  textArea5.setAttribute('cols', '25');
  textArea5.setAttribute('rows','1');
  textArea5.value = tablef.rows.item(5).cells.item(1).innerHTML;
  tablef.rows.item(5).deleteCell(1);
  tablef.rows.item(5).append(textArea5);

  const item2 = {
    name:"ff6",
    // imageurl:'imgurl2',
    // availability:false,
    // company:'Danny',
    // url:'url',
    modelnumber:"STRIX B550-F GAMING (WI-FI)"
    // price:434

    // name:textArea1.value,
    // imageurl:'imageurl',
    // availability:textArea2.value,
    // company:'comp',
    // url:textArea3.value,
    // modelnumber:modelNumber,
    // price:textArea5.value
  };

  fetch(uri, {
    method: 'PUT',
    mode: 'cors',
    headers: {
      'Accept': 'application/json',
      'Content-Type': 'application/json'
      // 'Authorization': 'bearer ' + tokenNewEgg
    },
    body: JSON.stringify(item2)
  })
  .then(response => response.json())
  .then(() => {
    // getItems();
  })
}

 function submitAddItem(modelNumber, name, photo, availability,
                          url, price) {
  
  var formData = new FormData();

  formData.append('modelNumber', modelNumber);
  formData.append('name', name);
  formData.append('photo', photo);
  formData.append("availability", availability);
  formData.append("url", url);
  formData.append("price", price);

   fetch(uri, {

    method: 'POST',
    mode: 'cors',
    headers: {
      'Accept': 'application/json',
      'Authorization': 'bearer ' + tokenNewEgg
    },
    body: formData
  })

  .then(response => {
    if(response.ok) {
      alert('correct');
    }
    else {
      alert('error')
    }
  })
  // .then(async function () => response => {
  //   if(response.ok) await responseResult()})
  // .then(() => new Promise())
  // .then(async () => getItemsByFilter());
}
 function responseResult(response){
  if(response.ok) {
    alert('correct');
    getItemsByFilter();
  }
  else {
    alert('error')
  }
}
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

  var x = await fetch(uri, {
    method: 'PUT',
    mode: 'cors',
    headers: {
      'Accept': 'application/json',
    },
    body:formData
  })
  .then(async response => {
    if(response.ok) {
      alert('correct');
    }
    else {
      alert('error');
    }
    await getItemsByFilter();
 })
  // .then(getItemsByFilter());
  // .then( response =>  response.json())
  // .then(() => {
  //   getItems();
  // })
  // .then(async () => await goodjob())
  
}

async function submitDeleteItem(modelNumber, newDivRow) {
  var result = confirm("Want to delete?");
  if(!result) {
    return;
  }

   fetch(uri + '?modelNumber=' +modelNumber, {
    method: 'DELETE',
    mode: 'cors',
    headers: {
      'Accept': 'application/json',
      'Content-Type': 'application/json'
    },
    // body: "100-N1-0550-L1"
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

// async function goodjob() {
//   console.log('good job');
//   setTimeout(() => {
//     console.log('u waited');
//     // button.classList.add('successButton');
//   }, 2000);
// }


var ul = document.getElementById('listOfDivs');

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

async function getItemsByFilter() {
  var extraURI = "?filtersString=";
  var priceOrder = "&order=";
  var checkboxes = document.querySelectorAll('input[type=checkbox]');
  checkboxes.forEach(checkbox => {
    if(checkbox.checked) {
      // cpu-checkbox => cpu
      extraURI += checkbox.id.split('-')[0] + ",";
    }
  })

  var sort = document.querySelectorAll('input[type=radio]');
  sort.forEach(btn => {
    if(btn.checked) {
      console.log(btn.id);

      priceOrder += btn.id;
    }
  })

  await fetch(uri + extraURI + priceOrder, {
    method: 'GET',
    mode: 'cors',
    headers: {
      'Accept': 'application/json',
      'Content-Type': 'application/json',
      'Authorization': 'bearer ' + tokenDanny
    },
  })
  // .then(ul.innerHTML='')
  .then(response => response.json())
  .then(getAllModelNumbers())
  .then(async data => await displayItemsFilter(data));
  // refreshData;
}


 async function displayItemsFilter(data) {

  ul.innerHTML = '';
  var new_ul = document.createElement('div');
  new_ul.classList.add('innerDiv');

  var divTable = document.createElement('div');
  divTable.classList.add('div-table');

  var divRow = document.createElement('div');
  divRow.classList.add('div-table-row');

  var ImageHeaderDiv = document.createElement('div');
  ImageHeaderDiv.classList.add('div-table-col');
  ImageHeaderDiv.classList.add('center');
  ImageHeaderDiv.style="width:130px";
  ImageHeaderDiv.innerHTML="Image";

  var ModelNumberHeaderDiv = document.createElement('div');
  ModelNumberHeaderDiv.classList.add('div-table-col');
  ModelNumberHeaderDiv.classList.add('center');
  ModelNumberHeaderDiv.style="width:175px";
  ModelNumberHeaderDiv.innerHTML="Model Number";

  var NameHeaderDiv = document.createElement('div');
  NameHeaderDiv.classList.add('div-table-col');
  NameHeaderDiv.classList.add('center');
  NameHeaderDiv.style="width:500px";
  NameHeaderDiv.innerHTML="Name";

  var UrlHeaderDiv = document.createElement('div');
  UrlHeaderDiv.classList.add('div-table-col');
  UrlHeaderDiv.classList.add('center');
  UrlHeaderDiv.style="width:300px";
  UrlHeaderDiv.innerHTML="Url";

  var AvailabilityHeaderDiv = document.createElement('div');
  AvailabilityHeaderDiv.classList.add('div-table-col');
  AvailabilityHeaderDiv.classList.add('center');
  AvailabilityHeaderDiv.style="width:100px";
  AvailabilityHeaderDiv.innerHTML="Availability";

  var PriceHeaderDiv = document.createElement('div');
  PriceHeaderDiv.classList.add('div-table-col');
  PriceHeaderDiv.classList.add('center');
  PriceHeaderDiv.style="width:100px";
  PriceHeaderDiv.innerHTML="Price";

  var ButtonHeaderDiv = document.createElement('div');
  ButtonHeaderDiv.classList.add('div-table-col');
  ButtonHeaderDiv.style="width:120px";
  // ButtonHeaderDiv.innerHTML="f";

  divRow.appendChild(ImageHeaderDiv);
  divRow.appendChild(ModelNumberHeaderDiv);
  divRow.appendChild(NameHeaderDiv);
  divRow.appendChild(UrlHeaderDiv);
  divRow.appendChild(AvailabilityHeaderDiv);
  divRow.appendChild(PriceHeaderDiv);
  divRow.appendChild(ButtonHeaderDiv);

  divTable.appendChild(divRow);

  new_ul.appendChild(divTable);
  ul.append(new_ul);

  var allDivRows = document.createElement('div');
  allDivRows.classList.add('allProducts');
  divTable.appendChild(allDivRows);
   addItem();

  data.forEach(item => {
    var newDivRow = document.createElement('div');

    var newDivImage = document.createElement('div');
    newDivImage.classList.add('div-table-col');
    newDivImage.classList.add('no-line-height');
    newDivImage.style="width:130px";

    var ImageUrlSmall = new Image(50,50);
    var ImageUrlLarge = new Image(200,200);
    newDivImage.appendChild(ImageUrlSmall);
    newDivImage.appendChild(document.createElement('br'))

    var upload = document.createElement('input');
    upload.type = 'file';
    upload.style = "width:90px; margin-top:-50px";
    newDivImage.appendChild(upload);

    upload.addEventListener('input', (() => {
      console.log('hey');
      var photo = upload.files[0];
      const reader = new FileReader();
      reader.addEventListener("load", function() {
        ImageUrlSmall.src = reader.result;
        ImageUrlLarge.src = reader.result;
      }, false);
      if(photo) {
        reader.readAsDataURL(photo);
      }
    }))


    ImageUrlSmall.addEventListener('mouseover', () => {
      ImageUrlLarge.classList.toggle('displayNone');
    })
    ImageUrlSmall.addEventListener('mouseout', () => {
      ImageUrlLarge.classList.toggle('displayNone');
    })
    
    ImageUrlLarge.classList.add('largePhoto');
    ImageUrlLarge.classList.add('displayNone');

    ImageUrlLarge.src = item["imageUrl"];
    document.body.append(ImageUrlLarge);

    ImageUrlSmall.src = item["imageUrl"];

    var newDivModelNumber = document.createElement('div');
    newDivModelNumber.classList.add('div-table-col');
    newDivModelNumber.classList.add('header');

    // newDivModelNumber.style = "line-height: 15px";
    newDivModelNumber.style="width:175px";

    var ModelNumberText = document.createElement('text');
    ModelNumberText.innerHTML = item["modelNumber"];
    // ModelNumberText.style = "margin-top: 50px";
    newDivModelNumber.appendChild(ModelNumberText);

    var newDivName = document.createElement('div');
    newDivName.classList.add('div-table-col');
    newDivName.style="width:500px";

    var ProductNameText = document.createElement('text');
    ProductNameText.innerHTML = item["name"];
    newDivName.append(ProductNameText);
    
    var ProductNameTextArea = document.createElement('textArea');
    ProductNameTextArea.setAttribute('cols', 60);
    ProductNameTextArea.setAttribute('rows',3);
    ProductNameTextArea.innerHTML = item["name"];
    // newDivName.appendChild(ProductNameTextArea);

    var newDivUrl = document.createElement('div');
    newDivUrl.classList.add('div-table-col');
    newDivUrl.style="width:300px";

    var UrlText = document.createElement('a');
    UrlText.innerHTML = item["url"];
    UrlText.href = item["url"];
    newDivUrl.append(UrlText);

    var ProductUrlTextArea = document.createElement('textArea');
    ProductUrlTextArea.setAttribute('cols', 34);
    ProductUrlTextArea.setAttribute('rows',3);
    ProductUrlTextArea.innerHTML = item["url"];

    var newDivAvailability = document.createElement('div');
    newDivAvailability.classList.add('div-table-col');
    newDivAvailability.classList.add('center');
    newDivAvailability.style="width:100px";

    var AvailabilityCheckbox = document.createElement('input');
    AvailabilityCheckbox.type = "checkbox";
    AvailabilityCheckbox.style = "  transform: scale(2);";
    if(item["availability"] == true) {
      AvailabilityCheckbox.checked = true;
    }
    newDivAvailability.appendChild(AvailabilityCheckbox);    

    var newDivPrice = document.createElement('div');
    newDivPrice.classList.add('div-table-col');
    newDivPrice.classList.add('center');
    newDivPrice.style="width:100px";

    var ProductPriceText = document.createElement('text');
    ProductPriceText.innerHTML = item["price"].toFixed(2);

    var ProductPriceInput = document.createElement('input');
    ProductPriceInput.classList.add('center');
    ProductPriceInput.style.width = '70px';
    ProductPriceInput.value = item["price"].toFixed(2);

    var priceDiv = document.createElement('div');
    var dollarElement = document.createElement('text');
    dollarElement.textContent = '$'
    priceDiv.appendChild(dollarElement);
    priceDiv.appendChild(ProductPriceText);
    newDivPrice.appendChild(priceDiv);

    var buttonDiv = document.createElement('div');
    buttonDiv.classList.add('div-table-col');
    buttonDiv.classList.add('center');
    buttonDiv.style="width:120px";
    var editButton = document.createElement('button');
    editButton.innerHTML = "Edit";
    editButton.style = "margin-right:10px";
    buttonDiv.appendChild(editButton);

    editButton.addEventListener('click', () => {
      newDivName.removeChild(ProductNameText);
      // newDivName.innerHTML = '';
      newDivUrl.removeChild(UrlText);
      // newDivUrl.innerHTML = '';
      priceDiv.removeChild(ProductPriceText);
      newDivName.append(ProductNameTextArea);
      newDivUrl.appendChild(ProductUrlTextArea);
      priceDiv.appendChild(ProductPriceInput);

      // newDivRow.style ="display:none";
      var photo = upload.files[0];
      // const reader = new FileReader();
      // reader.addEventListener("load", function() {
      //   ImageUrlSmall.src = reader.result;
      //   if(photo) {
      //     reader.readAsDataURL(photo);
      //   }
      // }, false);

      console.log(photo);
      var imageUrl = item["imageUrl"];

      console.log(imageUrl);
      var saveButton = document.createElement('button');
      saveButton.onsubmit = "return false";
      saveButton.innerHTML = "Save";
      saveButton.style = "margin-right:10px";
      buttonDiv.innerHTML = '';

      saveButton.addEventListener('click', async () => {
        await submitEditItem(item["modelNumber"], ProductNameTextArea.value, 
        photo, imageUrl, AvailabilityCheckbox.checked, 
        ProductUrlTextArea.value, ProductPriceInput.value);
      })
      // buttonDiv.appendChild(saveButton);
      buttonDiv.append(saveButton);
      buttonDiv.append(deleteButton);

    })

    var deleteButton = document.createElement('button');
    deleteButton.innerHTML = "Delete";
    buttonDiv.appendChild(deleteButton);
    deleteButton.addEventListener('click', async () => {
      await submitDeleteItem(item["modelNumber"], newDivRow);
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


// correct one...but it has tables
function displayItemsFilter2(data) {
  var table = document.createElement('table');

  var tableRow = document.createElement('tr');

  var ImageUrlColumn = document.createElement('td');
  ImageUrlColumn.innerHTML = 'Image Url';
  ImageUrlColumn.style = 'width: 50px';

  var NameColumn = document.createElement('td');
  NameColumn.innerHTML = 'Name';
  NameColumn.style = 'width:500px';

  var UrlColumn = document.createElement('td');
  UrlColumn.innerHTML = 'Url';
  UrlColumn.style='width:500px';

  var AvailabilityColumn = document.createElement('td');
  AvailabilityColumn.innerHTML = 'Availability';
  AvailabilityColumn.style='width:100px';

  var PriceColumn = document.createElement('td');
  PriceColumn.innerHTML = 'Price';
  PriceColumn.style='width:100px';

  tableRow.append(ImageUrlColumn);
  tableRow.append(NameColumn);
  tableRow.append(UrlColumn);
  tableRow.append(AvailabilityColumn);
  tableRow.append(PriceColumn);
  table.append(tableRow);

  var new_ul = document.createElement('div');
  new_ul.classList.add('innerDiv');
  new_ul.appendChild(table);
  ul.append(new_ul);


  data.forEach(item => {
    var newRow = document.createElement('tr');

    var ImageUrl = document.createElement('td');
    ImageUrl.classList.add('photo');
    var ImageUrl2 = new Image(50,50);
    var ImageUrlLarge = new Image(200,200);
    ImageUrl.addEventListener('mouseover', () => {
      ImageUrlLarge.classList.toggle('displayNone');
    })
    ImageUrl.addEventListener('mouseout', () => {
      ImageUrlLarge.classList.toggle('displayNone');
    })

    ImageUrlLarge.classList.add('largePhoto');
    ImageUrlLarge.classList.add('displayNone');

    ImageUrlLarge.src = item["imageUrl"];
    document.body.append(ImageUrlLarge);
    // ImageUrlLarge.style.position = "absolute"

    ImageUrl.appendChild(ImageUrl2);

    ImageUrl2.src = item["imageUrl"];

    var ProductName = document.createElement('td');
    var ProductNameTextArea = document.createElement('textArea');
    ProductNameTextArea.setAttribute('cols', 50);
    ProductNameTextArea.setAttribute('rows','3');
    ProductNameTextArea.innerHTML = item["name"];
    ProductName.appendChild(ProductNameTextArea);

    var ProductUrl = document.createElement('td');
    var ProductUrlTextArea = document.createElement('textArea');
    ProductUrlTextArea.setAttribute('cols', 50);
    ProductUrlTextArea.setAttribute('rows','3');
    ProductUrlTextArea.innerHTML = item["url"];
    ProductUrl.appendChild(ProductUrlTextArea);

    var Availability = document.createElement('td');
    var AvailabilityCheckbox = document.createElement('input');
    AvailabilityCheckbox.type = "checkbox";
    if(item["availability"] == true) {
      AvailabilityCheckbox.checked = true;
    }
    Availability.appendChild(AvailabilityCheckbox);
    // Availability.innerHTML = item["availability"];

    var Price = document.createElement('td');
    Price.innerHTML = item["price"];

    newRow.append(ImageUrl);
    newRow.append(ProductName);
    newRow.append(ProductUrl);
    newRow.append(Availability);
    newRow.append(Price);

    // ImageUrlLarge.style.right = 900 + 90 + 'px';
    
    table.append(newRow);
    console.log(ImageUrl2.left +' ' +  ImageUrl2.x);
    console.log(ImageUrl2.top + ' ' +  ImageUrl2.y);



    ImageUrlLarge.style.left = ImageUrl2.x + 150 + 'px';
    ImageUrlLarge.style.top = ImageUrl2.y - 67.5 + 'px';
  })
}

// function displayItemsFilter(data) {
//   data.forEach(item => {
//       console.log(item);
//       var new_ul = document.createElement('div');
//       new_ul.classList.add('innerDiv');
//       var ol = document.createElement('ol');
//       // var img = document.createElement('img');
//       var img = new Image(250, 250);
//       img.src = item["imageUrl"];
//       ol.appendChild(img);
//       new_ul.appendChild(ol);

//       var table = document.createElement('table');

//       var tableRow = document.createElement('tr');
//       var tableDataKey = document.createElement('td');
//       var tableDataValue = document.createElement('td');
//       tableRow.append(tableDataKey);
//       tableRow.append(tableDataValue);
//       tableDataKey.innerHTML = 'name';
//       tableDataValue.innerHTML = item["name"];
//       table.appendChild(tableRow);

//       tableRow = document.createElement('tr');
//       tableDataKey = document.createElement('td');
//       tableDataValue = document.createElement('td');
//       tableRow.append(tableDataKey);
//       tableRow.append(tableDataValue);
//       tableDataKey.innerHTML = 'availability';
//       tableDataValue.innerHTML = item["availability"];
//       table.appendChild(tableRow);

//       tableRow = document.createElement('tr');
//       tableDataKey = document.createElement('td');
//       tableDataValue = document.createElement('td');
//       tableRow.append(tableDataKey);
//       tableRow.append(tableDataValue);
//       tableDataKey.innerHTML = 'company';
//       tableDataValue.innerHTML = item["company"];
//       table.appendChild(tableRow);

//       tableRow = document.createElement('tr');
//       tableDataKey = document.createElement('td');
//       tableDataValue = document.createElement('td');
//       tableRow.append(tableDataKey);
//       tableRow.append(tableDataValue);
//       tableDataKey.innerHTML = 'url';
//       tableDataValue.innerHTML = item["url"];
//       table.appendChild(tableRow);

//       tableRow = document.createElement('tr');
//       tableDataKey = document.createElement('td');
//       tableDataValue = document.createElement('td');
//       tableRow.append(tableDataKey);
//       tableRow.append(tableDataValue);
//       tableDataKey.innerHTML = 'modelNumber';
//       var modelNumber = item["modelNumber"];
//       tableDataValue.innerHTML = modelNumber;
//       table.appendChild(tableRow);

//       tableRow = document.createElement('tr');
//       tableDataKey = document.createElement('td');
//       tableDataValue = document.createElement('td');
//       tableRow.append(tableDataKey);
//       tableRow.append(tableDataValue);
//       tableDataKey.innerHTML = 'price';
//       tableDataValue.innerHTML = item["price"];
//       table.appendChild(tableRow);
//       // ol = document.createElement('ol');
//       // ol.appendChild(document.createTextNode('name: ' + item["name"]));
//       // div.appendChild(ol);

//       // ol = document.createElement('ol');
//       // ol.appendChild(document.createTextNode('availability: ' + item["availability"]));
//       // div.appendChild(ol);

//       // ol = document.createElement('ol');
//       // ol.appendChild(document.createTextNode('company: ' + item["company"]));
//       // div.appendChild(ol);

//       // ol = document.createElement('ol');
//       // ol.appendChild(document.createTextNode('url: ' + item["url"]));
//       // div.appendChild(ol);
      
//       // ol = document.createElement('ol');
//       // ol.appendChild(document.createTextNode('modelNumber: ' + item["modelNumber"]));
//       // div.appendChild(ol);

//       // ol = document.createElement('ol');
//       // ol.appendChild(document.createTextNode('price: ' + item["price"]));
//       // div.appendChild(ol);
//       // new_ul.appendChild(div);
//       new_ul.appendChild(table);
//       // ol.appendChild(document.createTextNode(item["company"]));
//       ul.appendChild(new_ul);
//       var btn = document.createElement('button');
//       btn.addEventListener('click', function() {
//         editItem(item, new_ul, table, modelNumber);
//       });
//       btn.innerHTML = "Edit";
//       new_ul.appendChild(btn);
//   })
// }

// function displayItemsFilter(data) {
//   data.forEach(item => {
//       console.log(item);
//       var new_ul = document.createElement('div');
//       new_ul.classList.add('innerDiv');
//       var ol = document.createElement('ol');
//       // var img = document.createElement('img');
//       var img = new Image(250, 250);
//       img.src = item["imageUrl"];
//       ol.appendChild(img);
//       new_ul.appendChild(ol);

//       var div = document.createElement('div');
//       ol = document.createElement('ol');
//       ol.appendChild(document.createTextNode('name: ' + item["name"]));
//       div.appendChild(ol);

//       ol = document.createElement('ol');
//       ol.appendChild(document.createTextNode('availability: ' + item["availability"]));
//       div.appendChild(ol);

//       ol = document.createElement('ol');
//       ol.appendChild(document.createTextNode('company: ' + item["company"]));
//       div.appendChild(ol);

//       ol = document.createElement('ol');
//       ol.appendChild(document.createTextNode('url: ' + item["url"]));
//       div.appendChild(ol);
      
//       ol = document.createElement('ol');
//       ol.appendChild(document.createTextNode('modelNumber: ' + item["modelNumber"]));
//       div.appendChild(ol);

//       ol = document.createElement('ol');
//       ol.appendChild(document.createTextNode('price: ' + item["price"]));
//       div.appendChild(ol);
//       new_ul.appendChild(div);
//       // ol.appendChild(document.createTextNode(item["company"]));
//       ul.appendChild(new_ul);
//       var btn = document.createElement('button');
//       btn.addEventListener('click', function() {
//         editItem(item, new_ul, div);
//       });
//       btn.innerHTML = "Edit";
//       console.log('yoo');
//       ul.appendChild(btn);
//   })
// }

// let customRequest = Object.assign(fetchRequest, { method: 'GET' })

// customRequest.body(JSON.stringify(item));

// fetch(customRequest)


function myFunction() {
  var x = document.getElementById("add-passHash");
  if (x.type === "password") {
    x.type = "text";
  } else {
    x.type = "password";
  }
} 
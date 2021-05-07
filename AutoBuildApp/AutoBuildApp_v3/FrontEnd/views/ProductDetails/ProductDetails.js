const uri ='https://localhost:5001/productdetails';

const fetchRequest = {
    method: 'GET',
    mode: 'cors',
    headers: {
      'Accept': 'application/json',
      'Content-Type': 'application/json'
    }
  };

  window.onload = getProductDetails('100-100000071BOX')

  async function getProductDetails(modelNumber) {
      await fetch(uri + '/' + modelNumber)
      .then(response => response.json())
      .then(data => displayProductDetails(data));
  }

  function displayProductDetails(data) {
    console.log(data);
    for (var key in specs) {
        console.log(key + " - " + specs[key])
    }

    // Get the product title div and set it
    var productTitle = document.querySelector('.product-title');
    productTitle.innerHTML = data["productName"]

    // Get the product image div and set it
    var imageDiv = document.querySelector('.product-image');
    var image = new Image(300, 300);
    image.src = data["imageUrl"];
    imageDiv.appendChild(image);

    // Get the average rating div and set it
    var averageRating = document.querySelector('.average-rating');
    averageRating.innerHTML = 'Average rating: ' + data["averageRating"];

    // Get the total reviews div and set it
    var totalReviews = document.querySelector('.total-reviews');
    totalReviews.innerHTML = 'Total Reviews: ' + data["totalReviews"];

    // Get the side nav bar
    var sideNavigationBar = document.querySelector('.side-nav');

    // Get the specs for the product
    var specs = data["specs"];

    // For each key value in the specs, add it to the nav bar
    for(var key in specs) {
        var h3 = document.createElement('h3');
        h3.innerHTML = key;

        var h4 = document.createElement('text');
        h4.innerHTML = specs[key];
        sideNavigationBar.appendChild(h3);
        sideNavigationBar.appendChild(h4);
    }

    // Hacky way to add space at the end of the side nav
    sideNavigationBar.appendChild(document.createElement('br'))
    sideNavigationBar.appendChild(document.createElement('br'))
    sideNavigationBar.appendChild(document.createElement('br'))
    sideNavigationBar.appendChild(document.createElement('br'))
    sideNavigationBar.appendChild(document.createElement('br'))
    sideNavigationBar.appendChild(document.createElement('br'))
    sideNavigationBar.appendChild(document.createElement('br'))

    // Div that represents all vendor information
    var vendorInformationTable = document.querySelector('.vendor-information-table');

    // Get the vendorInformation attribute in the json object
    var vendorInformation = data["vendorInformation"];

    // Loop through each key-value pair in the vendor information attribute
    //      to get each vendor's information regarding the product
    for(var vendor in vendorInformation) {

        // Create a new row div for the vendor
        var vendorRowDiv = document.createElement('div');

        // Create vendor name div
        var vendorNameDiv = document.createElement('div');
        vendorNameDiv.classList.add('vendor-information-row-div');
        vendorNameDiv.classList.add('vendor-name');

        // Create vendor name text
        var vendorNameText = document.createElement('text');
        vendorNameText.innerText = vendor;

        // Append vendor text to the vendor name div
        vendorNameDiv.append(vendorNameText);

        // Append the vendor name div to the row
        vendorRowDiv.appendChild(vendorNameDiv);

        var currentVendor = vendorInformation[vendor];

        // Create vendor price div
        var vendorPriceDiv = document.createElement('div');
        vendorPriceDiv.classList.add('vendor-information-row-div');
        vendorPriceDiv.classList.add('vendor-name');

        // Create vendor price text
        var vendorPriceText = document.createElement('text');
        vendorPriceText.innerText = '$' + currentVendor["price"];
        vendorPriceDiv.appendChild(vendorPriceText);

        vendorRowDiv.appendChild(vendorPriceDiv);

        // Create vendor availability div
        var vendorAvailabilityDiv = document.createElement('div');
        vendorAvailabilityDiv.classList.add('vendor-information-row-div');
        vendorAvailabilityDiv.classList.add('vendor-name');

        // Create vendor availability text
        var vendorAvailabilityText = document.createElement('text');
        vendorAvailabilityText.innerText = currentVendor["availability"];
        vendorAvailabilityDiv.appendChild(vendorAvailabilityText);

        if(currentVendor["availability"] == false) {
            var notifyMeButton = document.createElement('button');
            notifyMeButton.innerText = "Notify me!";

            notifyMeButton.addEventListener('click', async () => {
                await submitAddEmailToNotificationList();
            })
            vendorAvailabilityDiv.appendChild(document.createElement('br'))
            vendorAvailabilityDiv.appendChild(notifyMeButton);
        }

        vendorRowDiv.appendChild(vendorAvailabilityDiv);

        // Create vendor product link div
        var vendorProductLinkDiv = document.createElement('div');
        vendorProductLinkDiv.classList.add('vendor-information-row-div');
        vendorProductLinkDiv.classList.add('vendor-link');

        // Create vendor product link text
        var vendorProductLinkText = document.createElement('a');
        vendorProductLinkText.innerText = "Buy!";
        vendorProductLinkText.href =  currentVendor["url"];
        vendorProductLinkDiv.appendChild(vendorProductLinkText);

        vendorRowDiv.appendChild(vendorProductLinkDiv);

        vendorInformationTable.append(vendorRowDiv);
    }

  }

  async function submitAddEmailToNotificationList() {
      await fetch(uri + '/emailSubmit', fetchRequest)
      .then(response => {
          if(response.ok) {
              alert('successfully added email to email list');
          }
          else {
              alert('error');
          }
      })
  }
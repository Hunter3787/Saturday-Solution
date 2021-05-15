const uri ='http://localhost:8081/productdetails';
var tokenDanny = 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJBdXRvYnVpbGQiLCJzdWIiOiJBdXRvYnVpbGQgVXNlciIsImF1ZCI6IlVTIiwiaWF0IjoxNjE4NDUzMzM2LCJleHAiOjE2Mjk2NzcyMzYsIm5iZiI6MTYyOTY3NzIzNiwiVXNlcm5hbWUiOiJkYW5ueSIsIlVzZXJDTGFpbXMiOlt7IlBlcm1pc3Npb24iOiJDUkVBVEUiLCJzY29wZU9mUGVybWlzc2lvbnMiOiJSRVZJRVdTIn0seyJQZXJtaXNzaW9uIjoiREVMRVRFIiwic2NvcGVPZlBlcm1pc3Npb25zIjoiU0VMRiJ9LHsiUGVybWlzc2lvbiI6IkRFTEVURSIsInNjb3BlT2ZQZXJtaXNzaW9ucyI6IlNFTEZfUkVWSUVXUyJ9LHsiUGVybWlzc2lvbiI6IkVESVQiLCJzY29wZU9mUGVybWlzc2lvbnMiOiJTRUxGIn0seyJQZXJtaXNzaW9uIjoiUkVBRF9PTkxZIiwic2NvcGVPZlBlcm1pc3Npb25zIjoiQVVUT0JVSUxEIn0seyJQZXJtaXNzaW9uIjoiVVBEQVRFIiwic2NvcGVPZlBlcm1pc3Npb25zIjoiU0VMRiJ9LHsiUGVybWlzc2lvbiI6IlVQREFURSIsInNjb3BlT2ZQZXJtaXNzaW9ucyI6IlNFTEZfUkVWSUVXUyJ9LHsiUGVybWlzc2lvbiI6IkNSRUFURSIsInNjb3BlT2ZQZXJtaXNzaW9ucyI6IlBST0RVQ1RTIn0seyJQZXJtaXNzaW9uIjoiVVBEQVRFIiwic2NvcGVPZlBlcm1pc3Npb25zIjoiVkVORE9SX1BST0RVQ1RTIn0seyJQZXJtaXNzaW9uIjoiREVMRVRFIiwic2NvcGVPZlBlcm1pc3Npb25zIjoiVkVORE9SX1BST0RVQ1RTIn1dfQ.242uVukArptSKQY6mQpxH_MRdhRO0uEhDdUA4U6qJc4';
var tokenNewEgg = 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJBdXRvYnVpbGQiLCJzdWIiOiJBdXRvYnVpbGQgVXNlciIsImF1ZCI6IlVTIiwiaWF0IjoxNjE4NDUzMzM2LCJleHAiOjE2Mjk2NzcyMzYsIm5iZiI6MTYyOTY3NzIzNiwiVXNlcm5hbWUiOiJuZXcgZWdnIiwiVXNlckNMYWltcyI6W3siUGVybWlzc2lvbiI6IkNyZWF0ZSIsIlNjb3BlT2ZQZXJtaXNzaW9ucyI6IlJldmlld3MifSx7IlBlcm1pc3Npb24iOiJEZWxldGUiLCJTY29wZU9mUGVybWlzc2lvbnMiOiJTZWxmIn0seyJQZXJtaXNzaW9uIjoiRGVsZXRlIiwiU2NvcGVPZlBlcm1pc3Npb25zIjoiU2VsZlJldmlld3MifSx7IlBlcm1pc3Npb24iOiJFZGl0IiwiU2NvcGVPZlBlcm1pc3Npb25zIjoiU2VsZiJ9LHsiUGVybWlzc2lvbiI6IlJlYWRPbmx5IiwiU2NvcGVPZlBlcm1pc3Npb25zIjoiQXV0b0J1aWxkIn0seyJQZXJtaXNzaW9uIjoiVXBkYXRlIiwiU2NvcGVPZlBlcm1pc3Npb25zIjoiU2VsZiJ9LHsiUGVybWlzc2lvbiI6IlVwZGF0ZSIsIlNjb3BlT2ZQZXJtaXNzaW9ucyI6IlNlbGZSZXZpZXdzIn0seyJQZXJtaXNzaW9uIjoiQ3JlYXRlIiwiU2NvcGVPZlBlcm1pc3Npb25zIjoiUHJvZHVjdHMifSx7IlBlcm1pc3Npb24iOiJVcGRhdGUiLCJTY29wZU9mUGVybWlzc2lvbnMiOiJWZW5kb3JQcm9kdWN0cyJ9LHsiUGVybWlzc2lvbiI6IkRlbGV0ZSIsIlNjb3BlT2ZQZXJtaXNzaW9ucyI6IlZlbmRvclByb2R1Y3RzIn1dfQ.MQyT1fFd2VZjFlxX0RiEhpLk4liae6xuPdpewqRDpZg';

var currentToken = tokenNewEgg;

let jwt_token = getCookie("JWT");

const fetchRequest = {
    method: 'GET',
    mode: 'cors',
    headers: {
      'Accept': 'application/json',
      'Content-Type': 'application/json',
      'Authorization': 'bearer ' + currentToken
    }
  };

  var modelNumber = sessionStorage.getItem('modelNumber');
  if(modelNumber == null) {
    window.location.assign("../ComponentCatalog/ComponentCatalog.html")
  }
  window.onload = getProductDetails(modelNumber);

  async function getProductDetails(modelNumber) {
      await fetch(uri + '/' + modelNumber)
      .then(response => {
          if(!response.ok) {
              alert("That model number doesn't exist")
          }
          return response.json()
      })
      .then(data => displayProductDetails(data));
  }

  function displayProductDetails(data) {

    // Get the product title div and set it
    var productTitle = document.querySelector('.product-title');
    productTitle.innerHTML = data["productName"]

    // Get the product image div and set it
    var imageDiv = document.querySelector('.product-image');
    imageDiv.style = "margin-bottom:50px"
    var image = new Image(300, 300);
    image.src = data["imageUrl"];
    imageDiv.appendChild(image);

    // Get the average rating div and set it
    var averageRatingAndTotalRatings = document.querySelector('.average-rating-and-total-reviews');

    // Get the total reviews div and set it
    var totalReviews = document.querySelector('.total-reviews');
    
    var stars = document.querySelector('.stars');
    stars.style = "margin-right:10px"
    var ratingRounded = Math.round(data["averageRating"]);

    for(var i = 0; i < ratingRounded; i++) {
        stars.children[i].classList.add('checked');
    }

    averageRatingAndTotalRatings.appendChild(stars);

    var totalReviewsText = document.createElement('text');
    totalReviewsText.style="font-size:30px"
    totalReviewsText.innerHTML = '(' + data["totalReviews"] + ')';

    averageRatingAndTotalRatings.appendChild(totalReviewsText);
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

    // Boolean that tracks whether a product is available
    var productIsAvailable = false;

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

        // If the product is available for a vendor, set productIsAvailable to true
        if(currentVendor["availability"] == true) {
            productIsAvailable = true;
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

    // If no vendor has the product in stock, display the notify me button and listen for a click which sends
    //      the request to the back end to add their email to en email list
    if(productIsAvailable == false) {
        var notifyMeButton = document.querySelector('.notify-me-button');

        // Show the button
        notifyMeButton.style ="display:table";
    
        notifyMeButton.addEventListener('click', async () => {
            await submitAddEmailToNotificationList(data["modelNumber"]);
        })
    }


  }

  async function submitAddEmailToNotificationList(modelNumber) {

    await fetch(uri + '/emailSubmit/' + modelNumber, {
        method: 'POST',
        mode: 'cors',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization': 'bearer ' + currentToken
        }
    })
    .then(response => {
        if(response.ok) {
            alert('successfully added email to email list');
        }
        else {
            alert("You've already been added to this email list!");
        }
    })
  }

  function hideButtons() {
    var x = document.getElementById("profilePage");
    var y = document.getElementById("loginPage");
    var z = document.getElementById("registrationPage");
    if (jwt_token != "") {
      y.style.display = "none";
      z.style.display = "none";
    } else {
      x.style.display = "none";
    }
  }
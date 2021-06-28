const uri ='http://ec2-13-52-186-63.us-west-1.compute.amazonaws.com:8081/productdetails';

let jwt_token = getCookie("JWT");

const fetchRequest = {
    method: 'GET',
    mode: 'cors',
    headers: {
      'Accept': 'application/json',
      'Content-Type': 'application/json',
      'Authorization': 'bearer ' + jwt_token
    }
  };

  var modelNumber = sessionStorage.getItem('modelNumber');
  if(modelNumber == null) {
    window.location.assign("../ComponentCatalog/ComponentCatalog.html")
  }

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
            'Authorization': 'bearer ' + jwt_token
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

  var profilePage = document.getElementById("profilePage")

  profilePage.addEventListener('click', async () => {
    if (jwt_token == "" || jwt_token == "Invalid username or password" || jwt_token == "Account is locked") {
      alert("You are not logged in");
    } else {
      if (await checkAdminPrivilege() == true) {
        changePageAdmin();
      } else {
        changePageNotAdmin();
      }
    }
  })
  
  async function checkAdminPrivilege() {
    var result = false;
    var url = "http://ec2-13-52-186-63.us-west-1.compute.amazonaws.com:8081/authentication/admin"
    await fetch(url, {
      method: 'POST',
      mode: 'cors',
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json',
        'Authorization': 'bearer ' + jwt_token
      },
    })
    .then(response => response.json())
      .then(data => result = data);
      return result;
  }
  
  function getCookie(cname) {
      var jwt = "";
      var name = cname + "=";
      var decodedCookie = decodeURIComponent(document.cookie);
      var ca = decodedCookie.split(';');
      for(var i = 0; i <ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') {
          c = c.substring(1);
        }
        if (c.indexOf(name) == 0) {
          jwt =  c.substring(name.length, c.length);
          jwt = jwt.replace(/"/g,"");
          return jwt;
        }
      }
      return "";
    }
  
  function changePageAdmin() {
    window.location.href = "/views/UMUser(Admin)/UMUser(Admin).html"
  }
  
  function changePageNotAdmin() {
    window.location.href = "/views/UMUser/UMUser.html"
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

getProductDetails(modelNumber);

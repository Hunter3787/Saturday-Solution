let jwt_token = getCookie("JWT")

const uri ='http://http://ec2-13-52-186-63.us-west-1.compute.amazonaws.com:8081/componentcatalog';

const fetchRequest = {
    method: 'GET',
    mode: 'cors',
    headers: {
      'Accept': 'application/json',
      'Content-Type': 'application/json',
    }
  };

// Event listeners to upload the display when a filter is changed
var checkboxes = document.querySelectorAll('input[type=checkbox]');
checkboxes.forEach(checkbox => checkbox.addEventListener('click', getProductsByFilter));
var radioButtons = document.querySelectorAll('input[type=radio]');
radioButtons.forEach(button => button.addEventListener('click', getProductsByFilter));


function getProductsByFilter() {
    var minimumValueInput = document.querySelector('.minimum-value-input');
    var maximumValueInput = document.querySelector('.maximum-value-input');

    // console.log(minimumValueInput);
    var filtersQueryParameter = "?filtersString=";
    var sortQueryParameter = "&order=";
    var minPriceParameter = "&minimumPrice=";
    var maxPriceParameter = "&maximumPrice=";
    var checkboxes = document.querySelectorAll('input[type=checkbox]');

    // For every checkbox, if it's checked, split the id and get the first split.
    // Append to the filters query parameter
    //    e.g. power supply-button => 'power supply'
    checkboxes.forEach(checkbox => {
      if(checkbox.checked) {
        filtersQueryParameter += checkbox.id.split('-')[0] + ",";
      }
    })

    var sort = document.querySelectorAll('input[type=radio]');
    sort.forEach(btn => {
      if(btn.checked) {
        sortQueryParameter += btn.id;
      }
    })

    if(minimumValueInput.value == '') {
        minimumValueInput = 0;
    }
    else {
        minimumValueInput = minimumValueInput.value;
    }

    if(maximumValueInput.value == '') {
        maximumValueInput = 2000000000;
    }
    else {
        maximumValueInput = maximumValueInput.value;
    }

    minPriceParameter += minimumValueInput;
    maxPriceParameter += maximumValueInput;

    console.log(minPriceParameter);
    console.log(maxPriceParameter);
      fetch(uri + filtersQueryParameter + sortQueryParameter 
        + minPriceParameter + maxPriceParameter, fetchRequest)
      .then(response => response.json())
      .then(data => displayProducts(data))
  }

  function displayProducts(data) {

    // var listOfDivs = document.querySelector('.list-of-divs');

    var productTable = document.querySelector('.product-table');
    productTable.innerHTML = '';
    data.forEach(product => {

        var productDiv = document.createElement('div');
        productDiv.classList.add('new-product')
        // Create the new image div
        var newDivImage = document.createElement('div');
        // newDivImage.style = "line-height: 10px; width: 130px";

        newDivImage.addEventListener('mouseover', () => {
            newDivImage.classList.add('pointer');
        })

        newDivImage.addEventListener('click', () => {
            sessionStorage.setItem('modelNumber', product["modelNumber"]);
            window.location.assign("../ProductDetails/ProductDetails.html")
        })

        // Create the images and append the small image to the image div
        var productImage = new Image(250,250);
        newDivImage.appendChild(productImage);
        newDivImage.style = "margin-bottom:30px; margin-top:20px";
        productImage.src = product["imageUrl"];

        // New name div
        var newDivName = document.createElement('div');
        newDivName.classList.add('name-div');
        // newDivName.style="width:500px";

        // Product name text
        var ProductNameText = document.createElement('text');
        ProductNameText.innerHTML = product["name"];
        ProductNameText.style = "font-size:18px";
        newDivName.append(ProductNameText);

        ProductNameText.addEventListener('mouseover', () => {
            ProductNameText.classList.add('pointer');
        })

        ProductNameText.addEventListener('click', () => {
            sessionStorage.setItem('modelNumber', product["modelNumber"]);
            window.location.assign("../ProductDetails/ProductDetails.html")
        })
        
        var ratingAndReviewsDiv = document.createElement('div')
        ratingAndReviewsDiv.style = "margin-bottom:20px";
        var ratingTextDiv = document.createElement('text');

        var stars = document.querySelector('.stars').cloneNode(true);
        var ratingRounded = Math.round(product["averageRating"]);

        for(var i = 0; i < ratingRounded; i++) {
            stars.children[i].classList.add('checked');
        }

        ratingTextDiv.appendChild(stars);

        ratingTextDiv.style="margin-right:10px"
        ratingAndReviewsDiv.appendChild(ratingTextDiv);

        // var reviewsDiv = document.createElement('div');
        var reviewsTextDiv = document.createElement('text');
        reviewsTextDiv.style = 'font-size:20px'
        reviewsTextDiv.innerHTML = '(' + product["totalReviews"] + ')';
        ratingAndReviewsDiv.appendChild(reviewsTextDiv);

        // ratingAndReviewsDiv.appendChild(ratingDiv);
        // ratingAndReviewsDiv.appendChild(reviewsDiv);

        // var priceDiv = document.createElement('div');

        var priceText = document.createElement('text');
        priceText.style = "font-size:30px; font-weight: bold"
        priceText.innerHTML = '$' + product["price"].toFixed(2)
        
        productDiv.appendChild(newDivImage);
        productDiv.appendChild(ratingAndReviewsDiv);
        productDiv.appendChild(newDivName);

        productDiv.appendChild(priceText);

        productTable.appendChild(productDiv);
    })
    var listOfProducts = document.createElement('div');
    var text = document.createElement('text');
    text.innerHTML = "hey";
    var text2 = document.createElement('text');
    text2.innerHTML = "hey";
    listOfProducts.appendChild(text);
    listOfProducts.appendChild(text2);
    // listOfDivs.appendChild(listOfProducts);
  }

  async function submitGetProducts() {
      var minimumValue = document.querySelector('.minimum-value-input');
      console.log(minimumValue.value);
      var maximumValue = document.querySelector('.maximum-value-input')
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
var url = "http://http://ec2-13-52-186-63.us-west-1.compute.amazonaws.com:8081/authentication/admin"
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

getProductsByFilter();
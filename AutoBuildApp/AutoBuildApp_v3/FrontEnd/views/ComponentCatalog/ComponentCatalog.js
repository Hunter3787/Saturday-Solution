const uri ='https://localhost:5001/componentcatalog';

const fetchRequest = {
    method: 'GET',
    mode: 'cors',
    headers: {
      'Accept': 'application/json',
      'Content-Type': 'application/json',
    }
  };

  window.onload = getProductsByFilter();

  async function getProductsByFilter() {
      fetch(uri, fetchRequest)
      .then(response => response.json())
      .then(data => displayProducts(data))
  }

  function displayProducts(data) {
    var listOfDivs = document.querySelector('.list-of-divs');

    data.forEach(product => {
        var product = document.createElement('div');

        // Create the new image div
        var newDivImage = document.createElement('div');
        newDivImage.classList.add('div-table-row');
        newDivImage.style = "line-height: 10px; width: 130px";

        // Create the images and append the small image to the image div
        var ImageUrlSmall = new Image(100,100);
        newDivImage.appendChild(ImageUrlSmall);
        newDivImage.appendChild(document.createElement('br'))

        product.appendChild(newDivImage);

        listOfDivs.appendChild(product);
    })
    var listOfProducts = document.createElement('div');
    var text = document.createElement('text');
    text.innerHTML = "hey";
    var text2 = document.createElement('text');
    text2.innerHTML = "hey";
    listOfProducts.appendChild(text);
    listOfProducts.appendChild(text2);
    listOfDivs.appendChild(listOfProducts);
  }

  async function submitGetProducts() {
      var minimumValue = document.querySelector('.minimum-value-input');
      console.log(minimumValue.value);
      var maximumValue = document.querySelector('.maximum-value-input')
  }


const uri ='https://localhost:5001/productdetails';

const fetchRequest = {
    method: 'GET',
    mode: 'cors',
    headers: {
      'Accept': 'application/json',
      'Content-Type': 'application/json',
      'Authorization': 'bearer ' + currentToken
    }
  };

  window.onload = getProductDetails('100-100000071BOX')

  async function getProductDetails(modelNumber) {
      await fetch(uri + '/' + modelNumber)
      .then(response => response.json())
      .then(data => displayProductDetails(data));
  }

  function displayProductDetails(data) {
    
  }
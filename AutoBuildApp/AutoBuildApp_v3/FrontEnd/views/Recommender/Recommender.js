const uri ='http://localhost:8081/Recommendation/BuildRecommend';

let jwt_token = getCookie('JWT').toString();


const fetchRequest = {
    method: 'POST',
    mode: 'cors',
    headers: {
      'Accept': 'application/json',
      'Content-Type': 'application/json',
    }
  };

// window.onload = getProductsByFilter();

function getBuild() {

    var budget = document.querySelector('.budget');
    const item = {
        "Budget":parseInt(budget.value)
    }

    fetch(uri, {
        method: 'POST',
        mode: 'cors',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(item)
    })
    .then(response => response.json())
    .then(data => displayBuild(data))
}


function displayBuild(data) {
    // document.querySelector('.build-table').innerHTML = '';
    console.log(data);

    var totalPrice = 0;
    var gpu = document.querySelector('.gpu');
    var gpuTD = document.createElement('td');
    gpu.appendChild(gpuTD);

    var gpuPriceTD = document.createElement('td');
    gpuPriceTD.innerHTML = "$" + data[0]["gpu"]["price"].toFixed(2);
    totalPrice += data[0]["gpu"]["price"];
    gpu.appendChild(gpuPriceTD);

    var gpuDiv = document.createElement('div');
    gpuTD.appendChild(gpuDiv);
    gpuDiv.style ="width:300px;padding:25px";

    var gpuTitle = document.createElement('text');
    gpuTitle.textContent = data[0]["gpu"]["productName"];

    var gpuImage = new Image(150,150);
    gpuImage.src = data[0]["gpu"]["productImageStrings"][0]
    
    gpuDiv.appendChild(gpuImage);
    gpuDiv.appendChild(document.createElement('br'))
    gpuDiv.appendChild(gpuTitle);

//
    var cpu = document.querySelector('.cpu');
    var cpuTD = document.createElement('td');
    cpu.appendChild(cpuTD);

    var cpuPriceTD = document.createElement('td');
    cpuPriceTD.innerHTML = "$" + data[0]["cpu"]["price"].toFixed(2);
    totalPrice += data[0]["cpu"]["price"];
    cpu.appendChild(cpuPriceTD);

    var cpuDiv = document.createElement('div');
    cpuTD.appendChild(cpuDiv);
    cpuDiv.style ="width:300px;padding:25px";

    var cpuTitle = document.createElement('text');
    cpuTitle.textContent = data[0]["cpu"]["productName"];

    var cpuImage = new Image(150,150);
    cpuImage.src = data[0]["cpu"]["productImageStrings"][0]

    cpuDiv.appendChild(cpuImage);
    cpuDiv.appendChild(document.createElement('br'))
    cpuDiv.appendChild(cpuTitle);
//
    var motherboard = document.querySelector('.motherboard');
    var motherboardTD = document.createElement('td');
    motherboard.appendChild(motherboardTD);

    var motherboardPriceTD = document.createElement('td');
    motherboardPriceTD.innerHTML = "$" + data[0]["mobo"]["price"].toFixed(2);
    totalPrice += data[0]["mobo"]["price"];
    motherboard.appendChild(motherboardPriceTD);

    var motherboardDiv = document.createElement('div');
    motherboardTD.appendChild(motherboardDiv);
    motherboardDiv.style ="width:300px;padding:25px";

    var motherboardTitle = document.createElement('text');
    motherboardTitle.textContent = data[0]["mobo"]["productName"];

    var motherboardImage = new Image(150,150);
    motherboardImage.src = data[0]["mobo"]["productImageStrings"][0]

    motherboardDiv.appendChild(motherboardImage);
    motherboardDiv.appendChild(document.createElement('br'))
    motherboardDiv.appendChild(motherboardTitle);

//
    var psu = document.querySelector('.psu');
    var psuTD = document.createElement('td');
    psu.appendChild(psuTD);

    var psuPriceTD = document.createElement('td');
    psuPriceTD.innerHTML = "$" + data[0]["psu"]["price"].toFixed(2);
    totalPrice += data[0]["psu"]["price"];
    psu.appendChild(psuPriceTD);

    var psuDiv = document.createElement('div');
    psuTD.appendChild(psuDiv);
    psuDiv.style ="width:300px;padding:25px";

    var psuTitle = document.createElement('text');
    psuTitle.textContent = data[0]["psu"]["productName"];

    var psuImage = new Image(150,150);
    psuImage.src = data[0]["psu"]["productImageStrings"][0]

    psuDiv.appendChild(psuImage);
    psuDiv.appendChild(document.createElement('br'))
    psuDiv.appendChild(psuTitle);
 
    //
    var ssd = document.querySelector('.ssd');
    var ssdTD = document.createElement('td');
    ssd.appendChild(ssdTD);

    var ssdPriceTD = document.createElement('td');
    ssdPriceTD.innerHTML = "$" + data[0]["hardDrives"][0]["price"].toFixed(2);
    totalPrice += data[0]["hardDrives"][0]["price"];
    ssd.appendChild(ssdPriceTD);

    var ssdDiv = document.createElement('div');
    ssdTD.appendChild(ssdDiv);
    ssdDiv.style ="width:300px;padding:25px";

    var ssdTitle = document.createElement('text');
    ssdTitle.textContent = data[0]["hardDrives"][0]["productName"];

    var ssdImage = new Image(150,150);
    ssdImage.src = data[0]["hardDrives"][0]["productImageStrings"][0]

    ssdDiv.appendChild(ssdImage);
    ssdDiv.appendChild(document.createElement('br'))
    ssdDiv.appendChild(ssdTitle);

    //
    var ram = document.querySelector('.ram');
    var ramTD = document.createElement('td');
    ram.appendChild(ramTD);

    var ramPriceTD = document.createElement('td');
    ramPriceTD.innerHTML = "$" + data[0]["ram"]["price"].toFixed(2);
    totalPrice += data[0]["ram"]["price"];
    ram.appendChild(ramPriceTD);

    var ramDiv = document.createElement('div');
    ramTD.appendChild(ramDiv);
    ramDiv.style ="width:300px;padding:25px";

    var ramTitle = document.createElement('text');
    ramTitle.textContent = data[0]["ram"]["productName"];

    var ramImage = new Image(150,150);
    ramImage.src = data[0]["ram"]["productImageStrings"][0]

    ramDiv.appendChild(ramImage);
    ramDiv.appendChild(document.createElement('br'))
    ramDiv.appendChild(ramTitle);

    //
    var _case = document.querySelector('.case');
    var _caseTD = document.createElement('td');
    _case.appendChild(_caseTD);

    var _casePriceTD = document.createElement('td');
    _casePriceTD.innerHTML = "$" + data[0]["case"]["price"].toFixed(2);
    totalPrice += data[0]["case"]["price"];
    _case.appendChild(_casePriceTD);

    var _caseDiv = document.createElement('div');
    _caseTD.appendChild(_caseDiv);
    _caseDiv.style ="width:300px;padding:25px";

    var _caseTitle = document.createElement('text');
    _caseTitle.textContent = data[0]["case"]["productName"];

    var _caseImage = new Image(150,150);
    _caseImage.src = data[0]["case"]["productImageStrings"][0]

    _caseDiv.appendChild(_caseImage);
    _caseDiv.appendChild(document.createElement('br'))
    _caseDiv.appendChild(_caseTitle);

    document.querySelector('.total-price').innerHTML = "$" + totalPrice.toFixed(2);
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
  var url = "http://localhost:8081/authentication/admin"
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
  window.location.href = "http://127.0.0.1:5501/views/UMUser(Admin)/UMUser(Admin).html"
}

function changePageNotAdmin() {
  window.location.href = "http://127.0.0.1:5501/views/UMUser/UMUser.html"
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

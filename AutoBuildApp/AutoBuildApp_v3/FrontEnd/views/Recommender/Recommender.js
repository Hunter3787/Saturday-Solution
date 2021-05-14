const uri ='http://localhost:8081/Recommendation/BuildRecommend';

const fetchRequest = {
    method: 'POST',
    mode: 'cors',
    headers: {
      'Accept': 'application/json',
      'Content-Type': 'application/json',
    }
  };


var form = document.getElementById("build-budget-form");
form.addEventListener("submit", () => getBuild())

// window.onload = getProductsByFilter();

function getBuild() {

    var budget = document.getElementById("budget");


    console.log(budget.value)

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

    console.log(data)

    // <div class="items-container">
    //     <div class="item">
    //         <div class="item-type">
    //             <h4>GPU</h4>
    //         </div>
    //         <div class="item-image">
    //             <img src="Logo.png" alt="" srcset="">
    //         </div>
    //         <div class="item-title">
    //             <h4>GPVHOSO RX 560 4G Video Card PC Gaming Graphics Card AMD Radeon GPU GA Fan Edition, Low Profile 4G/128bit/GDDR5 PCI-Express 3.0x8 DirectX 12,DVI-D HDMI DP Desktop Graphics Card</h4>    
    //         </div>
    //         <div class="item-price">
    //             <h4>$1000</h4>
    //         </div>
    //     </div>
    // </div>

    var main = document.getElementById('items-container');
    main.innerHTML = '';

    var totalPrice = 0;

    var fragment = document.createDocumentFragment()

    // GPU
    var gpu = document.createElement('div');
    gpu.classList.add('item');

    var gpuName = document.createElement('div');
    gpuName.classList.add('item-type');
    var gpuNameText = document.createElement('h4');
    gpuNameText.innerHTML = "GPU";
    gpuName.appendChild(gpuNameText);
    gpu.appendChild(gpuName);

    var gpuImage = document.createElement('div');
    gpuImage.classList.add('item-image');
    var gpuImageSrc = document.createElement('img');
    gpuImageSrc.src = data[0]["gpu"]["productImageStrings"][0]
    gpuImage.appendChild(gpuImageSrc);
    gpu.appendChild(gpuImage);

    var gpuTitle = document.createElement('div');
    gpuTitle.classList.add('item-title');
    var gpuTitleText = document.createElement('h4');
    gpuTitleText.innerHTML = data[0]["gpu"]["productName"];
    gpuTitle.appendChild(gpuTitleText);
    gpu.appendChild(gpuTitle);

    var gpuPrice = document.createElement('div');
    gpuPrice.classList.add('item-price');
    var gpuPriceText = document.createElement('h4');
    gpuPriceText.innerHTML = "$" + data[0]["gpu"]["price"].toFixed(2);
    totalPrice += data[0]["gpu"]["price"];
    gpuPrice.appendChild(gpuPriceText);
    gpu.appendChild(gpuPrice);

    main.appendChild(gpu);

    // CPU
    var cpu = document.createElement('div');
    cpu.classList.add('item');

    var cpuName = document.createElement('div');
    cpuName.classList.add('item-type');
    var cpuNameText = document.createElement('h4');
    cpuNameText.innerHTML = "CPU";
    cpuName.appendChild(cpuNameText);
    cpu.appendChild(cpuName);

    var cpuImage = document.createElement('div');
    cpuImage.classList.add('item-image');
    var cpuImageSrc = document.createElement('img');
    cpuImageSrc.src = data[0]["cpu"]["productImageStrings"][0]
    cpuImage.appendChild(cpuImageSrc);
    cpu.appendChild(cpuImage);

    var cpuTitle = document.createElement('div');
    cpuTitle.classList.add('item-title');
    var cpuTitleText = document.createElement('h4');
    cpuTitleText.innerHTML = data[0]["cpu"]["productName"];
    cpuTitle.appendChild(cpuTitleText);
    cpu.appendChild(cpuTitle);

    var cpuPrice = document.createElement('div');
    cpuPrice.classList.add('item-price');
    var cpuPriceText = document.createElement('h4');
    cpuPriceText.innerHTML = "$" + data[0]["cpu"]["price"].toFixed(2);
    totalPrice += data[0]["cpu"]["price"];
    cpuPrice.appendChild(cpuPriceText);
    cpu.appendChild(cpuPrice);

    main.appendChild(cpu);

    // Motherboard
    var motherboard = document.createElement('div');
    motherboard.classList.add('item');
    
    var motherboardName = document.createElement('div');
    motherboardName.classList.add('item-type');
    var motherboardNameText = document.createElement('h4');
    motherboardNameText.innerHTML = "Motherboard";
    motherboardName.appendChild(motherboardNameText);
    motherboard.appendChild(motherboardName);
    
    var motherboardImage = document.createElement('div');
    motherboardImage.classList.add('item-image');
    var motherboardImageSrc = document.createElement('img');
    motherboardImageSrc.src = data[0]["mobo"]["productImageStrings"][0]
    motherboardImage.appendChild(motherboardImageSrc);
    motherboard.appendChild(motherboardImage);
    
    var motherboardTitle = document.createElement('div');
    motherboardTitle.classList.add('item-title');
    var motherboardTitleText = document.createElement('h4');
    motherboardTitleText.innerHTML = data[0]["mobo"]["productName"];
    motherboardTitle.appendChild(motherboardTitleText);
    motherboard.appendChild(motherboardTitle);
    
    var motherboardPrice = document.createElement('div');
    motherboardPrice.classList.add('item-price');
    var motherboardPriceText = document.createElement('h4');
    motherboardPriceText.innerHTML = "$" + data[0]["mobo"]["price"].toFixed(2);
    totalPrice += data[0]["mobo"]["price"];
    motherboardPrice.appendChild(motherboardPriceText);
    motherboard.appendChild(motherboardPrice);
    
    main.appendChild(motherboard);

    // PSU
    var psu = document.createElement('div');
    psu.classList.add('item');
    
    var psuName = document.createElement('div');
    psuName.classList.add('item-type');
    var psuNameText = document.createElement('h4');
    psuNameText.innerHTML = "Power Supply";
    psuName.appendChild(psuNameText);
    psu.appendChild(psuName);
    
    var psuImage = document.createElement('div');
    psuImage.classList.add('item-image');
    var psuImageSrc = document.createElement('img');
    psuImageSrc.src = data[0]["psu"]["productImageStrings"][0]
    psuImage.appendChild(psuImageSrc);
    psu.appendChild(psuImage);
    
    var psuTitle = document.createElement('div');
    psuTitle.classList.add('item-title');
    var psuTitleText = document.createElement('h4');
    psuTitleText.innerHTML = data[0]["psu"]["productName"];
    psuTitle.appendChild(psuTitleText);
    psu.appendChild(psuTitle);
    
    var psuPrice = document.createElement('div');
    psuPrice.classList.add('item-price');
    var psuPriceText = document.createElement('h4');
    psuPriceText.innerHTML = "$" + data[0]["psu"]["price"].toFixed(2);
    totalPrice += data[0]["psu"]["price"];
    psuPrice.appendChild(psuPriceText);
    psu.appendChild(psuPrice);
    
    main.appendChild(psu);

    // SSD
    var ssd = document.createElement('div');
    ssd.classList.add('item');
    
    var ssdName = document.createElement('div');
    ssdName.classList.add('item-type');
    var ssdNameText = document.createElement('h4');
    ssdNameText.innerHTML = "Storage";
    ssdName.appendChild(ssdNameText);
    ssd.appendChild(ssdName);
    
    var ssdImage = document.createElement('div');
    ssdImage.classList.add('item-image');
    var ssdImageSrc = document.createElement('img');
    ssdImageSrc.src = data[0]["hardDrives"][0]["productImageStrings"][0]
    ssdImage.appendChild(ssdImageSrc);
    ssd.appendChild(ssdImage);
    
    var ssdTitle = document.createElement('div');
    ssdTitle.classList.add('item-title');
    var ssdTitleText = document.createElement('h4');
    ssdTitleText.innerHTML = data[0]["hardDrives"][0]["productName"];
    ssdTitle.appendChild(ssdTitleText);
    ssd.appendChild(ssdTitle);
    
    var ssdPrice = document.createElement('div');
    ssdPrice.classList.add('item-price');
    var ssdPriceText = document.createElement('h4');
    ssdPriceText.innerHTML = "$" + data[0]["hardDrives"][0]["price"].toFixed(2);
    totalPrice += data[0]["hardDrives"][0]["price"];
    ssdPrice.appendChild(ssdPriceText);
    ssd.appendChild(ssdPrice);
    
    main.appendChild(ssd);

    // Ram
    var ram = document.createElement('div');
    ram.classList.add('item');
    
    var ramName = document.createElement('div');
    ramName.classList.add('item-type');
    var ramNameText = document.createElement('h4');
    ramNameText.innerHTML = "Power Supply";
    ramName.appendChild(ramNameText);
    ram.appendChild(ramName);
    
    var ramImage = document.createElement('div');
    ramImage.classList.add('item-image');
    var ramImageSrc = document.createElement('img');
    ramImageSrc.src = data[0]["ram"]["productImageStrings"][0]
    ramImage.appendChild(ramImageSrc);
    ram.appendChild(ramImage);
    
    var ramTitle = document.createElement('div');
    ramTitle.classList.add('item-title');
    var ramTitleText = document.createElement('h4');
    ramTitleText.innerHTML = data[0]["ram"]["productName"];
    ramTitle.appendChild(ramTitleText);
    ram.appendChild(ramTitle);
    
    var ramPrice = document.createElement('div');
    ramPrice.classList.add('item-price');
    var ramPriceText = document.createElement('h4');
    ramPriceText.innerHTML = "$" + data[0]["ram"]["price"].toFixed(2);
    totalPrice += data[0]["ram"]["price"];
    ramPrice.appendChild(ramPriceText);
    ram.appendChild(ramPrice);
    
    main.appendChild(ram);

    // Case
    var pcCase = document.createElement('div');
    pcCase.classList.add('item');
    
    var pcCaseName = document.createElement('div');
    pcCaseName.classList.add('item-type');
    var pcCaseNameText = document.createElement('h4');
    pcCaseNameText.innerHTML = "Memory";
    pcCaseName.appendChild(pcCaseNameText);
    pcCase.appendChild(pcCaseName);
    
    var pcCaseImage = document.createElement('div');
    pcCaseImage.classList.add('item-image');
    var pcCaseImageSrc = document.createElement('img');
    pcCaseImageSrc.src = data[0]["case"]["productImageStrings"][0]
    pcCaseImage.appendChild(pcCaseImageSrc);
    pcCase.appendChild(pcCaseImage);
    
    var pcCaseTitle = document.createElement('div');
    pcCaseTitle.classList.add('item-title');
    var pcCaseTitleText = document.createElement('h4');
    pcCaseTitleText.innerHTML = data[0]["case"]["productName"];
    pcCaseTitle.appendChild(pcCaseTitleText);
    pcCase.appendChild(pcCaseTitle);
    
    var pcCasePrice = document.createElement('div');
    pcCasePrice.classList.add('item-price');
    var pcCasePriceText = document.createElement('h4');
    pcCasePriceText.innerHTML = "$" + data[0]["case"]["price"].toFixed(2);
    totalPrice += data[0]["case"]["price"];
    pcCasePrice.appendChild(pcCasePriceText);
    pcCase.appendChild(pcCasePrice);
    
    main.appendChild(pcCase);

    // Total Price
    var totalPriceDiv = document.createElement('div');
    totalPriceDiv.classList.add('total-price')
    var totalPriceText = document.createElement('h2');
    totalPriceText.innerHTML = "Total: " + "$" + totalPrice.toFixed(2);
    totalPriceDiv.appendChild(totalPriceText);

    main.appendChild(totalPriceDiv);

    console.log(totalPriceDiv);

//     document.querySelector('.total-price').innerHTML = "$" + totalPrice.toFixed(2);
}
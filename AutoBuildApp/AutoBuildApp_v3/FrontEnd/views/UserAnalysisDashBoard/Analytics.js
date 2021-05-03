// instantiation of the fetch URL
const uri ='https://localhost:5001/analytics';
let JWT_TOKEN = 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJBdXRvYnVpbGQiLCJzdWIiOiJBdXRvYnVpbGQgVXNlciIsImF1ZCI6IlVTIiwiaWF0IjoxNjE5NTI1NDkyLCJleHAiOjE2MjAxMzAyOTIsIm5iZiI6MTYyMDEzMDI5MiwiVXNlcm5hbWUiOiJraW5nUGVuaTM5MyIsIlVzZXJDTGFpbXMiOlt7IlBlcm1pc3Npb24iOiJBTEwiLCJzY29wZU9mUGVybWlzc2lvbnMiOiJBTEwifV19.77P2PjnKf9aVF8rAF7ybhpwnUe6b7ug-xlttfefelFw'


let ana =
document.getElementById("GetAnalytics");
ana.addEventListener("click", GetAnalytics);



const fetchRequest = {
  method: 'GET',
  mode: 'cors',
headers: {
  'Accept': 'application/json',
  'Content-Type': 'application/json',
  'Authorization': 'Bearer ' + JWT_TOKEN
}
};


/*    
 function process(){
    getItems();
  }
  
  function looping(){
    setTimeout(process, 3000);
  }
   */
  //var refreshData = setInterval(looping, 3000);


 function GetAnalytics() {
  console.log('here')
  document.getElementById("Analytics").innerHTML = "Hello World";

  let customRequest = Object.assign( fetchRequest,{ method: 'GET' });

  fetch(uri, customRequest)
    //body: JSON.stringify()
    .then(response => response.json())
    .then(response => DisplayData(response))
    .then(response =>  renderAnalyticsData(response.json()))
    .catch(error => console.error('Unable to get items.', error));
    //refreshData;
}

function renderAnalyticsData(response)
{
let data = response;

data.forEach( data =>
  {

  });



}


// creating a javascript object:
var chartDatas = {
      xLabel  : "N/A",
      yValue  : "N/A",
      legend  : "N/A"
  }

var Charts = {
  chartTitle : " ",
  yAxisTitle : " ",
  xAxisTitle : " ",
   legend    : "NONE",
  chartType  :   2,
  xScale     :   2,
  yScale     :   2,
  chartDatas : Object.keys(chartDatas),
}
let graphData = [];

function DisplayData(graphData)
{
  console.log(graphData)
 //var data = JSON.stringify(graphData);
 var data = JSON.parse(graphData);
    const tBody = document.getElementById("GetAnalytics"); 
    // This will get the id of the form from the HTML.
    tBody.innerHTML = 'here'; 
    // appends a null value to the inner HTML, as is not required.

    var text =  document.createTextNode(JSON.stringify(graphData));
    tBody.appendChild(text);
    console.log("here2");


    const item = 
    graphData.find(item => item["analyticChartsRequisted"] === id.toString()); // finds the item that the id is associated with.

    item.forEach(element => {
      console.log("here3");
      tBody.appendChild(element.chartTitle)


      var chartDatas = element.chartDatas
      console.log('Title')
      console.log(element.chartTitle)
      console.log('x,y,z')
      chartDatas.forEach(
        ele=> { 
            console.log(ele.xLabel)
            console.log(ele.yValue)
            console.log(ele.legend)
        });

      
    });
   

}

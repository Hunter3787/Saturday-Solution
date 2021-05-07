// instantiation of the fetch URL
const uri ='https://localhost:5001/analytics';
let JWT_TOKEN = 
'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJBdXRvYnVpbGQiLCJzdWIiOiJBdXRvYnVpbGQgVXNlciIsImF1ZCI6IlVTIiwiaWF0IjoxNjIwMzg4Nzk3LCJleHAiOjE2MjA5OTM1OTcsIm5iZiI6MTYyMDk5MzU5NywiVXNlcm5hbWUiOiJraW5nUGVuaTM5MyIsIlVzZXJDTGFpbXMiOlt7IlBlcm1pc3Npb24iOiJBbGwiLCJTY29wZU9mUGVybWlzc2lvbnMiOiJBbGwifV19.uOuiOwuHH-qdYYj99W_mnblJz_LIu145FT4NRE9zeug'

let ana =
document.getElementById("GetAnalytics");
ana.addEventListener("click",() => getItems());

const fetchRequest = {
  method: 'GET',
  mode: 'cors',
headers: {
  'Accept': 'application/json',
  'Content-Type': 'application/json',
  'Authorization': 'Bearer ' + JWT_TOKEN
}
};

const urlAnalytics = new URL("https://localhost:5001/analytics")

async function FetchGraphData(GraphType)
{
  GraphType = 1;
  urlAnalytics.searchParams.append('GraphType', GraphType);
  let customRequest1 = Object.assign( fetchRequest,{ method: 'GET' });
  try{

    let response = await fetch(urlAnalytics, customRequest1);
    let data = await response.json();

    if(response.status === 200){
       console.log(response.status);
      console.log(data);
      
      return await response.json();
    }
    
  }
  catch(error)
  {
    console.log(error);
  }

}

// This function will call a fetch request.
async function getItems() {

  let GraphType = 1;
  urlAnalytics.searchParams.append('GraphType', GraphType);
  let customRequest1 = Object.assign( fetchRequest,{ method: 'GET' });
  await fetch(urlAnalytics, customRequest1) // fetches the default URI
      .then(response => response.json()) // Will receive a response from the default response.json.
      .then(data =>  DisplayAnalyticsData(data))
}



// creating a javascript object:
const chartDatas = {
  xLabel  : "N/A",
  yValue  : "N/A",
  legend  : "N/A"
}

const Charts = {
chartTitle : " ",
yAxisTitle : " ",
xAxisTitle : " ",
legend    : "NONE",
chartType  :   2,
xScale     :   2,
yScale     :   2,
chartDatas : Object.keys(chartDatas),
}


async function DisplayAnalyticsData(chart)
{
    //let chart = await FetchGraphData();
    const tBody = document.getElementById("Bar2"); 
    // This will get the id of the form from the HTML.
    tBody.innerHTML = ' here '; 
    // appends a null value to the inner HTML, as is not required.
    

    chart['chartDatas'].forEach(  item =>{





      Object.entries(item).forEach(([key, value]) => {
        console.log(`${key} ${value}`);

        var blockchart = document.createElement('div');
        blockchart.classList.add('blockchart');
  
          // create a title element, appends text to it, 
          //and then appends all to a build block.
          var title = document.createElement('p');
          var titletext = document.createTextNode("chart Datas "+ item);
          title.appendChild(titletext);
          blockchart.appendChild(title);






    });
    console.log('-------------------');


     


    });













    var text =  document.createTextNode(JSON.stringify(chart));
    tBody.appendChild(text);


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

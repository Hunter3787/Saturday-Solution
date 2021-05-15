// instantiation of the fetch URL
const uri ='https://http://ec2-13-52-186-63.us-west-1.compute.amazonaws.com:5001/analytics';

let jwt_token = getCookie("JWT");

// let ana = document.getElementById("GetAnalytics");
// ana.addEventListener("click",() => getBarItems());


const fetchRequest = {
method: 'GET',
mode: 'cors',
headers: {
'Accept': 'application/json',
'Content-Type': 'application/json',
'Authorization': 'Bearer ' + jwt_token
}
};

const urlAnalytics = new URL("https://http://ec2-13-52-186-63.us-west-1.compute.amazonaws.com:5001/analytics")

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
 async function getBarItems()
{

  //await getItems(1);
  //await getItems(2);
  //await getItems(5);
  //DisplayLineData(4);
  //DisplayLineData(5);

}

// This function will call a fetch request.
async function getItems(GraphType) {

let customRequest = Object.assign( fetchRequest,{ method: 'GET' });
urlAnalytics.searchParams.append('GraphType', GraphType);

 await fetch(urlAnalytics, customRequest) // fetches the default URI
                .then(response => response.json())
                .then(async data =>   await DisplayAnalyticsData(data, GraphType)); // Will receive a response from the default response.json.
              // .then(data =>  DisplayAnalyticsData(data, GraphType));

              // await the return 
        //return await fetchResponse;

}

// creating a javascript object:
const Charts = {
chartTitle : " ",
yAxisTitle : " ",
xAxisTitle : " ",
legend    : "NONE",
chartType :   1,
xScale     :  1,
yScale     :  1,
};

const points = {
  x  : [],
  y  : [],
  legend: [],
  type : 'bar'
  };


async function DisplayAnalyticsData(data,GraphType)
{

console.log('---------DisplayAnalyticsData----------');
  //let chart = await FetchGraphData();
  const innerDiv= document.getElementById("Bar1"); 
  // This will get the id of the form from the HTML.
  innerDiv.innerHTML = ' here\n'; 

    Charts.chartTitle = data['chartTitle'];
    Charts.xAxisTitle = data['xAxisTitle'];
    Charts.yAxisTitle = data['yAxisTitle'];
    Charts.legend = data['legend'];
    console.log('---------Charts ----------');
    console.log(Charts);

      data["chartDatas"].forEach( item =>{

        let xvalues =
            points.x.push(item['xLabel']);
          
            points.y.push(item['yValue']);

            points.legend.push(item['legend']);

      });
      var blockchart = document.createElement('div');
          blockchart.classList.add('blockchart');
        
          var title = document.createElement('p');
          var titletext = document.createTextNode(points.legend);
          title.appendChild(titletext);
          blockchart.appendChild(title);

          innerDiv.appendChild(blockchart);

          
    console.log('here i am ')
    console.log(points.x)
    var trace2 = {
      x: points.x,
      y: points.y,
      type: 'bar',
      text: points.legend.map(String),
      textposition: 'auto',
      hoverinfo: 'none',
      marker: {
        color: 'rgb(158,202,225)',
        opacity: 0.6,
        line: {
          color: 'rgb(8,48,107)',
          width: 1.5
        }
      }
    };
    var data2 = [trace2];
    var layout = {
      title: Charts.chartTitle,
      barmode: 'group'
    };
 await Plotly.newPlot('Graph'+ GraphType, data2, layout);

}


async function DisplayLineData(GraphType)
{

let data = await getItems(GraphType)

console.log('---------DisplayAnalyticsData----------');
  //let chart = await FetchGraphData();
  const innerDiv= document.getElementById("Bar1"); 
  // This will get the id of the form from the HTML.
  innerDiv.innerHTML = ' here\n'; 

    Charts.chartTitle = data['chartTitle'];
    Charts.xAxisTitle = data['xAxisTitle'];
    Charts.yAxisTitle = data['yAxisTitle'];
    Charts.legend = data['legend'];
    console.log('---------Charts ----------');
    console.log(Charts);

      data["chartDatas"].forEach( item =>{

            points.x.push(item['xLabel']);
          
            points.y.push(item['yValue']);

            points.legend.push(item['legend']);

      });
      var blockchart = document.createElement('div');
          blockchart.classList.add('blockchart');
        
          var title = document.createElement('p');
          var titletext = document.createTextNode(points.legend);
          title.appendChild(titletext);
          blockchart.appendChild(title);

          innerDiv.appendChild(blockchart);

    console.log(points)
    console.log('here i am ')
    var trace2 = {
      x: points.x,
      mode: 'lines',  
      y: points.y,
      type: 'scatter',
      text: points.legend.map(String),
      textposition: 'auto',
      hoverinfo: 'none',
      marker: {
        color: 'rgb(158,202,225)',
        opacity: 0.6,
        line: {
          color: 'rgb(8,48,107)',
          width: 1.5
        }
      }
    };
    var data2 = [trace2];
    var layout = {
      title: Charts.chartTitle,
      barmode: 'group',
      xaxis: {
              title: Charts.xAxisTitle,
              titlefont: {
              family: 'Arial, sans-serif',
              size: 18,
              color: 'lightgrey'
              }
              },
      yaxis: {
              title: Charts.yAxisTitle,
              titlefont: {
              family: 'Arial, sans-serif',
              size: 18,
              color: 'lightgrey',
              }
          }
          
          };
 Plotly.newPlot('Graph'+ GraphType, data2, layout);

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
    console.log(result)
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
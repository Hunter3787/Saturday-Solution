const uri = 'https://localhost:5001/UserGarage/';
let posts = [];
let token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJBdXRvYnVpbGQiLCJzdWIiOiJBdXRvYnVpbGQgVXNlciIsImF1ZCI6IlVTIiwiaWF0IjoxNjIwNDM3OTkyLCJleHAiOjE2MjEwNDI3OTIsIm5iZiI6MTYyMTA0Mjc5MiwiVXNlcm5hbWUiOiJaZWluYSIsIlVzZXJDTGFpbXMiOlt7IlBlcm1pc3Npb24iOiJDcmVhdGUiLCJTY29wZU9mUGVybWlzc2lvbnMiOiJSZXZpZXdzIn0seyJQZXJtaXNzaW9uIjoiRGVsZXRlIiwiU2NvcGVPZlBlcm1pc3Npb25zIjoiU2VsZiJ9LHsiUGVybWlzc2lvbiI6IkRlbGV0ZSIsIlNjb3BlT2ZQZXJtaXNzaW9ucyI6IlNlbGZSZXZpZXdzIn0seyJQZXJtaXNzaW9uIjoiRWRpdCIsIlNjb3BlT2ZQZXJtaXNzaW9ucyI6IlNlbGYifSx7IlBlcm1pc3Npb24iOiJSZWFkT25seSIsIlNjb3BlT2ZQZXJtaXNzaW9ucyI6IkF1dG9CdWlsZCJ9LHsiUGVybWlzc2lvbiI6IlVwZGF0ZSIsIlNjb3BlT2ZQZXJtaXNzaW9ucyI6IlNlbGYifSx7IlBlcm1pc3Npb24iOiJVcGRhdGUiLCJTY29wZU9mUGVybWlzc2lvbnMiOiJTZWxmUmV2aWV3cyJ9XX0.Jtkm9Gh9TCQfpQkjD7keZ2gd2G19H7xbo95MM5QB1_4";
const fetchRequst = {
    method: 'GET',
    mode: 'cors',
    header: {
        'Accept': 'application/json',
        'Content-Type': 'application/json',
        'Authorization': 'bearer ' + token
    }
};

window.onload = function (){
    getGarageContents();

}

async function getGarageContents(){
    await fetch(uri + getFilterString(), fetchRequst)
        .then(response => response.json())
        .then(data => {
            console.log(data);
            displayItems(data)
            })
}

let shelfName = document.getElementById("shelf-dropdown");
shelfName.onchange = function(){

}

function displayElements(data){
    

}

function loadShelf(){
    
}

function getFilterString(){
    return "getShelf?shelfName='TacoBell'&username='Zeina'";
}

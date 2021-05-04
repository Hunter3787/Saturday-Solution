
const uri ='https://localhost:5001/authdemo';
let todos = [];
var JWT_Token = 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJBdXRvYnVpbGQiLCJzdWIiOiJBdXRvYnVpbGQgVXNlciIsImF1ZCI6IlVTIiwiaWF0IjoxNjE4NDMxOTg0LCJleHAiOjE2MTkwMzY3ODQsIm5iZiI6MTYxOTAzNjc4NCwiVXNlcm5hbWUiOiJaZWluYSIsIlVzZXJDTGFpbXMiOlt7IlBlcm1pc3Npb24iOiJDUkVBVEUiLCJzY29wZU9mUGVybWlzc2lvbnMiOiJSRVZJRVdTIn0seyJQZXJtaXNzaW9uIjoiREVMRVRFIiwic2NvcGVPZlBlcm1pc3Npb25zIjoiU0VMRiJ9LHsiUGVybWlzc2lvbiI6IkRFTEVURSIsInNjb3BlT2ZQZXJtaXNzaW9ucyI6IlNFTEZfUkVWSUVXUyJ9LHsiUGVybWlzc2lvbiI6IkVESVQiLCJzY29wZU9mUGVybWlzc2lvbnMiOiJTRUxGIn0seyJQZXJtaXNzaW9uIjoiUkVBRF9PTkxZIiwic2NvcGVPZlBlcm1pc3Npb25zIjoiQVVUT0JVSUxEIn0seyJQZXJtaXNzaW9uIjoiVVBEQVRFIiwic2NvcGVPZlBlcm1pc3Npb25zIjoiU0VMRiJ9LHsiUGVybWlzc2lvbiI6IlVQREFURSIsInNjb3BlT2ZQZXJtaXNzaW9ucyI6IlNFTEZfUkVWSUVXUyJ9XX0.D4no6hO0ab8J9idjFi2M8FQLdEMIVXx3MCsNGzBPVLs';
  //https://gomakethings.com/using-oauth-with-fetch-in-vanilla-js/
  // https://www.digitalocean.com/community/tutorials/how-to-use-the-javascript-fetch-api-to-get-data

var msg = 'Hello world';


document.getElementById("myBtn")
    .addEventListener("click", getItems)
    //JWT_Token = ''
console.log(msg);


function getItemsggg() {
  fetch('https://localhost:5001/authdemo', {
  method: 'GET',
    headers: {
      'Accept': 'application/json',
      'Content-Type': 'application/json',
      'Authorization': 'bearer ' + JWT_Token
    },
    //body: JSON.stringify()
  })
    .then(response => response.json())
    .then(response => displayToken(response))
    .catch(error => console.error('Unable to get items.', error));
}


var item = ' ';
const fetchRequest = {
  method: 'POST',
  mode: 'cors',
 credentials: 'include', // Useful for including session ID (and, IIRC, authorization headers)
headers: {
  'Accept': 'application/json',
  'Content-Type': 'application/json',
  'Authorization': 'bearer ' + JWT_Token
  
}
};
let customRequest = 
Object.assign( 
  fetchRequest,
   { method: 'GET', body : JSON.stringify(item) })

function getItems() {
  fetch(uri, customRequest)
    //body: JSON.stringify()
    .then(response => response.json())
    .then(response => displayToken(response))
    .catch(error => console.error('Unable to get items.', error));
}


//https://www.w3schools.com/js/tryit.asp?filename=tryjs_addeventlistener_displaydate
document.getElementById("myBtn").addEventListener("click", displayDate);



// add notes
function displayToken(id)
{
  var getBody = document.getElementById("FEEDBACK");
  console.log(id);
  var text =  document.createTextNode(JSON.stringify(id));
  JWT_Token = id;
  alert(JWT_Token);
  getBody.appendChild(text);

}
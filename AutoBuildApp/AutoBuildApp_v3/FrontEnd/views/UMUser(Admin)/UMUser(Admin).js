
let token = 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJBdXRvYnVpbGQiLCJzdWIiOiJBdXRvYnVpbGQgVXNlciIsImF1ZCI6IlVTIiwiaWF0IjoxNjIwNDYyMDc0LCJleHAiOjE2MjEwNjY4NzQsIm5iZiI6MTYyMTA2Njg3NCwiVXNlcm5hbWUiOiJraW5nUGVuaTM5MyIsIlVzZXJDTGFpbXMiOlt7IlBlcm1pc3Npb24iOiJBbGwiLCJTY29wZU9mUGVybWlzc2lvbnMiOiJBbGwifV19.kno-KBPpzf-s446l5b4lONfBZQH8wdeCsVRy0BoONuY';

//let token = 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJBdXRvYnVpbGQiLCJzdWIiOiJBdXRvYnVpbGQgVXNlciIsImF1ZCI6IlVTIiwiaWF0IjoxNjIwNDYyMjAxLCJleHAiOjE2MjEwNjcwMDEsIm5iZiI6MTYyMTA2NzAwMSwiVXNlcm5hbWUiOiJjcmtvYmVsIiwiVXNlckNMYWltcyI6W3siUGVybWlzc2lvbiI6IkNyZWF0ZSIsIlNjb3BlT2ZQZXJtaXNzaW9ucyI6IlJldmlld3MifSx7IlBlcm1pc3Npb24iOiJEZWxldGUiLCJTY29wZU9mUGVybWlzc2lvbnMiOiJTZWxmIn0seyJQZXJtaXNzaW9uIjoiRGVsZXRlIiwiU2NvcGVPZlBlcm1pc3Npb25zIjoiU2VsZlJldmlld3MifSx7IlBlcm1pc3Npb24iOiJFZGl0IiwiU2NvcGVPZlBlcm1pc3Npb25zIjoiU2VsZiJ9LHsiUGVybWlzc2lvbiI6IlJlYWRPbmx5IiwiU2NvcGVPZlBlcm1pc3Npb25zIjoiQXV0b0J1aWxkIn0seyJQZXJtaXNzaW9uIjoiVXBkYXRlIiwiU2NvcGVPZlBlcm1pc3Npb25zIjoiU2VsZiJ9LHsiUGVybWlzc2lvbiI6IlVwZGF0ZSIsIlNjb3BlT2ZQZXJtaXNzaW9ucyI6IlNlbGZSZXZpZXdzIn1dfQ.rXRopsDv6V5SKmfWUHmjXJHUgaA2ojWfL5TyaHmepB8';
token = ' ';
var updateUsername = document.getElementById("updateUsername")

    updateUsername.addEventListener('click', () => {
      var username = document.getElementById("add-username").value;
      UpdateUsername(username, "crkobel@verizon.net");
  })

function UpdateUsername(username, activeEmail) {
    var formData = new FormData
    
    formData.append('username', username);
    formData.append('activeEmail', activeEmail);

    fetch("https://localhost:5001/usermanagement/username", {
      method: 'PUT',
      mode: 'cors',
      headers: {
        'Accept': 'application/json',
        'Authorization': 'bearer ' + token
      },
      body: formData
    })
      // Display the output message to the screen
      .then(response => response.json())
      .then(response => displayResponse(response))
      .catch(error => console.error('Unable to Authenticate.', error));
  
    return false;
  }


  var updateEmail = document.getElementById("updateEmail")

    updateEmail.addEventListener('click', () => {
      var inputEmail = document.getElementById("add-email").value;
      UpdateEmail(inputEmail, "bunnyB@gmail.com");
  })

function UpdateEmail(inputEmail, activeEmail) {
    var formData = new FormData
    
    formData.append('inputEmail', inputEmail);
    formData.append('activeEmail', activeEmail);

    fetch("https://localhost:5001/usermanagement/email", {
      method: 'PUT',
      mode: 'cors',
      headers: {
        'Accept': 'application/json',
        'Authorization': 'bearer ' + token
      },
      body: formData
    })
      // Display the output message to the screen
      .then(response => response.json())
      .then(response => displayResponse(response))
      .catch(error => console.error('Unable to Authenticate.', error));
  
    return false;
  }


  var updatePassword = document.getElementById("updatePassword")

    updatePassword.addEventListener('click', () => {
      var password = document.getElementById("add-password").value;
      var passwordCheck = document.getElementById("add-passwordCheck").value;
      UpdatePassword(password, passwordCheck, "crkobel@verizon.net");
  })

function UpdatePassword(password, passwordCheck, activeEmail) {
    var formData = new FormData
    
    formData.append('password', password);
    formData.append('passwordCheck', passwordCheck);
    formData.append('activeEmail', activeEmail);

    fetch("https://localhost:5001/usermanagement/password", {
      method: 'PUT',
      mode: 'cors',
      headers: {
        'Accept': 'application/json',
        'Authorization': 'bearer ' + token
      },
      body: formData
    })
      // Display the output message to the screen
      .then(response => response.json())
      .then(response => displayResponse(response))
      .catch(error => console.error('Unable to Authenticate.', error));
  
    return false;
  }

  function displayResponse(id)
  {
      alert(id);
  }

function changePage() {
    window.location.href = "RegisteredUsersList.html"
  }
const uri = 'https://localhost:44317/reviewrating';
let todos = [];

function getItems() {
    fetch('https://localhost:44317/reviewrating')
        .then(response => response.json())
        .then(data => _displayItems(data))
        .catch(error => console.error('Unable to get items.', error));
}

function addItem() {
    var starValue;
    const addUserNameTextbox = document.getElementById('add-username');
    //const addStarTextbox = $("input[name='interview']:checked").val();
    var ele = document.getElementsByName('rate');
    for (var i = 0; i < ele.length; i++) {
        if (ele[i].checked) {
            //choices.push(ele[i].value);
            starValue = ele[i].value
        }
    }
    const addMessageTextbox = document.getElementById('add-message');
    const addFilePathTextbox = document.getElementById('add-imagepath');

    //console.log(starValue);

  const item = {
      //isComplete: false,
      username: addUserNameTextbox.value.trim(),
      starRating: parseInt(starValue),
      //starRating: parseInt(addStarTextbox.value),
      message: addMessageTextbox.value.trim(),
      filePath: addFilePathTextbox.value
    };


    fetch('https://localhost:44317/ReviewRating', {
        method: 'POST',
        mode: 'cors',
    headers: {
      'Accept': 'application/json',
      'Content-Type': 'application/json'
    },
    body: JSON.stringify(item)
  })
    .then(response => response.json())
    .then(() => {
      getItems();
    })
    .catch(error => console.error('Unable to add item.', error));
}

function deleteItem(id) {
    fetch(`${uri}/${id}`, {
        method: 'DELETE',
        mode: 'cors',
  })
  //.then(() => getItems())
  .catch(error => console.error('Unable to delete item.', error));
}

function displayEditForm(id) {

    const item = todos.find(item => item["entityId"] === id.toString());

    document.getElementById('edit-id').value = item.entityId;
    document.getElementById('edit-message').value = item.message;
    document.getElementById('edit-starRating').value = item.starRating;
    document.getElementById('edit-filePath').value = item.filePath;
    document.getElementById('editForm').style.display = 'block';
}

function updateItem() {
    const itemId = document.getElementById('edit-id').value;

    const item = {
        //entityId: parseInt(itemId),
        //starRating: parseInt(document.getElementById('edit-starRating').value),
        //message: document.getElementById('edit-message').value.trim(),
        //filePath: document.getElementById('edit-filePath').value
        entityId: "30000",
        starRating: 5,
        message: "WORKS",
        filePath: null
  };

    console.log(item);

    fetch('https://localhost:44317/ReviewRating', {
        method: 'PUT',
        mode: 'cors',
    headers: {
      'Accept': 'application/json',
      'Content-Type': 'application/json'
    },
    body: JSON.stringify(item)
  })
  .then(() => getItems())
  .catch(error => console.error('Unable to update item.', error));

  closeInput();

  return false;
}

function closeInput() {
  document.getElementById('editForm').style.display = 'none';
}

function _displayCount(itemCount) {
  const name = (itemCount === 1) ? 'to-do' : 'to-dos';

  document.getElementById('counter').innerText = `${itemCount} ${name}`;
}

function _displayItems(data) {

    const tBody = document.getElementById('todos');
    tBody.innerHTML = '';
    //data.forEach(item => console.log(item["username"]));

  //const tBody = document.getElementById('todos');
    //tBody.innerHTML = '';
    data.forEach(item => {
        var stars = "";
        var table = document.createElement("tr"); // grandparent
        var para1 = document.createElement("td"); // parent
        var node1 = document.createTextNode(item["username"]); // child
        para1.appendChild(node1);
        table.appendChild(para1);
        var para2 = document.createElement("td"); // parent
        for (var i = 0; i < item["starRating"]; i++) {
            stars += String.fromCharCode(9733);
        }
        var node2 = document.createTextNode(stars); // child
        para2.appendChild(node2);
        table.appendChild(para2);
        var para3 = document.createElement("td"); // parent
        var node3 = document.createTextNode(item["message"]); // child
        para3.appendChild(node3);
        table.appendChild(para3);
        var para4 = document.createElement("td"); // parent
        var node4 = document.createTextNode(item["dateTime"]); // child
        para4.appendChild(node4);
        table.appendChild(para4);

        var para5 = document.createElement("td"); // parent
        var img = document.createElement("img");
        img.src = item["filePath"];
        para5.appendChild(img);
        table.appendChild(para5);

        var para6 = document.createElement("td");
        var deleteButton = document.createElement("button");
        deleteButton.innerText = 'Delete';
        deleteButton.setAttribute('onclick', `deleteItem(${item.entityId})`);
        para6.appendChild(deleteButton);
        table.appendChild(para6);

        var para7 = document.createElement("td");
        var editButton = document.createElement("button");
        editButton.innerText = 'Edit';
        editButton.setAttribute('onclick', `displayEditForm(${item.entityId})`);
        para7.appendChild(editButton);
        table.appendChild(para7);


        var element = document.getElementById("reviews-saved"); // great-grandparent
        element.appendChild(table);
    });
    // One grandparent appends many 1parent-1child groups


  //_displayCount(data.length);

  //const button = document.createElement('button');

  //data.forEach(item => {
  //  let isCompleteCheckbox = document.createElement('input');
  //  isCompleteCheckbox.type = 'checkbox';
  //  isCompleteCheckbox.disabled = true;
  //  isCompleteCheckbox.checked = item.isComplete;

  //  let editButton = button.cloneNode(false);
  //  editButton.innerText = 'Edit';
  //  editButton.setAttribute('onclick', `displayEditForm(${item.id})`);

  //  let deleteButton = button.cloneNode(false);
  //  deleteButton.innerText = 'Delete';
  //  deleteButton.setAttribute('onclick', `deleteItem(${item.id})`);

  //  let tr = tBody.insertRow();
    
  //  let td1 = tr.insertCell(0);
  //  td1.appendChild(isCompleteCheckbox);

  //  let td2 = tr.insertCell(1);
  //  let textNode = document.createTextNode(item.name);
  //  td2.appendChild(textNode);

  //  let td3 = tr.insertCell(2);
  //  td3.appendChild(editButton);

  //  let td4 = tr.insertCell(3);
  //  td4.appendChild(deleteButton);
  //});

  todos = data;
}
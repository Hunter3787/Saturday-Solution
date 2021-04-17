var checkbox = document.getElementsByClassName("checkbox-filter")

checkbox.addEventListener('change', function() {

    for (var i = 1;i <= 4; i++)
    {
        document.getElementById("F" + i).checked = false;
    }
    document.getElementById(id).checked = true;

});
const uri = "api/todos";
let todos = null;
function getCount(data) {
    const cap = $("#counter");
    let name = "Todo item";
    if (data) {
        if (data > 1) {
            name = "Todo items";
        }
        cap.text(data + " " + name + "");
    } else {
        cap.text("No Todo items found.");
    }
}

$(document).ready(function () {
    $('#caption').text("Welcome, " + sessionStorage.getItem("userName") + "!");
    getData();
});

function getData() {    
    $.ajax({
        type: "GET",
        url: uri,
        cache: false,
        headers: { "Authorization": "Bearer " + sessionStorage.getItem("accessToken") },
        data: { "username": sessionStorage.getItem("userName") },
        success: function (data) {                 
            const tBody = $("#todos");
            $(tBody).empty();
            getCount(data.length);
            $.each(data, function (key, item) {                   
                const tr = $("<tr></tr>")
                    .append(
                        $("<td></td>").append(
                            $("<input/>", {
                                type: "checkbox",
                                disabled: true,
                                checked: item.isChecked == 1 ? true : false
                            })
                        )
                )
                    .append($("<td></td>").text(item.taskName))
                    .append($("<td></td>").text(item.priority))
                    .append($("<td></td>").text(item.dueDate))
                    .append($("<td></td>").text(item.status))
                    .append(
                        $("<td></td>").append(
                        $("<button>Edit</button>").on("click", function () {
                            editItem(item.id);
                            })
                        )
                    )
                    .append(
                        $("<td></td>").append(
                        $("<button>Delete</button>").on("click", function () {
                            deleteItem(item.id);
                            })
                        )
                    );                
                tr.appendTo(tBody);
            });

            todos = data;
        }
    });
}

function addItem() {
    //performing empty field validations    
    // Checking the value before submission
    var task = $.trim($("#add-TaskName").val());
    var pri = $.trim($("#add-Priority").val());
    var date = $.trim($("#add-DueDate").val());
    var status = $.trim($("#add-Status").val());

    // Check if empty or not
    if (task === '') {
        alert('Please enter a Task Name.');
        return false;
    }
    if (pri === '') {
        alert('Please enter a Priority.');
        return false;
    }
    if (date === '') {
        alert('Please enter a Due Date.');
        return false;
    }
    if (status === '') {
        alert('Please enter a Status.');
        return false;
    }    

    const item = {
        userName: sessionStorage.getItem("userName"),
        taskName: $("#add-TaskName").val(),
        priority: $("#add-Priority").val(),
        dueDate: $("#add-DueDate").val(),
        status: $("#add-Status").val(),
        isChecked: $("#add-IsChecked").is(":checked") ? 1 : 0
    };    
    $.ajax({
        type: "POST",
        accepts: "application/json",
        url: uri,
        contentType: "application/json",
        headers: { "Authorization": "Bearer " + sessionStorage.getItem("accessToken") },
        data: JSON.stringify(item),
        error: function (jqXHR, textStatus, errorThrown) {
            alert("Something went wrong!");
        },
        success: function (result) {
            getData();
            $("#add-name").val("");
        }
    });
}

function deleteItem(id) {
    $.ajax({
        url: uri + "/" + id,
        type: "DELETE",
        headers: { "Authorization": "Bearer " + sessionStorage.getItem("accessToken") },
        success: function (result) {
            getData();
        }
    });
}

function login() {
    $.ajax({
        // Post username, password & the grant type to /token
        url: 'api/auth/token',
        type: 'POST',
        contentType: 'application/json',
        data: {
            username: $('#txtUsername').val(),
            password: $('#txtPassword').val(),
            grant_type: 'password'
        },
        //whatever you need
        beforeSend: function (xhr) {
            xhr.setRequestHeader('Authorization', make_base_auth($('#txtUsername').val(), $('#txtPassword').val()));
        },
        // When the request completes successfully, save the
        // access token in the browser session storage and
        // redirect the user to Data.html page. We do not have
        // this page yet. So please add it to the
        // EmployeeService project before running it
        success: function (response) {            
            $('#divErrorText').text(JSON.stringify(response));
            $('#divError').show('fade');
            sessionStorage.setItem("accessToken", response);
            sessionStorage.setItem("userName", $('#txtUsername').val());
            window.location.href = "index.html";
        },
        // Display errors if any in the Bootstrap alert <div>
        error: function (jqXHR) {            
            $('#divErrorText').text("Invalid username/password.");
            $('#divError').show('fade');
        }
    });
}

function logoff()
{    
    sessionStorage.removeItem("accessToken");
    window.location.href = "Login.html";
}
//encryption
function make_base_auth(user, password) {    
    var token = user + ':' + password;
    var hash = btoa(token);
    return 'Basic ' + hash;
}

function editItem(id) {    
    $.each(todos, function (key, item) {                
        if (item.id === id) {
            $("#edit-TaskName").val(item.taskName);
            $("#edit-Priority").val(item.priority);
            //datepicker() not working for some reason
            $('#edit-DueDate').val(item.dueDate);            
            $("#edit-Status").val(item.status);
            $("#edit-ID").val(item.id);
            $("#edit-IsChecked")[0].checked = item.isChecked;
        }
    });
    $("#spoiler").css({ display: "block" });
}

$(".my-form").on("submit", function () {    
    // Checking the value before submission
    var task = $.trim($("#edit-TaskName").val());
    var pri = $.trim($("#edit-Priority").val());
    var date = $.trim($("#edit-DueDate").val());
    var status = $.trim($("#edit-Status").val());

    // Check if empty or not
    if (task === '' ) {
        alert('Please enter a Task Name.');
        return false;
    }
    if (pri === '') {
        alert('Please enter a Priority.');
        return false;
    }
    if (date === '') {
        alert('Please enter a Due Date.');
        return false;
    }
    if (status === '') {
        alert('Please enter a Status.');
        return false;
    }    
    const item = {
        id: $("#edit-ID").val(),
        taskName: $("#edit-TaskName").val(),
        priority: $("#edit-Priority").val(),
        dueDate: $("#edit-DueDate").val(),
        status: $("#edit-Status").val(),
        isChecked: $("#edit-IsChecked").is(":checked") ? 1 : 0
    };

    $.ajax({
        url: uri + "/" + $("#edit-ID").val(),
        type: "PUT",
        accepts: "application/json",
        contentType: "application/json",
        headers: { "Authorization": "Bearer " + sessionStorage.getItem("accessToken") },
        data: JSON.stringify(item),
        success: function (result) {
            getData();
        }
    });

    closeInput();
    return false;
});

function closeInput() {
    $("#spoiler").css({ display: "none" });
}
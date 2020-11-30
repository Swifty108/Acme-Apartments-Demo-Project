$(document).ready(function () {
    $("#nav-reqhistory-tab").click(function () {
        $("#showError").hide();
        $.ajax({
            type: "GET",
            url: "/residentaccount/getreqhistory",
            async: true,
            dataType: "json",
            success: function (response) {
                if (response.list.length != 0) {
                    //$("#showError").hide();
                    createTable(response.list);
                }
                else {
                    $("#showError").show();
                    $("#reqhistorytable").hide();
                }
            },
            failure: function (response) {
                alert("failed");
            },
            error: function (response) {
                alert(response);
            }
        });
    });

    function createTable(data) {
        $("#tbody-appened").empty();
        //var tbody = $("<tbody />"), tr;

        $.each(data, function (_, obj) {
            var tr = $("<tr />");
            $.each(obj, function (index, value) {
                switch (index) {
                    case "dateRequested":
                        date = new Date(value);
                        year = date.getFullYear();
                        month = date.getMonth() + 1;
                        dt = date.getDate();

                        if (dt < 10) {
                            dt = '0' + dt;
                        }
                        if (month < 10) {
                            month = '0' + month;
                        }
                        tr.append("<td> " + month + '/' + dt + '/' + year + " </td>");
                        break;
                    case "problemDescription":
                        tr.append("<td> " + value + " </td>");
                        break;
                    case "status":
                        var toAppend = (value == "Approved") ? "<td> Approved </td>" : "<td> <i class='fa fa-clock-o'></i> Pending Approval </td>";
                        tr.append(toAppend);
                        break;
                }
            });
            tr.appendTo("#tbody-appened");
        });
        //tbody.appendTo("#reqhistorytable");
    }
});
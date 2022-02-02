$(document).ready(function () {
    $("#nav-reqhistory-tab").click(function () {
        $("#showError").hide();
        $.ajax({
            type: "GET",
            url: "/resident/getreqhistory",
            async: true,
            dataType: "json",
            success: function (response) {
                if (response.list.length != 0) {
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
                        let toAppend;

                        if (value == "Approved") {
                            toAppend = "<td> <span style='color: darkgreen'><i class='fa fa-check text-success'></i> Approved</span></td>"
                        }
                        else if (value == "UnApproved") {
                            toAppend = "<td><span style='color: darkred'><i class='fa fa-ban'></i> Denied</span></td>"
                        }
                        else if (value == "Pending Approval") {
                            toAppend = "<td><i class='fa fa-clock-o'></i> Pending Approval</td>"
                        }

                        tr.append(toAppend);
                        break;
                }
            });
            tr.appendTo("#tbody-appened");
        });
    }
});
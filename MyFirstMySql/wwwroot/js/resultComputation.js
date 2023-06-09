$("body").on("click", "#btnAdd", function () {
    //Reference the Name and Country TextBoxes.
    var subject = $("#subject");
    var asses1 = $("#asses1");
    var asses2 = $("#asses2");
    var test = $("#test");
    var exams = $("#exams");
    var total = $("#total");
    var grade = $("#grade");
    var remark = $("#remark");
    var id = $("#id");

    //Get the reference of the Table's TBODY element.
    var tBody = $("#tblCustomers > TBODY")[0];

    //Add Row.
    var row = tBody.insertRow(-1);

    //Add subject cell.
    var cell = $(row.insertCell(-1));
    cell.html(subject.val());
    //Add asses1 cell.
    cell = $(row.insertCell(-1));
    cell.html(asses1.val());
    //Add asses2 cell.
    cell = $(row.insertCell(-1));
    cell.html(asses2.val());
    //Add test cell.
    cell = $(row.insertCell(-1));
    cell.html(test.val());
    //Add exams cell.
    cell = $(row.insertCell(-1));
    cell.html(exams.val());
    //Add total cell.
    cell = $(row.insertCell(-1));
    cell.html(total.val());
    //Add grade cell.
    cell = $(row.insertCell(-1));
    cell.html(grade.val());
    //Add remark cell.
    cell = $(row.insertCell(-1));
    cell.html(remark.val());
    //Add id cell.
    cell = $(row.insertCell(-1));
    cell.html(id.val());

    //Add Remove cell.
    cell = $(row.insertCell(-1));
    var btnRemove = $("<input />");
    btnRemove.attr("type", "button");
    btnRemove.attr("onclick", "Remove(this);");
    btnRemove.val("Remove");
    cell.append(btnRemove);

    //Clear the TextBoxes.
    asses1.val("");
    asses2.val("");
    test.val("");
    exams.val("");
    total.val("");
    grade.val("");
    remark.val("");
});

function Remove(button) {
    //Determine the reference of the Row using the Button.
    var row = $(button).closest("TR");
    var name = $("TD", row).eq(0).html();
    if (confirm("Do you want to delete: " + name)) {
        //Get the reference of the Table.
        var table = $("#tblCustomers")[0];

        //Delete the Table row using it's Index.
        table.deleteRow(row[0].rowIndex);
    }
};

$("body").on("click", "#btnSave", function () {

    //Loop through the Table rows and build a JSON array.
    var resultsExams = new Array();
    $("#tblCustomers TBODY TR").each(function () {
        var row = $(this);
        var result = {};
        result.Subjects = row.find("TD").eq(0).html();
        result.Assessment1 = row.find("TD").eq(1).html();
        result.Assessment2 = row.find("TD").eq(2).html();
        result.Test = row.find("TD").eq(3).html();
        result.Examination = row.find("TD").eq(4).html();
        result.Total = row.find("TD").eq(5).html();
        result.Grade = row.find("TD").eq(6).html();
        result.Remark = row.find("TD").eq(7).html();
        result.Id = row.find("TD").eq(8).html();
        resultsExams.push(result);
    });
    var JsonData = {
        "resultsExams": resultsExams
    }
    //Send the JSON array to Controller using AJAX.
    $.ajax({
        type: "POST",
        url: "/api/SendResult",
        data: JsonData,
        //contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function () {
            // Show image container
            $("#loader").show();
        },
        success: function (r) {
            alert(r + " record(s) inserted.");
        },
        complete: function (data) {
            // Hide image container
            $("#loader").hide();
            location.reload();
        }
    });
});
function sum() {
    var A1 = document.getElementById("asses1").value;
    var A2 = document.getElementById("asses2").value;
    var Test = document.getElementById("test").value;
    var Exams = document.getElementById("exams").value;
    var sum = Number(A1) + Number(A2) + Number(Test) + Number(Exams);
    document.getElementById("total").value = sum;
    if (sum < 40) {
        document.getElementById("grade").value = 'E';
        document.getElementById("remark").value = "Poor";
    }
    else if (sum < 55) {
        document.getElementById("grade").value = 'D';
        document.getElementById("remark").value = "Fair";
    }
    else if (sum < 65) {
        document.getElementById("grade").value = 'C';
        document.getElementById("remark").value = "Good";
    }
    else if (sum < 75) {
        document.getElementById("grade").value = 'B';
        document.getElementById("remark").value = "Very Good";
    }
    if (sum > 74) {
        document.getElementById("grade").value = 'A';
        document.getElementById("remark").value = "Distintction";
    }

    if (A1 > 20) {
        document.getElementById("asses1").value = '';
        document.getElementById("total").value = '';
        return alert('Assessment can not be morethan 10, check and try again');
    }
    if (A2 > 20) {
        document.getElementById("total").value = '';
        document.getElementById("asses2").value = '';
        return alert('Assessment 2 can not be morethan 10, check and try again');
    }
    if (Test > 20) {
        document.getElementById("total").value = '';
        document.getElementById("test").value = '';
        return alert('Test can not be greater than 20, check and try again');
    }
    if (Exams > 70) {
        document.getElementById("total").value = '';
        document.getElementById("exams").value = '';
        return alert('Examination can not be morethan 100, check and try again');
    }
    if (sum > 100) {
        document.getElementById("total").value = '';
        document.getElementById("exams").value = '';
        return alert('Total Examination can not be morethan 100, check and try again');
    }
}
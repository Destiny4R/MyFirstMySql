var dataTable;
var i = 1;
$(document).ready(function () {
    dataTable = $("#subjectTable").DataTable({
        "processing": true,
        "serverSide": true,
        "filter": true,
        "paging": true,
        "ajax": {
            "url": "/api/StudentTermData",
            "type": "POST",
            "datatype": "json"
        },
        "columnDefs": [{
            "targets": [0],
            "visible": true,
            "searchable": true,
        }],
        "columns": [
            {
                "data": "id",
                render: function (data, type, row, meta) {
                    return meta.row + meta.settings._iDisplayStart + 1;
                }
            },
            { "data": "applicationUser.surName", "autoWidth": true },
            { "data": "applicationUser.otherName", "autoWidth": true },
            { "data": "applicationUser.firstName", "autoWidth": true },
            { "data": "applicationUser.userName", "autoWidth": true },
            { "data": "sessionYear.name", "autoWidth": true },
            { "data": "term", "autoWidth": true },
            { "data": "classesInSchool.className", "autoWidth": true },
            { "data": "subClasses.className", "autoWidth": true },
            { "data": "id",
                "render": function (data) {
                    return `
                        <div class="text-center">
                            <a class="btn btn-success btn-sm" href="/Admin/TermRegistration/EditRegistration?id=${data}" style="cursor:pointer">
                                <i class="fa fa-edit"></i>
                            </a>
                        </div>
                    `;
                }
            },
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <div class="text-center">
                            <a onclick=Delete("/api/DeleteTermReg/${data}") class="btn btn-danger btn-sm" href="#" style="cursor:pointer">
                                <i class="fa fa-trash-o"></i>
                            </a>
                        </div>
                    `;
                }
            },
        ]
    });
});  
function Remove() {
    alert("Are you sure you want to remove this? It cannot be undone after deleting");
};
function Delete(url) {
    swal({
        title: "Are you sure you want to delete?",
        text: "You will not be able to recover this data!",
        icon: "warning",
        buttons: true,
        dangerModel: false
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                type: "DELETE",
                url: url,
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        dataTable.ajax.reload();
                    } else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    });
}

    
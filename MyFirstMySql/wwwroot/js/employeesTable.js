var dataTable;
$(document).ready(function () {
    dataTable = $("#subjectTable").DataTable({
        "processing": true,
        "serverSide": true,
        "filter": true,
        "ajax": {
            "url": "/api/StaffData",
            "type": "POST",
            "datatype": "json"
        },
        "columnDefs": [{
            "targets": [0],
            "visible": true,
            "searchable": true,
        }],
        "columns": [
            { "data": "surName", "autoWidth": true },
            { "data": "firstName", "autoWidth": true },
            { "data": "otherName", "autoWidth": true },
            { "data": "userName", "autoWidth": true },
            { "data": "gender", "autoWidth": true },
            { "data": "id",
                "render": function (data) {
                    return `
                        <div class="text-center">
                            <a class="btn btn-success btn-sm" href="/Admin/Employees/EditSystemUser?id=${data}" style="cursor:pointer">
                                <i class="fa fa-edit"></i>
                            </a>
                            <a onclick=Delete("/api/UserDelete/${data}") class="btn btn-danger btn-sm" href="#" style="cursor:pointer">
                                <i class="fa fa-trash-o"></i>
                            </a>
                        </div>
                    `;
                }
            }
        ]
    });
});

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


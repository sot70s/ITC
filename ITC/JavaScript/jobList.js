var dataTable;

$(document).ready(() => {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#DT_load').DataTable({
        destroy: true,
        responsive: true,
        ajax: {
            "url": "./GetJobReqHeader/",
            "type": "GET",
            "datatype": "json"
        },
        columns: [
            { data: "WorkRequest"},
            { data: "CreateDate" },
            { data: "Requestor"},
            {
                "data": "Id",
                "render": function (data) {
                    return `<div class="text-center">
                        <a href="./Upsert?id=${data}" class="btn btn-success text-white" style="cursor:pointer;width70px;">
                            Edit                        
                        </a>
                        &nbsp;
                        <a class="btn btn-danger text-white" style="cursor:pointer;width:70px;"
                            onclick=Delete("./Delete?id=${data}")>
                            Delete
                        </a>
                        </div>`;

                }
            }
        ],
        language: {
            "emptyTable": "no data found"
        }
    });
}

function Delete(url) {
    swal({
        title: "Are you sure?",
        text: "Once deleted,you will not be able to recover",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                type: "DELETE",
                url: url,
                success: (data) => {
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
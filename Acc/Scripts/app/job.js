function initJobs() {
    var tabelMemRef = $("#tdataTable").DataTable({
        ajax: {
            url: "/api/Job/GetJob",
            dataSrc: ""
        },
        columns: [
            {

                data: "jobId"
            },

            {
                data: "jobArName"

            },


            {
                data: "jobId",
                render: function (data, type, r) {
                   

                    return "<a  class='btn btn-outline btn-success js-modify' href='/Job/Modify/" + data + "'> @Resources.Resource.Modify</a>";


                }
            },


            {
                data: "jobId",
                render: function (data) {
                    return "<a  class='btn btn-outline btn-danger js-modify' href='/Job/Delete/" + data + "'> @Resources.Resource.Delete</a>";


                }

            }


        ]


    });


    $('#tdataTable').on('click', '.js-delete', function (e) {
        e.preventDefault();
        $(this).attr('data-target', '#modal-container');
        $(this).attr('data-toggle', 'modal');
    });

    $('#tdataTable').on('click', '.js-modify', function (e) {
        e.preventDefault();
        $(this).attr('data-target', '#modal-container');
        $(this).attr('data-toggle', 'modal');
    });

    $('#js-addnew').on('click', function (e) {
        e.preventDefault();
        $(this).attr('data-target', '#modal-container');
        $(this).attr('data-toggle', 'modal');
    });

    // Attach listener to .modal-close-btn's so that when the button is pressed the modal dialog disappears
    $('#tdataTable').on('click', '.modal-close-btn', function () {
        $('#modal-container').modal('hide');
    });

    //clear modal cache, so that new content can be loaded
    $('#modal-container').on('hidden.bs.modal', function () {
        $(this).removeData('bs.modal');
        console.log('modal-containe');
    });

    $('#CancelModal').on('click', function () {
        return false;
    });


    $(document).on('click', '#save', function () {
        var dataToSave = {
            JobArName: $("#JobArName").val(),
            JobId: $("#JobId").val(), CompanyProfileId: $("#CompanyProfileId").val()
        };
        $.ajax({
            type: "POST",
            url: "/Job/Save",
            data: JSON.stringify(dataToSave),
            contentType: 'application/json',
            success: function (data) {
                $('#modal-container').modal('hide');
                toastr.success("@Resources.Resource.OperationCompletedSuccessfully");

                tabelMemRef.ajax.reload();

            },
            error: function (data) {
                toastr.error("@Resources.Resource.SomthingWentWrong");
            }

        });
    });
    $(document).on('click', '#delete', function () {
        var dataToSave = {
            JobArName: $("#JobArName").val(),
            JobId: $("#JobId").val(), CompanyProfileId: $("#CompanyProfileId").val(), AddBYUserID: $("#AddBYUserID").val()
        };
        $.ajax({
            type: "POST",
            url: "/Job/Remove",
            data: JSON.stringify(dataToSave),
            contentType: 'application/json',
            success: function (data) {
                $('#modal-container').modal('hide');
                toastr.success("@Resources.Resource.OperationCompletedSuccessfully");

                tabelMemRef.ajax.reload();

            },
            error: function (data) {
                toastr.error("@Resources.Resource.SomthingWentWrong");
            }

        });
    });

};
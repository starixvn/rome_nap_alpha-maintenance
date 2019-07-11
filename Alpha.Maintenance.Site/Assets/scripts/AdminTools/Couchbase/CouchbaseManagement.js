$(document).ready(function () {
    $('#submit').click(function () {
        $('#error-message').hide();
        if ($('#custIds').val() == undefined || $('#custIds').val() == '') {
            $('#error-message').html("CustIds cannot be empty.").show();
            return;
        }

        $.ajax({
            url: "CouchbaseSelTransferDocument",
            data: {
                custIds: $('#custIds').val()
            },
            type: 'POST',
            dataType: 'json',
            cache: false,
            beforeSend: function () {
                NAP.loadingBox.show();
            },
            complete: function () {
                NAP.loadingBox.close();
            },
            success: function (result) {
                var $resultTableBody = $("#result-table>tbody");
                $resultTableBody.empty();
                var custIds = $('#custIds').val().split(',');

                for (var i = 0; i < result.length; i++) {
                    $resultTableBody.append(
                        '<tr>' +
                        '<td>' + custIds[i] + '</td>' +
                        '<td>' + result[i] + '</td>' +
                        '</tr>');
                }
            },
            error: function (xhr, status, err) {
                NAP.handleAjaxError(xhr, status, err);
            }
        });
    });

    $('#del-document').click(function () {
        $('#error-message').hide();
        if ($('#custIds').val() == undefined || $('#custIds').val() == '') {
            $('#error-message').html("CustIds cannot be empty.").show();
            return;
        }

        $.ajax({
            url: "CouchbaseDelTransferDocument",
            data: {
                custIds: $('#custIds').val()
            },
            type: 'POST',
            dataType: 'json',
            cache: false,
            beforeSend: function () {
                NAP.loadingBox.show();
            },
            complete: function () {
                NAP.loadingBox.close();
            },
            success: function (result) {
                $('#submit').click();
            },
            error: function (xhr, status, err) {
                NAP.handleAjaxError(xhr, status, err);
            }
        });
    });
});
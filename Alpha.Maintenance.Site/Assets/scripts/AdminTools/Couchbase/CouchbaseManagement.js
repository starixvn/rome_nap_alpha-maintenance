$(document).ready(function () {
    $('select').material_select();

    function generateModel() {
        if ($('#keys').val() == undefined || $('#keys').val() == '') {
            $('#error-message').html("Keys cannot be empty.").show();
            return;
        }

        var keys = $('#keys').val().split(',');
        var filterKeys = [];
        for (var key of keys) {
            var trimKey = $.trim(key);
            if (trimKey) {
                filterKeys.push(trimKey);
            }
        }

        var bucket = $('#bucket').val();
        return {
            keys: filterKeys,
            pattern: $('#pattern').val(),
            bucket: bucket
        };
    }

    $('#submit').click(function () {
        $('#error-message').hide();
        var model = generateModel();
        if (!model) return;

        $.ajax({
            url: NAP.resolveClientUrl("AdminTools/CouchbaseSel"),
            data: JSON.stringify(model),
            type: 'POST',
            contentType: "application/json",
            dataType: "json",
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
                for (var i = 0; i < result.length; i++) {
                    $resultTableBody.append(
                        '<tr>' +
                        '<td>' + result[i].cacheKey + '</td>' +
                        '<td>' + result[i].cacheValue + '</td>' +
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
        var model = generateModel();
        if (!model) return;

        $.ajax({
            url: NAP.resolveClientUrl("AdminTools/CouchbaseDel"),
            data: JSON.stringify(model),
            type: 'POST',
            contentType: "application/json",
            dataType: "json",
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
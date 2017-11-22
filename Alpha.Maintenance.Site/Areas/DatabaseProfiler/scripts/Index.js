
var Logs = (function ($) {
    return {
        Init: function () {
            $("#submit").on('click', function () {
                Logs.Getdata();
            });
        },
        Getdata: function () {
            var server = $("#Server").val();
            var site = $("#Site").val();
            var traceTimer = $("#TraceTimer").val();
            var traceStartEvents = $("#TraceStartEvents").val();
            var textDataLike = $("#TextDataLike").val();
            var textDataLike2 = $("#TextDataLike2").val();
            var textDataLike3 = $("#TextDataLike3").val();

            if (textDataLike == '' && textDataLike2 == '' && textDataLike3 == '') {
                NAP.headlineMsgBox.showInfo("Text Like cannot be empty.", { closeBtn: false });
                return;
            }

            if (traceTimer > 120)
            {
                NAP.headlineMsgBox.showInfo("Trace timer must be less than 120s", { closeBtn: false });
                return;
            }


            $.ajax({
                url: NAP.resolveClientUrl("DBProfiler/tracedlog/Logs"),
                data: {
                    Server: server,
                    Site: site,
                    TraceTimer: traceTimer,
                    TextDataLike: textDataLike,
                    TextDataLike2: textDataLike2,
                    TextDataLike3: textDataLike3,
                    TraceStartEvents: traceStartEvents
                },
                type: 'POST',
                dataType: 'HTML',
                cache: false,
                beforeSend: function () {
                    NAP.loadingBox.show();
                },
                complete: function () {
                    NAP.loadingBox.close();
                },
                success: function (result) {
                    $("#content").html(result);
                },
                error: function (xhr, status, err) {
                    NAP.handleAjaxError(xhr, status, err);
                }
            });
        }
    };
}(jQuery));

$(document).ready(function () {
    Logs.Init();
});
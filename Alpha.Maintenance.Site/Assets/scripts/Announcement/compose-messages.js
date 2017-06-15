"use strict";

var ComposedAnnouncement = (function ($) {
    return {
        init: function () {
            $('.jqte-test').jqte();
            $('ul.tabs').tabs();
            $('select').material_select();
            this.bindEvent();
        },
        bindEvent: function ()
        {

            $('#btnSave').on('click', function () {
                var systems = $("input[name='group-systems']:checked").map(function () {
                    return this.id;
                }).get();

                var environments = $("input[name='group-environments']:checked").map(function (index, domElement) {
                    return this.id;
                }).get();

                var contents = [];

                $("textarea").map(function (index, domElement) {
                    contents.push({ LanguageKey: this.id, Text: $(this).val() });
                });

                var announcement = {
                    code: $("#txtcode").val(),
                    title: $("#txttitle").val(),
                    isActive: $("#ChkActive").is(":checked"),
                    announcementType: $('input[name=group-popup]:checked').val(),
                    contents: contents,
                    environments: environments,
                    fromdate: $('#FromDate').val(),
                    todate: $('#ToDate').val(),
                    order: $('#group-order option:selected').val(),
                    specifications: [],
                    systems: systems
                };

                $.ajax({
                    url: NAP.resolveClientUrl("Announcement/InsertAnnouncement"),
                    type: 'POST',
                    data: JSON.stringify(announcement),
                    dataType: 'json',
                    contentType: "application/json; charset=utf-8",
                    beforeSend: function () {
                        NAP.loadingBox.show();
                    },
                    complete: function () {
                        NAP.loadingBox.close();
                    },
                    success: function (result) {

                    },
                    error: function (xhr, status, err) {
                        NAP.handleAjaxError(xhr, status, err);
                    }
                });
            });
        }
    }
}(jQuery));


$(document).ready(function () {
    ComposedAnnouncement.init();
});


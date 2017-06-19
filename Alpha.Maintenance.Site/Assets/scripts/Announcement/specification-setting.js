"use strict";

var SpecificationSetting = (function ($) {
    return {
        init: function () {
            $('select').material_select();
            this.bindEvent();
        },
        bindEvent: function () {
            $('#add-spec').on('click', function () {
                $("div[id^='specification']:hidden:first").removeClass("hiden");
                //$('div:hidden:first').removeClass("hiden");
                $("div[id^='specification']:visible").length == $('#number-of-specifications').val() ? $(this).addClass("hiden") : $(this).removeClass("hiden");
                var lengthddd = $('div:visible').length;
                //"number-of-specifications""
            });

            $('.hidden-specification').on('click', function () {
                $(this).parent('div').parent('div').addClass("hiden");
                $("div[id^='specification']:visible").length < $('#number-of-specifications').val() ? $("#add-spec").removeClass("hiden") : $("#add-spec").addClass("hiden");
            });

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
                    id: $('#AnnouncementId').val(),
                    code: $("#txtcode").val(),
                    title: $("#txttitle").val(),
                    isActive: $("#ChkActive").is(":checked"),
                    announcementType: $('input[name=group-popup]:checked').val(),
                    contents: contents,
                    environments: environments,
                    fromdate: $('#FromDate').val(),
                    todate: $('#ToDate').val(),
                    order: $('#group-order option:selected').val(),
                    systems: systems,
                    specifications: []
                };


                $.ajax({
                    url: NAP.resolveClientUrl("Announcement/InsertAnnouncement"),
                    type: 'POST',
                    data: { "announcement": JSON.stringify(announcement), action: $('#actionType').val() },
                    dataType: 'json',
                    //contentType: "application/json; charset=utf-8",
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
    SpecificationSetting.init();
});


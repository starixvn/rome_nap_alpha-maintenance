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
            var announcement = {
                code: 125,
                announcementType: 1,
                contents: [{ LanguageKey: "Eng-eng", Text: "My contents are from Eng-eng" },
                            { LanguageKey: "zh-CN", Text: "My contents are from zh-CN" },
                            { LanguageKey: "zh-TW", Text: "My contents are from zh-TW" }],
                Environments: ['Local', 'UAT'],
                fromdate: '2017-06-05',
                todate: '2017-06-16',
                isActive: true,
                order: 9,
                specifications: ['dfafdagfagfsgfs', 'dfasfasfdafdaf'],
                systems: ['MBC', '123'],
                title: 'Example'
            };

            $('#btnSave').on('click', function () {
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


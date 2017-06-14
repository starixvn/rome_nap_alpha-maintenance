"use strict";

var ShowedAnnouncement = (function ($) {
    return {
        init: function () {
            $('select').material_select();
            $("#submit").click(function () {
                $("#report-form").submit();
            });
        }
    }
}(jQuery));


$(document).ready(function () {
    ShowedAnnouncement.init();
});
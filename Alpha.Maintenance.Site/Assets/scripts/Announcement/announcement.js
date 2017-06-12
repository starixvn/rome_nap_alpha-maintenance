"use strict";

var Announcement = (function ($) {
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
    Announcement.init();
});
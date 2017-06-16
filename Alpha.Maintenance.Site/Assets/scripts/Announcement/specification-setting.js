"use strict";

var SpecificationSetting = (function ($) {
    return {
        init: function () {
            $('select').material_select();
            this.bindEvent();
        },
        bindEvent: function ()
        {

        }
    }
}(jQuery));


$(document).ready(function () {
    SpecificationSetting.init();
});


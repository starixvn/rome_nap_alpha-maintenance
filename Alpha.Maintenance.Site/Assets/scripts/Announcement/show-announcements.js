"use strict";
var ShowedAnnouncement = (function () {
    function ShowedAnnouncement($) {
        var _this = this;
        this.init = function () {
            _this.$('select').material_select();
            _this.$("#submit").click(function () {
                this.$("#report-form").submit();
            });
        };
        this.$ = $;
    }
    return ShowedAnnouncement;
}());
var showedAnnouncement = new ShowedAnnouncement($);
$(function () {
    showedAnnouncement.init();
});

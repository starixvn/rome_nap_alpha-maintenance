"use strict";
var ShowedAnnouncement = (function () {
    function ShowedAnnouncement($) {
        var _this = this;
        this.init = function () {
            var self = _this;
            _this.$('select').material_select();
            _this.$("#submit").click(function () {
                self.$("#report-form").submit();
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

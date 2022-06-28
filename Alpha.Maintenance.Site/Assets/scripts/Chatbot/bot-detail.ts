"use strict";


class BotDetail {
    $: JQueryStatic;

    constructor($: JQueryStatic) {
        this.$ = $;
    }

    $checkboxes: JQuery;

    init = () => {
        this.$checkboxes = this.$("input[type=checkbox]");
        var self = this;
        this.$checkboxes.click(function () {
            self.$(this).val(self.$(this).is(":checked") ? "True": "False");
        });
    }
}

var bot = new BotDetail($);
$(() => {
    bot.init();
});
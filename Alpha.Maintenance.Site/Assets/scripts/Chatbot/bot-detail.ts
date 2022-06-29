"use strict";
interface Window {
    NAP: any;
}

class BotDetail {
    $: JQueryStatic;
    $NAP: any;

    constructor($: JQueryStatic, NAP: any) {
        this.$ = $;
        this.$NAP = NAP;
    }

    $checkboxes: JQuery;
    $btnDeleteWebhook: JQuery;
    $token: JQuery;

    init = () => {
        this.$checkboxes = this.$("input[type=checkbox]");
        this.$btnDeleteWebhook = this.$("#btnDeleteWebhook");
        this.$token = this.$("#txtAccessToken");
        var self = this;
        this.$checkboxes.click(function () {
            self.$(this).val(self.$(this).is(":checked") ? "True": "False");
        });
        this.$btnDeleteWebhook.click(this.deleteWebhook.bind(this));
    }

    deleteWebhook = () => {
        this.$.ajax({
            url: this.$NAP.resolveClientUrl("Chatbot/DeleteWebhook"),
            data: { token: this.$token.val() },
            type: 'POST',
            dataType: 'json',
            cache: false,
            beforeSend: () => {
                this.$NAP.loadingBox.show();
            },
            complete: () => {
                this.$NAP.loadingBox.close();
            },
            success: (result) => {
                window.alert(result.IsOk ? "Successful" : "Failed");
            },
            error: (xhr, status, err) => {
                this.$NAP.handleAjaxError(xhr, status, err);
            }
        })
    }
}

var bot = new BotDetail($, window.NAP);
$(() => {
    bot.init();
});
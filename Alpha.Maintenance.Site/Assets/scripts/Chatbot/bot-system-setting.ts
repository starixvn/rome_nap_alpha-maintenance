"use strict";
interface JQuery {
    material_select(): void;
}

interface Window {
    NAP: any;
}

class BotSystemSetting {
    $: JQueryStatic;
    $NAP: any;

    $btnSubmit: JQuery;
    $contentTable: JQuery;
    $serviceName: JQuery;

    constructor($: JQueryStatic, nap: any) {
        this.$ = $;
        this.$NAP = nap;
    }

    init = () => {
        this.$("select").material_select();
        this.$btnSubmit = this.$('#btnSubmit');
        this.$contentTable = this.$('#content');
        this.$serviceName = this.$("#ServiceName");
        this.$btnSubmit.click(() => { this.getBotSystemSettings(""); })
        this.getBotSystemSettings(this.$serviceName.val());
    };

    getBotSystemSettings = (settingName) => {
        const serviceName = settingName || this.$serviceName.val();
        this.$.ajax({
            url: this.$NAP.resolveClientUrl("Chatbot/GetBotSystemSettings"),
            data: { serviceName: serviceName },
            type: 'GET',
            dataType: 'HTML',
            cache: false,
            beforeSend: () => {
                this.$NAP.loadingBox.show();
            },
            complete: () => {
                this.$NAP.loadingBox.close();
            },
            success: (result) => {
                this.$contentTable.html(result);
            },
            error: (xhr, status, err) => {
                this.$NAP.handleAjaxError(xhr, status, err);
            }
        })
    }
}

$(() => {
    const botSystemSetting = new BotSystemSetting($, window.NAP);
    botSystemSetting.init();
});
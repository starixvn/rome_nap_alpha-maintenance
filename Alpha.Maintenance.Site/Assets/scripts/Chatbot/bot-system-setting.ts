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
        this.$btnSubmit.click(() => { this.getBotSystemSettings(); })
    };

    getBotSystemSettings = () => {
        const serviceName = this.$serviceName.val();
        const url = this.$NAP.resolveClientUrl("Chatbot/Settings?serviceName=");
        window.location.href = url + serviceName;
    }
}

$(() => {
    const botSystemSetting = new BotSystemSetting($, window.NAP);
    botSystemSetting.init();
});
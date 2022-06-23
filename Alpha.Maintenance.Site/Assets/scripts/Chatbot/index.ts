"use strict";

interface Window {
    NAP: any;
}

class Chatbot {
    $: JQueryStatic;
    $NAP: any;

    $btnSubmit: JQuery;
    $contentTable: JQuery;
    $enabledCheckbox: JQuery;
    $isTestCheckbox: JQuery;

    constructor($: JQueryStatic, NAP: any) {
        this.$ = $;
        this.$NAP = NAP;
    }

    init = () => {
        this.$enabledCheckbox = this.$("#Enabled");
        this.$isTestCheckbox = this.$("#IsTest");
        this.$btnSubmit = this.$('#btnSubmit');
        this.$contentTable = this.$('#content');
        this.$btnSubmit.click(() => { this.getBots(); })
        this.getBots();
    };

    getBots = () => {
        const enabled = this.$enabledCheckbox.is(":checked");
        const isTest = this.$isTestCheckbox.is(":checked");
        this.$.ajax({
            url: this.$NAP.resolveClientUrl("Chatbot/GetBots"),
            data: { enabled: enabled, isTest: isTest },
            type: 'GET',
            dataType: 'HTML',
            cache: false,
            beforeSend: ()=> {
                this.$NAP.loadingBox.show();
            },
            complete: () =>{
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

const chatbot = new Chatbot($, window.NAP);

$(() => {
    chatbot.init();
});
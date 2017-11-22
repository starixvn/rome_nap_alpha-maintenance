"use strict";

interface Window {
    NAP: any;
    JSON: any;
}

class IPInformation {
    $: JQueryStatic;
    $NAP: any;
    $JSON: any;

    $ip: JQuery;
    $btnSubmit: JQuery;
    $contentTable: JQuery;

    constructor($: JQueryStatic, NAP: any, JSON: any) {
        this.$ = $;
        this.$NAP = NAP;
        this.$JSON = JSON;
    }

    init = () => {
        this.$ip = this.$('#txtip');
        this.$btnSubmit = this.$('#btnSubmit');
        this.$contentTable = this.$('#content');

        this.bindEvent();
    }

    bindEvent = () => {
        this.$btnSubmit.click(this.generateData);
    }

    generateData = () => {
        this.$.ajax({
            url: this.$NAP.resolveClientUrl("IPInformation/Index"),
            data: { ip: this.$ip.val() },
            type: 'POST',
            dataType: 'json',
            cache: false,
            beforeSend: () => {
                this.$NAP.loadingBox.show();
            },
            complete: () => {
                this.$NAP.loadingBox.close();
            },
            success: (result: any) => {
                this.$NAP.loadingBox.show();

                var data = result.IPInfo;
                this.$contentTable.html('');

                if (data) {
                    var content = "<tr><td>IP Database</td><td>" + data.CountryCode + "</td><td>" +
                        data.CountryName + "</td><td>" +
                        data.Region + "</td><td>" +
                        data.City + "</td></tr>"

                    this.$contentTable.append(content);
                }

                this.$NAP.loadingBox.close();
            },
            error: (xhr: JQueryXHR, status: string, err: string) => {
                this.$NAP.handleAjaxError(xhr, status, err);
            }
        });
    };
}

const ipInformation = new IPInformation($, window.NAP, window.JSON);

$(() => {
    ipInformation.init();
});

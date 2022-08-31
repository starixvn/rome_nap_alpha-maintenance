"use strict";

interface Window {
    NAP: any;
    JSON: any;
}

class IPInformation {
    $: JQueryStatic;
    $NAP: any;
    $JSON: any;

    $txtIP: JQuery;
    $btnSubmit: JQuery;
    $contentTable: JQuery;
    $APIInfoJson: JQuery;

    constructor($: JQueryStatic, NAP: any, JSON: any) {
        this.$ = $;
        this.$NAP = NAP;
        this.$JSON = JSON;
    }

    init = () => {
        this.$txtIP = this.$('#txtIP');
        this.$btnSubmit = this.$('#btnSubmit');
        this.$contentTable = this.$('#content');
        this.$APIInfoJson = this.$('#APIInfoJson').val();

        this.bindEvent();
    };

    bindEvent = () => {
        this.$btnSubmit.click(() => {
            if (this.validateIPAddress()) {
                this.getIPInfo();
            }
        });

        this.$txtIP.keyup((e: JQueryKeyEventObject) => {
            if (e.which == 13) {
                this.$btnSubmit.trigger('click');
            }
        });
    };

    getIPInfo = () => {
        let ip = this.$txtIP.val();
        this.$.ajax({
            url: this.$NAP.resolveClientUrl("IPInformation/GetIPInfo"),
            data: { ip: ip },
            type: 'GET',
            dataType: 'json',
            cache: false,
            beforeSend: () => {
                this.$NAP.loadingBox.show();
            },
            success: (result: any) => {
                if (!result.IPInfos) {
                    return;
                }

                let content = "";
                this.$.each(result.IPInfos, (index,data) => {
                    var row = '<tr>\
                                    <td>' + data.DataSource + '<a href="' + data.Url + '" target="_blank" class="icon-link icon-link-variant"></a></td>\
                                    <td>' + data.CountryCode + '</td>\
                                    <td>' + data.CountryName + '</td>\
                                    <td>' + data.Region + '</td>\
                                    <td>' + data.City + '</td>\
                                </tr>';
                    content += row;
                });

                this.$contentTable.empty().append(content);
            },
            complete: () => {
                this.$NAP.loadingBox.close();
            },
            error: (xhr: JQueryXHR, status: string, err: string) => {
                this.$NAP.handleAjaxError(xhr, status, err);
            }
        });
    };

    validateIPAddress = () => this.$txtIP.val().trim() !== "";
}

const ipInformation = new IPInformation($, window.NAP, window.JSON);

$(() => {
    ipInformation.init();
});
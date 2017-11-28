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

    constructor($: JQueryStatic, NAP: any, JSON: any) {
        this.$ = $;
        this.$NAP = NAP;
        this.$JSON = JSON;
    }

    init = () => {
        this.$txtIP = this.$('#txtIP');
        this.$btnSubmit = this.$('#btnSubmit');
        this.$contentTable = this.$('#content');

        this.bindEvent();
    }

    bindEvent = () => {
        this.$btnSubmit.click(() => {
            if (!this.validateIPAddress()) {
                return;
            }

            this.getIPInfoFromDb();
        });

        this.$txtIP.keyup((e: JQueryKeyEventObject) => {
            if (!this.validateIPAddress()) {
                return;
            }

            if (e.which == 13) {
                this.$btnSubmit.trigger('click');
            }
        });
    }

    getIPInfoFromDb = () => {
        let ip = this.$txtIP.val();
        this.$.ajax({
            url: this.$NAP.resolveClientUrl("IPInformation/GetIPInfoFromDB"),
            data: { ip: ip },
            type: 'GET',
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
                    var content = '<tr><td>' + data.DataSource + '</td><td>' +
                        data.CountryCode + '</td><td>' +
                        data.CountryName + '</td><td>' +
                        data.Region + '</td><td>' +
                        data.City + '</td></tr>';

                    this.$contentTable.append(content);
                }

                this.getIPInfoFromAPIs(ip);
                this.$NAP.loadingBox.close();
            },
            error: (xhr: JQueryXHR, status: string, err: string) => {
                this.$NAP.handleAjaxError(xhr, status, err);
            }
        });
    };

    getIPInfoFromAPIs = (ip: string) => {
        this.$.ajax({
            url: this.$NAP.resolveClientUrl("IPInformation/GetIPInfoFromAPIs"),
            data: { ip: ip },
            type: 'GET',
            dataType: 'json',
            cache: false,
            beforeSend: () => {
                this.addWaitingRow();
            },
            success: (result: any) => {

                var data = result.ListIPInfo;

                if (data.length > 0) {
                    let content = '';
                    this.$.each(data, (key, value) => {
                        content += '<tr><td>' + value.DataSource + '<a href="' + value.WebLink + '" target="_blank" class="icon-link icon-link-variant"></a></td><td>' +
                            value.CountryCode + '</td><td>' +
                            value.CountryName + '</td><td>' +
                            value.Region + '</td><td>' +
                            value.City + '</td></tr>';

                    });
                    this.$contentTable.append(content);
                    this.removeWaitingRow();
                }
            },
            error: (xhr: JQueryXHR, status: string, err: string) => {
                this.$NAP.handleAjaxError(xhr, status, err);
            }
        });
    }

    validateIPAddress = () => {
        let ip = this.$txtIP.val().trim();
        if (ip === "") {
            return false;
        }
        else {
            return true;
        }
    };

    addWaitingRow = () => this.$contentTable.append('<tr class="waiting-row"><td class="center red-text" colspan="6">getting info from other data source ...</td></tr>')

    removeWaitingRow = () => this.$('tr.waiting-row').remove();
}

const ipInformation = new IPInformation($, window.NAP, window.JSON);

$(() => {
    ipInformation.init();
});

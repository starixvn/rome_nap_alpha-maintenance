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
        this.$btnSubmit.click(this.generateData);
    }

    generateData = () => {
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
                    var content = "<tr><td>" + ip + "</td><td>" +
                        data.DataSource + "</td><td>" +
                        data.CountryCode + "</td><td>" +
                        data.CountryName + "</td><td>" +
                        data.Region + "</td><td>" +
                        data.City + "</td></tr>";

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
                this.$NAP.loadingBox.show();
            },
            complete: () => {
                this.$NAP.loadingBox.close();
            },
            success: (result: any) => {
                this.$NAP.loadingBox.show();

                var data = result.ListIPInfo;

                if (data.length > 0) {
                    this.$.each(data, (key, value) => {
                        var content = "<tr><td>" + ip + "</td><td>" +
                            value.DataSource + "</td><td>" +
                            value.CountryCode + "</td><td>" +
                            value.CountryName + "</td><td>" +
                            value.Region + "</td><td>" +
                            value.City + "</td></tr>";

                        this.$contentTable.append(content);
                    });
                }

                this.$NAP.loadingBox.close();
            },
            error: (xhr: JQueryXHR, status: string, err: string) => {
                this.$NAP.handleAjaxError(xhr, status, err);
            }
        });
    }
}

const ipInformation = new IPInformation($, window.NAP, window.JSON);

$(() => {
    ipInformation.init();
});

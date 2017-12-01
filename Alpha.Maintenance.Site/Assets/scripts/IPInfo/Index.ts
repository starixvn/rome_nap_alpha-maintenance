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
            if (this.validateIPAddress()) {
                this.getIPInfo();
            }
        });

        this.$txtIP.keyup((e: JQueryKeyEventObject) => {
            if (e.which == 13) {
                this.$btnSubmit.trigger('click');
            }
        });
    }

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
            complete: () => {
                this.$NAP.loadingBox.close();
            },
            success: (result: any) => {
                var data = result.IPInfo;
                var apis = result.APIs;
                this.$contentTable.empty();

                if (data) {
                    var content = '<tr>\
                                        <td>' + data.DataSource + '</td>\
                                        <td>' + data.CountryCode + '</td>\
                                        <td>' + data.CountryName + '</td>\
                                        <td>' + data.Region + '</td>\
                                        <td>' + data.City + '</td>\
                                   </tr>';

                    this.$contentTable.append(content);
                }

                if (apis) {
                    this.getIPInfoFromAPIs(apis);
                }
            },
            error: (xhr: JQueryXHR, status: string, err: string) => {
                this.$NAP.handleAjaxError(xhr, status, err);
            }
        });
    };

    getIPInfoFromAPIs = (apis: any) => {
        let ip = this.$txtIP.val();

        if (apis.length === 0) {
            return;
        }

        this.renderWaitingRows(apis);

        this.$.each(apis, (key, value) => {
            let apiInfo = value;
            let url = apiInfo.APILink + ip;
            var ajax = this.$.ajax({
                url: url,
                type: 'GET',
                dataType: 'jsonp',
                cache: false,
                success: (result) => {
                    if (!result) {
                        return;
                    }

                    let webLink = apiInfo.WebLink + ip;
                    let content = '<tr>\
                                    <td>' + apiInfo.Name + '<a href="' + webLink + '" target="_blank" class="icon-link icon-link-variant"></a></td>\
                                    <td>' + this.formatValue(result[apiInfo.CountryCodeKey]) + '</td>\
                                    <td>' + this.formatValue(result[apiInfo.CountryNameKey]) + '</td>\
                                    <td>' + this.formatValue(result[apiInfo.RegionKey]) + '</td>\
                                    <td>' + this.formatValue(result[apiInfo.CityKey]) + '</td>\
                                </tr>';

                    this.$('[class="' + apiInfo.Name + '"]').replaceWith(content);
                }
            });

        });
    }

    renderWaitingRows = (apis: any) => {
        let content = '';
        this.$.each(apis, (key, value) => {
            let apiInfo = value;
            if (apiInfo) {
                content += '<tr class="' + apiInfo.Name + '">\
                                <td>' + apiInfo.Name + '<div class="loading"></div></td>\
                                <td></td>\
                                <td></td>\
                                <td></td>\
                                <td></td>\
                            </tr>';
            }
        });
        this.$contentTable.append(content);
    }

    validateIPAddress = () => this.$txtIP.val().trim() !== "";

    formatValue = (value: string) => (value && value.trim()) ? value : "-";
}

const ipInformation = new IPInformation($, window.NAP, window.JSON);

$(() => {
    ipInformation.init();
});

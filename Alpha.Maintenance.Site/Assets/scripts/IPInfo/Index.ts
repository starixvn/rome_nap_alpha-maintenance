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
        var apis = this.$JSON.parse(this.$APIInfoJson);
        this.initWaitingView(apis);

        this.getIPInfoFromDb();
        this.getIPInfoFromAPIs(apis);
    };

    initWaitingView = (apis: any) => {
        let content = this.renderWaitingRow('IPDB');

        this.$.each(apis, (key, apiInfo) => {
            if (apiInfo) {
                content += this.renderWaitingRow(apiInfo.Name);
            }
        });

        this.$contentTable.empty().append(content);
    };

    getIPInfoFromDb = () => {
        let ip = this.$txtIP.val();
        this.$.ajax({
            url: this.$NAP.resolveClientUrl("IPInformation/GetIPInfo"),
            data: { ip: ip },
            type: 'GET',
            dataType: 'json',
            cache: false,
            success: (result: any) => {
                var data = result.IPInfo;

                if (!data) {
                    return;
                }

                var row = '<tr>\
                                    <td>' + data.DataSource + '</td>\
                                    <td>' + data.CountryCode + '</td>\
                                    <td>' + data.CountryName + '</td>\
                                    <td>' + data.Region + '</td>\
                                    <td>' + data.City + '</td>\
                                </tr>';

                this.$('[class="IPDB"]').replaceWith(row);
            },
            error: (xhr: JQueryXHR, status: string, err: string) => {
                this.handleAjaxError(xhr, status, err, 'IPDB');
            }
        });
    };

    getIPInfoFromAPIs = (apis: any) => {
        let ip = this.$txtIP.val();

        this.$.each(apis, (key, apiInfo) => {
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
                },
                error: (xhr: JQueryXHR, status: string, err: string) => {
                    this.handleAjaxError(xhr, status, err, apiInfo.Name);
                }
            });
        });
    };

    renderWaitingRow = (name: string) => {
        let row = '<tr class="' + name + '">\
                            <td>'+ name + '<div class="loading"></div></td>\
                            <td></td>\
                            <td></td>\
                            <td></td>\
                            <td></td>\
                        </tr>';
        return row;
    };

    validateIPAddress = () => this.$txtIP.val().trim() !== "";

    formatValue = (value: string) => (value && value.trim()) ? value : "-";

    handleAjaxError = (xhr: JQueryXHR, status: string, err: string, dataSource: string) => {
        this.$NAP.handleAjaxError(xhr, status, err);

        let row = '<tr>\
                        <td>' + dataSource + '</td>\
                        <td>-</td>\
                        <td>-</td>\
                        <td>-</td>\
                        <td>-</td>\
                    </tr>';

        this.$('[class="' + dataSource + '"]').replaceWith(row);
    };
}

const ipInformation = new IPInformation($, window.NAP, window.JSON);

$(() => {
    ipInformation.init();
});

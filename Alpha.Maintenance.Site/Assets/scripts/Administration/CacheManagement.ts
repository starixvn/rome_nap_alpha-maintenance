"use strict";

interface JQuery {
    material_select(): void;
    jqte(): void;
    closeModal(): void;
    openModal(): void;
    dropify(): void;
}

interface Window {
    NAP: any;
}

class CacheManagement {
    $: JQueryStatic;
    $nap: any;

    constructor($: JQueryStatic, NAP: any) {
        this.$ = $;
        this.$nap = NAP;
    }

    init = () => {
        this.$('select').material_select();
        this.$('#submit').click(() => this.get());
        this.$('#del-document').click(() => this.delete());
    };

    get = () => {
        this.$('#error-message').hide();
        var model = this.generateModel();
        if (!model) return;

        this.$.ajax({
            url: this.$nap.resolveClientUrl("Administration/CacheManagement/Get"),
            data: JSON.stringify(model),
            type: 'POST',
            contentType: "application/json",
            dataType: "json",
            cache: false,
            beforeSend: () => {
                this.$nap.loadingBox.show();
            },
            complete: () => {
                this.$nap.loadingBox.close();
            },
            success: (result) => {
                var $resultTableBody = this.$("#result-table>tbody");
                $resultTableBody.empty();
                for (var i = 0; i < result.length; i++) {
                    $resultTableBody.append(
                        '<tr>' +
                        '<td>' + result[i].cacheKey + '</td>' +
                        '<td>' + result[i].cacheValue + '</td>' +
                        '</tr>');
                }
            },
            error: (xhr, status, err) => {
                this.$nap.handleAjaxError(xhr, status, err);
            }
        });
    }

    delete = () => {
        this.$('#error-message').hide();
        var model = this.generateModel();
        if (!model) return;

        this.$.ajax({
            url: this.$nap.resolveClientUrl("Administration/CacheManagement/Delete"),
            data: JSON.stringify(model),
            type: 'POST',
            contentType: "application/json",
            cache: false,
            beforeSend: () => {
                this.$nap.loadingBox.show();
            },
            complete: () => {
                this.$nap.loadingBox.close();
            },
            success: () => {
                this.$('#submit').click();
            },
            error: (xhr, status, err) => {
                this.$nap.handleAjaxError(xhr, status, err);
            }
        });
    }

    generateModel = () => {
        if (this.$('#keys').val() == undefined || this.$('#keys').val() == '') {
            this.$('#error-message').html("Keys cannot be empty.").show();
            return;
        }

        var keys = this.$('#keys').val().split(',');
        var filterKeys = [];
        for (var key of keys) {
            var trimKey = this.$.trim(key);
            if (trimKey) {
                filterKeys.push(trimKey);
            }
        }

        return {
            keys: filterKeys,
            pattern: this.$('#pattern').val(),
            siteGroup: this.$('#siteGroup').val()
        };
    }


}

const cacheManagement = new CacheManagement($, window.NAP);

$(() => {
    cacheManagement.init();
});
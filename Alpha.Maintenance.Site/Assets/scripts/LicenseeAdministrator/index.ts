"use strict";

declare var ace: any;

class LicenseeAdministration {
    $: JQueryStatic;
    $body: any;
    $response: any;
    samplePayloads = {
        "1": {
            url: "api/deposit/players",
            payload: "{\"userName\": \"string\",\"userID\": 2147483647,\"password\": \"string\",\"oddsType\": \"string\",\"agentId\": 2147483647,\"affiliateID\": 2147483647,\"country\": \"string\",\"subsidiaryID\": 2147483647,\"isDebug\": true}",
        },
        "2": {
            url: "api/deposit/shadowaccounts",
            payload: "{\"realCustID\": 2147483647,\"agentID\": 2147483647,\"isDebug\": true}",
        }
    };
    selectedAPI: any;
    $NAP: any;

    constructor($: JQueryStatic, NAP: any) {
        this.$ = $;
        this.$NAP = NAP;
    }

    init = () => {
        this.$('ul.tabs').tabs();
        this.$('select').material_select();
        this.$body = ace.edit('body', {
            mode: 'ace/mode/json',
            selectionStyle: 'text',
            showPrintMargin: false,
            theme: 'ace/theme/chrome'
        });

        this.$response = ace.edit('response', {
            mode: 'ace/mode/json',
            selectionStyle: 'text',
            showPrintMargin: false,
            theme: 'ace/theme/chrome'
        });

        const selectedApiId = this.$('#api').val();
        this.changePayload(selectedApiId);

        var self = this;
        this.$("#api").change(function () {
            const id = self.$(this).val();
            self.changePayload(id);
        });

        this.$("#submit").click(this.sendRequest);

        this.$body.on('paste', (event) => {
            try {
                this.$body.text = JSON.stringify(JSON.parse(event.text), null, 4)
            } catch (err) {
                // meh
            }
        });

        this.$("#btn-beautify").click(this.formatBody);
        this.$("#btnReactive").click(this.reactivePlayer);
    }

    formatBody = () => {
        try {
            const current = JSON.parse(this.$body.getValue())
            this.$body.setValue(JSON.stringify(current, null, 4))
            this.$body.focus()
            this.$body.selectAll()
            document.execCommand('copy')
        } catch (err) {
            alert('ERROR: Unable to parse text as JSON')
        }
    }

    changePayload = (id: string) => {
        this.selectedAPI = this.samplePayloads[id];
        const payload = JSON.parse(this.selectedAPI.payload);
        this.$body.setValue(JSON.stringify(payload, null, 4));
        this.$body.focus();
    }

    sendRequest = () => {
        this.$.ajax({
            url: this.$NAP.resolveClientUrl("LicenseeAdministration/Index"),
            data: { url: this.selectedAPI.url, payload: this.$body.getValue() },
            type: 'POST',
            dataType: 'json',
            cache: false,
            beforeSend: () => {
                this.$NAP.loadingBox.show();
            },
            complete: () => {
                this.$NAP.loadingBox.close();
            },
            success: (result, textStatus, xhr) => {
                this.$response.setValue(JSON.stringify(JSON.parse(result.result), null, 4));
                this.$response.focus();
                this.$("#statusCode").html(`${xhr.status}`);
            },
            error: (xhr, status, err) => {
                this.$NAP.handleAjaxError(xhr, status, err);
            }
        })
    }

    reactivePlayer = () => {
        this.$.ajax({
            url: this.$NAP.resolveClientUrl("LicenseeAdministration/Reactive"),
            data: {
                custId: this.$("#txtCustId").val(),
                subsidiaryID: this.$("#txtSubsidiaryID").val(),
                reinitOldBalance: this.$("#ChkBalance").val()
            },
            type: 'POST',
            dataType: 'json',
            cache: false,
            beforeSend: () => {
                this.$NAP.loadingBox.show();
            },
            complete: () => {
                this.$NAP.loadingBox.close();
            },
            success: (result, textStatus, xhr) => {
                this.$response.setValue(JSON.stringify(JSON.parse(result.result), null, 4));
                this.$response.focus();
                this.$("#statusCode").html(`${xhr.status}`);
            },
            error: (xhr, status, err) => {
                this.$NAP.handleAjaxError(xhr, status, err);
            }
        })
    }
}

$(() => {
    var licenseeAdministration = new LicenseeAdministration($, window.NAP);
    licenseeAdministration.init();
});
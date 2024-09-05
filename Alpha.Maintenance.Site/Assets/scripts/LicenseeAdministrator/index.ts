"use strict";

declare var ace: any;

class LicenseeAdministration {
    $: JQueryStatic;
    $body: any;
    $response: any;
    $NAP: any;
    $url: JQuery;

    constructor($: JQueryStatic, NAP: any) {
        this.$ = $;
        this.$NAP = NAP;
    }

    init() {
        this.$('ul.tabs').tabs();
        this.$('select').material_select();
        this.$url = this.$("#url");
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

        this.$body.on('paste', (event) => {
            try {
                this.$body.text = JSON.stringify(JSON.parse(event.text), null, 4)
            } catch (err) {
                // meh
            }
        });

        this.$body.setValue(JSON.stringify(JSON.parse(this.$("#payload").val()), null, 4));
        this.$body.focus();

        this.$("#btn-beautify").click(this.formatBody);
        this.$("#submit").click(this.sendRequest);
        this.$("#btnReactive").click(this.reactivePlayer);

        var self = this;
        this.$("#api").change(function () {
            const id = self.$(this).val();
            self.changePayload(id);
        });
    }

    changePayload = (id: string) => {
        this.$.ajax({
            url: this.$NAP.resolveClientUrl("LicenseeAdministration/GetSamplePageLoad?apiId=" + id),
            type: 'GET',
            dataType: 'json',
            cache: false,
            beforeSend: () => {
                this.$NAP.loadingBox.show();
            },
            complete: () => {
                this.$NAP.loadingBox.close();
            },
            success: (result, textStatus, xhr) => {
                if (result) {
                    this.$url.val(result.Url);
                    this.$body.setValue(JSON.stringify(JSON.parse(result.Payload), null, 4));
                    this.$body.focus();
                }
            },
            error: (xhr, status, err) => {
                this.$NAP.handleAjaxError(xhr, status, err);
            }
        })
    }

    getData() {
        return { url: this.$url.val(), payload: this.$body.getValue() };
    }

    sendRequest = () => {
        this.$.ajax({
            url: this.$NAP.resolveClientUrl("LicenseeAdministration/Index"),
            data: this.getData(),
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
                result.result && this.$response.setValue(JSON.stringify(JSON.parse(result.result), null, 4));
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
}

$(() => {
    var licenseeAdministrationBase = new LicenseeAdministration($, window.NAP);
    licenseeAdministrationBase.init();
});
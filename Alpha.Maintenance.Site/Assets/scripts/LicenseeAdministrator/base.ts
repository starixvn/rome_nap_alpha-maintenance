"use strict";

declare var ace: any;

class LicenseeAdministrationBase {
    $: JQueryStatic;
    $body: any;
    $response: any;
    $NAP: any;
    $url: string;

    constructor($: JQueryStatic, NAP: any) {
        this.$ = $;
        this.$NAP = NAP;
    }

    init() {
        this.$url = this.$("#url").val();
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
    }

    getData() {
        return { payload: this.$body.getValue() };
    }

    sendRequest = () => {
        this.$.ajax({
            url: this.$NAP.resolveClientUrl(this.$url),
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
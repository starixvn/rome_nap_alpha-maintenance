"use strict";

declare var ace: any;

class ReactivateDormantAccount {
    $: JQueryStatic;
    $response: any;
    $NAP: any;

    constructor($: JQueryStatic, NAP: any) {
        this.$ = $;
        this.$NAP = NAP;
    }

    init = () => {
        this.$response = ace.edit('response', {
            mode: 'ace/mode/json',
            selectionStyle: 'text',
            showPrintMargin: false,
            theme: 'ace/theme/chrome'
        });

        this.$("#btnReactive").click(this.reactivePlayer);
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
    var reactivateDormantAccount = new ReactivateDormantAccount($, window.NAP);
    reactivateDormantAccount.init();
});
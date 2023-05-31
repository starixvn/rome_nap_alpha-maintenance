"use strict";
declare var MaterialDatetimePicker;

interface JQuery {
    dropify(options): any;
}

interface HTMLElement {
    files: any
}

class AutoTransferJobConfig {
    $: JQueryStatic;
    nap: any;

    txtManualTime: JQuery;
    divMessage: JQuery;
    hasManualTime: boolean;
    site: string;

    viewResources: any = {
        requiredDefaultTimeError: "Please input default time.",
        updateSuccess: "Update successfully!"
    };

    constructor($: JQueryStatic, nap: any) {
        this.$ = $;
        this.nap = nap;
    }

    init(): void {
        const self = this;

        this.txtManualTime = this.$("#txt-manual-time");
        this.divMessage = this.$("#div-message");
        this.site = this.$("#Site").val();

        this.$("select").material_select();

        this.hasManualTime = this.txtManualTime.attr("data-hasvalue") === "1";
        this.initDateTimePicker(this.txtManualTime);
        this.$("#btnSubmit").click(function () {
            self.upddateConfig();
        });
    }

    private initDateTimePicker(dateTimePicker: JQuery) {
        const self = this;
        const dateTimeValue = new Date(dateTimePicker.val());

        const todate = new Date();

        const materialDateTimePicker = new MaterialDatetimePicker({
            default: dateTimeValue
        }).on("submit", function (d) {
            dateTimePicker.val(d.format("MMM DD,YYYY HH:mm"));
        }).on("close", function () { dateTimePicker.focus(); });

        dateTimePicker.on("click", function () {
            materialDateTimePicker.open();
        });

        if (!this.hasManualTime) {
            this.txtManualTime.val("");
        }
    }

    private upddateConfig() {
        const self = this;

        var param = {
            Site: this.site,
            DefaultTime: this.$("#txt-default-time").val(),
            RunningMode: this.$("#sel-running-mode").val(),
            ManualTime: this.$("#txt-manual-time").val(),
            IsActive: this.$("#chk-isactive").is(':checked')
        };

        if (param.DefaultTime.length === 0) {
            this.showErrorMessage(this.viewResources.requiredDefaultTimeError);
            return null;
        }

        this.$.ajax({
            url: this.nap.resolveClientUrl("AutoTransferJobConfig/Update"),
            type: "POST",
            dataType: "json",
            contentType: "application/json",
            processData: false,
            data: JSON.stringify(param),
            success: function (result) {
                if (result.IsOk) {
                    self.showSuccessMsg(self.viewResources.updateSuccess);
                } else {
                    self.showErrorMessage(result.Message);
                }
            },
            error: function (xhr) {
                if (xhr.status === 403) {
                    self.showErrorMessage(xhr.statusText);
                }
            },
            complete: function () { }
        });
    }

    private showSuccessMsg(message: string) {
        this.divMessage
            .html(message)
            .addClass("green green-text")
            .removeClass("red red-text hiden");
    }

    private showErrorMessage(error: string) {
        this.divMessage
            .html(error)
            .addClass("red red-text")
            .removeClass("green green-text hiden")
    }
}

$(() => {
    const config = new AutoTransferJobConfig($, window.NAP);
    config.init();
});
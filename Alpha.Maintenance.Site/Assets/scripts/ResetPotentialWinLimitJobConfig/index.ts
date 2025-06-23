"use strict";
declare var MaterialDatetimePicker;

interface JQuery {
    dropify(options): any;
}

interface HTMLElement {
    files: any
}

class ResetPotentialWinLimitJobConfig {
    $: JQueryStatic;
    nap: any;
    divMessage: JQuery;
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
        this.initCommon();
    }

    initCommon(): void {
        const self = this;
        this.divMessage = $(".div-message");
        var form = $('.filter-bar');

        var txtManualTime = form.find(".txt-manual-time");

        var hasManualTime = txtManualTime.attr("data-hasvalue") === "1";
        this.initDateTimePicker('txt-manual-time');
        this.initDateTimePicker('txt-running-date');
        this.initDateTimePicker('txt-completed-time');

        form.find(".btnSubmit").click(function () {
            self.updateConfig(form);
        });

    }

    private initDateTimePicker(dateTimePickerId: string) {
        const dateTimePicker = this.$("#" + dateTimePickerId);
        var hasManualTime = dateTimePicker.attr("data-hasvalue") === "1";
        const dateTimeValue = new Date(dateTimePicker.val());

        const materialDateTimePicker = new MaterialDatetimePicker({
            default: dateTimeValue
        }).on("submit", function (d) {
            dateTimePicker.val(d.format("MMM DD,YYYY HH:mm"));
        }).on("close", function () { dateTimePicker.focus(); });

        dateTimePicker.on("click", function () {
            materialDateTimePicker.open();
        });

        if (hasManualTime) {
            return;
        }

        dateTimePicker.val("");
    }

    private updateConfig(form: JQuery) {
        const self = this;
        var param = {
            Config: {
                DefaultTime: form.find(".txt-default-time").val(),
                ManualTime: form.find(".txt-manual-time").val(),
                IsActive: form.find(".chk-isactive").is(':checked'),
            },
            Status: {
                RunningDate: form.find(".txt-running-date").val(),
                CompletedTime: form.find(".txt-completed-time").val(),
                LastMaxCustID: form.find(".txt-last-max-cust-id").val(),
            }   
        };

        if (param.Config.DefaultTime.length === 0) {
            this.showErrorMessage(this.viewResources.requiredDefaultTimeError);
            return null;
        }

        this.$.ajax({
            url: this.nap.resolveClientUrl("ResetPotentialWinLimitJobConfig/Update"),
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
    const config = new ResetPotentialWinLimitJobConfig($, window.NAP);
    config.init();
});
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
        this.initEnv("UAT");
        this.initEnv("PRO");
        this.initCommon();
    }

    initCommon(): void {
        const self = this;
        this.divMessage = $(".div-message");
        this.site = this.$("#Site").val();
        this.$("select").material_select();

        this.$("select.site-dbs option").click(function () {
            var db = self.$(this).val();
            alert(db);
        });

        this.$("select.site-dbs").change(function () {
            var db = self.$(this).val();

            if (db !== self.site) {
                location.href = "./Index?site=" + db;
            }
        });
    }
    initEnv(env): void {
        const self = this;
        var form = $('.filter-bar-' + env);

        var txtManualTime = form.find(".txt-manual-time");
        var divMessage = form.find(".div-message");
        var hasManualTime = txtManualTime.attr("data-hasvalue") === "1";
        this.initDateTimePicker(txtManualTime, hasManualTime);
        form.find(".btnSubmit").click(function () {
            self.updateConfig(env, form);
        });
    }

    private initDateTimePicker(dateTimePicker: JQuery, hasManualTime: boolean) {
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

        if (!hasManualTime) {
            dateTimePicker.val("");
        }
    }

    private updateConfig(env: string, form: JQuery) {
        const self = this;
        var param = {
            Site: this.site,
            DefaultTime: form.find(".txt-default-time").val(),
            RunningMode: form.find("select.sel-running-mode").val(),
            ManualTime: form.find(".txt-manual-time").val(),
            IsActive: form.find(".chk-isactive").is(':checked'),
            Environment: env,
            TestSuperIds: form.find(".txt-test-super-ids").val(),
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
"use strict";
interface Window {
    NAP: any;
}

class BalanceRecord {
    $: JQueryStatic;
    nap: any;

    constructor($: JQueryStatic, nap: any) {
        this.$ = $;
        this.nap = nap;
    }

    private showSuccessMsg(env: string, message: string) {
        this.$("#divMessage" + env)
            .html(message)
            .addClass("green green-text")
            .removeClass("red red-text hiden");
    }

    private showErrorMessage(env: string, error: string) {
        this.$("#divMessage" + env)
            .html(error)
            .addClass("red red-text")
            .removeClass("green green-text hiden")
    }

    private hideMessage(env: string) {
        this.$("#divMessage" + env)
            .addClass("hiden")
            .html("");
    }

    private showConfirmPopup(env: string) {
        const confirmPopup = this.$("#cofirmMessages" + env);
        if (confirmPopup.length) {
            confirmPopup.openModal();
        }
    }

    private getFormData(env: string) {
        this.hideMessage(env);
        const self = this;
        let formData = [];
        $(".balance-record-job" + env).each(function () {
            const element = self.$(this);
            const checked = element.is(":checked");
            const newValue = checked ? "1" : "0";
            if (newValue !== element.attr("data-oldvalue")) {
                formData.push({
                    name: element.data("jobname"),
                    status: checked,
                    environment: element.data("environment"),
                    dbAlias: element.data("dbalias")
                });
            }
        });

        if (!formData.length) {
            this.showErrorMessage(env, "There is no change to update.");
            return;
        }

        return formData;
    }

    private buildAndShowResultsMessage(env, results) {
        let hasSuccessResult = false;
        let errors = [];
        for (let item of results) {
            if (item.IsOk) {
                hasSuccessResult = true;
            } else {
                errors.push(item.Message);
            }
        }

        if (errors.length) {
            const errorMessage = hasSuccessResult
                ? ("Updated successfully except " + errors.join("<br/>"))
                : errors.join("<br/>")
            this.showErrorMessage(env, errorMessage);
        } else {
            this.showSuccessMsg(env, "Updated successfully.");
            setTimeout(() => {
                window.location.href = window.location.href;
            }, 3000);
        }
    }

    private updateSettings(env: string, formData) {
        this.nap.loadingBox.show();
        this.$.ajax({
            url: this.nap.resolveClientUrl("BalanceRecord/Update"),
            data: JSON.stringify(formData),
            type: "POST",
            contentType: "application/json",
            success: (results) => {
                this.buildAndShowResultsMessage(env, results);
            },
            error: xhr => {
                if (xhr.status === 403) {
                    this.showErrorMessage(env, xhr.responseText);
                }
            },
            complete: this.nap.loadingBox.close(),
        });
    }

    init(): void {
        const self = this;
        $(".btn-save").click(function () {
            const env = self.$(this).data("environment");
            const formData = self.getFormData(env);
            formData && self.showConfirmPopup(env);
        });

        $(".btn-yes").click(function () {
            const env = self.$(this).data("environment");
            const formData = self.getFormData(env);
            formData && self.updateSettings(env, formData);
        });
    }
}

$(() => {
    new BalanceRecord($, window.NAP).init();
});
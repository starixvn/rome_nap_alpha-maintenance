"use strict";
interface Window {
    NAP: any;
}

class WinLossMonitor {
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

    private getFormData(env: string, isCreditJob: boolean) {
        this.hideMessage(env);
        let elements: JQuery[] = [];

        if (isCreditJob) {
            const lastIdElement = this.$("#txtLastId" + env);
            const lastIdValue = lastIdElement.val();
            if (!lastIdValue.length) {
                this.showErrorMessage(env, "Please input Last Id.");
                return;
            }

            const lastId = +lastIdValue;
            if (isNaN(lastId)) {
                this.showErrorMessage(env, "Please input Last Id with number format.");
                return;
            }

            elements.push(this.$("#chkAccumulateWinLossJob" + env));
            elements.push(lastIdElement);
        }

        const lastCheckTimeElement = this.$("#txtLastCheckTime" + env);
        const lastCheckTimeValue = lastCheckTimeElement.val();
        if (!lastCheckTimeValue.length) {
            this.showErrorMessage(env, "Please input Last Check Time.");
            return;
        }

        const lastCheckTime = Date.parse(lastCheckTimeValue);
        if (isNaN(lastCheckTime)) {
            this.showErrorMessage(env, "Please input Last Check Time with following format yyyy-MM-dd hh:mm:ss.ttt. For example 2022-07-25 05:05:23.627");
            return;
        }

        const lastScanSettingDateElement = this.$("#txtLastScanSettingDate" + env);
        const lastScanSettingDateValue = lastScanSettingDateElement.val();
        if (!lastScanSettingDateValue.length) {
            this.showErrorMessage(env, "Please input Last Scan Setting Date.");
            return;
        }

        const lastScanSettingDate = Date.parse(lastScanSettingDateValue);
        if (isNaN(lastScanSettingDate)) {
            this.showErrorMessage(env, "Please input Last Scan Setting Date with following format yyyy-MM-dd hh:mm:ss.ttt. For example 2022-07-25 05:05:23.627");
            return;
        }

        elements.push(this.$("#chkChangeAccumulatedWinLossJob" + env));
        elements.push(lastCheckTimeElement);
        elements.push(this.$("#chkChangeThresholdWinLossJob" + env));
        elements.push(lastScanSettingDateElement);
        elements.push(this.$("#txtSuperIds" + env));

        let formData = [];
        for (let item of elements) {
            const setting = this.getInputValue(item, env);
            if (setting) {
                formData.push(setting);
            }
        }

        if (!formData.length) {
            this.showErrorMessage(env, "There is no change to update.");
            return;
        }

        return formData;
    }

    private getInputValue(element: JQuery, env: string) {
        const inputType = element.attr("type");
        if (inputType === "checkbox") {
            return this.compareNewOldValue(element, element.is(":checked") ? "1" : "0", env);
        }

        if (inputType === "text") {
            return this.compareNewOldValue(element, element.val(), env);
        }
    }

    private compareNewOldValue(element: JQuery, value, env: string) {
        if (value !== element.attr("data-oldvalue")) {
            return {
                ServiceName: element.data("servicename") || this.$("#hiddenServiceName" + env).val(),
                SettingName: element.data("settingname"),
                SettingValue: value
            };
        }
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

    private updateGlobalSettings(env: string, formData) {
        this.nap.loadingBox.show();
        const self = this;
        this.$.post(
            this.nap.resolveClientUrl("WinLossMonitor/Update"),
            { globalSettings: formData, dbAlias: self.$("#hiddenDBAlias" + env).val() },
            function (results) {
                self.buildAndShowResultsMessage(env, results);
            })
            .fail(function (xhr) {
                if (xhr.status === 403) {
                    self.showErrorMessage(env, xhr.responseText);
                }
            })
            .always(function () {
                self.nap.loadingBox.close();
            });
    }

    init(): void {
        const self = this;
        $(".btn-save").click(function () {
            const env = self.$(this).data("environment");
            const isCreditJob = self.$("#hiddenIsCreditJob" + env).val() === "1";
            const formData = self.getFormData(env, isCreditJob);
            formData && self.showConfirmPopup(env);
        });

        $(".btn-yes").click(function () {
            const env = self.$(this).data("environment");
            const isCreditJob = self.$("#hiddenIsCreditJob" + env).val() === "1";
            const formData = self.getFormData(env, isCreditJob);
            formData && self.updateGlobalSettings(env, formData);
        });
    }
}

$(() => {
    const winLossMonitor = new WinLossMonitor($, window.NAP);
    winLossMonitor.init();
});
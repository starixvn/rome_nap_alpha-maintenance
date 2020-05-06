"use strict";
declare var MaterialDatetimePicker;

interface JQuery {
    dropify(options): any;
}

interface Window {
    NAP: any;
}

interface HTMLElement {
    files: any
}

class UploadBanner {
    $: JQueryStatic;
    nap: any;
    divMessage: JQuery;
    txtTitle: JQuery;
    chkActive: JQuery;
    dtpReleasedDate: JQuery;
    dtpExpirationDate: JQuery;
    txtMobileBgColor: JQuery;
    hiddenBannerId: number;
    uploadBannerURL: string;
    viewResources: any = {
        desktopBannerMsgDefault: "Drag and drop a desktop banner here or click",
        mobileBannerMsgDefault: "Drag and drop a mobile banner here or click",
        bannerMsgError: "Ooops, something wrong happended.",
        bannerErrorFileSize: "The file size is too big ({{ value }} max).",
        bannerErrorImageFormat: "Sorry, you choose invalid image extension ({{ value }} only).",
        desktopBannerErrorWidth: "The image width must be 1920px.",
        desktopBannerErrorHeight: "The image height must be 48px.",
        mobileBannerErrorWidth: "The image width must be 750px.",
        mobileBannerErrorHeight: "The image height must be 96px.",
        requiredTitleError: "Please input banner title.",
        invalidDateRangeError: "Expiration Date must be greater than Released Date.",
        requiredUploadedBannerError: "Please select image to upload.",
        invalidMobileBgColor: "Mobile Background (Hex Code) is invalid. Please input Hex code value."
    };

    constructor($: JQueryStatic, nap: any) {
        this.$ = $;
        this.nap = nap;
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

    private hideMessage() {
        this.divMessage
            .addClass("hiden")
            .html("");
    }

    private cacheDOMElements() {
        this.divMessage = this.$("#div-message");
        this.txtTitle = this.$("#txt-title");
        this.chkActive = this.$("#chk-active");
        this.dtpReleasedDate = this.$("#dtp-releasedDate");
        this.dtpExpirationDate = this.$("#dtp-expirationDate");
        this.txtMobileBgColor = this.$("#txt-mobileBgColor");
        this.hiddenBannerId = parseInt(this.$("#txt-bannerid").val());
        this.uploadBannerURL = this.nap.resolveClientUrl(this.hiddenBannerId ? "Banner/Edit" : "Banner/Add");
    }

    private initDateTimePicker(dateTimePicker: JQuery) {
        const self = this;
        const dateTimeValue = new Date(dateTimePicker.val());

        const materialDateTimePicker = new MaterialDatetimePicker({
            default: dateTimeValue
        }).on("submit", function (d) {
            dateTimePicker.val(d.format("MMM DD,YYYY HH:mm"));
        }).on("close", function () { dateTimePicker.focus(); });

        dateTimePicker.on("click", function () {
            materialDateTimePicker.open();
        });
    }

    private uploadBannerFormPost() {
        this.nap.loadingBox.show();
        const formData = this.getFormData();

        if (!formData) {
            this.nap.loadingBox.close();
            return;
        }

        const self = this;
        this.$.ajax({
            url: this.uploadBannerURL,
            type: "POST",
            dataType: "json",
            contentType: false,
            processData: false,
            data: formData,
            success: function (result) {
                if (result.IsOk) {
                    self.showSuccessMsg(result.Message);
                    setTimeout(() => {
                        window.location.href = self.nap.resolveClientUrl("Banner/Index");
                    }, 3000);
                } else {
                    self.showErrorMessage(result.Message);
                }
            },
            error: function (xhr) {
                if (xhr.status === 403) {
                    self.showErrorMessage(xhr.responseText);
                }
            },
            complete: function () {
                self.nap.loadingBox.close();
            }
        });
    }

    private getFormData() {
        const title = this.$.trim(this.txtTitle.val());

        if (title.length === 0) {
            this.showErrorMessage(this.viewResources.requiredTitleError);
            return null;
        }

        const releasedDate = this.$.trim(this.dtpReleasedDate.val());
        const expirationDate = this.$.trim(this.dtpExpirationDate.val());

        if (!releasedDate.length || !expirationDate.length || new Date(releasedDate) >= new Date(expirationDate)) {
            this.showErrorMessage(this.viewResources.invalidDateRangeError);
            return null;
        }

        const desktopBanners = this.getUploadedBanners("desktop-banner");
        const mobileBanners = this.getUploadedBanners("mobile-banner");

        if (!this.hiddenBannerId && this.$.isEmptyObject(desktopBanners) && this.$.isEmptyObject(mobileBanners)) {
            this.showErrorMessage(this.viewResources.requiredUploadedBannerError);
            return null;
        }

        const mobileBgColor = this.$.trim(this.txtMobileBgColor.val());

        if (mobileBgColor.length > 0 && !/^#([0-9A-F]{3}){1,2}$/i.test(mobileBgColor)) {
            this.showErrorMessage(this.viewResources.invalidMobileBgColor);
            return null;
        }

        this.hideMessage();

        let formData = new FormData();
        formData.append("title", title);
        formData.append("isActive", this.chkActive.is(":checked") ? "true" : "false");

        if (releasedDate.length) {
            formData.append("releasedDate", releasedDate);
        }

        if (expirationDate.length) {
            formData.append("expirationDate", expirationDate);
        }

        formData.append("environments", this.getCheckedCheckboxValues(this.$(".banner-environment:checked")));
        formData.append("sites", this.getCheckedCheckboxValues(this.$(".banner-site:checked")));

        Object.keys(desktopBanners).forEach(function (key) {
            formData.append(key, desktopBanners[key]);
        });

        Object.keys(mobileBanners).forEach(function (key) {
            formData.append(key, mobileBanners[key]);
        });

        formData.append("mobileBgColor", mobileBgColor);

        if (this.hiddenBannerId) {
            formData.append("id", this.hiddenBannerId.toString());
        }

        return formData;
    }

    private getCheckedCheckboxValues(checkboxes) {
        let values = [];
        checkboxes.each(function () { values.push(this.value); });

        return values.join(", ");
    }

    private getUploadedBanners(className) {
        let banner = {};
        const inputFiles = document.getElementsByClassName(className);

        for (let i = 0, max = inputFiles.length; i < max; i++) {
            let inputFile = <HTMLElement>inputFiles[i];
            if (inputFile.files.length > 0) {
                banner[this.$(inputFile).attr("name")] = inputFile.files[0]
            }
        }

        return banner;
    }

    init(): void {
        this.cacheDOMElements();

        this.initDateTimePicker(this.dtpReleasedDate);
        this.initDateTimePicker(this.dtpExpirationDate);

        this.$(".desktop-banner").dropify({
            messages: {
                default: this.viewResources.desktopBannerMsgDefault,
                error: this.viewResources.bannerMsgError
            },
            error: {
                fileSize: this.viewResources.bannerErrorFileSize,
                minWidth: this.viewResources.desktopBannerErrorWidth,
                maxWidth: this.viewResources.desktopBannerErrorWidth,
                minHeight: this.viewResources.desktopBannerErrorHeight,
                maxHeight: this.viewResources.desktopBannerErrorHeight,
                imageFormat: this.viewResources.bannerErrorImageFormat,
                fileExtension: this.viewResources.bannerErrorImageFormat
            }
        });
        this.$(".mobile-banner").dropify({
            messages: {
                default: this.viewResources.mobileBannerMsgDefault,
                error: this.viewResources.bannerMsgError
            },
            error: {
                fileSize: this.viewResources.bannerErrorFileSize,
                minWidth: this.viewResources.mobileBannerErrorWidth,
                maxWidth: this.viewResources.mobileBannerErrorWidth,
                minHeight: this.viewResources.mobileBannerErrorHeight,
                maxHeight: this.viewResources.mobileBannerErrorHeight,
                imageFormat: this.viewResources.bannerErrorImageFormat,
                fileExtension: this.viewResources.bannerErrorImageFormat
            }
        });

        const self = this;
        this.$("#btn-upload").click(function () {
            self.uploadBannerFormPost();
        });
    }
}

$(() => {
    const uploadBanner = new UploadBanner($, window.NAP);
    uploadBanner.init();
});
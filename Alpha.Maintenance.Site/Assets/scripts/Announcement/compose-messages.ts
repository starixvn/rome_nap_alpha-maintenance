"use strict";

interface JQuery {
    material_select(): void;
    material_select(option: any): void;
    jqte(): void;
    jqteVal(text: string): void;
    modal(): void;
    closeModal(): void;
    sort(option: any): void;
    remove(): void;
    validate(): void;
    openModal(): void;
    dropify(): void;
}

interface Window {
    NAP: any;
    JSON: any;
}

class ComposedAnnouncement {
    $: JQueryStatic;

    $divChooseSpecification: JQuery;
    $hiddenSpec: JQuery;
    $btnNext: JQuery;
    $btnPrev: JQuery;
    $btnSave: JQuery;
    $btnCopy: JQuery;
    $txttitle: JQuery;
    $specSettingTab: JQuery;
    $specInformationTab: JQuery;
    $numberOfSpecs: JQuery;
    $btnAgree: JQuery;
    $confirmMessage: JQuery;

    dllChooseSpecification: string;
    dllConjunctionOperator: string;
    visibleSpecDivs: string;
    txtSpecificationValue: string;
    userLevelsSpecificationValue: string;
    warningTilteMessage: string;
    saveAnnouncementFailedMessage: string;
    actionType: string;
    defaultConjunction: number;

    manageAnnoucementUrl: string;
    composeAnnouncementUrl: string;
    insertAnnouncementUrl: string;
    updateAnnoucementUrl: string;

    confirmSaveAnnouncementContent: string;
    confirmMakeACopyAnnouncementContent: string;
    fillOutAllSpecsInput: string;

    $nap: any;
    $JSON: any;

    constructor($: JQueryStatic, NAP: any, JSON: any) {
        this.$ = $;
        this.$nap = NAP;
        this.$JSON = JSON;
    }

    init = () => {
        this.$('ul.tabs').tabs();
        this.$('select').material_select();
        this.$("#content-group").removeClass("hiden");
        this.$("#specs-setting").removeClass("hiden");
        this.$(".language-tab").first().find("a").click();
        this.cacheDom();
        this.bindEvent();
        this.correctFirstSpec();
        this.initSpecOptionsDll();
        this.$('.jqte-test').jqte();
    };

    cacheDom = () => {
        this.$divChooseSpecification = this.$("#div-choose-specification");
        this.$hiddenSpec = this.$('.hidden-specification');
        this.$btnNext = this.$(".btnNext");
        this.$btnPrev = this.$(".btnPrev");
        this.$btnSave = this.$(".btnSave");
        this.$btnCopy = this.$(".btnCopy");
        this.$specSettingTab = this.$("#tab-specs-setting");
        this.$specInformationTab = this.$("#tab-information-settings");
        this.$numberOfSpecs = this.$('#number-of-specifications');
        this.$txttitle = this.$('#txttitle');
        this.$btnAgree = this.$("#agree");
        this.$confirmMessage = this.$("#cofirmMessages");

        this.dllChooseSpecification = "#dll-choose-specification";
        this.visibleSpecDivs = "div[name='div-specification']:not(.hiden)";
        this.dllConjunctionOperator = "#dll-specification-conjunctionOperator";
        this.userLevelsSpecificationValue = '#dll-specification-value';
        this.txtSpecificationValue = '#txt-specification-value';
        this.actionType = this.$('#actionType').val();
        this.defaultConjunction = 1;

        this.manageAnnoucementUrl = "Announcement/ManageAnnouncements";
        this.composeAnnouncementUrl = "Announcement/ComposeMessage";
        this.insertAnnouncementUrl = "Announcement/InsertAnnouncement";
        this.updateAnnoucementUrl = "Announcement/UpdateAnnouncement";

        this.confirmMakeACopyAnnouncementContent = "Are you sure you want to make a copy of this announcement ?";
        this.confirmSaveAnnouncementContent = "Are you sure you want to save this announcement ?";
        this.warningTilteMessage = "Please input announcement title !";
        this.fillOutAllSpecsInput = "You must fill out all fields before submit.";
        this.saveAnnouncementFailedMessage = "Save announcements failed."
    };

    bindEvent = () => {
        let self = this;

        this.$btnNext.on('click', function () {
            self.$specSettingTab.click();
        });

        this.$btnPrev.on('click', function () {
            self.$specInformationTab.click();
        });

        this.$(this.$btnCopy).on('click', function () {
            if (self.validateSpecificationInput()) {
                self.$(self.$confirmMessage).openModal();
                self.btnCopyClickOn();
            }
        });

        this.$(this.$btnSave).on('click', function () {
            if (self.validateTitle() && self.validateSpecificationInput()) {
                self.$(self.$confirmMessage).openModal();
                self.btnSaveClickOn();
            }
        });

        this.$(this.dllChooseSpecification).on('change', function () {
            self.onChangeSpecOption();
        });

        this.$hiddenSpec.on('click', function () {
            self.deleteSpec(this);
        });

        this.$(this.$btnAgree).on('click', function () {
            self.$(self.$confirmMessage).closeModal();
            self.submit();
        });

        this.$txttitle.on('keyup', function () {
            self.validateTitle();
        });

        this.$("input[name='txt-specification-value']").on('keyup', function () {
            self.validateSpecificationInput();
        });

        this.$("select[name='dll-specification-value']").on('change', function () {
            self.validateSpecificationInput();
        });
    };

    hideError = () => this.$('.card-panel').addClass('hiden');

    showWarningMessage = (message: string) => this.$('.card-panel').html(message).addClass("red red-text").removeClass('hiden');

    validateSpecificationInput = () => {
        let self = this;
        let $empty = this.$(this.visibleSpecDivs).filter(function () {
            if (self.$(this).find(self.txtSpecificationValue).length > 0) {
                return self.$(this).find(self.txtSpecificationValue).val() === "";
            }

            if (self.$(this).find(self.userLevelsSpecificationValue).length > 0) {
                return self.$(this).find(self.userLevelsSpecificationValue).val() == null;
            }
        });

        if ($empty.length) {
            this.showWarningMessage(this.fillOutAllSpecsInput);
            this.$specSettingTab.click();
            return false;
        }
        else {
            this.hideError();
            return true;
        };
    };

    validateTitle = () => {
        let title = this.$("#txttitle").val().trim();
        if (title === "") {
            this.showWarningMessage(this.warningTilteMessage);
            this.$specInformationTab.click();
            return false;
        }
        else {
            this.hideError();
            return true;
        }
    };

    btnCopyClickOn = () => {
        this.$("#announcementUrl").val(this.insertAnnouncementUrl);
        this.$("#isCopy").val("true");
        this.$("#cofirmMessages-content").html(this.confirmMakeACopyAnnouncementContent);
    };

    btnSaveClickOn = () => {
        let url = this.actionType == "Create" ? this.insertAnnouncementUrl : this.updateAnnoucementUrl;
        this.$("#announcementUrl").val(url);
        this.$("#isCopy").val("false");
        this.$("#cofirmMessages-content").html(this.confirmSaveAnnouncementContent);
    };

    onChangeSpecOption = () => {
        this.changeSpecPositions();
        this.reRenderSpecOptions();
        this.correctFirstSpec();
        this.toogleAddSpecButton();
    };

    changeSpecPositions = () => {
        let $spec = this.$("#" + this.$(this.dllChooseSpecification).find('option:selected').val());
        $spec.find(this.dllConjunctionOperator).removeClass("hiden");
        $spec.removeClass("hiden");
        $spec.insertBefore(this.$divChooseSpecification);
    };

    reRenderSpecOptions = () => {
        this.$(this.dllChooseSpecification).find('option:selected').remove();
        this.initSpecOptionsDll();
    };

    correctFirstSpec = () => {
        let $firstDiv = this.$(this.visibleSpecDivs).first();
        $firstDiv.find(this.dllConjunctionOperator).addClass("hiden");
    };

    initSpecOptionsDll = () => {
        let self = this;
        var options = this.$(this.dllChooseSpecification + " option");
        options.detach().sort(function (a, b) {
            var at = self.$(a).text();
            var bt = self.$(b).text();
            return (at > bt) ? 1 : ((at < bt) ? -1 : 0);
        });
        options.appendTo(this.dllChooseSpecification);

        this.$(this.dllChooseSpecification).material_select('destroy');
        this.$(this.dllChooseSpecification).val("").material_select();
    }

    search = (jsonobject: any, valueList: any) => {
        return this.$.map(jsonobject.ValueRange, function (obj) {
            for (let i = 0; i < valueList.length; i++) {
                if (obj.Id == valueList[i]) {
                    return obj;
                }
            }
        });
    };

    deleteSpec = (element: any) => {
        this.$divChooseSpecification.addClass("hiden");

        if (this.$(element).data("id") != null &&
            this.$(this.dllChooseSpecification + " option[value=" + this.$(element).data("id") + "]").length == 0) {
            this.$(this.dllChooseSpecification).append(this.$('<option>', {
                value: this.$(element).data("id"),
                text: this.$(element).data("name")
            }));
        }

        this.initSpecOptionsDll();
        this.$(element).parent('div').addClass("hiden");

        this.correctFirstSpec();
        this.toogleAddSpecButton();
        this.validateSpecificationInput();
    };

    submit = () => {
        let announcement = this.getAnnouncementParam();
        let self = this;
        var formdata = new FormData();
        formdata.append("announcement", self.$JSON.stringify(announcement));
        formdata.append("isCopy", self.$("#isCopy").val());
        let url = self.$("#announcementUrl").val();

        this.$.ajax({
            url: self.$nap.resolveClientUrl(url),
            type: 'POST',
            dataType: 'json',
            data: formdata,
            contentType: false,
            processData: false,
            beforeSend: function () { },
            complete: function () {
                self.$nap.loadingBox.close();
            },
            success: function (result) {
                self.onSuccess(result, url, announcement.id);
            },
            error: function (xhr, status, err) {
                self.$nap.handleAjaxError(xhr, status, err);
                self.$('.card-panel').html(self.saveAnnouncementFailedMessage).addClass("red red-text").removeClass('green green-text hiden');
            }
        });
    };

    onSuccess = (result: any, url: string, announcementId: string) => {
        let self = this;
        if (result.Code > 0) {
            this.$('.card-panel').html(result.Message).addClass("red red-text").removeClass('green green-text hiden');
        }
        else {
            this.$('.card-panel').html(result.Message).addClass("green green-text").removeClass('red red-text hiden');
            setTimeout(() => {
                let url = self.insertAnnouncementUrl ? this.manageAnnoucementUrl : `${this.composeAnnouncementUrl}/${announcementId}`;
                window.location.href = this.$nap.resolveClientUrl(url)
            }, 3000);
        }
    };

    getAnnouncementParam = () => {
        let self = this;
        return {
            id: self.$('#announcementId').val(),
            code: self.$("#txtcode").val().trim(),
            title: self.$("#txttitle").val().trim(),
            isActive: self.$("#ChkActive").is(":checked"),
            announcementType: self.$('input[name=group-popup]:checked').val(),
            contents: self.getContents(),
            environments: self.getEnvironments(),
            fromdate: self.$('#FromDate').val(),
            todate: self.$('#ToDate').val(),
            ordinal: self.$('#group-order option:selected').val(),
            systems: self.getSystems(),
            specifications: self.getSpecs()
        };
    };

    toogleAddSpecButton = () => this.$(this.visibleSpecDivs).length == this.$(this.$numberOfSpecs).val() ? this.$divChooseSpecification.addClass("hiden") : this.$divChooseSpecification.removeClass("hiden");

    getSystems = () => {
        return this.$("input[name='group-systems']:checked").map(function () {
            return this.id;
        }).get();
    };

    getEnvironments = () => {
        return this.$("input[name='group-environments']:checked").map(function () {
            return this.id;
        }).get();
    };

    getContents = () => {
        let self = this;
        let contents = [];

        this.$(".language-content").map(function (index, domElement) {
            let contentText = self.$(this).find('textarea');
            let contentUrl = self.$(this).find('INPUT');

            contents.push({
                LanguageKey: self.$(contentText).attr('id'),
                Text: self.$(contentText).val(),
                Url: self.$(this).find('#txtannouncement-url').val()
            });
        });

        return contents;
    };

    getSpecs = () => {
        let self = this;
        let specs = [];

        this.$(this.visibleSpecDivs).map(function (index, domElement) {
            let jsonObject = self.$(domElement).data("jsonobject");

            jsonObject.ConjunctionOperator = self.defaultConjunction;
            jsonObject.SpecificationOperator = self.getOperatorValueById(domElement, "#dll-specification-specificationOperator");

            if (self.$(domElement).find(self.txtSpecificationValue).length > 0) {
                let values = self.$(domElement).find(self.txtSpecificationValue).val();
                jsonObject.Value = self.splitStringtoArray(values);
            }

            if (self.$(domElement).find(self.userLevelsSpecificationValue).length > 0) {
                jsonObject.Value = self.$(domElement).find(self.userLevelsSpecificationValue).val();
            }

            if (self.$(domElement).find("input[type=checkbox]").length > 0) {
                jsonObject.Value = self.$(domElement).find("input[type=checkbox]").prop("checked");
            }

            specs.push(jsonObject);
        });

        return specs;
    };

    splitStringtoArray = (value: string) => {
        return value.match(/(?:\\,|[^,])+/g);
    };

    getOperatorValueById = (domElement: any, id: string) => {
        let operator = this.$(domElement).find(id);
        return this.$(operator).find('option:selected').val();
    };
}

const composedAnnouncement = new ComposedAnnouncement($, window.NAP, window.JSON);

$(() => {
    composedAnnouncement.init();
});

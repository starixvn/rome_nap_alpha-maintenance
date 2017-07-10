"use strict";

interface JQuery {
    material_select(): void;
    material_select(option: any): void;
    jqte(): void;
    jqteVal(text: string): void;
}

interface Window {
    NAP: any;
    JSON: any;
    utf8: any;
    base64: any;
}

class ComposedAnnouncement {
    $: JQueryStatic;
   
    $divChooseSpecification: JQuery;
    $hiddenSpec: JQuery;
    $btnNext: JQuery;
    $btnPrev: JQuery;
    $txttitle: JQuery;
    $specSettingTab: JQuery;
    $specInformationTab: JQuery;
    $numberOfSpecs: JQuery;

    dllChooseSpecification: string;
    dllConjunctionOperator: string;
    btnSave: string;
    visibleSpecDivs: string;   
    txtSpecificationValue: string;
    userLevelsSpecificationValue: string;

    $nap: any;
    $JSON: any;
    $utf8: any;
    $base64: any;

    constructor($: JQueryStatic, NAP: any, JSON: any, utf8: any, base64: any) {
        this.$ = $;
        this.$nap = NAP;
        this.$JSON = JSON;
        this.$utf8 = utf8;
        this.$base64 = base64;
    }

    init = () => {
        this.$('ul.tabs').tabs();
        this.$('select').material_select();
        this.$("#content-group").removeClass("hiden");
        this.cacheDom();
        this.bindEvent();
        this.correctFirstSpec();
        this.initSpecOptionsDll();
        this.$('.jqte-test').jqte();
    };

    cacheDom = () => {
       
        this.$divChooseSpecification = this.$("#div-choose-specification");
        this.$hiddenSpec = this.$('.hidden-specification');
        this.$btnNext = this.$("#btnNext");
        this.$btnPrev = this.$("#btnPrev");
        this.$specSettingTab = this.$("#tab-specs-setting");
        this.$specInformationTab = this.$("#tab-information-settings");
        this.$numberOfSpecs = this.$('#number-of-specifications');
        this.$txttitle = this.$('#txtcode');

        this.dllChooseSpecification = "#dll-choose-specification";
        this.visibleSpecDivs = "div[name='div-specification']:not(.hiden)";
        this.dllConjunctionOperator = "dll-specification-conjunctionOperator";
        this.btnSave = "INPUT[name='btnSave']";
        this.userLevelsSpecificationValue = '#dll-specification-value';
        this.txtSpecificationValue = '#txt-specification-value';
    };

    bindEvent = () => {
        let self = this;

        this.bindingContents();
       
        this.$(this.dllChooseSpecification).on('change', function () {
            self.onChangeSpecOption();
        });

        this.$hiddenSpec.on('click', function () {
            self.deleteSpec(this);
        });

        this.$(this.btnSave).on('click', function () {
            self.submit();
        });

        this.$btnNext.on('click', function () {
            self.$specSettingTab.click();
        });

        this.$btnPrev.on('click', function () {
            self.$specInformationTab.click();
        });

        this.$txttitle.on('keyup', function () {
            if (self.$(this).val().trim() != '') {
                self.$('.card-panel').addClass('hiden');
            }
        });
    };

    bindingContents = () => {
        let self = this;
        this.$(".language-content").map(function (index, domElement) {
            let $textarea = self.$(this).find('textarea')
            let contentText = $textarea.data("content");
            let decodecontext = self.$utf8.decode(self.$base64.decode(contentText));
            $textarea.html(decodecontext);
        });
    };

    onChangeSpecOption = () => {
        this.changeSpecPositions();
        this.reRenderSpecOptions();
        this.correctFirstSpec();
        this.toogleAddSpecButton();
    };

    changeSpecPositions = () => {
        let $spec = this.$("#" + this.$(this.dllChooseSpecification).find('option:selected').val());
        $spec.find("#" + this.dllConjunctionOperator).parent().removeClass("hiden");

        $spec.removeClass("hiden");
        $spec.insertBefore(this.$divChooseSpecification);
    };

    reRenderSpecOptions = () => {
        this.$(this.dllChooseSpecification).find('option:selected').remove();
        this.initSpecOptionsDll();
    };

    correctFirstSpec = () => {
        let $firstDiv = this.$(this.visibleSpecDivs).first();
        $firstDiv.find("#" + this.dllConjunctionOperator).find('option:selected').val(0);
        $firstDiv.find("#" + this.dllConjunctionOperator).parent().addClass("hiden");
    };

    initSpecOptionsDll = () => {
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
    };

    submit = () => {
        let announcement = this.getAnnouncementParam();

        if (announcement.title != '') {
            this.doUpdate(announcement);
        }
        else {
            this.showWarningMessage("Please input announcement title !");
        }
    };

    showWarningMessage = (message: string) => this.$('.card-panel').html(message).addClass("red red-text").removeClass('hiden');

    doUpdate = (announcement: any) => {
        let self = this;
        let file = <HTMLInputElement>this.$("#fileUpload")[0];
        var formdata = new FormData();
        formdata.append("file", file.files[0]);
        formdata.append("announcement", self.$JSON.stringify(announcement));
        formdata.append("action", self.$('#actionType').val());

        this.$.ajax({
            url: self.$nap.resolveClientUrl("Announcement/InsertAnnouncement"),
            type: 'POST',
            dataType: 'json',
            data: formdata,
            contentType: false,
            processData: false,
            beforeSend: function () {
                self.$nap.loadingBox.show();
            },
            complete: function () {
                self.$nap.loadingBox.close();
            },
            success: function (result) {
                let actionType = self.$('#actionType').val();
                let message = actionType == 'Create' ? 'Created successfully !' : 'Updated successfully !';
                self.$('.card-panel').html(message).addClass("green green-text").removeClass('red red-text hiden');
                setTimeout(() => {
                    self.refreshPage(actionType, announcement.id);
                }, 3000);
            },
            error: function (xhr, status, err) {
                self.$nap.handleAjaxError(xhr, status, err);
            }
        });
    };

    refreshPage = (actionType: string, announcementId: string) => {
        let url = "Announcement/ComposeMessage/" + (actionType == 'Create' ? '' : announcementId);
        window.location.href = this.$nap.resolveClientUrl(url)
    };

    getAnnouncementParam = () => {
        let self = this;
        return {
            id: self.$('#AnnouncementId').val(),
            code: self.$("#txtcode").val().trim(),
            title: self.$("#txttitle").val().trim(),
            isActive: self.$("#ChkActive").is(":checked"),
            announcementType: self.$('input[name=group-popup]:checked').val(),
            contents: self.getContents(),
            environments: self.getEnvironments(),
            fromdate: self.$('#FromDate').val(),
            todate: self.$('#ToDate').val(),
            order: self.$('#group-order option:selected').val(),
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
                Text: self.$base64.encode(self.$utf8.encode(self.$(contentText).val())),
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

            jsonObject.ConjunctionOperator = self.getConjunctionOperatorValue(domElement);
            jsonObject.SpecificationOperator = self.getSpecificationOperatorValue(domElement);

            if (self.$(domElement).find(self.txtSpecificationValue).length > 0) {
                let sites = self.$(domElement).find(self.txtSpecificationValue).val();
                jsonObject.Value = self.splitStringtoArray(sites);
            }

            if (self.$(domElement).find(self.userLevelsSpecificationValue).length > 0) {
                jsonObject.Value = self.$(domElement).find(self.userLevelsSpecificationValue).val();
            }

            specs.push(jsonObject);
        });

        return specs;
    };

    splitStringtoArray = (value: string) => {
        return value.match(/(?:\\,|[^,])+/g);
    };

    getConjunctionOperatorValue = (domElement: any) => {
        let object = this.$(domElement).find("#dll-specification-conjunctionOperator");
        return this.$(object).find('option:selected').val();
    };

    getSpecificationOperatorValue = (domElement: any) => {
        let object = this.$(domElement).find("#dll-specification-specificationOperator");
        return this.$(object).find('option:selected').val();
    };
}

const composedAnnouncement = new ComposedAnnouncement($, window.NAP, window.JSON, window.utf8, window.base64);

$(() => {
    composedAnnouncement.init();
});
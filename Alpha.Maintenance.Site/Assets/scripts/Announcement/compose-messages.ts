"use strict";

interface JQuery {
    material_select(): void;
    material_select(option: any): void;
    jqte(): void;
}

interface Window {
    NAP: any;
    JSON: any;
}

class ComposedAnnouncement {
    $: JQueryStatic;
    $dllChooseSpecification: string;
    $divChooseSpecification: JQuery;
    $dllConjunctionOperator: string;
    $btnAddSpec: JQuery;
    $btnSave: string;
    $hiddenSpec: JQuery;
    $btnNext: JQuery;
    $btnPrev: JQuery;
    $specSettingTab: JQuery;
    $specInformationTab: JQuery;
    $visibleSpecDivs: string;
    $numberOfSpecs: JQuery;

    $nap: any;
    $JSON: any;

    constructor($: JQueryStatic, NAP: any, JSON: any) {
        this.$ = $;
        this.$nap = NAP;
        this.$JSON = JSON;
    }

    init = () => {
        this.$('.jqte-test').jqte();
        this.$('ul.tabs').tabs();
        this.$('select').material_select();
        this.$("#content-group").removeClass("hiden");
        this.cacheDom();
        this.bindEvent();
        this.correctFirstSpec();
    };

    cacheDom = () => {
        this.$dllChooseSpecification = "#dll-choose-specification";
        this.$divChooseSpecification = this.$("#div-choose-specification");
        this.$dllConjunctionOperator = "dll-specification-conjunctionOperator";
        this.$btnAddSpec = this.$("#add-spec");
        this.$btnSave = "INPUT[name='btnSave']";
        this.$hiddenSpec = this.$('.hidden-specification');
        this.$btnNext = this.$("#btnNext");
        this.$btnPrev = this.$("#btnPrev");
        this.$specSettingTab = this.$("#tab-specs-setting");
        this.$specInformationTab = this.$("#tab-information-settings");
        this.$visibleSpecDivs = "div[name='div-specification']:not(.hiden)";
        this.$numberOfSpecs = this.$('#number-of-specifications');
    };

    bindEvent = () => {
        let self = this;

        this.$(this.$dllChooseSpecification).on('change', function () {
            self.onChangeSpecOption(this);
        });

        this.$btnAddSpec.on('click', function () {
            self.addSpec(this);
        });

        this.$hiddenSpec.on('click', function () {
            self.deleteSpec(this);
        });

        this.$(this.$btnSave).on('click', function () {
            self.submit();
        });

        this.$btnNext.on('click', function () {
            self.$specSettingTab.click();
        });
        
        this.$btnPrev.on('click', function () {
            self.$specInformationTab.click();
        });
    };

    onChangeSpecOption = (element: any) => {
        this.changeSpecPositions(element);
        this.reRenderSpecOptions(element);
        this.correctFirstSpec();
        this.toogleAddSpecButton();
    };

    changeSpecPositions = (element: any) => {
        let $spec = this.$("#" + this.$(element).find('option:selected').val());
        $spec.find("#" + this.$dllConjunctionOperator).parent().removeClass("hiden");

        $spec.removeClass("hiden");
        $spec.insertBefore(this.$divChooseSpecification);
        this.$divChooseSpecification.addClass("hiden");
    };

    reRenderSpecOptions = (element: any) => {
        this.$(element).find('option:selected').remove();
        this.$(element).material_select('destroy');
        this.$(element).material_select();
    };

    correctFirstSpec = () => {
        let $firstDiv = this.$(this.$visibleSpecDivs).first();
        $firstDiv.find("#" + this.$dllConjunctionOperator).find('option:selected').val(0);
        $firstDiv.find("#" + this.$dllConjunctionOperator).parent().addClass("hiden");
    };

    search = (jsonobject: any, valueList: any) => {
        return this.$.map(jsonobject.ValueRange, function (obj) {
            for (let i = 0; i < valueList.length; i++) {
                if (obj.Id == valueList[i]) {
                    return obj;
                }
            }
        });
    };

    addSpec = (element: any) => {
        this.$divChooseSpecification.removeClass("hiden");
        this.$(element).addClass("hiden");
    };

    deleteSpec = (element: any) => {
        this.$divChooseSpecification.addClass("hiden");
        this.$(this.$dllChooseSpecification).material_select('destroy');

        if (this.$(element).data("id") != null &&
            this.$(this.$dllChooseSpecification + " option[value=" + this.$(element).data("id") + "]").length == 0) {
            this.$(this.$dllChooseSpecification).append(this.$('<option>', {
                value: this.$(element).data("id"),
                text: this.$(element).data("name")
            }));
        }

        this.$(this.$dllChooseSpecification).material_select();
        this.$(element).parent('div').addClass("hiden");

        this.correctFirstSpec();
        this.toogleAddSpecButton();
    };

    submit = () => {
        let announcement = this.getAnnouncementParam();

        this.$.ajax({
            url: this.$nap.resolveClientUrl("Announcement/InsertAnnouncement"),
            type: 'POST',
            data: { "announcement": this.$JSON.stringify(announcement), action: this.$('#actionType').val() },
            dataType: 'json',
            beforeSend: function () {
                this.$nap.loadingBox.show();
            },
            complete: function () {
                this.$nap.loadingBox.close();
            },
            success: function (result) {

            },
            error: function (xhr, status, err) {
                this.$nap.handleAjaxError(xhr, status, err);
            }
        });
    };

    getAnnouncementParam = () => {
        let self = this;
        return {
            id: self.$('#AnnouncementId').val(),
            code: self.$("#txtcode").val(),
            title: self.$("#txttitle").val(),
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

    toogleAddSpecButton = () => this.$(this.$visibleSpecDivs).length == this.$(this.$numberOfSpecs).val() ? this.$btnAddSpec.addClass("hiden") : this.$btnAddSpec.removeClass("hiden");

    getSystems = () => this.$("input[name='group-systems']:checked").map(function () {
        return this.id;
    }).get();

    getEnvironments = () => this.$("input[name='group-environments']:checked").map(function () {
        return this.id;
    }).get();

    getContents = () => {
        let self = this;
        let contents = [];

        this.$("textarea").map(function (index, domElement) {
            contents.push({ LanguageKey: this.id, Text: self.$(this).val() });
        });

        return contents;
    };

    getSpecs = () => {
        let self = this;
        let specs = [];

        this.$(this.$visibleSpecDivs).map(function (index, domElement) {
            let jsonObject = self.$(domElement).data("jsonobject");

            if (self.$(domElement).find("#txt-specification-value").length > 0) {
                let object = self.$(domElement).find("#txt-specification-value");
                jsonObject.Value = self.$(object).val().match(/[^,(?! )]+/g);
            }

            if (self.$(domElement).find('#dll-specification-value').length > 0) {
                let object = self.$(domElement).find("#dll-specification-value");
                jsonObject.Value = self.$(object).val();

                let selectedIds = self.$(object).val();
                let selectedValues = [];

                self.$.each(selectedIds, function (i, val) {
                    let search = self.search(jsonObject, val);
                    if (search.length > 0) {
                        selectedValues.push(search);
                    }
                });

                jsonObject.Value = selectedValues;
            }

            jsonObject.ConjunctionOperator = this.getConjunctionOperatorValue(domElement);
            jsonObject.SpecificationOperator = this.getSpecificationOperatorValue(domElement);

            specs.push(jsonObject);
        });

        return specs;
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

const composedAnnouncement = new ComposedAnnouncement($, window.NAP, window.JSON);

$(() => {
    composedAnnouncement.init();
});
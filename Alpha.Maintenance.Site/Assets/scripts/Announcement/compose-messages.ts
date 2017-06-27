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
    $dllSpecificationOperator: JQuery;
    $dllSpecificationValue: JQuery;
    $txtSpecificationValue: JQuery;
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
        this.$dllSpecificationOperator = this.$("SELECT[name='dll-specification-specificationOperator'");
        this.$dllSpecificationValue = this.$("SELECT[name='dll-specification-value'");
        this.$txtSpecificationValue = this.$("INPUT[name='txt-specification-value'");
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

        this.$dllSpecificationValue.on('change', function () {
            self.onChangeSpecValueDll(this);
        });

        this.$txtSpecificationValue.on('change', function () {
            self.onChangeSpecValueTxt(this);
        });

        this.$("SELECT[name='" + this.$dllConjunctionOperator + "'").on('change', function () {
            self.onChangesConjunctionOperator(this);
        });

        this.$dllSpecificationOperator.on('change', function () {
            self.onChangesSpecificationOperator(this);
        });

        this.$btnAddSpec.on('click', function () {
            self.addSpec(this);
        });

        this.$hiddenSpec.on('click', function () {
            self.deleteSpec(this);
        });

        this.$(this.$btnSave).on('click', self.submit);

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
        var $spec = this.$("#" + this.$(element).find('option:selected').val());
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
        var $firstDiv = this.$(this.$visibleSpecDivs).first();
        $firstDiv.find("#" + this.$dllConjunctionOperator).find('option:selected').val(0);
        $firstDiv.find("#" + this.$dllConjunctionOperator).parent().addClass("hiden");
    };

    onChangeSpecValueTxt = (element: any) => {
        var specId = this.$(element).data("specificationId");
        var jsonobject = this.$("#" + specId).data("jsonobject");

        jsonobject.values = this.$(element).val().match(/[^,(?! )]+/g);
        this.$("#" + specId).data("jsonobject", jsonobject);
    };

    onChangeSpecValueDll = (element: any) => {
        var specId = this.$(element).data("specificationId");
        var jsonobject = this.$("#" + specId).data("jsonobject");

        var selectedIds = this.$(element).val();
        var selectedValues = [];

        this.$.each(selectedIds, function (i, val) {
            var search02 = this.$JSON.search(jsonobject, '//*[ Id=' + val + ']');
            if (search02.length > 0) {
                selectedValues.push(search02);
            }
        });

        jsonobject.Value = selectedValues;

        this.$("#" + specId).data("jsonobject", jsonobject);
    };

    onChangesConjunctionOperator = (element: any) => {
        var specId = this.$(element).data("specificationId");
        var jsonobject = this.$("#" + specId).data("jsonobject");

        jsonobject.ConjunctionOperator = this.$(element).find('option:selected').val();
        this.$("#" + specId).data("jsonobject", jsonobject);
    };

    onChangesSpecificationOperator = (element: any) => {
        var specId = this.$(element).data("specificationId");
        var jsonobject = this.$("#" + specId).data("jsonobject");

        jsonobject.SpecificationOperator = this.$(element).find('option:selected').val();
        this.$("#" + specId).data("jsonobject", jsonobject);
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
        var announcement = this.getAnnouncementParam();

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
        return {
            id: this.$('#AnnouncementId').val(),
            code: this.$("#txtcode").val(),
            title: this.$("#txttitle").val(),
            isActive: this.$("#ChkActive").is(":checked"),
            announcementType: this.$('input[name=group-popup]:checked').val(),
            contents: this.getContents(),
            environments: this.getEnvironments(),
            fromdate: this.$('#FromDate').val(),
            todate: this.$('#ToDate').val(),
            order: this.$('#group-order option:selected').val(),
            systems: this.getSystems(),
            specifications: this.getSpecs()
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
        var contents = [];

        this.$("textarea").map(function (index, domElement) {
            contents.push({ LanguageKey: this.id, Text: this.$(this).val() });
        });

        return contents;
    };

    getSpecs = () => {
        var specs = [];

        this.$(this.$visibleSpecDivs).map(function (index, domElement) {
            specs.push(this.$(this).data("jsonobject"));
        });

        return specs;
    };
}

const composedAnnouncement = new ComposedAnnouncement($, window.NAP, window.JSON);

$(() => {
    composedAnnouncement.init();
});
"use strict";
var ComposedAnnouncement = (function () {
    function ComposedAnnouncement($, NAP, JSON) {
        var _this = this;
        this.init = function () {
            _this.$('.jqte-test').jqte();
            _this.$('ul.tabs').tabs();
            _this.$('select').material_select();
            _this.$("#content-group").removeClass("hiden");
            _this.cacheDom();
            _this.bindEvent();
            _this.correctFirstSpec();
        };
        this.cacheDom = function () {
            _this.$dllChooseSpecification = "#dll-choose-specification";
            _this.$divChooseSpecification = _this.$("#div-choose-specification");
            _this.$dllConjunctionOperator = "dll-specification-conjunctionOperator";
            _this.$dllSpecificationOperator = _this.$("SELECT[name='dll-specification-specificationOperator'");
            _this.$dllSpecificationValue = _this.$("SELECT[name='dll-specification-value'");
            _this.$txtSpecificationValue = _this.$("INPUT[name='txt-specification-value'");
            _this.$btnAddSpec = _this.$("#add-spec");
            _this.$btnSave = "INPUT[name='btnSave']";
            _this.$hiddenSpec = _this.$('.hidden-specification');
            _this.$btnNext = _this.$("#btnNext");
            _this.$btnPrev = _this.$("#btnPrev");
            _this.$specSettingTab = _this.$("#tab-specs-setting");
            _this.$specInformationTab = _this.$("#tab-information-settings");
            _this.$visibleSpecDivs = "div[name='div-specification']:not(.hiden)";
            _this.$numberOfSpecs = _this.$('#number-of-specifications');
        };
        this.bindEvent = function () {
            var self = _this;
            _this.$(_this.$dllChooseSpecification).on('change', function () {
                self.onChangeSpecOption(this);
            });
            _this.$dllSpecificationValue.on('change', function () {
                self.onChangeSpecValueDll(this);
            });
            _this.$txtSpecificationValue.on('change', function () {
                self.onChangeSpecValueTxt(this);
            });
            _this.$("SELECT[name='" + _this.$dllConjunctionOperator + "'").on('change', function () {
                self.onChangesConjunctionOperator(this);
            });
            _this.$dllSpecificationOperator.on('change', function () {
                self.onChangesSpecificationOperator(this);
            });
            _this.$btnAddSpec.on('click', function () {
                self.addSpec(this);
            });
            _this.$hiddenSpec.on('click', function () {
                self.deleteSpec(this);
            });
            _this.$(_this.$btnSave).on('click', self.submit);
            _this.$btnNext.on('click', function () {
                self.$specSettingTab.click();
            });
            _this.$btnPrev.on('click', function () {
                self.$specInformationTab.click();
            });
        };
        this.onChangeSpecOption = function (element) {
            _this.changeSpecPositions(element);
            _this.reRenderSpecOptions(element);
            _this.correctFirstSpec();
            _this.toogleAddSpecButton();
        };
        this.changeSpecPositions = function (element) {
            var $spec = _this.$("#" + _this.$(element).find('option:selected').val());
            $spec.find("#" + _this.$dllConjunctionOperator).parent().removeClass("hiden");
            $spec.removeClass("hiden");
            $spec.insertBefore(_this.$divChooseSpecification);
            _this.$divChooseSpecification.addClass("hiden");
        };
        this.reRenderSpecOptions = function (element) {
            _this.$(element).find('option:selected').remove();
            _this.$(element).material_select('destroy');
            _this.$(element).material_select();
        };
        this.correctFirstSpec = function () {
            var $firstDiv = _this.$(_this.$visibleSpecDivs).first();
            $firstDiv.find("#" + _this.$dllConjunctionOperator).find('option:selected').val(0);
            $firstDiv.find("#" + _this.$dllConjunctionOperator).parent().addClass("hiden");
        };
        this.onChangeSpecValueTxt = function (element) {
            var specId = _this.$(element).data("specificationId");
            var jsonobject = _this.$("#" + specId).data("jsonobject");
            jsonobject.values = _this.$(element).val().match(/[^,(?! )]+/g);
            _this.$("#" + specId).data("jsonobject", jsonobject);
        };
        this.onChangeSpecValueDll = function (element) {
            var specId = _this.$(element).data("specificationId");
            var jsonobject = _this.$("#" + specId).data("jsonobject");
            var selectedIds = _this.$(element).val();
            var selectedValues = [];
            _this.$.each(selectedIds, function (i, val) {
                var search02 = this.$JSON.search(jsonobject, '//*[ Id=' + val + ']');
                if (search02.length > 0) {
                    selectedValues.push(search02);
                }
            });
            jsonobject.Value = selectedValues;
            _this.$("#" + specId).data("jsonobject", jsonobject);
        };
        this.onChangesConjunctionOperator = function (element) {
            var specId = _this.$(element).data("specificationId");
            var jsonobject = _this.$("#" + specId).data("jsonobject");
            jsonobject.ConjunctionOperator = _this.$(element).find('option:selected').val();
            _this.$("#" + specId).data("jsonobject", jsonobject);
        };
        this.onChangesSpecificationOperator = function (element) {
            var specId = _this.$(element).data("specificationId");
            var jsonobject = _this.$("#" + specId).data("jsonobject");
            jsonobject.SpecificationOperator = _this.$(element).find('option:selected').val();
            _this.$("#" + specId).data("jsonobject", jsonobject);
        };
        this.addSpec = function (element) {
            _this.$divChooseSpecification.removeClass("hiden");
            _this.$(element).addClass("hiden");
        };
        this.deleteSpec = function (element) {
            _this.$divChooseSpecification.addClass("hiden");
            _this.$(_this.$dllChooseSpecification).material_select('destroy');
            if (_this.$(element).data("id") != null &&
                _this.$(_this.$dllChooseSpecification + " option[value=" + _this.$(element).data("id") + "]").length == 0) {
                _this.$(_this.$dllChooseSpecification).append(_this.$('<option>', {
                    value: _this.$(element).data("id"),
                    text: _this.$(element).data("name")
                }));
            }
            _this.$(_this.$dllChooseSpecification).material_select();
            _this.$(element).parent('div').addClass("hiden");
            _this.correctFirstSpec();
            _this.toogleAddSpecButton();
        };
        this.submit = function () {
            var announcement = _this.getAnnouncementParam();
            _this.$.ajax({
                url: _this.$nap.resolveClientUrl("Announcement/InsertAnnouncement"),
                type: 'POST',
                data: { "announcement": _this.$JSON.stringify(announcement), action: _this.$('#actionType').val() },
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
        this.getAnnouncementParam = function () {
            return {
                id: _this.$('#AnnouncementId').val(),
                code: _this.$("#txtcode").val(),
                title: _this.$("#txttitle").val(),
                isActive: _this.$("#ChkActive").is(":checked"),
                announcementType: _this.$('input[name=group-popup]:checked').val(),
                contents: _this.getContents(),
                environments: _this.getEnvironments(),
                fromdate: _this.$('#FromDate').val(),
                todate: _this.$('#ToDate').val(),
                order: _this.$('#group-order option:selected').val(),
                systems: _this.getSystems(),
                specifications: _this.getSpecs()
            };
        };
        this.toogleAddSpecButton = function () { return _this.$(_this.$visibleSpecDivs).length == _this.$(_this.$numberOfSpecs).val() ? _this.$btnAddSpec.addClass("hiden") : _this.$btnAddSpec.removeClass("hiden"); };
        this.getSystems = function () { return _this.$("input[name='group-systems']:checked").map(function () {
            return this.id;
        }).get(); };
        this.getEnvironments = function () { return _this.$("input[name='group-environments']:checked").map(function () {
            return this.id;
        }).get(); };
        this.getContents = function () {
            var contents = [];
            _this.$("textarea").map(function (index, domElement) {
                contents.push({ LanguageKey: this.id, Text: this.$(this).val() });
            });
            return contents;
        };
        this.getSpecs = function () {
            var specs = [];
            _this.$(_this.$visibleSpecDivs).map(function (index, domElement) {
                specs.push(this.$(this).data("jsonobject"));
            });
            return specs;
        };
        this.$ = $;
        this.$nap = NAP;
        this.$JSON = JSON;
    }
    return ComposedAnnouncement;
}());
var composedAnnouncement = new ComposedAnnouncement($, window.NAP, window.JSON);
$(function () {
    composedAnnouncement.init();
});

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
            _this.toogleSaveAndNextButton(_this.$txtcode);
        };
        this.cacheDom = function () {
            _this.$dllChooseSpecification = "#dll-choose-specification";
            _this.$divChooseSpecification = _this.$("#div-choose-specification");
            _this.$dllConjunctionOperator = "dll-specification-conjunctionOperator";
            _this.$btnAddSpec = _this.$("#add-spec");
            _this.$btnSave = "INPUT[name='btnSave']";
            _this.$hiddenSpec = _this.$('.hidden-specification');
            _this.$btnNext = _this.$("#btnNext");
            _this.$btnPrev = _this.$("#btnPrev");
            _this.$specSettingTab = _this.$("#tab-specs-setting");
            _this.$specInformationTab = _this.$("#tab-information-settings");
            _this.$visibleSpecDivs = "div[name='div-specification']:not(.hiden)";
            _this.$numberOfSpecs = _this.$('#number-of-specifications');
            _this.$txtcode = _this.$('#txtcode');
            _this.userIdsSpecificationValue = '#txt-userids-pecification-value';
            _this.userLevelsSpecificationValue = '#dll-userlevel-specification-value';
            _this.siteSpecificationValue = '#txt-site-specification-value';
        };
        this.bindEvent = function () {
            var self = _this;
            _this.$(_this.$dllChooseSpecification).on('change', function () {
                self.onChangeSpecOption(this);
            });
            _this.$btnAddSpec.on('click', function () {
                self.addSpec(this);
            });
            _this.$hiddenSpec.on('click', function () {
                self.deleteSpec(this);
            });
            _this.$(_this.$btnSave).on('click', function () {
                self.submit();
            });
            _this.$btnNext.on('click', function () {
                self.$specSettingTab.click();
            });
            _this.$btnPrev.on('click', function () {
                self.$specInformationTab.click();
            });
            _this.$txtcode.on('keyup', function () {
                self.toogleSaveAndNextButton(this);
            });
        };
        this.toogleSaveAndNextButton = function (element) {
            var currentCode = _this.$(element).val().trim();
            var originalCode = _this.$("#originalCode").val().trim();
            if (currentCode == '' || currentCode == originalCode) {
                _this.disableSaveAndNextButton();
            }
            else {
                _this.$('.card-panel').addClass("hiden");
                _this.enableSaveAndNextButton();
            }
        };
        this.enableSaveAndNextButton = function () {
            _this.$btnNext.prop("disabled", false).parent().removeClass("disabled");
            _this.$(_this.$btnSave).prop("disabled", false).parent().removeClass("disabled");
        };
        this.disableSaveAndNextButton = function () {
            _this.$btnNext.prop("disabled", true).parent().addClass("disabled");
            _this.$(_this.$btnSave).prop("true", true).parent().addClass("disabled");
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
        this.search = function (jsonobject, valueList) {
            return _this.$.map(jsonobject.ValueRange, function (obj) {
                for (var i = 0; i < valueList.length; i++) {
                    if (obj.Id == valueList[i]) {
                        return obj;
                    }
                }
            });
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
            if (announcement.code != '') {
                _this.doUpdate(announcement);
            }
            else {
                _this.$('.card-panel').html("Please input annoucement code !").addClass("red red-text").removeClass('hiden');
            }
        };
        this.doUpdate = function (announcement) {
            var self = _this;
            _this.$.ajax({
                url: self.$nap.resolveClientUrl("Announcement/InsertAnnouncement"),
                type: 'POST',
                data: { "announcement": self.$JSON.stringify(announcement), action: self.$('#actionType').val() },
                dataType: 'json',
                beforeSend: function () {
                    self.$nap.loadingBox.show();
                },
                complete: function () {
                    self.$nap.loadingBox.close();
                },
                success: function (result) {
                    var actionType = self.$('#actionType').val();
                    var message = actionType == 'Create' ? 'Created successfully !' : 'Updated successfully !';
                    self.$('.card-panel').html(message).addClass("green green-text").removeClass('red red-text hiden');
                    setTimeout(function () {
                        self.refreshPage(actionType, announcement.id);
                    }, 3000);
                },
                error: function (xhr, status, err) {
                    self.$nap.handleAjaxError(xhr, status, err);
                }
            });
        };
        this.refreshPage = function (actionType, announcementId) {
            var url = "Announcement/ComposeMessage/" + (actionType == 'Create' ? '' : announcementId);
            window.location.href = _this.$nap.resolveClientUrl(url);
        };
        this.getAnnouncementParam = function () {
            var self = _this;
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
        this.toogleAddSpecButton = function () { return _this.$(_this.$visibleSpecDivs).length == _this.$(_this.$numberOfSpecs).val() ? _this.$btnAddSpec.addClass("hiden") : _this.$btnAddSpec.removeClass("hiden"); };
        this.getSystems = function () {
            return _this.$("input[name='group-systems']:checked").map(function () {
                return this.id;
            }).get();
        };
        this.getEnvironments = function () {
            return _this.$("input[name='group-environments']:checked").map(function () {
                return this.id;
            }).get();
        };
        this.getContents = function () {
            var self = _this;
            var contents = [];
            _this.$("textarea").map(function (index, domElement) {
                contents.push({ LanguageKey: this.id, Text: self.$(this).val() });
            });
            return contents;
        };
        this.getSpecs = function () {
            var self = _this;
            var specs = [];
            _this.$(_this.$visibleSpecDivs).map(function (index, domElement) {
                var jsonObject = self.$(domElement).data("jsonobject");
                if (self.$(domElement).find(self.siteSpecificationValue).length > 0) {
                    var sites = self.$(domElement).find(self.siteSpecificationValue).val();
                    jsonObject.Sites = self.splitStringtoArray(sites);
                }
                if (self.$(domElement).find(self.userIdsSpecificationValue).length > 0) {
                    var userIds = self.$(domElement).find(self.userIdsSpecificationValue).val();
                    jsonObject.UserIds = self.splitStringtoArray(userIds);
                }
                if (self.$(domElement).find(self.userLevelsSpecificationValue).length > 0) {
                    jsonObject.Levels = self.$(domElement).find(self.userLevelsSpecificationValue).val();
                }
                jsonObject.ConjunctionOperator = self.getConjunctionOperatorValue(domElement);
                jsonObject.SpecificationOperator = self.getSpecificationOperatorValue(domElement);
                specs.push(jsonObject);
            });
            return specs;
        };
        this.splitStringtoArray = function (value) {
            return value.match(/(?:\\,|[^,])+/g);
        };
        this.getConjunctionOperatorValue = function (domElement) {
            var object = _this.$(domElement).find("#dll-specification-conjunctionOperator");
            return _this.$(object).find('option:selected').val();
        };
        this.getSpecificationOperatorValue = function (domElement) {
            var object = _this.$(domElement).find("#dll-specification-specificationOperator");
            return _this.$(object).find('option:selected').val();
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

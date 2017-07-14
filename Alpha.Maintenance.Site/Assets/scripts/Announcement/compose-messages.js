"use strict";
var ComposedAnnouncement = (function () {
    function ComposedAnnouncement($, NAP, JSON, utf8, base64) {
        var _this = this;
        this.init = function () {
            _this.$('ul.tabs').tabs();
            _this.$('select').material_select();
            _this.$("#content-group").removeClass("hiden");
            _this.cacheDom();
            _this.bindEvent();
            _this.correctFirstSpec();
            _this.initSpecOptionsDll();
            _this.$('.jqte-test').jqte();
        };
        this.cacheDom = function () {
            _this.$divChooseSpecification = _this.$("#div-choose-specification");
            _this.$hiddenSpec = _this.$('.hidden-specification');
            _this.$btnNext = _this.$("#btnNext");
            _this.$btnPrev = _this.$("#btnPrev");
            _this.$specSettingTab = _this.$("#tab-specs-setting");
            _this.$specInformationTab = _this.$("#tab-information-settings");
            _this.$numberOfSpecs = _this.$('#number-of-specifications');
            _this.$txttitle = _this.$('#txtcode');
            _this.dllChooseSpecification = "#dll-choose-specification";
            _this.visibleSpecDivs = "div[name='div-specification']:not(.hiden)";
            _this.dllConjunctionOperator = "dll-specification-conjunctionOperator";
            _this.btnSave = "INPUT[name='btnSave']";
            _this.userLevelsSpecificationValue = '#dll-specification-value';
            _this.txtSpecificationValue = '#txt-specification-value';
        };
        this.bindEvent = function () {
            var self = _this;
            _this.bindingContents();
            _this.$(_this.dllChooseSpecification).on('change', function () {
                self.onChangeSpecOption();
            });
            _this.$hiddenSpec.on('click', function () {
                self.deleteSpec(this);
            });
            _this.$(_this.btnSave).on('click', function () {
                self.submit();
            });
            _this.$btnNext.on('click', function () {
                self.$specSettingTab.click();
            });
            _this.$btnPrev.on('click', function () {
                self.$specInformationTab.click();
            });
            _this.$txttitle.on('keyup', function () {
                if (self.$(this).val().trim() != '') {
                    self.$('.card-panel').addClass('hiden');
                }
            });
        };
        this.bindingContents = function () {
            var self = _this;
            _this.$(".language-content").map(function (index, domElement) {
                var $textarea = self.$(this).find('textarea');
                var contentText = $textarea.data("content");
                var decodecontext = contentText;
                $textarea.html(decodecontext);
            });
        };
        this.onChangeSpecOption = function () {
            _this.changeSpecPositions();
            _this.reRenderSpecOptions();
            _this.correctFirstSpec();
            _this.toogleAddSpecButton();
        };
        this.changeSpecPositions = function () {
            var $spec = _this.$("#" + _this.$(_this.dllChooseSpecification).find('option:selected').val());
            $spec.find("#" + _this.dllConjunctionOperator).parent().removeClass("hiden");
            $spec.removeClass("hiden");
            $spec.insertBefore(_this.$divChooseSpecification);
        };
        this.reRenderSpecOptions = function () {
            _this.$(_this.dllChooseSpecification).find('option:selected').remove();
            _this.initSpecOptionsDll();
        };
        this.correctFirstSpec = function () {
            var $firstDiv = _this.$(_this.visibleSpecDivs).first();
            $firstDiv.find("#" + _this.dllConjunctionOperator).find('option:selected').val(0);
            $firstDiv.find("#" + _this.dllConjunctionOperator).parent().addClass("hiden");
        };
        this.initSpecOptionsDll = function () {
            _this.$(_this.dllChooseSpecification).material_select('destroy');
            _this.$(_this.dllChooseSpecification).val("").material_select();
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
        this.deleteSpec = function (element) {
            _this.$divChooseSpecification.addClass("hiden");
            if (_this.$(element).data("id") != null &&
                _this.$(_this.dllChooseSpecification + " option[value=" + _this.$(element).data("id") + "]").length == 0) {
                _this.$(_this.dllChooseSpecification).append(_this.$('<option>', {
                    value: _this.$(element).data("id"),
                    text: _this.$(element).data("name")
                }));
            }
            _this.initSpecOptionsDll();
            _this.$(element).parent('div').addClass("hiden");
            _this.correctFirstSpec();
            _this.toogleAddSpecButton();
        };
        this.submit = function () {
            var announcement = _this.getAnnouncementParam();
            if (announcement.title != '') {
                _this.doUpdate(announcement);
            }
            else {
                _this.showWarningMessage("Please input announcement title !");
            }
        };
        this.showWarningMessage = function (message) { return _this.$('.card-panel').html(message).addClass("red red-text").removeClass('hiden'); };
        this.doUpdate = function (announcement) {
            var self = _this;
            var formdata = new FormData();
            formdata.append("announcement", self.$JSON.stringify(announcement));
            formdata.append("action", self.$('#actionType').val());
            _this.$.ajax({
                url: self.$nap.resolveClientUrl("Announcement/InsertOrUpdateAnnouncement"),
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
            var url = actionType == 'Create' ? "Announcement/ManageAnnouncements" : "Announcement/ComposeMessage/" + announcementId;
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
        this.toogleAddSpecButton = function () { return _this.$(_this.visibleSpecDivs).length == _this.$(_this.$numberOfSpecs).val() ? _this.$divChooseSpecification.addClass("hiden") : _this.$divChooseSpecification.removeClass("hiden"); };
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
            _this.$(".language-content").map(function (index, domElement) {
                var contentText = self.$(this).find('textarea');
                var contentUrl = self.$(this).find('INPUT');
                contents.push({
                    LanguageKey: self.$(contentText).attr('id'),
                    Text: self.$(contentText).val(),
                    Url: self.$(this).find('#txtannouncement-url').val()
                });
            });
            return contents;
        };
        this.getSpecs = function () {
            var self = _this;
            var specs = [];
            _this.$(_this.visibleSpecDivs).map(function (index, domElement) {
                var jsonObject = self.$(domElement).data("jsonobject");
                jsonObject.ConjunctionOperator = self.getConjunctionOperatorValue(domElement);
                jsonObject.SpecificationOperator = self.getSpecificationOperatorValue(domElement);
                if (self.$(domElement).find(self.txtSpecificationValue).length > 0) {
                    var sites = self.$(domElement).find(self.txtSpecificationValue).val();
                    jsonObject.Value = self.splitStringtoArray(sites);
                }
                if (self.$(domElement).find(self.userLevelsSpecificationValue).length > 0) {
                    jsonObject.Value = self.$(domElement).find(self.userLevelsSpecificationValue).val();
                }
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
        this.$utf8 = utf8;
        this.$base64 = base64;
    }
    return ComposedAnnouncement;
}());
var composedAnnouncement = new ComposedAnnouncement($, window.NAP, window.JSON, window.utf8, window.base64);
$(function () {
    composedAnnouncement.init();
});
//# sourceMappingURL=compose-messages.js.map
declare var MaterialDatetimePicker, jqTextEditor;

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

interface Document {
    selection;
}

class ComposedAnnouncement {
    $: JQueryStatic;

    $divChooseSpecification: JQuery;
    $hiddenSpec: JQuery;
    $btnNext: JQuery;
    $dpkReleasedDate: JQuery;
    $dpkExpiredDate: JQuery;
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

    jqTextEditor: any;
    carretPos: number;

    viewResources: any = {
        uploadImages: "Upload Images",
        uploadImages_Error: "Image upload failed. Please check!",
        uploadImages_ErrorFileSize: "The total file size is over 10 MB. Please check!",
        uploadImages_ErrorImageFormat: "File is not supported. Please check!",
    };

    constructor($: JQueryStatic, NAP: any, JSON: any, jqTextEditor: any) {
        this.$ = $;
        this.$nap = NAP;
        this.$JSON = JSON;
        this.jqTextEditor = jqTextEditor;
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

        this.initTextEditor();
        this.initUploadPictures();
    };

    cacheDom = () => {
        this.$dpkReleasedDate = this.$("#ReleasedDate");
        this.$dpkExpiredDate = this.$("#ExpiredDate");
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
        var releasedDate = new Date(self.$dpkReleasedDate.val());
        var pickerReleasedDate = new MaterialDatetimePicker({
            default: releasedDate
        }).on('submit', function (d) {
            self.$dpkReleasedDate.val(d.format("MMM DD,YYYY HH:mm"));
        }).on('close', function () { self.$dpkReleasedDate.focus() });

        self.$dpkReleasedDate.on('click', function () {
            pickerReleasedDate.open();
        });

        var expiredDate = new Date(self.$dpkExpiredDate.val());
        var pickerExpiredDate = new MaterialDatetimePicker({
            default: expiredDate
        }).on('submit', function (d) {
            self.$dpkExpiredDate.val(d.format("MMM DD,YYYY HH:mm"));
        }).on('close', function () { self.$dpkExpiredDate.focus() });

        self.$dpkExpiredDate.on('click', function () {
            pickerExpiredDate.open();
        });

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

        this.$("#IsPopup").on('change', function () {
            if (this.checked) {
                self.$("#expiredDateSection").removeClass("hide");
            } else {
                self.$("#expiredDateSection").addClass("hide");
            }
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
        formdata.append("prevImages", self.$JSON.stringify(self.getPrevImages()));

        var images = this.getUploadImages();
        Object.keys(images).forEach(function (key) {
            formdata.append(key, images[key]);
        });

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
            announcementType: self.$("#IsPopup").is(":checked") ? self.$("#EmergencyPopupId").val() : self.$("#NormalAnnouncementId").val(),
            isPopup: self.$("#IsPopup").is(":checked"),
            isActive: self.$("#ChkActive").is(":checked"),
            contents: self.getContents(),
            environments: self.getEnvironments(),
            releasedDate: self.$('#ReleasedDate').val(),
            expiredDate: self.$('#ExpiredDate').val(),
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

            if (self.$(domElement).find(".toogle").length > 0) {
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

    initTextEditor = () => {
        this.$('.jqte-test').jqte();

        var editors = this.$('.jqte');

        var btn = '<div class="jqte_tool jqte_tool_22 unselectable" role="button" data-tool="21" unselectable="on" style="user-select: none;" title="Icons">\
                        <a class="jqte_tool_icon unselectable" unselectable="on" style="user-select: none;"></a>\
                    </div>';
        editors.find('.jqte_toolbar').append(btn);

        var self = this;
        editors.find('.jqte_tool_22').click(function (e) {
            self.showIcons(self.$(this));

            e.stopPropagation();
            return false;
        });

        this.$('body:not(.jqte_tool_22):not(.div-icons)').click(function () {
            self.$('.div-icons').addClass('hide');
        });


        this.$('.div-icon').mousedown(function (e) {
            self.getCaretPosition(self.$(this));
        });

        this.$('.div-icon').click(function (e) {
            self.pasteIcon(self.$(this));
            e.stopPropagation();
            return false;
        });
    };

    showIcons = (elm) => {
        var prt = elm.closest('.language-content');
        var divIcons = prt.find('.div-icons');
        var isHide = divIcons.hasClass('hide');
        if (isHide) {
            divIcons.removeClass('hide');
        } else {
            divIcons.addClass('hide');
        }
    };

    getCaretPosition = (elm) => {
        var prt = elm.closest('.language-content');
        const editor = prt.find('.jqte_editor');
        var pos = this.jqTextEditor.getCaretPosition(editor);
        this.carretPos = pos;
    };

    pasteIcon = (elm) => {
        var prt = elm.closest('.language-content');
        const editor = prt.find('.jqte_editor');
        var val = elm.html();
        var editorVal = editor.html();
        editor.html(editorVal.substring(0, this.carretPos) + val + editorVal.substring(this.carretPos));

        this.carretPos += val.length;

        this.jqTextEditor.restoreCarretPos(editor, this.carretPos);
    };

    initUploadPictures = () => {
        this.$(".upload-pictures").dropify({
            messages: {
                default: this.viewResources.uploadImages,
                error: this.viewResources.uploadImages_Error
            },
            error: {
                fileSize: this.viewResources.uploadImages_ErrorFileSize,
                imageFormat: this.viewResources.uploadImages_ErrorImageFormat,
                fileExtension: this.viewResources.uploadImages_ErrorImageFormat,
            }
        });

        var self = this;
        this.$('.div-image-remove').click(function () {
            var p = self.$(this).parent();
            p.addClass('hide');
        });
    };

    getPrevImages = () => {
        let images = [];
        const inputFiles = this.$('.div-image:not(.hide)');

        inputFiles.each((i, e) => {
            var $e = this.$(e);
            var name = $e.find('.div-image-name').html();
            images.push({ name });
        });

        return images;
    };
    getUploadImages = () => {
        let images = {};
        const inputFiles = document.getElementsByClassName('upload-pictures');

        for (let i = 0, max = inputFiles.length; i < max; i++) {
            let inputFile = <HTMLElement>inputFiles[i];
            var name = this.$(inputFile).attr("name");
            for (var j = 0; j < inputFile.files.length; j++) {
                images[name + "_" + j] = inputFile.files[j];
            }
        }

        return images;
    };
}

const composedAnnouncement = new ComposedAnnouncement($, window.NAP, window.JSON, jqTextEditor);

$(() => {
    composedAnnouncement.init();
});

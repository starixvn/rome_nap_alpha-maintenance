"use strict";

var ComposedAnnouncement = (function ($) {
    return {
        cacheDom: {
            dllChooseSpecification: "#dll-choose-specification",
            divChooseSpecification: "#div-choose-specification",
            dllConjunctionOperator: "dll-specification-conjunctionOperator",
            dllSpecificationOperator: "dll-specification-specificationOperator",
            dllSpecificationValue: "dll-specification-value",
            btnAddSpec: "#add-spec"
        },

        init: function () {
            $('.jqte-test').jqte();
            $('ul.tabs').tabs();
            $('select').material_select();
            $("#content-group").removeClass("hiden");
            this.bindEvent();
            //this.toogleAddSpecButton();
        },

        bindEvent: function ()
        {
            $(ComposedAnnouncement.cacheDom.dllChooseSpecification).on('change', function () {
                ComposedAnnouncement.onChangeSpecOption(this);
            });

            $("SELECT[name='" + ComposedAnnouncement.cacheDom.dllSpecificationValue + "'").on('change', function () {
                alert($(this).find('option:selected').text());
            });

            $("SELECT[name='" + ComposedAnnouncement.cacheDom.dllConjunctionOperator + "'").on('change', function () {
                ComposedAnnouncement.onChangesConjunctionOperator(this);
            });

            $("SELECT[name='" + ComposedAnnouncement.cacheDom.dllSpecificationOperator + "'").on('change', function () {
                ComposedAnnouncement.onChangesSpecificationOperator(this);
            });

            $(ComposedAnnouncement.cacheDom.btnAddSpec).on('click', function () {
                ComposedAnnouncement.addSpec(this);
            });

            $('.hidden-specification').on('click', function () {
                ComposedAnnouncement.deleteSpec(this);
            });

            $("INPUT[name='btnSave']").on('click', function () {
                ComposedAnnouncement.submit();
            });

            $("#btnNext").on('click', function () {
                $("#tab-specs-setting").click();
            });

            $("#btnPrev").on('click', function () {
                $("#tab-information-settings").click();
            });
        },

        onChangeSpecOption: function ($this)
        {
            ComposedAnnouncement.changeSpecPositions($this);
            ComposedAnnouncement.reRenderSpecOptions($this);
            ComposedAnnouncement.correctFirstSpec($this);
            ComposedAnnouncement.toogleAddSpecButton();
        },

        changeSpecPositions: function (Id)
        {
            var $spec = $("#" + $(Id).find('option:selected').val());
            $spec.find("#" + ComposedAnnouncement.cacheDom.dllConjunctionOperator).parent().removeClass("hiden");

            $spec.removeClass("hiden");
            $spec.insertBefore($(ComposedAnnouncement.cacheDom.divChooseSpecification));
            $(ComposedAnnouncement.cacheDom.divChooseSpecification).addClass("hiden");
        },

        reRenderSpecOptions: function ($this)
        {
            $($this).find('option:selected').remove();
            $($this).material_select('destroy');
            $($this).material_select();
        },

        correctFirstSpec: function()
        {
            var $firstDiv = $("div[name='div-specification']:visible").first();
            $firstDiv.find("#" + ComposedAnnouncement.cacheDom.dllConjunctionOperator).find('option:selected').val(0);
            $firstDiv.find("#" + ComposedAnnouncement.cacheDom.dllConjunctionOperator).parent().addClass("hiden");
        },

        onChangesConjunctionOperator: function ($this) {
            var specId = "#" + $($this).data("parentid");
            var jsonobject = $(specId).data("jsonobject");

            jsonobject.ConjunctionOperator = $($this).find('option:selected').val();
            $(specId).data("jsonobject", jsonobject);
        },

        onChangesSpecificationOperator: function ($this)
        {
            var specId = "#" + $($this).data("parentid");
            var jsonobject = $(specId).data("jsonobject");

            jsonobject.SpecificationOperator = $($this).find('option:selected').val();
            $(specId).data("jsonobject", jsonobject);
        },

        addSpec: function ($this)
        {
            $(ComposedAnnouncement.cacheDom.divChooseSpecification).removeClass("hiden");
            $($this).addClass("hiden");
        },

        deleteSpec: function ($this)
        {
            $(ComposedAnnouncement.cacheDom.divChooseSpecification).addClass("hiden");
            $(ComposedAnnouncement.cacheDom.dllChooseSpecification).material_select('destroy');

            if ($($this).data("id") != null &&
                $(ComposedAnnouncement.cacheDom.dllChooseSpecification + " option[value=" + $($this).data("id") + "]").length == 0) {
                $(ComposedAnnouncement.cacheDom.dllChooseSpecification).append($('<option>', {
                    value: $($this).data("id"),
                    text: $($this).data("name")
                }));
            }


            //if ($($this).data("id") != null) {
            //    $(ComposedAnnouncement.cacheDom.dllChooseSpecification).append($('<option>', {
            //        value: $($this).data("id"),
            //        text: $($this).data("name")
            //    }));
            //}

            $(ComposedAnnouncement.cacheDom.dllChooseSpecification).material_select();
            $($this).parent('div').addClass("hiden");

            ComposedAnnouncement.correctFirstSpec($this);
            ComposedAnnouncement.toogleAddSpecButton();
        },

        toogleAddSpecButton: function()
        {
            $("div[name='div-specification']:visible").length == $('#number-of-specifications').val() ? $(ComposedAnnouncement.cacheDom.btnAddSpec).addClass("hiden") : $(ComposedAnnouncement.cacheDom.btnAddSpec).removeClass("hiden");
        },

        submit: function () {
           var announcement =  ComposedAnnouncement.getAnnouncementParam();
            $.ajax({
                url: NAP.resolveClientUrl("Announcement/InsertAnnouncement"),
                type: 'POST',
                data: { "announcement": JSON.stringify(announcement), action: $('#actionType').val() },
                dataType: 'json',
                beforeSend: function () {
                    NAP.loadingBox.show();
                },
                complete: function () {
                    NAP.loadingBox.close();
                },
                success: function (result) {

                },
                error: function (xhr, status, err) {
                    NAP.handleAjaxError(xhr, status, err);
                }
            });
        },

        getAnnouncementParam: function()
        {
            return {
                id: $('#AnnouncementId').val(),
                code: $("#txtcode").val(),
                title: $("#txttitle").val(),
                isActive: $("#ChkActive").is(":checked"),
                announcementType: $('input[name=group-popup]:checked').val(),
                contents: ComposedAnnouncement.getContents(),
                environments: ComposedAnnouncement.getEnvironments(),
                fromdate: $('#FromDate').val(),
                todate: $('#ToDate').val(),
                order: $('#group-order option:selected').val(),
                systems: ComposedAnnouncement.getSystems(),
                specifications: ComposedAnnouncement.getSpecs()
            };
        },

        getSystems: function()
        {
            return $("input[name='group-systems']:checked").map(function () {
                return this.id;
            }).get();
        },

        getEnvironments: function () {
            return $("input[name='group-environments']:checked").map(function () {
                return this.id;
            }).get();
        },

        getContents: function()
        {
            var contents = [];

            $("textarea").map(function (index, domElement) {
                contents.push({ LanguageKey: this.id, Text: $(this).val() });
            });

            return contents;
        },

        getSpecs: function ()
        {
            var specs = [];

            $("div[name='div-specification']:not(.hiden)").map(function (index, domElement) {
                specs.push($(this).data("jsonobject"));
            });

            return specs;
        }
    
    }
}(jQuery));


$(document).ready(function () {
    ComposedAnnouncement.init();
});


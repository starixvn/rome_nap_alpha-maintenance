"use strict";

var ComposedAnnouncement = (function ($) {
    return {
        cacheDom: {
            dllChooseSpecification: "#dll-choose-specification",
            divChooseSpecification: "#div-choose-specification",
            dllConjunctionOperator: "#dll-specification-conjunctionOperator",
            dllSpecificationOperator: "#dll-specification-specificationOperator",
            dllSpecificationValue: "#dll-specification-value",
            btnAddSpec: "#add-spec"
            },
        init: function () {
            $('.jqte-test').jqte();
            $('ul.tabs').tabs();
            $('select').material_select();
            $("#content-group").removeClass("hiden");
            this.bindEvent();
        },

        bindEvent: function ()
        {
            $(ComposedAnnouncement.cacheDom.dllChooseSpecification).on('change', function () {
                ComposedAnnouncement.onChangeOptionSpec(this);
            });

            $("SELECT[name='" + ComposedAnnouncement.cacheDom.dllSpecificationValue + "']").on('change', function () {
                alert($(this).find('option:selected').text());
            });

            $("SELECT[name='" + ComposedAnnouncement.cacheDom.dllConjunctionOperator + "']").on('change', function () {
                ComposedAnnouncement.onChangesConjunctionOperator(this);
            });

            $("SELECT[name='" + ComposedAnnouncement.cacheDom.dllSpecificationOperator + "']").on('change', function () {
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
        },

        onChangeOptionSpec: function($this)
        {
            var specId = "specification_" + $($this).find('option:selected').val();
                
            var $dlladdspec = $(ComposedAnnouncement.cacheDom.divChooseSpecification).closest('div');
            $dlladdspec.insertBefore($(ComposedAnnouncement.cacheDom.btnAddSpec));

            var $spec = $("#" + specId);
            $spec.find(ComposedAnnouncement.cacheDom.dllConjunctionOperator).find('option:selected').val(0).parent().removeClass("hiden");
            $spec.removeClass("hiden");
            $spec.insertBefore($(ComposedAnnouncement.cacheDom.divChooseSpecification));

            $(ComposedAnnouncement.cacheDom.divChooseSpecification).addClass("hiden");
            $($this).find('option:selected').remove();
            $($this).material_select('destroy');
            $($this).material_select();

            $("div[name='div-specification']:visible").first().find(ComposedAnnouncement.cacheDom.dllConjunctionOperator).parent().addClass("hiden");
            ComposedAnnouncement.toogleAddSpecButton();
        },

        onChangesConjunctionOperator: function ($this) {
            var value = $($this).find('option:selected').val();
            var parentId = $($this).data("parentid");
            var specId = "specification_" + parentId;

            var jsonobject = $("#" + specId).data("jsonobject");
            jsonobject.ConjunctionOperator = value;
            $("#" + specId).data("jsonobject", jsonobject);
        },

        onChangesSpecificationOperator: function ($this)
        {
            var value = $($this).find('option:selected').val();
            var parentId = $($this).data("parentid");
            var specId = "specification_" + parentId;

            var jsonobject = $("#" + specId).data("jsonobject");
            jsonobject.SpecificationOperator = value;
            $("#" + specId).data("jsonobject", jsonobject);
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

            if ($($this).data("id") != null) {
                $(ComposedAnnouncement.cacheDom.dllChooseSpecification).append($('<option>', {
                    value: $($this).data("id"),
                    text: $($this).data("name")
                }));
            }

            $(ComposedAnnouncement.cacheDom.dllChooseSpecification).material_select();

            $($this).parent('div').addClass("hiden");
            ComposedAnnouncement.toogleAddSpecButton();
        },

        toogleAddSpecButton: function()
        {
            $("div[id^='specification']:visible").length == $('#number-of-specifications').val() ? $(ComposedAnnouncement.cacheDom.btnAddSpec).addClass("hiden") : $(ComposedAnnouncement.cacheDom.btnAddSpec).removeClass("hiden");
        },

        submit: function () {
            var systems = $("input[name='group-systems']:checked").map(function () {
                return this.id;
            }).get();

            var environments = $("input[name='group-environments']:checked").map(function (index, domElement) {
                return this.id;
            }).get();

            var contents = [];

            $("textarea").map(function (index, domElement) {
                contents.push({ LanguageKey: this.id, Text: $(this).val() });
            });

            var specs = [];

            $("div[name='div-specification']:visible").map(function (index, domElement) {
                specs.push($(this).data("jsonobject"));
            });

            var announcement = {
                id: $('#AnnouncementId').val(),
                code: $("#txtcode").val(),
                title: $("#txttitle").val(),
                isActive: $("#ChkActive").is(":checked"),
                announcementType: $('input[name=group-popup]:checked').val(),
                contents: contents,
                environments: environments,
                fromdate: $('#FromDate').val(),
                todate: $('#ToDate').val(),
                order: $('#group-order option:selected').val(),
                systems: systems,
                specifications: specs
            };

            console.log(announcement);
            return;

            $.ajax({
                url: NAP.resolveClientUrl("Announcement/InsertAnnouncement"),
                type: 'POST',
                data: { "announcement": JSON.stringify(announcement), action: $('#actionType').val() },
                dataType: 'json',
                //contentType: "application/json; charset=utf-8",
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
        }
    }
}(jQuery));


$(document).ready(function () {
    ComposedAnnouncement.init();
});


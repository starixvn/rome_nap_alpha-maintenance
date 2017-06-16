"use strict";

var ComposedAnnouncement = (function ($) {
    return {
        init: function () {

            $('.jqte-test').jqte();
            $('ul.tabs').tabs();
            $('select').material_select();
            $("#content-group").removeClass("hiden");
            this.bindEvent();
        },
        bindEvent: function ()
        {

            $('#btnSave').on('click', function () {
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
                var specs = [
                         {
                             'Value': [
                                'Site1',
                                'Site4'
                             ],
                             'Sites': [
                                'Site1',
                                'Site4'
                             ],
                             'Id': '6edc73e0-aa87-4757-96e7-e5ffea0ce06a',
                             'Name': 'Sites',
                             'Combinable': true,
                             'TypeName': 'Alpha.Announcements.Specs.SiteSpecification, Alpha.Announcements, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null',
                             'ConjunctionOperator': 0,
                             'SpecificationOperator': 1,
                             'ValueRange': null
                         },
                         {
                             'Value': [
                                3,
                                4
                             ],
                             'Levels': [
                                3,
                                4
                             ],
                             'ValueRange': {
                                 '0': 'All',
                                 '2': 'Agent',
                                 '3': 'Master',
                                 '4': 'Super'
                             },
                             'Id': 'e6cacf4d-37d8-4031-a17c-8b02600d4c3f',
                             'Name': 'User Level Specification',
                             'Combinable': true,
                             'TypeName': 'Alpha.Announcements.Specs.UserLevelSpecification, Alpha.Announcements, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null',
                             'ConjunctionOperator': 2,
                             'SpecificationOperator': 1
                         }
                ];
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
                    action: $('#actionType').val(),
                    specifications: [],
                    specs: specs
                };

                
                $.ajax({
                    url: NAP.resolveClientUrl("Announcement/InsertAnnouncement"),
                    type: 'POST',
                    data: JSON.stringify(announcement),
                    dataType: 'json',
                    contentType: "application/json; charset=utf-8",
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
            });
        }
    }
}(jQuery));


$(document).ready(function () {
    ComposedAnnouncement.init();
});


/* global NAP: false */

"use strict";
var fanexGridLanguage = {};
fanexGridLanguage['en-US'] = {
    noData: 'There is no data',
    pageSize: 'Page size',
    goToPage: 'Go To Page',
    pagingText: 'Displaying <b>{0}</b> to <b>{1}</b> of <b>{2}</b> item(s)'
};

var fanexGridCurrentLanguage = fanexGridLanguage['en-US'];
var Super = (function ($) {

    return {
        init: function () {
            Super.pagers = $('#pagination-1');
            $('#submit').on('click', function () {
                Super.getSuperLimitDate(50, 0);
            })

            var $input = $('.datepicker').pickadate();
            var picker = $input.pickadate('picker');

            $('#raMinDate').on('click', function (event) {
                event.stopPropagation();
                picker.open();
            });
        },

        deleteSuperLimitDate: function (id, userName) {
            var answer = confirm("Are you sure delete customer limit date of: " + userName);
            if (answer) {
                $.ajax({
                    type: "POST",
                    data: { CustId: id },
                    url: NAP.resolveClientUrl("SuperLimitDate/Delete"),
                    beforeSend: function () {
                        NAP.loadingBox.show();
                    },
                    complete: function () {
                        NAP.loadingBox.close();
                    },
                    success: function (data) {
                        if (data.IsSuccessful == true) {
                            NAP.headlineMsgBox.showInfo("Delete successfull !", { closeBtn: false });
                            Super.getSuperLimitDate(50, 0);
                        }
                        else {
                            NAP.headlineMsgBox.showErr(data.Message, { closeBtn: false });
                        }

                    },
                    error: function (xhr, status, err) {
                        NAP.handleAjaxError(xhr, status, err);
                    }
                });
            }
        },
        disableLimitDays: function () {
            $('#LimitDays').attr("readonly", "true");
            $('#MinDate').removeAttr("readonly");
        },
        disableMinDate: function () {
            $('#MinDate').attr("readonly", "true");
            $('#LimitDays').removeAttr("readonly");
            $('#LimitDays').focus();
        },
        getSuperLimitDate: function (pageSize, index) {
            index++;

            $.ajax({
                url: NAP.resolveClientUrl("SuperLimitDate/Get"),
                data: {
                    UserName: $('#userName').val(),
                    PageIndex: index,
                    PageSize: pageSize
                },
                type: 'POST',
                beforeSend: function () {
                    NAP.loadingBox.show();
                },
                complete: function () {
                    NAP.loadingBox.close();
                },
                success: function (result) {
                    NAP.loadingBox.show();

                    $('#content').html(result);

                    Super.pagers.show().fanexPagination(totalRows, {
                        itemsPerPage: pageSize,
                        startPerPage: 50,
                        itemsPerPageArray: [50, 100, 500],
                        numDisplayEntries: 5,
                        currentPage: index - 1,
                        numEdgeEntries: 1,
                        linkTo: "javascript:void(0)",
                        prevText: "&lt;",
                        nextText: "&gt;",
                        ellipseText: "...",
                        prevShowAlways: true,
                        nextShowAlways: true,
                        maxTtemsPerPage: 100,
                        showDetail: true,
                        callback: Super.reRenderAfterPaged
                    });

                    NAP.loadingBox.close();
                },
                error: function (xhr, status, err) {
                    NAP.handleAjaxError(xhr, status, err);
                }
            });
        },
        reRenderAfterPaged: function (pageIndex, itemsPerPage) {
            Super.getSuperLimitDate(itemsPerPage, pageIndex);
        }
    };

}(jQuery));

$(document).ready(function () {
    Super.init();
});
"use strict";

interface JQuery {
    material_select(): void;
    jqte(): void;
    closeModal(): void;
    openModal(): void;
    dropify(): void;
}

interface Window {
    NAP: any;
    JSON: any;
}

class ManagesAnnouncements {
    $: JQueryStatic;
    $nap: any;
    $JSON: any;

     constructor($: JQueryStatic, NAP: any, JSON: any) {
        this.$ = $;
        this.$nap = NAP;
        this.$JSON = JSON;
    }

     init = () => {
         this.$('select').material_select();
         this.$('.dropify').dropify();

         this.bindEvent();
     };

     bindEvent = () => {
         let self = this;
         this.$(".btn-import").on('click', function () {
             self.$("#cofirmMessages").openModal();
         });

         this.$("#btn-submit").on('click', function () {
             self.$('#announcements-form').attr('action', self.$("#manage-announcements-url").val()).submit();
         });
         this.$("#btn-exportAll").on('click', function () {
             self.$('#announcements-form').attr('action', self.$("#export-all-announcements-url").val()).submit();
         });

         this.$("#agree").on('click', function () {
             self.$("#cofirmMessages").closeModal();
             self.importAnnoucement();
         });
     };

     importAnnoucement = () => {
         let self = this;
         var formdata = new FormData();
         let file = <HTMLInputElement>this.$("#input-file-now")[0];
         formdata.append("file", file.files[0]);

         this.$.ajax({
             url: self.$nap.resolveClientUrl("Announcement/Import"),
             type: 'POST',
             dataType: 'json',
             data: formdata,
             contentType: false,
             processData: false,
             beforeSend: function () { },
             complete: function () {
                 self.$nap.loadingBox.close();
             },
             success: function (results) {
                 for (var result in results)
                 {
                     if (results.Code > 0)
                     {
                         self.onSuccess(results.Code);
                         return;
                     }
                 }
                 self.onSuccess();
             },
             error: function (xhr, status, err) {
                 self.$nap.handleAjaxError(xhr, status, err);
                 self.onSuccess(101);
             }
         });
     };

     onSuccess = (code: number = 0) => {
         let self = this;
         if (code > 0) {
             this.$('.card-panel').html("Import announcement failed. Please check your file!").addClass("red red-text").removeClass('green green-text hiden');
         }
         else {
             this.$('.card-panel').html("Import announcements successfully!").addClass("green green-text").removeClass('red red-text hiden');
             setTimeout(() => {
                 window.location.href = this.$nap.resolveClientUrl("Announcement/ManageAnnouncements")
             }, 3000);
         }
     };
}

const managesAnnouncements = new ManagesAnnouncements($, window.NAP, window.JSON);

$(() => {
    managesAnnouncements.init();
});
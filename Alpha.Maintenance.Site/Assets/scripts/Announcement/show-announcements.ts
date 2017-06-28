"use strict";

interface JQuery {
    material_select(): void;
}

class ShowedAnnouncement {
    $: JQueryStatic;

    constructor($: JQueryStatic) {
        this.$ = $;
    }

    init = () => {
        let self = this;
        this.$('select').material_select();
        this.$("#submit").click(function () {
            self.$("#report-form").submit();
        });
    };
}

const showedAnnouncement = new ShowedAnnouncement($);

$(() => {
    showedAnnouncement.init();
});
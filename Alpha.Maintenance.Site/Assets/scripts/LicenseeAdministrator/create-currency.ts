/// <reference path="base.ts" />
"use strict";

class CreateCurrency extends LicenseeAdministrationBase {
    constructor($: JQueryStatic, NAP: any) {
        super($, NAP);
    }

    init() {
        super.init();
    }

    getData() {
        return { siteName: this.$("#txtSiteName").val(), payload: this.$body.getValue() };
    }
}

$(() => {
    var createCurrency = new CreateCurrency($, window.NAP);
    createCurrency.init();
});
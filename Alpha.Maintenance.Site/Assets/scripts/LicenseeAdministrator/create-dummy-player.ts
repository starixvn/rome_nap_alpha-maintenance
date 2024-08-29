/// <reference path="base.ts" />
"use strict";

class CreateDummyPlayer extends LicenseeAdministrationBase {
    constructor($: JQueryStatic, NAP: any) {
        super($, NAP);
    }

    init() {
        super.init();
    }

    getData() {
        return { agentId: this.$("#txtAgentId").val(), payload: this.$body.getValue() };
    }
}

$(() => {
    var createDummyPlayer = new CreateDummyPlayer($, window.NAP);
    createDummyPlayer.init();
});
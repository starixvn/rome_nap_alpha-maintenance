"use strict";
interface JQuery {
    material_select(): void;
}

interface Window {
    NAP: any;
}

class ManageBanner {
    $: JQueryStatic;
    nap: any;

    constructor($: JQueryStatic, nap: any) {
        this.$ = $;
        this.nap = nap;
    }

    private cacheDOMElements() {
        //this.divMessage = this.$("#div-message");
    }

    init(): void {
        this.$("select").material_select();
    }
}

$(() => {
    const manageBanner = new ManageBanner($, window.NAP);
    manageBanner.init();
});
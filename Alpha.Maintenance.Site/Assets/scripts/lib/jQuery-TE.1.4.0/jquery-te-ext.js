var jqTextEditor = {
    getCaretPosition: function (editableDiv) {
        var caretPos = 0;
        var range;

        if (window.getSelection) {
            var sel = window.getSelection();
            if (sel.getRangeAt) {
                range = sel.getRangeAt(0);
                caretPos = range.startOffset;
            }

            return caretPos;
        }

        if (document.selection && document.selection.createRange) {
            range = document.selection.createRange();
            if (range.parentElement() == editableDiv) {
                var tempEl = document.createElement("span");
                editableDiv.insertBefore(tempEl, editableDiv.firstChild);
                var tempRange = range.duplicate();
                tempRange.moveToElementText(tempEl);
                tempRange.setEndPoint("EndToEnd", range);
                caretPos = tempRange.text.length;
            }
        }

        return caretPos;
    },

    restoreCarretPos: function (editableDiv, currentPos) {
        var selection = window.getSelection();
        var range = document.createRange();
        range.setStart(editableDiv[0].firstChild, currentPos);
        range.collapse(true);

        selection.removeAllRanges();
        selection.addRange(range);
        editableDiv.focus();
    }
};
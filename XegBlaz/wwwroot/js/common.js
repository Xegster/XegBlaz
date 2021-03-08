window.clipboardCopy = {
	copyText: function (text) {
		navigator.clipboard.writeText(text);
	},
	selectText: function (domElementId, select) {
		if (select) {
			var selection = window.getSelection();
			var range = document.createRange();
			range.selectNodeContents(document.getElementById(domElementId));
			selection.removeAllRanges();
			selection.addRange(range);
		}
		else {
			window.getSelection().removeAllRanges();
		}
	}
};
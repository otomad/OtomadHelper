{ // Init number extensions
	defineGetterInPrototype(Number, "isPositive", function () {
		return +this > 0 || Object.is(+this, +0);
	});

	defineGetterInPrototype(Number, "isInteger", function () {
		return Number.isInteger(+this);
	});

	defineGetterInPrototype(Number, "isSafeInteger", function () {
		return Number.isSafeInteger(+this);
	});

	defineGetterInPrototype(Number, "isFinite", function () {
		return Number.isFinite(+this);
	});

	defineGetterInPrototype(Number, "isNaN", function () {
		return Number.isNaN(+this);
	});

	Number.prototype.toFixedNumber = function (fractionDigits) {
		return +this.toFixed(fractionDigits);
	};

	Number.prototype.countDecimals = function () {
		if (Number.isInteger(+this)) return 0;
		const str = toPlain(this);
		return str.split(".")[1]?.length || 0;
	};

	makePrototypeKeysNonEnumerable(Number);
}

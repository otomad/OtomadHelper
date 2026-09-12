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
		const str = normalizeNumber(this);
		return str.split(".")[1]?.length || 0;
	};

	makePrototypeKeysNonEnumerable(Number);
}

/**
 * Validates whether a value is a valid number.
 *
 * value | returns
 * --- | :--:
 * `-123.45e-56` | true
 * `"1.0e-8"` | true
 * `256n` | true
 * `"0xDeadBeef"` | true
 * `""` | false
 * `NaN` | false
 * `Infinity` | false
 *
 * @param value - The value to validate. Can be of any type.
 * @returns Is the value a valid finite number, non-empty string that represents a number, or a BigInt?
 */
export function isValidNumber(value: unknown) {
	// eslint-disable-next-line no-restricted-globals
	return value !== "" && ["number", "string"].includes(typeof value) && isFinite(value as number) || typeof value === "bigint";
}

/**
 * Normalize the number. Reject stupid scientific notation.
 * @param num - Number.
 * @returns Normalized number, or `"NaN"` if the number is invalid.
 * @see https://stackoverflow.com/a/61281355/19553213
 * @note `-0` will be converted to `"0"`.
 */
export function normalizeNumber(num: WithWrapperType<number | bigint | string>) {
	num = num.valueOf();
	return (() => {
		if (typeof num === "string")
			if (num.match(/^(NaN|[+-]?Infinity)$/)) return num;
			else if (num.match(/^0[box]/i)) try { num = BigInt(num); } catch { }
		if (!isValidNumber(num)) return "NaN";
		return ("" + num).replace(/([+-]?)(\d*)\.?(\d*)e([+-]?\d+)/i,
			(_, sign, int, frac, exp) => exp < 0 ?
				sign + "0." + Array(1 - exp - int.length).join("0") + int + frac :
				sign + int + frac + Array(exp - frac.length + 1).join("0"));
	})().replace(/^\+/, "");
}

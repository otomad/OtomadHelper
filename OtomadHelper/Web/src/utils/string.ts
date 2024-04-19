/// <reference path="string.d.ts" />

{ // Init string extensions
	String.prototype.countChar = function (...chars) {
		let count = 0;
		for (const char of this)
			if (chars.includes(char))
				count++;
		return count;
	};

	String.prototype.reverse = function () {
		return Array.from(this).reverse().join("");
	};

	String.prototype.toBoolean = function () {
		return this.trim().toLowerCase() !== "false";
	};

	String.prototype.toArray = function () {
		return Array.from(this);
	};

	String.prototype.inTwo = function (sep = ",") {
		return Array.from(this).join(sep);
	};

	String.prototype.in = function (...list) {
		return list.includes(this as never);
	};

	String.prototype.removeSpace = function () {
		return this.replace(/\s/g, "");
	};

	String.prototype.holeString = function (start, end) {
		return this.slice(0, start) + this.slice(end);
	};

	makePrototypeKeysNonEnumerable(String);
}

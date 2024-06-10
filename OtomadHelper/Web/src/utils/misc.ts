/// <reference path="misc.d.ts" />

{
	Response.prototype.xml = async function () {
		const text = await this.text();
		return new window.DOMParser().parseFromString(text, "text/xml");
	};

	makePrototypeKeysNonEnumerable(Response);
}

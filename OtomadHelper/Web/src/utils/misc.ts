{
	Response.prototype.xml = async function () {
		const text = await this.text();
		return new DOMParser().parseFromString(text, "text/xml");
	};
}
{
	HTMLImageElement.prototype.waitForLoaded = function () {
		return new Promise<HTMLImageElement>((resolve, reject) => {
			this.onload = () => resolve(this);
			this.onerror = e => reject(e);
		});
	};
}
{
	HTMLCanvasElement.prototype.toBlobURL = function () {
		return new Promise<string>(resolve => this.toBlob(blob => resolve(blob! && URL.createObjectURL(blob))));
	};
}
{
	defineGetterInPrototype(Element, "path", function () {
		return getPath(this);
	});

	// Polyfill for Firefox.
	Element.prototype.scrollIntoViewIfNeeded ??= Element.prototype.scrollIntoView;

	Element.prototype.indexIn = function (parent): Any {
		const shouldReturnAncestor = typeof parent === "string";
		parent ??= this.parentNode;
		if (typeof parent === "string") parent = this.closest(parent);
		let immediate: Node | null = this;
		while (immediate) {
			if (immediate.parentNode === parent) break;
			immediate = immediate.parentNode;
		}
		const index = !immediate || !parent ? -1 :
			!(immediate instanceof Element) ? [...parent.childNodes].indexOf(immediate as ChildNode) :
			parent instanceof Element ? [...parent.children].indexOf(immediate) :
			[...parent.childNodes].filter(node => node instanceof Element).indexOf(immediate);
		return !shouldReturnAncestor ? index : [index, parent];
	};

	Element.prototype.querySelectorWithSelf = function (selector: string) {
		return this.matches(selector) ? this : this.querySelector(selector);
	};
}
{
	DOMTokenList.prototype.containsAny = function (...tokens) {
		return tokens.some(token => this.contains(token));
	};
}
{
	console.image = function (url, size = 100) {
		const image = new Image();
		image.src = url;
		return new Promise<void>((resolve, reject) => {
			image.onload = function () {
				const style = [
					"font-size: 1px;",
					"padding: " + image.height / 100 * size + "px " + image.width / 100 * size + "px;",
					"background: url(" + url + ") no-repeat;",
					"background-size: contain;",
				].join(" ");
				// eslint-disable-next-line unicorn/no-console-spaces
				console.log("%c ", style);
				// resolve();
			};
			image.onerror = reject;
		});
	};
}
{
	HTMLInputElement.prototype.selectAndFocus = function () {
		this.select();
		this.scrollIntoViewIfNeeded();
		this.focus();
	};
}
{
	defineGetterInPrototype(Symbol, "isGlobal", function () {
		return Symbol.keyFor(this.valueOf()) !== undefined;
	});
}

import { readFile, writeFile } from "fs/promises";

let svgPath = process.argv[2];
if (svgPath === "--") svgPath = process.argv[3];
if (!svgPath) throw new ReferenceError("SVG file not specified");
let svg = await readFile(svgPath, "utf-8");

// It's just some simple processing. If it is complicated, it cannot be fully supported for the time being.

svg = svg.replaceAll(/\sfill="[^"]*"/g, "").replaceAll(/\s*<\/?g.*>/g, "").replaceAll(/\s*<defs>.*<\/defs>/gs, "").replaceAll(/\t+/g, "\t");
writeFile(svgPath, svg);

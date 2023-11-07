import type { Property } from "csstype";
export type Cursor = Property.Cursor;

const forceCursors: Cursor[] = ["not-allowed", "no-drop", "col-resize", "grabbing"];
export default forceCursors;

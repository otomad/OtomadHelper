import crowdin from "@crowdin/crowdin-api-client";
import { token } from "./token";

const Crowdin = (crowdin as unknown as { default: typeof crowdin }).default;

export const { translationsApi, uploadStorageApi, sourceFilesApi } = new Crowdin({ token });

export const fgGreen = "\x1b[32m";

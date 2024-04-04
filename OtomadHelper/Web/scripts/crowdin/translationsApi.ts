import crowdin from "@crowdin/crowdin-api-client";
import { token } from "./token";

const Crowdin = (crowdin as unknown as { default: typeof crowdin }).default;

const { translationsApi } = new Crowdin({ token });

export default translationsApi;

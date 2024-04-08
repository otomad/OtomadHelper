type VersionLike = string | number[];

export default class Version {
	version: number[];

	constructor(version: VersionLike) {
		this.version = Array.isArray(version) ? version : Version.parseVersion(version);
		Version.checkIsValidVersion(this.version);
	}

	private static parseVersion(versionString: string) {
		let version = versionString.match(/(?<=v)\d(\.\d)*/i)?.[0];
		if (!version) version = versionString.match(/\d(\.\d)*/i)?.[0];
		if (!version) throw new Error(`Invalid version string: ${versionString}`);
		return version.split(".").map(v => parseInt(v, 10));
	}

	private static checkIsValidVersion(version: number[]) {
		if (!version.length || version.some(v => !Number.isFinite(v)))
			throw new Error(`Invalid version: ${version}`);
	}

	compareTo(other: Version | VersionLike) {
		if (!(other instanceof Version)) other = new Version(other);
		for (let i = 0; i < Math.max(this.version.length, other.version.length); i++) {
			const v1 = this.version[i] || 0, v2 = other.version[i] || 0;
			if (v1 !== v2) return Math.sign(v1 - v2);
		}
		return 0;
	}

	equals(other: Version | VersionLike) {
		return this.compareTo(other) === 0;
	}

	isLessThan(other: Version | VersionLike) {
		return this.compareTo(other) < 0;
	}

	isGreaterThan(other: Version | VersionLike) {
		return this.compareTo(other) > 0;
	}

	isLessThanOrEqualTo(other: Version | VersionLike) {
		return this.compareTo(other) <= 0;
	}

	isGreaterThanOrEqualTo(other: Version | VersionLike) {
		return this.compareTo(other) >= 0;
	}
}

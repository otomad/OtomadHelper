namespace OtomadHelper.Helpers;

public static class ContentType {
	public static string GetContentType(string extension) => extension switch {
		// Texts
		"html" or "htm" => "text/html",
		"js" or "cjs" or "mjs" or "jsm" => "text/javascript",
		"css" => "text/css",
		"vtt" => "text/vtt",
		"txt" or "ini" => "text/plain",
		"md" => "text/markdown",
		"appcache" => "text/cache-manifest",
		"htc" => "text/x-component",
		"mht" or "mhtml" => "message/rfc822",
		"hta" => "application/hta",
		"json" or "jsonc" => "application/json",
		"xml" => "application/xml",
		"yaml" or "yml" => "application/yaml",
		"toml" => "application/toml",
		"json5" => "application/json5",
		"xhtml" => "application/xhtml+xml",
		"manifest" or "webmanifest" => "application/manifest+json",

		// Images
		"jpg" or "jpeg" or "jpe" or "jfif" => "image/jpeg",
		"png" => "image/png",
		"gif" => "image/gif",
		"bmp" => "image/bmp",
		"tif" or "tiff" => "image/tiff",
		"svg" or "svgz" => "image/svg+xml",
		"apng" => "image/apng",
		"webp" => "image/webp",
		"avif" => "image/avif",
		"heif" => "image/heif",
		"heic" => "image/heic",
		"heifs" => "image/heif-sequence",
		"heics" => "image/heic-sequence",
		"ico" => "image/vnd.microsoft.icon",
		"cur" => "image/x-win-bitmap",
		"ani" => "application/x-navi-animation",

		// Fonts
		"woff" => "font/woff",
		"woff2" => "font/woff2",
		"ttf" => "font/ttf",
		"otf" => "font/otf",
		"ttc" => "font/collection",
		"eot" => "application/vnd.ms-fontobject",

		// Shaders
		"frag" => "x-shader/x-fragment",
		"vert" => "x-shader/x-vertex",
		"glsl" => "x-shader/x-glsl",

		// Audios
		"mid" or "midi" => "audio/midi",
		"mp3" or "mpa" or "mp2" or "mp1" or "mpga" => "audio/mpeg",
		"ogg" or "oga" => "audio/ogg",
		"aac" => "audio/aac",
		"m4a" => "audio/mp4",
		"flac" => "audio/flac",
		"wav" => "audio/wav",
		"weba" => "audio/webm",
		"ape" => "audio/x-ape",
		"wma" => "audio/x-ms-wma",

		// Videos
		"mp4" => "video/mp4",
		"mpeg" or "mpg" or "mpe" => "video/mpeg",
		"mov" or "qt" => "video/quicktime",
		"ogv" => "video/ogg",
		"ts" => "video/mp2t",
		"webm" => "video/webm",
		"wmv" => "video/x-ms-wmv",
		"avi" => "video/x-msvideo",
		"flv" => "video/x-flv",
		"lottie" => "application/zip+dotlottie",
		"swf" => "application/vnd.adobe.flash.movie",
		"rmvb" => "application/vnd.rn-realmedia-vbr",
		"m3u8" => "application/x-mpegurl",

		// Compressed files
		"gz" => "application/gzip",
		"zip" => "application/zip",
		"rar" => "application/vnd.rar",
		"7z" => "application/x-7z-compressed",
		"tar" => "application/x-tar",

		// Misc
		"ogx" => "application/ogg",
		_ => "application/octet-stream",
	};
}

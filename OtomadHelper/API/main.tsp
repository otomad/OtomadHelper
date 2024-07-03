import "@typespec/http";
import "@typespec/rest";
import "@typespec/openapi3";

using TypeSpec.Http;
using TypeSpec.Rest;

/** This is a pet store service. */
@service({
	title: "Pet Store Service",
})
@server("https://example.com", "The service endpoint")
namespace PetStore;

@route("/pets")
interface Pets {
	list(): Pet[];
}

model Pet {
	@minLength(100)
	name: string;

	@minValue(0)
	@maxValue(100)
	age: int32;

	kind: "dog" | "cat" | "fish";
}

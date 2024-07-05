namespace PetStore;

interface Pets {
	list(): Pet[];
}

model Pet {
	name: string;

	age: int32;

	kind: "dog" | "cat" | "fish";
}

# ObjectPointer
Example of three ways to get an managed object pointer

## Intermediate Language
``` csharp
var person = new Person("Steal", 99);

// get managed pointer from object
IntPtr pointer = UnsafeObject.GetAddressFromIL(ref person);

// get object from managed pointer
var obj = UnsafeObject.GetObjectFromIL<Person>(pointer);
```

## TypedReference
``` csharp
var person = new Person("Steal", 99);

// get TypedReference's pointer from object
IntPtr pointer = UnsafeObject.GetAddressFromTypedReference(ref person);

// get object from TypedReference's pointer
var obj = UnsafeObject.GetObjectFromTypedReference<Person>(pointer);
```

## GCHandle
``` csharp
var person = new Person("Steal", 99);

// get GCHandle from object
IntPtr pointer = UnsafeObject.GetAddressFromGC(person);

// get object from GCHandle
var obj = UnsafeObject.GetObjectFromGC<Person>(pointer);
```

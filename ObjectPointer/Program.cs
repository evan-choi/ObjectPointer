using System;

namespace ObjectPointer
{
    class Program
    {
        static void Main(string[] args)
        {
            // class object pointer

            var person = new Person("Mark", 21);

            IntPtr ilAddress = UnsafeObject.GetAddressFromIL(ref person);
            IntPtr gcAddress = UnsafeObject.GetAddressFromGC(person);
            IntPtr trAddress = UnsafeObject.GetAddressFromTypedReference(ref person);

            Console.WriteLine($"{ilAddress} (IL)");
            Console.WriteLine($"{gcAddress} (GCHandle)");
            Console.WriteLine($"{trAddress} (TypedReference)");

            var ilObject = UnsafeObject.GetObjectFromIL<Person>(ilAddress);
            var gcObject = UnsafeObject.GetObjectFromGC<Person>(gcAddress);
            var trObject = UnsafeObject.GetObjectFromTypedReference<Person>(trAddress);

            Console.WriteLine($"'{ilObject.Name}' from IL");
            Console.WriteLine($"'{gcObject.Name}' from GCHandle");
            Console.WriteLine($"'{trObject.Name}' from TypedReference");

            Console.WriteLine();

            // delegate pointer

            var action = new Action<string>(message =>
            {
                Console.WriteLine(message);
            });

            ilAddress = UnsafeObject.GetAddressFromIL(ref action);
            gcAddress = UnsafeObject.GetAddressFromGC(action);
            trAddress = UnsafeObject.GetAddressFromTypedReference(ref action);

            UnsafeObject.GetObjectFromIL<Action<string>>(ilAddress)("Message from IL");
            UnsafeObject.GetObjectFromGC<Action<string>>(gcAddress)("Message from GCHandle");
            UnsafeObject.GetObjectFromTypedReference<Action<string>>(trAddress)("Message from TypedReference");
        }
    }
}

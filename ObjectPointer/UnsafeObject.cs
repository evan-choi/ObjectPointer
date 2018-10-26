using System;
using System.Reflection.Emit;
using System.Runtime.InteropServices;

namespace ObjectPointer
{
    class UnsafeObject
    {
        #region Intermediate Language
        delegate IntPtr GetAddressDelegate(object obj);
        delegate object GetObjectDelegate(IntPtr obj);

        static readonly GetAddressDelegate _getAddress;
        static readonly GetObjectDelegate _getObject;

        static UnsafeObject()
        {
            var getAddressMethod = CreateDynamicMethod<object, IntPtr>(nameof(GetAddressFromIL));
            var getObjectMethod = CreateDynamicMethod<IntPtr, object>(nameof(GetObjectFromIL));

            BuildIL(getAddressMethod);
            BuildIL(getObjectMethod);

            _getAddress = getAddressMethod.CreateDelegate(typeof(GetAddressDelegate)) as GetAddressDelegate;
            _getObject = getObjectMethod.CreateDelegate(typeof(GetObjectDelegate)) as GetObjectDelegate;
        }

        static DynamicMethod CreateDynamicMethod<T, TResult>(string name)
        {
            return new DynamicMethod(
                name,
                typeof(TResult),
                new[] { typeof(T) },
                typeof(UnsafeObject),
                true);
        }

        static void BuildIL(DynamicMethod method)
        {
            ILGenerator ilGenerator = method.GetILGenerator();
            ilGenerator.Emit(OpCodes.Ldarg_0);
            ilGenerator.Emit(OpCodes.Ret);
        }

        public static IntPtr GetAddressFromIL<T>(ref T obj) where T : class
        {
            return _getAddress(obj);
        }

        public static T GetObjectFromIL<T>(IntPtr pointer) where T : class
        {
            return _getObject(pointer) as T;
        }
        #endregion

        #region GCHandle
        public static IntPtr GetAddressFromGC<T>(T obj) where T : class
        {
            return GCHandle.ToIntPtr(GCHandle.Alloc(obj, GCHandleType.Normal));
        }

        public static T GetObjectFromGC<T>(IntPtr pointer) where T : class
        {
            var handle = GCHandle.FromIntPtr(pointer);
            var target = handle.Target as T;
            handle.Free();

            return target;
        }
        #endregion

        #region TypedReference
        public unsafe static IntPtr GetAddressFromTypedReference<T>(ref T obj) where T : class
        {
            TypedReference typedReference = __makeref(obj);

            return *(IntPtr*)&typedReference;
        }

        public unsafe static T GetObjectFromTypedReference<T>(IntPtr pointer)
        {
            T buffer = default(T);
            TypedReference typedReference = __makeref(buffer);
            *(IntPtr*)&typedReference = pointer;

            Type type = __reftype(typedReference);

            if (typeof(T).IsAssignableFrom(type))
                return __refvalue(typedReference, T);

            return default(T);
        }
        #endregion
    }
}

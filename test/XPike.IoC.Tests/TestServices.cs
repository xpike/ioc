using System;

namespace XPike.IoC.Tests
{
    public interface IFoo
    {
        int Increment();
    }

    public class Foo : IFoo
    {
        int bar = 0;

        public int Increment()
        {
            return ++bar;
        }
    }

    public class Foo2 : IFoo
    {
        int bar = 0;

        public int Increment()
        {
            return ++bar;
        }
    }

    public interface IGeneric<T>
    {
        T Foo();
    }

    public class Generic<T> : IGeneric<T>
    {
        public T Foo()
        {
            return default(T);
        }
    }

    public class Generic2<T> : IGeneric<T>
    {
        public T Foo()
        {
            return default(T);
        }
    }

    public interface IMyFoo : IFoo
    {
        void My();
    }

    public class MyFoo : IMyFoo
    {
        public int Increment()
        {
            throw new NotImplementedException();
        }

        public void My()
        {
            throw new NotImplementedException();
        }
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable SA1402 // File may only contain a single class

namespace TagNDropLibrary {
    public class PoorMansDI {
        public static PoorMansDI Instance = new PoorMansDI();

        [System.Diagnostics.DebuggerStepThrough]
        public PoorMansDI() {
            this.Factories = new Dictionary<Type, Factory>();
        }

        public Dictionary<Type, Factory> Factories { get; }

        [System.Diagnostics.DebuggerStepThrough]
        public void Add<I>(Factory<I> factory) {
            this.Factories.Add(typeof(I), factory);
        }

        [System.Diagnostics.DebuggerStepThrough]
        public void Add<I, T>()
            where T : I, new() {
            this.Factories.Add(typeof(I), new Factory<I, T>(() => new T()));
        }

        [System.Diagnostics.DebuggerStepThrough]
        public Factory Get<I>() {
            Factory result;
            this.Factories.TryGetValue(typeof(I), out result);
            return result;
        }

        [System.Diagnostics.DebuggerStepThrough]
        public I Create<I>() {
            Factory factory;
            if (this.Factories.TryGetValue(typeof(I), out factory)) {
                return (I)(factory.Create());
            }
            return default(I);
        }
    }

    public abstract class Factory {
        public abstract Type GetTargetType();

        public override bool Equals(object obj) {
            if (ReferenceEquals(this, obj)) { return true; }
            if (ReferenceEquals(null, obj)) { return false; }
            var other = obj as Factory;
            if (ReferenceEquals(null, other)) { return false; }
            return this.GetTargetType() == other.GetTargetType();
        }

        public override int GetHashCode() { return this.GetTargetType().GetHashCode(); }

        public abstract object Create();
    }

    public class Factory<I> : Factory {
        private Func<I> _Generator;

        public Factory(Func<I> generator) {
            this._Generator = generator;
        }

        public override Type GetTargetType() => typeof(I);

        public override object Create() => this._Generator();
    }

    public class Factory<I, T> : Factory
        where T : I {
        private Func<T> _Generator;

        public Factory(Func<T> generator) {
            this._Generator = generator;
        }

        public override Type GetTargetType() => typeof(I);

        public override object Create() => this._Generator();
    }
}

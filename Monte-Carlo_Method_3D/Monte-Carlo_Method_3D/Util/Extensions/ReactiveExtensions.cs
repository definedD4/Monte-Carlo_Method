using System;
using System.Reactive;
using System.Reactive.Linq;

namespace Monte_Carlo_Method_3D.Util
{
    public static class ReactiveExtensions
    {
        public static IObservable<Unit> ToSignal<T>(this IObservable<T> observable) => 
            observable.Select(x => Unit.Default);
    }
}

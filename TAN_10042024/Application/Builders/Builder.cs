﻿namespace TAN_10042024.Application.Builders {
    public abstract class Builder<T, TResult> where T : new() {
        public static T Initialize() {
            return new T();
        }

        public virtual TResult Build() {
            SetupValidations();
            return default!;
        }

        protected abstract void SetupValidations();
    }
}

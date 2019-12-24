﻿namespace BookService.CommonEntities
{
    public static class EmptyArray<T>
    {
        private static readonly T[] InstanceInternal = new T[0];

        public static T[] Instance => InstanceInternal;
    }
}

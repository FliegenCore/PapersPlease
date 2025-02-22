namespace Common.Utils
{
    public struct Result<T>
    {
        public readonly T Object;
        public readonly bool IsExist;

        public Result(T result, bool isExit)
        {
            Object = result;
            IsExist = isExit;
        }
    }
}

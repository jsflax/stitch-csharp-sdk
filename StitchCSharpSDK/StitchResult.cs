using System;

namespace Stitch
{
    public class StitchResult<T>
    {
        private bool? _isSuccessful = null;
        public bool IsSuccessful 
        { 
            get
            {
                return _isSuccessful ?? Error == null;
            }
            internal set
            {
                _isSuccessful = value;
            }
        }
        public Exception Error { get; internal set; }
        public T Value { get; internal set; }

        public StitchResult()
        {
        }

        public StitchResult(bool isSuccessful,
                            T result,
                            Exception error = null)
        {
            this.IsSuccessful = isSuccessful;
			this.Value = result;
			this.Error = error;
        }

        public override string ToString()
        {
            return string.Format("[StitchResult: IsSuccessful={0}, Error={1}, Result={2}]", IsSuccessful, Error, Value);
        }

        public static implicit operator T(StitchResult<T> result)
        {
            return result.Value;
        }
    }
}

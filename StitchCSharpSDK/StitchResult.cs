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
                return _isSuccessful ?? Error != null;
            }
            internal set
            {
                _isSuccessful = value;
            }
        }
        public Exception Error { get; internal set; }
        public T Result { get; internal set; }

        public StitchResult()
        {
        }

        public StitchResult(bool isSuccessful,
                            T result,
                            Exception error = null)
        {
            this.IsSuccessful = isSuccessful;
			this.Result = result;
			this.Error = error;
        }

        public override string ToString()
        {
            return string.Format("[StitchResult: IsSuccessful={0}, Error={1}, Result={2}]", IsSuccessful, Error, Result);
        }
    }
}

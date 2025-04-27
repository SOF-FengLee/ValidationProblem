namespace TestValidation.ViewModel.Helpers
{
    public class PropertySetter<T>
    {
        private readonly Action<T> _setter;

        public PropertySetter(Action<T> setter)
        {
            _setter = setter;
        }

        public void SetValue(T value)
        {
            _setter(value);
        }
    }
}

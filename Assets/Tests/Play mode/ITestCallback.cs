namespace InfoPanelTests
{
    public interface ITestCallback
    {
        public void Callback();
    }

    public interface ITestCallback<T1>
    {
        public void Callback(T1 arg1);
    }
}
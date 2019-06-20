namespace BookStore.Contract
{
    public interface IStore
    {
        void Import(string catallogAsJson);
        int Quantity(string name);
        double Buy(params string[] basketByNames);
    }
}
namespace Services
{
    public interface ICustomerActivationService
    {
        bool Activate(int id);

        bool Deactivate(int id);
    }
}
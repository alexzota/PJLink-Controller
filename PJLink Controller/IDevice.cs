using PJLink_Controller.Commands;

namespace PJLink_Controller
{
    public interface IDevice
    {
        bool TurnOn();
        bool TurnOff();
        bool PowerQuery();
    }
}

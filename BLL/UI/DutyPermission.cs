using Core.Models;
using UI.ConsoleManagers;
using Core.Enums;

namespace UI
{
    public class DutyPermission
    {
        private readonly StateManagerUI _stateManagerUI;
        private readonly TesterUI _testerUI;
        private readonly DeveloperUI _developerUI;
        public async Task DutyIdentifier(User user)
        {
            switch (user.Duty)
            {
                case Duty.StateManager:
                    await _stateManagerUI.PerformOperationsAsync(user);
                    break;
                case Duty.Tester:
                    await _testerUI.PerformOperationsAsync(user);
                    break;
                case Duty.Developer:
                    await _developerUI.PerformOperationsAsync(user);
                    break;
                case Duty.Unassigned:

                    break;
                default:
                    throw new Exception("Wrong data input");
            }
        }
    }
}
